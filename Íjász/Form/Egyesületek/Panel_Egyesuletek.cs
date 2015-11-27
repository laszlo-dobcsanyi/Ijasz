using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
    public struct Egyesulet {
        public string Azonosito;
        public string Cim;
        public string Vezeto;
        public string Telefon1;
        public string Telefon2;
        public string Email1;
        public string Email2;
        public bool Listazando;
        public int TagokSzama;

        public Egyesulet(   string _Azonosito, 
                            string _Cim, 
                            string _Vezeto,
                            string _Telefon1,
                            string _Telefon2,
                            string _Email1,
                            string _Email2, 
                            bool _Listazando, 
                            int _TagokSzama)
        {
            Azonosito = _Azonosito;
            Cim = _Cim;
            Vezeto = _Vezeto;
            Telefon1 = _Telefon1;
            Telefon2 = _Telefon2;
            Email1 = _Email1;
            Email2 = _Email2;
            Listazando = _Listazando;
            TagokSzama = _TagokSzama;
        }
    }

    public delegate void Egyesulet_Hozzaadva(Egyesulet _egyesulet);
    public delegate void Egyesulet_Modositva(Egyesulet _regi, Egyesulet _uj);
    public delegate void Egyesulet_Torolve(string _azonosito);
    public delegate void Egyesulet_Lezarva(string _azonosito);

    public sealed class Panel_Egyesuletek : Control
    {
        public Egyesulet_Hozzaadva egyesulet_hozzaadva;
        public Egyesulet_Modositva egyesulet_modositva;
        public Egyesulet_Torolve egyesulet_torolve;

        public DataTable data;
        private DataGridView table;

        public 
        Panel_Egyesuletek()
        {
            InitializeContent();
        }

        private void 
        InitializeContent()
        {
            table = new DataGridView();
            table.DataSource = CreateSource();
            table.Dock = DockStyle.Left;
            table.RowHeadersVisible = false;
            table.AllowUserToResizeRows = false;
            table.AllowUserToResizeColumns = false;
            table.AllowUserToAddRows = false;
            table.Width = 803;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ///

            Button btnHozzaadas = new Button();
            btnHozzaadas.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnHozzaadas.Text = "Hozzáadás";
            btnHozzaadas.Size = new System.Drawing.Size(96, 32);
            btnHozzaadas.Location = new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16);
            btnHozzaadas.Click += btnHozzaadas_Click;

            Button btnTorles = new Button();
            btnTorles.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnTorles.Text = "Törlés";
            btnTorles.Size = new System.Drawing.Size(96, 32);
            btnTorles.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            btnTorles.Click += btnTorles_Click;

            ///

            Controls.Add(table);

            Controls.Add(btnHozzaadas);
            Controls.Add(btnTorles);
        }

        private DataTable 
        CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Cím", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Vezető", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Telefon", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("E-mail", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Listázandó", System.Type.GetType("System.Boolean")));
            data.Columns.Add(new DataColumn("Tagok", System.Type.GetType("System.Int32")));

            List<Egyesulet> Egyesuletek = Program.database.Egyesuletek();

            foreach (Egyesulet current in Egyesuletek)
            {
                DataRow row = data.NewRow();
                row[0] = current.Azonosito;
                row[1] = current.Cim;
                row[2] = current.Vezeto;
                row[3] = current.Telefon1   +  Environment.NewLine + current.Telefon2;
                row[4] = current.Email1     + Environment.NewLine + current.Email2;
                row[5] = current.Listazando;
                row[6] = current.TagokSzama;
                data.Rows.Add(row);
            }

            return data;
        }
        
        #region Accessors

        private delegate void 
        Egyesulet_Hozzaadas_Callback(Egyesulet _egyesulet);
        public void 
        Egyesulet_Hozzaadas(Egyesulet _egyesulet)
        {
            if (InvokeRequired)
            {
                Egyesulet_Hozzaadas_Callback callback = new Egyesulet_Hozzaadas_Callback(Egyesulet_Hozzaadas);
                Invoke(callback, new object[] { _egyesulet });
            }
            else
            {
                if (!Program.database.UjEgyesulet(_egyesulet)) 
                { 
                    MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", 
                                    "Hiba", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error); 
                    return; 
                }

                string nl = Environment.NewLine;

                DataRow row = data.NewRow();
                row[0] = _egyesulet.Azonosito;
                row[1] = _egyesulet.Cim;
                row[2] = _egyesulet.Vezeto;
                row[3] = _egyesulet.Telefon1 + Environment.NewLine + _egyesulet.Telefon2;
                row[4] = _egyesulet.Email1 + Environment.NewLine + _egyesulet.Email2;
                row[5] = _egyesulet.Listazando;
                row[6] = _egyesulet.TagokSzama;
                data.Rows.Add(row);

                if (egyesulet_hozzaadva != null) egyesulet_hozzaadva(_egyesulet);
            }
        }

        private delegate void 
        Egyesulet_Modositva_Callback(Egyesulet _regi, Egyesulet _uj);

        public void 
        Egyesulet_Modositas(Egyesulet _regi, Egyesulet _uj)
        {
            if (InvokeRequired)
            {
                Egyesulet_Modositva_Callback callback = new Egyesulet_Modositva_Callback (Egyesulet_Modositas);
                Invoke(callback, new object[] { _regi, _uj });
            }
            else
            {
                if (!Program.database.EgyesuletModositas(_regi, _uj)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_regi.Azonosito == current[0].ToString())
                    {
                        current[0] = _uj.Azonosito;
                        current[1] = _uj.Cim;
                        current[2] = _uj.Vezeto;
                        current[3] = _uj.Telefon1 + Environment.NewLine + _uj.Telefon2;
                        current[4] = _uj.Email1 + Environment.NewLine + _uj.Email2;
                        current[5] = _uj.Listazando;
                        current[6] = _uj.TagokSzama;
                        break;
                    }
                }

                if (egyesulet_modositva != null) egyesulet_modositva(_regi,_uj);
            }
        }

        private delegate void 
        Egyesulet_Torolve_Callback(string _azonosito);

        public void 
        Egyesulet_Torles(string _azonosito)
        {
            if (InvokeRequired)
            {
                Egyesulet_Torolve_Callback callback = new Egyesulet_Torolve_Callback(Egyesulet_Torles);
                Invoke(callback, new object[] { _azonosito });
            }
            else
            {
                if (!Program.database.EgyesuletTorles(_azonosito))
                { 
                    MessageBox.Show("Adatbázis hiba!", 
                                    "Hiba", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error); 
                    return; 
                }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosito == current[0].ToString())
                    {
                        data.Rows.RemoveAt(table.SelectedRows[0].Index);
                        break;
                    }
                }

                if (egyesulet_torolve != null) egyesulet_torolve(_azonosito);
            }
        }

        #endregion

        #region EventHandlers

        private void 
        table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 210;
            table.Columns[1].Width = 153;
            table.Columns[2].Width = 100;
            table.Columns[3].Width = 100;
            table.Columns[4].Width = 100;
            table.Columns[5].Width = 60;
            table.Columns[6].Width = 60;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void 
        Modositas_Click(object _sender, EventArgs _event)
        {
            if ((string)data.Rows[table.SelectedRows[0].Index][0] == "") return;

            string[] telefon = data.Rows[table.SelectedRows[0].Index][3].ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string[] email = data.Rows[table.SelectedRows[0].Index][4].ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Egyesulet egyesulet = new Egyesulet( data.Rows[table.SelectedRows[0].Index][0].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][1].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][2].ToString(),
                                                 telefon[0],
                                                 telefon[1],
                                                 email[0],
                                                 email[1],
                                                 Convert.ToBoolean(data.Rows[table.SelectedRows[0].Index][5]),
                                                 Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][6]));

            Form_Egyesulet form_egyesulet = new Form_Egyesulet(egyesulet);
            form_egyesulet.Show();  

        }

        private void 
        btnHozzaadas_Click(object _sender, EventArgs _event)
        {
            Form_Egyesulet form = new Form_Egyesulet();
            form.Show();
        }

        private void 
        btnTorles_Click(object _sender, EventArgs _event)
        {
            if (table.SelectedRows.Count != 1 || (string)data.Rows[table.SelectedRows[0].Index][0] == "") return;
            if (0 < (int)(data.Rows[table.SelectedRows[0].Index][6]))
            { 
                MessageBox.Show("Ez az egyesület nem törölhető, mivel van hozzárendelve induló!",
                                "Hiba", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error); 
                return;
            }


            if (MessageBox.Show("Biztosan törli ezt az egyesületet?", 
                                "Megerősítés", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question) != DialogResult.Yes) return;

            Program.mainform.egyesuletek_panel.Egyesulet_Torles(data.Rows[table.SelectedRows[0].Index][0].ToString());

        }

        public void 
        InduloHozzaadas(Induló _indulo)
        {
            if(_indulo.Egyesulet=="") return ;

            if(!Program.database.EgyesuletTagokNoveles(_indulo.Egyesulet))
            {
                 MessageBox.Show("Adatbázis hiba!", 
                                 "Hiba", 
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error); 
                 return;
            }

            foreach(DataRow current in data.Rows)
            {
                if( _indulo.Egyesulet==current[0].ToString() )
                {
                    current[6] = (int)current[6]+1;
                }
            }
        }

        public void
        InduloTorles(Induló _indulo)
        {
            if(_indulo.Egyesulet=="") return;
            if(!Program.database.EgyesuletTagokCsokkentes(_indulo.Egyesulet))
            {
                MessageBox.Show("Adatbázis hiba!",
                                "Hiba",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); 
            }

            foreach(DataRow current in data.Rows)
            {
                if(_indulo.Egyesulet==current[0].ToString())
                {
                    current[6] = (int)current[6]-1;
                }
            }
        }

        public void
        InduloModositas( Induló _eredeti, Induló _uj )
        {
            if( _eredeti.Egyesulet == _uj.Egyesulet ) return;

            if( (!Program.database.EgyesuletTagokCsokkentes(_eredeti.Egyesulet) ||
                (!Program.database.EgyesuletTagokNoveles(_uj.Egyesulet))))
            {
                MessageBox.Show("Adatbázis hiba!",
                "Hiba",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error); 
            }

            foreach (DataRow current in data.Rows)
            {
                if (_eredeti.Egyesulet == current[0].ToString())
                {
                    current[6] = (int)current[6]-1;
                }
                if( _uj.Egyesulet == current[0].ToString() )
                {
                    current[6] = (int)current[6]+1;
                }
            }
        }

        #endregion

      
    }                                         
}

