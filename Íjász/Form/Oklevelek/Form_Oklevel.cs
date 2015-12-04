using System;
using System.Drawing;
using System.Windows.Forms;

//NOTE(mate): sablon típusa nem módosítható !!!
namespace Íjász {
    public sealed class Form_Oklevel : Form {
        TextBox txt_Azonosito;
        ComboBox cbo_Tipus;
        TextBox txt_NevX;
        TextBox txt_NevY;
        TextBox txt_NevH;
        TextBox txt_HelyezesX;
        TextBox txt_HelyezesY;
        TextBox txt_HelyezesH;
        TextBox txt_KategoriaX;
        TextBox txt_KategoriaY;
        TextBox txt_KategoriaH;
        TextBox txt_HelyszinX;
        TextBox txt_HelyszinY;
        TextBox txt_HelyszinH;
        TextBox txt_DatumX;
        TextBox txt_DatumY;
        TextBox txt_DatumH;
        TextBox txt_EgyesuletX;
        TextBox txt_EgyesuletY;
        TextBox txt_EgyesuletH;

        //NOTE(mate): új sablon
        public Form_Oklevel( ) {
            InitializeForm( );
            InitializeContent( );
            InitializeData( );
        }

        public Form_Oklevel( Oklevel _Oklevel ) {

            if( _Oklevel.Tipus.Equals( "Verseny" ) ) {
                InitializeForm_Verseny( );
                InitializeContent_Verseny( _Oklevel );
                InitializeData_Verseny( _Oklevel );
            }
            else if( _Oklevel.Tipus.Equals( "Versenysorozat" ) ) {
                InitializeForm_Versenysorozat( );
                InitializeContent_Versenysorozat( _Oklevel );
                InitializeData_Versenysorozat( _Oklevel );
            }
        }

