using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public enum ClientCommand
    {
        NONE = 0,
        LOGIN = 1,

        INDULÓ_HOZZÁADÁS = 10,
        INDULÓ_MÓDOSÍTÁS = 12,
        INDULÓ_BEÍRÁS = 13,

        EREDMÉNY_MÓDOSÍTÁS = 22,
        EREDMÉNYEK = 23
    }

    public enum ServerCommand
    {
        NONE = 0,
        ERROR = 1,

        INDULÓ_HOZZÁADÁS = 12,
        INDULÓ_MÓDOSÍTÁS = 13,
        INDULÓ_ÁTNEVEZÉS = 14,
        INDULÓ_TÖRLÉS = 15,

        VERSENY_HOZZÁADÁS = 20,
        VERSENY_MÓDOSÍTÁS = 21,
        VERSENY_TÖRLÉS = 22,
        VERSENY_LEZÁRÁS = 23,
        VERSENY_MEGNYITÁS = 24,

        ÍJTÍPUS_HOZZÁADÁS = 30,
        ÍJTÍPUS_MÓDOSÍTÁS = 31,
        ÍJTÍPUS_TÖRLÉS = 32,

        EREDMÉNY_ADAT = 40,
        EREDMÉNY_BEÍRÁS_HOZZÁADÁS = 41,
        EREDMÉNY_BEÍRÁS_MÓDOSÍTÁS = 42,
        EREDMÉNY_MÓDOSÍTÁS = 43,
        EREDMÉNY_TÖRLÉS = 44
    }

    public sealed class Connection
    {
        public const int MAX_PACKET_SIZE = 4 * 256 * 512;

        private Thread thread;
        private Socket listener;

        public Connection(Socket _listener)
        {
            listener = _listener;
            listener.NoDelay = true;
            listener.ReceiveBufferSize = MAX_PACKET_SIZE;

            thread = new Thread(new ThreadStart(Receive));
            thread.Start();

            Program.network.Broadcaster += Send;

            Program.mainform.kapcsolatok_panel.KapcsolatHozzáadás(listener.LocalEndPoint.ToString());

            List<Induló> indulók = Program.database.Indulók();
            foreach (Induló current in indulók)
            {
                Send(ServerCommand.INDULÓ_HOZZÁADÁS, current.név + ";" + current.nem + ";" + current.születés + ";" + current.engedély + ";" + current.egyesület + ";" + current.eredmények);
            }

            List<Verseny> versenyek = Program.database.Versenyek_Aktív();
            foreach (Verseny current in versenyek)
            {
                Send(ServerCommand.VERSENY_HOZZÁADÁS, current.Azonosito + ";" + current.Osszes + ";" + current.Lezarva);
            }

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            foreach (Íjtípus current in íjtípusok)
            {
                Send(ServerCommand.ÍJTÍPUS_HOZZÁADÁS, current.azonosító);
            }
        }

        public void Send(ServerCommand _command, string _data)
        {
            byte[] packet = System.Text.Encoding.Unicode.GetBytes(_command.ToString() + ";" + _data + ";");
            try
            {
                listener.Send(packet, packet.Length, SocketFlags.None);
            }
            catch (Exception e) { MessageBox.Show("Hálózati hiba az üzenet küldése során!\n" + e.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Receive()
        {
            try
            {
                byte[] packet = new byte[MAX_PACKET_SIZE];
                while (true)
                {
                    int size = listener.Receive(packet, MAX_PACKET_SIZE, SocketFlags.None);

                    ProcessData(Encoding.Unicode.GetString(packet, 0, size));
                }
            }
            catch (ThreadAbortException)
            {
                Program.mainform.kapcsolatok_panel.KapcsolatTörlés(listener.LocalEndPoint.ToString());
                Shutdown();
            }
            catch(System.Net.Sockets.SocketException)
            {
                Program.mainform.kapcsolatok_panel.KapcsolatTörlés(listener.LocalEndPoint.ToString());
                Shutdown();
            }

            /*catch
            {
                Program.mainform.kapcsolatok_panel.KapcsolatTörlés(listener.LocalEndPoint.ToString());
                Shutdown();
            }*/
        }

        private void ProcessData(string _data)
        {
            string[] arguments = _data.Split(new char[] { ';' }, 2);
            if (arguments.Length != 2)
            {
                MessageBox.Show("Hálózati hiba!\nArgumentum szeparátor hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClientCommand command = ClientCommand.NONE;
            try
            {
                command = (ClientCommand)Enum.Parse(typeof(ClientCommand), arguments[0]);
            }
            catch (Exception) { MessageBox.Show("Hálózati hiba!\nIsmeretlen utasítás!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            switch (command)
            {
                case ClientCommand.LOGIN:
                    Program.mainform.kapcsolatok_panel.NévHozzáadás(listener.LocalEndPoint.ToString(), arguments[1]);
                    break;

                ///

                case ClientCommand.INDULÓ_HOZZÁADÁS:
                    string[] data = arguments[1].Split(new char[] { ';' });
                    if (data.Length != 5) { MessageBox.Show("Hálózati hiba!\nA kapott induló hozzáadása adatok száma nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    Program.mainform.indulók_panel.Induló_Hozzáadás(new Induló(data[0], data[1], data[2], data[3], data[4], 0));
                    break;

                case ClientCommand.INDULÓ_BEÍRÁS:
                    data = arguments[1].Split(new char[] { ';' });
                    if (data.Length != 6) { MessageBox.Show("Hálózati hiba!\nA kapott induló beírás adatok száma nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    Program.mainform.eredmények_panel.Eredmény_Beírás_Hálózat(data[0],
                                                                              data[1], 
                                                                              data[2], 
                                                                              Convert.ToInt32(data[3]), 
                                                                              Convert.ToBoolean(data[4]),
                                                                              Convert.ToBoolean( data[5] ),
                                                                              Convert.ToBoolean( data[6] ), 
                                                                              this);
                    break;

                case ClientCommand.INDULÓ_MÓDOSÍTÁS:
                    data = arguments[1].Split(new char[] { ';' });
                    if (data.Length != 6) { MessageBox.Show("Hálózati hiba!\nA kapott induló módosítás adatok száma nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    Induló? temp = Program.database.Induló(data[0]);

                    Program.mainform.indulók_panel.Induló_Módosítás(temp.Value,
                                                                     new Induló(data[1], 
                                                                                data[2], 
                                                                                data[3], 
                                                                                data[4], 
                                                                                data[5], 
                                                                                -666));
                    break;

                ///

                case ClientCommand.EREDMÉNYEK:
                    List<Eredmény> eredmények = Program.database.Eredmények(arguments[1]);
                    foreach (Eredmény eredmény in eredmények)
                    {
                        Send(ServerCommand.EREDMÉNY_ADAT, arguments[1] + ";" + eredmény.név + ";" + eredmény.sorszám + ";" + eredmény.íjtípus + ";" + eredmény.csapat + ";" + eredmény.találat_10 + ";"
                            + eredmény.találat_08 + ";" + eredmény.találat_05 + ";" + eredmény.mellé + ";" + eredmény.összpont.Value + ";" + eredmény.százalék.Value + ";" + eredmény.megjelent);
                    }
                    break;

                case ClientCommand.EREDMÉNY_MÓDOSÍTÁS:
                    data = arguments[1].Split(new char[] { ';' });
                    if (data.Length != 9) { MessageBox.Show("Hálózati hiba!\nA kapott eredmény módosítás adatok száma nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    Eredmény? érték = Program.database.Eredmény(data[0], data[1]);

                    if (érték != null)
                        Program.mainform.eredmények_panel.Eredmény_Módosítás_Hálózat(data[0], 
                            érték.Value, 
                            new Eredmény(data[1], 
                                         null, 
                                         data[2], 
                                         Convert.ToInt32(data[3]),
                                         Convert.ToInt32(data[4]), 
                                         Convert.ToInt32(data[5]), 
                                         Convert.ToInt32(data[6]), 
                                         Convert.ToInt32(data[7]), 
                                         null, 
                                         null, 
                                         Convert.ToBoolean(data[8]),
                                         Convert.ToBoolean(data[9]),
                                         data[10]),
                            this);
                    else
                        Send(ServerCommand.ERROR, "A módosítandó eredmény nem létezik!");
                    break;
            }
        }

        public void Shutdown()
        {
            Program.network.Broadcaster -= Send;
        }
    }
}
