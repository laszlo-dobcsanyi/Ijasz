using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class Form_Íjtípus : Form
   {
      private string eredeti_azonosító = null;

      private TextBox text_íjazon;
      private TextBox text_megn;
      private TextBox text_listsorszam;
      private Label label_eredmények;

      public Form_Íjtípus( )
      {
         InitializeForm( );
         InitializeContent( );
         InitializeData( );
      }

      public Form_Íjtípus( Íjtípus _íjtípus )
      {
         InitializeForm( );
         InitializeContent( );
         InitializeData( _íjtípus );
      }

      private void InitializeForm( )
      {
         Text = "Íjtípus";
         ClientSize = new System.Drawing.Size( 400 - 64, 128 + 64 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( )
      {
         Label label_íjazon = new Label();
         label_íjazon.Text = "Azonosító:";
         label_íjazon.AutoSize = true;
         label_íjazon.Location = new System.Drawing.Point( 16, 16 + 0 * 32 );

         Label label_megnevezés = new Label();
         label_megnevezés.Text = "Megnevezés:";
         label_megnevezés.AutoSize = true;
         label_megnevezés.Location = new System.Drawing.Point( label_íjazon.Location.X, 16 + 1 * 32 );

         Label label_listsorszam = new Label();
         label_listsorszam.Text = "Listázási sorszám:";
         label_listsorszam.Location = new System.Drawing.Point( label_íjazon.Location.X, 16 + 2 * 32 );

         Label eredmények = new Label();
         eredmények.Text = "Eredmények:";
         eredmények.Location = new System.Drawing.Point( label_íjazon.Location.X, 16 + 3 * 32 );

         ///

         text_íjazon = new TextBox( );
         text_íjazon.Location = new System.Drawing.Point( label_íjazon.Location.X + label_íjazon.Size.Width + 16, 16 + 0 * 32 );
         text_íjazon.MaxLength = 10;

         text_megn = new TextBox( );
         text_megn.Location = new System.Drawing.Point( text_íjazon.Location.X, 16 + 1 * 32 );
         text_megn.MaxLength = 30;

         text_listsorszam = new TextBox( );
         text_listsorszam.Location = new System.Drawing.Point( text_íjazon.Location.X, 16 + 2 * 32 );

         label_eredmények = new Label( );
         label_eredmények.Location = new System.Drawing.Point( eredmények.Location.X + eredmények.Size.Width + 16, eredmények.Location.Y );

         Button rendben = new Button();
         rendben.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
         rendben.Text = "Rendben";
         rendben.Size = new System.Drawing.Size( 96, 32 );
         rendben.Location = new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 );
         rendben.Click += rendben_Click;

         ///

         Controls.Add( label_íjazon );
         Controls.Add( label_megnevezés );
         Controls.Add( label_listsorszam );
         //Controls.Add(eredmények);

         Controls.Add( text_íjazon );
         Controls.Add( text_megn );
         Controls.Add( text_listsorszam );
         //Controls.Add(label_eredmények);
         Controls.Add( rendben );
      }

      private void InitializeData( )
      {
         text_íjazon.Text = "";
         text_íjazon.Enabled = true;
         text_listsorszam.Text = "";
         text_megn.Text = "";
         label_eredmények.Text = "0";
      }

      private void InitializeData( Íjtípus _íjtípus )
      {
         eredeti_azonosító = _íjtípus.Azonosito;

         text_íjazon.Text = _íjtípus.Azonosito;
         text_íjazon.Enabled = ( _íjtípus.Eredmenyek > 0 ? false : true );
         text_listsorszam.Text = _íjtípus.Sorszam.ToString( );
         text_megn.Text = _íjtípus.Megnevezes;
         label_eredmények.Text = _íjtípus.Eredmenyek.ToString( );
      }

      #region EventHandlers
      private void rendben_Click( object _sender, EventArgs _event )
      {
         if ( text_íjazon.Text.Length == 0 || text_íjazon.Text.Length > 10 ) { MessageBox.Show( "Íjtípusazonosító hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( text_íjazon.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( text_megn.Text.Length == 0 || text_megn.Text.Length > 30 ) { MessageBox.Show( "Íjtípus megnevezésének hossza nem megfelelő!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( text_megn.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         try { Convert.ToInt32( text_listsorszam.Text ); }
         catch { MessageBox.Show( "Nem megfelelő a sorszám formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( eredeti_azonosító != null )
            Program.mainform.íjtípusok_panel.Íjtípus_Módosítás( eredeti_azonosító, new Íjtípus( text_íjazon.Text, text_megn.Text, Convert.ToInt32( text_listsorszam.Text ), -666 ) );
         else
            Program.mainform.íjtípusok_panel.Íjtípus_Hozzáadás( new Íjtípus( text_íjazon.Text, text_megn.Text, Convert.ToInt32( text_listsorszam.Text ), 0 ) );

         Close( );
      }
      #endregion
   }
}
