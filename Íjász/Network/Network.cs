﻿using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public delegate void Send(ServerCommand _command, string _data);

    public sealed class Network
    {
        private Thread thread;
        private Socket listener;

        public Send Broadcaster;

        public Network()
        {

        }

        public bool Start(IPAddress _address, int _port)
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(_address, _port));
                listener.Listen(4);

                thread = new Thread(new ThreadStart(Listen));
                thread.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a hálózat megnyitása során!\n" + e.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public List<String> GetAddresses()
        {
            List<String> addresses = new List<String>();

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress current in host.AddressList)
            {
                if (current.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    addresses.Add(current.ToString());
                }
            }
            return addresses;
        }

        public EndPoint GetEndPoint()
        {
            return listener.LocalEndPoint;
        }

        #region Broadcast
        public void induló_hozzáadás(Induló _induló)
        {
            Broadcast(ServerCommand.INDULÓ_HOZZÁADÁS, _induló.Nev + ";" + _induló.Nem + ";" + _induló.SzuletesiDatum + ";" + _induló.Engedely + ";" + _induló.Egyesulet + ";" + _induló.Eredmenyek);
        }

        public void induló_módosítás(Induló _eredeti, Induló _uj)
        {
            Broadcast(ServerCommand.INDULÓ_MÓDOSÍTÁS, _eredeti.Nev + ";" + _uj.Nev + ";" + _uj.Nem + ";" + _uj.SzuletesiDatum + ";" + _uj.Engedely + ";" + _uj.Egyesulet + ";" + _uj.Eredmenyek);
        }

        public void induló_átnevezés(string _eredeti_név, string _új_név)
        {
            Broadcast(ServerCommand.INDULÓ_ÁTNEVEZÉS, _eredeti_név + ";" + _új_név);
        }

        public void induló_törlés(Induló _indulo)
        {
            Broadcast(ServerCommand.INDULÓ_TÖRLÉS, _indulo.Nev);
        }

        //

        public void verseny_hozzáadás(Verseny _verseny)
        {
            Broadcast(ServerCommand.VERSENY_HOZZÁADÁS, _verseny.Azonosito + ";" + _verseny.Osszes + ";" + _verseny.Lezarva);
        }

        public void verseny_módosítás(string _azonosító, Verseny _verseny)
        {
            Broadcast(ServerCommand.VERSENY_MÓDOSÍTÁS, _azonosító + ";" + _verseny.Azonosito + ";" + _verseny.Osszes);
        }

        public void verseny_törlés(string _azonosító)
        {
            Broadcast(ServerCommand.VERSENY_TÖRLÉS, _azonosító);
        }

        public void verseny_lezárás(string _azonosító)
        {
            Broadcast(ServerCommand.VERSENY_TÖRLÉS, _azonosító);
        }

        public void verseny_megnyitás(string _azonosító)
        {
            Broadcast(ServerCommand.VERSENY_MEGNYITÁS, _azonosító);
        }

        //

        public void íjtípus_hozzáadás(Íjtípus _íjtípus)
        {
            Broadcast(ServerCommand.ÍJTÍPUS_HOZZÁADÁS, _íjtípus.Azonosito);
        }

        public void íjtípus_módosítás(string _azonosító, Íjtípus _íjtípus)
        {
            Broadcast(ServerCommand.ÍJTÍPUS_MÓDOSÍTÁS, _azonosító + ";" + _íjtípus.Azonosito);
        }

        public void íjtípus_törlés(string _azonosító)
        {
            Broadcast(ServerCommand.ÍJTÍPUS_TÖRLÉS, _azonosító);
        }

        //

        public void eredmény_beírás(string _azonosító, Database.BeírásEredmény _beírás)
        {
            if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                Broadcast(ServerCommand.EREDMÉNY_BEÍRÁS_HOZZÁADÁS, _azonosító + ";" + _beírás.eredmény.Value.Nev + ";" + _beírás.eredmény.Value.Sorszam.Value + ";" + _beírás.eredmény.Value.Ijtipus + ";" +
                    _beírás.eredmény.Value.Csapat + ";" + _beírás.eredmény.Value.Megjelent);

            if (_beírás.flag == Database.BeírásEredmény.Flag.MÓDOSÍTOTT)
                Broadcast(ServerCommand.EREDMÉNY_BEÍRÁS_MÓDOSÍTÁS, _azonosító + ";" + _beírás.eredmény.Value.Nev + ";" + _beírás.eredmény.Value.Ijtipus + ";" +
                    _beírás.eredmény.Value.Csapat + ";" + _beírás.eredmény.Value.Megjelent);
        }

        public  void eredmény_módosítás(string _azonosító, Eredmény _eredeti, Eredmény _eredmény)
        {
            Broadcast(ServerCommand.EREDMÉNY_MÓDOSÍTÁS, _azonosító + ";" + _eredmény.Nev + ";" + _eredmény.Talalat10 + ";" + _eredmény.Talalat8 + ";" + _eredmény.Talalat5 + ";" + _eredmény.Melle + ";" + 
                _eredmény.Osszpont.Value + ";" + _eredmény.Szazalek.Value);
            
        }

        public void eredmény_törlés(string _azonosító, Eredmény _eredmény)
        {
            Broadcast(ServerCommand.EREDMÉNY_TÖRLÉS, _azonosító + ";" + _eredmény.Nev);
        }
        #endregion

        private void Broadcast(ServerCommand _command, string _data)
        {
            if (Broadcaster != null) Broadcaster(_command, _data);
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    Connection connection = new Connection(listener.Accept());
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Hálózati hiba!\n" + e.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Stop listening for new clients.
                listener.Close();
            }
        }

        public void Shutdown()
        {
            if (thread != null) thread.Abort();
            if (listener != null) listener.Close();
        }
    }
}
