using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
   public struct Oklevel
   {
      public string Azonosito { get; set; }
      public string Tipus { get; set; }
      public int Nev { get; set; }
      public int Helyezes { get; set; }
      public int Kategoria { get; set; }
      public int Helyszin { get; set; }
      public int Datum { get; set; }
      public int Egyesulet { get; set; }

      public Oklevel( string _Azonosito, string _Tipus, int _Nev, int _Helyezes = -100, int _Kategoria = -100, int _Helyszin = -100, int _Datum = -100, int _Egyesulet = -100 )
      {
         Azonosito = _Azonosito;
         Tipus = _Tipus;
         Nev = _Nev;
         Helyezes = _Helyezes;
         Kategoria = _Kategoria;
         Helyszin = _Helyszin;
         Datum = _Datum;
         Egyesulet = _Egyesulet;
      }
   }


   public delegate void Sablon_Hozzáadva( Oklevel _oklevel );
   public delegate void Sablon_Törölve( string _azonosito);

   public partial class Panel_Oklevelek : Control
   {
      public Sablon_Hozzáadva sablon_hozzáadva;
      public Sablon_Törölve sablon_törölve;


      CheckBox chkVerseny;
      CheckBox chkVersenysorozat;
      ComboBox cboVerseny;
      ComboBox cboVersenysorozat;

      private DataTable data;
      private DataGridView table;

      public Panel_Oklevelek( )
      {
         InitializeContent( );
      }

      public void
      InitializeContent( )
      {

         table = new DataGridView( );
         table.Dock = DockStyle.Left;
         table.RowHeadersVisible = false;
         table.AllowUserToResizeRows = false;
         table.AllowUserToResizeColumns = false;
         table.AllowUserToAddRows = false;
         table.Width = 563;
         table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         table.MultiSelect = false;
         table.ReadOnly = true;
         table.DataSource = CreateSource( );
         table.DataBindingComplete += table_DataBindingComplete;
         //table.CellDoubleClick += módosítás_Click;
         Controls.Add( table );

         int cWidth = ClientRectangle.Width;
         int cHeight = ClientRectangle.Height;

         Label txtOklevelTipus = new Label
         {
            Text = "Oklevél típusa:",
            Location = new Point(cWidth - 96 - 20 * 16,cHeight - 32 - 41 * 16),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            AutoSize = true
         };

         Label txtVerseny= new Label
         {
            Text = "Verseny:",
            Location = new Point(cWidth - 96 - 20 * 16,cHeight - 32 - 37 * 16),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            AutoSize = true
         };
         Label txtVersenysorozat= new Label
         {
            Text = "Versenysorozat:",
            Location = new Point(cWidth - 96 - 20 * 16,cHeight - 32 - 35 * 16),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            AutoSize = true
         };

         chkVerseny = new CheckBox
         {
            Text = "Verseny",
            Location = new Point( cWidth - 96 - 15 * 16, cHeight - 32 - 41 * 16 - 8 ),
            AutoSize = true,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            FlatStyle = FlatStyle.Flat,
            CheckState = CheckState.Unchecked
         };
         chkVerseny.Click += ChkVerseny_Click;

         chkVersenysorozat = new CheckBox
         {
            Text = "Versenysorozat",
            Location = new Point( cWidth - 96 - 6 * 16, cHeight - 32 - 41 * 16 - 8 ),
            AutoSize = true,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            FlatStyle = FlatStyle.Flat,
            CheckState = CheckState.Unchecked
         };
         chkVersenysorozat.Click += ChkVerseny_Click;


         cboVerseny = new ComboBox
         {
            Location = new Point( cWidth - 96 - 13 * 16 + 8, cHeight - 32 - 37 * 16 ),
            Size = new Size( 128, 24 ),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
         };
         var Versenyek = Program.database.Versenyek().Select( verseny => verseny.Azonosito);
         foreach ( var item in Versenyek )
         {
            cboVerseny.Items.Add( item );
         }
         if ( cboVerseny.Items.Count != 0 ) { cboVerseny.SelectedIndex = 0; }


         cboVersenysorozat = new ComboBox
         {
            Location = new Point( cWidth - 96 - 13 * 16 + 8, cHeight - 32 - 35 * 16 ),
            Size = new Size( 128, 24 ),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
         };
         var Versenysorozatok = Program.database.Versenysorozatok().Select( vs => vs.azonosító);
         foreach ( var item in Versenysorozatok )
         {
            cboVersenysorozat.Items.Add( item );
         }
         if ( cboVersenysorozat.Items.Count != 0 ) { cboVersenysorozat.SelectedIndex = 0; }

         Button btnNyomtat = new Button
         {
            Text = "Nyomtat",
            Location = new Point(cWidth - 3 * 96 - 150 - 20,cHeight - 32 - 16),
            Size = new Size(96, 32),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
         };
         Button btnUjSablon = new Button
         {
            Text = "Új sablon",
            Location = new Point(cWidth - 2 * 96 - 150 - 10,cHeight - 32 - 16),
            Size = new Size(96, 32),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
         };
         btnUjSablon.Click += BtnUjSablon_Click;

         Button btnTorlesSablon = new Button
         {
            Text = "Sablon törlése",
            Location = new Point(cWidth - 96 - 150,cHeight - 32 - 16),
            Size = new Size(96, 32),
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
         };
         btnTorlesSablon.Click += BtnTorlesSablon_Click;

         Controls.Add( txtOklevelTipus );
         Controls.Add( txtVerseny );
         Controls.Add( txtVersenysorozat );
         Controls.Add( chkVerseny );
         Controls.Add( chkVersenysorozat );
         Controls.Add( cboVerseny );
         Controls.Add( cboVersenysorozat );
         Controls.Add( btnNyomtat );
         Controls.Add( btnUjSablon );
         Controls.Add( btnTorlesSablon );
      }
      

      private DataTable CreateSource( )
      {
         data = new DataTable( );

         data.Columns.Add( new DataColumn( "Azonosító", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Típus", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Név", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Helyezes", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Kategória", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Helyszín", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Dátum", System.Type.GetType( "System.String" ) ) );
         data.Columns.Add( new DataColumn( "Egyesület", System.Type.GetType( "System.String" ) ) );

         var Oklevelek = Program.database.Oklevelek();
         foreach ( var oklevel in Oklevelek )
         {
            DataRow row = data.NewRow();
            row[ 0 ] = oklevel.Azonosito;
            row[ 1 ] = oklevel.Tipus;
            row[ 2 ] = oklevel.Nev != -66 ? oklevel.Nev.ToString() : " ";
            row[ 3 ] = oklevel.Helyezes != -66 ? oklevel.Helyezes.ToString() : " ";
            row[ 4 ] = oklevel.Kategoria != -66 ? oklevel.Kategoria.ToString() : " ";
            row[ 5 ] = oklevel.Helyszin != -66 ? oklevel.Helyszin.ToString() : " ";
            row[ 6 ] = oklevel.Datum != -66 ? oklevel.Datum.ToString() : " ";
            row[ 7 ] = oklevel.Egyesulet != -66 ? oklevel.Egyesulet.ToString() : " ";
            data.Rows.Add( row );
         }

         return data;
      }

      private void table_DataBindingComplete( object sender, DataGridViewBindingCompleteEventArgs e )
      {
         table.DataBindingComplete -= table_DataBindingComplete;
         foreach ( DataGridViewColumn column in table.Columns ) column.SortMode = DataGridViewColumnSortMode.NotSortable;

         table.Columns[ 2 ].Width = 60;
         table.Columns[ 3 ].Width = 60;
         table.Columns[ 4 ].Width = 60;
         table.Columns[ 5 ].Width = 60;
         table.Columns[ 6 ].Width = 60;
         table.Columns[ 7 ].Width = 60;

      }

      #region Accessors

      private delegate void Sablon_Hozzáadás_Callback( Oklevel _oklevel);
      public void Sablon_Hozzáadás( Oklevel _oklevel)
      {
         if ( InvokeRequired )
         {
            Sablon_Hozzáadás_Callback callback = new Sablon_Hozzáadás_Callback(Sablon_Hozzáadás);
            Invoke( callback, new object[ ] { _oklevel } );
         }
         else
         {
            if ( !Program.database.UjOklevel( _oklevel ) ) { MessageBox.Show( "Adatbázis hiba!\nLehet, hogy van már ilyen azonosító?", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            DataRow row = data.NewRow();

            row[ 0 ] = _oklevel.Azonosito;
            row[ 1 ] = _oklevel.Tipus;
            row[ 2 ] = _oklevel.Nev != -66 ? _oklevel.Nev.ToString( ) : " ";
            row[ 3 ] = _oklevel.Helyezes != -66 ? _oklevel.Helyezes.ToString( ) : " ";
            row[ 4 ] = _oklevel.Kategoria != -66 ? _oklevel.Kategoria.ToString( ) : " ";
            row[ 5 ] = _oklevel.Helyszin != -66 ? _oklevel.Helyszin.ToString( ) : " ";
            row[ 6 ] = _oklevel.Datum != -66 ? _oklevel.Datum.ToString( ) : " ";
            row[ 7 ] = _oklevel.Egyesulet != -66 ? _oklevel.Egyesulet.ToString( ) : " ";

            data.Rows.Add( row );

            if ( sablon_hozzáadva != null ) sablon_hozzáadva( _oklevel );
         }
      }

      private delegate void Sablon_Törlés_Callback( string _azonosító );
      public void Sablon_Törlés( string _azonosító )
      {
         if ( InvokeRequired )
         {
            Sablon_Törlés_Callback callback = new Sablon_Törlés_Callback(Sablon_Törlés);
            Invoke( callback, new object[ ] { _azonosító } );
         }
         else
         {
            if ( !Program.database.OklevelTorles( _azonosító ) ) { MessageBox.Show( "Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            foreach ( DataRow current in data.Rows )
            {
               if ( _azonosító == current[ 0 ].ToString( ) )
               {
                  data.Rows.Remove( current );
                  break;
               }

            }

            if ( sablon_törölve != null ) sablon_törölve( _azonosító );
         }
      }



      #endregion

      #region EventHandlers
      private void ChkVerseny_Click( object sender, EventArgs _e )
      {
         CheckBox aktív = sender as CheckBox;
         if ( aktív == null ) { return; }

         if ( aktív == chkVerseny )
         {

            chkVerseny.Checked = true;
            chkVersenysorozat.Checked = false;
            cboVerseny.Enabled = true;
            cboVersenysorozat.Enabled = false;
         }

         if ( aktív == chkVersenysorozat )
         {
            chkVersenysorozat.Checked = true;
            chkVerseny.Checked = false;
            cboVerseny.Enabled = false;
            cboVersenysorozat.Enabled = true;
         }

      }
      #endregion

      private void BtnUjSablon_Click( object sender, EventArgs e )
      {
         Form_Oklevel OklevelForm = new Form_Oklevel();
         OklevelForm.Show( );
      }

      private void BtnTorlesSablon_Click( object sender, EventArgs e )
      {
         if ( ( table.SelectedRows.Count == 0 ) || ( table.SelectedRows[ 0 ].Index == data.Rows.Count ) ) return;
         if ( MessageBox.Show( "Biztosan törli ezt a sablont?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ) return;
         Sablon_Törlés(data.Rows[table.SelectedRows[0].Index][0].ToString());
      }

      public sealed class Form_Oklevel : Form
      {

         TextBox txt_Azonosito;
         ComboBox cbo_Tipus;
         TextBox txt_Nev;
         TextBox txt_Helyezes;
         TextBox txt_Kategoria;
         TextBox txt_Helyszin;
         TextBox txt_Datum;
         TextBox txt_Egyesulet;


         public Form_Oklevel( )
         {
            InitializeForm( );
            InitializeContent( );
            InitializeData( );
         }

         private void InitializeForm( )
         {
            Text = "Sablon szerksztő";
            ClientSize = new Size( 400 - 64, 320 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         }

         private void InitializeContent( )
         {
            int cWidth = ClientRectangle.Width;
            int cHeight = ClientRectangle.Height;

            #region Labels

            Label lblAzonosito = new Label
            {
               Text = "Azonosító:",
               Location = new Point( 16 , 1 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblTipus = new Label
            {
               Text = "Típus:",
               Location = new Point( 16 , 3 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };

            Label lblNev = new Label
            {
               Text = "Név:",
               Location = new Point( 16 , 5 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblHelyezes = new Label
            {
               Text = "Helyezés:",
               Location = new Point( 16 , 7 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblKategoria = new Label
            {
               Text = "Kategória:",
               Location = new Point( 16 , 9 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblHelyszin = new Label
            {
               Text = "Helyszín:",
               Location = new Point( 16 , 11 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblDatum = new Label
            {
               Text = "Dátum:",
               Location = new Point( 16 , 13 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            Label lblEgyesulet = new Label
            {
               Text = "Egyesület:",
               Location = new Point( 16 , 15 * 16 ),
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
               AutoSize = true
            };
            #endregion


            txt_Azonosito = new TextBox
            {
               Location = new Point( 5 * 16, 1 * 16 - 8 )
            };

            cbo_Tipus = new ComboBox
            {
               Location = new Point( 5 * 16, 3 * 16 - 8 ),
               Size = new Size( 128, 24 ),
               DropDownStyle = ComboBoxStyle.DropDownList,
               Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            cbo_Tipus.Items.Add( "Verseny" );
            cbo_Tipus.Items.Add( "Versenysorozat" );
            cbo_Tipus.SelectedIndexChanged += Cbo_Tipus_SelectedIndexChanged;

            txt_Nev = new TextBox
            {
               Location = new Point( 5 * 16, 5 * 16 - 4 )
            };
            txt_Helyezes = new TextBox
            {
               Location = new Point( 5 * 16, 7 * 16 - 4 )
            };
            txt_Kategoria = new TextBox
            {
               Location = new Point( 5 * 16, 9 * 16 - 4 )
            };
            txt_Helyszin = new TextBox
            {
               Location = new Point( 5 * 16, 11 * 16 - 4 )
            };
            txt_Datum = new TextBox
            {
               Location = new Point( 5 * 16, 13 * 16 - 4 )
            };
            txt_Egyesulet = new TextBox
            {
               Location = new Point( 5 * 16, 15 * 16 - 4 )
            };

            Button btnRrendben = new iButton( "Rendben",
                                                 new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                 new Size( 96, 32 ),
                                                 btnRendben_Click,
                                                 this );


            Controls.Add( lblAzonosito );
            Controls.Add( lblTipus );
            Controls.Add( lblNev );
            Controls.Add( lblHelyezes );
            Controls.Add( lblKategoria );
            Controls.Add( lblHelyszin );
            Controls.Add( lblDatum );
            Controls.Add( lblEgyesulet );

            Controls.Add( txt_Azonosito );
            Controls.Add( cbo_Tipus );
            Controls.Add( txt_Nev );
            Controls.Add( txt_Helyezes );
            Controls.Add( txt_Kategoria );
            Controls.Add( txt_Helyszin );
            Controls.Add( txt_Datum );
            Controls.Add( txt_Egyesulet );
         }

         private void Cbo_Tipus_SelectedIndexChanged( object sender, EventArgs e )
         {

            if ( cbo_Tipus.Text == null ) return;

            if ( cbo_Tipus.Text == "Verseny" )
            {
               txt_Helyezes.Enabled = true;
               txt_Nev.Enabled = true;
               txt_Egyesulet.Enabled = true;
               txt_Datum.Enabled = true;

               txt_Kategoria.Enabled = false;
               txt_Helyszin.Enabled = false;
            }
            else if ( cbo_Tipus.Text == "Versenysorozat" )
            {
               txt_Nev.Enabled = true;
               txt_Helyezes.Enabled = true;
               txt_Helyszin.Enabled = true;
               txt_Datum.Enabled = true;
               txt_Kategoria.Enabled = true;

               txt_Egyesulet.Enabled = false;

            }

         }

         private void btnRendben_Click( object sender, EventArgs e )
         {

            if ( txt_Azonosito.Text.Length == 0 ) { MessageBox.Show( "Hiba az azonosító mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( cbo_Tipus.Text.Length == 0 ) { MessageBox.Show( "Hiba a típus mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            if ( cbo_Tipus.Text == "Verseny" )
            {
               if ( txt_Helyezes.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Nev.Text.Length == 0 ) { MessageBox.Show( "Hiba az név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Egyesulet.Text.Length == 0 ) { MessageBox.Show( "Hiba az egyesület mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Datum.Text.Length == 0 ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

               Oklevel oklevél = new Oklevel
               {
                  Azonosito = txt_Azonosito.Text,
                  Tipus = cbo_Tipus.Text,

                  Helyezes = Convert.ToInt32( txt_Helyezes.Text ),
                  Nev = Convert.ToInt32( txt_Nev.Text ),
                  Egyesulet = Convert.ToInt32( txt_Egyesulet.Text ),
                  Datum = Convert.ToInt32( txt_Datum.Text ),

                  Kategoria = -66,
                  Helyszin = -66
               };
               Program.mainform.oklevelek_panel.Sablon_Hozzáadás( oklevél );

               this.Close( );
               return;
            }

            if ( cbo_Tipus.Text == "Versenysorozat" )
            {
               if ( txt_Nev.Text.Length == 0 ) { MessageBox.Show( "Hiba az név mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Helyezes.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyezés mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Helyszin.Text.Length == 0 ) { MessageBox.Show( "Hiba a helyszín mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Datum.Text.Length == 0 ) { MessageBox.Show( "Hiba a dátum mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
               if ( txt_Kategoria.Text.Length == 0 ) { MessageBox.Show( "Hiba a kategória mezőben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

               Oklevel oklevél = new Oklevel
               {
                  Azonosito = txt_Azonosito.Text,
                  Tipus = cbo_Tipus.Text,

                  Nev = Convert.ToInt32( txt_Nev.Text ),
                  Helyezes = Convert.ToInt32( txt_Helyezes.Text ),
                  Helyszin = Convert.ToInt32( txt_Helyszin.Text ),
                  Datum = Convert.ToInt32( txt_Datum.Text ),
                  Kategoria = Convert.ToInt32( txt_Kategoria.Text ),

                  Egyesulet = -66
               };
               Program.mainform.oklevelek_panel.Sablon_Hozzáadás( oklevél );
               this.Close( );
               return;
            }
         }

         private void InitializeData( )
         {

         }

      }

   }

}

