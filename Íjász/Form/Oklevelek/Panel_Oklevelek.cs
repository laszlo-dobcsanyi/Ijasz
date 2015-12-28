#define DEBUG
#undef DEBUG
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász {
    //asdadsdsadsaad
    public struct Oklevel {
        public string Azonosito { get; set; }
        public string Tipus { get; set; }

        public int NevX { get; set; }
        public int NevY { get; set; }
        public int NevH { get; set; }

        public int HelyezesX { get; set; }
        public int HelyezesY { get; set; }
        public int HelyezesH { get; set; }

        public int KategoriaX { get; set; }
        public int KategoriaY { get; set; }
        public int KategoriaH { get; set; }

        public int HelyszinX { get; set; }
        public int HelyszinY { get; set; }
        public int HelyszinH { get; set; }

        public int DatumX { get; set; }
        public int DatumY { get; set; }
        public int DatumH { get; set; }

        public int EgyesuletX { get; set; }
        public int EgyesuletY { get; set; }
        public int EgyesuletH { get; set; }

        public Oklevel( string _Azonosito, string _Tipus,
           int _NevX, int _NevY, int _NevH,
           int _HelyezesX, int _HelyezesY, int _HelyezesH,
           int _KategoriaX, int _KategoriaY, int _KategoriaH,
           int _HelyszinX, int _HelyszinY, int _HelyszinH,
           int _DatumX, int _DatumY, int _DatumH,
           int _EgyesuletX, int _EgyesuletY, int _EgyesuletH
           ) {
            Azonosito = _Azonosito;
            Tipus = _Tipus;
            NevX = _NevX;
            NevY = _NevY;
            NevH = _NevH;
            HelyezesX = _HelyezesX;
            HelyezesY = _HelyezesY;
            HelyezesH = _HelyezesH;
            KategoriaX = _KategoriaX;
            KategoriaY = _KategoriaY;
            KategoriaH = _KategoriaH;
            HelyszinX = _HelyszinX;
            HelyszinY = _HelyszinY;
            HelyszinH = _HelyszinH;
            DatumX = _DatumX;
            DatumY = _DatumY;
            DatumH = _DatumH;
            EgyesuletX = _EgyesuletX;
            EgyesuletY = _EgyesuletY;
            EgyesuletH = _EgyesuletH;
        }


    }

    public delegate void Sablon_Hozzáadva( Oklevel _oklevel );
    public delegate void Sablon_Modositva( string _azonosito, Oklevel _oklevel );
    public delegate void Sablon_Törölve( string _azonosito );

    public partial class Panel_Oklevelek : Control {
        public Sablon_Hozzáadva sablon_hozzáadva;
        public Sablon_Modositva sablon_modositva;
        public Sablon_Törölve sablon_törölve;

        CheckBox chkVerseny;
        CheckBox chkVersenysorozat;
        ComboBox cboVerseny;
        ComboBox cboVersenysorozat;
        ComboBox cboHelyezes;

        private DataTable data;
        private DataGridView table;

        public Panel_Oklevelek( ) {
            InitializeContent( );
        }

        public void
        InitializeContent( ) {


#if DEBUG
            List<Oklevel> sablon = (Program.database.Oklevelek().Where(q => q.Tipus == "Verseny").Take(1)).ToList();
            var versenyzo = new Nyomtat.OKLEVELVERSENYZO("Belinyak Mate", 1, "egyesulet" ,"2012.03.33");
            Nyomtat.NyomtatOklevelVersenyVersenyzo(sablon[0], versenyzo);
            System.Environment.Exit(1);
#endif
            table = new DataGridView {
                Dock = DockStyle.Left,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                Width = 680,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                DataSource = CreateSource( ),
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            };
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;
            Controls.Add( table );

            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            Label txtOklevelTipus = new Label {
                Text = "Oklevél típusa:",
                Location = new Point(cWidth - 96 - 15 * 16, cHeight - 32 - 41 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtHelyezes = new Label {
                Text = "Hány ember:",
                Location = new Point(cWidth - 96 - 15 * 16, cHeight - 32 - 39 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtVerseny = new Label {
                Text = "Verseny:",
                Location = new Point(cWidth - 96 - 15 * 16, cHeight - 32 - 37 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtVersenysorozat = new Label {
                Text = "Versenysorozat:",
                Location = new Point(cWidth - 96 - 15 * 16, cHeight - 32 - 35 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            chkVerseny = new CheckBox {
                Text = "Verseny",
                Location = new Point( cWidth - 96 - 12 * 16, cHeight - 32 - 41 * 16 - 8 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVerseny.Click += ChkVerseny_Click;

            chkVersenysorozat = new CheckBox {
                Text = "Versenysorozat",
                Location = new Point( cWidth - 96 - 5 * 16, cHeight - 32 - 41 * 16 - 8 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenysorozat.Click += ChkVerseny_Click;

            cboVerseny = new ComboBox {
                Location = new Point( cWidth - 96 - 8 * 16 + 8, cHeight - 32 - 37 * 16 ),
                Size = new Size( 128, 24 ),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            var Versenyek = Program.database.Versenyek().Select(verseny => verseny.Azonosito);
            foreach( var item in Versenyek ) {
                cboVerseny.Items.Add( item );
            }
            if( cboVerseny.Items.Count != 0 ) { cboVerseny.SelectedIndex = 0; }

            cboVersenysorozat = new ComboBox {
                Location = new Point( cWidth - 96 - 8 * 16 + 8, cHeight - 32 - 35 * 16 ),
                Size = new Size( 128, 24 ),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            var Versenysorozatok = Program.database.Versenysorozatok().Select(vs => vs.azonosító);
            foreach( var item in Versenysorozatok ) {
                cboVersenysorozat.Items.Add( item );
            }
            if( cboVersenysorozat.Items.Count != 0 ) { cboVersenysorozat.SelectedIndex = 0; }

            cboHelyezes = new ComboBox {
                Location = new Point( cWidth - 96 - 8 * 16 + 8, txtHelyezes.Location.Y ),
                Size = new Size( 128, 24 ),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Width = 8 * 8,
            };
            for( int i = 1; i < 10; i++ ) {
                cboHelyezes.Items.Add( i );
            }
            cboHelyezes.SelectedIndex = 0;

            Button btnNyomtat = new Button {
                Text = "Nyomtat",
                Location = new Point(cWidth - 3 * 96 - 40, cHeight - 32 - 16),
                Size = new Size(96, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            btnNyomtat.Click += BtnNyomtat_Click;


            Button btnUjSablon = new Button {
                Text = "Új sablon",
                Location = new Point(cWidth - 2 * 96 - 40, cHeight - 32 - 16),
                Size = new Size(96, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            btnUjSablon.Click += BtnUjSablon_Click;

            Button btnTorlesSablon = new Button {
                Text = "Sablon törlése",
                Location = new Point(cWidth - 1 * 96 - 40, cHeight - 32 - 16),
                Size = new Size(96, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            btnTorlesSablon.Click += BtnTorlesSablon_Click;

            Controls.Add( txtOklevelTipus );
            Controls.Add( txtHelyezes );
            Controls.Add( txtVerseny );
            Controls.Add( txtVersenysorozat );
            Controls.Add( chkVerseny );
            Controls.Add( chkVersenysorozat );
            Controls.Add( cboVerseny );
            Controls.Add( cboVersenysorozat );
            Controls.Add( cboHelyezes );
            Controls.Add( btnNyomtat );
            Controls.Add( btnUjSablon );
            Controls.Add( btnTorlesSablon );
        }

        private DataTable CreateSource( ) {
            data = new DataTable( );

            data.Columns.Add( new DataColumn( "Azonosító", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Típus", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "NévX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "NévY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "NévH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "HelyezesX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "KategóriaX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KategóriaY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KategóriaH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "HelyszínX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyszínY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyszínH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "DátumX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "EgyesületX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületH", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Helyezes", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Kategória", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Helyszín", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Dátum", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );

            var Oklevelek = Program.database.Oklevelek();
            foreach( var oklevel in Oklevelek ) {
                DataRow row = data.NewRow();
                row[0] = oklevel.Azonosito;
                row[1] = oklevel.Tipus;

                row[2] = oklevel.NevX != -66 ? oklevel.NevX.ToString( ) : " ";
                row[3] = oklevel.NevY != -66 ? oklevel.NevY.ToString( ) : " ";
                row[4] = oklevel.NevH != -66 ? oklevel.NevH.ToString( ) : " ";

                row[5] = oklevel.HelyezesX != -66 ? oklevel.HelyezesX.ToString( ) : " ";
                row[6] = oklevel.HelyezesY != -66 ? oklevel.HelyezesY.ToString( ) : " ";
                row[7] = oklevel.HelyezesH != -66 ? oklevel.HelyezesH.ToString( ) : " ";

                row[8] = oklevel.KategoriaX != -66 ? oklevel.KategoriaX.ToString( ) : " ";
                row[9] = oklevel.KategoriaY != -66 ? oklevel.KategoriaY.ToString( ) : " ";
                row[10] = oklevel.KategoriaH != -66 ? oklevel.KategoriaH.ToString( ) : " ";

                row[11] = oklevel.HelyszinX != -66 ? oklevel.HelyszinX.ToString( ) : " ";
                row[12] = oklevel.HelyszinY != -66 ? oklevel.HelyszinY.ToString( ) : " ";
                row[13] = oklevel.HelyszinH != -66 ? oklevel.HelyszinH.ToString( ) : " ";

                row[14] = oklevel.DatumX != -66 ? oklevel.DatumX.ToString( ) : " ";
                row[15] = oklevel.DatumY != -66 ? oklevel.DatumY.ToString( ) : " ";
                row[16] = oklevel.DatumH != -66 ? oklevel.DatumH.ToString( ) : " ";

                row[17] = oklevel.EgyesuletX != -66 ? oklevel.EgyesuletX.ToString( ) : " ";
                row[18] = oklevel.EgyesuletY != -66 ? oklevel.EgyesuletY.ToString( ) : " ";
                row[19] = oklevel.EgyesuletH != -66 ? oklevel.EgyesuletH.ToString( ) : " ";

                //-------------------------------------------------
                row[20] = oklevel.NevX != -66 ? oklevel.NevX.ToString( ) : " ";
                row[20] += Environment.NewLine;
                row[20] += oklevel.NevY != -66 ? oklevel.NevY.ToString( ) : " ";
                row[20] += Environment.NewLine;
                row[20] += oklevel.NevH != -66 ? oklevel.NevH.ToString( ) : " ";

                row[21] = oklevel.HelyezesX != -66 ? oklevel.HelyezesX.ToString( ) : " ";
                row[21] += Environment.NewLine;
                row[21] += oklevel.HelyezesY != -66 ? oklevel.HelyezesY.ToString( ) : " ";
                row[21] += Environment.NewLine;
                row[21] += oklevel.HelyezesH != -66 ? oklevel.HelyezesH.ToString( ) : " ";

                row[22] = oklevel.KategoriaX != -66 ? oklevel.KategoriaX.ToString( ) : " ";
                row[22] += Environment.NewLine;
                row[22] += oklevel.KategoriaY != -66 ? oklevel.KategoriaY.ToString( ) : " ";
                row[22] += Environment.NewLine;
                row[22] += oklevel.KategoriaH != -66 ? oklevel.KategoriaH.ToString( ) : " ";

                row[23] = oklevel.HelyszinX != -66 ? oklevel.HelyszinX.ToString( ) : " ";
                row[23] += Environment.NewLine;
                row[23] += oklevel.HelyszinY != -66 ? oklevel.HelyszinY.ToString( ) : " ";
                row[23] += Environment.NewLine;
                row[23] += oklevel.HelyszinH != -66 ? oklevel.HelyszinH.ToString( ) : " ";

                row[24] = oklevel.DatumX != -66 ? oklevel.DatumX.ToString( ) : " ";
                row[24] += Environment.NewLine;
                row[24] += oklevel.DatumY != -66 ? oklevel.DatumY.ToString( ) : " ";
                row[24] += Environment.NewLine;
                row[24] += oklevel.DatumH != -66 ? oklevel.DatumH.ToString( ) : " ";

                row[25] = oklevel.EgyesuletX != -66 ? oklevel.EgyesuletX.ToString( ) : " ";
                row[25] += Environment.NewLine;
                row[25] += oklevel.EgyesuletY != -66 ? oklevel.EgyesuletY.ToString( ) : " ";
                row[25] += Environment.NewLine;
                row[25] += oklevel.EgyesuletH != -66 ? oklevel.EgyesuletH.ToString( ) : " ";

                data.Rows.Add( row );
            }
            return data;
        }

        public void verseny_hozzáadás( Verseny _verseny ) {
            cboVerseny.Items.Add( _verseny.Azonosito );
        }

        public void verseny_módosítás( string _azonosító, Verseny _verseny ) {
            if( _azonosító != _verseny.Azonosito ) {
                for( int current = 0; current < cboVerseny.Items.Count; ++current ) {
                    if( _azonosító == cboVerseny.Items[current].ToString( ) ) {
                        cboVerseny.Items[current] = _verseny.Azonosito;
                        break;
                    }
                }
            }
        }

        public void verseny_törlés( string _azonosító ) {
            if( cboVerseny.SelectedItem != null && _azonosító == cboVerseny.SelectedItem.ToString( ) ) {
                data.Rows.Clear( );
            }
            cboVerseny.Items.Remove( _azonosító );
        }

        private void table_DataBindingComplete( object sender, DataGridViewBindingCompleteEventArgs e ) {
            table.DataBindingComplete -= table_DataBindingComplete;
            foreach( DataGridViewColumn column in table.Columns ) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            table.Columns[20].Width = 79;
            table.Columns[21].Width = 79;
            table.Columns[22].Width = 79;
            table.Columns[23].Width = 80;
            table.Columns[24].Width = 80;
            table.Columns[25].Width = 80;

            table.Columns[2].Visible = false;
            table.Columns[3].Visible = false;
            table.Columns[4].Visible = false;
            table.Columns[5].Visible = false;
            table.Columns[6].Visible = false;
            table.Columns[7].Visible = false;
            table.Columns[8].Visible = false;
            table.Columns[9].Visible = false;
            table.Columns[10].Visible = false;
            table.Columns[11].Visible = false;
            table.Columns[12].Visible = false;
            table.Columns[13].Visible = false;
            table.Columns[14].Visible = false;
            table.Columns[15].Visible = false;
            table.Columns[16].Visible = false;
            table.Columns[17].Visible = false;
            table.Columns[18].Visible = false;
            table.Columns[19].Visible = false;
        }

        #region Accessors
        private delegate void Sablon_Hozzáadás_Callback( Oklevel _oklevel );
        public void Sablon_Hozzáadás( Oklevel _oklevel ) {
            if( InvokeRequired ) {
                Sablon_Hozzáadás_Callback callback = new Sablon_Hozzáadás_Callback(Sablon_Hozzáadás);
                Invoke( callback, new object[] { _oklevel } );
            }
            else {
                if( !Program.database.UjOklevel( _oklevel ) ) {
                    MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                DataRow row = data.NewRow();

                row[0] = _oklevel.Azonosito;
                row[1] = _oklevel.Tipus;

                row[2] = _oklevel.NevX != -66 ? _oklevel.NevX.ToString( ) : " ";
                row[3] = _oklevel.NevY != -66 ? _oklevel.NevY.ToString( ) : " ";
                row[4] = _oklevel.NevH != -66 ? _oklevel.NevH.ToString( ) : " ";

                row[5] = _oklevel.HelyezesX != -66 ? _oklevel.HelyezesX.ToString( ) : " ";
                row[6] = _oklevel.HelyezesY != -66 ? _oklevel.HelyezesY.ToString( ) : " ";
                row[7] = _oklevel.HelyezesH != -66 ? _oklevel.HelyezesH.ToString( ) : " ";

                row[8] = _oklevel.KategoriaX != -66 ? _oklevel.KategoriaX.ToString( ) : " ";
                row[9] = _oklevel.KategoriaY != -66 ? _oklevel.KategoriaY.ToString( ) : " ";
                row[10] = _oklevel.KategoriaH != -66 ? _oklevel.KategoriaH.ToString( ) : " ";

                row[11] = _oklevel.HelyszinX != -66 ? _oklevel.HelyszinX.ToString( ) : " ";
                row[12] = _oklevel.HelyszinY != -66 ? _oklevel.HelyszinY.ToString( ) : " ";
                row[13] = _oklevel.HelyszinH != -66 ? _oklevel.HelyszinH.ToString( ) : " ";

                row[14] = _oklevel.DatumX != -66 ? _oklevel.DatumX.ToString( ) : " ";
                row[15] = _oklevel.DatumY != -66 ? _oklevel.DatumY.ToString( ) : " ";
                row[16] = _oklevel.DatumH != -66 ? _oklevel.DatumH.ToString( ) : " ";

                row[17] = _oklevel.EgyesuletX != -66 ? _oklevel.EgyesuletX.ToString( ) : " ";
                row[18] = _oklevel.EgyesuletY != -66 ? _oklevel.EgyesuletY.ToString( ) : " ";
                row[19] = _oklevel.EgyesuletH != -66 ? _oklevel.EgyesuletH.ToString( ) : " ";

                //--
                row[20] = _oklevel.NevX != -66 ? _oklevel.NevX.ToString( ) : " ";
                row[20] += Environment.NewLine;
                row[20] += _oklevel.NevY != -66 ? _oklevel.NevY.ToString( ) : " ";
                row[20] += Environment.NewLine;
                row[20] += _oklevel.NevH != -66 ? _oklevel.NevH.ToString( ) : " ";

                row[21] = _oklevel.HelyezesX != -66 ? _oklevel.HelyezesX.ToString( ) : " ";
                row[21] += Environment.NewLine;
                row[21] += _oklevel.HelyezesY != -66 ? _oklevel.HelyezesY.ToString( ) : " ";
                row[21] += Environment.NewLine;
                row[21] += _oklevel.HelyezesH != -66 ? _oklevel.HelyezesH.ToString( ) : " ";

                row[22] = _oklevel.KategoriaX != -66 ? _oklevel.KategoriaX.ToString( ) : " ";
                row[22] += Environment.NewLine;
                row[22] += _oklevel.KategoriaY != -66 ? _oklevel.KategoriaY.ToString( ) : " ";
                row[22] += Environment.NewLine;
                row[22] += _oklevel.KategoriaH != -66 ? _oklevel.KategoriaH.ToString( ) : " ";

                row[23] = _oklevel.HelyszinX != -66 ? _oklevel.HelyszinX.ToString( ) : " ";
                row[23] += Environment.NewLine;
                row[23] += _oklevel.HelyszinY != -66 ? _oklevel.HelyszinY.ToString( ) : " ";
                row[23] += Environment.NewLine;
                row[23] += _oklevel.HelyszinH != -66 ? _oklevel.HelyszinH.ToString( ) : " ";

                row[24] = _oklevel.DatumX != -66 ? _oklevel.DatumX.ToString( ) : " ";
                row[24] += Environment.NewLine;
                row[24] += _oklevel.DatumY != -66 ? _oklevel.DatumY.ToString( ) : " ";
                row[24] += Environment.NewLine;
                row[24] += _oklevel.DatumH != -66 ? _oklevel.DatumH.ToString( ) : " ";

                row[25] = _oklevel.EgyesuletX != -66 ? _oklevel.EgyesuletX.ToString( ) : " ";
                row[25] += Environment.NewLine;
                row[25] += _oklevel.EgyesuletY != -66 ? _oklevel.EgyesuletY.ToString( ) : " ";
                row[25] += Environment.NewLine;
                row[25] += _oklevel.EgyesuletH != -66 ? _oklevel.EgyesuletH.ToString( ) : " ";


                data.Rows.Add( row );
                if( sablon_hozzáadva != null )
                    sablon_hozzáadva( _oklevel );
            }
        }

        private delegate void Sablon_Módosítás_Callback( string _azonosito, Oklevel _oklevel );
        public void Sablon_Modositas( string _azonosito, Oklevel _oklevel ) {
            if( InvokeRequired ) {
                Sablon_Módosítás_Callback callback = new Sablon_Módosítás_Callback(Sablon_Modositas);
                Invoke( callback, new object[] { _azonosito, _oklevel } );
            }
            else {
                if( !( Program.database.OklevelModositas( _azonosito, _oklevel ) ) ) {
                    MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }
            foreach( DataRow current in data.Rows ) {
                if( _azonosito == current[0].ToString( ) ) {
                    current[0] = _oklevel.Azonosito;
                    current[1] = _oklevel.Tipus;

                    current[2] = _oklevel.NevX != -66 ? _oklevel.NevX.ToString( ) : " ";
                    current[3] = _oklevel.NevY != -66 ? _oklevel.NevY.ToString( ) : " ";
                    current[4] = _oklevel.NevH != -66 ? _oklevel.NevH.ToString( ) : " ";

                    current[5] = _oklevel.HelyezesX != -66 ? _oklevel.HelyezesX.ToString( ) : " ";
                    current[6] = _oklevel.HelyezesY != -66 ? _oklevel.HelyezesY.ToString( ) : " ";
                    current[7] = _oklevel.HelyezesH != -66 ? _oklevel.HelyezesH.ToString( ) : " ";

                    current[8] = _oklevel.KategoriaX != -66 ? _oklevel.KategoriaX.ToString( ) : " ";
                    current[9] = _oklevel.KategoriaY != -66 ? _oklevel.KategoriaY.ToString( ) : " ";
                    current[10] = _oklevel.KategoriaH != -66 ? _oklevel.KategoriaH.ToString( ) : " ";

                    current[11] = _oklevel.HelyszinX != -66 ? _oklevel.HelyszinX.ToString( ) : " ";
                    current[12] = _oklevel.HelyszinY != -66 ? _oklevel.HelyszinY.ToString( ) : " ";
                    current[13] = _oklevel.HelyszinH != -66 ? _oklevel.HelyszinH.ToString( ) : " ";

                    current[14] = _oklevel.DatumX != -66 ? _oklevel.DatumX.ToString( ) : " ";
                    current[15] = _oklevel.DatumY != -66 ? _oklevel.DatumY.ToString( ) : " ";
                    current[16] = _oklevel.DatumH != -66 ? _oklevel.DatumH.ToString( ) : " ";

                    current[17] = _oklevel.EgyesuletX != -66 ? _oklevel.EgyesuletX.ToString( ) : " ";
                    current[18] = _oklevel.EgyesuletY != -66 ? _oklevel.EgyesuletY.ToString( ) : " ";
                    current[19] = _oklevel.EgyesuletH != -66 ? _oklevel.EgyesuletH.ToString( ) : " ";

                    //--
                    current[20] = _oklevel.NevX != -66 ? _oklevel.NevX.ToString( ) : " ";
                    current[20] += Environment.NewLine;
                    current[20] += _oklevel.NevY != -66 ? _oklevel.NevY.ToString( ) : " ";
                    current[20] += Environment.NewLine;
                    current[20] += _oklevel.NevH != -66 ? _oklevel.NevH.ToString( ) : " ";

                    current[21] = _oklevel.HelyezesX != -66 ? _oklevel.HelyezesX.ToString( ) : " ";
                    current[21] += Environment.NewLine;
                    current[21] += _oklevel.HelyezesY != -66 ? _oklevel.HelyezesY.ToString( ) : " ";
                    current[21] += Environment.NewLine;
                    current[21] += _oklevel.HelyezesH != -66 ? _oklevel.HelyezesH.ToString( ) : " ";

                    current[22] = _oklevel.KategoriaX != -66 ? _oklevel.KategoriaX.ToString( ) : " ";
                    current[22] += Environment.NewLine;
                    current[22] += _oklevel.KategoriaY != -66 ? _oklevel.KategoriaY.ToString( ) : " ";
                    current[22] += Environment.NewLine;
                    current[22] += _oklevel.KategoriaH != -66 ? _oklevel.KategoriaH.ToString( ) : " ";

                    current[23] = _oklevel.HelyszinX != -66 ? _oklevel.HelyszinX.ToString( ) : " ";
                    current[23] += Environment.NewLine;
                    current[23] += _oklevel.HelyszinY != -66 ? _oklevel.HelyszinY.ToString( ) : " ";
                    current[23] += Environment.NewLine;
                    current[23] += _oklevel.HelyszinH != -66 ? _oklevel.HelyszinH.ToString( ) : " ";

                    current[24] = _oklevel.DatumX != -66 ? _oklevel.DatumX.ToString( ) : " ";
                    current[24] += Environment.NewLine;
                    current[24] += _oklevel.DatumY != -66 ? _oklevel.DatumY.ToString( ) : " ";
                    current[24] += Environment.NewLine;
                    current[24] += _oklevel.DatumH != -66 ? _oklevel.DatumH.ToString( ) : " ";

                    current[25] = _oklevel.EgyesuletX != -66 ? _oklevel.EgyesuletX.ToString( ) : " ";
                    current[25] += Environment.NewLine;
                    current[25] += _oklevel.EgyesuletY != -66 ? _oklevel.EgyesuletY.ToString( ) : " ";
                    current[25] += Environment.NewLine;
                    current[25] += _oklevel.EgyesuletH != -66 ? _oklevel.EgyesuletH.ToString( ) : " ";
                }
            }

            if( sablon_modositva != null ) {
                sablon_modositva( _azonosito, _oklevel );
            }
        }

        private delegate void Sablon_Törlés_Callback( string _azonosító );
        public void Sablon_Törlés( string _azonosító ) {
            if( InvokeRequired ) {
                Sablon_Törlés_Callback callback = new Sablon_Törlés_Callback(Sablon_Törlés);
                Invoke( callback, new object[] { _azonosító } );
            }
            else {
                if( !Program.database.OklevelTorles( _azonosító ) ) {
                    MessageBox.Show( "Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                foreach( DataRow current in data.Rows ) {
                    if( _azonosító == current[0].ToString( ) ) {
                        data.Rows.Remove( current );
                        break;
                    }

                }

                if( sablon_törölve != null )
                    sablon_törölve( _azonosító );
            }
        }
        #endregion

        #region EventHandlers
        private void ChkVerseny_Click( object sender, EventArgs _e ) {
            CheckBox aktív = sender as CheckBox;
            if( aktív == null ) { return; }

            if( aktív == chkVerseny ) {
                chkVerseny.Checked = true;
                chkVersenysorozat.Checked = false;
                cboVerseny.Enabled = true;
                cboVersenysorozat.Enabled = false;
            }

            if( aktív == chkVersenysorozat ) {
                chkVersenysorozat.Checked = true;
                chkVerseny.Checked = false;
                cboVerseny.Enabled = false;
                cboVersenysorozat.Enabled = true;
            }
        }

        private void BtnUjSablon_Click( object sender, EventArgs e ) {
            Form_Oklevel OklevelForm = new Form_Oklevel();
            OklevelForm.Show( );
        }

        private void Modositas_Click( object sender, DataGridViewCellEventArgs e ) {
            if( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[0].Index == data.Rows.Count ) ) {
                return;
            }
            Oklevel oklevel = new Oklevel {
                Azonosito = data.Rows[table.SelectedRows[0].Index][0].ToString(),
                Tipus = data.Rows[table.SelectedRows[0].Index][1].ToString(),

                NevX = data.Rows[table.SelectedRows[0].Index][2].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][2]),
                NevY = data.Rows[table.SelectedRows[0].Index][3].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][3]),
                NevH = data.Rows[table.SelectedRows[0].Index][4].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][4]),

                HelyezesX = data.Rows[table.SelectedRows[0].Index][5].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][5]),
                HelyezesY = data.Rows[table.SelectedRows[0].Index][6].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][6]),
                HelyezesH = data.Rows[table.SelectedRows[0].Index][7].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][7]),

                KategoriaX = data.Rows[table.SelectedRows[0].Index][8].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][8]),
                KategoriaY = data.Rows[table.SelectedRows[0].Index][9].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][9]),
                KategoriaH = data.Rows[table.SelectedRows[0].Index][10].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][10]),

                HelyszinX = data.Rows[table.SelectedRows[0].Index][11].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][11]),
                HelyszinY = data.Rows[table.SelectedRows[0].Index][12].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][12]),
                HelyszinH = data.Rows[table.SelectedRows[0].Index][13].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][13]),

                DatumX = data.Rows[table.SelectedRows[0].Index][14].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][14]),
                DatumY = data.Rows[table.SelectedRows[0].Index][15].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][15]),
                DatumH = data.Rows[table.SelectedRows[0].Index][16].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][16]),

                EgyesuletX = data.Rows[table.SelectedRows[0].Index][17].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][17]),
                EgyesuletY = data.Rows[table.SelectedRows[0].Index][18].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][18]),
                EgyesuletH = data.Rows[table.SelectedRows[0].Index][19].ToString() == " " ? -66 : Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][19]),
            };

            Form_Oklevel OklevelForm = new Form_Oklevel(oklevel);
            OklevelForm.ShowDialog( );
        }

        private void BtnTorlesSablon_Click( object sender, EventArgs e ) {
            if( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[0].Index == data.Rows.Count ) )
                return;
            if( MessageBox.Show( "Biztosan törli ezt a sablont?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes )
                return;
            Sablon_Törlés( data.Rows[table.SelectedRows[0].Index][0].ToString( ) );
        }

        public bool isBtnNyomtatEnabled( ) {

            if( table.SelectedRows.Count != 1 ) {
                return false;
            }

            if( ( chkVerseny.CheckState == CheckState.Unchecked ) &&
                ( chkVersenysorozat.CheckState == CheckState.Unchecked ) ) {
                return false;
            }

            return true;
        }

        private void BtnNyomtat_Click( object sender, EventArgs e ) {
            //TODO(mate): dialog box!

            if( ( chkVerseny.CheckState == CheckState.Unchecked ) &&
                ( chkVersenysorozat.CheckState == CheckState.Unchecked ) ) {
                return;
            }

            if( table.SelectedRows.Count != 1 ) {
                return;
            }


            DialogResult dialogResult = MessageBox.Show(@"Biztosan nyomtatni akar?", @"Nyomtatás", MessageBoxButtons.OKCancel);

            if( dialogResult == DialogResult.Cancel ) {
                return;
            }


            Oklevel sablon = Program.database.Oklevel(table.SelectedRows[0].Cells[0].Value.ToString());

            if( chkVerseny.Checked == true ) {
                Nyomtat.NyomtatOklevelVerseny( cboVerseny.Text, sablon, Convert.ToInt32( cboHelyezes.Text ) );
            }
            //TODO(mate): versenysorozatnyomtatás
            else {

            }
        }
        #endregion
    }
}

