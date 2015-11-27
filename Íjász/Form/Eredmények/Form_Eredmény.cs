using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class Form_Eredmény : Form
   {
      public string verseny_azonosító;
      private Eredmény eredeti;
      private int összespont;

      private ComboBox combo_név;
      private ComboBox combo_íjtípus;
      private ComboBox combo_csapat;
      private TextBox box_találat_10;
      private TextBox box_találat_8;
      private TextBox box_találat_5;
      private TextBox box_mellé;
      private Label label_összes;
      private Label label_százalék;
      private CheckBox box_megjelent;

      public Form_Eredmény( string _verseny, int _összespont, Eredmény _eredmény )
      {
         eredeti = _eredmény;
         összespont = _összespont;
         verseny_azonosító = _verseny;

         InitializeForm( );
         InitializeContent( );
         InitializeData( _eredmény );

         AddAcessorHooks( );
         FormClosing += RemoveAccessorHooks;
      }

      private void InitializeForm( )
      {
         Text = "Eredmény";
         ClientSize = new System.Drawing.Size( 464 - 64, 320 + 32 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      }

      private void InitializeContent( )
      {
         Label név = new Label();
         név.Text = "Név:";
         név.Location = new System.Drawing.Point( 32, 16 + 0 * 32 );

         Label íjtípus = new Label();
         íjtípus.Text = "Íjtípus:";
         íjtípus.Location = new System.Drawing.Point( név.Location.X, 16 + 1 * 32 );

         Label csapat = new Label();
         csapat.Text = "Csapatszám:";
         csapat.Location = new System.Drawing.Point( név.Location.X, 16 + 2 * 32 );

         Label találat_10 = new Label();
         találat_10.Text = "Tíz találat:";
         találat_10.Location = new System.Drawing.Point( név.Location.X, 16 + 3 * 32 );

         Label találat_8 = new Label();
         találat_8.Text = "Nyolc találat:";
         találat_8.Location = new System.Drawing.Point( név.Location.X, 16 + 4 * 32 );

         Label találat_5 = new Label();
         találat_5.Text = "Öt találat:";
         találat_5.Location = new System.Drawing.Point( név.Location.X, 16 + 5 * 32 );

         Label mellé = new Label();
         mellé.Text = "Mellé találat:";
         mellé.Location = new System.Drawing.Point( név.Location.X, 16 + 6 * 32 );

         Label összes = new Label();
         összes.Text = "Összes találat:";
         összes.Location = new System.Drawing.Point( név.Location.X, 16 + 7 * 32 );

         Label százalék = new Label();
         százalék.Text = "Eredmény százalék:";
         százalék.Location = new System.Drawing.Point( név.Location.X, 16 + 8 * 32 );
         százalék.AutoSize = true;

         Label megjelent = new Label();
         megjelent.Text = "Megjelent:";
         megjelent.Location = new System.Drawing.Point( név.Location.X, 16 + 9 * 32 );

         ///

         combo_név = new ComboBox( );
         combo_név.Location = new System.Drawing.Point( név.Location.X + név.Size.Width + 16, név.Location.Y );
         combo_név.Size = new System.Drawing.Size( 128 + 64, 24 );
         combo_név.DropDownStyle = ComboBoxStyle.DropDownList;

         combo_íjtípus = new ComboBox( );
         combo_íjtípus.Location = new System.Drawing.Point( íjtípus.Location.X + íjtípus.Size.Width + 16, íjtípus.Location.Y );
         combo_íjtípus.Size = combo_név.Size;
         combo_íjtípus.DropDownStyle = ComboBoxStyle.DropDownList;

         combo_csapat = new ComboBox( );
         combo_csapat.Location = new System.Drawing.Point( csapat.Location.X + csapat.Size.Width + 16, csapat.Location.Y );
         combo_csapat.Size = combo_név.Size;
         combo_csapat.DropDownStyle = ComboBoxStyle.DropDownList;

         box_találat_10 = new TextBox( );
         box_találat_10.Location = new System.Drawing.Point( találat_10.Location.X + találat_10.Size.Width + 16, találat_10.Location.Y );
         box_találat_10.Size = new System.Drawing.Size( 64, 24 );

         box_találat_8 = new TextBox( );
         box_találat_8.Location = new System.Drawing.Point( találat_8.Location.X + találat_8.Size.Width + 16, találat_8.Location.Y );
         box_találat_8.Size = box_találat_10.Size;

         box_találat_5 = new TextBox( );
         box_találat_5.Location = new System.Drawing.Point( találat_5.Location.X + találat_5.Size.Width + 16, találat_5.Location.Y );
         box_találat_5.Size = box_találat_10.Size;

         box_mellé = new TextBox( );
         box_mellé.Location = new System.Drawing.Point( mellé.Location.X + mellé.Size.Width + 16, mellé.Location.Y );
         box_mellé.Size = box_találat_10.Size;

         box_találat_10.TextChanged += eredmény_számolás;
         box_találat_8.TextChanged += eredmény_számolás;
         box_találat_5.TextChanged += eredmény_számolás;
         box_mellé.TextChanged += eredmény_számolás;

         label_összes = new Label( );
         label_összes.Location = new System.Drawing.Point( összes.Location.X + összes.Size.Width + 16, összes.Location.Y );

         label_százalék = new Label( );
         label_százalék.Location = new System.Drawing.Point( százalék.Location.X + százalék.Size.Width + 16, százalék.Location.Y );

         //

         box_megjelent = new CheckBox( );
         box_megjelent.Checked = false;
         box_megjelent.Location = new System.Drawing.Point( megjelent.Location.X + megjelent.Size.Width + 16, megjelent.Location.Y - 4 );

         Button rendben = new Button();
         rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
         rendben.Text = "Rendben";
         rendben.Size = new System.Drawing.Size( 96, 32 );
         rendben.Location = new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 );
         rendben.Click += rendben_Click;

         ///

         List<Induló> indulók = Program.database.Indulók();
         foreach ( Induló current in indulók )
            combo_név.Items.Add( current.Nev );

         List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
         foreach ( Íjtípus current in íjtípusok )
            combo_íjtípus.Items.Add( current.Azonosito );

         for ( int i = 0 ; i < 35 ; i++ ) combo_csapat.Items.Add( i + 1 );

         ///

         Controls.Add( név );
         Controls.Add( íjtípus );
         Controls.Add( csapat );
         Controls.Add( találat_10 );
         Controls.Add( találat_8 );
         Controls.Add( találat_5 );
         Controls.Add( mellé );
         Controls.Add( összes );
         Controls.Add( százalék );
         Controls.Add( megjelent );

         Controls.Add( combo_név );
         Controls.Add( combo_íjtípus );
         Controls.Add( combo_csapat );
         Controls.Add( box_találat_10 );
         Controls.Add( box_találat_8 );
         Controls.Add( box_találat_5 );
         Controls.Add( box_mellé );
         Controls.Add( label_összes );
         Controls.Add( label_százalék );
         Controls.Add( box_megjelent );
         Controls.Add( rendben );
      }

      private void InitializeData( Eredmény _eredmény )
      {
         combo_név.Text = _eredmény.Nev;
         combo_név.Enabled = false;
         combo_íjtípus.Text = _eredmény.Ijtipus;
         combo_íjtípus.Enabled = false;
         combo_csapat.SelectedItem = Convert.ToInt32( _eredmény.Csapat );
         combo_csapat.Enabled = false;

         box_találat_10.Text = _eredmény.Talalat10.ToString( );
         box_találat_8.Text = _eredmény.Talalat8.ToString( );
         box_találat_5.Text = _eredmény.Talalat5.ToString( );
         box_mellé.Text = _eredmény.Melle.ToString( );
         label_összes.Text = _eredmény.Osszpont.ToString( );
         label_százalék.Text = _eredmény.Szazalek.ToString( ) + "%";
         box_megjelent.Checked = _eredmény.Megjelent;
         box_megjelent.Enabled = false;
      }

      private void AddAcessorHooks( )
      {
         Program.mainform.íjtípusok_panel.íjtípus_hozzáadva += íjtípus_hozzáadás;
         Program.mainform.íjtípusok_panel.íjtípus_módosítva += íjtípus_módosítás;
         Program.mainform.íjtípusok_panel.íjtípus_törölve += íjtípus_törlés;

         Program.mainform.indulók_panel.induló_hozzáadva += induló_hozzáadás;
         Program.mainform.indulók_panel.induló_módosítva += induló_módosítás;
         Program.mainform.indulók_panel.induló_törölve += induló_törlés;
      }

      private void RemoveAccessorHooks( object _sender, EventArgs _event )
      {
         Program.mainform.íjtípusok_panel.íjtípus_hozzáadva -= íjtípus_hozzáadás;
         Program.mainform.íjtípusok_panel.íjtípus_módosítva -= íjtípus_módosítás;
         Program.mainform.íjtípusok_panel.íjtípus_törölve -= íjtípus_törlés;

         Program.mainform.indulók_panel.induló_hozzáadva -= induló_hozzáadás;
         Program.mainform.indulók_panel.induló_módosítva -= induló_módosítás;
         Program.mainform.indulók_panel.induló_törölve -= induló_törlés;
      }

      #region EventHandlers
      public void íjtípus_hozzáadás( Íjtípus _íjtípus )
      {
         combo_íjtípus.Items.Add( _íjtípus.Azonosito );
      }

      public void íjtípus_módosítás( string _azonosító, Íjtípus _íjtípus )
      {
         if ( _azonosító != _íjtípus.Azonosito )
         {
            for ( int current = 0 ; current < combo_íjtípus.Items.Count ; ++current )
            {
               if ( _azonosító == combo_íjtípus.Items[ current ].ToString( ) )
               {
                  combo_íjtípus.Items[ current ] = _íjtípus.Azonosito;
                  break;
               }
            }
         }
      }

      public void íjtípus_törlés( string _azonosító )
      {
         combo_íjtípus.Items.Remove( _azonosító );
      }

      public void induló_hozzáadás( Induló _induló )
      {
         combo_név.Items.Add( _induló.Nev );
      }

      public void induló_módosítás( Induló _eredeti, Induló _uj )
      {
         if ( _eredeti.Nev != _uj.Nev )
         {
            for ( int current = 0 ; current < combo_név.Items.Count ; ++current )
            {
               if ( _eredeti.Nev == combo_név.Items[ current ].ToString( ) )
               {
                  combo_név.Items[ current ] = _uj.Nev;
                  break;
               }
            }
         }
      }

      public void induló_törlés( Induló _indulo )
      {
         combo_név.Items.Remove( _indulo.Nev );
      }

      private void rendben_Click( object _sender, EventArgs _event )
      {
         if ( combo_név.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva név!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( combo_íjtípus.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva íjtípus!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int találat_10;
         try { találat_10 = Convert.ToInt32( box_találat_10.Text ); if ( találat_10 < 0 ) { MessageBox.Show( "Nem megfelelő a 10 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; } }
         catch { MessageBox.Show( "Nem megfelelő a 10 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int találat_8;
         try { találat_8 = Convert.ToInt32( box_találat_8.Text ); if ( találat_8 < 0 ) { MessageBox.Show( "Nem megfelelő a 8 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; } }
         catch { MessageBox.Show( "Nem megfelelő a 8 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int találat_5;
         try { találat_5 = Convert.ToInt32( box_találat_5.Text ); if ( találat_5 < 0 ) { MessageBox.Show( "Nem megfelelő az 5 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; } }
         catch { MessageBox.Show( "Nem megfelelő az 5 találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         int találat_mellé;
         try { találat_mellé = Convert.ToInt32( box_mellé.Text ); if ( találat_mellé < 0 ) { MessageBox.Show( "Nem megfelelő a mellé találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; } }
         catch { MessageBox.Show( "Nem megfelelő a mellé találatok formátuma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         int összes = találat_10 * 10 + találat_8 * 8 + találat_5 * 5;
         int százalék = (int)(((double)összes / (összespont * 10)) * 100);

         if ( !( ( találat_10 == 0 && találat_8 == 0 && találat_5 == 0 && találat_mellé == 0 ) || ( összespont == találat_10 + találat_8 + találat_5 + találat_mellé ) ) ) { MessageBox.Show( "Nem megfelelő a lövések darabszáma!\n" + "Lövések darabszáma: " + ( összespont ).ToString( ) + "\nBeírt lövések: " + ( találat_10 + találat_8 + találat_5 + találat_mellé ).ToString( ), "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         Program.mainform.eredmények_panel.Eredmény_Módosítás( verseny_azonosító, eredeti,
             new Eredmény( eredeti.Nev,
                 eredeti.Sorszam,
                 eredeti.Ijtipus,
                 eredeti.Csapat,
                 Convert.ToInt32( box_találat_10.Text ),
                 Convert.ToInt32( box_találat_8.Text ),
                 Convert.ToInt32( box_találat_5.Text ),
                 Convert.ToInt32( box_mellé.Text ),
                 összes,
                 százalék,
                 box_megjelent.Checked,
                 eredeti.KorosztalyModositott,
                 eredeti.KorosztalyAzonosito ) );

         Close( );
      }

      private void eredmény_számolás( object _sender, EventArgs _event )
      {
         int találat_10;
         try { találat_10 = Convert.ToInt32( box_találat_10.Text ); if ( találat_10 < 0 ) { return; } }
         catch { return; }
         int találat_8;
         try { találat_8 = Convert.ToInt32( box_találat_8.Text ); if ( találat_8 < 0 ) { return; } }
         catch { return; }
         int találat_5;
         try { találat_5 = Convert.ToInt32( box_találat_5.Text ); if ( találat_5 < 0 ) { return; } }
         catch { return; }
         int találat_mellé;
         try { találat_mellé = Convert.ToInt32( box_mellé.Text ); if ( találat_mellé < 0 ) { return; } }
         catch { return; }

         label_összes.Text = ( Convert.ToInt32( box_találat_10.Text ) * 10 + Convert.ToInt32( box_találat_8.Text ) * 8 + Convert.ToInt32( box_találat_5.Text ) * 5 ).ToString( );
         List<Verseny> versenyek = Program.database.Versenyek();
         foreach ( Verseny item in versenyek )
         {
            if ( item.Azonosito == verseny_azonosító ) { label_százalék.Text = ( ( int )( ( ( double )Convert.ToInt32( label_összes.Text ) / ( item.Osszes * 10 ) ) * 100 ) ).ToString( ) + "%"; return; }
         }
      }
      #endregion
   }
}
