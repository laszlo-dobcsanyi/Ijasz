using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Íjász
{
   public sealed class Form_Versenysorozat : Form
   {
      private string eredeti_azonosító = null;

      private TextBox txtAzonosito;
      private TextBox txtMegnevezes;
      private Label lblSzam;


      public Form_Versenysorozat( Versenysorozat _versenysorozat )
      {
         eredeti_azonosító = _versenysorozat.azonosító;

         InitializeForm( _versenysorozat );
         InitializeContent( _versenysorozat );
         InitializeData( _versenysorozat );
      }

      public Form_Versenysorozat( )
      {
         InitializeForm( );
         InitializeContent( );
      }

      #region Hozzáadás

      private void InitializeForm( )
      {
         Text = "Versenysorozat";
         ClientSize = new System.Drawing.Size( 400 - 64, 128 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;


      }

      private void InitializeContent( )
      {
         Label azonosító = new Label();
         azonosító.Text = "Azonosító:";
         azonosító.Location = new System.Drawing.Point( 16, 16 + 0 * 32 );
         //azonosító.Font = new System.Drawing.Font("Arial Black", 10);

         Label megnevezés = new Label();
         megnevezés.Text = "Megnevezés:";
         megnevezés.Location = new System.Drawing.Point( azonosító.Location.X, 16 + 1 * 32 );
         //megnevezés.Font = new System.Drawing.Font("Arial Black", 10);

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

         Button btnRendben = new iButton( "Rendben",
                                                new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new System.Drawing.Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

         Controls.Add( azonosító );
         Controls.Add( megnevezés );
      }
      #endregion

      #region Módosítás

      private void InitializeData( Versenysorozat _versenysorozat )
      {
         txtAzonosito.Text = _versenysorozat.azonosító;
         txtAzonosito.Enabled = ( ( _versenysorozat.versenyek == 0 ) ? true : false );
         txtMegnevezes.Text = _versenysorozat.megnevezés;
         lblSzam.Text = _versenysorozat.versenyek.ToString( );
      }

      private void InitializeForm( Versenysorozat _versenysorozat )
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
      btnRendben_Click( object _sender, EventArgs _event )
      {
         Regex rgx = new Regex("[^a-zA-Z0-9 ]");
         txtAzonosito.Text = rgx.Replace( txtAzonosito.Text, "" );

         if ( !( 0 < txtAzonosito.Text.Length && txtAzonosito.Text.Length <= 10 ) ) { MessageBox.Show( "Nem megfelelő az azonosító hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( txtAzonosito.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !( 0 < txtMegnevezes.Text.Length && txtMegnevezes.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő a megnevezés hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( txtMegnevezes.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( eredeti_azonosító != null )
            Program.mainform.versenysorozat_panel.VersenySorozatModositas( eredeti_azonosító, new Versenysorozat( txtAzonosito.Text, txtMegnevezes.Text, Convert.ToInt32( lblSzam.Text ) ) );
         else
            Program.mainform.versenysorozat_panel.VersenySorozatHozzaadas( new Versenysorozat( txtAzonosito.Text, txtMegnevezes.Text, 0 ) );

         Close( );
      }
   }
}
