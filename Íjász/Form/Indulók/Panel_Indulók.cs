using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Íjász
{
   public struct Induló
   {
      public string Nev;
      public string Nem;
      public string SzuletesiDatum;
      public string Engedely;
      public string Egyesulet;
      public int Eredmenyek;

      public Induló( string _Nev,
                    string _Nem,
                    string _SzuletesiDatum,
                    string _Engedely,
                    string _Egyesulet,
                    int _Eredmenyek )
      {
         Nev = _Nev;
         Nem = _Nem;
         SzuletesiDatum = _SzuletesiDatum;
         Engedely = _Engedely;
         Egyesulet = _Egyesulet;
         Eredmenyek = _Eredmenyek;
      }

        
    public override string ToString( ) {
            return "Indulo" + Environment.NewLine + 
                    "{" + Environment.NewLine +
                    "   Nev='" + Nev + '\'' + Environment.NewLine +
                    "   Nem='" + Nem + '\'' + Environment.NewLine +
                    "   SzuletesiDatum='" + SzuletesiDatum + '\'' + Environment.NewLine +
                    "   Engedely='" + Engedely + '\'' + Environment.NewLine +
                    "   Egyesulet='" + Egyesulet + '\'' + Environment.NewLine +
                    "   Eredmenyek=" + Eredmenyek + Environment.NewLine +
                    '}';
        }
    }

   public delegate void Induló_Hozzáadva( Induló _induló );
   public delegate void Induló_Módosítva( Induló _eredeti, Induló _uj );
   public delegate void Induló_Átnevezve( string _eredeti_név, string _új_név );
   public delegate void Induló_Törölve( Induló _indulo );

   public sealed class Panel_Indulók : Control
   {
      public Form_Induló_Teszt eredmény_form;

      public Induló_Hozzáadva induló_hozzáadva;
      public Induló_Módosítva induló_módosítva;
      public Induló_Átnevezve induló_átnevezve;
      public Induló_Törölve induló_törölve;

      private DataTable data;
      private DataGridView table;
      private TextBox keresés;

      public static int lastindex;

      public Panel_Indulók( )
      {
         InitializeContent( );
      }

      private void InitializeContent( )
      {
         table = new DataGridView( );
         table.DataSource = CreateSource( );
         table.Dock = DockStyle.Left;
         table.RowHeadersVisible = false;
         table.AllowUserToResizeRows = false;
         table.AllowUserToResizeColumns = false;
         table.AllowUserToAddRows = false;
         table.Width = 703;
         table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         table.MultiSelect = true;
         table.ReadOnly = true;
         table.DataBindingComplete += table_DataBindingComplete;
         table.CellDoubleClick += módosítás_Click;

         ///

         Button hozzáadás = new Button();
         hozzáadás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
         hozzáadás.Text = "Hozzáadás";
         hozzáadás.Size = new System.Drawing.Size( 96, 32 );
         hozzáadás.Location = new System.Drawing.Point( ClientRectangle.Width - 96, ClientRectangle.Height - 32 - 16 );
         hozzáadás.Click += hozzáadás_Click;

         Button beírás = new Button();
         beírás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
         beírás.Text = "Beírás";
         beírás.Size = new System.Drawing.Size( 96, 32 );
         beírás.Location = new System.Drawing.Point( ClientRectangle.Width - 96 - 8 - hozzáadás.Width, ClientRectangle.Height - 32 - 16 );
         beírás.Click += beírás_Click;

         Button törlés = new Button();
         törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
         törlés.Text = "Törlés";
         törlés.Size = new System.Drawing.Size( 96, 32 );
         törlés.Location = new System.Drawing.Point( table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16 );
         törlés.Click += törlés_Click;

         Label label_keresés = new Label();
         label_keresés.Text = "Név keresés:";
         label_keresés.Location = new System.Drawing.Point( table.Location.X + table.Size.Width + 16, 16 + 0 * 32 );

         keresés = new TextBox( );
         keresés.Location = new System.Drawing.Point( table.Location.X + table.Size.Width + label_keresés.Width + 16, 16 + 0 * 32 );
         keresés.TextChanged += keresés_TextChanged;
         keresés.MaxLength = 30;
         keresés.Width = 150;

         Controls.Add( label_keresés );
         Controls.Add( keresés );

         Controls.Add( table );

         Controls.Add( hozzáadás );
         Controls.Add( beírás );
         Controls.Add( törlés );
      }

      private DataTable CreateSource( )
      {
         data = new DataTable( );

         data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Nem", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Születési idő", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Engedély", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "#", System.Type.GetType( "System.Int32" ) ) );

         List<Induló> indulók = Program.database.Indulók();

         foreach ( Induló current in indulók )
         {
            DataRow row = data.NewRow();
            row[ 0 ] = current.Nev;
            row[ 1 ] = current.Nem;
            row[ 2 ] = current.SzuletesiDatum;
            row[ 3 ] = current.Engedely;
            row[ 4 ] = current.Egyesulet;
            row[ 5 ] = current.Eredmenyek;

            data.Rows.Add( row );
         }
         return data;
      }

      #region Accessors
      private delegate void Induló_Hozzáadás_Callback( Induló _induló );
      public void Induló_Hozzáadás( Induló _induló )
      {
         if ( InvokeRequired )
         {
            Induló_Hozzáadás_Callback callback = new Induló_Hozzáadás_Callback(Induló_Hozzáadás);
            Invoke( callback, new object[ ] { _induló } );
         }
         else
         {
            if ( !Program.database.ÚjInduló( _induló ) ) { MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            DataRow row = data.NewRow();
            row[ 0 ] = _induló.Nev;
            row[ 1 ] = _induló.Nem;
            row[ 2 ] = _induló.SzuletesiDatum;
            row[ 3 ] = _induló.Engedely;
            row[ 4 ] = _induló.Egyesulet;
            row[ 5 ] = 0; //

            data.Rows.Add( row );

            if ( induló_hozzáadva != null ) induló_hozzáadva( _induló );
         }
      }

      private delegate void Induló_Módosítás_Callback( Induló _eredeti, Induló _uj );
      public void Induló_Módosítás( Induló _eredeti, Induló _uj )
      {
         if ( InvokeRequired )
         {
            Induló_Módosítás_Callback callback = new Induló_Módosítás_Callback(Induló_Módosítás);
            Invoke( callback, new object[ ] { _eredeti, _uj } );
         }
         else
         {
            if ( !Program.database.IndulóMódosítás( _eredeti.Nev, _uj ) ) { MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            foreach ( DataRow current in data.Rows )
            {
               if ( _eredeti.Nev == current[ 0 ].ToString( ) )
               {
                  current[ 0 ] = _uj.Nev;
                  current[ 1 ] = _uj.Nem;
                  current[ 2 ] = _uj.SzuletesiDatum;
                  current[ 3 ] = _uj.Engedely;
                  current[ 4 ] = _uj.Egyesulet;

                  // Jól legyen broadcastolva a módosítás!
                  _uj.Eredmenyek = ( int )current[ 5 ];
                  break;
               }
            }

            if ( induló_módosítva != null ) induló_módosítva( _eredeti, _uj );

            if ( _eredeti.Nev != _uj.Nev && 0 < _uj.Eredmenyek )
            {
               if ( !Program.database.Induló_EredményekÁtnevezése( _eredeti.Nev, _uj.Nev ) ) { MessageBox.Show( "Adatbázis hiba!\nLEHETETLEN!!!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( induló_átnevezve != null ) induló_átnevezve( _eredeti.Nev, _uj.Nev );
            }

         }
      }

      private delegate void Induló_Törlés_Callback( Induló _indulo );
      public void Induló_Törlés( Induló _indulo )
      {
         if ( InvokeRequired )
         {
            Induló_Törlés_Callback callback = new Induló_Törlés_Callback(Induló_Törlés);
            Invoke( callback, new object[ ] { _indulo } );
         }
         else
         {
            if ( !Program.database.IndulóTörlés( _indulo.Nev ) ) { MessageBox.Show( "Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            foreach ( DataRow current in data.Rows )
            {
               if ( _indulo.Nev == current[ 0 ].ToString( ) )
               {
                  data.Rows.Remove( current );
                  break;
               }
            }

            if ( induló_törölve != null ) induló_törölve( _indulo );
         }
      }
      #endregion

      #region EventHandlers
      public void eredmények_beírás( string _azonosító, Database.BeírásEredmény _beírás )
      {
         if ( _beírás.flag == Database.BeírásEredmény.Flag.HOZZÁADOTT )
         {
            foreach ( DataRow current in data.Rows )
            {
               if ( _beírás.eredmény.Value.Nev == ( string )current[ 0 ] )
               {
                  current[ 5 ] = ( ( int )current[ 5 ] ) + 1;
                  break;
               }
            }
         }
      }

      public void eredmények_törlés( string _azonosító, Eredmény _eredmény )
      {
         foreach ( DataRow current in data.Rows )
         {
            if ( _eredmény.Nev == ( string )current[ 0 ] )
            {
               current[ 5 ] = ( ( int )current[ 5 ] ) - 1;
               break;
            }
         }

      }
      private void beírás_Click( object sender, EventArgs e )
      {
         List<Verseny> versenyek = Program.database.Versenyek();
         if ( versenyek.Count == 0 ) { MessageBox.Show( "Nincsen még egy verseny sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
         if ( íjtípusok.Count == 0 ) { MessageBox.Show( "Nincsen még egy íjtípus sem felvéve, először rögzítsen egyet!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( table.SelectedRows.Count != 1 ) { MessageBox.Show( "Nem megfelelő a kiválasztott indulók száma!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         eredmény_form = new Form_Induló_Teszt( table.SelectedRows[ 0 ].Cells[ 0 ].Value.ToString( ),lastindex );
         eredmény_form.ShowDialog( );
      }

      private void table_DataBindingComplete( object _sender, EventArgs _event )
      {
         table.DataBindingComplete -= table_DataBindingComplete;

         table.Columns[ 0 ].Width = 250;
         table.Columns[ 1 ].Width = 50;
         table.Columns[ 2 ].Width = 79;
         table.Columns[ 3 ].Width = 54;
         table.Columns[ 4 ].Width = 200;
         table.Columns[ 5 ].Width = 50;

         foreach ( DataGridViewColumn column in table.Columns ) column.SortMode = DataGridViewColumnSortMode.NotSortable;

         //rendezés
         table.Sort( table.Columns[ 0 ], System.ComponentModel.ListSortDirection.Ascending );
      }

      private void hozzáadás_Click( object _sender, EventArgs _event )
      {
         Form_Induló induló = new Form_Induló();
         induló.ShowDialog( );
      }

      private void módosítás_Click( object _sender, EventArgs _event )
      {
         if ( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[ 0 ].Index == data.Rows.Count ) ) { MessageBox.Show( "Nincs kiválasztva induló!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; };

         foreach ( DataGridViewRow current in table.Rows )
         {
            if ( table.SelectedRows[ 0 ] == current )
            {
               Form_Induló induló = new Form_Induló(new Induló(current.Cells[0].Value.ToString(),
                                                                    current.Cells[1].Value.ToString(),
                                                                    current.Cells[2].Value.ToString(),
                                                                    current.Cells[3].Value.ToString(),
                                                                    current.Cells[4].Value.ToString(),
                                                                    Convert.ToInt32(current.Cells[5].Value) ) ) ;
               induló.ShowDialog( );
               return;
            }

         }
      }

      private void törlés_Click( object _sender, EventArgs _event )
      {
         if ( !( table.SelectedRows.Count != 0 && table.SelectedRows[ 0 ].Index < data.Rows.Count && table.SelectedRows[ 0 ].Selected ) ) return;
         if ( Convert.ToInt32( table.SelectedRows[ 0 ].Cells[ 5 ].Value ) != 0 ) { MessageBox.Show( "Ez az induló nem törölhető, mivel szerepel versenyen!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         if ( MessageBox.Show( "Biztosan törli ezt az indulót?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ) return;

         foreach ( DataGridViewRow current in table.Rows )
         {
            if ( table.SelectedRows[ 0 ] == current )
            {
               Induló? _indulo = Program.database.Induló(table.SelectedRows[0].Cells[0].Value.ToString());
               Induló_Törlés( _indulo.Value );

            }
         }
      }

      private void keresés_TextChanged( object _sender, EventArgs _event )
      {
         foreach ( DataGridViewRow row in table.Rows )
         {
            int talált = 0;
            for ( int i = 0 ; i < keresés.Text.Length ; i++ )
            {
               if ( row.Cells[ 0 ].Value.ToString( ).Length < keresés.Text.Length )
               {
                  break;
               }
               if ( row.Cells[ 0 ].Value.ToString( )[ i ] == keresés.Text[ i ] || row.Cells[ 0 ].Value.ToString( )[ i ] == Char.ToUpper( keresés.Text[ i ] ) )
               {
                  talált++;
               }
            }
            if ( talált == keresés.Text.Length )
            {
               table.Rows[ row.Index ].Selected = true;
               table.FirstDisplayedScrollingRowIndex = row.Index;
               return;
            }
         }
      }

      #endregion
   }
}
