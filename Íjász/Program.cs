using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
    public static partial class Program
    {
        public static Object datalock = new Object();

        public static Network network;
        public static Database database;

        public static MainForm mainform;

        public static string Tulajdonos_Megnevezés = "Turul Koppány Íjászai HE, Hunvér Kft.";


        [STAThread]
        public static void Main()
        {
            try
            {
                network = new Network();
                database = new Database();

                if (!database.IsCorrectVersion())
                {
                    MessageBox.Show("Az adatbázis verziója nem megfelelő, le kell először futtatni az Íjász adatbázis kezelőt!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    network.Shutdown();

                    System.Environment.Exit(2);
                }

                AddressForm addressform;
                do
                {
                    addressform = new AddressForm();
                    Application.Run(addressform);
                    if (!addressform.selected) return;
                } while (!(addressform.selected && network.Start(addressform.selected_address, addressform.selected_port)));

                mainform = new MainForm();

                Timer timer = new Timer();
                timer.Interval = 5 * 60 * 1000;
                timer.Tick += timer_Tick;
                timer.Start();

                Application.Run(mainform);

                network.Shutdown();
            }
            catch (Exception e)
            {
                MessageBox.Show("Váratlan hiba:\n- " + e.Message + "\n\nAz Íjászt újra kell indítani!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                network.Shutdown();

                System.Environment.Exit(1);
            }
        }

        private static void timer_Tick(object _sender, EventArgs _event)
        {
            Program.database.CreateBackup("backup");

            //Spam_Data();
        }
    }
}
