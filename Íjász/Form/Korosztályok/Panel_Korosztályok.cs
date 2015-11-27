using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Íjász
{
   public struct Korosztály
   {
      public string Verseny;
      public string Azonosito;
      public string Megnevezes;
      public int AlsoHatar;
      public int FelsoHatar;
      public bool Nokre;
      public bool Ferfiakra;
      public int InduloNok;
      public int InduloFerfiak;
      public bool Egyben;

      public Korosztály( string _Verseny,
                         string _Azonosito,
                         string _Megnevezes,
                         int _AlsoHatar,
                         int _FelsoHatar,
                         bool _Nokre,
                         bool _Ferfiakra,
                         int _InduloNok,
                         int _InduloFerfiak,
                         bool _Egyben )
      {
         Verseny = _Verseny;
         Azonosito = _Azonosito;
         Megnevezes = _Megnevezes;
         AlsoHatar = _AlsoHatar;
         FelsoHatar = _FelsoHatar;
         Nokre = _Nokre;
         Ferfiakra = _Ferfiakra;
         InduloNok = _InduloNok;
         InduloFerfiak = _InduloFerfiak;
         Egyben = _Egyben;
      }
   }

   public delegate void Korosztály_Hozzáadva( Korosztály _korosztály );
   public delegate void Korosztály_Módosítva( string _azonosító, Korosztály _korosztály );
   public delegate void Korosztály_Törölve( string _azonosító );

   public sealed class Panel_Korosztályok : Control
   {
      public Korosztály_Hozzáadva korosztály_hozzáadva;
      public Korosztály_Módosítva korosztály_módosítva;
      public Korosztály_Törölve korosztály_törölve;

      private DataTable data;
      private DataGridView table;

      private ComboBox box_vazon;

      public Panel_Korosztályok( )
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
         table.Width = 653;
         table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         table.MultiSelect = false;
         table.ReadOnly = true;
         table.DataBindingComplete += table_DataBindingComplete;
         table.CellDoubleClick += módosítás_Click;

         ///

         Label label_vazon = new Label();
         label_vazon.Text = "Verseny azonosítója:";
         label_vazon.AutoSize = true;
         label_vazon.Location = new Point( table.Location.X + table.Size.Width + 16, 16 + 0 * 32 );

         box_vazon = new ComboBox( );
         box_vazon.Location = new Point( table.Location.X + table.Size.Width + 16 + 4 * 32, 16 + 0 * 32 );
         box_vazon.DropDownStyle = ComboBoxStyle.DropDownList;
         box_vazon.SelectedIndexChanged += box_vazon_SelectedIndexChanged;

         ///

         Button hozzáadás = new Button( );
         hozzáadás.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
         hozzáadás.Text = "Hozzáadás";
         hozzáadás.Size = new Size( 96, 32 );
         hozzáadás.Location = new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 );
         hozzáadás.Click += hozzáadás_Click;

         Button törlés = new Button();
         törlés.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
         törlés.Text = "Törlés";
         törlés.Size = new Size( 96, 32 );
         törlés.Location = new Point( table.Location.X + table.Size.Width + 16, ClientRectangle.Height - 32 - 16 );
         törlés.Click += törlés_Click;

         Button számolás = new Button();
         számolás.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
         számolás.Text = "Számolás";
         számolás.Size = new Size( 96, 32 );
         számolás.Location = new Point( törlés.Location.X + törlés.Size.Width + 16, törlés.Location.Y );
         számolás.Click += számolás_Click;

         Button btnKorosztalyEredmenyek = new iButton( "Tagok",
                                                        new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 64 ),
                                                        new Size( 96, 32 ),
                                                        btnKorosztalyTagok,
                                                        this );
         ///

         List<Verseny> versenyek = Program.database.Versenyek();

         foreach ( Verseny current in versenyek )
            box_vazon.Items.Add( current.Azonosito );

         if ( 0 < box_vazon.Items.Count ) box_vazon.SelectedIndex = 0;

         ///

         Controls.Add( table );

         Controls.Add( label_vazon );

         Controls.Add( box_vazon );

         Controls.Add( törlés );
         Controls.Add( számolás );
         Controls.Add( hozzáadás );
      }

      private DataTable CreateSource( )
      {
         data = new DataTable( );
         data.Columns.Add( new DataColumn( "Verseny azonosító", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Azonosító", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Megnevezés", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Alsó életkorhatár", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Felső életkorhatár", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Nők", System.Type.GetType( "System.Boolean" ) ) );
         data.Columns.Add( new DataColumn( "Férfiak", System.Type.GetType( "System.Boolean" ) ) );
         data.Columns.Add( new DataColumn( "# Nők", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "# Férfiak", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Egyben", System.Type.GetType( "System.Boolean" ) ) );

         return data;
      }

      #region Accessors
      private delegate void Korosztály_Hozzáadás_Callback( Korosztály _korosztály );
      public void Korosztály_Hozzáadás( Korosztály _korosztály )
      {
         if ( InvokeRequired )
         {
            Korosztály_Hozzáadás_Callback callback = new Korosztály_Hozzáadás_Callback(Korosztály_Hozzáadás);
            Invoke( callback, new object[ ] { _korosztály } );
         }
         else
         {
            if ( !Program.database.ÚjKorosztály( _korosztály ) ) { MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            if ( _korosztály.Verseny == box_vazon.SelectedItem.ToString( ) )
            {
               DataRow row = data.NewRow();
               row[ 0 ] = _korosztály.Verseny;
               row[ 1 ] = _korosztály.Azonosito;
               row[ 2 ] = _korosztály.Megnevezes;
               row[ 3 ] = _korosztály.AlsoHatar;
               row[ 4 ] = _korosztály.FelsoHatar;
               row[ 5 ] = _korosztály.Nokre;
               row[ 6 ] = _korosztály.Ferfiakra;
               row[ 7 ] = _korosztály.InduloNok;
               row[ 8 ] = _korosztály.InduloFerfiak;
               row[ 9 ] = _korosztály.Egyben;

               data.Rows.Add( row );
            }

            if ( korosztály_hozzáadva != null ) korosztály_hozzáadva( _korosztály );
         }
         //rendezés
         table.Sort( table.Columns[ 3 ], System.ComponentModel.ListSortDirection.Ascending );
      }

      private delegate void Korosztály_Módosítás_Callback( string _azonosító, Korosztály _korosztály );
      public void Korosztály_Módosítás( string _azonosító, Korosztály _korosztály )
      {
         if ( InvokeRequired )
         {
            Korosztály_Módosítás_Callback callback = new Korosztály_Módosítás_Callback(Korosztály_Módosítás);
            Invoke( callback, new object[ ] { _azonosító, _korosztály } );
         }
         else
         {
            if ( !Program.database.KorosztályMódosítás( _azonosító, _korosztály ) ) { MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            if ( _korosztály.Verseny == box_vazon.SelectedItem.ToString( ) )
            {
               foreach ( DataRow current in data.Rows )
               {
                  if ( _azonosító == current[ 1 ].ToString( ) )
                  {
                     current[ 0 ] = _korosztály.Verseny;
                     current[ 1 ] = _korosztály.Azonosito;
                     current[ 2 ] = _korosztály.Megnevezes;
                     current[ 3 ] = _korosztály.AlsoHatar;
                     current[ 4 ] = _korosztály.FelsoHatar;
                     current[ 5 ] = _korosztály.Nokre;
                     current[ 6 ] = _korosztály.Ferfiakra;
                     current[ 7 ] = _korosztály.InduloNok;
                     current[ 8 ] = _korosztály.InduloFerfiak;
                     current[ 9 ] = _korosztály.Egyben;

                     break;
                  }
               }
            }

            if ( korosztály_módosítva != null ) korosztály_módosítva( _azonosító, _korosztály );
         }
         //rendezés
         table.Sort( table.Columns[ 3 ], System.ComponentModel.ListSortDirection.Ascending );

      }

      private delegate void Korosztály_Törlés_Callback( string _verseny, string _azonosító );
      public void Korosztály_Törlés( string _verseny, string _azonosító )
      {
         if ( InvokeRequired )
         {
            Korosztály_Törlés_Callback callback = new Korosztály_Törlés_Callback(Korosztály_Törlés);
            Invoke( callback, new object[ ] { _verseny, _azonosító } );
         }
         else
         {
            if ( !Program.database.KorosztályTörlés( _verseny, _azonosító ) ) { MessageBox.Show( "Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            foreach ( DataRow current in data.Rows )
            {
               if ( _azonosító == current[ 1 ].ToString( ) )
               {
                  data.Rows.Remove( current );
                  break;
               }
            }

            if ( korosztály_törölve != null ) korosztály_törölve( _azonosító );
         }
      }

      //Csak belső használatra, nincs invokeolva!/
      public void Verseny_Számolás( string _verseny )
      {
         //if ( !Program.database.KorosztalyModositas( _verseny ) ) { MessageBox.Show( "Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
         Program.database.LINQ_KorosztalyModositas( _verseny );

         if ( _verseny == box_vazon.SelectedItem.ToString( ) )
         {
            List<Korosztály> korosztályok = Program.database.Korosztályok(box_vazon.Text);

            data.Rows.Clear( );

            foreach ( Korosztály current in korosztályok )
            {
               DataRow row = data.NewRow();
               row[ 0 ] = current.Verseny;
               row[ 1 ] = current.Azonosito;
               row[ 2 ] = current.Megnevezes;
               row[ 3 ] = current.AlsoHatar;
               row[ 4 ] = current.FelsoHatar;
               row[ 5 ] = current.Nokre;
               row[ 6 ] = current.Ferfiakra;
               row[ 7 ] = current.InduloNok;
               row[ 8 ] = current.InduloFerfiak;
               row[ 9 ] = current.Egyben;

               data.Rows.Add( row );
            }
         }
      }
      #endregion

      #region EventHandlers

      private void
      btnKorosztalyTagok( object _sender, EventArgs _event )
      {
         if ( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[ 0 ].Index == data.Rows.Count ) ) return;

         foreach ( DataGridViewRow current in table.Rows )
         {
            if ( table.SelectedRows[ 0 ] == current )
            {
               FormKorosztalyEredmeny korosztály = new FormKorosztalyEredmeny( box_vazon.Text, current.Cells[1].Value.ToString( ));
               korosztály.ShowDialog( );
               break;
            }
         }

      }

      public void verseny_hozzáadás( Verseny _verseny )
      {
         box_vazon.Items.Add( _verseny.Azonosito );
      }

      public void verseny_módosítás( string _azonosító, Verseny _verseny )
      {
         if ( _azonosító != _verseny.Azonosito )
         {
            for ( int current = 0 ; current < box_vazon.Items.Count ; ++current )
            {
               if ( _azonosító == box_vazon.Items[ current ].ToString( ) )
               {
                  box_vazon.Items[ current ] = _verseny.Azonosito;
                  break;
               }
            }
         }
      }

      public void verseny_törlés( string _azonosító )
      {
         if ( box_vazon.SelectedItem != null && _azonosító == box_vazon.SelectedItem.ToString( ) )
         {
            data.Rows.Clear( );
         }

         box_vazon.Items.Remove( _azonosító );
      }

      ///

      void table_DataBindingComplete( object sender, DataGridViewBindingCompleteEventArgs e )
      {
         table.DataBindingComplete -= table_DataBindingComplete;

         table.Columns[ 0 ].Visible = false;
         table.Columns[ 5 ].Width = 40;
         table.Columns[ 6 ].Width = 40;
         table.Columns[ 7 ].Width = 60;
         table.Columns[ 8 ].Width = 60;
         table.Columns[ 9 ].Width = 50;

         foreach ( DataGridViewColumn column in table.Columns ) column.SortMode = DataGridViewColumnSortMode.NotSortable;
         //rendezés
         table.Sort( table.Columns[ 3 ], System.ComponentModel.ListSortDirection.Ascending );
      }

      private void box_vazon_SelectedIndexChanged( object _sender, EventArgs _event )
      {
         List<Korosztály> korosztályok = Program.database.Korosztályok(box_vazon.Text);

         data.Rows.Clear( );

         foreach ( Korosztály current in korosztályok )
         {
            DataRow row = data.NewRow();
            row[ 0 ] = current.Verseny;
            row[ 1 ] = current.Azonosito;
            row[ 2 ] = current.Megnevezes;
            row[ 3 ] = current.AlsoHatar;
            row[ 4 ] = current.FelsoHatar;
            row[ 5 ] = current.Nokre;
            row[ 6 ] = current.Ferfiakra;
            row[ 7 ] = current.InduloNok;
            row[ 8 ] = current.InduloFerfiak;
            row[ 9 ] = current.Egyben;

            data.Rows.Add( row );
         }
      }

      private void hozzáadás_Click( object _sender, EventArgs _event )
      {
         if ( box_vazon.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         Form_Korosztály korosztály = new Form_Korosztály(box_vazon.Text);
         korosztály.ShowDialog( );
      }

      private void módosítás_Click( object _sender, EventArgs _event )
      {
         if ( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[ 0 ].Index == data.Rows.Count ) ) return;

         foreach ( DataGridViewRow current in table.Rows )
         {
            if ( table.SelectedRows[ 0 ] == current )
            {
               Form_Korosztály korosztály =  new Form_Korosztály(box_vazon.Text, new Korosztály(current.Cells[0].Value.ToString(),
                        current.Cells[1].Value.ToString(),
                        current.Cells[2].Value.ToString(),
                        Convert.ToInt32(current.Cells[3].Value),
                        Convert.ToInt32(current.Cells[4].Value),
                        (bool)current.Cells[5].Value,
                        (bool)current.Cells[6].Value,
                        Convert.ToInt32(current.Cells[7].Value),
                        Convert.ToInt32(current.Cells[8].Value),
                        (bool)current.Cells[9].Value));

               korosztály.ShowDialog( );
               break;
            }
         }
      }

      private void számolás_Click( object _sender, EventArgs _event )
      {
         if ( box_vazon.SelectedItem == null ) { MessageBox.Show( "Nincs kiválasztva verseny!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

         Verseny_Számolás( box_vazon.SelectedItem.ToString( ) );
      }

      //korosztályt ne lehessen törölni ha van hozzárendelve fix induló
      private void törlés_Click( object _sender, EventArgs _event )
      {
         List<Eredmény> eredmenyek = Program.database.Eredmények( box_vazon.Text );

         List<string> nevek = new List<string>( );

         if ( box_vazon.SelectedItem == null ) return;
         if ( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[ 0 ].Index == data.Rows.Count ) ) return;
         //if ((int)data.Rows[table.SelectedRows[0].Index][4] != 0) { MessageBox.Show("Ez a korosztály nem törölhető, mivel van hozzárendelve eredmény!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

         foreach ( DataGridViewRow current in table.Rows )
         {
            if ( table.SelectedRows[ 0 ] == current )
            {
               foreach ( Eredmény eredmeny in eredmenyek )
               {
                  if ( eredmeny.KorosztalyModositott == true &&
                      eredmeny.KorosztalyAzonosito == current.Cells[ 1 ].Value.ToString( ) )
                  {
                     nevek.Add( "\n" + eredmeny.Nev );

                  }
               }
               string errorstring = "Ez a korosztály nem törölhető, mivel van hozzárendelve módosított eredmény!";
               if ( nevek.Count != 0 )
               {
                  foreach ( string nev in nevek )
                  {
                     errorstring += nev;
                  }
                  MessageBox.Show( errorstring,
                                       "Hiba",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error );
                  return;
               }
               if ( MessageBox.Show( "Biztosan törli ezt a korosztályt?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ) return;
               Korosztály_Törlés( current.Cells[ 0 ].Value.ToString( ), current.Cells[ 1 ].Value.ToString( ) );

            }
         }
      }
      #endregion


   }
}