using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
   public static partial class Nyomtat
   {

      static public string
      NyomtatEredmenylapVersenysorozatEgyesulet( string _VSAZON )
      {
         string FileName = _VSAZON + "\\" + "ERLAPVSEGYE.docx";
         EREDMENYLAPVERSENYSOROZATEGYESULET Data = new EREDMENYLAPVERSENYSOROZATEGYESULET(_VSAZON);

         #region alap stringek
         string Cim = "EREDMÉNYLAP";
         string Tipus = "***egyesület***";
         string VersenySorozat = "Versenysorozat azonosítója, neve: ";
         #endregion


         var document = DocX.Create(FileName);
         document.AddHeaders( );
         PageNumber( document );

         #region header

         var titleFormat = new Formatting();
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph();
         title.Append( Cim );
         title.AppendLine( Tipus );
         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         #region Title

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph();

         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.Azonosito + "," + Data.Megnevezes );
         paragraph_1.AppendLine( );
         paragraph_1.Bold( );

         #endregion

         Table table = document.AddTable(1, 4);
         table.Alignment = Alignment.center;

         table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( "Sorrend" ).Bold( );
         table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Egyesület neve" ).Bold( );
         table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( "Egyesület címe" ).Bold( );
         table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( "ÖsszPont" ).Bold( );


         for ( int i = 0 ; i < Data.Egyesuletek.Count ; i++ )
         {
            table.InsertRow( );
            table.Rows[ table.Rows.Count - 1 ].Cells[ 0 ].Paragraphs[ 0 ].Append( i + 1 + "." );
            table.Rows[ table.Rows.Count - 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.Egyesuletek[ i ].Nev );
            table.Rows[ table.Rows.Count - 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Egyesuletek[ i ].Cim );
            table.Rows[ table.Rows.Count - 1 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Egyesuletek[ i ].OsszPont.ToString( ) );
         }
         EgyesuletTablazatFormazas( table );

         document.InsertTable( table );

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVSEGYE.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

         return FileName;
      }

      static public string
      NyomtatEredmenylapVersenySorozatReszletes( string _VSAZON )
      {
         string FileName = _VSAZON + "\\" + "ERLAPVSRESZ.docx";
         EREDMENYLAPVERSENYSOROZATRESZLETES Data = new EREDMENYLAPVERSENYSOROZATRESZLETES(_VSAZON);


         var document = DocX.Create(FileName);
         document.AddHeaders( );
         document.PageLayout.Orientation = Novacode.Orientation.Landscape;
         document.MarginLeft = 20;
         document.MarginRight = 20;
         document.MarginTop = 20;
         document.MarginBottom = 20;
         PageNumber( document );

         #region alap stringek
         string Cim = "EREDMÉNYLAP";
         string Tipus = "***részletes***";
         string VersenySorozat = "Versenysorozat azonosítója, neve: ";
         #endregion

         #region header

         var titleFormat = new Formatting();
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph();
         title.Append( Cim );
         title.AppendLine( Tipus );
         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         #region Title

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph();

         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.Azonosito + "," + Data.Megnevezes ).Bold( );
         paragraph_1.Bold( );

         #endregion

         #region HeaderTable
         Table HeaderTable = document.AddTable(1, Data.VersenyekSzama + 5);
         HeaderTable.AutoFit = AutoFit.ColumnWidth;
         for ( int z = 0 ; z < HeaderTable.Rows.Count ; z++ )
         {
            HeaderTable.Rows[ z ].Cells[ 0 ].Width = 30;
            HeaderTable.Rows[ z ].Cells[ 1 ].Width = 50;
            HeaderTable.Rows[ z ].Cells[ 2 ].Width = 200;
            HeaderTable.Rows[ z ].Cells[ 3 ].Width = 300;
            for ( int q = 4 ; q < HeaderTable.Rows[ z ].Cells.Count - 1 ; q++ )
            {
               HeaderTable.Rows[ z ].Cells[ q ].Width = 70;
            }
            HeaderTable.Rows[ z ].Cells[ HeaderTable.Rows[ 0 ].Cells.Count - 1 ].Width = 100;
            HeaderTable.Rows[ z ].Height = 27;
         }

         for ( int z = 0 ; z < Data.VersenyAzonositok.Count ; z++ )
         {
            HeaderTable.Rows[ 0 ].Cells[ z + 4 ].Paragraphs[ 0 ].Append( Data.VersenyAzonositok[ z ] ).Bold( );
            HeaderTable.Rows[ 0 ].Cells[ z + 4 ].Paragraphs[ 0 ].FontSize( 8D );

         }
         HeaderTable.Rows[ 0 ].Cells[ HeaderTable.Rows[ 0 ].Cells.Count - 1 ].Paragraphs[ 0 ].Append( "Összesen" ).Bold( ); ;
         HeaderTable.Rows[ 0 ].Cells[ HeaderTable.Rows[ 0 ].Cells.Count - 1 ].Paragraphs[ 0 ].FontSize( 8D );

         HeaderTable.AutoFit = AutoFit.ColumnWidth;
         EredmenyLapReszletesTablazatFormazas( HeaderTable );

         header.InsertTable( HeaderTable );
         #endregion

         for ( int i = 0 ; i < Data.IjTipusok.Count ; i++ )
         {
            Table table = null;
            for ( int j = 0 ; j < Data.IjTipusok[ i ].Korosztalyok.Count ; j++ )
            {
               if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 ||
                    Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 ||
                    Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
               {
                  Paragraph adatok = document.InsertParagraph();
                  adatok.Append( "Íjtípus: " );
                  adatok.Append( Data.IjTipusok[ i ].Megnevezes );
                  adatok.Bold( );
                  adatok.AppendLine( "    Korosztály: " );
                  adatok.Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Megnevezes );
                  adatok.Bold( );

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 )
                  {
                     Paragraph np = document.InsertParagraph();
                     np.Append( "      Nők: " );
                     np.Bold( );
                  }

                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count ; k++ )
                  {
                     table = document.AddTable( 1, Data.VersenyekSzama + 5 );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Nev );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Egyesulet );
                     for ( int m = 0 ; m < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Eredmenyek.Count ; m++ )
                     {
                        for ( int n = 0 ; n < Data.VersenyekSzama ; n++ )
                        {
                           if ( HeaderTable.Rows[ 0 ].Cells[ 4 + n ].Paragraphs[ 0 ].Text == Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Eredmenyek[ m ].Verseny )
                           {
                              table.Rows[ table.Rows.Count - 1 ].Cells[ n + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Eredmenyek[ m ].Pont.ToString( ) );
                           }
                        }
                     }
                     table.Rows[ table.Rows.Count - 1 ].Cells[ Data.VersenyekSzama + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].OsszPont.ToString( ) + " pont" ).Bold( );

                     for ( int ii = 0 ; ii < table.Rows.Count ; ii++ )
                     {
                        for ( int jj = 0 ; jj < Data.VersenyekSzama ; jj++ )
                        {
                           if ( table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Text == "" )
                           {
                              table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Append( "0" );
                           }
                        }
                     }

                     EredmenyLapReszletesTablazatFormazas( table );
                     document.InsertTable( table );
                  }

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph();
                     fp.Append( "      Férfiak: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count ; k++ )
                  {
                     table = document.AddTable( 1, Data.VersenyekSzama + 5 );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Nev );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Egyesulet );
                     for ( int m = 0 ; m < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Eredmenyek.Count ; m++ )
                     {
                        for ( int n = 0 ; n < Data.VersenyekSzama ; n++ )
                        {
                           if ( HeaderTable.Rows[ 0 ].Cells[ 4 + n ].Paragraphs[ 0 ].Text == Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Eredmenyek[ m ].Verseny )
                           {
                              table.Rows[ table.Rows.Count - 1 ].Cells[ n + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Eredmenyek[ m ].Pont.ToString( ) );
                           }
                        }
                     }
                     table.Rows[ table.Rows.Count - 1 ].Cells[ Data.VersenyekSzama + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].OsszPont.ToString( ) + " pont" ).Bold( );
                     for ( int ii = 0 ; ii < table.Rows.Count ; ii++ )
                     {
                        for ( int jj = 0 ; jj < Data.VersenyekSzama ; jj++ )
                        {
                           if ( table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Text == "" )
                           {
                              table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Append( "0" );
                           }
                        }
                     }
                     EredmenyLapReszletesTablazatFormazas( table );
                     document.InsertTable( table );
                  }

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Egyben: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count ; k++ )
                  {
                     table = document.AddTable( 1, Data.VersenyekSzama + 5 );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Nev );
                     table.Rows[ table.Rows.Count - 1 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Egyesulet );
                     for ( int m = 0 ; m < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Eredmenyek.Count ; m++ )
                     {
                        for ( int n = 0 ; n < Data.VersenyekSzama ; n++ )
                        {
                           if ( HeaderTable.Rows[ 0 ].Cells[ 4 + n ].Paragraphs[ 0 ].Text == Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Eredmenyek[ m ].Verseny )
                           {
                              table.Rows[ table.Rows.Count - 1 ].Cells[ n + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Eredmenyek[ m ].Pont.ToString( ) );
                           }
                        }
                     }
                     table.Rows[ table.Rows.Count - 1 ].Cells[ Data.VersenyekSzama + 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].OsszPont.ToString( ) + " pont" ).Bold( );
                     for ( int ii = 0 ; ii < table.Rows.Count ; ii++ )
                     {
                        for ( int jj = 0 ; jj < Data.VersenyekSzama ; jj++ )
                        {
                           if ( table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Text == "" )
                           {
                              table.Rows[ ii ].Cells[ jj + 4 ].Paragraphs[ 0 ].Append( "0" );
                           }
                        }
                     }
                     EredmenyLapReszletesTablazatFormazas( table );
                     document.InsertTable( table );
                  }
               }

            }

         }


         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVSRESZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return FileName;
      }

      static public string
      NyomtatEredmenylapVersenysorozatTeljes( string _VSAZON )
      {
         EREDMENYLAPVESENYSOROZATTELJES Data = new EREDMENYLAPVESENYSOROZATTELJES( _VSAZON );

         string filename = _VSAZON + "\\" + "ERLAPVSTELJ.docx";
         var document = DocX.Create(filename);
         document.AddHeaders( );
         PageNumber( document );
         #region alap stringek
         string headline = "EREDMÉNYLAP";
         string típus = "***teljes***";
         string st_vsorazon = "Versenysorozat azonosítója, neve: ";
         string megnevezés = null;
         #endregion

         #region header

         var titleFormat = new Formatting();
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph();
         title.Append( headline );
         title.AppendLine( típus );

         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés + "\n" );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph();
         //megnevezés
         List<Versenysorozat> vsor = Program.database.Versenysorozatok();

         foreach ( Versenysorozat item in vsor )
         {
            if ( _VSAZON == item.azonosító )
            {
               megnevezés = item.megnevezés;
               break;
            }
         }

         paragraph_1.Append( st_vsorazon );
         paragraph_1.Append( _VSAZON + ", " + megnevezés );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );

         #region formázás

         for ( int i = 0 ; i < Data.IjTipusok.Count ; i++ )
         {
            Table table = null;
            for ( int j = 0 ; j < Data.IjTipusok[ i ].Korosztalyok.Count ; j++ )
            {
               if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 ||
                   Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 ||
                   Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
               {
                  Paragraph adatok = document.InsertParagraph( );
                  adatok.Append( "IjTipusok: " );
                  adatok.Append( Data.IjTipusok[ i ].Megnevezes );
                  adatok.Bold( );
                  adatok.AppendLine( "    Korosztály: " );
                  adatok.Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Megnevezes );
                  adatok.Bold( );

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 )
                  {
                     Paragraph np = document.InsertParagraph( );
                     np.Append( "      Nők: " );
                     np.Bold( );
                  }

                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].AtlagSzazalek + " %" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );

                     document.InsertTable( table );
                  }
                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Férfiak: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].AtlagSzazalek + " %" ); ;
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );
                     document.InsertTable( table );
                  }

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Egyben: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].AtlagSzazalek + " %" ); ;
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );
                     document.InsertTable( table );
                  }

               }
            }
         }


         #endregion

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVSTELJ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return filename;
      }

      static public string
      NyomtatEredmenylapVersenysorozatMisz( string _VSAZON )
      {
         EREDMENYLAPVESENYSOROZATMISZ Data = new EREDMENYLAPVESENYSOROZATMISZ( _VSAZON );

         string filename = _VSAZON + "\\" + "ERLAPVSMISZ.docx";
         var document = DocX.Create( filename );
         document.AddHeaders( );
         PageNumber( document );
         #region alap stringek
         string headline = "EREDMÉNYLAP";
         string típus = "***MISZ***";
         string st_vsorazon = "Versenysorozat azonosítója, neve: ";
         string megnevezés = null;
         #endregion

         #region header

         var titleFormat = new Formatting( );
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph( );
         title.Append( headline );
         title.AppendLine( típus );

         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés + "\n" );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         var titleFormat2 = new Formatting( );
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph( );
         //megnevezés
         List<Versenysorozat> vsor = Program.database.Versenysorozatok( );

         foreach ( Versenysorozat item in vsor )
         {
            if ( _VSAZON == item.azonosító )
            {
               megnevezés = item.megnevezés;
               break;
            }
         }

         paragraph_1.Append( st_vsorazon );
         paragraph_1.Append( _VSAZON + ", " + megnevezés );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );

         #region formázás

         for ( int i = 0 ; i < Data.IjTipusok.Count ; i++ )
         {
            Table table = null;
            for ( int j = 0 ; j < Data.IjTipusok[ i ].Korosztalyok.Count ; j++ )
            {
               if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 ||
                   Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 ||
                   Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
               {
                  Paragraph adatok = document.InsertParagraph( );
                  adatok.Append( "IjTipusok: " );
                  adatok.Append( Data.IjTipusok[ i ].Megnevezes );
                  adatok.Bold( );
                  adatok.AppendLine( "    Korosztály: " );
                  adatok.Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Megnevezes );
                  adatok.Bold( );

                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 )
                  {
                     Paragraph np = document.InsertParagraph( );
                     np.Append( "      Nők: " );
                     np.Bold( );
                  }

                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].AtlagSzazalek + " %" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );

                     document.InsertTable( table );
                  }
                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Férfiak: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].AtlagSzazalek + " %" ); ;
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );
                     document.InsertTable( table );
                  }
                  if ( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Egyben: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].EletKor.ToString( ) );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].AtlagSzazalek + " %" ); ;
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.IjTipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].OsszPont.ToString( ) + " pont" );
                     EredmenyLapVersenySorozatTablazatFormazas( table );
                     document.InsertTable( table );
                  }
               }
            }
         }


         #endregion

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVSMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return filename;
      }
   }
}
