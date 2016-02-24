using System;
using System.Drawing;
using System.Windows.Forms;

//NOTE(mate): sablon típusa nem módosítható !!!
namespace Íjász {
    public sealed class Form_Oklevel : Form {
        bool modositas;  
        
        #region Controls
        TextBox txtAzonosito;
        ComboBox cboTipus;

        TextBox txtVersenyX;
        TextBox txtVersenyY;
        TextBox txtVersenyH;
        CheckBox chkVersenyBold;
        CheckBox chkVersenyItalic;
        CheckBox chkVersenyLeft;
        CheckBox chkVersenyRight;
        CheckBox chkVersenyMiddle;
        ComboBox cboVersenyB;
        TextBox txtVersenyM;

        TextBox txtVersenySorozatX;
        TextBox txtVersenySorozatY;
        TextBox txtVersenySorozatH;
        CheckBox chkVersenySorozatBold;
        CheckBox chkVersenySorozatItalic;
        CheckBox chkVersenySorozatLeft;
        CheckBox chkVersenySorozatRight;
        CheckBox chkVersenySorozatMiddle;
        ComboBox cboVersenySorozatB;
        TextBox txtVersenySorozatM;

        TextBox txtHelyezesX;
        TextBox txtHelyezesY;
        TextBox txtHelyezesH;
        CheckBox chkHelyezesBold;
        CheckBox chkHelyezesItalic;
        CheckBox chkHelyezesLeft;
        CheckBox chkHelyezesRight;
        CheckBox chkHelyezesMiddle;
        ComboBox cboHelyezesB;
        TextBox txtHelyezesM;

        TextBox txtInduloX;
        TextBox txtInduloY;
        TextBox txtInduloH;
        CheckBox chkInduloBold;
        CheckBox chkInduloItalic;
        CheckBox chkInduloLeft;
        CheckBox chkInduloRight;
        CheckBox chkInduloMiddle;
        ComboBox cboInduloB;
        TextBox txtInduloM;

        TextBox txtEgyesuletX;
        TextBox txtEgyesuletY;
        TextBox txtEgyesuletH;
        CheckBox chkEgyesuletBold;
        CheckBox chkEgyesuletItalic;
        CheckBox chkEgyesuletLeft;
        CheckBox chkEgyesuletRight;
        CheckBox chkEgyesuletMiddle;
        ComboBox cboEgyesuletB;
        TextBox txtEgyesuletM;

        TextBox txtIjtipusX;
        TextBox txtIjtipusY;
        TextBox txtIjtipusH;
        CheckBox chkIjtipusBold;
        CheckBox chkIjtipusItalic;
        CheckBox chkIjtipusLeft;
        CheckBox chkIjtipusRight;
        CheckBox chkIjtipusMiddle;
        ComboBox cboIjtipusB;
        TextBox txtIjtipusM;

        TextBox txtKorosztalyX;
        TextBox txtKorosztalyY;
        TextBox txtKorosztalyH;
        CheckBox chkKorosztalyBold;
        CheckBox chkKorosztalyItalic;
        CheckBox chkKorosztalyLeft;
        CheckBox chkKorosztalyRight;
        CheckBox chkKorosztalyMiddle;
        ComboBox cboKorosztalyB;
        TextBox txtKorosztalyM;

        TextBox txtInduloNemeX;
        TextBox txtInduloNemeY;
        TextBox txtInduloNemeH;
        CheckBox chkInduloNemeBold;
        CheckBox chkInduloNemeItalic;
        CheckBox chkInduloNemeLeft;
        CheckBox chkInduloNemeRight;
        CheckBox chkInduloNemeMiddle;
        ComboBox cboInduloNemeB;
        TextBox txtInduloNemeM;

        TextBox txtDatumX;
        TextBox txtDatumY;
        TextBox txtDatumH;
        CheckBox chkDatumBold;
        CheckBox chkDatumItalic;
        CheckBox chkDatumLeft;
        CheckBox chkDatumRight;
        CheckBox chkDatumMiddle;
        ComboBox cboDatumB;
        TextBox txtDatumM;
        #endregion

