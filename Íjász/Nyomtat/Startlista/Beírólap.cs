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
   NyomtatBeirolap( string _VEAZON, Eredmény _eredmény )
      {
         BEIROLAP Data = new BEIROLAP( _VEAZON, _eredmény );

         #region Feliratok

         string Alairas1 = "\n\n\n       ------------------------------      ------------------------------";
         string Alairas2 = "                                 Beíró aláírása                                               Versenyző aláírása";

         string HeadLine = "B E Í R Ó L A P";
         string VersenyAzonosito = "Verseny azonosító, név: ";
         string VersenyIdo = "Verseny ideje: ";
         string VersenyOsszPont = "Verseny össz pontszám: ";
         string VersenySorozat = "Versenysorozat azonosító, név: ";
         string Sorszam = "Versenyző nevezés sorszám: ";
         string Csapat = "Csapatszám: ";
         string Nev = "Név: ";
         string Kor = "Betöltött kor: ";
         string Nem = "Nem: ";
         string Egyesulet = "Egyesület: ";
         string Engedely = "Versenyengedélyszám: ";
         string Ijtipus = "Íj típus: ";
         string Korosztaly = "Korosztály: ";
         #endregion

         string FileName=null;

         if ( Data.VersenyAdatok.VSAZON != null )
         {
            FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "BEIRLAP.docx";
         }
         else
         {
            FileName = _VEAZON + "\\" + "BEIRLAP.docx";
         }

         var document = DocX.Create(FileName);
         document.MarginBottom = 10;
         document.AddHeaders( );

         #region Title

         var titleFormat = new Formatting();
         titleFormat.Size = 10D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph();
         title.Append( HeadLine );
         title.Alignment = Alignment.center;
         title.AppendLine( Program.Tulajdonos_Megnevezés + "\n" );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         #region Data

         Table table = document.AddTable(Data.VersenyAdatok.VEALSZ + 3, 8);
         table.Alignment = Alignment.center;

         table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( "Sorszám" ).Bold( );
         table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Lőállás" ).Bold( );
         table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( "10 pont" ).Bold( );
         table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( "8 pont" ).Bold( );
         table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( "5 pont" ).Bold( );
         table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( "Mellé" ).Bold( );
         table.Rows[ 0 ].Cells[ 6 ].Paragraphs[ 0 ].Append( "Összesen" ).Bold( );
         table.Rows[ 0 ].Cells[ 7 ].Paragraphs[ 0 ].Append( "Göngyölt" ).Bold( );

         for ( int i = 1 ; i <= Data.VersenyAdatok.VEALSZ ; i++ )
         {
            table.Rows[ i ].Cells[ 0 ].Paragraphs[ 0 ].Append( ( i ).ToString( ) );
         }

         table.Rows[ Data.VersenyAdatok.VEALSZ + 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Össz darab" ).Bold( );
         table.Rows[ Data.VersenyAdatok.VEALSZ + 2 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Össz pont" ).Bold( );
         #endregion

         #region Header

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = document.InsertParagraph(VersenyAzonosito, false, titleFormat2);

         Table header_table = document.AddTable(4,3);

         header_table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Sorszam );
         titleFormat2.Size = 18D;
         header_table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].InsertText( Data.VersenyzoAdatok.INSOSZ.ToString( ), false, titleFormat2 );

         header_table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Csapat );
         header_table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.INCSSZ.ToString( ) ).Bold( );

         header_table.Rows[ 1 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Nev );
         titleFormat2.Size = _eredmény.Nev.Length > 20 ? 14D : 18D;
         header_table.Rows[ 1 ].Cells[ 0 ].Paragraphs[ 0 ].InsertText( _eredmény.Nev.ToString( ), false, titleFormat2 );

         header_table.Rows[ 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Kor );
         header_table.Rows[ 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.INBEEK.ToString( ) ).Bold( );

         header_table.Rows[ 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Nem );
         header_table.Rows[ 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.INNEME.ToString( ) ).Bold( );

         header_table.Rows[ 2 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Egyesulet );
         header_table.Rows[ 2 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.INEGYE.ToString( ) ).Bold( );

         header_table.Rows[ 2 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Engedely );
         header_table.Rows[ 2 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.INVEEN.ToString( ) ).Bold( );

         header_table.Rows[ 3 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Ijtipus );
         header_table.Rows[ 3 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.ITMEGN.ToString( ) ).Bold( );

         header_table.Rows[ 3 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Korosztaly );
         header_table.Rows[ 3 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok.KOMEGN.ToString( ) ).Bold( );

         BeirolapHeaderTablazatFormazas( header_table );
         document.InsertTable( header_table );
         Paragraph temp = document.InsertParagraph();

         paragraph_1.Append( _VEAZON + ", " + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + VersenyIdo );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t\t" + VersenyOsszPont );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + VersenySorozat );

         if ( Data.VersenyAdatok.VSAZON != null )
         {
            paragraph_1.Append( Data.VersenyAdatok.VSAZON + ", " + Data.VersenyAdatok.VSMEGN );
         }
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );
         #endregion

         BeirolapTablazatFormazas( table );
         document.InsertTable( table );

         Paragraph paragraph_3 = document.InsertParagraph(Alairas1, false, titleFormat2);
         paragraph_3.AppendLine( Alairas2 );

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "BEIRLAP.DOCX ", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return FileName;
      }


   }
}
