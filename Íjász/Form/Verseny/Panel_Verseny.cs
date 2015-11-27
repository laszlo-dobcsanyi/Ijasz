using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Íjász
{
    public struct Verseny
    {
        public string Azonosito;
        public string Megnevezes;
        public string Datum;
        public string VersenySorozat;
        public int Osszes;
        public int Allomasok;
        public int Indulok;
        public bool Lezarva;
        public bool DublaBeirlap;


        public Verseny( string _Azonosito, string _Megnevezes, string _Datum, string _VersenySorozat, int _Osszes, int _Allomasok, int _Indulok, bool _Lezarva, bool _DublaBeirlap )
        {
            Azonosito = _Azonosito;
            Megnevezes = _Megnevezes;
            Datum = _Datum;
            VersenySorozat = _VersenySorozat;
            Osszes = _Osszes;
            Allomasok = _Allomasok;
            Indulok = _Indulok;
            Lezarva = _Lezarva;
            DublaBeirlap = _DublaBeirlap;
        }
    }

    public delegate void Verseny_Hozzáadva(Verseny _verseny);
    public delegate void Verseny_Módosítva(string _azonosító, Verseny _verseny);
    public delegate void Verseny_Törölve(string _azonosító);
    public delegate void Verseny_Lezárva(string _azonosító);

    public sealed class Panel_Verseny : Control
    {
        public Verseny_Hozzáadva verseny_hozzáadva;
        public Verseny_Módosítva verseny_módosítva;
        public Verseny_Törölve verseny_törölve;
        public Verseny_Lezárva verseny_lezárva;
        public Verseny_Lezárva verseny_megnyitva;

        public DataTable data;
        private DataGridView table;

        public Form_Verseny verseny_form;

        public Panel_Verseny()
        {
            InitializeContent();

            verseny_form = new Form_Verseny();
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
            table.Width = 803;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;

            Button btnHozzaadas = new iButton( "Hozzáadás",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 32 - 32 ),
                                                new Size( 96, 32 ),
                                                btnHozzaadas_Click,
                                                this );

            Button btnTorles = new iButton("Törlés",
                                            new Point( ClientRectangle.Width - 96 - 16 , ClientRectangle.Height - 32 - 16 ),
                                            new Size(96, 32),
                                            btnTorles_Click,
                                            this);

            Controls.Add(table);
        }

        private DataTable CreateSource()
        {
            data = new DataTable();

            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Dátum", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Versenysorozat", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Lövések", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Állomások", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Indulók száma", System.Type.GetType("System.Int32")));
            data.Columns.Add(new DataColumn("Lezárva", System.Type.GetType("System.Boolean")));
            data.Columns.Add(new DataColumn("Dupla Beírólap", System.Type.GetType("System.Boolean")));

            List<Verseny> versenysorozatok = Program.database.Versenyek();

            foreach (Verseny current in versenysorozatok)
            {
                DataRow row = data.NewRow();
                row[0] = current.Azonosito;
                row[1] = current.Megnevezes;
                row[2] = current.Datum;
                row[3] = current.VersenySorozat;
                row[4] = current.Osszes;
                row[5] = current.Allomasok;
                row[6] = current.Indulok;
                row[7] = current.Lezarva;
                row[8] = current.DublaBeirlap;

                data.Rows.Add(row);
            }

            return data;
        }

        #region Accessors
        private delegate void Verseny_Hozzáadás_Callback(Verseny _verseny);
        public void Verseny_Hozzáadás(Verseny _verseny)
        {
            if (InvokeRequired)
            {
                Verseny_Hozzáadás_Callback callback = new Verseny_Hozzáadás_Callback(Verseny_Hozzáadás);
                Invoke(callback, new object[] { _verseny });
            }
            else
            {
                if (_verseny.Azonosito.Contains(" ")) { MessageBox.Show("A versenyazonosító nem tartalmazhat szóközt!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Program.database.ÚjVerseny(_verseny)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _verseny.Azonosito;
                row[1] = _verseny.Megnevezes;
                row[2] = _verseny.Datum;
                row[3] = _verseny.VersenySorozat;
                row[4] = _verseny.Osszes;
                row[5] = _verseny.Allomasok;
                row[6] = _verseny.Indulok;
                row[7] = _verseny.Lezarva;
                row[8] = _verseny.DublaBeirlap;
                data.Rows.Add(row);

                if (verseny_hozzáadva != null) verseny_hozzáadva(_verseny);
            }
        }

        private delegate void Verseny_Módosítva_Callback(string _azonosító, Verseny _verseny);
        public void Verseny_Módosítás(string _azonosító, Verseny _verseny)
        {
            if (InvokeRequired)
            {
                Verseny_Módosítva_Callback callback = new Verseny_Módosítva_Callback(Verseny_Módosítás);
                Invoke(callback, new object[] { _azonosító, _verseny });
            }
            else
            {
                if (!Program.database.VersenyMódosítás(_azonosító, _verseny)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[0] = _verseny.Azonosito;
                        current[1] = _verseny.Megnevezes;
                        current[2] = _verseny.Datum;
                        current[3] = _verseny.VersenySorozat;
                        current[4] = _verseny.Osszes;
                        current[5] = _verseny.Allomasok;
                        current[6] = _verseny.Indulok;
                        current[7] = _verseny.Lezarva;
                        current[8] = _verseny.DublaBeirlap;
                        break;
                    }
                }

                if (verseny_módosítva != null) verseny_módosítva(_azonosító, _verseny);
            }
        }

        private delegate void Verseny_Törölve_Callback(string _azonosító);
        public void Verseny_Törlés(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Törölve_Callback callback = new Verseny_Törölve_Callback(Verseny_Törlés);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.VersenyTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        data.Rows.RemoveAt(table.SelectedRows[0].Index);
                        break;
                    }
                }

                if (verseny_törölve != null) verseny_törölve(_azonosító);
            }
        }

        private delegate void Verseny_Lezárás_Callback(string _azonosító);
        public void Verseny_Lezárás(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Lezárás_Callback callback = new Verseny_Lezárás_Callback(Verseny_Lezárás);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.Verseny_Lezárás(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[6] = true;
                        break;
                    }
                }

                if (verseny_lezárva != null) verseny_lezárva(_azonosító);
            }
        }

        private delegate void Verseny_Megnyitás_Callback(string _azonosító);
        public void Verseny_Megnyitás(string _azonosító)
        {
            if (InvokeRequired)
            {
                Verseny_Megnyitás_Callback callback = new Verseny_Megnyitás_Callback(Verseny_Lezárás);
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.Verseny_Megnyitás(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[6] = false;
                        break;
                    }
                }

                if (verseny_megnyitva != null) verseny_megnyitva(_azonosító);
            }
        }
        #endregion

        #region EventHandlers
        public void eredmény_beírás(string _azonosító, Database.BeírásEredmény _beírás)
        {
            if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
            {
                if (!Program.database.Verseny_IndulókNövelés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[5] = ((int)current[6]) + 1;
                        return;
                    }
                }

                MessageBox.Show("Nem taláható a verseny?!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void eredmény_törlés(string _azonosító, Eredmény _eredmény)
        {
            if (!Program.database.Verseny_IndulókCsökkentés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[5] = ((int)current[6]) - 1;
                    return;
                }
            }

            MessageBox.Show("Nem taláható a verseny?!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 80;
            table.Columns[1].Width = 200;
            table.Columns[2].Width = 90;
            table.Columns[3].Width = 80;
            table.Columns[4].Width = 60;
            table.Columns[5].Width = 60;
            table.Columns[6].Width = 90;
            table.Columns[7].Width = 50;
            table.Columns[8].Width = 90;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void btnHozzaadas_Click(object _sender, EventArgs _event)
        {
            verseny_form = new Form_Verseny();
            verseny_form.ShowDialog();
        }

        private void Modositas_Click( object _sender, EventArgs _event )
        {
            if (table.SelectedRows.Count != 1) return;

            verseny_form = new Form_Verseny(new Verseny(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                                                        data.Rows[table.SelectedRows[0].Index][1].ToString(), 
                                                        data.Rows[table.SelectedRows[0].Index][2].ToString(),
                                                        data.Rows[table.SelectedRows[0].Index][3].ToString(),
                                                        Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][4]),
                                                        Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][5]),
                                                        Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][6]),
                                                        Convert.ToBoolean(data.Rows[table.SelectedRows[0].Index][7]),
                                                        data.Rows[table.SelectedRows[0].Index][8].ToString()=="True"));
            verseny_form.ShowDialog();
        }

        private void btnTorles_Click(object _sender, EventArgs _event)
        {
            if (table.SelectedRows.Count != 1) return;
            //TODO: ez igy milyen?
            Verseny? verseny = Program.database.Verseny( (string)( data.Rows[table.SelectedRows[0].Index][0] ) );
            
            if (verseny.Value.Indulok!=0) { MessageBox.Show("Ez a verseny nem törölhető, mivel van hozzárendelve eredmény!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBox.Show("Biztosan törli ezt a versenyt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // TODO: ezt egyben kellene!
            Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés(data.Rows[table.SelectedRows[0].Index][3].ToString());
            Verseny_Törlés(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion
      /*
        public sealed class Form_Verseny : Form
        {
            private string eredeti_azonosító = null;
            private string eredeti_versenysorozat = null;

            private TextBox txtAzonosito;
            private TextBox txtMegnevezes;
            private TextBox txtLovesek;
            private TextBox txtAllomasok;
            private DateTimePicker dátumválasztó;
            private ComboBox cboVersenySorozat;
            private CheckBox chkDuplaBeirolap;
            private Label lblIndulok;
            private Label lblLezarva;

            public Form_Verseny()
            {
                InitializeForm();
                InitializeContent();
                InitializeData();
            }

            public Form_Verseny(Verseny _verseny)
            {
                eredeti_azonosító = _verseny.Azonosito;
                eredeti_versenysorozat = _verseny.VersenySorozat;

                InitializeForm(_verseny);
                InitializeContent(_verseny);
                InitializeData(_verseny);
            }

            #region Verseny Hozzáadás
            private void InitializeForm( )
            {
                Text = "Verseny";
                ClientSize = new Size( 400 - 64, 280 );
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }
            
            private void InitializeContent( )
            {
                Label lblAzonosito = new iLabel( "Azonosító:",
                                                new Point( 16, 16 + 0 * 32 ),
                                                this );

                Label lblMegnevezes = new iLabel( "Megnevezés:",
                                                new Point( lblAzonosito.Location.X, 16 + 1 * 32 ),
                                                this );

                Label lblDatum = new iLabel( "Dátum:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

                Label lblVersenySorozat = new iLabel( "Versenysorozat:",
                                                    new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                                    this );

                Label lblLovesek = new iLabel( "Lövések száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                                this );

                Label lblAllomasok = new iLabel( "Állomások száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                                this );

                Label lblDuplaBeirolap = new iLabel( "Dupla beírólap:",
                                                    new Point( lblAzonosito.Location.X, 16 + 6 * 32 ),
                                                    this );

                txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Size.Width + 16 + 32, lblAzonosito.Location.Y ),
                                            10,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                            30,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                dátumválasztó = new DateTimePicker( );
                dátumválasztó.Location = new Point( txtAzonosito.Location.X, lblDatum.Location.Y );
                dátumválasztó.Size = txtAzonosito.Size;
                dátumválasztó.Value = DateTime.Now;

                cboVersenySorozat = new iComboBox( new Point( txtAzonosito.Location.X, lblVersenySorozat.Location.Y ),
                                                   new Size( 128 + 64, 24 ),
                                                   null,
                                                   this );

                cboVersenySorozat.Items.Add( "" );
                List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
                foreach ( Versenysorozat current in versenysorozatok ) { cboVersenySorozat.Items.Add( current.azonosító ); }

                txtLovesek = new iTextBox( new Point( txtAzonosito.Location.X, lblLovesek.Location.Y ),
                                            null,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtAllomasok = new iTextBox( new Point( txtAzonosito.Location.X, lblAllomasok.Location.Y ),
                                            30,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                chkDuplaBeirolap = new iCheckBox( null,
                                                new Point( txtAzonosito.Location.X, lblDuplaBeirolap.Location.Y ),
                                                null,
                                                this );
                chkDuplaBeirolap.Size = txtAzonosito.Size;

                Button btnRendben = new iButton( "Rendben",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

                Controls.Add( dátumválasztó );
            }

            private void InitializeData()
            {
                dátumválasztó.Value = DateTime.Now;
            }
            #endregion

            #region Verseny Módosítás
            private void InitializeForm( Verseny _verseny )
            {
                Text = "Verseny";
                ClientSize = new Size(400 - 64, 358);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent(Verseny _verseny )
            {
                Label lblAzonosito = new iLabel( "Azonosító:",
                                                new Point( 16, 16 + 0 * 32 ),
                                                this );

                Label lblMegnevezes = new iLabel( "Megnevezés:",
                                                new Point( lblAzonosito.Location.X, 16 + 1 * 32 ),
                                                this );

                Label lblDatum = new iLabel( "Dátum:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

                Label lblVersenySorozat = new iLabel( "Versenysorozat:",
                                                    new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                                    this );

                Label lblLovesek = new iLabel( "Lövések száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                                this );

                Label lblAllomasok = new iLabel( "Állomások száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                                this );

                lblIndulok = new iLabel( "Indulók száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 6 * 32 ),
                                                this );

                Label lblDuplaBeirolap = new iLabel( "Dupla beírólap:",
                                                    new Point( lblAzonosito.Location.X, 16 + 7 * 32 ),
                                                    this );

                lblLezarva = new iLabel( "Lezárva:",
                                                new Point( lblAzonosito.Location.X, 16 + 8 * 32 ),
                                                this );

                txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Size.Width + 16 + 32, lblAzonosito.Location.Y ),
                                            10,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                            30,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                dátumválasztó = new DateTimePicker( );
                dátumválasztó.Location = new Point( txtAzonosito.Location.X, lblDatum.Location.Y );
                dátumválasztó.Size = txtAzonosito.Size;
                dátumválasztó.Value = DateTime.Now;

                cboVersenySorozat = new iComboBox( new Point( txtAzonosito.Location.X, lblVersenySorozat.Location.Y ),
                                                   new Size( 128 + 64, 24 ),
                                                   null,
                                                   this );

                cboVersenySorozat.Items.Add( "" );
                List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
                foreach ( Versenysorozat current in versenysorozatok ) { cboVersenySorozat.Items.Add( current.azonosító ); }

                txtLovesek = new iTextBox( new Point( txtAzonosito.Location.X, lblLovesek.Location.Y ),
                                            null,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtAllomasok = new iTextBox( new Point( txtAzonosito.Location.X, lblAllomasok.Location.Y ),
                                            30,
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

                chkDuplaBeirolap = new iCheckBox( null,
                                                new Point( txtAzonosito.Location.X, lblDuplaBeirolap.Location.Y ),
                                                null,
                                                this );
                chkDuplaBeirolap.Size = txtAzonosito.Size;

                lblIndulok = new iLabel( null,
                                        new Point( txtAzonosito.Location.X, lblIndulok.Location.Y ),
                                        this );
                lblIndulok.Size = txtAzonosito.Size;

                lblLezarva = new iLabel( null,
                                        new Point( txtAzonosito.Location.X, lblLezarva.Location.Y ),
                                        this );

                Button btnRendben = new iButton( "Rendben",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

                Controls.Add( dátumválasztó );
            }

            private void InitializeData(Verseny _verseny)
            {
                txtAzonosito.Text = _verseny.Azonosito;
                txtAzonosito.Enabled = (_verseny.Indulok == 0) ? true : false;
                txtMegnevezes.Text = _verseny.Megnevezes;
                dátumválasztó.Value = DateTime.Parse(_verseny.Datum);
                cboVersenySorozat.Text = _verseny.VersenySorozat;
                txtLovesek.Text = (_verseny.Osszes).ToString();
                txtLovesek.Enabled = ( _verseny.Indulok == 0 ) ? true : false;
                txtAllomasok.Text = (_verseny.Allomasok).ToString();
                txtAllomasok.Enabled = (_verseny.Indulok == 0) ? true : false;
                lblIndulok.Text = _verseny.Indulok.ToString();
                lblLezarva.Text = _verseny.Lezarva ? "Igen" : "Nem";
                chkDuplaBeirolap.Checked = _verseny.DublaBeirlap;
            }
            #endregion

            #region EventHandlers

            public void VersenySorozatHozzaadas(Versenysorozat _versenysorozat)
            {
                cboVersenySorozat.Items.Add(_versenysorozat.megnevezés + " (" + _versenysorozat.azonosító + ")");
            }

            public void VersenySorozatModositas(string _azonosító, Versenysorozat _versenysorozat)
            {
                if (_azonosító != _versenysorozat.azonosító)
                {
                    for ( int current = 0; current < cboVersenySorozat.Items.Count; ++current )
                    {
                        if ( _azonosító == cboVersenySorozat.Items[current].ToString( ) )
                        {
                            cboVersenySorozat.Items[current] = _versenysorozat.azonosító;
                            return;
                        }
                    }
                }
            }

            public void VeresenySorozatTorles(string _azonosító)
            {
                cboVersenySorozat.Items.Remove( _azonosító );
            }

            public void EredmenyBeiras(string _azonosító, Database.BeírásEredmény _beírás)
            {
                if (_beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT)
                {
                    if (_azonosító == eredeti_azonosító) lblIndulok.Text = ((Convert.ToInt32(lblIndulok.Text)) + 1).ToString();
                }
            }

            public void EredmenyTorles(string _azonosító, Eredmény _eredmény)
            {
                if (_azonosító == eredeti_azonosító)
                {
                    lblIndulok.Text = ((Convert.ToInt32(lblIndulok.Text)) - 1).ToString();
                }
            }

            private void btnRendben_Click(object _sender, EventArgs _event)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                txtAzonosito.Text = rgx.Replace(txtAzonosito.Text, "");

                if ( !Database.IsCorrectSQLText( txtMegnevezes.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                int összes; try { összes = Convert.ToInt32(txtLovesek.Text); } catch { MessageBox.Show("Nem szám található a lövéseknél!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (összes < 1) { MessageBox.Show("Túl kevés a lövések száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                int állomások; try { állomások = Convert.ToInt32(txtAllomasok.Text); } catch { MessageBox.Show("Nem szám található az állomásoknál!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eredeti_azonosító != null)
                {
                    if ((0 < Convert.ToInt32(lblIndulok.Text)) && (eredeti_azonosító != txtAzonosito.Text))
                    { MessageBox.Show("Ez a verseny nem átnevezhető!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; };

                    // TODO: ezt sem kéne külön csinálni!
                    if (cboVersenySorozat.Text != eredeti_versenysorozat)
                    {
                        Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés(eredeti_versenysorozat);
                        Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(cboVersenySorozat.Text);
                    }

                    Program.mainform.verseny_panel.Verseny_Módosítás(eredeti_azonosító, new Verseny(txtAzonosito.Text, 
                                                                                                    txtMegnevezes.Text, 
                                                                                                    dátumválasztó.Value.ToShortDateString(),
                                                                                                    cboVersenySorozat.Text, 
                                                                                                    összes,
                                                                                                    állomások, Convert.ToInt32(lblIndulok.Text), 
                                                                                                    lblLezarva.Text == "Igen" ? true : false, 
                                                                                                    chkDuplaBeirolap.Checked ? true : false));
                    
                }
                else
                {
                    // TODO: ezt sem kéne külön csinálni!
                    Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(cboVersenySorozat.Text);
                    Program.mainform.verseny_panel.Verseny_Hozzáadás(new Verseny(txtAzonosito.Text, 
                                                                                txtMegnevezes.Text, 
                                                                                dátumválasztó.Value.ToShortDateString(), 
                                                                                cboVersenySorozat.Text, 
                                                                                összes, 
                                                                                állomások, 
                                                                                0, 
                                                                                false, 
                                                                                chkDuplaBeirolap.Checked ? true : false));
                }

                Close();
            }
            #endregion
        }
      */
    }
}