        public void LoadTestData( ) {
            cboTipus.SelectedIndex = 0;

            chkVersenyBold.CheckState = CheckState.Checked;
            chkVersenyItalic.CheckState = CheckState.Unchecked;
            //chkVersenyLeft.CheckState = CheckState.Checked;
            //chkVersenyRight.CheckState = CheckState.Unchecked;
            //chkVersenyMiddle.CheckState = CheckState.Unchecked;
            chkVersenySorozatBold.CheckState = CheckState.Checked;
            chkVersenySorozatItalic.CheckState = CheckState.Unchecked;
            chkVersenySorozatLeft.CheckState = CheckState.Checked;
            chkVersenySorozatRight.CheckState = CheckState.Unchecked;
            chkVersenySorozatMiddle.CheckState = CheckState.Unchecked;
            chkHelyezesBold.CheckState = CheckState.Checked;
            chkHelyezesItalic.CheckState = CheckState.Unchecked;
            chkHelyezesLeft.CheckState = CheckState.Checked;
            chkHelyezesRight.CheckState = CheckState.Unchecked;
            chkHelyezesMiddle.CheckState = CheckState.Unchecked;
            chkInduloBold.CheckState = CheckState.Checked;
            chkInduloItalic.CheckState = CheckState.Unchecked;
            chkInduloLeft.CheckState = CheckState.Checked;
            chkInduloRight.CheckState = CheckState.Unchecked;
            chkInduloMiddle.CheckState = CheckState.Unchecked;
            chkEgyesuletBold.CheckState = CheckState.Checked;
            chkEgyesuletItalic.CheckState = CheckState.Unchecked;
            chkEgyesuletLeft.CheckState = CheckState.Checked;
            chkEgyesuletRight.CheckState = CheckState.Unchecked;
            chkEgyesuletMiddle.CheckState = CheckState.Unchecked;
            chkIjtipusBold.CheckState = CheckState.Checked;
            chkIjtipusItalic.CheckState = CheckState.Unchecked;
            chkIjtipusLeft.CheckState = CheckState.Checked;
            chkIjtipusRight.CheckState = CheckState.Unchecked;
            chkIjtipusMiddle.CheckState = CheckState.Unchecked;
            chkKorosztalyBold.CheckState = CheckState.Checked;
            chkKorosztalyItalic.CheckState = CheckState.Unchecked;
            chkKorosztalyLeft.CheckState = CheckState.Checked;
            chkKorosztalyRight.CheckState = CheckState.Unchecked;
            chkKorosztalyMiddle.CheckState = CheckState.Unchecked;
            chkInduloNemeBold.CheckState = CheckState.Checked;
            chkInduloNemeItalic.CheckState = CheckState.Unchecked;
            chkInduloNemeLeft.CheckState = CheckState.Checked;
            chkInduloNemeRight.CheckState = CheckState.Unchecked;
            chkInduloNemeMiddle.CheckState = CheckState.Unchecked;
            chkDatumBold.CheckState = CheckState.Checked;
            chkDatumItalic.CheckState = CheckState.Unchecked;
            chkDatumLeft.CheckState = CheckState.Checked;
            chkDatumRight.CheckState = CheckState.Unchecked;
            chkDatumMiddle.CheckState = CheckState.Unchecked;

            int c = -1;
            txtAzonosito.Text = ( ++c ).ToString( );
            txtVersenyX.Text = ( ++c ).ToString( );
            txtVersenyY.Text = ( ++c ).ToString( );
            txtVersenyH.Text = ( ++c ).ToString( );
            txtVersenyM.Text = ( ++c ).ToString( );
            txtVersenySorozatX.Text = ( ++c ).ToString( );
            txtVersenySorozatY.Text = ( ++c ).ToString( );
            txtVersenySorozatH.Text = ( ++c ).ToString( );
            txtVersenySorozatM.Text = ( ++c ).ToString( );
            txtHelyezesX.Text = ( ++c ).ToString( );
            txtHelyezesY.Text = ( ++c ).ToString( );
            txtHelyezesH.Text = ( ++c ).ToString( );
            txtHelyezesM.Text = ( ++c ).ToString( );
            txtInduloX.Text = ( ++c ).ToString( );
            txtInduloY.Text = ( ++c ).ToString( );
            txtInduloH.Text = ( ++c ).ToString( );
            txtInduloM.Text = ( ++c ).ToString( );
            txtEgyesuletX.Text = ( ++c ).ToString( );
            txtEgyesuletY.Text = ( ++c ).ToString( );
            txtEgyesuletH.Text = ( ++c ).ToString( );
            txtEgyesuletM.Text = ( ++c ).ToString( );
            txtIjtipusX.Text = ( ++c ).ToString( );
            txtIjtipusY.Text = ( ++c ).ToString( );
            txtIjtipusH.Text = ( ++c ).ToString( );
            txtIjtipusM.Text = ( ++c ).ToString( );
            txtKorosztalyX.Text = ( ++c ).ToString( );
            txtKorosztalyY.Text = ( ++c ).ToString( );
            txtKorosztalyH.Text = ( ++c ).ToString( );
            txtKorosztalyM.Text = ( ++c ).ToString( );
            //txtInduloNemeX.Text = ( ++c ).ToString( );
            //txtInduloNemeY.Text = ( ++c ).ToString( );
            //txtInduloNemeH.Text = ( ++c ).ToString( );
            //txtInduloNemeM.Text = ( ++c ).ToString( );
            txtDatumX.Text = ( ++c ).ToString( );
            txtDatumY.Text = ( ++c ).ToString( );
            txtDatumH.Text = ( ++c ).ToString( );
            txtDatumM.Text = ( ++c ).ToString( );
        }

        //NOTE(mate): új sablon
        public Form_Oklevel( ) {
            InitializeForm( );
            InitializeContent( );
            InitializeData( );
        }

        public Form_Oklevel( Oklevel _Oklevel ) {
            InitializeForm( );
            InitializeContent( );
            InitializeData( _Oklevel );
        }

