using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Íjász
{
   public sealed class Form_Verseny : Form
   {
      private string eredeti_azonosító = null;
      private string eredeti_versenysorozat = null;

      private TextBox txtAzonosito;
      private TextBox txtMegnevezes;
      private TextBox txtLovesek;
      private TextBox txtAllomasok;
      private DateTimePicker dátumválasztó;
      private ComboBox cboVersenySorozat;
      private CheckBox chkDuplaBeirolap;
      private Label lblIndulok;
      private Label lblLezarva;

      public Form_Verseny( )
      {
         InitializeForm( );
         InitializeContent( );
         InitializeData( );
      }

      public Form_Verseny( Verseny _verseny )
      {
         eredeti_azonosító = _verseny.Azonosito;
         eredeti_versenysorozat = _verseny.VersenySorozat;

         InitializeForm( _verseny );
         InitializeContent( _verseny );
         InitializeData( _verseny );
      }

      #region Verseny Hozzáadás
      private void InitializeForm( )
      {
         Text = "Verseny";
         ClientSize = new Size( 400 - 64, 280 );
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

         Label lblDatum = new iLabel( "Dátum:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

         Label lblVersenySorozat = new iLabel( "Versenysorozat:",
                                                    new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                                    this );

         Label lblLovesek = new iLabel( "Lövések száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                                this );

         Label lblAllomasok = new iLabel( "Állomások száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                                this );

         Label lblDuplaBeirolap = new iLabel( "Dupla beírólap:",
                                                    new Point( lblAzonosito.Location.X, 16 + 6 * 32 ),
                                                    this );

         txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Size.Width + 16 + 32, lblAzonosito.Location.Y ),
                                     10,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                     30,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         dátumválasztó = new DateTimePicker( );
         dátumválasztó.Location = new Point( txtAzonosito.Location.X, lblDatum.Location.Y );
         dátumválasztó.Size = txtAzonosito.Size;
         dátumválasztó.Value = DateTime.Now;

         cboVersenySorozat = new iComboBox( new Point( txtAzonosito.Location.X, lblVersenySorozat.Location.Y ),
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

         cboVersenySorozat.Items.Add( "" );
         List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
         foreach ( Versenysorozat current in versenysorozatok ) { cboVersenySorozat.Items.Add( current.azonosító ); }

         txtLovesek = new iTextBox( new Point( txtAzonosito.Location.X, lblLovesek.Location.Y ),
                                     null,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         txtAllomasok = new iTextBox( new Point( txtAzonosito.Location.X, lblAllomasok.Location.Y ),
                                     30,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         chkDuplaBeirolap = new iCheckBox( null,
                                         new Point( txtAzonosito.Location.X, lblDuplaBeirolap.Location.Y ),
                                         null,
                                         this );
         chkDuplaBeirolap.Size = txtAzonosito.Size;

         Button btnRendben = new iButton( "Rendben",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

         Controls.Add( dátumválasztó );
      }

      private void InitializeData( )
      {
         dátumválasztó.Value = DateTime.Now;
      }
      #endregion

      #region Verseny Módosítás
      private void InitializeForm( Verseny _verseny )
      {
         Text = "Verseny";
         ClientSize = new Size( 400 - 64, 358 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( Verseny _verseny )
      {
         Label lblAzonosito = new iLabel( "Azonosító:",
                                                new Point( 16, 16 + 0 * 32 ),
                                                this );

         Label lblMegnevezes = new iLabel( "Megnevezés:",
                                                new Point( lblAzonosito.Location.X, 16 + 1 * 32 ),
                                                this );

         Label lblDatum = new iLabel( "Dátum:",
                                            new Point( lblAzonosito.Location.X, 16 + 2 * 32 ),
                                            this );

         Label lblVersenySorozat = new iLabel( "Versenysorozat:",
                                                    new Point( lblAzonosito.Location.X, 16 + 3 * 32 ),
                                                    this );

         Label lblLovesek = new iLabel( "Lövések száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 4 * 32 ),
                                                this );

         Label lblAllomasok = new iLabel( "Állomások száma:",
                                                new Point( lblAzonosito.Location.X, 16 + 5 * 32 ),
                                                this );

         lblIndulok = new iLabel( "Indulók száma:",
                                         new Point( lblAzonosito.Location.X, 16 + 6 * 32 ),
                                         this );

         Label lblDuplaBeirolap = new iLabel( "Dupla beírólap:",
                                                    new Point( lblAzonosito.Location.X, 16 + 7 * 32 ),
                                                    this );

         lblLezarva = new iLabel( "Lezárva:",
                                         new Point( lblAzonosito.Location.X, 16 + 8 * 32 ),
                                         this );

         txtAzonosito = new iTextBox( new Point( lblAzonosito.Location.X + lblAzonosito.Size.Width + 16 + 32, lblAzonosito.Location.Y ),
                                     10,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         txtMegnevezes = new iTextBox( new Point( txtAzonosito.Location.X, lblMegnevezes.Location.Y ),
                                     30,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         dátumválasztó = new DateTimePicker( );
         dátumválasztó.Location = new Point( txtAzonosito.Location.X, lblDatum.Location.Y );
         dátumválasztó.Size = txtAzonosito.Size;
         dátumválasztó.Value = DateTime.Now;

         cboVersenySorozat = new iComboBox( new Point( txtAzonosito.Location.X, lblVersenySorozat.Location.Y ),
                                            new Size( 128 + 64, 24 ),
                                            null,
                                            this );

         cboVersenySorozat.Items.Add( "" );
         List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
         foreach ( Versenysorozat current in versenysorozatok ) { cboVersenySorozat.Items.Add( current.azonosító ); }

         txtLovesek = new iTextBox( new Point( txtAzonosito.Location.X, lblLovesek.Location.Y ),
                                     null,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         txtAllomasok = new iTextBox( new Point( txtAzonosito.Location.X, lblAllomasok.Location.Y ),
                                     30,
                                     new Size( 128 + 64, 24 ),
                                     null,
                                     this );

         chkDuplaBeirolap = new iCheckBox( null,
                                         new Point( txtAzonosito.Location.X, lblDuplaBeirolap.Location.Y ),
                                         null,
                                         this );
         chkDuplaBeirolap.Size = txtAzonosito.Size;

         lblIndulok = new iLabel( null,
                                 new Point( txtAzonosito.Location.X, lblIndulok.Location.Y ),
                                 this );
         lblIndulok.Size = txtAzonosito.Size;

         lblLezarva = new iLabel( null,
                                 new Point( txtAzonosito.Location.X, lblLezarva.Location.Y ),
                                 this );

         Button btnRendben = new iButton( "Rendben",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

         Controls.Add( dátumválasztó );
      }

      private void InitializeData( Verseny _verseny )
      {
         txtAzonosito.Text = _verseny.Azonosito;
         txtAzonosito.Enabled = ( _verseny.Indulok == 0 ) ? true : false;
         txtMegnevezes.Text = _verseny.Megnevezes;
         dátumválasztó.Value = DateTime.Parse( _verseny.Datum );
         cboVersenySorozat.Text = _verseny.VersenySorozat;
         txtLovesek.Text = ( _verseny.Osszes ).ToString( );
         txtLovesek.Enabled = ( _verseny.Indulok == 0 ) ? true : false;
         txtAllomasok.Text = ( _verseny.Allomasok ).ToString( );
         txtAllomasok.Enabled = ( _verseny.Indulok == 0 ) ? true : false;
         lblIndulok.Text = _verseny.Indulok.ToString( );
         lblLezarva.Text = _verseny.Lezarva ? "Igen" : "Nem";
         chkDuplaBeirolap.Checked = _verseny.DublaBeirlap;
      }
      #endregion

      #region EventHandlers

      public void VersenySorozatHozzaadas( Versenysorozat _versenysorozat )
      {
         cboVersenySorozat.Items.Add( _versenysorozat.megnevezés + " (" + _versenysorozat.azonosító + ")" );
      }

      public void VersenySorozatModositas( string _azonosító, Versenysorozat _versenysorozat )
      {
         if ( _azonosító != _versenysorozat.azonosító )
         {
            for ( int current = 0 ; current < cboVersenySorozat.Items.Count ; ++current )
            {
               if ( _azonosító == cboVersenySorozat.Items[ current ].ToString( ) )
               {
                  cboVersenySorozat.Items[ current ] = _versenysorozat.azonosító;
                  return;
               }
            }
         }
      }

      public void VeresenySorozatTorles( string _azonosító )
      {
         cboVersenySorozat.Items.Remove( _azonosító );
      }

      public void EredmenyBeiras( string _azonosító, Database.BeírásEredmény _beírás )
      {
         if ( _beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT )
         {
            if ( _azonosító == eredeti_azonosító ) lblIndulok.Text = ( ( Convert.ToInt32( lblIndulok.Text ) ) + 1 ).ToString( );
         }
      }

      public void EredmenyTorles( string _azonosító, Eredmény _eredmény )
      {
         if ( _azonosító == eredeti_azonosító )
         {
            lblIndulok.Text = ( ( Convert.ToInt32( lblIndulok.Text ) ) - 1 ).ToString( );
         }
      }

      private void btnRendben_Click( object _sender, EventArgs _event )
      {
         Regex rgx = new Regex("[^a-zA-Z0-9]");
         txtAzonosito.Text = rgx.Replace( txtAzonosito.Text, "" );

         if ( !Database.IsCorrectSQLText( txtMegnevezes.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int összes; try { összes = Convert.ToInt32( txtLovesek.Text ); } catch { MessageBox.Show( "Nem szám található a lövéseknél!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( összes < 1 ) { MessageBox.Show( "Túl kevés a lövések száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int állomások; try { állomások = Convert.ToInt32( txtAllomasok.Text ); } catch { MessageBox.Show( "Nem szám található az állomásoknál!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         if ( eredeti_azonosító != null )
         {
            if ( ( 0 < Convert.ToInt32( lblIndulok.Text ) ) && ( eredeti_azonosító != txtAzonosito.Text ) )
            { MessageBox.Show( "Ez a verseny nem átnevezhető!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; };

            // TODO: ezt sem kéne külön csinálni!
            if ( cboVersenySorozat.Text != eredeti_versenysorozat )
            {
               Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés( eredeti_versenysorozat );
               Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés( cboVersenySorozat.Text );
            }

            Program.mainform.verseny_panel.Verseny_Módosítás( eredeti_azonosító, new Verseny( txtAzonosito.Text,
                                                                                            txtMegnevezes.Text,
                                                                                            dátumválasztó.Value.ToShortDateString( ),
                                                                                            cboVersenySorozat.Text,
                                                                                            összes,
                                                                                            állomások, Convert.ToInt32( lblIndulok.Text ),
                                                                                            lblLezarva.Text == "Igen" ? true : false,
                                                                                            chkDuplaBeirolap.Checked ? true : false ) );

         }
         else
         {
            // TODO: ezt sem kéne külön csinálni!
            Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés( cboVersenySorozat.Text );
            Program.mainform.verseny_panel.Verseny_Hozzáadás( new Verseny( txtAzonosito.Text,
                                                                        txtMegnevezes.Text,
                                                                        dátumválasztó.Value.ToShortDateString( ),
                                                                        cboVersenySorozat.Text,
                                                                        összes,
                                                                        állomások,
                                                                        0,
                                                                        false,
                                                                        chkDuplaBeirolap.Checked ? true : false ) );
         }

         Close( );
      }
      #endregion
   }
}
