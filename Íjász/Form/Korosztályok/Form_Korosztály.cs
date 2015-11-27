using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class Form_Korosztály : Form
   {
      private string eredeti_azonosító = null;
      private string eredeti_verseny = null;

      private TextBox txtAzonosito;
      private TextBox txtMegnevezes;
      private TextBox txtFelso;
      private TextBox txtAlso;
      private CheckBox chkNok;
      private CheckBox chkFerfiak;
      private Label txtNo;
      private Label txtFerfi;

      private CheckBox chkEgyben;

      public Form_Korosztály( string _verseny )
      {
         eredeti_verseny = _verseny;

         InitializeForm( );
         InitializeContent( );
         InitializeData( );
      }

      public Form_Korosztály( string _verseny, Korosztály _korosztály )
      {
         eredeti_verseny = _verseny;

         InitializeForm( _verseny, _korosztály );
         InitializeContent( _verseny, _korosztály );
         InitializeData( _korosztály );
      }

      private void
      InitializeForm( )
      {
         Text = "Korosztály";
         ClientSize = new Size( 400 - 64, 254 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void
      InitializeForm( string _verseny, Korosztály _korosztály )
      {
         Text = "Korosztály";
         ClientSize = new Size( 400 - 64, 304 );
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

         Label lblAlso = new iLabel( "Alsó életkorhatár:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

         Label lblFelso = new iLabel( "Felső életkorhatár:",
                                            new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                            this );

         Label lblNok = new iLabel( "Nők:",
                                            new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                            this );
         lblNok.Size = new Size( 64, 24 );

         Label lblFerfiak = new iLabel( "Férfiak:",
                                                new Point( lblAzonosito.Location.X + 128, 16 + 4 * 32 ),
                                                this );
         lblFerfiak.Size = new Size( 64, 24 );

         Label lblEgyben = new iLabel( "Egyben:",
                                            new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                            this );

         txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Width + 32 + 16, lblAzonosito.Location.Y ),
                                                                null,
                                                                null,
                                                                null,
                                                                this );

         txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                       null,
                                       null,
                                       null,
                                       this );

         txtAlso = new iTextBox( new Point( txtAzonosito.Location.X, lblAlso.Location.Y ),
                                 null,
                                 null,
                                 null,
                                 this );

         txtFelso = new iTextBox( new Point( txtAzonosito.Location.X, lblFelso.Location.Y ),
                                 null,
                                 null,
                                 null,
                                 this );

         chkNok = new iCheckBox( null,
                                 new Point( lblNok.Location.X + lblNok.Size.Width + 16, lblNok.Location.Y ),
                                 null,
                                 this );

         chkFerfiak = new iCheckBox( null,
                                     new Point( lblFerfiak.Location.X + lblFerfiak.Size.Width + 16, lblFerfiak.Location.Y ),
                                     null,
                                     this );

         chkEgyben = new iCheckBox( null,
                                  new Point( txtAzonosito.Location.X, lblEgyben.Location.Y ),
                                  null,
                                  this );

         ///

         Button btnRrendben = new iButton( "Rendben",
                                                 new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                 new Size( 96, 32 ),
                                                 btnRendben_Click,
                                                 this );
      }

      private void InitializeContent( string _verseny, Korosztály _korosztály )
      {
         Label lblAzonosito = new iLabel( "Azonosító:",
                                                new Point( 16, 16 + 0 * 32 ),
                                                this );

         Label lblMegnevezes = new iLabel( "Megnevezés:",
                                                    new Point( lblAzonosito.Location.X, 16 + 1 * 32 ),
                                                    this );

         Label lblAlso = new iLabel( "Alsó életkorhatár:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

         Label lblFelso = new iLabel( "Felső életkorhatár:",
                                            new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                            this );

         Label lblNok = new iLabel( "Nők:",
                                            new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                            this );
         lblNok.Size = new Size( 64, 24 );

         Label lblFerfiak = new iLabel( "Férfiak:",
                                                new Point( lblAzonosito.Location.X + 128, 16 + 4 * 32 ),
                                                this );
         lblFerfiak.Size = new Size( 64, 24 );

         Label lblNoIndulok = new iLabel( "Nő indulók:",
                                                new Point( lblAzonosito.Location.X, 16 + 6 * 32 ),
                                                this );

         Label lblFerfiIndulok = new iLabel( "Férfi indulók:",
                                                    new Point( lblAzonosito.Location.X, 16 + 7 * 32 ),
                                                    this );

         Label lblEgyben = new iLabel( "Egyben:",
                                            new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                            this );

         txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Width + 32 + 16, lblAzonosito.Location.Y ),
                                                                null,
                                                                null,
                                                                null,
                                                                this );

         txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                       null,
                                       null,
                                       null,
                                       this );

         txtAlso = new iTextBox( new Point( txtAzonosito.Location.X, lblAlso.Location.Y ),
                                 null,
                                 null,
                                 null,
                                 this );

         txtFelso = new iTextBox( new Point( txtAzonosito.Location.X, lblFelso.Location.Y ),
                                 null,
                                 null,
                                 null,
                                 this );

         chkNok = new iCheckBox( null,
                                 new Point( lblNok.Location.X + lblNok.Size.Width + 16, lblNok.Location.Y ),
                                 null,
                                 this );

         chkFerfiak = new iCheckBox( null,
                                     new Point( lblFerfiak.Location.X + lblFerfiak.Size.Width + 16, lblFerfiak.Location.Y ),
                                     null,
                                     this );

         txtNo = new iLabel( null,
                             new Point( txtAzonosito.Location.X, lblNoIndulok.Location.Y ),
                             this );
         txtNo.Size = new Size( 64, 24 );

         txtFerfi = new iLabel( null,
                                new Point( txtAzonosito.Location.X, lblFerfiIndulok.Location.Y ),
                                this );
         txtFerfi.Size = new Size( 64, 24 );

         chkEgyben = new iCheckBox( null,
                                  new Point( txtAzonosito.Location.X, lblEgyben.Location.Y ),
                                  null,
                                  this );

         ///

         Button btnRrendben = new iButton( "Rendben",
                                                 new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                 new Size( 96, 32 ),
                                                 btnRendben_Click,
                                                 this );
      }

      private void InitializeData( )
      {
         chkNok.Checked = true;
         chkFerfiak.Checked = true;
      }

      private void InitializeData( Korosztály _korosztály )
      {
         eredeti_azonosító = _korosztály.Azonosito;
         eredeti_verseny = _korosztály.Verseny;

         txtAzonosito.Text = _korosztály.Azonosito;
         txtMegnevezes.Text = _korosztály.Megnevezes;
         txtAlso.Text = _korosztály.AlsoHatar.ToString( );
         txtFelso.Text = _korosztály.FelsoHatar.ToString( );
         chkNok.Checked = _korosztály.Nokre;
         chkFerfiak.Checked = _korosztály.Ferfiakra;
         txtNo.Text = _korosztály.InduloNok.ToString( );
         txtFerfi.Text = _korosztály.InduloFerfiak.ToString( );
         chkEgyben.Checked = _korosztály.Egyben;
      }

      #region EventHandlers
      private void btnRendben_Click( object _sender, EventArgs _event )
      {
         if ( txtAzonosito.Text.Length == 0 || txtAzonosito.Text.Length > 10 ) { MessageBox.Show( "Korosztályazonosító hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( txtAzonosito.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( txtMegnevezes.Text.Length == 0 || txtMegnevezes.Text.Length > 30 ) { MessageBox.Show( "Korosztály megnevezése hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( txtMegnevezes.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         int alsó = 0, felső = 0;

         try { alsó = Convert.ToInt32( txtAlso.Text ); }
         catch { MessageBox.Show( "Nem megfelelő az alsó életkor formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         try { felső = Convert.ToInt32( txtFelso.Text ); }
         catch { MessageBox.Show( "Nem megfelelő a felső életkor formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( alsó <= 0 ) { MessageBox.Show( "Alsó korhatár túl kicsi!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( felső <= alsó ) { MessageBox.Show( "A felső korhatárnak nagyobbnak kell lenni, mint az alsónak", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( 100 < felső ) { if ( MessageBox.Show( "Felső korhatár túl magas, biztosan hagyjuk így?", "Korhatár", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) != DialogResult.OK ) return; }

         Database.CountPair indulók = Program.database.KorosztálySzámolás(eredeti_verseny, alsó, felső, chkNok.Checked, chkFerfiak.Checked, false);
         if ( eredeti_azonosító != null )
         {
            Program.mainform.korosztályok_panel.Korosztály_Módosítás( eredeti_azonosító, new Korosztály( eredeti_verseny,
                                                                                                       txtAzonosito.Text,
                                                                                                       txtMegnevezes.Text,
                                                                                                       alsó,
                                                                                                       felső,
                                                                                                       chkNok.Checked,
                                                                                                       chkFerfiak.Checked,
                                                                                                       indulók.nők,
                                                                                                       indulók.férfiak,
                                                                                                       chkEgyben.Checked ) );
         }
         else
         {
            Program.mainform.korosztályok_panel.Korosztály_Hozzáadás( new Korosztály( eredeti_verseny,
                                                                                    txtAzonosito.Text,
                                                                                    txtMegnevezes.Text,
                                                                                    alsó,
                                                                                    felső,
                                                                                    chkNok.Checked,
                                                                                    chkFerfiak.Checked,
                                                                                    indulók.nők,
                                                                                    indulók.férfiak,
                                                                                    chkEgyben.Checked ) );
         }

         Close( );
      }
      #endregion
   }
}
