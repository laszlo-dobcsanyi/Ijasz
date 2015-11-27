using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász {
   public sealed class Form_Induló : Form {
      private string eredeti_név = null;

      private TextBox box_név;
      private TextBox box_nem;
      private DateTimePicker date_születés;
      private TextBox box_engedély;
      private ComboBox cboEgyesulet;
      private Label eredmények_száma;

      public Form_Induló( ) {
         InitializeForm( );
         InitializeContent( );
         InitializeData( );
      }

      void EnterFullScreen( ) {
         this.WindowState = FormWindowState.Normal;
         this.FormBorderStyle = FormBorderStyle.None;
         this.WindowState = FormWindowState.Maximized;
      }

      void LeaveFullScreen( ) {
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
         this.WindowState = FormWindowState.Normal;
      }

      public Form_Induló( Induló _induló ) {
         eredeti_név = _induló.Nev;

         InitializeForm( );
         InitializeContent( );
         InitializeData( _induló );
      }

      private void InitializeForm( ) {
         Text = "Induló";
         ClientSize = new System.Drawing.Size( 400 - 32, 262 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( ) {
         Label név = new Label( );
         név.Text = "Név:";
         név.Location = new System.Drawing.Point( 16, 16 + 0 * 32 );

         Label nem = new Label( );
         nem.Text = "Nem:";
         nem.Location = new System.Drawing.Point( név.Location.X, 16 + 1 * 32 );

         Label születés = new Label( );
         születés.Text = "Születési idő:";
         születés.Location = new System.Drawing.Point( név.Location.X, 16 + 2 * 32 );

         Label engedély = new Label( );
         engedély.Text = "Engedélyszám:";
         engedély.Location = new System.Drawing.Point( név.Location.X, 16 + 3 * 32 );

         Label egyesület = new Label( );
         egyesület.Text = "Egyesület:";
         egyesület.Location = new System.Drawing.Point( név.Location.X, 16 + 4 * 32 );

         Label eredmények = new Label( );
         eredmények.Text = "Eredmények:";
         eredmények.Location = new System.Drawing.Point( név.Location.X, 16 + 5 * 32 );

         ///

         box_név = new TextBox( );
         box_név.Location = new System.Drawing.Point( név.Location.X + név.Size.Width + 16, név.Location.Y );
         box_név.Size = new System.Drawing.Size( 128 + 64, 24 );
         box_név.MaxLength = 30;

         box_nem = new TextBox( );
         box_nem.Location = new System.Drawing.Point( nem.Location.X + nem.Size.Width + 16, nem.Location.Y );
         box_nem.Size = new System.Drawing.Size( 64, 24 );
         box_nem.MaxLength = 10;

         date_születés = new DateTimePicker( );
         date_születés.Location = new System.Drawing.Point( születés.Location.X + születés.Size.Width + 16, születés.Location.Y );
         date_születés.Size = box_név.Size;
         date_születés.Value = DateTime.Now;

         box_engedély = new TextBox( );
         box_engedély.Location = new System.Drawing.Point( engedély.Location.X + engedély.Size.Width + 16, engedély.Location.Y );
         box_engedély.Size = box_név.Size;
         box_engedély.MaxLength = 30;

         cboEgyesulet = new ComboBox( );
         cboEgyesulet.Location = new System.Drawing.Point( egyesület.Location.X + egyesület.Size.Width + 16, egyesület.Location.Y );
         cboEgyesulet.Size = box_név.Size;
         cboEgyesulet.DropDownStyle = ComboBoxStyle.DropDownList;

         List<Egyesulet> egyesuletek = Program.database.Egyesuletek( );

         foreach ( Egyesulet item in egyesuletek ) {
            cboEgyesulet.Items.Add( item.Azonosito );
         }

         eredmények_száma = new Label( );
         eredmények_száma.Location = new System.Drawing.Point( eredmények.Location.X + eredmények.Size.Width + 16, eredmények.Location.Y );

         Button rendben = new Button( );
         rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
         rendben.Text = "Rendben";
         rendben.Size = new System.Drawing.Size( 96, 32 );
         rendben.Location = new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 );
         rendben.Click += rendben_Click;

         Button btnEgyesulet = new iButton( "Új Egyesület",
                                                    new Point( ClientRectangle.Width - 96 - 96 - 32, ClientRectangle.Height - 32 - 16 ),
                                                    new Size( 96, 32 ),
                                                    btnEgyesulet_Click,
                                                    this );

         ///

         Controls.Add( név );
         Controls.Add( nem );
         Controls.Add( születés );
         Controls.Add( engedély );
         Controls.Add( egyesület );
         Controls.Add( eredmények );

         Controls.Add( box_név );
         Controls.Add( box_nem );
         Controls.Add( date_születés );
         Controls.Add( box_engedély );
         Controls.Add( cboEgyesulet );
         Controls.Add( eredmények_száma );
         Controls.Add( rendben );
      }

      private void InitializeData( ) {
         box_név.Text = "";
         box_név.Enabled = true;
         box_nem.Text = "";
         date_születés.Value = DateTime.Now;
         box_engedély.Text = "";
         eredmények_száma.Text = "0";
      }

      private void InitializeData( Induló _induló ) {
         box_név.Text = _induló.Nev;
         box_nem.Text = _induló.Nem == "N" ? "Nő" : "Férfi";
         box_nem.Enabled = ( _induló.Eredmenyek > 0 ? false : true );
         date_születés.Value = DateTime.Parse( _induló.SzuletesiDatum );
         date_születés.Enabled = ( _induló.Eredmenyek > 0 ? false : true );
         box_engedély.Text = _induló.Engedely;
         eredmények_száma.Text = _induló.Eredmenyek.ToString( );
         cboEgyesulet.Text = _induló.Egyesulet;
      }

      private void rendben_Click( object _sender, EventArgs _event ) {
         if ( date_születés.Value.Year == DateTime.Now.Year && date_születés.Value.Month == DateTime.Now.Month && date_születés.Value.Day == DateTime.Now.Day ) { MessageBox.Show( "A születési dátum nem lehet a mai nap!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !( 0 < box_név.Text.Length && box_név.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő a név hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( box_név.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !( 0 < box_nem.Text.Length && box_nem.Text.Length <= 10 ) ) { MessageBox.Show( "Nem megfelelő a nem hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         bool nő = false;
         if ( box_nem.Text.ToLower( ) == "n" || box_nem.Text.ToLower( ) == "nő" )
            nő = true;
         else if ( !( box_nem.Text.ToLower( ) == "f" || box_nem.Text.ToLower( ) == "férfi" ) ) { MessageBox.Show( "Nem megfelelő nem!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !( box_engedély.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő az engedély hossza (0 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( box_engedély.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( eredeti_név != null ) {
            Induló? eredeti = Program.database.Induló( eredeti_név );
            Program.mainform.indulók_panel.Induló_Módosítás( eredeti.Value,
                                                            new Induló( box_név.Text,
                                                                        ( nő ? "N" : "F" ),
                                                                        date_születés.Value.ToShortDateString( ),
                                                                        box_engedély.Text,
                                                                        cboEgyesulet.Text,
                                                                        Convert.ToInt32( eredmények_száma.Text ) ) );
         }
         else {
            Program.mainform.indulók_panel.Induló_Hozzáadás( new Induló( box_név.Text, ( nő ? "N" : "F" ), date_születés.Value.ToShortDateString( ), box_engedély.Text, cboEgyesulet.Text, 0 ) );
         }

         Close( );
      }

      private void
      btnEgyesulet_Click( object _sender, EventArgs _event ) {
         Form_Egyesulet EgyesuletForm = new Form_Egyesulet( );
         EgyesuletForm.Show( );
         EgyesuletForm.FormClosed += EgyesuletForm_FormClosed;
      }

      void EgyesuletForm_FormClosed( object sender, FormClosedEventArgs e ) {
         cboEgyesulet.Items.Clear( );
         List<Egyesulet> egyesuletek = Program.database.Egyesuletek( );

         foreach ( Egyesulet item in egyesuletek ) {
            cboEgyesulet.Items.Add( item.Azonosito );
         }

         if ( cboEgyesulet.Items.Count != 0 )
            cboEgyesulet.SelectedIndex = cboEgyesulet.Items.Count - 1;
      }
   }
}
