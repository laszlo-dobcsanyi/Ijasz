using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Íjász
{
    public struct Versenysorozat
    {
        public string azonosító;
        public string megnevezés;
        public int versenyek;

        public Versenysorozat(string _azonosító, string _megnevezés, int _versenyek)
        {
            azonosító = _azonosító;
            megnevezés = _megnevezés;
            versenyek = _versenyek;
        }
    };

    public delegate void Versenysorozat_Hozzáadva(Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Módosítva(string _azonosító, Versenysorozat _versenysorozat);
    public delegate void Versenysorozat_Törölve(string _azonosító);

    public sealed class Panel_Versenysorozat : Control
    {
        public Versenysorozat_Hozzáadva versenysorozat_hozzáadva;
        public Versenysorozat_Módosítva versenysorozat_módosítva;
        public Versenysorozat_Törölve   versenysorozat_törölve;

        private DataTable data;
        private DataGridView table;


        public Panel_Versenysorozat()
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
            table.Width = 403;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.ReadOnly = true;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;

            Controls.Add(table);

            Button btnTorles = new iButton( "Törlés",
                                            new System.Drawing.Point( ClientRectangle.Width -96 - 96 - 32, ClientRectangle.Height - 32 - 16 ),
                                            new System.Drawing.Size( 96, 32 ),
                                            btnTorles_Click,
                                            this );
            Button btnHozzaadas = new iButton("Hozzáadás",
                                              new System.Drawing.Point(ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16),
                                              new System.Drawing.Size(96, 32),
                                              btnHozzaadas_Click,
                                              this);

        }

        private DataTable CreateSource()
        {
            data = new DataTable();
            
            data.Columns.Add(new DataColumn("Azonosító", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Megnevezés", System.Type.GetType("System.String")));
            data.Columns.Add(new DataColumn("Versenyek száma", System.Type.GetType("System.Int32")));

            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Versenysorozat current in versenysorozatok)
            {
                DataRow row = data.NewRow();
                row[0] = current.azonosító;
                row[1] = current.megnevezés;
                row[2] = current.versenyek;
               

                data.Rows.Add(row);
            }

            return data;
        }

        #region Accessors
        private delegate void Versenysorozat_Hozzáadás_Callback(Versenysorozat _versenysorozat);
        public void VersenySorozatHozzaadas(Versenysorozat _versenysorozat)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Hozzáadás_Callback callback = new Versenysorozat_Hozzáadás_Callback( VersenySorozatHozzaadas );
                Invoke(callback, new object[] { _versenysorozat });
            }
            else
            {
                if (!Program.database.ÚjVersenysorozat(_versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                DataRow row = data.NewRow();
                row[0] = _versenysorozat.azonosító;
                row[1] = _versenysorozat.megnevezés;
                row[2] = _versenysorozat.versenyek;

                data.Rows.Add(row);

                if (versenysorozat_hozzáadva != null) versenysorozat_hozzáadva(_versenysorozat);
            }
        }

        private delegate void Versenysorozat_Módosítás_Callback(string _azonosító, Versenysorozat _versenysorozat);
        public void VersenySorozatModositas(string _azonosító, Versenysorozat _versenysorozat)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Módosítás_Callback callback = new Versenysorozat_Módosítás_Callback( VersenySorozatModositas );
                Invoke(callback, new object[] { _azonosító, _versenysorozat });
            }
            else
            {
                if (!Program.database.VersenysorozatMódosítás(_azonosító, _versenysorozat)) { MessageBox.Show("Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        current[0] = _versenysorozat.azonosító;
                        current[1] = _versenysorozat.megnevezés;
                        current[2] = _versenysorozat.versenyek;
                        break;
                    }
                }

                if (versenysorozat_módosítva != null) versenysorozat_módosítva(_azonosító, _versenysorozat);
            }
        }

        private delegate void Versenysorozat_Törlés_Callback(string _azonosító);
        public void VersenySorozatTorles(string _azonosító)
        {
            if (InvokeRequired)
            {
                Versenysorozat_Törlés_Callback callback = new Versenysorozat_Törlés_Callback( VersenySorozatTorles );
                Invoke(callback, new object[] { _azonosító });
            }
            else
            {
                if (!Program.database.VersenysorozatTörlés(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                foreach (DataRow current in data.Rows)
                {
                    if (_azonosító == current[0].ToString())
                    {
                        data.Rows.Remove(current);
                        break;
                    }
                }

                if (versenysorozat_törölve != null) versenysorozat_törölve(_azonosító);
            }
        }

        ///

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyNövelés(string _azonosító)
        {
            if (!Program.database.Versenysorozat_VersenyekNövel(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[2] = ((int)current[2]) + 1;
                    break;
                }
            }
        }

        //Nincs invokeolva, csak belső hívásra!!
        public void Versenysorozat_VersenyCsökkentés(string _azonosító)
        {
            if (!Program.database.Versenysorozat_VersenyekCsökkent(_azonosító)) { MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            foreach (DataRow current in data.Rows)
            {
                if (_azonosító == current[0].ToString())
                {
                    current[2] = ((int)current[2]) - 1;
                    break;
                }
            }
        }
        #endregion

        #region EventHandlers
        private void table_DataBindingComplete(object _sender, EventArgs _event)
        {
            table.DataBindingComplete -= table_DataBindingComplete;

            table.Columns[0].Width = 80;
            table.Columns[1].Width = 200;
            table.Columns[2].Width = 120;

            foreach (DataGridViewColumn column in table.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void btnHozzaadas_Click(object _sender, EventArgs _event)
        {
            Form_Versenysorozat versenysorozat = new Form_Versenysorozat();
            versenysorozat.ShowDialog();
        }

        private void Modositas_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;

            Form_Versenysorozat versenysorozat = new Form_Versenysorozat(new Versenysorozat(data.Rows[table.SelectedRows[0].Index][0].ToString(),
                data.Rows[table.SelectedRows[0].Index][1].ToString(), Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][2])));
            versenysorozat.ShowDialog();
        }

        private void btnTorles_Click(object _sender, EventArgs _event)
        {
            if ((table.SelectedRows.Count == 0) || (table.SelectedRows[0].Index == data.Rows.Count)) return;
            if ((int)data.Rows[table.SelectedRows[0].Index][2] != 0) { MessageBox.Show("Ez a versenysorozat nem törölhető, mivel van hozzárendelve verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (MessageBox.Show("Biztosan törli ezt a versenysorozatot?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            VersenySorozatTorles(data.Rows[table.SelectedRows[0].Index][0].ToString());
        }
        #endregion

        public sealed class Form_Versenysorozat : Form
        {
            private string eredeti_azonosító = null;

            private TextBox txtAzonosito;
            private TextBox txtMegnevezes;
            private Label lblSzam;

            public Form_Versenysorozat(Versenysorozat _versenysorozat)
            {
                eredeti_azonosító = _versenysorozat.azonosító;

                InitializeForm( _versenysorozat );
                InitializeContent( _versenysorozat );
                InitializeData(_versenysorozat);
            }

            public Form_Versenysorozat(   )
            {
                InitializeForm(  );
                InitializeContent(  );
            }

            #region Hozzáadás
            
            private void InitializeForm()
            {
                Text = "Versenysorozat";
                ClientSize = new System.Drawing.Size(400 - 64, 128);
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent()
            {
                Label azonosító = new Label();
                azonosító.Text = "Azonosító:";
                azonosító.Location = new System.Drawing.Point(16, 16 + 0 * 32);
                //azonosító.Font = new System.Drawing.Font("Arial Black", 10);

                Label megnevezés = new Label();
                megnevezés.Text = "Megnevezés:";
                megnevezés.Location = new System.Drawing.Point(azonosító.Location.X, 16 + 1 * 32);
                //megnevezés.Font = new System.Drawing.Font("Arial Black", 10);


                txtAzonosito = new iTextBox( new System.Drawing.Point( azonosító.Location.X + azonosító.Size.Width + 16, azonosító.Location.Y ),
                                            10,
                                            new System.Drawing.Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtMegnevezes = new iTextBox(new System.Drawing.Point(megnevezés.Location.X + megnevezés.Size.Width + 16, megnevezés.Location.Y),
                                            30,
                                            new System.Drawing.Size( 128 + 64, 24 ),
                                            null,
                                            this);


                Button btnRendben = new iButton( "Rendben",
                                                new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new System.Drawing.Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

                Controls.Add(azonosító);
                Controls.Add(megnevezés);
            }
            #endregion

            #region Módosítás
            
            private void InitializeData(Versenysorozat _versenysorozat)
            {
                txtAzonosito.Text = _versenysorozat.azonosító;
                txtAzonosito.Enabled = ( ( _versenysorozat.versenyek == 0 ) ? true : false );
                txtMegnevezes.Text = _versenysorozat.megnevezés;
                lblSzam.Text = _versenysorozat.versenyek.ToString();
            }

            private void InitializeForm(Versenysorozat _versenysorozat )
            {
                Text = "Versenysorozat";
                ClientSize = new System.Drawing.Size( 400 - 64, 128 + 16 );
                MinimumSize = ClientSize;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            }

            private void InitializeContent( Versenysorozat _versenysorozat )
            {
                Label azonosító = new Label( );
                azonosító.Text = "Azonosító:";
                azonosító.Location = new System.Drawing.Point( 16, 16 + 0 * 32 );
                //azonosító.Font = new System.Drawing.Font("Arial Black", 10);

                Label megnevezés = new Label( );
                megnevezés.Text = "Megnevezés:";
                megnevezés.Location = new System.Drawing.Point( azonosító.Location.X, 16 + 1 * 32 );
                //megnevezés.Font = new System.Drawing.Font("Arial Black", 10);

                Label szám = new Label( );
                szám.Text = "Versenyek száma:";
                szám.Location = new System.Drawing.Point( azonosító.Location.X, 16 + 2 * 32 );
                //szám.Font = new System.Drawing.Font("Arial Black", 10);

                txtAzonosito = new iTextBox( new System.Drawing.Point( azonosító.Location.X + azonosító.Size.Width + 16, azonosító.Location.Y ),
                                            10,
                                            new System.Drawing.Size( 128 + 64, 24 ),
                                            null,
                                            this );

                txtMegnevezes = new iTextBox( new System.Drawing.Point( megnevezés.Location.X + megnevezés.Size.Width + 16, megnevezés.Location.Y ),
                                            30,
                                            new System.Drawing.Size( 128 + 64, 24 ),
                                            null,
                                            this );

                lblSzam = new iLabel( null,
                                      new System.Drawing.Point( szám.Location.X + szám.Size.Width + 16, szám.Location.Y ),
                                      this );

                lblSzam.Size = new System.Drawing.Size( 128 + 64, 24 );

                Button btnRendben = new iButton( "Rendben",
                                                new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new System.Drawing.Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

                Controls.Add( azonosító );
                Controls.Add( megnevezés );
                Controls.Add( szám );
            }
            #endregion

            private void 
            btnRendben_Click(object _sender, EventArgs _event)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 ]");
                txtAzonosito.Text = rgx.Replace(txtAzonosito.Text, "");

                if (!(0 < txtAzonosito.Text.Length && txtAzonosito.Text.Length <= 10)) { MessageBox.Show("Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!Database.IsCorrectSQLText(txtAzonosito.Text)) { MessageBox.Show("Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if ( !( 0 < txtMegnevezes.Text.Length && txtMegnevezes.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő a megnevezés hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if ( !Database.IsCorrectSQLText( txtMegnevezes.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

                if (eredeti_azonosító != null)
                    Program.mainform.versenysorozat_panel.VersenySorozatModositas( eredeti_azonosító, new Versenysorozat( txtAzonosito.Text, txtMegnevezes.Text, Convert.ToInt32( lblSzam.Text ) ) );
                else
                    Program.mainform.versenysorozat_panel.VersenySorozatHozzaadas( new Versenysorozat( txtAzonosito.Text, txtMegnevezes.Text, 0 ) );

                Close();
            }
        }
    }
}
