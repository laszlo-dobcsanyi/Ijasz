using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class Form_Induló_Teszt : Form
   {
      public  Form_Csapatlista csapatlista_form;

      public static int lastindex;

      public ComboBox cboVerseny;
      private Label lblIndulo;
      private ComboBox cboIjtipus;
      private ComboBox cboCsapat;
      private CheckBox chkMegjelent;
      private CheckBox chkKorosztalyFeluliras;
      private ComboBox cboKorosztaly;

      public Form_Induló_Teszt( string _Indulo, int _lastindex )
      {

         InitializeForm( );
         InitializeContent( _Indulo, _lastindex );
         InitializeData( );
      }

      private void InitializeForm( )
      {
         Text = "Beírás";
         ClientSize = new System.Drawing.Size( 464 - 64, 300 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( string _Indulo,int _lastindex )
      {
         lastindex = _lastindex;

         Label lblNev = new iLabel(
               "Név:",
               new Point( 32, 16 + 0 * 32 ),
               this
            );

         Label lblVerseny = new iLabel(
               "Verseny:",
               new Point( lblNev.Location.X, 16 + 1 * 32 ),
               this
            );

         Label lblIjtipus = new iLabel(
               "Íjtípus:",
               new Point( lblNev.Location.X, 16 + 2 * 32 ),
               this
            );

         Label lblCsapat = new iLabel(
               "Csapatszám:",
               new Point( lblNev.Location.X, 16 + 3 * 32 ),
               this
            );

         Label lblMegjelent = new iLabel(
               "Megjelent:",
               new Point( lblNev.Location.X, 16 + 4 * 32 ),
               this
            );

         Label lblKorosztalyFeluliras = new iLabel(
               "Korosztály felülírása:",
               new Point( lblNev.Location.X, 16 + 5 * 32 ),
               this
            );

         Label lblKorosztalyAzonosito = new iLabel(
               "Új Korosztály azonosítója:",
               new Point( lblNev.Location.X, 16 + 6 * 32 ),
               this
            );

         lblIndulo = new iLabel(
            _Indulo,
            new Point( lblNev.Location.X + lblNev.Size.Width + 16 + 3 * 32, lblNev.Location.Y ),
            this
         );
         lblIndulo.AutoSize = false;
         lblIndulo.Size = new Size( 128 + 64, 24 );

         cboVerseny = new iComboBox(
            new Point( lblIndulo.Location.X, lblVerseny.Location.Y ),
            lblIndulo.Size,
            cboVerseny_SelectedIndexChanged,
            this
         );

         List<Verseny> versenyek = Program.database.Versenyek( );
         foreach ( Verseny current in versenyek ) { cboVerseny.Items.Add( current.Azonosito ); }

         cboIjtipus = new iComboBox(
            new Point( lblIndulo.Location.X, lblIjtipus.Location.Y ),
            lblIndulo.Size,
            null,
            this
         );

         List<Íjtípus> ijtipusok = Program.database.Íjtípusok( );
         foreach ( Íjtípus current in ijtipusok ) { cboIjtipus.Items.Add( current.Azonosito ); }

         cboCsapat = new iComboBox(
            new Point( lblIndulo.Location.X, lblCsapat.Location.Y ),
            lblIndulo.Size,
            null,
            this
         );

         for ( int i = 0 ; i < 45 ; i++ ) { cboCsapat.Items.Add( i + 1 ); }
         cboCsapat.SelectedItem = cboCsapat.Items[ 0 ];

         chkMegjelent = new iCheckBox(
            "",
            new Point( lblIndulo.Location.X, lblMegjelent.Location.Y ),
            null,
            this
         );

         chkKorosztalyFeluliras = new iCheckBox(
            null,
            new Point( lblIndulo.Location.X, lblKorosztalyFeluliras.Location.Y ),
            chkKorosztalyFeluliras_Click,
            this
         );

         cboKorosztaly = new iComboBox(
            new Point( lblIndulo.Location.X, lblKorosztalyAzonosito.Location.Y ),
            lblIndulo.Size,
            null,
            this
         );
         cboKorosztaly.Enabled = false;

         Button btnRendben = new iButton(
               "Rendben",
               new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
               new Size( 96, 32 ),
               btnRendben_Click,
               this
            );

         Button btnCsapatok = new iButton(
               "Csapatok megtekintése",
               new Point( ClientRectangle.Width - 96 - btnRendben.Width - 32, ClientRectangle.Height - 32 - 16 ),
               new Size( 96, 32 ),
               btnCsapatok_Click,
               this
            );
      }

      private void
      InitializeData( )
      {
         // Lefut a combo_verseny_SelectedIndexChanged, majd az beállít mindent!
         cboVerseny.SelectedIndex = lastindex;
         cboKorosztaly.Items.Clear( );
         List<Korosztály> korosztalyok = Program.database.Korosztályok( cboVerseny.Text );
         foreach ( Korosztály item in korosztalyok )
         {
            cboKorosztaly.Items.Add( item.Megnevezes );
         }
         if ( cboKorosztaly.Items.Count != 0 ) { cboKorosztaly.SelectedIndex = 0; }
         Eredmény? eredmény = Program.database.Eredmény( cboVerseny.Text, lblIndulo.Text );
         if ( eredmény != null ) { cboKorosztaly.Text = eredmény.Value.KorosztalyAzonosito; }
      }

      #region EventHandlers

      private void
      cboVerseny_SelectedIndexChanged( object _sender, EventArgs _event )
      {
         Eredmény? eredmény = Program.database.Eredmény( cboVerseny.Text, lblIndulo.Text );
         if ( eredmény != null )
         {
            cboIjtipus.SelectedItem = eredmény.Value.Ijtipus;
            cboCsapat.SelectedItem = eredmény.Value.Csapat;
            chkMegjelent.Checked = eredmény.Value.Megjelent;
            chkKorosztalyFeluliras.Checked = eredmény.Value.KorosztalyModositott;
            cboKorosztaly.Text = eredmény.Value.KorosztalyAzonosito;
         }
         else
         {
            cboIjtipus.SelectedIndex = 0;
            cboCsapat.SelectedIndex = 0;
            chkMegjelent.Checked = false;
         }
      }

      private void
      btnRendben_Click( object _sender, EventArgs _event )
      {
         if ( cboVerseny.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( cboIjtipus.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva íjtípus!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( !Database.IsCorrectSQLText( cboCsapat.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( Program.database.Verseny_Lezárva( cboVerseny.Text ) ) { MessageBox.Show( "A verseny már le van zárva!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         List<Korosztály> korosztalyok = Program.database.Korosztályok( cboVerseny.Text );
         string KOAZON = null;
         if ( chkKorosztalyFeluliras.Checked == true )
         {
            foreach ( Korosztály korosztaly in korosztalyok )
            {
               if ( korosztaly.Megnevezes == cboKorosztaly.Text ) { KOAZON = korosztaly.Azonosito; }
            }
         }
         else
         {
            Eredmény? temp = Program.database.Eredmény(cboVerseny.Text,lblIndulo.Text);
            if ( temp != null ) { KOAZON = temp.Value.KorosztalyAzonosito; }
         }

         Program.mainform.eredmények_panel.Eredmény_Beírás(
            lblIndulo.Text,
            cboVerseny.Text,
            cboIjtipus.Text,
            cboCsapat.SelectedIndex + 1,
            chkMegjelent.Checked,
            chkKorosztalyFeluliras.Checked,
            KOAZON
         );

         if ( MessageBox.Show( "Nyomtassak beírólapot ennek a versenyzőnek: " + lblIndulo.Text + "?", "Nyomtatás", MessageBoxButtons.YesNo ) == DialogResult.Yes )
         {
            Eredmény? eredmény = Program.database.Eredmény( cboVerseny.Text, lblIndulo.Text );

            foreach ( DataRow item in Program.mainform.verseny_panel.data.Rows )
            {
               Verseny? verseny = Program.database.Verseny( cboVerseny.Text );
               if ( ( string )item[ 0 ] == cboVerseny.Text && verseny.Value.DublaBeirlap == false )
               {
                  Nyomtat.Print( Nyomtat.NyomtatBeirolap( cboVerseny.Text, eredmény.Value ) );
               }
               else if ( ( string )item[ 0 ] == cboVerseny.Text && verseny.Value.DublaBeirlap == true )
               {
                  Nyomtat.Print( Nyomtat.NyomtatBeirolap( cboVerseny.Text, eredmény.Value ) );
                  Nyomtat.Print( Nyomtat.NyomtatBeirolap( cboVerseny.Text, eredmény.Value ) );
               }
            }

         }

         lastindex = cboVerseny.SelectedIndex;
         Close( );
      }

      private void
      btnCsapatok_Click( object sender, EventArgs e )
      {
         if ( cboVerseny.SelectedItem == null )
         {
            return;
         }
         csapatlista_form = new Form_Csapatlista( cboVerseny.Text );
         csapatlista_form.ShowDialog( );
         return;
      }

      private void
      chkKorosztalyFeluliras_Click( object _sender, EventArgs _event )
      {
         CheckBox chkTemp = _sender as CheckBox;
         if ( chkTemp.Checked == true )
         {
            cboKorosztaly.Enabled = true;
         }
         else
         {
            cboKorosztaly.Enabled = false;
         }

         cboKorosztaly.Items.Clear( );
         List<Korosztály> korosztalyok = Program.database.Korosztályok( cboVerseny.Text );
         foreach ( Korosztály item in korosztalyok )
         {
            cboKorosztaly.Items.Add( item.Megnevezes );
         }
         if ( cboKorosztaly.Items.Count != 0 ) { cboKorosztaly.SelectedIndex = 0; }
      }
      #endregion
   }

}