        //NOTE(mate): új sablon
        private void InitializeForm( ) {
            Text = "Új sablon szerkesztő";
            ClientSize = new Size( 420 , 350 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void InitializeContent( ) {
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            #region Labels
            Label lblAzonosito = new Label {
                Text = "Azonosító:",
                Location = new Point( 16, 1 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblTipus = new Label {
                Text = "Típus:",
                Location = new Point( 16, 3 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblFelirat = new Label {
                Text = "                         Szélesség(mm)          Magasság(mm)                 Hossz(mm)",
                Location = new Point( 16, 5 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };



            Label lblNev = new Label {
                Text = "Név:",
                Location = new Point( 16, 7 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyezes = new Label {
                Text = "Helyezés:",
                Location = new Point( 16, 9 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblKategoria = new Label {
                Text = "Kategória:",
                Location = new Point( 16, 11 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyszin = new Label {
                Text = "Helyszín:",
                Location = new Point( 16, 13 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblDatum = new Label {
                Text = "Dátum:",
                Location = new Point( 16, 15 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblEgyesulet = new Label {
                Text = "Egyesület:",
                Location = new Point( 16, 17 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            #endregion

            txt_Azonosito = new TextBox {
                Location = new Point( 5 * 16, 1 * 16 - 8 )
            };
            cbo_Tipus = new ComboBox {
                Location = new Point( 5 * 16, 3 * 16 - 8 ),
                Size = new Size( 128, 24 ),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cbo_Tipus.Items.Add( "Verseny" );
            cbo_Tipus.Items.Add( "Versenysorozat" );
            cbo_Tipus.SelectedIndexChanged += Cbo_Tipus_SelectedIndexChanged;

            txt_NevX = new TextBox {
                Location = new Point( 5 * 16, 7 * 16 - 4 )
            };
            txt_NevY = new TextBox {
                Location = new Point( 12 * 16, 7 * 16 - 4 )
            };
            txt_NevH = new TextBox {
                Location = new Point( 19 * 16, 7* 16 - 4 )
            };

            txt_HelyezesX = new TextBox {
                Location = new Point( 5 * 16, 9 * 16 - 4 )
            };
            txt_HelyezesY = new TextBox {
                Location = new Point( 12 * 16, 9 * 16 - 4 )
            };
            txt_HelyezesH = new TextBox {
                Location = new Point( 19 * 16, 9 * 16 - 4 )
            };

            txt_KategoriaX = new TextBox {
                Location = new Point( 5 * 16, 11 * 16 - 4 )
            };
            txt_KategoriaY = new TextBox {
                Location = new Point( 12 * 16, 11 * 16 - 4 )
            };
            txt_KategoriaH = new TextBox {
                Location = new Point( 19 * 16, 11 * 16 - 4 )
            };

            txt_HelyszinX = new TextBox {
                Location = new Point( 5 * 16, 13 * 16 - 4 )
            };
            txt_HelyszinY = new TextBox {
                Location = new Point( 12 * 16, 13 * 16 - 4 )
            };
            txt_HelyszinH = new TextBox {
                Location = new Point( 19 * 16, 13 * 16 - 4 )
            };

            txt_DatumX = new TextBox {
                Location = new Point( 5 * 16, 15 * 16 - 4 )
            };
            txt_DatumY = new TextBox {
                Location = new Point( 12 * 16, 15 * 16 - 4 )
            };
            txt_DatumH = new TextBox {
                Location = new Point( 19 * 16, 15 * 16 - 4 )
            };

            txt_EgyesuletX = new TextBox {
                Location = new Point( 5 * 16, 17 * 16 - 4 )
            };
            txt_EgyesuletY = new TextBox {
                Location = new Point( 12 * 16, 17 * 16 - 4 )
            };
            txt_EgyesuletH = new TextBox {
                Location = new Point( 19 * 16, 17 * 16 - 4 )
            };

            Button btnRendben = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Rendben",
                Location = new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                Size = new Size( 96, 32 )
            };
            btnRendben.Click += btnRendben_Click;
            Controls.Add( btnRendben );

            Controls.Add( lblAzonosito );
            Controls.Add( lblTipus );
            Controls.Add( lblFelirat );
            Controls.Add( lblNev );
            Controls.Add( lblHelyezes );
            Controls.Add( lblKategoria );
            Controls.Add( lblHelyszin );
            Controls.Add( lblDatum );
            Controls.Add( lblEgyesulet );

            Controls.Add( txt_Azonosito );
            Controls.Add( cbo_Tipus );

            Controls.Add( txt_NevX );
            Controls.Add( txt_NevY );
            Controls.Add( txt_NevH );

            Controls.Add( txt_HelyezesX );
            Controls.Add( txt_HelyezesY );
            Controls.Add( txt_HelyezesH );

            Controls.Add( txt_KategoriaX );
            Controls.Add( txt_KategoriaY );
            Controls.Add( txt_KategoriaH );

            Controls.Add( txt_HelyszinX );
            Controls.Add( txt_HelyszinY );
            Controls.Add( txt_HelyszinH );

            Controls.Add( txt_DatumX );
            Controls.Add( txt_DatumY );
            Controls.Add( txt_DatumH );

            Controls.Add( txt_EgyesuletX );
            Controls.Add( txt_EgyesuletY );
            Controls.Add( txt_EgyesuletH );
        }
        private void InitializeData( ) {

        }

        //NOTE(mate): verseny sablon modosito
        private void InitializeForm_Verseny( ) {
            Text = "Verseny sablon szerkesztő";
            ClientSize = new Size( 430 , 200 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void InitializeData_Verseny( Oklevel _Oklevel ) {
            txt_Azonosito.Text = _Oklevel.Azonosito;
            cbo_Tipus.Text = _Oklevel.Tipus;
            txt_NevX.Text = _Oklevel.NevX.ToString( );
            txt_NevY.Text = _Oklevel.NevY.ToString( );
            txt_NevH.Text = _Oklevel.NevH.ToString( );
            txt_HelyezesX.Text = _Oklevel.HelyezesX.ToString( );
            txt_HelyezesY.Text = _Oklevel.HelyezesY.ToString( );
            txt_HelyezesH.Text = _Oklevel.HelyezesH.ToString( );
            // txt_Kategoria.Text = _Oklevel.Kategoria.ToString( );
            // txt_Helyszin.Text = _Oklevel.Helyszin.ToString( );
            txt_DatumX.Text = _Oklevel.DatumX.ToString( );
            txt_DatumY.Text = _Oklevel.DatumY.ToString( );
            txt_DatumH.Text = _Oklevel.DatumH.ToString( );
            txt_EgyesuletX.Text = _Oklevel.EgyesuletX.ToString( );
            txt_EgyesuletY.Text = _Oklevel.EgyesuletY.ToString( );
            txt_EgyesuletH.Text = _Oklevel.EgyesuletH.ToString( );
        }
        private void InitializeContent_Verseny( Oklevel _oklevel ) {
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            #region Labels
            Label lblNev = new Label {
                Text = "Név:",
                Location = new Point( 16, 1 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyezes = new Label {
                Text = "Helyezés:",
                Location = new Point( 16, 3 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblDatum = new Label {
                Text = "Dátum:",
                Location = new Point( 16, 5 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblEgyesulet = new Label {
                Text = "Egyesület:",
                Location = new Point( 16, 7 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            #endregion

            txt_Azonosito = new TextBox( );

            cbo_Tipus = new ComboBox( );
            cbo_Tipus.Items.Add( _oklevel.Tipus );
            cbo_Tipus.SelectedIndex = 0;

            txt_NevX = new TextBox {
                Location = new Point( 5 * 16, 1 * 16 - 4 )
            };
            txt_NevY = new TextBox {
                Location = new Point( 12 * 16, 1 * 16 - 4 )
            };
            txt_NevH = new TextBox {
                Location = new Point( 19 * 16, 1 * 16 - 4 )
            };

            txt_HelyezesX = new TextBox {
                Location = new Point( 5 * 16, 3 * 16 - 4 )
            };
            txt_HelyezesY = new TextBox {
                Location = new Point( 12 * 16, 3 * 16 - 4 )
            };
            txt_HelyezesH = new TextBox {
                Location = new Point( 19 * 16, 3 * 16 - 4 )
            };


            txt_KategoriaX = new TextBox( );
            txt_KategoriaY = new TextBox( );
            txt_KategoriaH = new TextBox( );

            txt_HelyszinX = new TextBox( );
            txt_HelyszinY = new TextBox( );
            txt_HelyszinH = new TextBox( );

            txt_DatumX = new TextBox {
                Location = new Point( 5 * 16, 5 * 16 - 4 )
            };
            txt_DatumY = new TextBox {
                Location = new Point( 12 * 16, 5 * 16 - 4 )
            };
            txt_DatumH = new TextBox {
                Location = new Point( 19 * 16, 5 * 16 - 4 )
            };

            txt_EgyesuletX = new TextBox {
                Location = new Point( 5 * 16, 7 * 16 - 4 )
            };
            txt_EgyesuletY = new TextBox {
                Location = new Point( 12 * 16, 7 * 16 - 4 )
            };
            txt_EgyesuletH = new TextBox {
                Location = new Point( 19 * 16, 7 * 16 - 4 )
            };

            Button btnRendben = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Rendben",
                Location = new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                Size = new Size( 96, 32 )
            };
            btnRendben.Click += btnRendben_Click;
            Controls.Add( btnRendben );

            Controls.Add( lblNev );
            Controls.Add( lblHelyezes );
            Controls.Add( lblDatum );
            Controls.Add( lblEgyesulet );

            Controls.Add( txt_NevX );
            Controls.Add( txt_NevY );
            Controls.Add( txt_NevH );
            Controls.Add( txt_HelyezesX );
            Controls.Add( txt_HelyezesY );
            Controls.Add( txt_HelyezesH );
            Controls.Add( txt_DatumX );
            Controls.Add( txt_DatumY );
            Controls.Add( txt_DatumH );
            Controls.Add( txt_EgyesuletX );
            Controls.Add( txt_EgyesuletY );
            Controls.Add( txt_EgyesuletH );
        }

        //NOTE(mate): versenysorozat sablon modosito
        private void InitializeForm_Versenysorozat( ) {
            Text = "Versenysorozat sablon szerkesztő";
            ClientSize = new Size( 430 , 240 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void InitializeData_Versenysorozat( Oklevel _Oklevel ) {
            txt_Azonosito.Text = _Oklevel.Azonosito;
            cbo_Tipus.Text = _Oklevel.Tipus;
            txt_NevX.Text = _Oklevel.NevX.ToString( );
            txt_NevY.Text = _Oklevel.NevY.ToString( );
            txt_NevH.Text = _Oklevel.NevH.ToString( );
            txt_HelyezesX.Text = _Oklevel.HelyezesX.ToString( );
            txt_HelyezesY.Text = _Oklevel.HelyezesY.ToString( );
            txt_HelyezesH.Text = _Oklevel.HelyezesH.ToString( );
            txt_KategoriaX.Text = _Oklevel.KategoriaX.ToString( );
            txt_KategoriaY.Text = _Oklevel.KategoriaY.ToString( );
            txt_KategoriaH.Text = _Oklevel.KategoriaH.ToString( );
            txt_HelyszinX.Text = _Oklevel.HelyszinX.ToString( );
            txt_HelyszinY.Text = _Oklevel.HelyszinY.ToString( );
            txt_HelyszinH.Text = _Oklevel.HelyszinH.ToString( );
            txt_DatumX.Text = _Oklevel.DatumX.ToString( );
            txt_DatumY.Text = _Oklevel.DatumY.ToString( );
            txt_DatumH.Text = _Oklevel.DatumH.ToString( );
            //txt_Egyesulet.Text = _Oklevel.Egyesulet.ToString( );
        }
        private void InitializeContent_Versenysorozat( Oklevel _oklevel ) {
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            #region Labels
            Label lblNev = new Label {
                Text = "Név:",
                Location = new Point( 16, 1 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyezes = new Label {
                Text = "Helyezés:",
                Location = new Point( 16, 3 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblKategoria = new Label {
                Text = "Kategória:",
                Location = new Point( 16, 5 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblHelyszin = new Label {
                Text = "Helyszín:",
                Location = new Point( 16, 7 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            Label lblDatum = new Label {
                Text = "Dátum:",
                Location = new Point( 16, 9 * 16 ),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSize = true
            };
            #endregion

            txt_Azonosito = new TextBox( );

            cbo_Tipus = new ComboBox( );
            cbo_Tipus.Items.Add( _oklevel.Tipus );

            txt_NevX = new TextBox {
                Location = new Point( 5 * 16, 1 * 16 - 4 )
            };
            txt_NevY = new TextBox {
                Location = new Point( 12 * 16, 1 * 16 - 4 )
            };
            txt_NevH = new TextBox {
                Location = new Point( 19 * 16, 1 * 16 - 4 )
            };

            txt_HelyezesX = new TextBox {
                Location = new Point( 5 * 16, 3 * 16 - 4 )
            };
            txt_HelyezesY = new TextBox {
                Location = new Point( 12 * 16, 3 * 16 - 4 )
            };
            txt_HelyezesH = new TextBox {
                Location = new Point( 19 * 16, 3 * 16 - 4 )
            };

            txt_KategoriaX = new TextBox {
                Location = new Point( 5 * 16, 5 * 16 - 4 )
            };
            txt_KategoriaY = new TextBox {
                Location = new Point( 12 * 16, 5 * 16 - 4 )
            };
            txt_KategoriaH = new TextBox {
                Location = new Point( 19 * 16, 5 * 16 - 4 )
            };

            txt_HelyszinX = new TextBox {
                Location = new Point( 5 * 16, 7 * 16 - 4 )
            };
            txt_HelyszinY = new TextBox {
                Location = new Point( 12 * 16, 7 * 16 - 4 )
            };
            txt_HelyszinH = new TextBox {
                Location = new Point( 19 * 16, 7 * 16 - 4 )
            };

            txt_DatumX = new TextBox {
                Location = new Point( 5 * 16, 9 * 16 - 4 )
            };
            txt_DatumY = new TextBox {
                Location = new Point( 12 * 16, 9 * 16 - 4 )
            };
            txt_DatumH = new TextBox {
                Location = new Point( 19 * 16, 9 * 16 - 4 )
            };

            txt_EgyesuletX = new TextBox( );
            txt_EgyesuletY = new TextBox( );
            txt_EgyesuletH = new TextBox( );

            Button btnRendben = new Button {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "Rendben",
                Location = new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                Size = new Size( 96, 32 )
            };
            btnRendben.Click += btnRendben_Click;
            Controls.Add( btnRendben );

            Controls.Add( lblNev );
            Controls.Add( lblHelyezes );
            Controls.Add( lblKategoria );
            Controls.Add( lblHelyszin );
            Controls.Add( lblDatum );

            Controls.Add( txt_NevX );
            Controls.Add( txt_NevY );
            Controls.Add( txt_NevH );
            Controls.Add( txt_HelyezesX );
            Controls.Add( txt_HelyezesY );
            Controls.Add( txt_HelyezesH );
            Controls.Add( txt_KategoriaX );
            Controls.Add( txt_KategoriaY );
            Controls.Add( txt_KategoriaH );
            Controls.Add( txt_HelyszinX );
            Controls.Add( txt_HelyszinY );
            Controls.Add( txt_HelyszinH );
            Controls.Add( txt_DatumX );
            Controls.Add( txt_DatumY );
            Controls.Add( txt_DatumH );
        }

        #region EventHandlers
        private void Cbo_Tipus_SelectedIndexChanged( object sender, EventArgs e ) {
            if( cbo_Tipus.Text == null ) {
                return;
            }

            if( cbo_Tipus.Text == "Verseny" ) {
                txt_HelyezesX.Enabled = true;
                txt_HelyezesY.Enabled = true;
                txt_HelyezesH.Enabled = true;
                txt_NevX.Enabled = true;
                txt_NevY.Enabled = true;
                txt_NevH.Enabled = true;
                txt_EgyesuletX.Enabled = true;
                txt_EgyesuletY.Enabled = true;
                txt_EgyesuletH.Enabled = true;
                txt_DatumX.Enabled = true;
                txt_DatumY.Enabled = true;
                txt_DatumH.Enabled = true;

                txt_KategoriaX.Enabled = false;
                txt_KategoriaY.Enabled = false;
                txt_KategoriaH.Enabled = false;
                txt_HelyszinX.Enabled = false;
                txt_HelyszinY.Enabled = false;
                txt_HelyszinH.Enabled = false;
            }
            else if( cbo_Tipus.Text == "Versenysorozat" ) {
                txt_NevX.Enabled = true;
                txt_NevY.Enabled = true;
                txt_NevH.Enabled = true;
                txt_HelyezesX.Enabled = true;
                txt_HelyezesY.Enabled = true;
                txt_HelyezesH.Enabled = true;
                txt_HelyszinX.Enabled = true;
                txt_HelyszinY.Enabled = true;
                txt_HelyszinH.Enabled = true;
                txt_DatumX.Enabled = true;
                txt_DatumY.Enabled = true;
                txt_DatumH.Enabled = true;
                txt_KategoriaX.Enabled = true;
                txt_KategoriaY.Enabled = true;
                txt_KategoriaH.Enabled = true;

                txt_EgyesuletX.Enabled = false;
                txt_EgyesuletY.Enabled = false;
                txt_EgyesuletH.Enabled = false;
            }
        }

        private void btnRendben_Click( object sender, EventArgs e ) {
            Oklevel oklevel = new Oklevel( );

            if( txt_Azonosito.Text.Length == 0 ) { MessageBox.Show( "Hiba az azonosító mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( cbo_Tipus.Text.Length == 0 ) { MessageBox.Show( "Hiba a típus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_HelyezesX.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_HelyezesY.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_HelyezesH.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_NevX.Text.Length == 0 ) { MessageBox.Show( "Hiba az név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_NevY.Text.Length == 0 ) { MessageBox.Show( "Hiba az név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_NevH.Text.Length == 0 ) { MessageBox.Show( "Hiba az név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_DatumX.Text.Length == 0 ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_DatumY.Text.Length == 0 ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if( txt_DatumH.Text.Length == 0 ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            try { Convert.ToInt32( txt_HelyezesX.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a helyezes mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_HelyezesY.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a helyezes mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_HelyezesH.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a helyezes mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }


            try { Convert.ToInt32( txt_NevX.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_NevY.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_NevH.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }


            try { Convert.ToInt32( txt_DatumX.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_DatumY.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToInt32( txt_DatumH.Text ); }
            catch( Exception ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            //NOTE(mate): versenysorozat szerkesztő
            if( cbo_Tipus.Text == "Versenysorozat" ) {
                if( txt_KategoriaX.Text.Length == 0 ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_KategoriaY.Text.Length == 0 ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_KategoriaH.Text.Length == 0 ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_HelyszinX.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_HelyszinY.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_HelyszinH.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_KategoriaX.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_KategoriaY.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_KategoriaH.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

                try { Convert.ToInt32( txt_HelyszinX.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_HelyszinY.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_HelyszinH.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

                oklevel = new Oklevel {
                    Azonosito = txt_Azonosito.Text,
                    Tipus = cbo_Tipus.Text,

                    HelyezesX = Convert.ToInt32( txt_HelyezesX.Text ),
                    HelyezesY = Convert.ToInt32( txt_HelyezesY.Text ),
                    HelyezesH = Convert.ToInt32( txt_HelyezesH.Text ),

                    NevX = Convert.ToInt32( txt_NevX.Text ),
                    NevY = Convert.ToInt32( txt_NevY.Text ),
                    NevH = Convert.ToInt32( txt_NevH.Text ),

                    DatumX = Convert.ToInt32( txt_DatumX.Text ),
                    DatumY = Convert.ToInt32( txt_DatumY.Text ),
                    DatumH = Convert.ToInt32( txt_DatumH.Text ),

                    KategoriaX = Convert.ToInt32( txt_KategoriaX.Text ),
                    KategoriaY = Convert.ToInt32( txt_KategoriaY.Text ),
                    KategoriaH = Convert.ToInt32( txt_KategoriaH.Text ),

                    HelyszinX = Convert.ToInt32( txt_HelyszinX.Text ),
                    HelyszinY = Convert.ToInt32( txt_HelyszinY.Text ),
                    HelyszinH = Convert.ToInt32( txt_HelyszinH.Text ),

                    EgyesuletX = -66,
                    EgyesuletY = -66,
                    EgyesuletH = -66,
                };
            }
            //NOTE(mate): verseny szerkesztő
            else if( cbo_Tipus.Text == "Verseny" ) {
                if( txt_EgyesuletX.Text.Length == 0 ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_EgyesuletY.Text.Length == 0 ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                if( txt_EgyesuletH.Text.Length == 0 ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

                try { Convert.ToInt32( txt_EgyesuletX.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_EgyesuletY.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
                try { Convert.ToInt32( txt_EgyesuletH.Text ); }
                catch( Exception ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

                oklevel = new Oklevel {
                    Azonosito = txt_Azonosito.Text,
                    Tipus = cbo_Tipus.Text,

                    HelyezesX = Convert.ToInt32( txt_HelyezesX.Text ),
                    HelyezesY = Convert.ToInt32( txt_HelyezesY.Text ),
                    HelyezesH = Convert.ToInt32( txt_HelyezesH.Text ),
                    NevX = Convert.ToInt32( txt_NevX.Text ),
                    NevY = Convert.ToInt32( txt_NevY.Text ),
                    NevH = Convert.ToInt32( txt_NevH.Text ),
                    DatumX = Convert.ToInt32( txt_DatumX.Text ),
                    DatumY = Convert.ToInt32( txt_DatumY.Text ),
                    DatumH = Convert.ToInt32( txt_DatumH.Text ),
                    EgyesuletX = Convert.ToInt32( txt_EgyesuletX.Text ),
                    EgyesuletY = Convert.ToInt32( txt_EgyesuletY.Text ),
                    EgyesuletH = Convert.ToInt32( txt_EgyesuletH.Text ),

                    KategoriaX = -66,
                    KategoriaY = -66,
                    KategoriaH = -66,
                    HelyszinX = -66,
                    HelyszinY = -66,
                    HelyszinH = -66,
                };
            }

            if( txt_Azonosito.FindForm( ) == null ) {
                Program.mainform.oklevelek_panel.Sablon_Modositas( txt_Azonosito.Text, oklevel );
            }
            else {
                Program.mainform.oklevelek_panel.Sablon_Hozzáadás( oklevel );
            }

            this.Close( );
            return;
        }
        #endregion
    }
}
