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
      NyomtatCsapatlista( string _VEAZON )
      {
         string FileName = null;
         CSAPATLISTA Data = new CSAPATLISTA( _VEAZON );

         #region Feliratok 
         string HeadLine = "C S A P A T L I S T A";
         string VersenyAzonosito = "Verseny azonosító, név: ";
         string VersenyIdo = "Verseny ideje: ";
         string VersenyOsszPont = "Verseny össz pontszám: ";
         string VersenyIndulokSzama = "Indulók száma: ";
         string VersenySorozat = "Versenysorozat azonosító, név: ";
         #endregion

         if ( Data.VersenyAdatok.VSAZON != null )
         {
            FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "CSAPATLISTA.docx";
         }
         else
         {
            FileName = _VEAZON + "\\" + "CSAPATLISTA.docx";
         }

         var document = DocX.Create( FileName );
         document.AddHeaders( );
         PageNumber( document );

         #region címbekezdés

         var titleFormat = new Formatting();
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         Header header = document.Headers.odd;

         Paragraph title = header.InsertParagraph();
         title.Append( HeadLine );
         title.Alignment = Alignment.center;

         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés + "\n" );
         title.Bold( );
         titleFormat.Position = 12;
         #endregion

         #region header

         var titleFormat2 = new Formatting();
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = header.InsertParagraph();
         paragraph_1.Append( VersenyAzonosito );

         paragraph_1.Append( _VEAZON + ", " + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + VersenyIdo );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t\t" + VersenyOsszPont );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t\t" + VersenyIndulokSzama );
         paragraph_1.Append( Data.VersenyAdatok.VEINSZ.ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.VersenyAdatok.VSAZON + ", " + Data.VersenyAdatok.VSMEGN );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );
         #endregion

         #region Data
         for ( int i = 0 ; i < Data.Csapatok.Count ; i++ )
         {
            Table table = document.AddTable( Data.Csapatok[i].versenyzoadatok.Count + 1, 6 );
            table.Alignment = Alignment.center;

            table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( "Csapat" ).Bold( );
            table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Sorszám" ).Bold( );
            table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( "Név" ).Bold( );
            table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( "Íjtípus" ).Bold( );
            table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( "Kor" ).Bold( );
            table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( "Egyesület" ).Bold( );

            int q = 1;
            foreach ( CSAPATLISTA.VERSENYZOADAT versenyzo in Data.Csapatok[ i ].versenyzoadatok )
            {
               table.Rows[ q ].Cells[ 0 ].Paragraphs[ 0 ].Append( versenyzo.INCSSZ.ToString( ) );
               table.Rows[ q ].Cells[ 1 ].Paragraphs[ 0 ].Append( versenyzo.INSOSZ.ToString( ) );
               table.Rows[ q ].Cells[ 2 ].Paragraphs[ 0 ].Append( versenyzo.INNEVE );
               table.Rows[ q ].Cells[ 3 ].Paragraphs[ 0 ].Append( versenyzo.ITMEGN );
               table.Rows[ q ].Cells[ 4 ].Paragraphs[ 0 ].Append( versenyzo.INSZUL.ToString( ) );
               table.Rows[ q ].Cells[ 5 ].Paragraphs[ 0 ].Append( versenyzo.INEGYE );
               q++;
            }
            CsapatlistaTablazatFormazas( table );
            document.InsertTable( table );
            if ( i != Data.Csapatok.Count - 1 )
            {
               document.InsertSectionPageBreak( );
            }
         }
         #endregion

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "CSAPATLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

         return FileName;

      }

   }
}