        //NOTE(mate): új sablon
        private void InitializeForm( ) {
            Text = "Új sablon szerkesztő";
            ClientSize = new Size( 850 , 500 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void InitializeContent( ) {
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            int labelPos = -1;

            int columnPos1 = 7;
            int columnPos2 = columnPos1 + 6;
            int columnPos3 = columnPos2 + 6;
            int columnPos4 = columnPos3 + 2;
            int columnPos5 = columnPos4 + 2;
            int columnPos6 = columnPos5 + 8;
            int columnPos7 = columnPos6 + 2;
            int columnPos8 = columnPos7 + 2;
            int columnPos9 = columnPos8 + 2;
            int columnPos10 = columnPos9 + 7;
            
            Size textBoxSize = new Size(64 - 16,28);

            #region Labels

            Label lblAzonosito = new Label {
                Text = "Azonosító:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblTipus = new Label {
                Text = "Típus:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblFelirat = new Label {
                Text = "                         Szélesség(mm)          Magasság(mm)       Hossz(mm)                      Format                 Méret                          Igazítás                         Betűtípus",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };

            Label lblVerseny = new Label {
                Text = "Verseny:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblVersenySorozat = new Label {
                Text = "Versenysorozat:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyezes = new Label {
                Text = "Helyezés:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblIndulo= new Label {
                Text = "Induló:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblEgyesulet= new Label {
                Text = "Egyesület:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblIjtipus= new Label {
                Text = "Íjtípus:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblKorosztaly= new Label {
                Text = "Korosztály:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblInduloNeme= new Label {
                Text = "Induló neme:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblDatum= new Label {
                Text = "Dátum:",
                Location = new Point( 16, (labelPos += 2) * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Controls.Add( lblAzonosito );
            Controls.Add( lblTipus );
            Controls.Add( lblFelirat );
            Controls.Add( lblVerseny );
            Controls.Add( lblVersenySorozat );
            Controls.Add( lblHelyezes );
            Controls.Add( lblIndulo );
            Controls.Add( lblEgyesulet );
            Controls.Add( lblIjtipus );
            Controls.Add( lblKorosztaly );
            Controls.Add( lblInduloNeme );
            Controls.Add( lblDatum );
            #endregion

            txtAzonosito = new TextBox {
                Location = new Point( columnPos1 * 16, lblAzonosito.Location.Y - 8 )
            };
            cboTipus = new ComboBox {
                Location = new Point( columnPos1 * 16, lblTipus.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboTipus.Items.Add( "Verseny" );
            cboTipus.Items.Add( "Versenysorozat" );
            cboTipus.SelectedIndexChanged += Cbo_Tipus_SelectedIndexChanged;

            Controls.Add( txtAzonosito );
            Controls.Add( cboTipus );

            #region Verseny

            txtVersenyX = new TextBox {
                Location = new Point( columnPos1 * 16, lblVerseny.Location.Y - 4 ),
                Size = textBoxSize
            };
            txtVersenyY = new TextBox {
                Location = new Point( columnPos2 * 16, lblVerseny.Location.Y - 4 ),
                Size = textBoxSize
            };
            txtVersenyH = new TextBox {
                Location = new Point( columnPos3 * 16, lblVerseny.Location.Y - 4 ),
                Size = textBoxSize
            };
            chkVersenyBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblVerseny.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenyItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblVerseny.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtVersenyM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblVerseny.Location.Y - 4 )
            };
            chkVersenyLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblVerseny.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenyMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblVerseny.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenyRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblVerseny.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            cboVersenyB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblVerseny.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboVersenyB.Items.Add("TODO");
            Controls.Add(cboVersenyB);

            Controls.Add( txtVersenyX );
            Controls.Add( txtVersenyY );
            Controls.Add( txtVersenyH );
            Controls.Add( chkVersenyBold );
            Controls.Add( chkVersenyItalic );
            Controls.Add( txtVersenyM );
            Controls.Add( chkVersenyLeft );
            Controls.Add( chkVersenyMiddle );
            Controls.Add( chkVersenyRight );
            #endregion

            #region VersenySorozat
            txtVersenySorozatX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblVersenySorozat.Location.Y - 4 )
            };
            txtVersenySorozatY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblVersenySorozat.Location.Y - 4 )
            };
            txtVersenySorozatH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblVersenySorozat.Location.Y - 4 )
            };
            chkVersenySorozatBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblVersenySorozat.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenySorozatItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblVersenySorozat.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtVersenySorozatM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblVersenySorozat.Location.Y - 4 )
            };
            chkVersenySorozatLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblVersenySorozat.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenySorozatMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblVersenySorozat.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkVersenySorozatRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblVersenySorozat.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };

            cboVersenySorozatB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblVersenySorozat.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboVersenySorozatB.Items.Add("TODO");
            Controls.Add(cboVersenySorozatB);

            Controls.Add( txtVersenySorozatX );
            Controls.Add( txtVersenySorozatY );
            Controls.Add( txtVersenySorozatH );
            Controls.Add( chkVersenySorozatBold );
            Controls.Add( chkVersenySorozatItalic );
            Controls.Add( txtVersenySorozatM );
            Controls.Add( chkVersenySorozatLeft );
            Controls.Add( chkVersenySorozatMiddle );
            Controls.Add( chkVersenySorozatRight );
            #endregion

            #region Helyezes
            txtHelyezesX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblHelyezes.Location.Y - 4 )
            };
            txtHelyezesY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblHelyezes.Location.Y - 4 )
            };
            txtHelyezesH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblHelyezes.Location.Y - 4 )
            };
            chkHelyezesBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblHelyezes.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkHelyezesItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblHelyezes.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtHelyezesM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblHelyezes.Location.Y - 4 )
            };
            chkHelyezesLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblHelyezes.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkHelyezesMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblHelyezes.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkHelyezesRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblHelyezes.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            cboHelyezesB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblHelyezes.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboHelyezesB.Items.Add("TODO");
            Controls.Add(cboHelyezesB);

            Controls.Add( txtHelyezesX );
            Controls.Add( txtHelyezesY );
            Controls.Add( txtHelyezesH );
            Controls.Add( chkHelyezesBold );
            Controls.Add( chkHelyezesItalic );
            Controls.Add( txtHelyezesM );
            Controls.Add( chkHelyezesLeft );
            Controls.Add( chkHelyezesMiddle );
            Controls.Add( chkHelyezesRight );
            #endregion

            #region Induló
            txtInduloX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblIndulo.Location.Y - 4 )
            };
            txtInduloY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblIndulo.Location.Y - 4 )
            };
            txtInduloH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblIndulo.Location.Y - 4 )
            };
            chkInduloBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblIndulo.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblIndulo.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtInduloM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblIndulo.Location.Y - 4 )
            };
            chkInduloLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblIndulo.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblIndulo.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblIndulo.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            cboInduloB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblIndulo.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboInduloB.Items.Add("TODO");
            Controls.Add(cboInduloB);

            Controls.Add( txtInduloX );
            Controls.Add( txtInduloY );
            Controls.Add( txtInduloH );
            Controls.Add( chkInduloBold );
            Controls.Add( chkInduloItalic );
            Controls.Add( txtInduloM );
            Controls.Add( chkInduloLeft );
            Controls.Add( chkInduloMiddle );
            Controls.Add( chkInduloRight );
            #endregion

            #region Egyesület
            txtEgyesuletX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblEgyesulet.Location.Y - 4 )
            };
            txtEgyesuletY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblEgyesulet.Location.Y - 4 )
            };
            txtEgyesuletH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblEgyesulet.Location.Y - 4 )
            };
            chkEgyesuletBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblEgyesulet.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkEgyesuletItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblEgyesulet.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtEgyesuletM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblEgyesulet.Location.Y - 4 )
            };
            chkEgyesuletLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblEgyesulet.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkEgyesuletMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblEgyesulet.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkEgyesuletRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblEgyesulet.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };

            cboEgyesuletB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblEgyesulet.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboEgyesuletB.Items.Add("TODO");
            Controls.Add(cboEgyesuletB);

            Controls.Add( txtEgyesuletX );
            Controls.Add( txtEgyesuletY );
            Controls.Add( txtEgyesuletH );
            Controls.Add( chkEgyesuletBold );
            Controls.Add( chkEgyesuletItalic );
            Controls.Add( txtEgyesuletM );
            Controls.Add( chkEgyesuletLeft );
            Controls.Add( chkEgyesuletMiddle );
            Controls.Add( chkEgyesuletRight );
            #endregion

            #region Íjtípus
            txtIjtipusX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblIjtipus.Location.Y - 4 )
            };
            txtIjtipusY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblIjtipus.Location.Y - 4 )
            };
            txtIjtipusH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblIjtipus.Location.Y - 4 )
            };
            chkIjtipusBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblIjtipus.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkIjtipusItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblIjtipus.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtIjtipusM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblIjtipus.Location.Y - 4 )
            };
            chkIjtipusLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblIjtipus.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkIjtipusMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblIjtipus.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkIjtipusRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblIjtipus.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };

            cboIjtipusB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblIjtipus.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboIjtipusB.Items.Add("TODO");
            Controls.Add(cboIjtipusB);

            Controls.Add( txtIjtipusX );
            Controls.Add( txtIjtipusY );
            Controls.Add( txtIjtipusH );
            Controls.Add( chkIjtipusBold );
            Controls.Add( chkIjtipusItalic );
            Controls.Add( txtIjtipusM );
            Controls.Add( chkIjtipusLeft );
            Controls.Add( chkIjtipusMiddle );
            Controls.Add( chkIjtipusRight );
            #endregion

            #region Koroszály
            txtKorosztalyX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblKorosztaly.Location.Y - 4 )
            };
            txtKorosztalyY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblKorosztaly.Location.Y - 4 )
            };
            txtKorosztalyH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblKorosztaly.Location.Y - 4 )
            };
            chkKorosztalyBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblKorosztaly.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkKorosztalyItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblKorosztaly.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtKorosztalyM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblKorosztaly.Location.Y - 4 )
            };
            chkKorosztalyLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblKorosztaly.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkKorosztalyMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblKorosztaly.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkKorosztalyRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblKorosztaly.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            cboKorosztalyB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblKorosztaly.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboKorosztalyB.Items.Add("TODO");
            Controls.Add(cboKorosztalyB);

            Controls.Add( txtKorosztalyX );
            Controls.Add( txtKorosztalyY );
            Controls.Add( txtKorosztalyH );
            Controls.Add( chkKorosztalyBold );
            Controls.Add( chkKorosztalyItalic );
            Controls.Add( txtKorosztalyM );
            Controls.Add( chkKorosztalyLeft );
            Controls.Add( chkKorosztalyMiddle );
            Controls.Add( chkKorosztalyRight );
            #endregion

            #region Indulóneme
            txtInduloNemeX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblInduloNeme.Location.Y - 4 )
            };
            txtInduloNemeY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblInduloNeme.Location.Y - 4 )
            };
            txtInduloNemeH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblInduloNeme.Location.Y - 4 )
            };
            chkInduloNemeBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblInduloNeme.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloNemeItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblInduloNeme.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtInduloNemeM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblInduloNeme.Location.Y - 4 )
            };
            chkInduloNemeLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblInduloNeme.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloNemeMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblInduloNeme.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkInduloNemeRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblInduloNeme.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            Controls.Add( txtInduloNemeX );
            Controls.Add( txtInduloNemeY );
            Controls.Add( txtInduloNemeH );
            Controls.Add( chkInduloNemeBold );
            Controls.Add( chkInduloNemeItalic );
            Controls.Add( txtInduloNemeM );
            Controls.Add( chkInduloNemeLeft );
            Controls.Add( chkInduloNemeMiddle );
            Controls.Add( chkInduloNemeRight );

            cboInduloNemeB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblInduloNeme.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboInduloNemeB.Items.Add("TODO");
            Controls.Add(cboInduloNemeB  );


            #endregion

            #region Dátum
            txtDatumX = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos1 * 16, lblDatum.Location.Y - 4 )
            };
            txtDatumY = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos2 * 16, lblDatum.Location.Y - 4 )
            };
            txtDatumH = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos3 * 16, lblDatum.Location.Y - 4 )
            };
            chkDatumBold = new CheckBox {
                Text = "B",
                Location = new Point( columnPos4 * 16, lblDatum.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkDatumItalic = new CheckBox {
                Text = "I",
                Location = new Point( columnPos5 * 16, lblDatum.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            txtDatumM = new TextBox {
                Size = textBoxSize,
                Location = new Point( columnPos6 * 16, lblDatum.Location.Y - 4 )
            };
            chkDatumLeft = new CheckBox {
                Text = "L",
                Location = new Point( columnPos7 * 16, lblDatum.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkDatumMiddle = new CheckBox {
                Text = "M",
                Location = new Point( columnPos8 * 16, lblDatum.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            chkDatumRight = new CheckBox {
                Text = "R",
                Location = new Point( columnPos9 * 16, lblDatum.Location.Y - 4 ),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                FlatStyle = FlatStyle.Flat,
                CheckState = CheckState.Unchecked
            };
            cboDatumB = new ComboBox {
                Location = new Point( columnPos10 * 16, lblDatum.Location.Y - 8 ),
                Size = new Size(128,16),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cboDatumB.Items.Add("TODO");
            Controls.Add(cboDatumB);


            Controls.Add( txtDatumX );
            Controls.Add( txtDatumY );
            Controls.Add( txtDatumH );
            Controls.Add( chkDatumBold );
            Controls.Add( chkDatumItalic );
            Controls.Add( txtDatumM );
            Controls.Add( chkDatumLeft );
            Controls.Add( chkDatumMiddle );
            Controls.Add( chkDatumRight );
            #endregion

            #region CheckBox EventHandlers
            chkVersenyLeft.Click += chkAll_Click;
            chkVersenyRight.Click += chkAll_Click;
            chkVersenyMiddle.Click += chkAll_Click;
            chkVersenySorozatLeft.Click += chkAll_Click;
            chkVersenySorozatRight.Click += chkAll_Click;
            chkVersenySorozatMiddle.Click += chkAll_Click;
            chkHelyezesLeft.Click += chkAll_Click;
            chkHelyezesRight.Click += chkAll_Click;
            chkHelyezesMiddle.Click += chkAll_Click;
            chkInduloLeft.Click += chkAll_Click;
            chkInduloRight.Click += chkAll_Click;
            chkInduloMiddle.Click += chkAll_Click;
            chkEgyesuletLeft.Click += chkAll_Click;
            chkEgyesuletRight.Click += chkAll_Click;
            chkEgyesuletMiddle.Click += chkAll_Click;
            chkIjtipusLeft.Click += chkAll_Click;
            chkIjtipusRight.Click += chkAll_Click;
            chkIjtipusMiddle.Click += chkAll_Click;
            chkKorosztalyLeft.Click += chkAll_Click;
            chkKorosztalyRight.Click += chkAll_Click;
            chkKorosztalyMiddle.Click += chkAll_Click;
            chkInduloNemeLeft.Click += chkAll_Click;
            chkInduloNemeRight.Click += chkAll_Click;
            chkInduloNemeMiddle.Click += chkAll_Click;
            chkDatumLeft.Click += chkAll_Click;
            chkDatumRight.Click += chkAll_Click;
            chkDatumMiddle.Click += chkAll_Click;
            #endregion

            Button btnRendben = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Rendben",
                Location = new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                Size = new Size( 96, 32 )
            };
            btnRendben.Click += btnRendben_Click;
            Controls.Add( btnRendben );
            //LoadTestData( );
        }
        private void InitializeData(  ) {
            modositas = false;

        }

        private void InitializeData( Oklevel oklevel ) {
            modositas = true;

            txtAzonosito.Text = oklevel.Azonosito;
            txtAzonosito.Enabled = false;

            cboTipus.Text = oklevel.Tipus;

            txtVersenyX.Text = oklevel.VersenyX == 0 ? "" : oklevel.VersenyX.ToString( );
            txtVersenyY.Text = oklevel.VersenyY == 0 ? "" : oklevel.VersenyY.ToString( );
            txtVersenyH.Text = oklevel.VersenyH == 0 ? "" : oklevel.VersenyH.ToString( );
            chkVersenyBold.Checked = ( ( oklevel.VersenyF == "B" ) || ( oklevel.VersenyF == "2" ) );
            chkVersenyItalic.Checked = ( ( oklevel.VersenyF == "I" ) || ( oklevel.VersenyF == "2" ) );
            chkVersenyLeft.Checked = ( oklevel.VersenyI == "L" );
            chkVersenyRight.Checked = ( oklevel.VersenyI == "R" );
            chkVersenyMiddle.Checked = ( oklevel.VersenyI == "M" );
            txtVersenyM.Text = oklevel.VersenyM == 0 ? "" : oklevel.VersenyM.ToString( );
            cboVersenyB.Text = oklevel.VersenyB;

            txtVersenySorozatX.Text = oklevel.VersenySorozatX == 0 ? "" : oklevel.VersenySorozatX.ToString( );
            txtVersenySorozatY.Text = oklevel.VersenySorozatY == 0 ? "" : oklevel.VersenySorozatY.ToString( );
            txtVersenySorozatH.Text = oklevel.VersenySorozatH == 0 ? "" : oklevel.VersenySorozatH.ToString( );
            chkVersenySorozatBold.Checked = ( ( oklevel.VersenySorozatF == "B" ) || ( oklevel.VersenySorozatF == "2" ) );
            chkVersenySorozatItalic.Checked = ( ( oklevel.VersenySorozatF == "I" ) || ( oklevel.VersenySorozatF == "2" ) );
            chkVersenySorozatLeft.Checked = ( oklevel.VersenySorozatI == "L" );
            chkVersenySorozatRight.Checked = ( oklevel.VersenySorozatI == "R" );
            chkVersenySorozatMiddle.Checked = ( oklevel.VersenySorozatI == "M" );
            txtVersenySorozatM.Text = oklevel.VersenySorozatM == 0 ? "" : oklevel.VersenySorozatM.ToString( );
            cboVersenySorozatB.Text = oklevel.VersenySorozatB;

            txtHelyezesX.Text = oklevel.HelyezesX == 0 ? "" : oklevel.HelyezesX.ToString( );
            txtHelyezesY.Text = oklevel.HelyezesY == 0 ? "" : oklevel.HelyezesY.ToString( );
            txtHelyezesH.Text = oklevel.HelyezesH == 0 ? "" : oklevel.HelyezesH.ToString( );
            chkHelyezesBold.Checked = ( ( oklevel.HelyezesF == "B" ) || ( oklevel.HelyezesF == "2" ) );
            chkHelyezesItalic.Checked = ( ( oklevel.HelyezesF == "I" ) || ( oklevel.HelyezesF == "2" ) );
            chkHelyezesLeft.Checked = ( oklevel.HelyezesI == "L" );
            chkHelyezesRight.Checked = ( oklevel.HelyezesI == "R" );
            chkHelyezesMiddle.Checked = ( oklevel.HelyezesI == "M" );
            txtHelyezesM.Text = oklevel.HelyezesM == 0 ? "" : oklevel.HelyezesM.ToString( );
            cboHelyezesB.Text = oklevel.HelyezesB;

            txtInduloX.Text = oklevel.InduloX == 0 ? "" : oklevel.InduloX.ToString( );
            txtInduloY.Text = oklevel.InduloY == 0 ? "" : oklevel.InduloY.ToString( );
            txtInduloH.Text = oklevel.InduloH == 0 ? "" : oklevel.InduloH.ToString( );
            chkInduloBold.Checked = ( ( oklevel.InduloF == "B" ) || ( oklevel.InduloF == "2" ) );
            chkInduloItalic.Checked = ( ( oklevel.InduloF == "I" ) || ( oklevel.InduloF == "2" ) );
            chkInduloLeft.Checked = ( oklevel.InduloI == "L" );
            chkInduloRight.Checked = ( oklevel.InduloI == "R" );
            chkInduloMiddle.Checked = ( oklevel.InduloI == "M" );
            txtInduloM.Text = oklevel.InduloM == 0 ? "" : oklevel.InduloM.ToString( );
            cboInduloB.Text = oklevel.InduloB;

            txtEgyesuletX.Text = oklevel.EgyesuletX == 0 ? "" : oklevel.EgyesuletX.ToString( );
            txtEgyesuletY.Text = oklevel.EgyesuletY == 0 ? "" : oklevel.EgyesuletY.ToString( );
            txtEgyesuletH.Text = oklevel.EgyesuletH == 0 ? "" : oklevel.EgyesuletH.ToString( );
            chkEgyesuletBold.Checked = ( ( oklevel.EgyesuletF == "B" ) || ( oklevel.EgyesuletF == "2" ) );
            chkEgyesuletItalic.Checked = ( ( oklevel.EgyesuletF == "I" ) || ( oklevel.EgyesuletF == "2" ) );
            chkEgyesuletLeft.Checked = ( oklevel.EgyesuletI == "L" );
            chkEgyesuletRight.Checked = ( oklevel.EgyesuletI == "R" );
            chkEgyesuletMiddle.Checked = ( oklevel.EgyesuletI == "M" );
            txtEgyesuletM.Text = oklevel.EgyesuletM == 0 ? "" : oklevel.EgyesuletM.ToString( );
            cboEgyesuletB.Text = oklevel.EgyesuletB;

            txtIjtipusX.Text = oklevel.IjtipusX == 0 ? "" : oklevel.IjtipusX.ToString( );
            txtIjtipusY.Text = oklevel.IjtipusY == 0 ? "" : oklevel.IjtipusY.ToString( );
            txtIjtipusH.Text = oklevel.IjtipusH == 0 ? "" : oklevel.IjtipusH.ToString( );
            chkIjtipusBold.Checked = ( ( oklevel.IjtipusF == "B" ) || ( oklevel.IjtipusF == "2" ) );
            chkIjtipusItalic.Checked = ( ( oklevel.IjtipusF == "I" ) || ( oklevel.IjtipusF == "2" ) );
            chkIjtipusLeft.Checked = ( oklevel.IjtipusI == "L" );
            chkIjtipusRight.Checked = ( oklevel.IjtipusI == "R" );
            chkIjtipusMiddle.Checked = ( oklevel.IjtipusI == "M" );
            txtIjtipusM.Text = oklevel.IjtipusM == 0 ? "" : oklevel.IjtipusM.ToString( );
            cboEgyesuletB.Text = oklevel.EgyesuletB;

            txtKorosztalyX.Text = oklevel.KorosztalyX == 0 ? "" : oklevel.KorosztalyX.ToString( );
            txtKorosztalyY.Text = oklevel.KorosztalyY == 0 ? "" : oklevel.KorosztalyY.ToString( );
            txtKorosztalyH.Text = oklevel.KorosztalyH == 0 ? "" : oklevel.KorosztalyH.ToString( );
            chkKorosztalyBold.Checked = ( ( oklevel.KorosztalyF == "B" ) || ( oklevel.KorosztalyF == "2" ) );
            chkKorosztalyItalic.Checked = ( ( oklevel.KorosztalyF == "I" ) || ( oklevel.KorosztalyF == "2" ) );
            chkKorosztalyLeft.Checked = ( oklevel.KorosztalyI == "L" );
            chkKorosztalyRight.Checked = ( oklevel.KorosztalyI == "R" );
            chkKorosztalyMiddle.Checked = ( oklevel.KorosztalyI == "M" );
            txtKorosztalyM.Text = oklevel.KorosztalyM == 0 ? "" : oklevel.KorosztalyM.ToString( );
            cboKorosztalyB.Text = oklevel.KorosztalyB;

            txtInduloNemeX.Text = oklevel.InduloNemeX == 0 ? "" : oklevel.InduloNemeX.ToString( );
            txtInduloNemeY.Text = oklevel.InduloNemeY == 0 ? "" : oklevel.InduloNemeY.ToString( );
            txtInduloNemeH.Text = oklevel.InduloNemeH == 0 ? "" : oklevel.InduloNemeH.ToString( );
            chkInduloNemeBold.Checked = ( ( oklevel.InduloNemeF == "B" ) || ( oklevel.InduloNemeF == "2" ) );
            chkInduloNemeItalic.Checked = ( ( oklevel.InduloNemeF == "I" ) || ( oklevel.InduloNemeF == "2" ) );
            chkInduloNemeLeft.Checked = ( oklevel.InduloNemeI == "L" );
            chkInduloNemeRight.Checked = ( oklevel.InduloNemeI == "R" );
            chkInduloNemeMiddle.Checked = ( oklevel.InduloNemeI == "M" );
            txtInduloNemeM.Text = oklevel.InduloNemeM == 0 ? "" : oklevel.InduloNemeM.ToString( );
            cboInduloNemeB.Text = oklevel.InduloNemeB;

            txtDatumX.Text = oklevel.DatumX == 0 ? "" : oklevel.DatumX.ToString( );
            txtDatumY.Text = oklevel.DatumY == 0 ? "" : oklevel.DatumY.ToString( );
            txtDatumH.Text = oklevel.DatumH == 0 ? "" : oklevel.DatumH.ToString( );
            chkDatumBold.Checked = ( ( oklevel.DatumF == "B" ) || ( oklevel.DatumF == "2" ) );
            chkDatumItalic.Checked = ( ( oklevel.DatumF == "I" ) || ( oklevel.DatumF == "2" ) );
            chkDatumLeft.Checked = ( oklevel.DatumI == "L" );
            chkDatumRight.Checked = ( oklevel.DatumI == "R" );
            chkDatumMiddle.Checked = ( oklevel.DatumI == "M" );
            txtDatumM.Text = oklevel.DatumM == 0 ? "" : oklevel.DatumM.ToString( );
            cboDatumB.Text = oklevel.DatumB;
        }

        #region EventHandlers
        private void Cbo_Tipus_SelectedIndexChanged( object sender, EventArgs e ) {
            //if( cbo_Tipus.Text == null ) {
            //    return;
            //}

            //if( cbo_Tipus.Text == "Verseny" ) {
            //    txt_VersenySorozatX.Enabled = true;
            //    txt_VersenySorozatY.Enabled = true;
            //    txt_VersenySorozatH.Enabled = true;
            //    txt_VersenyX.Enabled = true;
            //    txt_VersenyY.Enabled = true;
            //    txt_VersenyH.Enabled = true;
            //    txt_EgyesuletX.Enabled = true;
            //    txt_EgyesuletY.Enabled = true;
            //    txt_EgyesuletH.Enabled = true;
            //    txt_IjtipusX.Enabled = true;
            //    txt_IjtipusY.Enabled = true;
            //    txt_IjtipusH.Enabled = true;

            //    txt_HelyezesX.Enabled = false;
            //    txt_HelyezesY.Enabled = false;
            //    txt_HelyezesH.Enabled = false;
            //    txt_InduloX.Enabled = false;
            //    txt_InduloY.Enabled = false;
            //    txt_InduloH.Enabled = false;
            //}
            //else if( cbo_Tipus.Text == "Versenysorozat" ) {
            //    txt_VersenyX.Enabled = true;
            //    txt_VersenyY.Enabled = true;
            //    txt_VersenyH.Enabled = true;
            //    txt_VersenySorozatX.Enabled = true;
            //    txt_VersenySorozatY.Enabled = true;
            //    txt_VersenySorozatH.Enabled = true;
            //    txt_InduloX.Enabled = true;
            //    txt_InduloY.Enabled = true;
            //    txt_InduloH.Enabled = true;
            //    txt_IjtipusX.Enabled = true;
            //    txt_IjtipusY.Enabled = true;
            //    txt_IjtipusH.Enabled = true;
            //    txt_HelyezesX.Enabled = true;
            //    txt_HelyezesY.Enabled = true;
            //    txt_HelyezesH.Enabled = true;

            //    txt_EgyesuletX.Enabled = false;
            //    txt_EgyesuletY.Enabled = false;
            //    txt_EgyesuletH.Enabled = false;
            //}
        }

        private void btnRendben_Click( object sender, EventArgs e ) {
            Oklevel oklevel = new Oklevel( );

            #region Error Checks
            if( txtAzonosito.Text.Length == 0 ) { MessageBox.Show( "Hiba az azonosító mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( cboTipus.Text.Length == 0 ) { MessageBox.Show( "Hiba a típus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

          
            if( txtVersenyX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtVersenyX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a verseny mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenyY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a verseny mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenyH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a verseny mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenyM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a verseny mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtVersenySorozatX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtVersenySorozatX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Versenysorozat mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenySorozatY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Versenysorozat mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenySorozatH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Versenysorozat mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtVersenySorozatM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Versenysorozat mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtHelyezesX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtHelyezesX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtHelyezesY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtHelyezesH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtHelyezesM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtInduloX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtInduloX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtEgyesuletX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtEgyesuletX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtEgyesuletY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtEgyesuletH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtEgyesuletM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtIjtipusX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtIjtipusX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Íjtipus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtIjtipusY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Íjtipus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtIjtipusH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Íjtipus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtIjtipusM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Íjtipus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtKorosztalyX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtKorosztalyX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Korosztály mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtKorosztalyY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Korosztály mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtKorosztalyH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Korosztály mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtKorosztalyM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Korosztály mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtInduloNemeX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtInduloNemeX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló Neme mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloNemeY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló Neme mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloNemeH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló Neme mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtInduloNemeM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba az Induló Neme mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            if( txtDatumX.Text.Length != 0 ) {
                try { Convert.ToInt32( txtDatumX.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtDatumY.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtDatumH.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txtDatumM.Text ); } catch( Exception ) { MessageBox.Show( "Hiba a Dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            }
            #endregion


            oklevel = new Oklevel {
                Azonosito = txtAzonosito.Text,
                Tipus = cboTipus.Text,

                VersenyX = txtVersenyX.Text == "" ? 0 : Convert.ToInt32( txtVersenyX.Text ),
                VersenyY = txtVersenyY.Text == "" ? 0 : Convert.ToInt32( txtVersenyY.Text ),
                VersenyH = txtVersenyH.Text == "" ? 0 : Convert.ToInt32( txtVersenyH.Text ),
                VersenyF = chkVersenyBold.Checked == true ? ( chkVersenyItalic.Checked == true ? "2" : "B" ) : ( chkVersenyItalic.Checked == true ? "I" : "0" ),
                VersenyB = cboVersenyB.Text,
                VersenyM = txtVersenyM.Text == "" ? 0 : Convert.ToInt32( txtVersenyM.Text ),
                VersenyI = chkVersenyLeft.Checked == true ? "L" : ( chkVersenyRight.Checked == true ? "R" : ( chkVersenyMiddle.Checked == true ? "M" : "0" ) ),

                VersenySorozatX = txtVersenySorozatX.Text == "" ? 0 : Convert.ToInt32( txtVersenySorozatX.Text ),
                VersenySorozatY = txtVersenySorozatY.Text == "" ? 0 : Convert.ToInt32( txtVersenySorozatY.Text ),
                VersenySorozatH = txtVersenySorozatH.Text == "" ? 0 : Convert.ToInt32( txtVersenySorozatH.Text ),
                VersenySorozatF = chkVersenySorozatBold.Checked == true ? ( chkVersenySorozatItalic.Checked == true ? "2" : "B" ) : ( chkVersenySorozatItalic.Checked == true ? "I" : "0" ),
                VersenySorozatB = cboVersenySorozatB.Text,
                VersenySorozatM = txtVersenySorozatM.Text == "" ? 0 : Convert.ToInt32( txtVersenySorozatM.Text ),
                VersenySorozatI = chkVersenySorozatLeft.Checked == true ? "L" : ( chkVersenySorozatRight.Checked == true ? "R" : ( chkVersenySorozatMiddle.Checked == true ? "M" : "0" ) ),

                HelyezesX = txtHelyezesX.Text == "" ? 0 : Convert.ToInt32( txtHelyezesX.Text ),
                HelyezesY = txtHelyezesY.Text == "" ? 0 : Convert.ToInt32( txtHelyezesY.Text ),
                HelyezesH = txtHelyezesH.Text == "" ? 0 : Convert.ToInt32( txtHelyezesH.Text ),
                HelyezesB = cboHelyezesB.Text,
                HelyezesF = chkHelyezesBold.Checked == true ? ( chkHelyezesItalic.Checked == true ? "2" : "B" ) : ( chkHelyezesItalic.Checked == true ? "I" : "0" ),
                HelyezesM = txtHelyezesM.Text == "" ? 0 : Convert.ToInt32( txtHelyezesM.Text ),
                HelyezesI = chkHelyezesLeft.Checked == true ? "L" : ( chkHelyezesRight.Checked == true ? "R" : ( chkHelyezesMiddle.Checked == true ? "M" : "0" ) ),

                InduloX = txtInduloX.Text == "" ? 0 : Convert.ToInt32( txtInduloX.Text ),
                InduloY = txtInduloY.Text == "" ? 0 : Convert.ToInt32( txtInduloY.Text ),
                InduloH = txtInduloH.Text == "" ? 0 : Convert.ToInt32( txtInduloH.Text ),
                InduloB = cboInduloB.Text,
                InduloF = chkInduloBold.Checked == true ? ( chkInduloItalic.Checked == true ? "2" : "B" ) : ( chkInduloItalic.Checked == true ? "I" : "0" ),
                InduloM = txtInduloM.Text == "" ? 0 : Convert.ToInt32( txtInduloM.Text ),
                InduloI = chkInduloLeft.Checked == true ? "L" : ( chkInduloRight.Checked == true ? "R" : ( chkInduloMiddle.Checked == true ? "M" : "0" ) ),

                EgyesuletX = txtEgyesuletX.Text == "" ? 0 : Convert.ToInt32( txtEgyesuletX.Text ),
                EgyesuletY = txtEgyesuletY.Text == "" ? 0 : Convert.ToInt32( txtEgyesuletY.Text ),
                EgyesuletH = txtEgyesuletH.Text == "" ? 0 : Convert.ToInt32( txtEgyesuletH.Text ),
                EgyesuletB = cboEgyesuletB.Text,
                EgyesuletF = chkEgyesuletBold.Checked == true ? ( chkEgyesuletItalic.Checked == true ? "2" : "B" ) : ( chkEgyesuletItalic.Checked == true ? "I" : "0" ),
                EgyesuletM = txtEgyesuletM.Text == "" ? 0 : Convert.ToInt32( txtEgyesuletM.Text ),
                EgyesuletI = chkEgyesuletLeft.Checked == true ? "L" : ( chkEgyesuletRight.Checked == true ? "R" : ( chkEgyesuletMiddle.Checked == true ? "M" : "0" ) ),

                IjtipusX = txtIjtipusX.Text == "" ? 0 : Convert.ToInt32( txtIjtipusX.Text ),
                IjtipusY = txtIjtipusY.Text == "" ? 0 : Convert.ToInt32( txtIjtipusY.Text ),
                IjtipusH = txtIjtipusH.Text == "" ? 0 : Convert.ToInt32( txtIjtipusH.Text ),
                IjtipusB = cboIjtipusB.Text,
                IjtipusF = chkIjtipusBold.Checked == true ? ( chkIjtipusItalic.Checked == true ? "2" : "B" ) : ( chkIjtipusItalic.Checked == true ? "I" : "0" ),
                IjtipusM = txtIjtipusM.Text == "" ? 0 : Convert.ToInt32( txtIjtipusM.Text ),
                IjtipusI = chkIjtipusLeft.Checked == true ? "L" : ( chkIjtipusRight.Checked == true ? "R" : ( chkIjtipusMiddle.Checked == true ? "M" : "0" ) ),

                KorosztalyX = txtKorosztalyX.Text == "" ? 0 : Convert.ToInt32( txtKorosztalyX.Text ),
                KorosztalyY = txtKorosztalyY.Text == "" ? 0 : Convert.ToInt32( txtKorosztalyY.Text ),
                KorosztalyH = txtKorosztalyH.Text == "" ? 0 : Convert.ToInt32( txtKorosztalyH.Text ),
                KorosztalyB = cboKorosztalyB.Text,
                KorosztalyF = chkKorosztalyBold.Checked == true ? ( chkKorosztalyItalic.Checked == true ? "2" : "B" ) : ( chkKorosztalyItalic.Checked == true ? "I" : "0" ),
                KorosztalyM = txtKorosztalyM.Text == "" ? 0 : Convert.ToInt32( txtKorosztalyM.Text ),
                KorosztalyI = chkKorosztalyLeft.Checked == true ? "L" : ( chkKorosztalyRight.Checked == true ? "R" : ( chkKorosztalyMiddle.Checked == true ? "M" : "0" ) ),

                InduloNemeX = txtInduloNemeX.Text == "" ? 0 : Convert.ToInt32( txtInduloNemeX.Text ),
                InduloNemeY = txtInduloNemeY.Text == "" ? 0 : Convert.ToInt32( txtInduloNemeY.Text ),
                InduloNemeH = txtInduloNemeH.Text == "" ? 0 : Convert.ToInt32( txtInduloNemeH.Text ),
                InduloNemeB = cboInduloNemeB.Text,
                InduloNemeF = chkInduloNemeBold.Checked == true ? ( chkInduloNemeItalic.Checked == true ? "2" : "B" ) : ( chkInduloNemeItalic.Checked == true ? "I" : "0" ),
                InduloNemeM = txtInduloNemeM.Text == "" ? 0 : Convert.ToInt32( txtInduloNemeM.Text ),
                InduloNemeI = chkInduloNemeLeft.Checked == true ? "L" : ( chkInduloNemeRight.Checked == true ? "R" : ( chkInduloNemeMiddle.Checked == true ? "M" : "0" ) ),

                DatumX = txtDatumX.Text == "" ? 0 : Convert.ToInt32( txtDatumX.Text ),
                DatumY = txtDatumY.Text == "" ? 0 : Convert.ToInt32( txtDatumY.Text ),
                DatumH = txtDatumH.Text == "" ? 0 : Convert.ToInt32( txtDatumH.Text ),
                DatumB = cboDatumB.Text,
                DatumF = chkDatumBold.Checked == true ? ( chkDatumItalic.Checked == true ? "2" : "B" ) : ( chkDatumItalic.Checked == true ? "I" : "0" ),
                DatumM = txtDatumM.Text == "" ? 0 : Convert.ToInt32( txtDatumM.Text ),
                DatumI = chkDatumLeft.Checked == true ? "L" : ( chkDatumRight.Checked == true ? "R" : ( chkDatumMiddle.Checked == true ? "M" : "0" ) )
            };

            if( modositas == false ) {
                Program.mainform.oklevelek_panel.Sablon_Hozzáadás( oklevel );
            }
            else {
                Program.mainform.oklevelek_panel.Sablon_Modositas( oklevel.Azonosito, oklevel );

            }
            Close( );
        }

        private void chkAll_Click( object _sender, EventArgs _event ) {
            CheckBox chkAktiv = _sender as CheckBox;
            
            #region Verseny
            if( chkAktiv == chkVersenyLeft ) {
                chkVersenyLeft.Checked = true;
                chkVersenyRight.Checked = false;
                chkVersenyMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkVersenyRight ) {
                chkVersenyRight.Checked = true;
                chkVersenyMiddle.Checked = false;
                chkVersenyLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkVersenyMiddle ) {
                chkVersenyMiddle.Checked = true;
                chkVersenyRight.Checked = false;
                chkVersenyLeft.Checked = false;
                return;
            }
            #endregion
            #region Versenysorozat
            if( chkAktiv == chkVersenySorozatLeft ) {
                chkVersenySorozatLeft.Checked = true;
                chkVersenySorozatRight.Checked = false;
                chkVersenySorozatMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkVersenySorozatRight ) {
                chkVersenySorozatRight.Checked = true;
                chkVersenySorozatMiddle.Checked = false;
                chkVersenySorozatLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkVersenySorozatMiddle ) {
                chkVersenySorozatMiddle.Checked = true;
                chkVersenySorozatRight.Checked = false;
                chkVersenySorozatLeft.Checked = false;
                return;
            }
            #endregion
            #region Helyezes
            if( chkAktiv == chkHelyezesLeft ) {
                chkHelyezesLeft.Checked = true;
                chkHelyezesRight.Checked = false;
                chkHelyezesMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkHelyezesRight ) {
                chkHelyezesRight.Checked = true;
                chkHelyezesMiddle.Checked = false;
                chkHelyezesLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkHelyezesMiddle ) {
                chkHelyezesMiddle.Checked = true;
                chkHelyezesRight.Checked = false;
                chkHelyezesLeft.Checked = false;
                return;
            }
            #endregion
            #region Indulo
            if( chkAktiv == chkInduloLeft ) {
                chkInduloLeft.Checked = true;
                chkInduloRight.Checked = false;
                chkInduloMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkInduloRight ) {
                chkInduloRight.Checked = true;
                chkInduloMiddle.Checked = false;
                chkInduloLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkInduloMiddle ) {
                chkInduloMiddle.Checked = true;
                chkInduloRight.Checked = false;
                chkInduloLeft.Checked = false;
                return;
            }
            #endregion
            #region Egyesület
            if( chkAktiv == chkEgyesuletLeft ) {
                chkEgyesuletLeft.Checked = true;
                chkEgyesuletRight.Checked = false;
                chkEgyesuletMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkEgyesuletRight ) {
                chkEgyesuletRight.Checked = true;
                chkEgyesuletMiddle.Checked = false;
                chkEgyesuletLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkEgyesuletMiddle ) {
                chkEgyesuletMiddle.Checked = true;
                chkEgyesuletRight.Checked = false;
                chkEgyesuletLeft.Checked = false;
                return;
            }
            #endregion
            #region Ijtipus
            if( chkAktiv == chkIjtipusLeft ) {
                chkIjtipusLeft.Checked = true;
                chkIjtipusRight.Checked = false;
                chkIjtipusMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkIjtipusRight ) {
                chkIjtipusRight.Checked = true;
                chkIjtipusMiddle.Checked = false;
                chkIjtipusLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkIjtipusMiddle ) {
                chkIjtipusMiddle.Checked = true;
                chkIjtipusRight.Checked = false;
                chkIjtipusLeft.Checked = false;
                return;
            }

            #endregion
            #region Korosztaly
            if( chkAktiv == chkKorosztalyLeft ) {
                chkKorosztalyLeft.Checked = true;
                chkKorosztalyRight.Checked = false;
                chkKorosztalyMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkKorosztalyRight ) {
                chkKorosztalyRight.Checked = true;
                chkKorosztalyMiddle.Checked = false;
                chkKorosztalyLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkKorosztalyMiddle ) {
                chkKorosztalyMiddle.Checked = true;
                chkKorosztalyRight.Checked = false;
                chkKorosztalyLeft.Checked = false;
                return;
            }
            #endregion
            #region InduloNeme
            if( chkAktiv == chkInduloNemeLeft ) {
                chkInduloNemeLeft.Checked = true;
                chkInduloNemeRight.Checked = false;
                chkInduloNemeMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkInduloNemeRight ) {
                chkInduloNemeRight.Checked = true;
                chkInduloNemeMiddle.Checked = false;
                chkInduloNemeLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkInduloNemeMiddle ) {
                chkInduloNemeMiddle.Checked = true;
                chkInduloNemeRight.Checked = false;
                chkInduloNemeLeft.Checked = false;
                return;
            }
            #endregion
            #region Datum
            if( chkAktiv == chkDatumLeft ) {
                chkDatumLeft.Checked = true;
                chkDatumRight.Checked = false;
                chkDatumMiddle.Checked = false;
                return;
            }
            if( chkAktiv == chkDatumRight ) {
                chkDatumRight.Checked = true;
                chkDatumMiddle.Checked = false;
                chkDatumLeft.Checked = false;
                return;
            }
            if( chkAktiv == chkDatumMiddle ) {
                chkDatumMiddle.Checked = true;
                chkDatumRight.Checked = false;
                chkDatumLeft.Checked = false;
                return;
            }
                #endregion
        }
        #endregion

    }
}
//}
