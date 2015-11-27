using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
   public static partial class Nyomtat
   {
      static public string
NyomtatNevezesiLista( string _VEAZON, bool _NemMegjelentNyomtat )
      {
         NEVEZESILISTA Data = new NEVEZESILISTA( _VEAZON, _NemMegjelentNyomtat );

         #region Feliratok
         string HeadLine;
         string Indulok;

         if ( _NemMegjelentNyomtat )
         {
            HeadLine = "H I Á N Y Z Ó K  L I S T A";
            Indulok = "Hiányzók száma: ";

         }
         else
         {
            HeadLine = "N E V E Z É S I  L I S T A";
            Indulok = "Indulók száma: ";

         }
         string VersenyAzonosito = "Verseny azonosító, név: ";
         string VersenyIdo = "Verseny ideje: ";
         string VersenyOsszPont = "Verseny össz pontszám: ";
         string VersenySorozat = "Versenysorozat azonosító, név: ";
         #endregion

         #region FileName

         string FileName;

         if ( _NemMegjelentNyomtat )
         {
            if ( Data.VersenyAdatok.VSAZON != null )
            {
               FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "NEVEZLISTANEMMEGJELENT.docx";
            }
            else
            {
               FileName = _VEAZON + "\\" + "NEVEZLISTANEMMEGJELENT.docx";
            }
         }
         else
         {
            if ( Data.VersenyAdatok.VSAZON != null )
            {
               FileName = Data.VersenyAdatok.VSAZON + "\\" + Data.VersenyAdatok.VEAZON + "\\" + "NEVEZLISTA.docx";
            }
            else
            {
               FileName = _VEAZON + "\\" + "NEVEZLISTA.docx";
            }
         }
         #endregion

         var document = DocX.Create( FileName );
         PageNumber( document );
         document.DifferentFirstPage = true;

         #region FirstPageFooter

         Footer footer = document.Footers.first;

         Table FooterTable = footer.InsertTable( 1, 2 );
         FooterTable.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "1. oldal" );
         FooterTable.AutoFit = AutoFit.ColumnWidth;
         FooterTable.Rows[ 0 ].Cells[ 0 ].Width = document.PageWidth - 200;
         FooterTable.Rows[ 0 ].Cells[ 1 ].Width = 60;

         Border c = new Border( Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black );
         FooterTable.SetBorder( TableBorderType.InsideH, c );
         FooterTable.SetBorder( TableBorderType.InsideV, c );
         FooterTable.SetBorder( TableBorderType.Bottom, c );
         FooterTable.SetBorder( TableBorderType.Top, c );
         FooterTable.SetBorder( TableBorderType.Left, c );
         FooterTable.SetBorder( TableBorderType.Right, c );
         #endregion

         #region címbekezdés

         var titleFormat = new Formatting( );
         titleFormat.Size = 14D;
         titleFormat.Position = 1;
         titleFormat.Spacing = 5;
         titleFormat.Bold = true;

         document.AddHeaders( );
         Header FirstPageHeader = document.Headers.first;

         Paragraph title = FirstPageHeader.InsertParagraph();
         title.Append( HeadLine );
         title.Alignment = Alignment.center;
         titleFormat.Size = 10D;
         title.AppendLine( Program.Tulajdonos_Megnevezés + "\n" );
         title.Bold( );
         titleFormat.Position = 12;

         #endregion

         #region header

         var titleFormat2 = new Formatting( );
         titleFormat2.Size = 10D;
         titleFormat2.Position = 1;

         Paragraph paragraph_1 = FirstPageHeader.InsertParagraph( VersenyAzonosito, false, titleFormat2 );

         paragraph_1.Append( Data.VersenyAdatok.VEAZON + ", " + Data.VersenyAdatok.VEMEGN );
         paragraph_1.Bold( );
         titleFormat2.Bold = false;
         paragraph_1.Append( "\n" + VersenyIdo );
         paragraph_1.Append( Data.VersenyAdatok.VEDATU );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t\t" + VersenyOsszPont );
         paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\t\t" + Indulok );
         paragraph_1.Append( Data.VersenyAdatok.VEINSZ.ToString( ) );
         paragraph_1.Bold( );
         paragraph_1.Append( "\n" + VersenySorozat );
         paragraph_1.Append( Data.VersenyAdatok.VSAZON + ", " + Data.VersenyAdatok.VSMEGN );
         paragraph_1.Bold( );
         paragraph_1.AppendLine( );
         #endregion

         #region HeaderTable

         Header TablazatFejlec = document.Headers.odd;

         Table HeaderTable = document.AddTable( 1, 6 );
         HeaderTable.AutoFit = AutoFit.ColumnWidth;

         HeaderTable.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( "Sorszám" );
         HeaderTable.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Név" );
         HeaderTable.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( "Íjtípus" );
         HeaderTable.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( "Kor" );
         HeaderTable.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( "Egyesület" );
         HeaderTable.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( "Csapat" );

         NevezesiListaTablazatFormazas( HeaderTable );
         TablazatFejlec.InsertTable( HeaderTable );

         #endregion

         #region táblázat formázás

         Table table = document.AddTable( Data.VersenyAdatok.VEINSZ + 1, 6 );

         table.Rows[ 0 ].Cells[ 0 ].Paragraphs[ 0 ].Append( "Sorszám" );
         table.Rows[ 0 ].Cells[ 1 ].Paragraphs[ 0 ].Append( "Név" );
         table.Rows[ 0 ].Cells[ 2 ].Paragraphs[ 0 ].Append( "Íjtípus" );
         table.Rows[ 0 ].Cells[ 3 ].Paragraphs[ 0 ].Append( "Kor" );
         table.Rows[ 0 ].Cells[ 4 ].Paragraphs[ 0 ].Append( "Egyesület" );
         table.Rows[ 0 ].Cells[ 5 ].Paragraphs[ 0 ].Append( "Csapat" );
         #endregion

         #region táblázat adatok betöltése

         for ( int i = 0 ; i < Data.VersenyAdatok.VEINSZ ; i++ )
         {
            table.Rows[ i + 1 ].Cells[ 0 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok[ i ].INSOSZ.ToString( ) );
            table.Rows[ i + 1 ].Cells[ 1 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok[ i ].INNEVE );
            table.Rows[ i + 1 ].Cells[ 2 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok[ i ].ITMEGN );
            table.Rows[ i + 1 ].Cells[ 3 ].Paragraphs[ 0 ].Append( ( Data.VersenyzoAdatok[ i ].INSZUL.ToString( ) ) );
            table.Rows[ i + 1 ].Cells[ 4 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok[ i ].INEGYE );
            table.Rows[ i + 1 ].Cells[ 5 ].Paragraphs[ 0 ].Append( Data.VersenyzoAdatok[ i ].INCSSZ.ToString( ) );
         }
         #endregion

         NevezesiListaTablazatFormazas( table );
         document.InsertTable( table );

         try { document.Save( ); }
         catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "NEVEZLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
         return FileName;

      }

   }
}
