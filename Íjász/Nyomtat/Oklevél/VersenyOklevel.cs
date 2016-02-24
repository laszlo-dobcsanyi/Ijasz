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
            
            List<OKLEVELVERSENYZO> versenyzok = new List<OKLEVELVERSENYZO>( );

            EREDMENYLAPVERSENYTELJES Data = new EREDMENYLAPVERSENYTELJES( _VEAZON );
            Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
            Versenysorozat? versenysorozat = Program.database.Versenysorozat(verseny.VersenySorozat);

            if( versenysorozat == null ) {
                versenysorozat = new Versenysorozat();
            }

            string versenydatum = Program.database.Verseny( _VEAZON ).Value.Datum;

            foreach( var test in Data.Ijtipusok ) {
                foreach( var korosztaly in test.Korosztalyok ) {
                    if( korosztaly.Egyben == true ) {
                        //var indtest = korosztaly.Indulok.Egyben.Take(_Limit).ToList(); //NOTE(mate): nem kell orderby a EREDMENYLAPVERSENYTELJES megcsinálja
                        List<OKLEVELVERSENYZO> ind = ( from indulo in korosztaly.Indulok.Egyben
                                                       select new OKLEVELVERSENYZO {
                                                           Verseny = verseny.Megnevezes,
                                                           VersenySorozat = versenysorozat.Value.megnevezés,
                                                           Helyezes = 0,
                                                           Indulo = indulo.Nev,
                                                           Egyesulet = indulo.Egyesulet,
                                                           Ijtipus = test.Megnevezes,
                                                           Korosztaly = korosztaly.Megnevezes,
                                                           InduloNeme = indulo.Nem, 
                                                           Datum = versenydatum
                                                       } ).Take( _Limit ).ToList( );
                        var q = ind.ToArray( );

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[ i ].Helyezes = i + 1;
                        }

                        versenyzok.AddRange( q.ToList( ) );
                    }
                    else {
                        List<OKLEVELVERSENYZO> ind = ( from indulo in korosztaly.Indulok.Nok
                                                       select new OKLEVELVERSENYZO {
                                                           Verseny = verseny.Megnevezes,
                                                           VersenySorozat = versenysorozat.Value.megnevezés,
                                                           Helyezes = 0,
                                                           Indulo = indulo.Nev,
                                                           Egyesulet = indulo.Egyesulet,
                                                           Ijtipus = test.Megnevezes,
                                                           Korosztaly = korosztaly.Megnevezes,
                                                           InduloNeme = indulo.Nem,
                                                           Datum = versenydatum
                                                       } ).Take( _Limit ).ToList( );
                        var q = ind.ToArray( );

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[ i ].Helyezes = i + 1;
                        }
                        versenyzok.AddRange( q.ToList( ) );
                        ind = null;
                        ind = ( from indulo in korosztaly.Indulok.Ferfiak
                                select new OKLEVELVERSENYZO {
                                    Verseny = verseny.Megnevezes,
                                    VersenySorozat = versenysorozat.Value.megnevezés,
                                    Helyezes = 0,
                                    Indulo = indulo.Nev,
                                    Egyesulet = indulo.Egyesulet,
                                    Ijtipus = test.Megnevezes,
                                    Korosztaly = korosztaly.Megnevezes,
                                    InduloNeme = indulo.Nem,
                                    Datum = versenydatum
                                } ).Take( _Limit ).ToList( );
                        q = ind.ToArray( );

                        for( int i = 0; i < q.Count( ); ++i ) {
                            q[ i ].Helyezes = i + 1;
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
            private static float MmToUnit = 1.0f / 0.353f;

            public float X { get; set; }
            public float Y { get; set; }
            public float H { get; set; }
            public float M { get; set; }
            public string I { get; set; }
            public string F { get; set; }
            public string Value { get; set; }

        }

        static void DrawText( ColumnText _ColumnText, string _Text, Font _Font, float _X, float _Y, float _W, float _H , string _I) {
            switch( _I ) {
                case "L":
                    _ColumnText.SetSimpleColumn( new Phrase( new Chunk( _Text, _Font ) ),
                  _X, _Y,
                  _X + _W, _Y + _H,
                  25,
                  Element.ALIGN_LEFT /*| Element.Align_Bottom*/ );
                    _ColumnText.Go( );
                    break;
                case "R":
                    _ColumnText.SetSimpleColumn( new Phrase( new Chunk( _Text, _Font ) ),
                  _X, _Y,
                  _X + _W, _Y + _H,
                  25,
                  Element.ALIGN_RIGHT /*| Element.Align_Bottom*/ );
                    _ColumnText.Go( );
                    break;
                case "M":
                    _ColumnText.SetSimpleColumn( new Phrase( new Chunk( _Text, _Font ) ),
                  _X, _Y,
                  _X + _W, _Y + _H,
                  25,
                  Element.ALIGN_CENTER/*| Element.Align_Bottom*/ );
                    _ColumnText.Go( );
                    break;
                default:
                    _ColumnText.SetSimpleColumn( new Phrase( new Chunk( _Text, _Font ) ),
                  _X, _Y,
                  _X + _W, _Y + _H,
                  25,
                  Element.ALIGN_LEFT /*| Element.Align_Bottom*/ );
                    _ColumnText.Go( );
                    break;
            }
        }

        public static string NyomtatOklevelVersenyVersenyzo( Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo, string _Path ) {
            
            string filename = _Path + "/" + _Versenyzo.Indulo + ".pdf";

            Document document = new Document( PageSize.A4 );
            document.SetMargins( 0, 0, 0, 0 );

            PdfWriter pdfWriter = PdfWriter.GetInstance( document, new FileStream( filename, FileMode.Create ) );

            // 0.353 mm = 1 unit
            // w = 595 . h = 842

            document.Open( );

            PdfContentByte pdfContentByte = pdfWriter.DirectContent;
            ColumnText columnText = new ColumnText( pdfContentByte );

            float MmToUnit = 1.0f / 0.353f;
            float UnitToMm = 0.353f;

            float xOffset = 0;
            float yOffset = 4;

            //NOTE(mate): a (0,0) a lap bal also sarkaban van

            List<seged> Seged = new List<seged>( );

            if( _Oklevel.VersenyX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.VersenyX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.VersenyY * MmToUnit ) + (yOffset * MmToUnit),
                    H = _Oklevel.VersenyH * MmToUnit,
                    M = _Oklevel.VersenyM,
                    I = _Oklevel.VersenyI,
                    F = _Oklevel.VersenyF,
                    Value = _Versenyzo.Verseny,
                } );
            }
            if( _Oklevel.VersenySorozatX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.VersenySorozatX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.VersenySorozatY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.VersenySorozatH * MmToUnit,
                    M = _Oklevel.VersenySorozatM,
                    I = _Oklevel.VersenySorozatI,
                    F = _Oklevel.VersenySorozatF,
                    Value = _Versenyzo.VersenySorozat,
                } );
            }
            if( _Oklevel.HelyezesX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.HelyezesX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.HelyezesY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.HelyezesH * MmToUnit,
                    M = _Oklevel.HelyezesM,
                    I = _Oklevel.HelyezesI,
                    F = _Oklevel.HelyezesF,
                    Value = _Versenyzo.Helyezes.ToString(),
                } );
            }
            if( _Oklevel.InduloX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.InduloX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.InduloY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.InduloH * MmToUnit,
                    M = _Oklevel.InduloM,
                    I = _Oklevel.InduloI,
                    F = _Oklevel.InduloF,
                    Value = _Versenyzo.Indulo,
                } );
            }
            if( _Oklevel.EgyesuletX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.EgyesuletX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.EgyesuletY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.EgyesuletH * MmToUnit,
                    M = _Oklevel.EgyesuletM,
                    I = _Oklevel.EgyesuletI,
                    F = _Oklevel.EgyesuletF,
                    Value = _Versenyzo.Egyesulet,
                } );
            }
            if( _Oklevel.IjtipusX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.IjtipusX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.IjtipusY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.IjtipusH * MmToUnit,
                    M = _Oklevel.IjtipusM,
                    I = _Oklevel.IjtipusI,
                    F = _Oklevel.IjtipusF,
                    Value = _Versenyzo.Ijtipus,
                } );
            }
            if( _Oklevel.KorosztalyX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.KorosztalyX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.KorosztalyY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.KorosztalyH * MmToUnit,
                    M = _Oklevel.KorosztalyM,
                    I = _Oklevel.KorosztalyI,
                    F = _Oklevel.KorosztalyF,
                    Value = _Versenyzo.Korosztaly,
                } );
            }
            if( _Oklevel.InduloNemeX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.InduloNemeX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.InduloNemeY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.InduloNemeH * MmToUnit,
                    M = _Oklevel.InduloNemeM,
                    I = _Oklevel.InduloNemeI,
                    F = _Oklevel.InduloNemeF,
                    Value = _Versenyzo.InduloNeme,
                } );
            }
            if( _Oklevel.DatumX != 0 ) {
                Seged.Add( new seged {
                    X = ( _Oklevel.DatumX * MmToUnit ) + ( xOffset * MmToUnit ),
                    Y = 842.0f - ( _Oklevel.DatumY * MmToUnit ) + ( yOffset * MmToUnit ),
                    H = _Oklevel.DatumH * MmToUnit,
                    M = _Oklevel.DatumM,
                    I = _Oklevel.DatumI,
                    F = _Oklevel.DatumF,
                    Value = _Versenyzo.Datum,
                } );
            }

            foreach( var item in Seged ) {
                Font font;
                switch( item.F ) {
                    case "B":
                        font = FontFactory.GetFont( FontFactory.COURIER, item.M, Font.BOLD );
                        break;
                    case "I":
                        font = FontFactory.GetFont( FontFactory.COURIER, item.M, Font.ITALIC );
                        break;
                    case "2":
                        font = FontFactory.GetFont( FontFactory.COURIER, item.M, Font.BOLDITALIC );
                        break;
                    case "0":
                        font = FontFactory.GetFont( FontFactory.COURIER, item.M, Font.NORMAL );
                        break;
                    default:
                        font = FontFactory.GetFont( FontFactory.COURIER, item.M, Font.NORMAL );
                        break;
                }
                DrawText( columnText,
                    item.Value, 
                    font,
                    item.X, item.Y, item.H, 
                    40,
                    item.I);

            }
            document.Close( );
            return filename;
        }
    }
}
