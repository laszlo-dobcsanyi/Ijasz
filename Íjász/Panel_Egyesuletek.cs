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
    public struct Egyesulet
    {
        public string Azonosito;
        public string Nev;
        public string Cim;
        public string Vezeto;
        public string Telefon;
        public string Email;
        public bool Listazando;
        public int TagokSzama;

        public Egyesulet(   string _Azonosito, 
                            string _Nev, 
                            string _Cim, 
                            string _Vezeto, 
                            string _Telefon, 
                            string _Email, 
                            bool _Listazando, 
                            int _TagokSzama)
        {
            Azonosito = _Azonosito;
            Nev = _Nev;
            Cim = _Cim;
            Vezeto = _Vezeto;
            Telefon = _Telefon;
            Email = _Email;
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

            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Cím", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Vezető", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Telefon", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("E-mail", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Listázandó", System.Type.GetType("System.Boolean")));
            data.Columns.Add(new DataColumn("Tagok száma", System.Type.GetType("System.Int32")));

            List<Egyesulet> Egyesuletek = Program.database.Egyesuletek();

            foreach (Egyesulet current in Egyesuletek)
            {
                DataRow row = data.NewRow();
                row[0] = current.Azonosito;
                row[1] = current.Nev;
                row[2] = current.Cim;
                row[3] = current.Vezeto;
                row[4] = current.Telefon;
                row[5] = current.Email;
                row[6] = current.Listazando;
                row[7] = current.TagokSzama;
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
                if (_egyesulet.Azonosito.Contains(" ")) 
                {
                    MessageBox.Show("Az azonosito nem tartalmazhat szóközt!",
                                    "Hiba", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    return; 
                }
                if (!Program.database.UjEgyesulet(_egyesulet)) 
                { 
                    MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", 
                                    "Hiba", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error); 
                    return; 
                }

                DataRow row = data.NewRow();
                row[0] = _egyesulet.Azonosito;
                row[1] = _egyesulet.Nev;
                row[2] = _egyesulet.Cim;
                row[3] = _egyesulet.Vezeto;
                row[4] = _egyesulet.Telefon;
                row[5] = _egyesulet.Email;
                row[6] = _egyesulet.TagokSzama;
                row[7] = _egyesulet.Listazando;
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
                        current[1] = _uj.Nev;
                        current[2] = _uj.Cim;
                        current[3] = _uj.Vezeto;
                        current[4] = _uj.Telefon;
                        current[5] = _uj.Email;
                        current[6] = _uj.Listazando;
                        current[7] = _uj.TagokSzama;

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

            table.Columns[0].Width = 110;
            table.Columns[1].Width = 99;
            table.Columns[2].Width = 99;
            table.Columns[3].Width = 99;
            table.Columns[4].Width = 99;
            table.Columns[5].Width = 99;
            table.Columns[6].Width = 99;
            table.Columns[7].Width = 99;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void 
        Modositas_Click(object _sender, EventArgs _event)
        {
            Egyesulet egyesulet = new Egyesulet( data.Rows[table.SelectedRows[0].Index][0].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][1].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][2].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][3].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][4].ToString(),
                                                 data.Rows[table.SelectedRows[0].Index][5].ToString(),
                                                 Convert.ToBoolean(data.Rows[table.SelectedRows[0].Index][6]),
                                                 Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][7]));

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
            if (table.SelectedRows.Count != 1) return;
            if (0 < (int)(data.Rows[table.SelectedRows[0].Index][7])) { MessageBox.Show("Ez az egyesület nem törölhető, mivel van hozzárendelve induló!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (MessageBox.Show("Biztosan törli ezt az egyesületet?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Program.mainform.egyesuletek_panel.Egyesulet_Torles(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }

        #endregion

        public sealed class Form_Egyesulet : Form
        {
            private Egyesulet? egyesulet;
            TextBox txtAzonosito; 
            TextBox txtNev ;
            TextBox txtCim ;
            TextBox txtVezeto;
            TextBox txtTelefon;
            TextBox txtEmail;
            CheckBox chkListazando;

            public 
            Form_Egyesulet()
            {
                InitializeContent();
                InitializeForm();
            }

            public 
            Form_Egyesulet( Egyesulet _egyesulet )
            {
                egyesulet = _egyesulet;
                InitializeContent();
                InitializeForm();
                InitializeData(egyesulet);
            }

            private void 
            InitializeForm()
            {
                Text = "Egyesulet";
                ClientSize = new System.Drawing.Size(400, 250);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void 
            InitializeContent()
            {
                Label lblAzonosito = new iLabel("Azonosító:",new Point(16, 16 + 0 * 32),this);

                Label lblNev = new iLabel("Egyesület neve:",new Point(16, 16 + 1 * 32),this);

                Label lblCim = new iLabel("Egyesület címe:",new Point(16, 16 + 2 * 32),this);

                Label lblVezeto = new iLabel("Vezető:",new Point(16, 16 + 3 * 32),this);

                Label lblTelefon = new iLabel("Telefon:",new Point(16, 16 + 4 * 32),this);

                Label lblEmail = new iLabel("E-mail:",new Point(16, 16 + 5 * 32),this);

                Label lblListazando = new iLabel("Listázandó:",new Point(16, 16 + 6 * 32),this);

                txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Width + 16, lblAzonosito.Location.Y ),
                                                    10,
                                                    new System.Drawing.Size( 128 + 64, 24),
                                                    null,
                                                    this); 

                txtNev = new iTextBox( new Point( txtAzonosito.Location.X,lblNev.Location.Y ),
                                                    30,
                                                    new System.Drawing.Size( 128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                txtCim = new iTextBox( new Point( txtAzonosito.Location.X,lblCim.Location.Y ),
                                                    30,
                                                    new System.Drawing.Size(128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                txtVezeto = new iTextBox( new Point( txtAzonosito.Location.X,lblVezeto.Location.Y ),
                                                    30,
                                                    new System.Drawing.Size(128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                txtTelefon =new iTextBox( new Point( txtAzonosito.Location.X,lblTelefon.Location.Y ),
                                                    10,
                                                    new System.Drawing.Size( 128 + 64, 24),
                                                    null,
                                                    this); 

                txtEmail = new iTextBox( new Point( txtAzonosito.Location.X,lblEmail.Location.Y ),
                                                    30,
                                                    new System.Drawing.Size(128 + 2 * 64, 24),
                                                    null,
                                                    this); 

                chkListazando = new iCheckBox( new Point( lblListazando.Location.X + lblListazando.Width + 16,lblListazando.Location.Y),
                                                       this );

                Button btnRendben = new iButton("Rendben",
                                                new Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16),
                                                new Size(96, 32),
                                                btnRendben_Click,
                                                this);
                //teszteléshez

                txtAzonosito.Text = "azonosito";
                txtNev.Text = "megnevezes";
                txtCim.Text = "cim";
                txtVezeto.Text = "vezeto bela";
                txtTelefon.Text = "36303705065";
                txtEmail.Text = "asd@gmail.com";
            }

            private void 
            InitializeData(Egyesulet? _egyesulet)
            {
                txtAzonosito.Text = _egyesulet.Value.Azonosito;
                txtNev.Text = _egyesulet.Value.Nev;
                txtCim.Text = _egyesulet.Value.Cim;
                txtVezeto.Text = _egyesulet.Value.Vezeto;
                txtTelefon.Text = _egyesulet.Value.Telefon;
                txtEmail.Text = _egyesulet.Value.Email;
                chkListazando.Checked = _egyesulet.Value.Listazando;
            }

            #region EventHandlers

            private void btnRendben_Click(Object _sender, EventArgs _event)
            {
                if (txtAzonosito.Text.Length == 0) { MessageBox.Show("Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (txtNev.Text.Length == 0) { MessageBox.Show("Nem megfelelő a név hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (txtCim.Text.Length == 0) { MessageBox.Show("Nem megfelelő a címhossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (txtVezeto.Text.Length == 0) { MessageBox.Show("Nem megfelelő a vezető hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (txtTelefon.Text.Length == 0) { MessageBox.Show("Nem megfelelő a telefon hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (txtEmail.Text.Length == 0) { MessageBox.Show("Nem megfelelő az e-mail hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (egyesulet == null)
                {
                    egyesulet = new Egyesulet(txtAzonosito.Text, 
                                                        txtNev.Text,
                                                        txtCim.Text, 
                                                        txtVezeto.Text, 
                                                        txtTelefon.Text,
                                                        txtEmail.Text,
                                                        chkListazando.Checked, 
                                                        0);

                    Program.mainform.egyesuletek_panel.Egyesulet_Hozzaadas(egyesulet.Value);
                }
                else
                {
                    Egyesulet uj = new Egyesulet(txtAzonosito.Text,
                                    txtNev.Text,
                                    txtCim.Text,
                                    txtVezeto.Text,
                                    txtTelefon.Text,
                                    txtEmail.Text,
                                    chkListazando.Checked,
                                    0);
                    Egyesulet regi = egyesulet.Value;

                    Program.mainform.egyesuletek_panel.Egyesulet_Modositas(regi,uj);
                }

                this.Close();
            }                                 
            #endregion                        
        }                                     
    }                                         
}

