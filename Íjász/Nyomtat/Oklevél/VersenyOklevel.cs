using System;
using Novacode;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;


namespace Íjász {
    public partial class Nyomtat {

        /// <summary>kiszedi az első _Limit eredményt, meghívja rájuk a NyomtatOklevelVersenyVersenyzo fgv-t </summary>
        static public void NyomtatOklevelVerseny( string _VEAZON, Oklevel _Oklevel, int _Limit ) {
            List<OKLEVELVERSENYZO> versenyzok = new List<OKLEVELVERSENYZO>();

            EREDMENYLAPVERSENYTELJES Data = new EREDMENYLAPVERSENYTELJES( _VEAZON );
            string versenydatum = Program.database.Verseny(_VEAZON).Value.Datum;

            foreach( var test in Data.Ijtipusok ) {
                foreach( var korosztaly in test.Korosztalyok ) {
                    if( korosztaly.Egyben == true ) {
                        //var indtest = korosztaly.Indulok.Egyben.Take(_Limit).ToList(); //NOTE(mate): nem kell orderby a EREDMENYLAPVERSENYTELJES megcsinálja
                        List<OKLEVELVERSENYZO> ind = (from indulo in korosztaly.Indulok.Egyben
                                                      select new OKLEVELVERSENYZO{
                                                          Nev = indulo.Nev,
                                                          Egyesulet = indulo.Egyesulet,
                                                          Helyezes = 0,
                                                          Datum = versenydatum
                                                      }).Take(_Limit).ToList();
                        var q = ind.ToArray();

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[i].Helyezes = i + 1;
                        }

                        versenyzok.AddRange( q.ToList( ) );

                    }
                    else {
                        List<OKLEVELVERSENYZO> ind = (from indulo in korosztaly.Indulok.Nok
                                                      select new OKLEVELVERSENYZO{
                                                          Nev = indulo.Nev,
                                                          Egyesulet = indulo.Egyesulet,
                                                          Helyezes = 0,
                                                          Datum = versenydatum
                                                      }).Take(_Limit).ToList();
                        var q = ind.ToArray();

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[i].Helyezes = i + 1;
                        }
                        versenyzok.AddRange( q.ToList( ) );
                        ind = null;
                        ind = ( from indulo in korosztaly.Indulok.Ferfiak
                                select new OKLEVELVERSENYZO {
                                    Nev = indulo.Nev,
                                    Egyesulet = indulo.Egyesulet,
                                    Helyezes = 0,
                                    Datum = versenydatum
                                } ).Take( _Limit ).ToList( );
                        q = ind.ToArray( );

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[i].Helyezes = i + 1;
                        }

                        versenyzok.AddRange( q.ToList( ) );
                    }
                }
            }

            string path = Data.VersenyAdatok.VSAZON != null ? Data.VersenyAdatok.VSAZON + "/" + Data.VersenyAdatok.VEAZON + "/" + "Oklevelek" : Data.VersenyAdatok.VEAZON + "/" + "Oklevelek";

            if( !Directory.Exists( path ) ) {
                Directory.CreateDirectory( path );
            }

            foreach( var oklevelversenyzo in versenyzok ) {
                NyomtatOklevelVersenyVersenyzo( _Oklevel, oklevelversenyzo, path );
            }
        }

        /*
            float UnitToMm = 0.353f;
            float MmToUnit = 1.0f/0.353f;
        */
        public struct seged {
            private static float UnitToMm = 0.353f;
            private static float MmToUnit = 1.0f/0.353f;

            public float X { get; set; }

            public float Y { get; set; }
            public float H { get; set; }
            public string Value { get; set; }

        }

        static void DrawText( ColumnText _ColumnText, string _Text, Font _Font, float _X, float _Y, float _W, float _H ) {
            _ColumnText.SetSimpleColumn( new Phrase( new Chunk( _Text, _Font ) ),
                  _X, _Y,
                  _X + _W, _Y + _H,
                  25,
                  Element.ALIGN_LEFT | Element.ALIGN_BOTTOM );
            _ColumnText.Go( );
        }

        public static string NyomtatOklevelVersenyVersenyzo( Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo, string _Path ) {
            //float Pixel = 3.779528f;


            string filename = _Path + "/" + _Versenyzo.Nev + ".pdf";

            Document document = new Document(PageSize.A4);

            document.SetMargins( 0, 0, 0, 0 );

            PdfWriter pdfWriter = PdfWriter.GetInstance(document,new FileStream(filename,FileMode.Create));

            // 0.353 mm = 1 unit
            // w = 595 . h = 842

            document.Open( );

            PdfContentByte pdfContentByte = pdfWriter.DirectContent;
            ColumnText columnText = new ColumnText(pdfContentByte);

            Font font = FontFactory.GetFont(FontFactory.COURIER, 12, Font.NORMAL);
             float MmToUnit = 1.0f/0.353f;
             float UnitToMm = 0.353f;
        List<seged> Seged = new List<seged>();
            Seged.Add( new seged {
                X = 595.0f - ( _Oklevel.NevX* MmToUnit ),
                Y = 842.0f - ( _Oklevel.NevY * MmToUnit ),
                H = _Oklevel.NevH * MmToUnit,
                Value = _Versenyzo.Nev,
            } );
            Seged.Add( new seged {
                X = 595.0f - ( _Oklevel.EgyesuletX * MmToUnit ),
                Y = 842.0f - ( _Oklevel.EgyesuletY * MmToUnit ),
                H = _Oklevel.EgyesuletH * MmToUnit,
                Value = _Versenyzo.Egyesulet,
            } );
            Seged.Add( new seged {
                X = 595.0f - ( _Oklevel.HelyezesX * MmToUnit ),
                Y = 842.0f - ( _Oklevel.HelyezesY * MmToUnit ),
                H = _Oklevel.HelyezesH * MmToUnit,

                Value = _Versenyzo.Helyezes.ToString( ),
            } );
            Seged.Add( new seged {
                X = 595.0f - ( _Oklevel.DatumX * MmToUnit ),
                Y = 842.0f - ( _Oklevel.DatumY * MmToUnit ),
                H = _Oklevel.DatumH * MmToUnit,

                Value = _Versenyzo.Datum,
            } );

            foreach( var seged in Seged ) {
                DrawText( columnText, seged.Value, font, seged.X, seged.Y, seged.H, 40 );
            }
            document.Close( );
            return filename;
        }

        public static string NyomtatOklevelVersenyVersenyzoDocx( Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo, string _Path ) {
            float Pixel = 3.779528f;

            string filename = _Path + "/" + _Versenyzo.Nev + ".docx";
            var document = DocX.Create(filename);

            /* 
               oldal ||  w(cm)  |  h(cm)  ||  w  |  h   ||
                A4   ||   210   |   297   || 793 | 1122 ||
                A5   ||   148   |   210   || 558 | 793  ||
            */

            // NOTE(mate): A5
            /*
            document.PageHeight = (float)( 210.0f * Pixel );
            document.PageWidth = (float)( 148.0f * Pixel );
            */
            //NOTE(mate): nincs margo
            document.MarginBottom = 1;
            document.MarginLeft = 9;
            document.MarginRight = 9;
            document.MarginTop = 1;

            //NOTE(mate): 
            /*
                ami kell az oklevelbol:
                    -nev
                    -egyesulet
                    -helyezes
                    -datum
            */

            //TODO(mate): mi van ha egy sorban van 2 mező?????



            //TODO(mate): mekkor a sorköz?????



            List<seged> Seged = new List<seged>();
            Seged.Add( new seged {
                X = _Oklevel.NevX * Pixel,
                Y = _Oklevel.NevY * Pixel,
                H = _Oklevel.NevH * Pixel,
                Value = _Versenyzo.Nev,
            } );
            Seged.Add( new seged {
                X = _Oklevel.EgyesuletX * Pixel,
                Y = _Oklevel.EgyesuletY * Pixel,
                H = _Oklevel.EgyesuletH * Pixel,
                Value = _Versenyzo.Egyesulet,
            } );
            Seged.Add( new seged {
                X = _Oklevel.HelyezesX * Pixel,
                Y = _Oklevel.HelyezesY * Pixel,
                H = _Oklevel.HelyezesH * Pixel,
                Value = _Versenyzo.Helyezes.ToString( ),
            } );
            Seged.Add( new seged {
                X = _Oklevel.DatumX * Pixel,
                Y = _Oklevel.DatumY * Pixel,
                H = _Oklevel.DatumH * Pixel,
                Value = _Versenyzo.Datum,
            } );

            Seged = Seged.OrderBy( o => o.Y ).ToList( );

            List<Table> Tables = new List<Table>();
            Table t = null;
            Tables.Add( t );
            Tables.Add( t );
            Tables.Add( t );
            Tables.Add( t );
            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);

            for( int i = 0; i < Tables.Count; i++ ) {
                Tables[i] = document.AddTable( 1, 2 );
                Tables[i].Rows[0].Cells[0].Width = (int)Seged[i].X;
                Tables[i].Rows[0].Cells[1].Width = (int)Seged[i].H;
                if( i == 0 ) {
                    Tables[i].Rows[0].Height = (int)Seged[i].Y;
                }
                else {
                    Tables[i].Rows[0].Height = (int)( Seged[i].Y - Seged[i - 1].Y );
                }

                Tables[i].Rows[0].Cells[1].InsertParagraph( Seged[i].Value );
                Tables[i].Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                Tables[i].Rows[0].Cells[1].Paragraphs[1].Alignment = Alignment.center;
                Tables[i].Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                Tables[i].Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;



                /*   Tables[i].SetBorder( TableBorderType.InsideH, c );
                   Tables[i].SetBorder( TableBorderType.InsideV, c );
                   Tables[i].SetBorder( TableBorderType.Bottom, c );
                   Tables[i].SetBorder( TableBorderType.Top, c );
                   Tables[i].SetBorder( TableBorderType.Left, c );
                   Tables[i].SetBorder( TableBorderType.Right, c );
   */
                document.InsertTable( Tables[i] );
                if( i != Tables.Count - 1 ) {
                    document.InsertParagraph( ).Hide( );
                }
            }

            try { document.Save( ); }
            catch( System.Exception ) { MessageBox.Show( @"A dokumentum meg van nyitva!", @"OKLEVEL.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

            return filename;
        }


    }
}
