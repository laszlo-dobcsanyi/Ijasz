using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Íjász
{

   // INNEVE char(30) PRIMARY KEY,
   // INNEME char(1) NOT NULL, 
   // INSZUL char(20) NOT NULL, 
   // INVEEN char(30),
   // INERSZ int, 
   // EGAZON char(10));" +
   public struct Induló
   {
      private string m_Nev;
      private string m_Nem;
      private string m_SzuletesiDatum;
      private string m_Engedely;
      private string m_Egyesulet;
      private int    m_Eredmenyek;


      public string Nev
      {
         get { return m_Nev; }
         set
         {
            if ( ( value.Length > 30 ) || ( value.Length == 0 ) )
            {
               MessageBox.Show( "Nem megfelelő a név hossza ( " + value + " )", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return;
            }
            m_Nev = value;
         }
      }
      public string Nem
      {
         get { return m_Nem; }
         set
         {
            if ( value.ToString() != "F" && value.ToString()!= "N" )
            {
               MessageBox.Show( "Nem megfelelő a nem ( " + value + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }
            m_Nem = value;
         }
      }
      public string SzuletesiDatum
      {
         get { return m_SzuletesiDatum; }
         set
         {
            if ( value.Length > 20 || value.Length == 0 ) { MessageBox.Show( "Nem megfelelő a dátum ( " + value + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            try { Convert.ToDateTime( value ); }
            catch ( Exception ) { MessageBox.Show( "Nem megfelelő a születési dátum ( " + value + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            m_SzuletesiDatum = value;
         }
      }
      public string Engedely
      {
         get { return m_Engedely; }
         set
         {
            if ( value.Length >= 30 ) { MessageBox.Show( "Nem megfelelő az engedély ( " + value + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            m_Engedely = value;
         }
      }
      public string Egyesulet
      {
         get { return m_Egyesulet; }
         set
         {
            if ( value.Length > 30 ) { MessageBox.Show( "Nem megfelelő az egyesulet ( " + value + " " + value.Length + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            m_Egyesulet = value;
         }
      }
      public int Eredmenyek
      {
         get { return m_Eredmenyek; }
         set
         {
            try { Convert.ToInt32( m_Eredmenyek ); }
            catch ( Exception ) { MessageBox.Show( "Nem megfelelő az eredmenyek ( " + value + " )!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            m_Eredmenyek = value;
         }
      }

      public Induló( string _Nev,
               string _Nem,
               string _SzuletesiDatum,
               string _Engedely,
               string _Egyesulet,
               int _Eredmenyek )
         : this( )
      {
         Nev = _Nev;
         Nem = _Nem;
         SzuletesiDatum = _SzuletesiDatum;
         Engedely = _Engedely;
         Egyesulet = _Egyesulet;
         Eredmenyek = _Eredmenyek;
      }


      /*
   public string SzuletesiDatum{get{};set;}
   public string Engedely{get{};set;}
   public string Egyesulet{get{};set;}
   public int    Eredmenyek{get{};set;}
   */
   }







   public struct Induló2
   {
      public string Nev;
      public string Nem;
      public string SzuletesiDatum;
      public string Engedely;
      public string Egyesulet;
      public int Eredmenyek;







      public Induló2( string _Nev,
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

         eredmény_form = new Form_Induló_Teszt( table.SelectedRows[ 0 ].Cells[ 0 ].Value.ToString( ) );
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

      public sealed class Form_Induló : Form
      {
         private string eredeti_név = null;

         private TextBox box_név;
         private TextBox box_nem;
         private DateTimePicker date_születés;
         private TextBox box_engedély;
         private ComboBox cboEgyesulet;
         private Label eredmények_száma;

         public Form_Induló( )
         {
            InitializeForm( );
            InitializeContent( );
            InitializeData( );
         }

         public Form_Induló( Induló _induló )
         {
            eredeti_név = _induló.Nev;

            InitializeForm( );
            InitializeContent( );
            InitializeData( _induló );
         }

         private void InitializeForm( )
         {
            Text = "Induló";
            ClientSize = new System.Drawing.Size( 400 - 32, 262 );
            MinimumSize = ClientSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         }

         private void InitializeContent( )
         {
            Label név = new Label();
            név.Text = "Név:";
            név.Location = new System.Drawing.Point( 16, 16 + 0 * 32 );

            Label nem = new Label();
            nem.Text = "Nem:";
            nem.Location = new System.Drawing.Point( név.Location.X, 16 + 1 * 32 );

            Label születés = new Label();
            születés.Text = "Születési idő:";
            születés.Location = new System.Drawing.Point( név.Location.X, 16 + 2 * 32 );

            Label engedély = new Label();
            engedély.Text = "Engedélyszám:";
            engedély.Location = new System.Drawing.Point( név.Location.X, 16 + 3 * 32 );

            Label egyesület = new Label();
            egyesület.Text = "Egyesület:";
            egyesület.Location = new System.Drawing.Point( név.Location.X, 16 + 4 * 32 );

            Label eredmények = new Label();
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

            List<Egyesulet> egyesuletek = Program.database.Egyesuletek();

            foreach ( Egyesulet item in egyesuletek )
            {
               cboEgyesulet.Items.Add( item.Azonosito );
            }

            eredmények_száma = new Label( );
            eredmények_száma.Location = new System.Drawing.Point( eredmények.Location.X + eredmények.Size.Width + 16, eredmények.Location.Y );

            Button rendben = new Button();
            rendben.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            rendben.Text = "Rendben";
            rendben.Size = new System.Drawing.Size( 96, 32 );
            rendben.Location = new System.Drawing.Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 );
            rendben.Click += rendben_Click;

            Button btnEgyesulet = new iButton( "Új Egyesület",
                                                    new Point(ClientRectangle.Width - 96 - 96 - 32, ClientRectangle.Height - 32 - 16 ),
                                                    new Size(96,32),
                                                    btnEgyesulet_Click,
                                                    this);

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

         private void InitializeData( )
         {
            box_név.Text = "";
            box_név.Enabled = true;
            box_nem.Text = "";
            date_születés.Value = DateTime.Now;
            box_engedély.Text = "";
            eredmények_száma.Text = "0";
         }

         private void InitializeData( Induló _induló )
         {
            box_név.Text = _induló.Nev;
            box_nem.Text = _induló.Nem == "N" ? "Nő" : "Férfi";
            box_nem.Enabled = ( _induló.Eredmenyek > 0 ? false : true );
            date_születés.Value = DateTime.Parse( _induló.SzuletesiDatum );
            date_születés.Enabled = ( _induló.Eredmenyek > 0 ? false : true );
            box_engedély.Text = _induló.Engedely;
            eredmények_száma.Text = _induló.Eredmenyek.ToString( );
            cboEgyesulet.Text = _induló.Egyesulet;
         }

         private void rendben_Click( object _sender, EventArgs _event )
         {
            if ( date_születés.Value.Year == DateTime.Now.Year && date_születés.Value.Month == DateTime.Now.Month && date_születés.Value.Day == DateTime.Now.Day ) { MessageBox.Show( "A születési dátum nem lehet a mai nap!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( !( 0 < box_név.Text.Length && box_név.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő a név hossza (1 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( !Database.IsCorrectSQLText( box_név.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( !( 0 < box_nem.Text.Length && box_nem.Text.Length <= 10 ) ) { MessageBox.Show( "Nem megfelelő a nem hossza (1 - 10 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            bool nő = false;
            if ( box_nem.Text.ToLower( ) == "n" || box_nem.Text.ToLower( ) == "nő" ) nő = true;
            else if ( !( box_nem.Text.ToLower( ) == "f" || box_nem.Text.ToLower( ) == "férfi" ) ) { MessageBox.Show( "Nem megfelelő nem!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( !( box_engedély.Text.Length <= 30 ) ) { MessageBox.Show( "Nem megfelelő az engedély hossza (0 - 30 hosszú kell legyen)!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }
            if ( !Database.IsCorrectSQLText( box_engedély.Text ) ) { MessageBox.Show( "Nem megengedett karakterek a mezőben!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error ); return; }

            if ( eredeti_név != null )
            {
               Induló? eredeti = Program.database.Induló(eredeti_név);
               Program.mainform.indulók_panel.Induló_Módosítás( eredeti.Value,
                                                               new Induló( box_név.Text,
                                                                           ( nő ? "N" : "F" ),
                                                                           date_születés.Value.ToShortDateString( ),
                                                                           box_engedély.Text,
                                                                           cboEgyesulet.Text,
                                                                           Convert.ToInt32( eredmények_száma.Text ) ) );
            }
            else
            {
               Program.mainform.indulók_panel.Induló_Hozzáadás( new Induló( box_név.Text, ( nő ? "N" : "F" ), date_születés.Value.ToShortDateString( ), box_engedély.Text, cboEgyesulet.Text, 0 ) );
            }

            Close( );
         }

         private void
         btnEgyesulet_Click( object _sender, EventArgs _event )
         {
            Panel_Egyesuletek.Form_Egyesulet EgyesuletForm = new Panel_Egyesuletek.Form_Egyesulet();
            EgyesuletForm.Show( );
            EgyesuletForm.FormClosed += EgyesuletForm_FormClosed;
         }

         void EgyesuletForm_FormClosed( object sender, FormClosedEventArgs e )
         {
            cboEgyesulet.Items.Clear( );
            List<Egyesulet> egyesuletek = Program.database.Egyesuletek();

            foreach ( Egyesulet item in egyesuletek )
            {
               cboEgyesulet.Items.Add( item.Azonosito );
            }

            if ( cboEgyesulet.Items.Count != 0 )
               cboEgyesulet.SelectedIndex = cboEgyesulet.Items.Count - 1;
         }
      }

      public sealed class Form_Induló_Teszt : Form
      {
         public Form_Csapatlista csapatlista_form;

         public ComboBox cboVerseny;
         private Label lblIndulo;
         private ComboBox cboIjtipus;
         private ComboBox cboCsapat;
         private CheckBox chkMegjelent;
         private CheckBox chkKorosztalyFeluliras;
         private ComboBox cboKorosztaly;

         public Form_Induló_Teszt( string _Indulo )
         {

            InitializeForm( );
            InitializeContent( _Indulo );
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

         private void InitializeContent( string _Indulo )
         {
            Label lblNev = new iLabel( "Név:",
                                            new Point( 32, 16 + 0 * 32 ),
                                            this );

            Label lblVerseny = new iLabel( "Verseny:",
                                                new Point( lblNev.Location.X, 16 + 1 * 32 ),
                                                this );

            Label lblIjtipus = new iLabel( "Íjtípus:",
                                               new Point( lblNev.Location.X, 16 + 2 * 32 ),
                                               this );

            Label lblCsapat = new iLabel( "Csapatszám:",
                                             new Point( lblNev.Location.X, 16 + 3 * 32 ),
                                             this );

            Label lblMegjelent = new iLabel( "Megjelent:",
                                                 new Point( lblNev.Location.X, 16 + 4 * 32 ),
                                                 this );

            Label lblKorosztalyFeluliras = new iLabel( "Korosztály felülírása:",
                                                           new Point( lblNev.Location.X, 16 + 5 * 32 ),
                                                           this );

            Label lblKorosztalyAzonosito = new iLabel( "Új Korosztály azonosítója:",
                                                            new Point( lblNev.Location.X, 16 + 6 * 32 ),
                                                            this );


            lblIndulo = new iLabel( _Indulo,
                                    new Point( lblNev.Location.X + lblNev.Size.Width + 16 + 3 * 32, lblNev.Location.Y ),
                                    this );
            lblIndulo.AutoSize = false;
            lblIndulo.Size = new Size( 128 + 64, 24 );


            cboVerseny = new iComboBox( new Point( lblIndulo.Location.X, lblVerseny.Location.Y ),
                                        lblIndulo.Size,
                                        cboVerseny_SelectedIndexChanged,
                                        this );

            List<Verseny> versenyek = Program.database.Versenyek( );
            foreach ( Verseny current in versenyek ) { cboVerseny.Items.Add( current.Azonosito ); }

            cboIjtipus = new iComboBox( new Point( lblIndulo.Location.X, lblIjtipus.Location.Y ),
                                        lblIndulo.Size,
                                        null,
                                        this );

            List<Íjtípus> ijtipusok = Program.database.Íjtípusok( );
            foreach ( Íjtípus current in ijtipusok ) { cboIjtipus.Items.Add( current.Azonosito ); }

            cboCsapat = new iComboBox( new Point( lblIndulo.Location.X, lblCsapat.Location.Y ),
                                       lblIndulo.Size,
                                       null,
                                       this );
            for ( int i = 0 ; i < 45 ; i++ ) { cboCsapat.Items.Add( i + 1 ); }
            cboCsapat.SelectedItem = cboCsapat.Items[ 0 ];



            chkMegjelent = new iCheckBox( "",
                                        new Point( lblIndulo.Location.X, lblMegjelent.Location.Y ),
                                        null,
                                        this );

            chkKorosztalyFeluliras = new iCheckBox( null,
                                                    new Point( lblIndulo.Location.X, lblKorosztalyFeluliras.Location.Y ),
                                                    chkKorosztalyFeluliras_Click,
                                                    this );

            cboKorosztaly = new iComboBox( new Point( lblIndulo.Location.X, lblKorosztalyAzonosito.Location.Y ),
                                                      lblIndulo.Size,
                                                      null,
                                                      this );
            cboKorosztaly.Enabled = false;

            Button btnRendben = new iButton( "Rendben",
                                                new Point( ClientRectangle.Width - 96 - 16, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnRendben_Click,
                                                this );

            Button btnCsapatok = new iButton( "Csapatok megtekintése",
                                                new Point( ClientRectangle.Width - 96 - btnRendben.Width - 32, ClientRectangle.Height - 32 - 16 ),
                                                new Size( 96, 32 ),
                                                btnCsapatok_Click,
                                                this );
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

            Program.mainform.eredmények_panel.Eredmény_Beírás( lblIndulo.Text,
                                                                cboVerseny.Text,
                                                                cboIjtipus.Text,
                                                                cboCsapat.SelectedIndex + 1,
                                                                chkMegjelent.Checked,
                                                                chkKorosztalyFeluliras.Checked,
                                                                KOAZON );

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
}
