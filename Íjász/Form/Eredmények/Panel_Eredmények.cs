using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public struct Eredmény
    {
        public string Nev;
        public Int64? Sorszam;
        public string Ijtipus;
        public int Csapat;
        public int Talalat10;
        public int Talalat8;
        public int Talalat5;
        public int Melle;
        public int? Osszpont;
        public int? Szazalek;
        public bool Megjelent;
        public bool KorosztalyModositott;
        public string KorosztalyAzonosito;

        public Eredmény( string _Nev,
                         Int64? _Sorszam,
                         string _Ijtipus,
                         int _Csapat,
                         int _Talalat10,
                         int _Talalat8,
                         int _Talalat5,
                         int _Melle,
                         int? _Osszpont,
                         int? _Szazalek,
                         bool _Megjelent,
                         bool _KorosztalyModositott,
                         string _KorosztalyAzonosito )
        {
            Nev = _Nev;
            Sorszam = _Sorszam;
            Ijtipus = _Ijtipus;
            Csapat = _Csapat;
            Talalat10 = _Talalat10;
            Talalat8 = _Talalat8;
            Talalat5 = _Talalat5;
            Melle = _Melle;
            Osszpont = _Osszpont;
            Szazalek = _Szazalek;
            Megjelent = _Megjelent;
            KorosztalyModositott = _KorosztalyModositott;
            KorosztalyAzonosito = _KorosztalyAzonosito;
        }
    }

    public delegate void Eredmény_Beírva(string _azonosító, Database.BeírásEredmény _beírás);
    public delegate void Eredmény_Módosítva(string _azonosító, Eredmény _eredeti, Eredmény _eredmény);
    public delegate void Eredmény_Törölve(string _azonosító, Eredmény _eredmény);

    public class Panel_Eredmények : Control
    {
        public Eredmény_Beírva eredmény_beírva;
        public Eredmény_Módosítva eredmény_módosítva;
        public Eredmény_Törölve eredmény_törölve;

        public DataTable data;
        public DataGridView table;

        private bool lezárva;
        private int verseny_összpont;
        private ComboBox combo_versenyek;

        private TextBox keresés;

        public Panel_Eredmények()
        {
            InitializeContent();
        }

        private void InitializeContent()
        {
            table = new DataGridView();
            table.DataSource = CreateSource();
            table.Dock = DockStyle.Left;
            table.RowHeadersVisible = false;
            table.AllowUserToResizeRows = false;
            table.AllowUserToResizeColumns = false;
            table.AllowUserToAddRows = false;
            table.Width = 683;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += módosítás_Click;

            ///

            Label verseny = new Label();
            verseny.Text = "Verseny:";
            verseny.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 0 * 32);

            combo_versenyek = new ComboBox();
            combo_versenyek.Size = new System.Drawing.Size(128, 24);
            combo_versenyek.Location = new System.Drawing.Point(verseny.Location.X + verseny.Size.Width + 16, verseny.Location.Y);
            combo_versenyek.DropDownStyle = ComboBoxStyle.DropDownList;
            combo_versenyek.SelectedIndexChanged += combo_versenyek_SelectedIndexChanged;

            ///

            Button törlés = new Button();
            törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            törlés.Text = "Törlés";
            törlés.Size = new System.Drawing.Size(96, 32);
            törlés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 8, ClientRectangle.Height - 32 - 16);
            törlés.Click += törlés_Click;

            Button lezárás = new Button();
            lezárás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            lezárás.Text = "Lezárás";
            lezárás.Size = new System.Drawing.Size(96, 32);
            lezárás.Location = new System.Drawing.Point(törlés.Location.X + törlés.Size.Width + 16, ClientRectangle.Height - 32 - 16);
            lezárás.Click += lezárás_Click;

            Button megnyitás = new Button();
            megnyitás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            megnyitás.Text = "Megnyitás";
            megnyitás.Size = new System.Drawing.Size(96, 32);
            megnyitás.Location = new System.Drawing.Point(törlés.Location.X + törlés.Size.Width + lezárás.Size.Width + 32, ClientRectangle.Height - 32 - 16);
            megnyitás.Click += megnyitás_Click;

            ///

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny current in versenyek)
                combo_versenyek.Items.Add(current.Azonosito);

            if (0 < combo_versenyek.Items.Count) combo_versenyek.SelectedIndex = 0;

            ///

            Label label_keresés = new Label();
            label_keresés.Text = "Sorszám keresés:";
            label_keresés.Location = new System.Drawing.Point(table.Location.X + table.Size.Width + 16, 16 + 1 * 32);

            keresés = new TextBox();
            keresés.Location = new System.Drawing.Point(combo_versenyek.Location.X, 16 + 1 * 32);
            keresés.TextChanged += keresés_TextChanged;
            keresés.MaxLength = 30;
            keresés.Width = 150;
            keresés.KeyPress += keresés_KeyPress;

            Controls.Add(label_keresés);
            Controls.Add(keresés);

            Controls.Add(table);

            Controls.Add(verseny);
            Controls.Add(combo_versenyek);

            Controls.Add(törlés);
            Controls.Add(lezárás);
            Controls.Add(megnyitás);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Név", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Sorszám", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Íjtípus", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Csapat", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("10", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("8", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("5", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Mellé", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Összes", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("%", System.Type.GetType("System.Int32")));
            data.Columns.Add( new DataColumn( "Megjelent", System.Type.GetType( "System.Boolean" ) ) );
            data.Columns.Add( new DataColumn( "KorosztalyModositott", System.Type.GetType( "System.Boolean" ) ) );
            data.Columns.Add( new DataColumn( "Korosztaly", System.Type.GetType( "System.String" ) ) );

            return data;
        }

        #region Accessors

        //ÁTNÉZVE
        private delegate void Eredmény_Beírás_Callback(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _modositott, string _korosztalyazonosito );
        public void Eredmény_Beírás(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _modositott, string _korosztalyazonosito)
        {
            if (InvokeRequired)
            {
                Eredmény_Beírás_Callback callback = new Eredmény_Beírás_Callback(Eredmény_Beírás);
                Invoke(callback, new object[] { _név, _verseny, _íjtípus, _csapat, _megjelent });
            }
            else
            {
                Database.BeírásEredmény beírás = Program.database.EredményBeírás(
                   _név,
                   _verseny, 
                   _íjtípus, 
                   _csapat, 
                   _megjelent,
                   _modositott,
                   _korosztalyazonosito
                );

                if (beírás.eredmény == null) { MessageBox.Show("Adatbázis hiba a beírás során!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (_verseny == combo_versenyek.Text)
                {
                    if (beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                    {
                        DataRow row = data.NewRow();
                            row[0] = beírás.eredmény.Value.Nev;
                            row[1] = beírás.eredmény.Value.Sorszam;
                            row[2] = beírás.eredmény.Value.Ijtipus;
                            row[3] = beírás.eredmény.Value.Csapat;
                            row[4] = beírás.eredmény.Value.Talalat10;
                            row[5] = beírás.eredmény.Value.Talalat8;
                            row[6] = beírás.eredmény.Value.Talalat5;
                            row[7] = beírás.eredmény.Value.Melle;
                            row[8] = beírás.eredmény.Value.Osszpont;
                            row[9] = beírás.eredmény.Value.Szazalek;
                            row[10] = beírás.eredmény.Value.Megjelent;
                        data.Rows.Add(row);
                    }
                    else
                    {
                        foreach (DataRow current in data.Rows)
                        {
                            if (beírás.eredmény.Value.Nev == (string)current[0])
                            {
                                current[0] = beírás.eredmény.Value.Nev;
                                //current[1] = beírás.eredmény.sorszám;
                                current[2] = beírás.eredmény.Value.Ijtipus;
                                current[3] = beírás.eredmény.Value.Csapat;
                                current[10] = beírás.eredmény.Value.Megjelent;
                                break;
                            }
                        }
                    }
                }

                if (eredmény_beírva != null) eredmény_beírva(_verseny, beírás);
            }

            //rendezés
            table.Sort(table.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        //ÁTNÉZVE
        private delegate void Eredmény_Módosítás_Callback(string _azonosító, Eredmény _eredeti, Eredmény _eredmény);
        public void Eredmény_Módosítás(string _azonosító, Eredmény _eredeti, Eredmény _eredmény)
        {
            if (InvokeRequired)
            {
                Eredmény_Módosítás_Callback callback = new Eredmény_Módosítás_Callback(Eredmény_Módosítás);
                Invoke(callback, new object[] { _azonosító, _eredeti, _eredmény });
            }
            else
            {
                if (Program.database.EredményMódosítás(_azonosító, _eredeti, _eredmény) == -666) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_azonosító == combo_versenyek.Text)
                {
                    foreach (DataRow current in data.Rows)
                    {
                        if (_eredeti.Nev == current[0].ToString())
                        {
                            //current[0] = _eredmény.név;
                            //current[1] = _eredmény.sorszám;
                            //current[2] = _eredmény.íjtípus;
                            //current[3] = _eredmény.csapat;
                            current[4] = _eredmény.Talalat10;
                            current[5] = _eredmény.Talalat8;
                            current[6] = _eredmény.Talalat5;
                            current[7] = _eredmény.Melle;
                            current[8] = _eredmény.Osszpont;
                            current[9] = _eredmény.Szazalek;
                            //current[10] = _eredmény.megjelent;
                            break;
                        }
                    }
                }

                if (eredmény_módosítva != null) eredmény_módosítva(_azonosító, _eredeti, _eredmény);
            }
        }

        private delegate void Eredmény_Törlés_Callback(string _azonosító, Eredmény _eredmény);
        public void Eredmény_Törlés(string _azonosító, Eredmény _eredmény)
        {
            if (InvokeRequired)
            {
                Eredmény_Törlés_Callback callback = new Eredmény_Törlés_Callback(Eredmény_Törlés);
                Invoke(callback, new object[] { _azonosító, _eredmény });
            }
            else
            {
                if (!Program.database.EredményTörlés(_azonosító, _eredmény)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (_azonosító == combo_versenyek.Text)
                {
                    foreach (DataRow current in data.Rows)
                    {
                        if (_eredmény.Nev == current[0].ToString())
                        {
                            data.Rows.Remove(current);
                            break;
                        }
                    }
                }

                if (eredmény_törölve != null) eredmény_törölve(_azonosító, _eredmény);
            }
        }

        ///

        private delegate void Eredmény_Beírás_Hálózat_Callback( string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _nyomtat, bool _KorosztalyModositott, Connection _connection );
        public void Eredmény_Beírás_Hálózat(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _nyomtat, bool _KorosztalyModositott, Connection _connection)
        {
            if (InvokeRequired)
            {
                Eredmény_Beírás_Hálózat_Callback callback = new Eredmény_Beírás_Hálózat_Callback(Eredmény_Beírás_Hálózat);
                Invoke(callback, new object[] { _név, _verseny, _íjtípus, _csapat, _megjelent, _nyomtat,_KorosztalyModositott, _connection });
            }
            else
            {
                Database.BeírásEredmény beírás = Program.database.EredményBeírás_Ellenőrzött(_név, _verseny, _íjtípus, _csapat, _megjelent,_KorosztalyModositott );
                if (beírás.eredmény == null)
                {
                    _connection.Send(ServerCommand.ERROR, "Hiba az eredmény beírásakor!");
                    return;
                }
                else
                {
                    if (_verseny == combo_versenyek.Text)
                    {
                        if (beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                        {
                            DataRow row = data.NewRow();
                            row[0] = beírás.eredmény.Value.Nev;
                            row[1] = beírás.eredmény.Value.Sorszam;
                            row[2] = beírás.eredmény.Value.Ijtipus;
                            row[3] = beírás.eredmény.Value.Csapat;
                            row[4] = beírás.eredmény.Value.Talalat10;
                            row[5] = beírás.eredmény.Value.Talalat8;
                            row[6] = beírás.eredmény.Value.Talalat5;
                            row[7] = beírás.eredmény.Value.Melle;
                            row[8] = beírás.eredmény.Value.Osszpont;
                            row[9] = beírás.eredmény.Value.Szazalek;
                            row[10] = beírás.eredmény.Value.Megjelent;
                            data.Rows.Add(row);
                        }
                        else
                        {
                            foreach (DataRow current in data.Rows)
                            {
                                if (beírás.eredmény.Value.Nev == (string)current[0])
                                {
                                    current[0] = beírás.eredmény.Value.Nev;
                                    //current[1] = _eredmény.sorszám;
                                    current[2] = beírás.eredmény.Value.Ijtipus;
                                    current[3] = beírás.eredmény.Value.Csapat;
                                    //current[4] = beírás.eredmény.Value.találat_10;
                                    //current[5] = beírás.eredmény.Value.találat_08;
                                    //current[6] = beírás.eredmény.Value.találat_05;
                                    //current[7] = beírás.eredmény.Value.mellé;
                                    //current[8] = beírás.eredmény.Value.összpont;
                                    //current[9] = beírás.eredmény.Value.százalék;
                                    current[10] = beírás.eredmény.Value.Megjelent;
                                    break;
                                }
                            }
                        }
                    }

                    if (eredmény_beírva != null) eredmény_beírva(_verseny, beírás);
                    Verseny? verseny = Program.database.Verseny(_verseny);
                    if (_nyomtat)
                    {
                        if (verseny.Value.DublaBeirlap)
                        {
                            Nyomtat.Print( Nyomtat.NyomtatBeirolap( _verseny, beírás.eredmény.Value ) );
                            Nyomtat.Print( Nyomtat.NyomtatBeirolap( _verseny, beírás.eredmény.Value ) );
                        }
                        else
                        {
                            Nyomtat.Print( Nyomtat.NyomtatBeirolap( _verseny, beírás.eredmény.Value ) );
                        }
                    }
                }
            }
        }

        private delegate void Eredmény_Módosítás_Hálózat_Callback(string _azonosító, Eredmény _eredeti, Eredmény _eredmény, Connection _connection);
        public void Eredmény_Módosítás_Hálózat(string _azonosító, Eredmény _eredeti, Eredmény _eredmény, Connection _connection)
        {
            if (InvokeRequired)
            {
                Eredmény_Módosítás_Hálózat_Callback callback = new Eredmény_Módosítás_Hálózat_Callback(Eredmény_Módosítás_Hálózat);
                Invoke(callback, new object[] { _azonosító, _eredeti, _eredmény, _connection });
            }
            else
            {
                Eredmény? eredmény = Program.database.EredményMódosítás_Ellenőrzött(_azonosító, _eredeti, _eredmény);
                if (eredmény == null)
                {
                    _connection.Send(ServerCommand.ERROR, "Hiba az eredmény módosításakor!");
                    return;
                }
                else
                {
                    if (_azonosító == combo_versenyek.Text)
                    {
                        foreach (DataRow current in data.Rows)
                        {
                            if (_eredeti.Nev == current[0].ToString())
                            {
                                current[0] = eredmény.Value.Nev;
                                current[1] = eredmény.Value.Sorszam;
                                current[2] = eredmény.Value.Ijtipus;
                                current[3] = eredmény.Value.Csapat;
                                current[4] = eredmény.Value.Talalat10;
                                current[5] = eredmény.Value.Talalat8;
                                current[6] = eredmény.Value.Talalat5;
                                current[7] = eredmény.Value.Melle;
                                current[8] = eredmény.Value.Osszpont;
                                current[9] = eredmény.Value.Szazalek;
                                current[10] = eredmény.Value.Megjelent;
                                break;
                            }
                        }
                    }

                    if (eredmény_módosítva != null) eredmény_módosítva(_azonosító, _eredeti, eredmény.Value);
                }
            }
        }
        #endregion

        #region EventHandlers
        public void verseny_hozzáadás(Verseny _verseny)
        {
            combo_versenyek.Items.Add(_verseny.Azonosito);
        }

        public void verseny_módosítás(string _azonosító, Verseny _verseny)
        {
            if (_azonosító != _verseny.Azonosito)
            {
                for (int current = 0; current < combo_versenyek.Items.Count; ++current)
                {
                    if (_azonosító == combo_versenyek.Items[current].ToString())
                    {
                        combo_versenyek.Items[current] = _verseny.Azonosito;
                        break;
                    }
                }
            }
        }

        public void verseny_törlés(string _azonosító)
        {
            combo_versenyek.Items.Remove(_azonosító);
        }

        public void verseny_lezárás(string _azonosító)
        {
            if (_azonosító == combo_versenyek.Text)
            {
                lezárva = true;
            }
        }

        public void verseny_megnyitás(string _azonosító)
        {
            if (_azonosító == combo_versenyek.Text)
            {
                lezárva = false;
            }
        }

        ///

        public void induló_átnevezés(string _eredeti_név, string _új_név)
        {
            foreach (DataRow current in data.Rows)
            {
                if (_eredeti_név == current[0].ToString())
                {
                    current[0] = _új_név;
                    break;
                }
            }
        }

        ///

        private void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 200;
            table.Columns[1].Width = 50;
            table.Columns[2].Width = 80;
            table.Columns[3].Width = 45;
            table.Columns[4].Width = 35;
            table.Columns[5].Width = 35;
            table.Columns[6].Width = 35;
            table.Columns[7].Width = 35;
            table.Columns[8].Width = 45;
            table.Columns[9].Width = 45;
            table.Columns[10].Width = 58;

            table.Columns[11].Visible = false;
            table.Columns[12].Visible = false;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;

            //rendezés
            table.Sort(table.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void combo_versenyek_SelectedIndexChanged(object _sender, EventArgs _event)
        {
            List<Eredmény> eredmények = Program.database.Eredmények(combo_versenyek.Text);

            data.Rows.Clear();

            foreach (Eredmény current in eredmények)
            {
                DataRow row = data.NewRow();
                row[0] = current.Nev;
                row[1] = current.Sorszam;
                row[2] = current.Ijtipus;
                row[3] = current.Csapat;
                row[4] = current.Talalat10;
                row[5] = current.Talalat8;
                row[6] = current.Talalat5;
                row[7] = current.Melle;
                row[8] = current.Osszpont;
                row[9] = current.Szazalek;
                row[10] = current.Megjelent;
                row[11] = current.KorosztalyModositott;
                row[12] = current.KorosztalyAzonosito;

                data.Rows.Add(row);
            }

            lezárva = Program.database.Verseny_Lezárva(combo_versenyek.SelectedItem.ToString());
            verseny_összpont = Program.database.Verseny_Összespont(combo_versenyek.SelectedItem.ToString());
        }

        private void módosítás_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.Items.Count == 0) { MessageBox.Show("Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (table.SelectedRows.Count == 0) { MessageBox.Show("Nem megfelelő a kiválasztott eredmények száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            Form_Eredmény eredmény_form = new Form_Eredmény(combo_versenyek.Text, 
                                                            verseny_összpont, 
                                                            new Eredmény(table.SelectedRows[0].Cells[0].Value.ToString(),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[1].Value),
                                                                         table.SelectedRows[0].Cells[2].Value.ToString(),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[3].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[4].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[5].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[6].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[7].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[8].Value),
                                                                         Convert.ToInt32(table.SelectedRows[0].Cells[9].Value),
                                                                         Convert.ToBoolean(table.SelectedRows[0].Cells[10].Value),
                                                                         Convert.ToBoolean(table.SelectedRows[0].Cells[11].Value),
                                                                         table.SelectedRows[0].Cells[12].Value.ToString())
                );
            eredmény_form.ShowDialog();
        }

        private void törlés_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (!(table.SelectedRows.Count != 0 && table.SelectedRows[0].Index < data.Rows.Count && table.SelectedRows[0].Selected)) return;
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            if (MessageBox.Show("Biztosan törli ezt az eredményt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (DataGridViewRow current in table.Rows)
            {
                if (table.SelectedRows[0] == current)
                {
                    Eredmény_Törlés(combo_versenyek.Text, new Eredmény(

                        current.Cells[0].Value.ToString(),
                        Convert.ToInt32(current.Cells[1].Value),
                        current.Cells[2].Value.ToString(),
                        Convert.ToInt32(current.Cells[3].Value),
                        Convert.ToInt32(current.Cells[4].Value),
                        Convert.ToInt32(current.Cells[5].Value),
                        Convert.ToInt32(current.Cells[6].Value),
                        Convert.ToInt32(current.Cells[7].Value),
                        Convert.ToInt32(current.Cells[8].Value),
                        Convert.ToInt32(current.Cells[9].Value),
                        Convert.ToBoolean(current.Cells[10].Value),
                        Convert.ToBoolean(current.Cells[11].Value),
                        current.Cells[12].Value.ToString()
                        ));
                }
            }
        }

        private void lezárás_Click(object _sender, EventArgs _event)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };
            if (MessageBox.Show("Biztosan lezárja a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            foreach (DataRow current in data.Rows)
            {
                if (Convert.ToInt32(current[4]) + Convert.ToInt32(current[5]) + Convert.ToInt32(current[6]) + Convert.ToInt32(current[7]) != (verseny_összpont) && current[10].ToString() != "False")
                { MessageBox.Show("Nem megfelelő a pontozás a következő indulónál: " + current[0].ToString() + "!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }

            Program.mainform.verseny_panel.Verseny_Lezárás(combo_versenyek.Text);
            Program.mainform.korosztályok_panel.Verseny_Számolás(combo_versenyek.Text);
        }

        void megnyitás_Click(object sender, EventArgs e)
        {
            if (combo_versenyek.SelectedItem == null) return;
            if (!lezárva) { return; };
            if (MessageBox.Show("Biztosan megnyitja a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            //Program.mainform.verseny_panel.verse
            Program.mainform.verseny_panel.Verseny_Megnyitás(combo_versenyek.Text);
        }

        void keresés_TextChanged(object sender, EventArgs e)
        {
            if (keresés.Text.ToString().Length == 0)
                return;

            foreach (DataGridViewRow item in table.Rows)
            {
                bool találat = true;
                for (int i = 0; i < keresés.Text.Length; i++)
                {
                    if ((item.Cells[1].Value.ToString()) != keresés.Text )
                    {
                        találat = false;
                    }

                }
                if (találat == true)
                {
                    table.Rows[item.Index].Selected = true;
                    table.FirstDisplayedScrollingRowIndex = item.Index;
                    return;
                }

            }
        }

        void keresés_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (keresés.Focus() == true && keresés.Text.Length != 0)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (combo_versenyek.Items.Count == 0) { MessageBox.Show("Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (table.SelectedRows.Count == 0) { MessageBox.Show("Nem megfelelő a kiválasztott eredmények száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (lezárva) { MessageBox.Show("A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

                    Form_Eredmény eredmény_form = new Form_Eredmény(combo_versenyek.Text, verseny_összpont, new Eredmény(
                        table.SelectedRows[0].Cells[0].Value.ToString(),
                        Convert.ToInt32(table.SelectedRows[0].Cells[1].Value),
                        table.SelectedRows[0].Cells[2].Value.ToString(),
                        Convert.ToInt32(table.SelectedRows[0].Cells[3].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[4].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[5].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[6].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[7].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[8].Value),
                        Convert.ToInt32(table.SelectedRows[0].Cells[9].Value),
                        Convert.ToBoolean(table.SelectedRows[0].Cells[10].Value),
                        Convert.ToBoolean(table.SelectedRows[0].Cells[11].Value),
                        table.SelectedRows[0].Cells[12].Value.ToString()
                        ));
                    eredmény_form.ShowDialog();
                }
            }
        }     
        #endregion

    }
}
