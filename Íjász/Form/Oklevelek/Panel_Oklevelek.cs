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
        public string BetuTipus { get; set; }

        public int VersenyX { get; set; }
        public int VersenyY { get; set; }
        public int VersenyH { get; set; }
        public string VersenyF { get; set; }
        public int VersenyM { get; set; }
        public string VersenyI { get; set; }

        public int VersenySorozatX { get; set; }
        public int VersenySorozatY { get; set; }
        public int VersenySorozatH { get; set; }
        public string VersenySorozatF { get; set; }
        public int VersenySorozatM { get; set; }
        public string VersenySorozatI { get; set; }

        public int HelyezesX { get; set; }
        public int HelyezesY { get; set; }
        public int HelyezesH { get; set; }
        public string HelyezesF { get; set; }
        public int HelyezesM { get; set; }
        public string HelyezesI { get; set; }

        public int InduloX { get; set; }
        public int InduloY { get; set; }
        public int InduloH { get; set; }
        public string InduloF { get; set; }
        public int InduloM { get; set; }
        public string InduloI { get; set; }

        public int EgyesuletX { get; set; }
        public int EgyesuletY { get; set; }
        public int EgyesuletH { get; set; }
        public string EgyesuletF { get; set; }
        public int EgyesuletM { get; set; }
        public string EgyesuletI { get; set; }

        public int IjtipusX { get; set; }
        public int IjtipusY { get; set; }
        public int IjtipusH { get; set; }
        public string IjtipusF { get; set; }
        public int IjtipusM { get; set; }
        public string IjtipusI { get; set; }

        public int KorosztalyX { get; set; }
        public int KorosztalyY { get; set; }
        public int KorosztalyH { get; set; }
        public string KorosztalyF { get; set; }
        public int KorosztalyM { get; set; }
        public string KorosztalyI { get; set; }

        public int InduloNemeX { get; set; }
        public int InduloNemeY { get; set; }
        public int InduloNemeH { get; set; }
        public string InduloNemeF { get; set; }
        public int InduloNemeM { get; set; }
        public string InduloNemeI { get; set; }

        public int DatumX { get; set; }
        public int DatumY { get; set; }
        public int DatumH { get; set; }
        public string DatumF { get; set; }
        public int DatumM { get; set; }
        public string DatumI { get; set; }

        public Oklevel(string azonosito, string tipus, string betuTipus, 
            int versenyX, int versenyY, int versenyH, string versenyF, int versenyM, string versenyI,
            int versenySorozatX, int versenySorozatY, int versenySorozatH, string versenySorozatF, int versenySorozatM, string versenySorozatI,
            int helyezesX, int helyezesY, int helyezesH, string helyezesF, int helyezesM, string helyezesI, 
            int induloX, int induloY, int induloH, string induloF, int induloM, string induloI, 
            int egyesuletX, int egyesuletY, int egyesuletH, string egyesuletF, int egyesuletM, string egyesuletI, 
            int ijtipusX, int ijtipusY, int ijtipusH, string ijtipusF, int ijtipusM, string ijtipusI, 
            int korosztalyX, int korosztalyY, int korosztalyH, string korosztalyF, int korosztalyM, string korosztalyI, 
            int induloNemeX, int induloNemeY, int induloNemeH, string induloNemeF, int induloNemeM, string induloNemeI, 
            int datumX, int datumY, int datumH, string datumF, int datumM, string datumI)
        {
            Azonosito = azonosito;
            Tipus = tipus;
            BetuTipus = betuTipus;
            VersenyX = versenyX;
            VersenyY = versenyY;
            VersenyH = versenyH;
            VersenyF = versenyF;
            VersenyM = versenyM;
            VersenyI = versenyI;
            VersenySorozatX = versenySorozatX;
            VersenySorozatY = versenySorozatY;
            VersenySorozatH = versenySorozatH;
            VersenySorozatF = versenySorozatF;
            VersenySorozatM = versenySorozatM;
            VersenySorozatI = versenySorozatI;
            HelyezesX = helyezesX;
            HelyezesY = helyezesY;
            HelyezesH = helyezesH;
            HelyezesF = helyezesF;
            HelyezesM = helyezesM;
            HelyezesI = helyezesI;
            InduloX = induloX;
            InduloY = induloY;
            InduloH = induloH;
            InduloF = induloF;
            InduloM = induloM;
            InduloI = induloI;
            EgyesuletX = egyesuletX;
            EgyesuletY = egyesuletY;
            EgyesuletH = egyesuletH;
            EgyesuletF = egyesuletF;
            EgyesuletM = egyesuletM;
            EgyesuletI = egyesuletI;
            IjtipusX = ijtipusX;
            IjtipusY = ijtipusY;
            IjtipusH = ijtipusH;
            IjtipusF = ijtipusF;
            IjtipusM = ijtipusM;
            IjtipusI = ijtipusI;
            KorosztalyX = korosztalyX;
            KorosztalyY = korosztalyY;
            KorosztalyH = korosztalyH;
            KorosztalyF = korosztalyF;
            KorosztalyM = korosztalyM;
            KorosztalyI = korosztalyI;
            InduloNemeX = induloNemeX;
            InduloNemeY = induloNemeY;
            InduloNemeH = induloNemeH;
            InduloNemeF = induloNemeF;
            InduloNemeM = induloNemeM;
            InduloNemeI = induloNemeI;
            DatumX = datumX;
            DatumY = datumY;
            DatumH = datumH;
            DatumF = datumF;
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
                Width = 1000,
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

        
        public string formatSeged(string _code ) {
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
            data.Columns.Add( new DataColumn( "Betűtípus", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "VersenyX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenyI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "VersenySorozatX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "VersenySorozatI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "HelyezesX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "HelyezesI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "IndulóX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "EgyesületX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "EgyesületI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "ÍjtípusX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "ÍjtípusI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "KorosztályX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "KorosztályI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "IndulóNemeX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNemeI", System.Type.GetType( "System.String" ) ) );

            data.Columns.Add( new DataColumn( "DátumX", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumY", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumH", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumF", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumM", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "DátumI", System.Type.GetType( "System.String" ) ) );


            data.Columns.Add( new DataColumn( "Verseny", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Versenysorozat", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Helyezes", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Induló", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Íjtípus", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Korosztály", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "IndulóNeme", System.Type.GetType( "System.String" ) ) );
            data.Columns.Add( new DataColumn( "Dátum", System.Type.GetType( "System.String" ) ) );

            var Oklevelek = Program.database.Oklevelek();
            foreach( var oklevel in Oklevelek ) {
                DataRow row = data.NewRow();

                #region set Rows

                row[0] = oklevel.Azonosito;
                row[1] = oklevel.Tipus;
                row[2] = oklevel.BetuTipus;

                row[3] = oklevel.VersenyX.ToString( );
                row[4] = oklevel.VersenyY.ToString( );
                row[5] = oklevel.VersenyH.ToString( );
                row[6] = oklevel.VersenyF.ToString( );
                row[7] = oklevel.VersenyM.ToString( );
                row[8] = oklevel.VersenyI.ToString( );

                row[9] = oklevel.VersenySorozatX.ToString( );
                row[10] = oklevel.VersenySorozatY.ToString( );
                row[11] = oklevel.VersenySorozatH.ToString( );
                row[12] = oklevel.VersenySorozatF.ToString( );
                row[13] = oklevel.VersenySorozatM.ToString( );
                row[14] = oklevel.VersenySorozatI.ToString( );

                row[15] = oklevel.HelyezesX.ToString( );
                row[16] = oklevel.HelyezesY.ToString( );
                row[17] = oklevel.HelyezesH.ToString( );
                row[18] = oklevel.HelyezesF.ToString( );
                row[19] = oklevel.HelyezesM.ToString( );
                row[20] = oklevel.HelyezesI.ToString( );

                row[21] = oklevel.InduloX.ToString( );
                row[22] = oklevel.InduloY.ToString( );
                row[23] = oklevel.InduloH.ToString( );
                row[24] = oklevel.InduloF.ToString( );
                row[25] = oklevel.InduloM.ToString( );
                row[26] = oklevel.InduloI.ToString( );

                row[27] = oklevel.EgyesuletX.ToString( );
                row[28] = oklevel.EgyesuletY.ToString( );
                row[29] = oklevel.EgyesuletH.ToString( );
                row[30] = oklevel.EgyesuletF.ToString( );
                row[31] = oklevel.EgyesuletM.ToString( );
                row[32] = oklevel.EgyesuletI.ToString( );

                row[33] = oklevel.IjtipusX.ToString( );
                row[34] = oklevel.IjtipusY.ToString( );
                row[35] = oklevel.IjtipusH.ToString( );
                row[36] = oklevel.IjtipusF.ToString( );
                row[37] = oklevel.IjtipusM.ToString( );
                row[38] = oklevel.IjtipusI.ToString( );

                row[39] = oklevel.KorosztalyX.ToString( );
                row[40] = oklevel.KorosztalyY.ToString( );
                row[41] = oklevel.KorosztalyH.ToString( );
                row[42] = oklevel.KorosztalyF.ToString( );
                row[43] = oklevel.KorosztalyM.ToString( );
                row[44] = oklevel.KorosztalyI.ToString( );

                row[45] = oklevel.InduloNemeX.ToString( );
                row[46] = oklevel.InduloNemeY.ToString( );
                row[47] = oklevel.InduloNemeH.ToString( );
                row[48] = oklevel.InduloNemeF.ToString( );
                row[49] = oklevel.InduloNemeM.ToString( );
                row[50] = oklevel.InduloNemeI.ToString( );

                row[51] = oklevel.DatumX.ToString( );
                row[52] = oklevel.DatumY.ToString( );
                row[53] = oklevel.DatumH.ToString( );
                row[54] = oklevel.DatumF.ToString( );
                row[55] = oklevel.DatumM.ToString( );
                row[56] = oklevel.DatumI.ToString( );


                //-------------------------------------------------

                row[57] = oklevel.VersenyX != 0 ? "Xpos: " + oklevel.VersenyX.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += oklevel.VersenyY != 0 ? "Ypos: " + oklevel.VersenyY.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += oklevel.VersenyH != 0 ? "Hossz: " + oklevel.VersenyH.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += formatSeged( oklevel.VersenyF );
                row[57] += Environment.NewLine;
                row[57] += oklevel.VersenyM != 0 ? oklevel.VersenyM.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += igazitasSeged(oklevel.VersenyI);

                row[58] = oklevel.VersenySorozatX != 0 ? "Xpos: " + oklevel.VersenySorozatX.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += oklevel.VersenySorozatY != 0 ? "Ypos: " + oklevel.VersenySorozatY.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += oklevel.VersenySorozatH != 0 ? "Hossz: " + oklevel.VersenySorozatH.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += formatSeged( oklevel.VersenySorozatF );
                row[58] += Environment.NewLine;
                row[58] += oklevel.VersenySorozatM != 0 ? oklevel.VersenySorozatM.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += igazitasSeged(oklevel.VersenySorozatI);

                row[59] = oklevel.HelyezesX != 0 ? "Xpos: " + oklevel.HelyezesX.ToString( ) : " ";
                row[59] +=  Environment.NewLine;
                row[59] += oklevel.HelyezesY != 0 ? "Ypos: " + oklevel.HelyezesY.ToString( ) : " ";
                row[59] +=  Environment.NewLine;
                row[59] += oklevel.HelyezesH != 0 ? "Hossz: " + oklevel.HelyezesH.ToString( ) : " ";
                row[59] +=  Environment.NewLine;
                row[59] += formatSeged( oklevel.HelyezesF );
                row[59] +=  Environment.NewLine;
                row[59] += oklevel.HelyezesM != 0 ? oklevel.HelyezesM.ToString( ) : " ";
                row[59] +=  Environment.NewLine;
                row[59] += igazitasSeged( oklevel.HelyezesI);

                row[60] = oklevel.InduloX != 0 ? "Xpos: " + oklevel.InduloX.ToString( ) : " ";
                row[60] +=  Environment.NewLine;
                row[60] += oklevel.InduloY != 0 ? "Ypos: " + oklevel.InduloY.ToString( ) : " ";
                row[60] +=  Environment.NewLine;
                row[60] += oklevel.InduloH != 0 ? "Hossz: " + oklevel.InduloH.ToString( ) : " ";
                row[60] +=  Environment.NewLine;
                row[60] += formatSeged( oklevel.InduloF );
                row[60] +=  Environment.NewLine;
                row[60] += oklevel.InduloM != 0 ? oklevel.InduloM.ToString( ) : " ";
                row[60] +=  Environment.NewLine;
                row[60] += igazitasSeged(  oklevel.InduloI);

                row[61] = oklevel.EgyesuletX != 0 ? "Xpos: " + oklevel.EgyesuletX.ToString( ) : " ";
                row[61] +=  Environment.NewLine;
                row[61] += oklevel.EgyesuletY != 0 ?  "Ypos: " + oklevel.EgyesuletY.ToString( ) : " ";
                row[61] +=  Environment.NewLine;
                row[61] += oklevel.EgyesuletH != 0 ? "Hossz: " + oklevel.EgyesuletH.ToString( ) : " ";
                row[61] +=  Environment.NewLine;
                row[61] += formatSeged( oklevel.EgyesuletF );
                row[61] +=  Environment.NewLine;
                row[61] += oklevel.EgyesuletM != 0 ? oklevel.EgyesuletM.ToString( ) : " ";
                row[61] +=  Environment.NewLine;
                row[61] += igazitasSeged( oklevel.EgyesuletI);

                row[62] = oklevel.IjtipusX != 0 ? "Xpos: " + oklevel.IjtipusX.ToString( ) : " ";
                row[62] +=  Environment.NewLine;
                row[62] += oklevel.IjtipusY != 0 ? "Ypos: " + oklevel.IjtipusY.ToString( ) : " ";
                row[62] +=  Environment.NewLine;
                row[62] += oklevel.IjtipusH != 0 ? "Hossz: " + oklevel.IjtipusH.ToString( ) : " ";
                row[62] +=  Environment.NewLine;
                row[62] += formatSeged( oklevel.IjtipusF );
                row[62] +=  Environment.NewLine;
                row[62] += oklevel.IjtipusM != 0 ? oklevel.IjtipusM.ToString( ) : " ";
                row[62] +=  Environment.NewLine;
                row[62] += igazitasSeged( oklevel.IjtipusI);

                row[63] = oklevel.KorosztalyX != 0 ? "Xpos: " + oklevel.KorosztalyX.ToString( ) : " ";
                row[63] +=  Environment.NewLine;
                row[63] += oklevel.KorosztalyY != 0 ? "Ypos: " + oklevel.KorosztalyY.ToString( ) : " ";
                row[63] +=  Environment.NewLine;
                row[63] += oklevel.KorosztalyH != 0 ? "Hossz: " + oklevel.KorosztalyH.ToString( ) : " ";
                row[63] +=  Environment.NewLine;
                row[63] += formatSeged( oklevel.KorosztalyF );
                row[63] +=  Environment.NewLine;
                row[63] += oklevel.KorosztalyM != 0 ? oklevel.KorosztalyM.ToString( ) : " ";
                row[63] +=  Environment.NewLine;
                row[63] += igazitasSeged( oklevel.KorosztalyI );

                row[64] = oklevel.InduloNemeX != 0 ? "Xpos: " + oklevel.InduloNemeX.ToString( ) : " ";
                row[64] +=  Environment.NewLine;
                row[64] += oklevel.InduloNemeY != 0 ? "Ypos: " + oklevel.InduloNemeY.ToString( ) : " ";
                row[64] +=  Environment.NewLine;
                row[64] += oklevel.InduloNemeH != 0 ? "Hossz: " + oklevel.InduloNemeH.ToString( ) : " ";
                row[64] +=  Environment.NewLine;
                row[64] += formatSeged( oklevel.InduloNemeF );
                row[64] +=  Environment.NewLine;
                row[64] += oklevel.InduloNemeM != 0 ? oklevel.InduloNemeM.ToString( ) : " ";
                row[64] +=  Environment.NewLine;
                row[64] += igazitasSeged( oklevel.InduloNemeI );

                row[65] = oklevel.DatumX != 0 ? "Xpos: " + oklevel.DatumX.ToString( ) : " ";
                row[65] +=  Environment.NewLine;
                row[65] += oklevel.DatumY != 0 ? "Ypos: " + oklevel.DatumY.ToString( ) : " ";
                row[65] +=  Environment.NewLine;
                row[65] += oklevel.DatumH != 0 ? "Hossz: " + oklevel.DatumH.ToString( ) : " ";
                row[65] +=  Environment.NewLine;
                row[65] += formatSeged( oklevel.DatumF );
                row[65] +=  Environment.NewLine;
                row[65] += oklevel.DatumM != 0 ? oklevel.DatumM.ToString( ) : " ";
                row[65] +=  Environment.NewLine;
                row[65] += igazitasSeged(oklevel.DatumI);
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
            for( int i = 3; i < 57; i++ ) {
                table.Columns[i].Visible = false;
            }

            //NOTE set visible columns width
            for( int i = 57; i < 66; i++ ) {
                table.Columns[i].Width = 79;
            }
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

                #region set Rows

                row[0] = _oklevel.Azonosito;
                row[1] = _oklevel.Tipus;
                row[2] = _oklevel.BetuTipus;

                row[3] = _oklevel.VersenyX.ToString( );
                row[4] = _oklevel.VersenyY.ToString( );
                row[5] = _oklevel.VersenyH.ToString( );
                row[6] = _oklevel.VersenyF.ToString( );
                row[7] = _oklevel.VersenyM.ToString( );
                row[8] = _oklevel.VersenyI.ToString( );

                row[9] = _oklevel.VersenySorozatX.ToString( );
                row[10] = _oklevel.VersenySorozatY.ToString( );
                row[11] = _oklevel.VersenySorozatH.ToString( );
                row[12] = _oklevel.VersenySorozatF.ToString( );
                row[13] = _oklevel.VersenySorozatM.ToString( );
                row[14] = _oklevel.VersenySorozatI.ToString( );

                row[15] = _oklevel.HelyezesX.ToString( );
                row[16] = _oklevel.HelyezesY.ToString( );
                row[17] = _oklevel.HelyezesH.ToString( );
                row[18] = _oklevel.HelyezesF.ToString( );
                row[19] = _oklevel.HelyezesM.ToString( );
                row[20] = _oklevel.HelyezesI.ToString( );

                row[21] = _oklevel.InduloX.ToString( );
                row[22] = _oklevel.InduloY.ToString( );
                row[23] = _oklevel.InduloH.ToString( );
                row[24] = _oklevel.InduloF.ToString( );
                row[25] = _oklevel.InduloM.ToString( );
                row[26] = _oklevel.InduloI.ToString( );

                row[27] = _oklevel.EgyesuletX.ToString( );
                row[28] = _oklevel.EgyesuletY.ToString( );
                row[29] = _oklevel.EgyesuletH.ToString( );
                row[30] = _oklevel.EgyesuletF.ToString( );
                row[31] = _oklevel.EgyesuletM.ToString( );
                row[32] = _oklevel.EgyesuletI.ToString( );

                row[33] = _oklevel.IjtipusX.ToString( );
                row[34] = _oklevel.IjtipusY.ToString( );
                row[35] = _oklevel.IjtipusH.ToString( );
                row[36] = _oklevel.IjtipusF.ToString( );
                row[37] = _oklevel.IjtipusM.ToString( );
                row[38] = _oklevel.IjtipusI.ToString( );

                row[39] = _oklevel.KorosztalyX.ToString( );
                row[40] = _oklevel.KorosztalyY.ToString( );
                row[41] = _oklevel.KorosztalyH.ToString( );
                row[42] = _oklevel.KorosztalyF.ToString( );
                row[43] = _oklevel.KorosztalyM.ToString( );
                row[44] = _oklevel.KorosztalyI.ToString( );

                row[45] = _oklevel.InduloNemeX.ToString( );
                row[46] = _oklevel.InduloNemeY.ToString( );
                row[47] = _oklevel.InduloNemeH.ToString( );
                row[48] = _oklevel.InduloNemeF.ToString( );
                row[49] = _oklevel.InduloNemeM.ToString( );
                row[50] = _oklevel.InduloNemeI.ToString( );

                row[51] = _oklevel.DatumX.ToString( );
                row[52] = _oklevel.DatumY.ToString( );
                row[53] = _oklevel.DatumH.ToString( );
                row[54] = _oklevel.DatumF.ToString( );
                row[55] = _oklevel.DatumM.ToString( );
                row[56] = _oklevel.DatumI.ToString( );


                //-------------------------------------------------

                row[57] = _oklevel.VersenyX != 0 ? "Xpos: " + _oklevel.VersenyX.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += _oklevel.VersenyY != 0 ? "Ypos: " + _oklevel.VersenyY.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += _oklevel.VersenyH != 0 ? "Hossz: " + _oklevel.VersenyH.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += formatSeged( _oklevel.VersenyF );
                row[57] += Environment.NewLine;
                row[57] += _oklevel.VersenyM != 0 ? _oklevel.VersenyM.ToString( ) : " ";
                row[57] += Environment.NewLine;
                row[57] += igazitasSeged( _oklevel.VersenyI );

                row[58] = _oklevel.VersenySorozatX != 0 ? "Xpos: " + _oklevel.VersenySorozatX.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += _oklevel.VersenySorozatY != 0 ? "Ypos: " + _oklevel.VersenySorozatY.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += _oklevel.VersenySorozatH != 0 ? "Hossz: " + _oklevel.VersenySorozatH.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += formatSeged( _oklevel.VersenySorozatF );
                row[58] += Environment.NewLine;
                row[58] += _oklevel.VersenySorozatM != 0 ? _oklevel.VersenySorozatM.ToString( ) : " ";
                row[58] += Environment.NewLine;
                row[58] += igazitasSeged( _oklevel.VersenySorozatI );

                row[59] = _oklevel.HelyezesX != 0 ? "Xpos: " + _oklevel.HelyezesX.ToString( ) : " ";
                row[59] += Environment.NewLine;
                row[59] += _oklevel.HelyezesY != 0 ? "Ypos: " + _oklevel.HelyezesY.ToString( ) : " ";
                row[59] += Environment.NewLine;
                row[59] += _oklevel.HelyezesH != 0 ? "Hossz: " + _oklevel.HelyezesH.ToString( ) : " ";
                row[59] += Environment.NewLine;
                row[59] += formatSeged( _oklevel.HelyezesF );
                row[59] += Environment.NewLine;
                row[59] += _oklevel.HelyezesM != 0 ? _oklevel.HelyezesM.ToString( ) : " ";
                row[59] += Environment.NewLine;
                row[59] += igazitasSeged( _oklevel.HelyezesI );

                row[60] = _oklevel.InduloX != 0 ? "Xpos: " + _oklevel.InduloX.ToString( ) : " ";
                row[60] += Environment.NewLine;
                row[60] += _oklevel.InduloY != 0 ? "Ypos: " + _oklevel.InduloY.ToString( ) : " ";
                row[60] += Environment.NewLine;
                row[60] += _oklevel.InduloH != 0 ? "Hossz: " + _oklevel.InduloH.ToString( ) : " ";
                row[60] += Environment.NewLine;
                row[60] += formatSeged( _oklevel.InduloF );
                row[60] += Environment.NewLine;
                row[60] += _oklevel.InduloM != 0 ? _oklevel.InduloM.ToString( ) : " ";
                row[60] += Environment.NewLine;
                row[60] += igazitasSeged( _oklevel.InduloI );

                row[61] = _oklevel.EgyesuletX != 0 ? "Xpos: " + _oklevel.EgyesuletX.ToString( ) : " ";
                row[61] += Environment.NewLine;
                row[61] += _oklevel.EgyesuletY != 0 ? "Ypos: " + _oklevel.EgyesuletY.ToString( ) : " ";
                row[61] += Environment.NewLine;
                row[61] += _oklevel.EgyesuletH != 0 ? "Hossz: " + _oklevel.EgyesuletH.ToString( ) : " ";
                row[61] += Environment.NewLine;
                row[61] += formatSeged( _oklevel.EgyesuletF );
                row[61] += Environment.NewLine;
                row[61] += _oklevel.EgyesuletM != 0 ? _oklevel.EgyesuletM.ToString( ) : " ";
                row[61] += Environment.NewLine;
                row[61] += igazitasSeged( _oklevel.EgyesuletI );

                row[62] = _oklevel.IjtipusX != 0 ? "Xpos: " + _oklevel.IjtipusX.ToString( ) : " ";
                row[62] += Environment.NewLine;
                row[62] += _oklevel.IjtipusY != 0 ? "Ypos: " + _oklevel.IjtipusY.ToString( ) : " ";
                row[62] += Environment.NewLine;
                row[62] += _oklevel.IjtipusH != 0 ? "Hossz: " + _oklevel.IjtipusH.ToString( ) : " ";
                row[62] += Environment.NewLine;
                row[62] += formatSeged( _oklevel.IjtipusF );
                row[62] += Environment.NewLine;
                row[62] += _oklevel.IjtipusM != 0 ? _oklevel.IjtipusM.ToString( ) : " ";
                row[62] += Environment.NewLine;
                row[62] += igazitasSeged( _oklevel.IjtipusI );

                row[63] = _oklevel.KorosztalyX != 0 ? "Xpos: " + _oklevel.KorosztalyX.ToString( ) : " ";
                row[63] += Environment.NewLine;
                row[63] += _oklevel.KorosztalyY != 0 ? "Ypos: " + _oklevel.KorosztalyY.ToString( ) : " ";
                row[63] += Environment.NewLine;
                row[63] += _oklevel.KorosztalyH != 0 ? "Hossz: " + _oklevel.KorosztalyH.ToString( ) : " ";
                row[63] += Environment.NewLine;
                row[63] += formatSeged( _oklevel.KorosztalyF );
                row[63] += Environment.NewLine;
                row[63] += _oklevel.KorosztalyM != 0 ? _oklevel.KorosztalyM.ToString( ) : " ";
                row[63] += Environment.NewLine;
                row[63] += igazitasSeged( _oklevel.KorosztalyI );

                row[64] = _oklevel.InduloNemeX != 0 ? "Xpos: " + _oklevel.InduloNemeX.ToString( ) : " ";
                row[64] += Environment.NewLine;
                row[64] += _oklevel.InduloNemeY != 0 ? "Ypos: " + _oklevel.InduloNemeY.ToString( ) : " ";
                row[64] += Environment.NewLine;
                row[64] += _oklevel.InduloNemeH != 0 ? "Hossz: " + _oklevel.InduloNemeH.ToString( ) : " ";
                row[64] += Environment.NewLine;
                row[64] += formatSeged( _oklevel.InduloNemeF );
                row[64] += Environment.NewLine;
                row[64] += _oklevel.InduloNemeM != 0 ? _oklevel.InduloNemeM.ToString( ) : " ";
                row[64] += Environment.NewLine;
                row[64] += igazitasSeged( _oklevel.InduloNemeI );

                row[65] = _oklevel.DatumX != 0 ? "Xpos: " + _oklevel.DatumX.ToString( ) : " ";
                row[65] += Environment.NewLine;
                row[65] += _oklevel.DatumY != 0 ? "Ypos: " + _oklevel.DatumY.ToString( ) : " ";
                row[65] += Environment.NewLine;
                row[65] += _oklevel.DatumH != 0 ? "Hossz: " + _oklevel.DatumH.ToString( ) : " ";
                row[65] += Environment.NewLine;
                row[65] += formatSeged( _oklevel.DatumF );
                row[65] += Environment.NewLine;
                row[65] += _oklevel.DatumM != 0 ? _oklevel.DatumM.ToString( ) : " ";
                row[65] += Environment.NewLine;
                row[65] += igazitasSeged( _oklevel.DatumI );
                #endregion

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
                    #region set Rows

                    current[0] = _oklevel.Azonosito;
                    current[1] = _oklevel.Tipus;
                    current[2] = _oklevel.BetuTipus;

                    current[3] = _oklevel.VersenyX.ToString( );
                    current[4] = _oklevel.VersenyY.ToString( );
                    current[5] = _oklevel.VersenyH.ToString( );
                    current[6] = _oklevel.VersenyF.ToString( );
                    current[7] = _oklevel.VersenyM.ToString( );
                    current[8] = _oklevel.VersenyI.ToString( );

                    current[9] = _oklevel.VersenySorozatX.ToString( );
                    current[10] = _oklevel.VersenySorozatY.ToString( );
                    current[11] = _oklevel.VersenySorozatH.ToString( );
                    current[12] = _oklevel.VersenySorozatF.ToString( );
                    current[13] = _oklevel.VersenySorozatM.ToString( );
                    current[14] = _oklevel.VersenySorozatI.ToString( );

                    current[15] = _oklevel.HelyezesX.ToString( );
                    current[16] = _oklevel.HelyezesY.ToString( );
                    current[17] = _oklevel.HelyezesH.ToString( );
                    current[18] = _oklevel.HelyezesF.ToString( );
                    current[19] = _oklevel.HelyezesM.ToString( );
                    current[20] = _oklevel.HelyezesI.ToString( );

                    current[21] = _oklevel.InduloX.ToString( );
                    current[22] = _oklevel.InduloY.ToString( );
                    current[23] = _oklevel.InduloH.ToString( );
                    current[24] = _oklevel.InduloF.ToString( );
                    current[25] = _oklevel.InduloM.ToString( );
                    current[26] = _oklevel.InduloI.ToString( );

                    current[27] = _oklevel.EgyesuletX.ToString( );
                    current[28] = _oklevel.EgyesuletY.ToString( );
                    current[29] = _oklevel.EgyesuletH.ToString( );
                    current[30] = _oklevel.EgyesuletF.ToString( );
                    current[31] = _oklevel.EgyesuletM.ToString( );
                    current[32] = _oklevel.EgyesuletI.ToString( );

                    current[33] = _oklevel.IjtipusX.ToString( );
                    current[34] = _oklevel.IjtipusY.ToString( );
                    current[35] = _oklevel.IjtipusH.ToString( );
                    current[36] = _oklevel.IjtipusF.ToString( );
                    current[37] = _oklevel.IjtipusM.ToString( );
                    current[38] = _oklevel.IjtipusI.ToString( );

                    current[39] = _oklevel.KorosztalyX.ToString( );
                    current[40] = _oklevel.KorosztalyY.ToString( );
                    current[41] = _oklevel.KorosztalyH.ToString( );
                    current[42] = _oklevel.KorosztalyF.ToString( );
                    current[43] = _oklevel.KorosztalyM.ToString( );
                    current[44] = _oklevel.KorosztalyI.ToString( );

                    current[45] = _oklevel.InduloNemeX.ToString( );
                    current[46] = _oklevel.InduloNemeY.ToString( );
                    current[47] = _oklevel.InduloNemeH.ToString( );
                    current[48] = _oklevel.InduloNemeF.ToString( );
                    current[49] = _oklevel.InduloNemeM.ToString( );
                    current[50] = _oklevel.InduloNemeI.ToString( );

                    current[51] = _oklevel.DatumX.ToString( );
                    current[52] = _oklevel.DatumY.ToString( );
                    current[53] = _oklevel.DatumH.ToString( );
                    current[54] = _oklevel.DatumF.ToString( );
                    current[55] = _oklevel.DatumM.ToString( );
                    current[56] = _oklevel.DatumI.ToString( );


                    //-------------------------------------------------

                    current[57] = _oklevel.VersenyX != 0 ? "Xpos: " + _oklevel.VersenyX.ToString( ) : " ";
                    current[57] += Environment.NewLine;
                    current[57] += _oklevel.VersenyY != 0 ? "Ypos: " + _oklevel.VersenyY.ToString( ) : " ";
                    current[57] += Environment.NewLine;
                    current[57] += _oklevel.VersenyH != 0 ? "Hossz: " + _oklevel.VersenyH.ToString( ) : " ";
                    current[57] += Environment.NewLine;
                    current[57] += formatSeged( _oklevel.VersenyF );
                    current[57] += Environment.NewLine;
                    current[57] += _oklevel.VersenyM != 0 ? _oklevel.VersenyM.ToString( ) : " ";
                    current[57] += Environment.NewLine;
                    current[57] += igazitasSeged( _oklevel.VersenyI );

                    current[58] = _oklevel.VersenySorozatX != 0 ? "Xpos: " + _oklevel.VersenySorozatX.ToString( ) : " ";
                    current[58] += Environment.NewLine;
                    current[58] += _oklevel.VersenySorozatY != 0 ? "Ypos: " + _oklevel.VersenySorozatY.ToString( ) : " ";
                    current[58] += Environment.NewLine;
                    current[58] += _oklevel.VersenySorozatH != 0 ? "Hossz: " + _oklevel.VersenySorozatH.ToString( ) : " ";
                    current[58] += Environment.NewLine;
                    current[58] += formatSeged( _oklevel.VersenySorozatF );
                    current[58] += Environment.NewLine;
                    current[58] += _oklevel.VersenySorozatM != 0 ? _oklevel.VersenySorozatM.ToString( ) : " ";
                    current[58] += Environment.NewLine;
                    current[58] += igazitasSeged( _oklevel.VersenySorozatI );

                    current[59] = _oklevel.HelyezesX != 0 ? "Xpos: " + _oklevel.HelyezesX.ToString( ) : " ";
                    current[59] += Environment.NewLine;
                    current[59] += _oklevel.HelyezesY != 0 ? "Ypos: " + _oklevel.HelyezesY.ToString( ) : " ";
                    current[59] += Environment.NewLine;
                    current[59] += _oklevel.HelyezesH != 0 ? "Hossz: " + _oklevel.HelyezesH.ToString( ) : " ";
                    current[59] += Environment.NewLine;
                    current[59] += formatSeged( _oklevel.HelyezesF );
                    current[59] += Environment.NewLine;
                    current[59] += _oklevel.HelyezesM != 0 ? _oklevel.HelyezesM.ToString( ) : " ";
                    current[59] += Environment.NewLine;
                    current[59] += igazitasSeged( _oklevel.HelyezesI );

                    current[60] = _oklevel.InduloX != 0 ? "Xpos: " + _oklevel.InduloX.ToString( ) : " ";
                    current[60] += Environment.NewLine;
                    current[60] += _oklevel.InduloY != 0 ? "Ypos: " + _oklevel.InduloY.ToString( ) : " ";
                    current[60] += Environment.NewLine;
                    current[60] += _oklevel.InduloH != 0 ? "Hossz: " + _oklevel.InduloH.ToString( ) : " ";
                    current[60] += Environment.NewLine;
                    current[60] += formatSeged( _oklevel.InduloF );
                    current[60] += Environment.NewLine;
                    current[60] += _oklevel.InduloM != 0 ? _oklevel.InduloM.ToString( ) : " ";
                    current[60] += Environment.NewLine;
                    current[60] += igazitasSeged( _oklevel.InduloI );

                    current[61] = _oklevel.EgyesuletX != 0 ? "Xpos: " + _oklevel.EgyesuletX.ToString( ) : " ";
                    current[61] += Environment.NewLine;
                    current[61] += _oklevel.EgyesuletY != 0 ? "Ypos: " + _oklevel.EgyesuletY.ToString( ) : " ";
                    current[61] += Environment.NewLine;
                    current[61] += _oklevel.EgyesuletH != 0 ? "Hossz: " + _oklevel.EgyesuletH.ToString( ) : " ";
                    current[61] += Environment.NewLine;
                    current[61] += formatSeged( _oklevel.EgyesuletF );
                    current[61] += Environment.NewLine;
                    current[61] += _oklevel.EgyesuletM != 0 ? _oklevel.EgyesuletM.ToString( ) : " ";
                    current[61] += Environment.NewLine;
                    current[61] += igazitasSeged( _oklevel.EgyesuletI );

                    current[62] = _oklevel.IjtipusX != 0 ? "Xpos: " + _oklevel.IjtipusX.ToString( ) : " ";
                    current[62] += Environment.NewLine;
                    current[62] += _oklevel.IjtipusY != 0 ? "Ypos: " + _oklevel.IjtipusY.ToString( ) : " ";
                    current[62] += Environment.NewLine;
                    current[62] += _oklevel.IjtipusH != 0 ? "Hossz: " + _oklevel.IjtipusH.ToString( ) : " ";
                    current[62] += Environment.NewLine;
                    current[62] += formatSeged( _oklevel.IjtipusF );
                    current[62] += Environment.NewLine;
                    current[62] += _oklevel.IjtipusM != 0 ? _oklevel.IjtipusM.ToString( ) : " ";
                    current[62] += Environment.NewLine;
                    current[62] += igazitasSeged( _oklevel.IjtipusI );

                    current[63] = _oklevel.KorosztalyX != 0 ? "Xpos: " + _oklevel.KorosztalyX.ToString( ) : " ";
                    current[63] += Environment.NewLine;
                    current[63] += _oklevel.KorosztalyY != 0 ? "Ypos: " + _oklevel.KorosztalyY.ToString( ) : " ";
                    current[63] += Environment.NewLine;
                    current[63] += _oklevel.KorosztalyH != 0 ? "Hossz: " + _oklevel.KorosztalyH.ToString( ) : " ";
                    current[63] += Environment.NewLine;
                    current[63] += formatSeged( _oklevel.KorosztalyF );
                    current[63] += Environment.NewLine;
                    current[63] += _oklevel.KorosztalyM != 0 ? _oklevel.KorosztalyM.ToString( ) : " ";
                    current[63] += Environment.NewLine;
                    current[63] += igazitasSeged( _oklevel.KorosztalyI );

                    current[64] = _oklevel.InduloNemeX != 0 ? "Xpos: " + _oklevel.InduloNemeX.ToString( ) : " ";
                    current[64] += Environment.NewLine;
                    current[64] += _oklevel.InduloNemeY != 0 ? "Ypos: " + _oklevel.InduloNemeY.ToString( ) : " ";
                    current[64] += Environment.NewLine;
                    current[64] += _oklevel.InduloNemeH != 0 ? "Hossz: " + _oklevel.InduloNemeH.ToString( ) : " ";
                    current[64] += Environment.NewLine;
                    current[64] += formatSeged( _oklevel.InduloNemeF );
                    current[64] += Environment.NewLine;
                    current[64] += _oklevel.InduloNemeM != 0 ? _oklevel.InduloNemeM.ToString( ) : " ";
                    current[64] += Environment.NewLine;
                    current[64] += igazitasSeged( _oklevel.InduloNemeI );

                    current[65] = _oklevel.DatumX != 0 ? "Xpos: " + _oklevel.DatumX.ToString( ) : " ";
                    current[65] += Environment.NewLine;
                    current[65] += _oklevel.DatumY != 0 ? "Ypos: " + _oklevel.DatumY.ToString( ) : " ";
                    current[65] += Environment.NewLine;
                    current[65] += _oklevel.DatumH != 0 ? "Hossz: " + _oklevel.DatumH.ToString( ) : " ";
                    current[65] += Environment.NewLine;
                    current[65] += formatSeged( _oklevel.DatumF );
                    current[65] += Environment.NewLine;
                    current[65] += _oklevel.DatumM != 0 ? _oklevel.DatumM.ToString( ) : " ";
                    current[65] += Environment.NewLine;
                    current[65] += igazitasSeged( _oklevel.DatumI );
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
                BetuTipus = data.Rows[table.SelectedRows[0].Index][++c].ToString(),

                VersenyX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenyM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenyI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                VersenySorozatX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                VersenySorozatM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                VersenySorozatI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                HelyezesX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                HelyezesM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                HelyezesI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                InduloX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                EgyesuletX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                EgyesuletM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                EgyesuletI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                IjtipusX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                IjtipusM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                IjtipusI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                KorosztalyX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                KorosztalyM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                KorosztalyI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                InduloNemeX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeF = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),
                InduloNemeM = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                InduloNemeI = (data.Rows[table.SelectedRows[0].Index][++c]).ToString(),

                DatumX = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumY = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
                DatumH = Convert.ToInt32(data.Rows[table.SelectedRows[0].Index][++c]),
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

