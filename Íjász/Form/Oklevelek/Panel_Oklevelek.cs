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

        public int VersenyX { get; set; }
        public int VersenyY { get; set; }
        public int VersenyH { get; set; }
        public string VersenyF { get; set; }
        public string VersenyB { get; set; }
        public int VersenyM { get; set; }
        public string VersenyI { get; set; }

        public int VersenySorozatX { get; set; }
        public int VersenySorozatY { get; set; }
        public int VersenySorozatH { get; set; }
        public string VersenySorozatF { get; set; }
        public string VersenySorozatB { get; set; }
        public int VersenySorozatM { get; set; }
        public string VersenySorozatI { get; set; }

        public int HelyezesX { get; set; }
        public int HelyezesY { get; set; }
        public int HelyezesH { get; set; }
        public string HelyezesF { get; set; }
        public string HelyezesB { get; set; }
        public int HelyezesM { get; set; }
        public string HelyezesI { get; set; }

        public int InduloX { get; set; }
        public int InduloY { get; set; }
        public int InduloH { get; set; }
        public string InduloF { get; set; }
        public string InduloB { get; set; }
        public int InduloM { get; set; }
        public string InduloI { get; set; }

        public int EgyesuletX { get; set; }
        public int EgyesuletY { get; set; }
        public int EgyesuletH { get; set; }
        public string EgyesuletF { get; set; }
        public string EgyesuletB { get; set; }
        public int EgyesuletM { get; set; }
        public string EgyesuletI { get; set; }

        public int IjtipusX { get; set; }
        public int IjtipusY { get; set; }
        public int IjtipusH { get; set; }
        public string IjtipusF { get; set; }
        public string IjtipusB { get; set; }
        public int IjtipusM { get; set; }
        public string IjtipusI { get; set; }

        public int KorosztalyX { get; set; }
        public int KorosztalyY { get; set; }
        public int KorosztalyH { get; set; }
        public string KorosztalyF { get; set; }
        public string KorosztalyB { get; set; }
        public int KorosztalyM { get; set; }
        public string KorosztalyI { get; set; }

        public int InduloNemeX { get; set; }
        public int InduloNemeY { get; set; }
        public int InduloNemeH { get; set; }
        public string InduloNemeF { get; set; }
        public string InduloNemeB { get; set; }
        public int InduloNemeM { get; set; }
        public string InduloNemeI { get; set; }

        public int DatumX { get; set; }
        public int DatumY { get; set; }
        public int DatumH { get; set; }
        public string DatumF { get; set; }
        public string DatumB { get; set; }
        public int DatumM { get; set; }
        public string DatumI { get; set; }

        public Oklevel( string azonosito, string tipus, int versenyX, int versenyY, int versenyH, string versenyF, string versenyB, int versenyM, string versenyI, int versenySorozatX, int versenySorozatY, int versenySorozatH, string versenySorozatF, string versenySorozatB, int versenySorozatM, string versenySorozatI, int helyezesX, int helyezesY, int helyezesH, string helyezesF, string helyezesB, int helyezesM, string helyezesI, int induloX, int induloY, int induloH, string induloF, string induloB, int induloM, string induloI, int egyesuletX, int egyesuletY, int egyesuletH, string egyesuletF, string egyesuletB, int egyesuletM, string egyesuletI, int ijtipusX, int ijtipusY, int ijtipusH, string ijtipusF, string ijtipusB, int ijtipusM, string ijtipusI, int korosztalyX, int korosztalyY, int korosztalyH, string korosztalyF, string korosztalyB, int korosztalyM, string korosztalyI, int induloNemeX, int induloNemeY, int induloNemeH, string induloNemeF, string induloNemeB, int induloNemeM, string induloNemeI, int datumX, int datumY, int datumH, string datumF, string datumB, int datumM, string datumI ) {
            Azonosito = azonosito;
            Tipus = tipus;
            VersenyX = versenyX;
            VersenyY = versenyY;
            VersenyH = versenyH;
            VersenyF = versenyF;
            VersenyB = versenyB;
            VersenyM = versenyM;
            VersenyI = versenyI;
            VersenySorozatX = versenySorozatX;
            VersenySorozatY = versenySorozatY;
            VersenySorozatH = versenySorozatH;
            VersenySorozatF = versenySorozatF;
            VersenySorozatB = versenySorozatB;
            VersenySorozatM = versenySorozatM;
            VersenySorozatI = versenySorozatI;
            HelyezesX = helyezesX;
            HelyezesY = helyezesY;
            HelyezesH = helyezesH;
            HelyezesF = helyezesF;
            HelyezesB = helyezesB;
            HelyezesM = helyezesM;
            HelyezesI = helyezesI;
            InduloX = induloX;
            InduloY = induloY;
            InduloH = induloH;
            InduloF = induloF;
            InduloB = induloB;
            InduloM = induloM;
            InduloI = induloI;
            EgyesuletX = egyesuletX;
            EgyesuletY = egyesuletY;
            EgyesuletH = egyesuletH;
            EgyesuletF = egyesuletF;
            EgyesuletB = egyesuletB;
            EgyesuletM = egyesuletM;
            EgyesuletI = egyesuletI;
            IjtipusX = ijtipusX;
            IjtipusY = ijtipusY;
            IjtipusH = ijtipusH;
            IjtipusF = ijtipusF;
            IjtipusB = ijtipusB;
            IjtipusM = ijtipusM;
            IjtipusI = ijtipusI;
            KorosztalyX = korosztalyX;
            KorosztalyY = korosztalyY;
            KorosztalyH = korosztalyH;
            KorosztalyF = korosztalyF;
            KorosztalyB = korosztalyB;
            KorosztalyM = korosztalyM;
            KorosztalyI = korosztalyI;
            InduloNemeX = induloNemeX;
            InduloNemeY = induloNemeY;
            InduloNemeH = induloNemeH;
            InduloNemeF = induloNemeF;
            InduloNemeB = induloNemeB;
            InduloNemeM = induloNemeM;
            InduloNemeI = induloNemeI;
            DatumX = datumX;
            DatumY = datumY;
            DatumH = datumH;
            DatumF = datumF;
            DatumB = datumB;
            DatumM = datumM;
            DatumI = datumI;
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
                Width = 240,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                DataSource = CreateSource( ),
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True },
            };
            table.DataBindingComplete += table_DataBindingComplete;
            table.CellDoubleClick += Modositas_Click;
            Controls.Add( table );


            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            Label txtOklevelTipus = new Label {
                Text = "Oklevél típusa:", 
                Location = new Point(cWidth - 96 - 35 * 16, cHeight - 32 - 41 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtHelyezes = new Label {
                Text = "Hány ember:",
                Location = new Point(cWidth - 96 - 35 * 16, cHeight - 32 - 39 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtVerseny = new Label {
                Text = "Verseny:",
                Location = new Point(cWidth - 96 - 35 * 16, cHeight - 32 - 37 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label txtVersenysorozat = new Label {
                Text = "Versenysorozat:",
                Location = new Point(cWidth - 96 - 35 * 16, cHeight - 32 - 35 * 16),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            chkVerseny = new CheckBox {
                Text = "Verseny",
                Location = new Point( cWidth - 96 - 32 * 16, cHeight - 32 - 41 * 16 - 8 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVerseny.Click += ChkVerseny_Click;

            chkVersenysorozat = new CheckBox {
                Text = "Versenysorozat",
                Location = new Point( cWidth - 96 - 25 * 16, cHeight - 32 - 41 * 16 - 8 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenysorozat.Click += ChkVerseny_Click;

            cboVerseny = new ComboBox {
                Location = new Point( cWidth - 96 - 28 * 16 + 8, cHeight - 32 - 37 * 16 ),
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
                Location = new Point( cWidth - 96 - 28 * 16 + 8, cHeight - 32 - 35 * 16 ),
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
                Location = new Point( cWidth - 96 - 28 * 16 + 8, txtHelyezes.Location.Y ),
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


        public string formatSeged( string _code ) {
            switch( _code ) {
                case "B":
                    return "FK";
                case "I":
                    return "D";
                case "2":
                    return "FK/D";
                default:
                    return "";
            }
        }
        public string igazitasSeged( string _code ) {
            switch( _code ) {
                case "L":
                    return "<-";
                case "R":
                    return "->";
                case "M":
                    return "-";
                default:
                    return "";
            }
        }

        private DataTable CreateSource( ) {
            data = new DataTable( );

            data.Columns.Add( new DataColumn( "Azonosító", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Típus", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "VersenyX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "VersenySorozatX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "HelyezesX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "IndulóX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "EgyesületX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "ÍjtípusX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "KorosztályX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "IndulóNemeX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "DátumX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumB", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumI", System.Type.GetType( "System.String" ) ) );

            /*
            data.Columns.Add( new DataColumn( "Verseny", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Versenysorozat", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Helyezes", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Induló", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Íjtípus", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Korosztály", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNeme", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Dátum", System.Type.GetType( "System.String" ) ) );
            */

            var Oklevelek = Program.database.Oklevelek();
            foreach( var oklevel in Oklevelek ) {
                DataRow row = data.NewRow();

                #region set Rows

                int count = -1;
                row[++count] = oklevel.Azonosito;
                row[++count] = oklevel.Tipus;

                row[++count] = oklevel.VersenyX.ToString( );
                row[++count] = oklevel.VersenyY.ToString( );
                row[++count] = oklevel.VersenyH.ToString( );
                row[++count] = oklevel.VersenyB.ToString( );
                row[++count] = oklevel.VersenyF.ToString( );
                row[++count] = oklevel.VersenyM.ToString( );
                row[++count] = oklevel.VersenyI.ToString( );

                row[++count] = oklevel.VersenySorozatX.ToString( );
                row[++count] = oklevel.VersenySorozatY.ToString( );
                row[++count] = oklevel.VersenySorozatH.ToString( );
                row[++count] = oklevel.VersenySorozatB.ToString( );
                row[++count] = oklevel.VersenySorozatF.ToString( );
                row[++count] = oklevel.VersenySorozatM.ToString( );
                row[++count] = oklevel.VersenySorozatI.ToString( );

                row[++count] = oklevel.HelyezesX.ToString( );
                row[++count] = oklevel.HelyezesY.ToString( );
                row[++count] = oklevel.HelyezesH.ToString( );
                row[++count] = oklevel.HelyezesB.ToString( );
                row[++count] = oklevel.HelyezesF.ToString( );
                row[++count] = oklevel.HelyezesM.ToString( );
                row[++count] = oklevel.HelyezesI.ToString( );

                row[++count] = oklevel.InduloX.ToString( );
                row[++count] = oklevel.InduloY.ToString( );
                row[++count] = oklevel.InduloH.ToString( );
                row[++count] = oklevel.InduloB.ToString( );
                row[++count] = oklevel.InduloF.ToString( );
                row[++count] = oklevel.InduloM.ToString( );
                row[++count] = oklevel.InduloI.ToString( );

                row[++count] = oklevel.EgyesuletX.ToString( );
                row[++count] = oklevel.EgyesuletY.ToString( );
                row[++count] = oklevel.EgyesuletH.ToString( );
                row[++count] = oklevel.EgyesuletB.ToString( );
                row[++count] = oklevel.EgyesuletF.ToString( );
                row[++count] = oklevel.EgyesuletM.ToString( );
                row[++count] = oklevel.EgyesuletI.ToString( );

                row[++count] = oklevel.IjtipusX.ToString( );
                row[++count] = oklevel.IjtipusY.ToString( );
                row[++count] = oklevel.IjtipusH.ToString( );
                row[++count] = oklevel.IjtipusB.ToString( );
                row[++count] = oklevel.IjtipusF.ToString( );
                row[++count] = oklevel.IjtipusM.ToString( );
                row[++count] = oklevel.IjtipusI.ToString( );

                row[++count] = oklevel.KorosztalyX.ToString( );
                row[++count] = oklevel.KorosztalyY.ToString( );
                row[++count] = oklevel.KorosztalyH.ToString( );
                row[++count] = oklevel.KorosztalyB.ToString( );
                row[++count] = oklevel.KorosztalyF.ToString( );
                row[++count] = oklevel.KorosztalyM.ToString( );
                row[++count] = oklevel.KorosztalyI.ToString( );

                row[++count] = oklevel.InduloNemeX.ToString( );
                row[++count] = oklevel.InduloNemeY.ToString( );
                row[++count] = oklevel.InduloNemeH.ToString( );
                row[++count] = oklevel.InduloNemeB.ToString( );
                row[++count] = oklevel.InduloNemeF.ToString( );
                row[++count] = oklevel.InduloNemeM.ToString( );
                row[++count] = oklevel.InduloNemeI.ToString( );

                row[++count] = oklevel.DatumX.ToString( );
                row[++count] = oklevel.DatumY.ToString( );
                row[++count] = oklevel.DatumH.ToString( );
                row[++count] = oklevel.DatumB.ToString( );
                row[++count] = oklevel.DatumF.ToString( );
                row[++count] = oklevel.DatumM.ToString( );
                row[++count] = oklevel.DatumI.ToString( );
                #endregion

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

            //NOTE hiding columns
            for( int i = 2; i < table.Columns.Count; i++ ) {
                table.Columns[i].Visible = false;
            }

            //NOTE set visible columns width
            for( int i = 0; i < 2; i++ ) {
                table.Columns[i].Width = 120;
            }
        }

        #region Accessors
        private delegate void Sablon_Hozzáadás_Callback( Oklevel _oklevel );
        public void Sablon_Hozzáadás( Oklevel oklevel ) {
            if( InvokeRequired ) {
                Sablon_Hozzáadás_Callback callback = new Sablon_Hozzáadás_Callback(Sablon_Hozzáadás);
                Invoke( callback, new object[] { oklevel } );
            }
            else {
                if( !Program.database.UjOklevel( oklevel ) ) {
                    MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }

                DataRow row = data.NewRow();

                int count = -1;
                row[++count] = oklevel.Azonosito;
                row[++count] = oklevel.Tipus;

                row[++count] = oklevel.VersenyX.ToString( );
                row[++count] = oklevel.VersenyY.ToString( );
                row[++count] = oklevel.VersenyH.ToString( );
                row[++count] = oklevel.VersenyB.ToString( );
                row[++count] = oklevel.VersenyF.ToString( );
                row[++count] = oklevel.VersenyM.ToString( );
                row[++count] = oklevel.VersenyI.ToString( );

                row[++count] = oklevel.VersenySorozatX.ToString( );
                row[++count] = oklevel.VersenySorozatY.ToString( );
                row[++count] = oklevel.VersenySorozatH.ToString( );
                row[++count] = oklevel.VersenySorozatB.ToString( );
                row[++count] = oklevel.VersenySorozatF.ToString( );
                row[++count] = oklevel.VersenySorozatM.ToString( );
                row[++count] = oklevel.VersenySorozatI.ToString( );

                row[++count] = oklevel.HelyezesX.ToString( );
                row[++count] = oklevel.HelyezesY.ToString( );
                row[++count] = oklevel.HelyezesH.ToString( );
                row[++count] = oklevel.HelyezesB.ToString( );
                row[++count] = oklevel.HelyezesF.ToString( );
                row[++count] = oklevel.HelyezesM.ToString( );
                row[++count] = oklevel.HelyezesI.ToString( );

                row[++count] = oklevel.InduloX.ToString( );
                row[++count] = oklevel.InduloY.ToString( );
                row[++count] = oklevel.InduloH.ToString( );
                row[++count] = oklevel.InduloB.ToString( );
                row[++count] = oklevel.InduloF.ToString( );
                row[++count] = oklevel.InduloM.ToString( );
                row[++count] = oklevel.InduloI.ToString( );

                row[++count] = oklevel.EgyesuletX.ToString( );
                row[++count] = oklevel.EgyesuletY.ToString( );
                row[++count] = oklevel.EgyesuletH.ToString( );
                row[++count] = oklevel.EgyesuletB.ToString( );
                row[++count] = oklevel.EgyesuletF.ToString( );
                row[++count] = oklevel.EgyesuletM.ToString( );
                row[++count] = oklevel.EgyesuletI.ToString( );

                row[++count] = oklevel.IjtipusX.ToString( );
                row[++count] = oklevel.IjtipusY.ToString( );
                row[++count] = oklevel.IjtipusH.ToString( );
                row[++count] = oklevel.IjtipusB.ToString( );
                row[++count] = oklevel.IjtipusF.ToString( );
                row[++count] = oklevel.IjtipusM.ToString( );
                row[++count] = oklevel.IjtipusI.ToString( );

                row[++count] = oklevel.KorosztalyX.ToString( );
                row[++count] = oklevel.KorosztalyY.ToString( );
                row[++count] = oklevel.KorosztalyH.ToString( );
                row[++count] = oklevel.KorosztalyB.ToString( );
                row[++count] = oklevel.KorosztalyF.ToString( );
                row[++count] = oklevel.KorosztalyM.ToString( );
                row[++count] = oklevel.KorosztalyI.ToString( );

                row[++count] = oklevel.InduloNemeX.ToString( );
                row[++count] = oklevel.InduloNemeY.ToString( );
                row[++count] = oklevel.InduloNemeH.ToString( );
                row[++count] = oklevel.InduloNemeB.ToString( );
                row[++count] = oklevel.InduloNemeF.ToString( );
                row[++count] = oklevel.InduloNemeM.ToString( );
                row[++count] = oklevel.InduloNemeI.ToString( );

                row[++count] = oklevel.DatumX.ToString( );
                row[++count] = oklevel.DatumY.ToString( );
                row[++count] = oklevel.DatumH.ToString( );
                row[++count] = oklevel.DatumB.ToString( );
                row[++count] = oklevel.DatumF.ToString( );
                row[++count] = oklevel.DatumM.ToString( );
                row[++count] = oklevel.DatumI.ToString( );

                data.Rows.Add( row );
                if( sablon_hozzáadva != null )
                    sablon_hozzáadva( oklevel );
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
                    #region set Rows

                    int count = -1;

                    current[++count] = _oklevel.Azonosito;
                    current[++count] = _oklevel.Tipus;

                    current[++count] = _oklevel.VersenyX.ToString( );
                    current[++count] = _oklevel.VersenyY.ToString( );
                    current[++count] = _oklevel.VersenyH.ToString( );
                    current[++count] = _oklevel.VersenyB.ToString( );
                    current[++count] = _oklevel.VersenyF.ToString( );
                    current[++count] = _oklevel.VersenyM.ToString( );
                    current[++count] = _oklevel.VersenyI.ToString( );

                    current[++count] = _oklevel.VersenySorozatX.ToString( );
                    current[++count] = _oklevel.VersenySorozatY.ToString( );
                    current[++count] = _oklevel.VersenySorozatH.ToString( );
                    current[++count] = _oklevel.VersenySorozatB.ToString( );
                    current[++count] = _oklevel.VersenySorozatF.ToString( );
                    current[++count] = _oklevel.VersenySorozatM.ToString( );
                    current[++count] = _oklevel.VersenySorozatI.ToString( );

                    current[++count] = _oklevel.HelyezesX.ToString( );
                    current[++count] = _oklevel.HelyezesY.ToString( );
                    current[++count] = _oklevel.HelyezesH.ToString( );
                    current[++count] = _oklevel.HelyezesB.ToString( );
                    current[++count] = _oklevel.HelyezesF.ToString( );
                    current[++count] = _oklevel.HelyezesM.ToString( );
                    current[++count] = _oklevel.HelyezesI.ToString( );

                    current[++count] = _oklevel.InduloX.ToString( );
                    current[++count] = _oklevel.InduloY.ToString( );
                    current[++count] = _oklevel.InduloH.ToString( );
                    current[++count] = _oklevel.InduloB.ToString( );
                    current[++count] = _oklevel.InduloF.ToString( );
                    current[++count] = _oklevel.InduloM.ToString( );
                    current[++count] = _oklevel.InduloI.ToString( );

                    current[++count] = _oklevel.EgyesuletX.ToString( );
                    current[++count] = _oklevel.EgyesuletY.ToString( );
                    current[++count] = _oklevel.EgyesuletH.ToString( );
                    current[++count] = _oklevel.EgyesuletB.ToString( );
                    current[++count] = _oklevel.EgyesuletF.ToString( );
                    current[++count] = _oklevel.EgyesuletM.ToString( );
                    current[++count] = _oklevel.EgyesuletI.ToString( );

                    current[++count] = _oklevel.IjtipusX.ToString( );
                    current[++count] = _oklevel.IjtipusY.ToString( );
                    current[++count] = _oklevel.IjtipusH.ToString( );
                    current[++count] = _oklevel.IjtipusB.ToString( );
                    current[++count] = _oklevel.IjtipusF.ToString( );
                    current[++count] = _oklevel.IjtipusM.ToString( );
                    current[++count] = _oklevel.IjtipusI.ToString( );

                    current[++count] = _oklevel.KorosztalyX.ToString( );
                    current[++count] = _oklevel.KorosztalyY.ToString( );
                    current[++count] = _oklevel.KorosztalyH.ToString( );
                    current[++count] = _oklevel.KorosztalyB.ToString( );
                    current[++count] = _oklevel.KorosztalyF.ToString( );
                    current[++count] = _oklevel.KorosztalyM.ToString( );
                    current[++count] = _oklevel.KorosztalyI.ToString( );

                    current[++count] = _oklevel.InduloNemeX.ToString( );
                    current[++count] = _oklevel.InduloNemeY.ToString( );
                    current[++count] = _oklevel.InduloNemeH.ToString( );
                    current[++count] = _oklevel.InduloNemeB.ToString( );
                    current[++count] = _oklevel.InduloNemeF.ToString( );
                    current[++count] = _oklevel.InduloNemeM.ToString( );
                    current[++count] = _oklevel.InduloNemeI.ToString( );

                    current[++count] = _oklevel.DatumX.ToString( );
                    current[++count] = _oklevel.DatumY.ToString( );
                    current[++count] = _oklevel.DatumH.ToString( );
                    current[++count] = _oklevel.DatumB.ToString( );
                    current[++count] = _oklevel.DatumF.ToString( );
                    current[++count] = _oklevel.DatumM.ToString( );
                    current[++count] = _oklevel.DatumI.ToString( );
                    #endregion
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
            int c = -1;
            Oklevel oklevel = new Oklevel {
                Azonosito = data.Rows[table.SelectedRows[0].Index][++c].ToString(),
                Tipus = data.Rows[table.SelectedRows[0].Index][++c].ToString(),

                VersenyX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenyF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenyM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                VersenySorozatX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenySorozatF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenySorozatM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                HelyezesX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                HelyezesF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                HelyezesM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                InduloX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                EgyesuletX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                EgyesuletF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                EgyesuletM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                IjtipusX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                IjtipusF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                IjtipusM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                KorosztalyX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                KorosztalyF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                KorosztalyM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                InduloNemeX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloNemeF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloNemeM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                DatumX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumB = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                DatumF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                DatumM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
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

