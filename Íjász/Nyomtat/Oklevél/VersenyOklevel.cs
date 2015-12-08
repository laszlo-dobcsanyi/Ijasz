using Novacode;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Íjász {
    public partial class Nyomtat {

        /// <summary>kiszedi az első _Limit eredményt, meghívja rájuk a NyomtatOklevelVersenyVersenyzo fgv-t </summary>
        static public void NyomtatOklevelVerseny( string _VEAZON, Oklevel _Oklevel, int _Limit ) {
            List<OKLEVELVERSENYZO> versenyzok = new List<OKLEVELVERSENYZO>();
            //NOTE(mate): mi van ha nincs elég versenyző??? nem cink :D
            List<Eredmény> eredmenyek = (Program.database.Eredmények(_VEAZON).OrderBy(eredmeny => eredmeny.Osszpont).Take(_Limit)).ToList();
            var indulok = Program.database.Indulók();
            string versenydatum = Program.database.Verseny(_VEAZON).Value.Datum;
            List<OKLEVELVERSENYZO> eredmenyek2 = (from indulo in indulok
                                                  join eredmeny in eredmenyek on indulo.Nev equals eredmeny.Nev
                                                  select new OKLEVELVERSENYZO
                                                  {
                                                      Nev = indulo.Nev,
                                                      Egyesulet = indulo.Egyesulet,
                                                      Helyezes = 0,
                                                      Datum = versenydatum
                                                  }).ToList();

            var q = eredmenyek2.ToArray();

            for( int i = 0; i < q.Count( ); ++i ) {
                q[i].Helyezes = i + 1;
                NyomtatOklevelVersenyVersenyzo( _Oklevel, q[i] );
            }
        }

        public struct seged {
            public double X;
            public double Y;
            public double H;
            public string Value;

        }

        static public string NyomtatOklevelVersenyVersenyzo( Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo ) {
            //TODO(mate): egyedi filename? egyesével lehessen nyomtatni....?????
            //NOTE(mate): _oklevelből: 4 * (x,y)
            //NOTE(mate): az oklevel sablont le kell sortolni y szerint!
            double Pixel = 3.779528f;

            string filename = null;
            var document = DocX.Create("oklevel_" + _Versenyzo.Nev + ".docx");

            /* 
               oldal ||  w(cm)  |  h(cm)  ||  w  |  h   ||
                A4   ||   210   |   297   || 793 | 1122 ||
                A5   ||   148   |   210   || 558 | 793  ||
            */

            //NOTE(mate): A5
            document.PageHeight = (float)( 210.0f * Pixel );
            document.PageWidth = (float)( 148.0f * Pixel );
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

            //NOTE(mate): ver 0.2

            List<Table> Tables = new List<Table>();
            Table t = null;// = document.AddTable(1,2);
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

                Tables[i].SetBorder( TableBorderType.InsideH, c );
                Tables[i].SetBorder( TableBorderType.InsideV, c );
                Tables[i].SetBorder( TableBorderType.Bottom, c );
                Tables[i].SetBorder( TableBorderType.Top, c );
                Tables[i].SetBorder( TableBorderType.Left, c );
                Tables[i].SetBorder( TableBorderType.Right, c );

                document.InsertTable( Tables[i] );
                if( i != Tables.Count - 1 ) {
                    document.InsertParagraph( ).Hide( );
                }
            }

            #region ver 0.1
            //NOTE(mate): ver 0.1 
            /*
                        Table table1 = document.AddTable(1, 2);
                        table1.Rows[0].Height = (int)Seged[0].Y;
                        table1.Rows[0].Cells[0].Width = (int)Seged[0].X;
                        table1.Rows[0].Cells[1].Width = (int)Seged[0].H;
                        table1.Rows[0].Cells[1].InsertParagraph( Seged[0].Value );
                        table1.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                        table1.Rows[0].Cells[1].Paragraphs[1].Alignment = Alignment.center;
                        table1.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                        table1.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

                        Table table2 = document.AddTable(1, 2);
                        table2.Rows[0].Height = (int)( Seged[1].Y - Seged[0].Y );
                        table2.Rows[0].Cells[0].Width = (int)Seged[1].X;
                        table2.Rows[0].Cells[1].Width = (int)Seged[1].H;
                        table2.Rows[0].Cells[1].InsertParagraph( Seged[1].Value );
                        table2.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                        table2.Rows[0].Cells[1].Paragraphs[1].Alignment = Alignment.center;
                        table2.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                        table2.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;

                        Table table3 = document.AddTable(1, 2);
                        table3.Rows[0].Height = (int)( Seged[2].Y - Seged[1].Y );
                        table3.Rows[0].Cells[0].Width = (int)Seged[2].X;
                        table3.Rows[0].Cells[1].Width = (int)Seged[2].H;
                        table3.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                        table3.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
                        table3.Rows[0].Cells[1].InsertParagraph( Seged[2].Value );
                        table3.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                        table3.Rows[0].Cells[1].Paragraphs[1].Alignment = Alignment.center;

                        Table table4 = document.AddTable(1, 2);
                        table4.Rows[0].Height = (int)( Seged[3].Y - Seged[2].Y);
                        table4.Rows[0].Cells[0].Width = (int)Seged[3].X;
                        table4.Rows[0].Cells[1].Width = (int)Seged[3].H;
                        table4.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                        table4.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
                        table4.Rows[0].Cells[1].InsertParagraph( Seged[3].Value );
                        table4.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
                        table4.Rows[0].Cells[1].Paragraphs[1].Alignment = Alignment.center;

                        //Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);

                        table1.SetBorder( TableBorderType.InsideH, c );
                        table1.SetBorder( TableBorderType.InsideV, c );
                        table1.SetBorder( TableBorderType.Bottom, c );
                        table1.SetBorder( TableBorderType.Top, c );
                        table1.SetBorder( TableBorderType.Left, c );
                        table1.SetBorder( TableBorderType.Right, c );

                        table2.SetBorder( TableBorderType.InsideH, c );
                        table2.SetBorder( TableBorderType.InsideV, c );
                        table2.SetBorder( TableBorderType.Bottom, c );
                        table2.SetBorder( TableBorderType.Top, c );
                        table2.SetBorder( TableBorderType.Left, c );
                        table2.SetBorder( TableBorderType.Right, c );

                        table3.SetBorder( TableBorderType.InsideH, c );
                        table3.SetBorder( TableBorderType.InsideV, c );
                        table3.SetBorder( TableBorderType.Bottom, c );
                        table3.SetBorder( TableBorderType.Top, c );
                        table3.SetBorder( TableBorderType.Left, c );
                        table3.SetBorder( TableBorderType.Right, c );

                        table4.SetBorder( TableBorderType.InsideH, c );
                        table4.SetBorder( TableBorderType.InsideV, c );
                        table4.SetBorder( TableBorderType.Bottom, c );
                        table4.SetBorder( TableBorderType.Top, c );
                        table4.SetBorder( TableBorderType.Left, c );
                        table4.SetBorder( TableBorderType.Right, c );

                        document.InsertTable( table1 );
                        document.InsertParagraph( ).Hide( );
                        document.InsertTable( table2 );
                        document.InsertParagraph( ).Hide( );
                        document.InsertTable( table3 );
                        document.InsertParagraph( ).Hide( );
                        document.InsertTable( table4 );
            */
            #endregion

            try { document.Save( ); }
            catch( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "OKLEVEL.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

            return filename;
        }
    }
}
