using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class FormKorosztalyEredmeny : Form
   {
      private DataTable data;
      private DataGridView table;

      public FormKorosztalyEredmeny( string _VEAZON, string _KOAZON )
      {
         InitializeForm( _VEAZON, _KOAZON );
         InitializeContent( _VEAZON, _KOAZON );
      }

      private void InitializeForm( string _VEAZON, string _KOAZON )
      {
         Text = "Indulók (" + _VEAZON + ")";
         ClientSize = new Size( 435 + 8, 500 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( string _VEAZON, string _KOAZON )
      {

         table = new DataGridView( );
         table.Dock = DockStyle.Left;
         table.RowHeadersVisible = false;
         table.AllowUserToResizeRows = false;
         table.AllowUserToResizeColumns = false;
         table.AllowUserToAddRows = false;
         table.Width = 520 + 3;
         table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         table.MultiSelect = false;
         table.ReadOnly = true;
         table.DataSource = CreateSource( _VEAZON, _KOAZON );
         table.DataBindingComplete += table_DataBindingComplete;

         Controls.Add( table );

      }

      private DataTable CreateSource( string _VEAZON, string _KOAZON )
      {

         data = new DataTable( );
         data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Nem", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Korosztály", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Életkor", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Módosított", System.Type.GetType( "System.String" ) ) );

         Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
         List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );

         for ( int i = 0 ; i < eredmenyek.Count ; i++ )
         {
            if ( eredmenyek[ i ].Megjelent == true && eredmenyek[ i ].KorosztalyAzonosito == _KOAZON )
            {
               Induló indulo = Program.database.Induló( eredmenyek[i].Nev ).Value;
               DataRow row = data.NewRow( );
               row[ 0 ] = eredmenyek[ i ].Nev;
               row[ 1 ] = indulo.Nem;
               row[ 2 ] = eredmenyek[ i ].KorosztalyAzonosito;
               row[ 3 ] = Program.database.InduloKora( verseny.Azonosito, indulo.Nev );
               row[ 4 ] = ( eredmenyek[ i ].KorosztalyModositott == true ) ? "I" : "N";
               data.Rows.Add( row );
            }
         }
         return data;
      }

      private void table_DataBindingComplete( object _sender, EventArgs _event )
      {
         table.DataBindingComplete -= table_DataBindingComplete;

         table.Columns[ 0 ].Width = 200;
         table.Columns[ 1 ].Width = 80;
         table.Columns[ 2 ].Width = 80;
         table.Columns[ 3 ].Width = 80;
         table.Columns[ 4 ].Width = 80;

         foreach ( DataGridViewColumn column in table.Columns ) column.SortMode = DataGridViewColumnSortMode.NotSortable;

         //rendezés
         table.Sort( table.Columns[ 3 ], System.ComponentModel.ListSortDirection.Ascending );

      }

   }
}
