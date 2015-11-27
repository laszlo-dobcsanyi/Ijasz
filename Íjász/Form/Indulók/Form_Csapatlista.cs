using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{

   public sealed class Form_Csapatlista : Form
   {
      private SplitContainer splitContainer1;
      private DataGridView dataGridView1;
      private DataGridView dataGridView2;
      private DataTable data;
      string azonosító;

      public Form_Csapatlista( string _azonosító )
      {
         azonosító = _azonosító;
         InitializeForm( );
         InitializeContent( );
      }

      private void InitializeForm( )
      {
         Text = " Csapatok (" + azonosító + ")";
         ClientSize = new System.Drawing.Size( 788 - 64 + 8, 700 );
         MinimumSize = ClientSize;
         StartPosition = FormStartPosition.CenterScreen;
         FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      }

      private void InitializeContent( )
      {

         splitContainer1 = new SplitContainer( );
         dataGridView1 = new DataGridView( );
         dataGridView2 = new DataGridView( );
         splitContainer1.Dock = DockStyle.Fill;

         splitContainer1.Location = new System.Drawing.Point( 0, 0 );
         splitContainer1.Name = "splitContainer1";
         splitContainer1.Panel1.Controls.Add( dataGridView1 );
         splitContainer1.Panel2.Controls.Add( dataGridView2 );
         splitContainer1.Size = ClientSize;

         dataGridView1.Dock = DockStyle.Fill;
         splitContainer1.SplitterDistance = dataGridView1.Width - 24;
         splitContainer1.IsSplitterFixed = true;
         dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
         //dataGridView2.Size = new System.Drawing.Size(splitContainer1.Width / 2 - 10, splitContainer1.Height - 5);
         dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

         dataGridView1.RowHeadersVisible = false;
         dataGridView1.AllowUserToResizeColumns = false;
         dataGridView1.AllowUserToResizeRows = false;
         dataGridView1.AllowUserToAddRows = false;
         dataGridView1.MultiSelect = false;
         dataGridView1.ReadOnly = true;

         dataGridView2.RowHeadersVisible = false;
         dataGridView2.AllowUserToResizeColumns = false;
         dataGridView2.AllowUserToResizeRows = false;
         dataGridView2.AllowUserToAddRows = false;
         dataGridView2.MultiSelect = false;
         dataGridView2.ReadOnly = true;

         Controls.Add( splitContainer1 );
         dataGridView1.DataSource = CreateSource( );
         dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

      }

      void dataGridView1_SelectionChanged( object sender, EventArgs e )
      {
         if ( dataGridView1.Rows.Count == 0 || dataGridView1.SelectedRows.Count == 0 )
         {
            return;
         }
         if ( dataGridView1.SelectedRows[ 0 ].Cells[ 1 ].Value.ToString( ) == "0" )
         {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = CreateEmptySource( );
            return;
         }

         List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
         foreach ( Eredmény item in eredmények )
         {
            if ( item.Csapat == ( int )dataGridView1.SelectedRows[ 0 ].Cells[ 0 ].Value )
            {
               dataGridView2.DataSource = CreateSource2( );
            }
         }
      }

      private DataTable CreateSource( )
      {
         const int CsapatokSzama = 45;

         data = new DataTable( );
         data.Columns.Add( new DataColumn( "Csapat", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Indulók száma", System.Type.GetType( "System.Int32" ) ) );

         for ( int i = 0 ; i < CsapatokSzama ; i++ )
         {
            DataRow row = data.NewRow();
            row[ 0 ] = i + 1;
            row[ 1 ] = 0;
            data.Rows.Add( row );
         }

         List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
         foreach ( Eredmény item in eredmények )
         {
            int a = Convert.ToInt32(data.Rows[Convert.ToInt32(item.Csapat) - 1][1]);
            data.Rows[ Convert.ToInt32( item.Csapat ) - 1 ][ 1 ] = a + 1;
         }
         return data;
      }

      private DataTable CreateSource2( )
      {
         data = new DataTable( );
         data.Columns.Add( new DataColumn( "Szám", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Íjtípus", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Kor", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );
         if ( dataGridView2.Columns.Count != 0 )
         {
            dataGridView2.Columns[ 0 ].Width = 40;
            dataGridView2.Columns[ 1 ].Width = 160;
            dataGridView2.Columns[ 2 ].Width = 100;
            dataGridView2.Columns[ 3 ].Width = 40;
            dataGridView2.Columns[ 4 ].Width = 160;
         }

         List<Eredmény> eredmények = Program.database.Eredmények(azonosító);
         List<Induló> indulók = Program.database.Indulók();
         int seged = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
         foreach ( Eredmény item in eredmények )
         {
            if ( item.Csapat == seged )
            {
               DataRow row = data.NewRow();
               foreach ( Induló inner in indulók )
               {
                  if ( inner.Nev == item.Nev )
                  {
                     row[ 3 ] = ( new DateTime( 1, 1, 1 ) + ( DateTime.Now - DateTime.Parse( inner.SzuletesiDatum ) ) ).Year - 1;
                     row[ 4 ] = inner.Egyesulet;

                  }
               }
               row[ 0 ] = item.Sorszam;
               row[ 1 ] = item.Nev + ( item.Megjelent ? "(megjelent)" : "(nem jelent meg)" );
               row[ 2 ] = item.Ijtipus;
               data.Rows.Add( row );
            }
         }

         return data;
      }

      private DataTable CreateEmptySource( )
      {
         data = new DataTable( );
         data.Columns.Add( new DataColumn( "Szám", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Íjtípus", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Kor", System.Type.GetType( "System.Int32" ) ) );
         data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );
         return data;
      }
   }
}
