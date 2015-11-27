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
      NyomtatEredmenylapVersenyTeljes( string _VEAZON )
      {
         string filename = null;

         EREDMENYLAPVERSENYTELJES Data = new EREDMENYLAPVERSENYTELJES( _VEAZON );

         #region alap stringek
         string headline = "EREDMÉNYLAP";
         string típus = "***TELJES***";
         string st_vazon_vnev = "Verseny azonosítója, neve: ";
         string st_ido = "Verseny ideje: ";
         string st_vosszp = "Verseny összpontszáma: ";
         string st_insz = "Indulók száma: ";
         string st_vsorazon = "Versenysorozat azonosítója, neve: ";
         #endregion

         if ( Data.VersenyAdatok.VSAZON != null )
         {
            filename = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVETELJ.docx";
         }
         else
         {
            filename = _VEAZON + "\\" + "ERLAPVETELJ.docx";
         }
         var document = DocX.Create(filename);
         document.AddHeaders( );
         document.MarginBottom = 10;
         PageNumber( document );
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
         title.AppendLine( Program.Tulajdonos_Megnevezés );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph();
         paragraph_1.AppendLine( st_vazon_vnev );

         paragraph_1.Append( _VEAZON + "," + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + st_ido );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t" + st_vosszp );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t" + st_insz );
         paragraph_1.Append( Data.VersenyAdatok.VEINSZ.ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + st_vsorazon );
         paragraph_1.Append( Data.VersenyAdatok.VSAZON + "," + Data.VersenyAdatok.VSMEGN );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );

         for ( int i = 0 ; i < Data.Ijtipusok.Count ; i++ )
         {
            Table table = null;
            for ( int j = 0 ; j < Data.Ijtipusok[ i ].Korosztalyok.Count ; j++ )
            {
               if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 ||
                   Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 ||
                   Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
               {
                  Paragraph adatok = document.InsertParagraph( );
                  adatok.Append( "Ijtipusok: " );
                  adatok.Append( Data.Ijtipusok[ i ].Megnevezes );
                  adatok.Bold( );
                  adatok.AppendLine( "    Korosztály: " );
                  adatok.Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Megnevezes );
                  adatok.Bold( );

                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 )
                  {
                     Paragraph np = document.InsertParagraph( );
                     np.Append( "      Nők: " );
                     np.Bold( );
                  }

                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );

                     document.InsertTable( table );
                  }
                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Férfiak: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );
                     document.InsertTable( table );
                  }
                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Egyben: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );
                     document.InsertTable( table );
                  }

               }
            }
         }

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVETELJ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return filename;
      }

      static public string
      NyomtatEredmenylapVersenyMisz( string _VEAZON )
      {
         string FileName = null;

         EREDMENYLAPVERSENYMISZ Data = new EREDMENYLAPVERSENYMISZ( _VEAZON );

         #region Feliratok
         string HeadLine = "EREDMÉNYLAP";
         string Tipus = "***MISZ***";
         string VersenyAzonosito = "Verseny azonosítója, neve: ";
         string VersenyIdo = "Verseny ideje: ";
         string VersenyOsszPont = "Verseny összpontszáma: ";
         string VersenyIndulok = "Indulók száma: ";
         string VersenySorozat = "Versenysorozat azonosítója, neve: ";
         #endregion

         #region FileName
         if ( Data.VersenyAdatok.VSAZON != null )
         {
            FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVEMISZ.docx";
         }
         else
         {
            FileName = _VEAZON + "\\" + "ERLAPVEMISZ.docx";
         }
         #endregion

         var document = DocX.Create( FileName );
         document.AddHeaders( );
         document.MarginBottom = 10;
         PageNumber( document );

         #region Header

         var titleFormat = new Formatting( );
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph( );
         title.Append( HeadLine );
         title.AppendLine( Tipus );
         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         var titleFormat2 = new Formatting( );
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph( );
         paragraph_1.AppendLine( VersenyAzonosito );

         paragraph_1.Append( _VEAZON + "," + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + VersenyIdo );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t" + VersenyOsszPont );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t" + VersenyIndulok );
         paragraph_1.Append( Data.VersenyAdatok.VEINSZ.ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.VersenyAdatok.VSAZON + "," + Data.VersenyAdatok.VSMEGN );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );

         for ( int i = 0 ; i < Data.Ijtipusok.Count ; i++ )
         {
            Table table = null;
            for ( int j = 0 ; j < Data.Ijtipusok[ i ].Korosztalyok.Count ; j++ )
            {
               if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 ||
                   Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 ||
                   Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
               {
                  Paragraph adatok = document.InsertParagraph( );
                  adatok.Append( "Ijtipusok: " );
                  adatok.Append( Data.Ijtipusok[ i ].Megnevezes );
                  adatok.Bold( );
                  adatok.AppendLine( "    Korosztály: " );
                  adatok.Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Megnevezes );
                  adatok.Bold( );

                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count != 0 )
                  {
                     Paragraph np = document.InsertParagraph( );
                     np.Append( "      Nők: " );
                     np.Bold( );
                  }

                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Nok[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );

                     document.InsertTable( table );
                  }
                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Férfiak: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Ferfiak[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );
                     document.InsertTable( table );
                  }

                  if ( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count != 0 )
                  {
                     Paragraph fp = document.InsertParagraph( );
                     fp.Append( "      Egyben: " );
                     fp.Bold( );
                  }
                  for ( int k = 0 ; k < Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben.Count ; k++ )
                  {
                     table = document.AddTable( 1, 7 );
                     table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( ( k + 1 ) + "." );
                     table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Sorszam.ToString( ) );
                     table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Nev );
                     table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Egyesulet );
                     table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].OsszPont.ToString( ) + " pont" );
                     table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( Data.Ijtipusok[ i ].Korosztalyok[ j ].Indulok.Egyben[ k ].Szazalek.ToString( ) + " %" );
                     EredmenyLapVersenyTablazatFormazas( table );
                     document.InsertTable( table );
                  }

               }
            }
         }

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVEMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return FileName;
      }

      static public string
      NyomtatEredmenylapVersenyEgyesulet( string _VEAZON )
      {
         EREDMENYLAPVERSENYEGYESULET Data = new EREDMENYLAPVERSENYEGYESULET(_VEAZON);

         string FileName = null;

         #region alap stringek
         string Cim = "EREDMÉNYLAP";
         string Tipus = "***egyesület***";
         string Verseny = "Verseny azonosítója, neve: ";
         string Ido = "Verseny ideje: ";
         string Osszpont = "Verseny összpontszáma: ";
         string VersenySorozat = "Versenysorozat azonosítója, neve: ";
         #endregion

         if ( Data.VersenyAdatok.VSAZON != null )
         {
            FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVEEGYE.docx";
         }
         else
         {
            FileName = _VEAZON + "\\" + "ERLAPVEEGYE.docx";
         }

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
         paragraph_1.AppendLine( Verseny );

         paragraph_1.Append( _VEAZON + ", " + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + Ido );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t" + Osszpont );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.VersenyAdatok.VSAZON + "," + Data.VersenyAdatok.VSMEGN );
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
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "CSAPATLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

         return FileName;
      }

   }
}
