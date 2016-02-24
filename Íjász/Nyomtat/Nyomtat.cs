using Novacode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Íjász {
    public static partial class Nyomtat {
        public static Form dialog;
        public static string DialogFileName;

        public struct CSAPATLISTA {
            public struct VERSENYADATOK {
                public string   VEAZON;
                public string   VEMEGN;
                public string   VEDATU;
                public int      VEOSPO;
                public int      VEINSZ;
                public string   VSAZON;
                public string   VSMEGN;

                public VERSENYADATOK( string _VEAZON ) {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                    List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);

                    foreach( Versenysorozat item in versenysorozatok ) {
                        if( item.azonosító == verseny.VersenySorozat ) {
                            VSAZON = item.azonosító;
                            VSMEGN = item.megnevezés;
                            break;
                        }
                    }
                    VEAZON = verseny.Azonosito;
                    VEMEGN = verseny.Megnevezes;
                    VEDATU = verseny.Datum;
                    VEOSPO = verseny.Osszes;
                    VEINSZ = eredmenyek.Count( eredmeny => eredmeny.Megjelent.Equals( true ) );
                }


            }

            public struct CSAPAT {
                public int INCSSZ;
                public List<VERSENYZOADAT> versenyzoadatok;

                public CSAPAT( int _INCSSZ ) {
                    INCSSZ = _INCSSZ;
                    versenyzoadatok = new List<VERSENYZOADAT>( );
                }
            }

            public struct VERSENYZOADAT {
                public string INNEVE;
                public int INSOSZ;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;
                public string ITMEGN;

                public VERSENYZOADAT( string _INNEVE, int _INSOSZ, int _INSZUL, string _INEGYE, int _INCSSZ, string _ITMEGN ) {
                    ITMEGN = _ITMEGN;
                    INNEVE = _INNEVE;
                    INSOSZ = _INSOSZ;
                    INSZUL = _INSZUL;
                    INEGYE = _INEGYE;
                    INCSSZ = _INCSSZ;
                }

                public class RendezCsapatSzam : IComparer<VERSENYZOADAT> {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 ) {
                        return v1.INCSSZ.CompareTo( v2.INCSSZ );
                    }
                }

                public class RendezNev : IComparer<VERSENYZOADAT> {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 ) {
                        if( v1.INCSSZ.CompareTo( v2.INCSSZ ) == 0 ) {
                            return v1.INNEVE.CompareTo( v2.INNEVE );
                        }
                        else {
                            return 0;
                        }

                    }
                }
            }

            public List<CSAPAT> CsapatokLista( string _VEAZON ) {
                List<CSAPAT> list_csapatok = Program.database.CsapatLista(_VEAZON);
                CSAPAT[] csapatok = list_csapatok.ToArray();

                for( int i = 0; i < csapatok.Length; i++ ) {
                    csapatok[i].versenyzoadatok = VersenyzoAdatok( _VEAZON, csapatok[i].INCSSZ );
                    var test = VersenyzoAdatok( _VEAZON, csapatok[i].INCSSZ );

                    var firstnotsec =  csapatok[i].versenyzoadatok.Except(test);
                    var secondnotfirst = test.Except( csapatok[i].versenyzoadatok );

                    if( firstnotsec.Count( ) != 0 || secondnotfirst.Count( ) != 0 ) {
                        Console.WriteLine( );
                    }

                }

                list_csapatok = new List<CSAPAT>( csapatok );
                return list_csapatok;
            }

            public List<VERSENYZOADAT> VersenyzoAdatok( string _VEAZON, int _Csapat ) {
                List<VERSENYZOADAT> Data = new List<VERSENYZOADAT>();

                List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                List<Induló> indulok = Program.database.Indulók();
                List<Íjtípus> ijtipusok = Program.database.Íjtípusok();
                foreach( Eredmény eredmeny in eredmenyek ) {
                    foreach( Induló indulo in indulok ) {
                        if( eredmeny.Nev == indulo.Nev ) {
                            if( eredmeny.Megjelent == true && eredmeny.Csapat == _Csapat ) {
                                foreach( Íjtípus ijtipus in ijtipusok ) {
                                    if( ijtipus.Azonosito == eredmeny.Ijtipus ) {
                                        Data.Add( new VERSENYZOADAT( eredmeny.Nev,
                                                                                (int)eredmeny.Sorszam,
                                                                                Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                                                                                indulo.Egyesulet,
                                                                                eredmeny.Csapat,
                                                                                ijtipus.Megnevezes ) );
                                    }
                                }
                            }
                        }
                    }
                }

                IComparer<VERSENYZOADAT> Comparer1 = new CSAPATLISTA.VERSENYZOADAT.RendezCsapatSzam();
                Data.Sort( Comparer1 );
                IComparer<VERSENYZOADAT> Comparer2 = new CSAPATLISTA.VERSENYZOADAT.RendezNev();
                Data.Sort( Comparer2 );

                return Data;
            }

            public List<VERSENYZOADAT> VersenyzoAdatokLINQ( string _VEAZON, int _Csapat ) {
                List<VERSENYZOADAT> Data = new List<VERSENYZOADAT>();

                List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                List<Induló> indulok = Program.database.Indulók();
                List<Íjtípus> ijtipusok = Program.database.Íjtípusok();

                Data = ( ( from eredmeny in eredmenyek
                           join indulo in indulok
                           on eredmeny.Nev equals indulo.Nev
                           join ijtipus in ijtipusok
                           on eredmeny.Ijtipus equals ijtipus.Azonosito
                           where eredmeny.Megjelent.Equals( true ) && eredmeny.Csapat.Equals( _Csapat )
                           select new VERSENYZOADAT {
                               INCSSZ = eredmeny.Csapat,
                               INEGYE = indulo.Egyesulet,
                               INNEVE = indulo.Nev,
                               INSOSZ = (int)eredmeny.Sorszam,
                               INSZUL = Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                               ITMEGN = ijtipus.Megnevezes
                           } ).OrderBy( q => q.INCSSZ ).ThenBy( z => z.INNEVE ) ).ToList( );

                return Data;
            }

            public CSAPATLISTA( string _VEAZON )
                : this( ) {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                Csapatok = CsapatokLista( _VEAZON );
            }

            public VERSENYADATOK VersenyAdatok;
            public List<CSAPAT> Csapatok;
        };

        public struct NEVEZESILISTA {
            public struct VERSENYADATOK {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;

                public
                VERSENYADATOK( string _VEAZON, bool _NemMegjelentNyomtat ) {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                    List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                    int MegjelentIndulok = 0;
                    foreach( Eredmény item in eredmenyek ) {
                        if( _NemMegjelentNyomtat == true ) {
                            if( item.Megjelent == false ) {
                                MegjelentIndulok++;
                            }

                        }
                        else if( _NemMegjelentNyomtat == false ) {
                            if( item.Megjelent == true ) {
                                MegjelentIndulok++;
                            }
                        }
                    }

                    foreach( Versenysorozat item in versenysorozatok ) {
                        if( item.azonosító == verseny.VersenySorozat ) {
                            VSAZON = item.azonosító;
                            VSMEGN = item.megnevezés;
                            break;
                        }
                    }
                    VEAZON = verseny.Azonosito;
                    VEMEGN = verseny.Megnevezes;
                    VEDATU = verseny.Datum;
                    VEOSPO = verseny.Osszes;
                    VEINSZ = MegjelentIndulok;
                }
            }

            public struct VERSENYZOADAT {
                public string INNEVE;
                public int INSOSZ;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;
                public string ITMEGN;

                public VERSENYZOADAT( string _INNEVE, int _INSOSZ, int _INSZUL, string _INEGYE, int _INCSSZ, string _ITMEGN ) {
                    INNEVE = _INNEVE;
                    ITMEGN = _ITMEGN;
                    INSOSZ = _INSOSZ;
                    INSZUL = _INSZUL;
                    INEGYE = _INEGYE;
                    INCSSZ = _INCSSZ;
                }

                public class RendezNev : IComparer<VERSENYZOADAT> {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 ) {
                        return v1.INNEVE.CompareTo( v2.INNEVE );
                    }
                }
            }

            public List<VERSENYZOADAT>
            Versenyzodatok( string _VEAZON, bool _NemMegjelentNyomtat ) {
                List<VERSENYZOADAT> Data = new List<VERSENYZOADAT>();

                List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                List<Induló> indulok = Program.database.Indulók();
                List<Íjtípus> ijtipusok = Program.database.Íjtípusok();
                foreach( Eredmény eredmeny in eredmenyek ) {
                    foreach( Induló indulo in indulok ) {
                        if( eredmeny.Nev == indulo.Nev ) {
                            if( _NemMegjelentNyomtat == true ) {
                                if( eredmeny.Megjelent == false ) {
                                    foreach( Íjtípus ijtipus in ijtipusok ) {
                                        if( ijtipus.Azonosito == eredmeny.Ijtipus ) {
                                            Data.Add( new VERSENYZOADAT( eredmeny.Nev,
                                                                        (int)eredmeny.Sorszam,
                                                                        Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                                                                        indulo.Egyesulet,
                                                                        eredmeny.Csapat,
                                                                        ijtipus.Megnevezes ) );
                                        }
                                    }
                                }
                            }
                            else if( _NemMegjelentNyomtat == false ) {
                                if( eredmeny.Megjelent == true ) {
                                    foreach( Íjtípus ijtipus in ijtipusok ) {
                                        if( ijtipus.Azonosito == eredmeny.Ijtipus ) {
                                            Data.Add( new VERSENYZOADAT( eredmeny.Nev,
                                                                        (int)eredmeny.Sorszam,
                                                                        Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                                                                        indulo.Egyesulet,
                                                                        eredmeny.Csapat,
                                                                        ijtipus.Megnevezes ) );
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                IComparer<VERSENYZOADAT> Comparer2 = new NEVEZESILISTA.VERSENYZOADAT.RendezNev();
                Data.Sort( Comparer2 );

                return Data;
            }

            public NEVEZESILISTA( string _VEAZON, bool _NemMegjelentNyomtat )
                : this( ) {
                VersenyAdatok = new VERSENYADATOK( _VEAZON, _NemMegjelentNyomtat );
                VersenyzoAdatok = Versenyzodatok( _VEAZON, _NemMegjelentNyomtat );
            }

            public VERSENYADATOK VersenyAdatok;
            public List<VERSENYZOADAT> VersenyzoAdatok;
        };

        public struct BEIROLAP {
            public struct VERSENYADATOK {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public string VSAZON;
                public string VSMEGN;
                public int VEALSZ;

                public
                VERSENYADATOK( string _VEAZON ) {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                    List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                    int MegjelentIndulok = 0;
                    foreach( Eredmény item in eredmenyek ) { if( item.Megjelent == true ) { MegjelentIndulok++; } }

                    foreach( Versenysorozat item in versenysorozatok ) {
                        if( item.azonosító == verseny.VersenySorozat ) {
                            VSAZON = item.azonosító;
                            VSMEGN = item.megnevezés;
                            break;
                        }
                    }
                    VEAZON = verseny.Azonosito;
                    VEMEGN = verseny.Megnevezes;
                    VEDATU = verseny.Datum;
                    VEOSPO = verseny.Osszes;
                    VEALSZ = verseny.Allomasok;
                }
            }

            public struct VERSENYZOADATOK {
                public string INNEVE;
                public string INBEEK;
                public string INNEME;
                public string INEGYE;
                public string INVEEN;

                public int INSOSZ;
                public int INCSSZ;
                public string ITMEGN;
                public string KOMEGN;

                public VERSENYZOADATOK( string _VEAZON, Eredmény _eredmeny ) {
                    Induló? indulo = Program.database.Induló(_eredmeny.Nev);
                    List<Íjtípus> ijtipusok = Program.database.Íjtípusok();

                    ITMEGN = null;
                    INSOSZ = (int)_eredmeny.Sorszam;
                    INCSSZ = _eredmeny.Csapat;
                    INNEVE = _eredmeny.Nev;
                    INBEEK = Program.database.InduloKora( _VEAZON, _eredmeny.Nev ).ToString( );
                    INNEME = indulo.Value.Nem;
                    INEGYE = indulo.Value.Egyesulet;
                    INVEEN = indulo.Value.Engedely;
                    KOMEGN = _eredmeny.KorosztalyAzonosito;

                    foreach( Íjtípus ijtipus in ijtipusok ) {
                        if( ijtipus.Azonosito == _eredmeny.Ijtipus ) {
                            ITMEGN = ijtipus.Megnevezes;
                            break;
                        }
                    }
                }

            }

            public BEIROLAP( string _VEAZON, Eredmény _eredmeny ) : this( ) {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                VersenyzoAdatok = new VERSENYZOADATOK( _VEAZON, _eredmeny );
            }
            public VERSENYADATOK VersenyAdatok;
            public VERSENYZOADATOK VersenyzoAdatok;
        }

        public struct EREDMENYLAPVERSENYTELJES {
            public struct VERSENYADATOK {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;

                public
                VERSENYADATOK( string _VEAZON ) {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                    List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                    int MegjelentIndulok = 0;
                    foreach( Eredmény item in eredmenyek ) { if( item.Megjelent == true ) { MegjelentIndulok++; } }

                    foreach( Versenysorozat item in versenysorozatok ) {
                        if( item.azonosító == verseny.VersenySorozat ) {
                            VSAZON = item.azonosító;
                            VSMEGN = item.megnevezés;
                            break;
                        }
                    }
                    VEAZON = verseny.Azonosito;
                    VEMEGN = verseny.Megnevezes;
                    VEDATU = verseny.Datum;
                    VEOSPO = verseny.Osszes;
                    VEINSZ = MegjelentIndulok;
                }
            }

            public struct IJTIPUS {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VEAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Korosztály> korosztalyok = Program.database.Korosztályok(verseny.Azonosito);

                    foreach( Korosztály korosztaly in korosztalyok ) {
                        Korosztalyok.Add( new KOROSZTALY( _Azonosito, korosztaly.Azonosito, korosztaly.Megnevezes, korosztaly.Egyben, _OSSZESINDULO ) );
                    }

                }

            }

            public struct KOROSZTALY {
                public string Azonosito;
                public string Megnevezes;
                public bool Egyben;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus, string _Azonosito, string _Megnevezes, bool _Egyben, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Egyben = _Egyben;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                    Indulok.Nok = Nok( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                    Indulok.Egyben = Egybe( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                }
            }

            public struct INDULOK {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak;
                public List<INDULO> Egyben;
            }

            public struct INDULO {
                public string Ijtipus;
                public string Korosztaly;
                public string Nem;
                public string Nev;
                public int Sorszam;
                public string Egyesulet;
                public int OsszPont;
                public int Szazalek;
                public string Verseny;

                public INDULO( string _Ijtipus,
                                string _Korosztaly,
                                string _Nem,
                                string _Nev,
                                int _Sorszam,
                                string _Egyesulet,
                                int _OsszPont,
                                int _Szazalek,
                                string _Verseny ) {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    Nem = _Nem;
                    Nev = _Nev;
                    Sorszam = _Sorszam;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Szazalek = _Szazalek;
                    Verseny = _Verseny;
                }
            }

            public List<IJTIPUS>
            ijtipusok( string _VEAZON, List<INDULO> _Indulok ) {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for( int i = IjtipusokSeged.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    for( int j = 0; j < _Indulok.Count; j++ ) {
                        if( IjtipusokSeged[i].Azonosito == _Indulok[j].Ijtipus ) {
                            found = true;
                        }
                    }
                    if( !found ) { IjtipusokSeged.RemoveAt( i ); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach( Íjtípus ijtipus in IjtipusokSeged ) {
                    IJTIPUS temp = new IJTIPUS(_VEAZON, ijtipus.Azonosito, ijtipus.Megnevezes, _Indulok);
                    Data.Add( temp );
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F" ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }

                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N" ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Egybe( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public List<INDULO>
            Osszesindulo( string _VEAZON ) {
                List<INDULO> Data = new List<INDULO>();

                List<Induló> indulók = Program.database.Indulók();
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
                Verseny verseny = Program.database.Verseny(_VEAZON).Value;

                foreach( Eredmény eredmeny in eredmények ) {
                    foreach( Induló indulo in indulók ) {
                        if( eredmeny.Nev == indulo.Nev && eredmeny.Megjelent == true ) {
                            INDULO temp = new INDULO(eredmeny.Ijtipus,
                                                               eredmeny.KorosztalyAzonosito,
                                                               indulo.Nem,
                                                               indulo.Nev,
                                                               (int)eredmeny.Sorszam,
                                                               indulo.Egyesulet,
                                                               eredmeny.Osszpont.Value,
                                                               eredmeny.Szazalek.Value,
                                                               verseny.Azonosito);
                            Data.Add( temp );
                        }
                    }
                }
                return Data;
            }

            public VERSENYADATOK VersenyAdatok;
            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> Ijtipusok;

            public class Order : IComparer<INDULO> {
                public int Compare( INDULO i1, INDULO i2 ) {
                    int z = i1.OsszPont.CompareTo(i2.OsszPont);
                    return ( -1 * z );
                }
            }

            public EREDMENYLAPVERSENYTELJES( string _VEAZON )
                : this( ) {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                OsszesIndulo = Osszesindulo( _VEAZON );
                Ijtipusok = ijtipusok( _VEAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order();

                foreach( IJTIPUS ijtipus in Ijtipusok ) {
                    foreach( KOROSZTALY korosztaly in ijtipus.Korosztalyok ) {
                        korosztaly.Indulok.Ferfiak.Sort( Comparer );
                        korosztaly.Indulok.Nok.Sort( Comparer );
                        korosztaly.Indulok.Egyben.Sort( Comparer );
                    }
                }
            }
        }

        public struct EREDMENYLAPVERSENYMISZ {
            public struct VERSENYADATOK {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;

                public
                VERSENYADATOK( string _VEAZON ) {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();
                    List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                    List<Induló> indulok = Program.database.Indulók();
                    int MegjelentIndulok = 0;
                    foreach( Eredmény item in eredmenyek ) {
                        foreach( Induló indulo in indulok ) {
                            if( item.Megjelent == true && item.Nev == indulo.Nev && indulo.Engedely != null ) {
                                MegjelentIndulok++;
                            }
                        }
                    }

                    foreach( Versenysorozat item in versenysorozatok ) {
                        if( item.azonosító == verseny.VersenySorozat ) {
                            VSAZON = item.azonosító;
                            VSMEGN = item.megnevezés;
                            break;
                        }
                    }
                    VEAZON = verseny.Azonosito;
                    VEMEGN = verseny.Megnevezes;
                    VEDATU = verseny.Datum;
                    VEOSPO = verseny.Osszes;
                    VEINSZ = MegjelentIndulok;
                }
            }

            public struct IJTIPUS {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VEAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    List<Korosztály> korosztalyok = Program.database.Korosztályok(verseny.Azonosito);

                    foreach( Korosztály korosztaly in korosztalyok ) {
                        Korosztalyok.Add( new KOROSZTALY( _Azonosito, korosztaly.Azonosito, korosztaly.Megnevezes, korosztaly.Egyben, _OSSZESINDULO ) );
                    }

                }

            }

            public struct KOROSZTALY {
                public string Azonosito;
                public string Megnevezes;
                public bool Egyben;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus, string _Azonosito, string _Megnevezes, bool _Egyben, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Egyben = _Egyben;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                    Indulok.Nok = Nok( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                    Indulok.Egyben = Egybe( _Ijtipus, _Azonosito, _Egyben, _OSSZESINDULO );
                }
            }

            public struct INDULOK {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak;
                public List<INDULO> Egyben;
            }

            public struct INDULO {
                public string Ijtipus;
                public string Korosztaly;
                public string Nem;
                public string Nev;
                public int Sorszam;
                public string Egyesulet;
                public int OsszPont;
                public int Szazalek;
                public string Verseny;

                public INDULO( string _Ijtipus,
                                string _Korosztaly,
                                string _Nem,
                                string _Nev,
                                int _Sorszam,
                                string _Egyesulet,
                                int _OsszPont,
                                int _Szazalek,
                                string _Verseny ) {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    Nem = _Nem;
                    Nev = _Nev;
                    Sorszam = _Sorszam;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Szazalek = _Szazalek;
                    Verseny = _Verseny;
                }
            }

            public List<IJTIPUS>
            ijtipusok( string _VEAZON, List<INDULO> _Indulok ) {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for( int i = IjtipusokSeged.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    for( int j = 0; j < _Indulok.Count; j++ ) {
                        if( IjtipusokSeged[i].Azonosito == _Indulok[j].Ijtipus ) {
                            found = true;
                        }
                    }
                    if( !found ) { IjtipusokSeged.RemoveAt( i ); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach( Íjtípus ijtipus in IjtipusokSeged ) {
                    IJTIPUS temp = new IJTIPUS(_VEAZON, ijtipus.Azonosito, ijtipus.Megnevezes, _Indulok);
                    Data.Add( temp );
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F" ) {
                        for( int j = 0; j < Data.Count; j++ ) {
                            if( Data[j].Nev == _Indulok[i].Nev ) {
                                //hozzáadni az eredmenyt
                                found = true;
                            }
                        }
                        if( !found ) {
                            Data.Add( _Indulok[i] );
                            _Indulok.RemoveAt( i );
                        }

                    }
                }

                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N" ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Egybe( string _Ijtipus, string _Korosztaly, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public List<INDULO>
            Osszesindulo( string _VEAZON ) {
                List<INDULO> Data = new List<INDULO>();

                List<Induló> indulók = Program.database.Indulók();
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
                Verseny verseny = Program.database.Verseny(_VEAZON).Value;

                foreach( Eredmény eredmeny in eredmények ) {
                    foreach( Induló indulo in indulók ) {
                        if( eredmeny.Nev == indulo.Nev && eredmeny.Megjelent == true && indulo.Engedely != null ) {

                            if( indulo.Nem == "F" ) {

                                INDULO temp = new INDULO(eredmeny.Ijtipus,
                                                                    eredmeny.KorosztalyAzonosito,
                                                                    indulo.Nem,
                                                                    indulo.Nev,
                                                                    (int)eredmeny.Sorszam,
                                                                    indulo.Egyesulet,
                                                                    eredmeny.Osszpont.Value,
                                                                    eredmeny.Szazalek.Value,
                                                                    verseny.Azonosito);
                                Data.Add( temp );
                            }
                            else if( indulo.Nem == "N" && eredmeny.Megjelent == true && indulo.Engedely != null ) {

                                INDULO temp = new INDULO(eredmeny.Ijtipus,
                                                                    eredmeny.KorosztalyAzonosito,
                                                                    indulo.Nem,
                                                                    indulo.Nev,
                                                                    (int)eredmeny.Sorszam,
                                                                    indulo.Egyesulet,
                                                                    eredmeny.Osszpont.Value,
                                                                    eredmeny.Szazalek.Value,
                                                                    verseny.Azonosito);
                                Data.Add( temp );
                            }
                        }
                    }
                }
                return Data;
            }

            public VERSENYADATOK VersenyAdatok;
            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> Ijtipusok;

            public class Order : IComparer<INDULO> {
                public int Compare( INDULO i1, INDULO i2 ) {
                    int z = i1.OsszPont.CompareTo(i2.OsszPont);
                    return ( -1 * z );
                }
            }

            public EREDMENYLAPVERSENYMISZ( string _VEAZON )
                : this( ) {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                OsszesIndulo = Osszesindulo( _VEAZON );
                Ijtipusok = ijtipusok( _VEAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order();

                foreach( IJTIPUS ijtipus in Ijtipusok ) {
                    foreach( KOROSZTALY korosztaly in ijtipus.Korosztalyok ) {
                        korosztaly.Indulok.Ferfiak.Sort( Comparer );
                        korosztaly.Indulok.Nok.Sort( Comparer );
                    }
                }
            }
        }

        public struct EGYESULETADAT {
            public int Helyezes;
            public string Nev;
            public string Cim;
            public int OsszPont;

            public EGYESULETADAT( int _Helyezes, string _Nev, string _Cim, int _OsszPont ) {
                Helyezes = _Helyezes;
                Nev = _Nev;
                Cim = _Cim;
                OsszPont = _OsszPont;
            }
        }

        public struct EREDMENYLAPVERSENYEGYESULET {
            public struct VERSENYADATOK {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public string VSAZON;
                public string VSMEGN;

                public VERSENYADATOK( string _VEAZON ) : this( ) {
                    List<Verseny> versenyek = Program.database.Versenyek();
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

                    foreach( Verseny verseny in versenyek ) {
                        if( verseny.Azonosito == _VEAZON ) {
                            VEAZON = verseny.Azonosito;
                            VEMEGN = verseny.Megnevezes;
                            VEDATU = verseny.Datum;
                            VEOSPO = verseny.Osszes;
                            VSAZON = verseny.VersenySorozat;
                        }
                    }
                    foreach( Versenysorozat versenysorozat in versenysorozatok ) {
                        if( versenysorozat.azonosító == VSAZON )
                            VSMEGN = versenysorozat.megnevezés;
                    }
                }
            }

            public List<EGYESULETADAT> Egyesuletek;
            public VERSENYADATOK VersenyAdatok;

            public EREDMENYLAPVERSENYEGYESULET( string _VEAZON ) {
                Egyesuletek = Program.database.EredmenylapVersenyEgyesulet( _VEAZON );
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
            }
        }

        public struct EREDMENYLAPVERSENYSOROZATEGYESULET {
            public string Azonosito;
            public string Megnevezes;
            public List<EGYESULETADAT> Egyesuletek;

            public EREDMENYLAPVERSENYSOROZATEGYESULET( string _VSAZON ) : this( ) {
                Versenysorozat tmpvs = Program.database.Versenysorozat(_VSAZON).Value;
                Azonosito = _VSAZON;
                Megnevezes = tmpvs.megnevezés;

                Egyesuletek = new List<EGYESULETADAT>( );
                List<Egyesulet> egyes = Program.database.Egyesuletek();
                foreach( Egyesulet item in egyes ) {
                    if( item.Listazando == true )
                        Egyesuletek.Add( new EGYESULETADAT( 0, item.Azonosito, item.Cim, 0 ) );
                }

                List<List<EGYESULETADAT>> temp = new List<List<EGYESULETADAT>>();

                List<Verseny> versenyek = Program.database.Versenyek();
                for( int i = versenyek.Count - 1; i >= 0; i-- ) {
                    if( versenyek[i].VersenySorozat == _VSAZON ) {
                        temp.Add( Program.database.EredmenylapVersenyEgyesulet( versenyek[i].Azonosito ) );
                    }
                }
                EGYESULETADAT[] Egyesuletek2 = Egyesuletek.ToArray();

                foreach( List<EGYESULETADAT> item in temp ) {
                    foreach( EGYESULETADAT item2 in item ) {
                        for( int i = 0; i < Egyesuletek2.Length; i++ ) {
                            if( item2.Nev == Egyesuletek2[i].Nev ) {
                                Egyesuletek2[i].OsszPont += item2.OsszPont;
                            }
                        }
                    }
                }
                Egyesuletek = new List<EGYESULETADAT>( Egyesuletek2 );

                for( int i = 0; i < Egyesuletek.Count; i++ ) {
                    for( int j = 0; j < Egyesuletek.Count; j++ ) {
                        if( Egyesuletek[i].OsszPont > Egyesuletek[j].OsszPont ) {
                            EGYESULETADAT temp3 = Egyesuletek[i];
                            Egyesuletek[i] = Egyesuletek[j];
                            Egyesuletek[j] = temp3;
                        }
                    }
                }
            }
        }

        public struct EREDMENYLAPVERSENYSOROZATRESZLETES {
            public string Azonosito;
            public string Megnevezes;
            public int VersenyekSzama;
            public List<string> VersenyAzonositok;

            public struct INDULO {
                public string Ijtipus;
                public string Korosztaly;
                public bool KorosztalyEgyben;
                public string Nem;
                public string Nev;
                public string Egyesulet;
                public List<EREDMENY> Eredmenyek;
                public int OsszPont;
                public string Verseny;

                public INDULO( string _Ijtipus,
                                string _Korosztaly,
                                bool _KorosztalyEgyben,
                                string _Nem,
                                string _Nev,
                                string _Egyesulet,
                                int _OsszPont,
                                List<EREDMENY> _Eredmenyek,
                                string _Verseny ) {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    KorosztalyEgyben = _KorosztalyEgyben;
                    Nem = _Nem;
                    Nev = _Nev;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Eredmenyek = _Eredmenyek;
                    Verseny = _Verseny;
                }
            }

            public struct IJTIPUS {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VSAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    List<Verseny> versenyek = Program.database.Versenyek();
                    foreach( Verseny item in versenyek ) {
                        if( item.VersenySorozat == _VSAZON ) {
                            List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                            foreach( Korosztály item2 in temp ) {
                                Korosztalyok.Add( new KOROSZTALY( _Azonosito,
                                                                  item2.Azonosito,
                                                                  item2.Megnevezes,
                                                                  item.Azonosito,
                                                                  item2.Egyben,
                                                                  _OSSZESINDULO ) );
                            }
                        }
                    }
                }
            }

            /*az a baj, hogy a korosztály azonosítója és megnevezése alsó,felső lehet ugyanaz, 
            de ha az egyben más, akkor nem lehet összevonni-> tárolni kell mindent a korosztályról???
             */
            public struct KOROSZTALY {
                public string Azonosito;
                public string Megnevezes;
                public string Verseny;
                public bool Egyben;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus,
                                   string _Azonosito,
                                   string _Megnevezes,
                                   string _Verseny,
                                   bool _Egyben,
                                   List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Verseny = _Verseny;
                    Egyben = _Egyben;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Nok = Nok( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Egyben = Egybe( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );

                }
            }

            public struct INDULOK {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak;
                public List<INDULO> Egyben;
            }

            public struct EREDMENY {
                public string Verseny;
                public int Pont;
                public EREDMENY( string _Verseny, int _Pont ) {
                    Verseny = _Verseny;
                    Pont = _Pont;
                }
            }

            public List<IJTIPUS>
            Ijtipusok( string _VSAZON, List<INDULO> _Indulok ) {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for( int i = IjtipusokSeged.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    for( int j = 0; j < _Indulok.Count; j++ ) {
                        if( IjtipusokSeged[i].Azonosito == _Indulok[j].Ijtipus ) {
                            found = true;
                        }
                    }
                    if( !found ) { IjtipusokSeged.RemoveAt( i ); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach( Íjtípus ijtipus in IjtipusokSeged ) {
                    IJTIPUS temp = new IJTIPUS(_VSAZON, ijtipus.Azonosito, ijtipus.Megnevezes, _Indulok);
                    Data.Add( temp );
                }
                return Data;
            }

            public List<KOROSZTALY>
            Korosztalyok( string _Ijtipus, string _VSAZON, string _Egyben, List<INDULO> _OSSZESINDULO ) {
                List<KOROSZTALY> Data = new List<KOROSZTALY>();
                //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                List<Verseny> versenyek = Program.database.Versenyek();
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                        foreach( Korosztály item2 in temp ) {
                            Data.Add( new KOROSZTALY( _Ijtipus,
                                                      item2.Azonosito,
                                                      item2.Megnevezes,
                                                      item.Azonosito,
                                                      item2.Egyben,
                                                      _OSSZESINDULO ) );
                        }
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {

                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Egybe( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( !_Egyben ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public
            EREDMENYLAPVERSENYSOROZATRESZLETES( string _VSAZON ) : this( ) {
                Azonosito = _VSAZON;
                Versenysorozat temp = Program.database.Versenysorozat(_VSAZON).Value;
                Megnevezes = temp.megnevezés;
                VersenyekSzama = temp.versenyek;

                VersenyAzonositok = new List<string>( );

                List<Verseny> versenyek = Program.database.Versenyek();

                IComparer<Verseny> Compare = new OrderVerseny();
                versenyek.Sort( Compare );
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        VersenyAzonositok.Add( item.Azonosito );
                    }
                }
                OsszesIndulo = Seged1( _VSAZON );
                IjTipusok = Ijtipusok( _VSAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order();

                foreach( IJTIPUS ijtipus in IjTipusok ) {
                    foreach( KOROSZTALY korosztaly in ijtipus.Korosztalyok ) {
                        korosztaly.Indulok.Ferfiak.Sort( Comparer );
                        korosztaly.Indulok.Nok.Sort( Comparer );
                        korosztaly.Indulok.Egyben.Sort( Comparer );
                    }
                }

            }

            #region Seged
            public List<INDULO> Seged1( string _VSAZON ) {
                OsszesIndulo = new List<INDULO>( );
                List<Verseny> Versenyek = Program.database.Versenyek();
                foreach( Verseny verseny in Versenyek ) {
                    if( verseny.VersenySorozat == _VSAZON ) {
                        List<INDULO> temp = Seged2(verseny.Azonosito);
                        OsszesIndulo.AddRange( temp );

                    }
                }

                for( int i = ( OsszesIndulo.Count - 1 ); i >= 0; i-- ) {
                    for( int j = i - 1; j >= 0; j-- ) {
                        if( OsszesIndulo[i].Verseny != OsszesIndulo[j].Verseny &&
                              OsszesIndulo[i].Nev == OsszesIndulo[j].Nev &&
                              OsszesIndulo[i].Korosztaly == OsszesIndulo[j].Korosztaly &&
                              OsszesIndulo[i].KorosztalyEgyben == OsszesIndulo[j].KorosztalyEgyben &&
                              OsszesIndulo[i].Ijtipus == OsszesIndulo[j].Ijtipus ) {
                            OsszesIndulo[i].Eredmenyek.Add( OsszesIndulo[j].Eredmenyek[0] );
                            OsszesIndulo.RemoveAt( j );
                            i--;
                        }
                    }
                }

                for( int i = OsszesIndulo.Count - 1; i >= 0; i-- ) {
                    int sum = 0;
                    foreach( EREDMENY item in OsszesIndulo[i].Eredmenyek ) {
                        sum += item.Pont;
                    }

                    INDULO tmp = OsszesIndulo[i];
                    tmp.OsszPont = sum;
                    OsszesIndulo.RemoveAt( i );
                    OsszesIndulo.Add( tmp );

                }
                return OsszesIndulo;
            }

            public List<INDULO> Seged2( string _VEAZON ) {
                List<INDULO> temp_indulok = new List<INDULO>();

                List<Induló> indulók = Program.database.Indulók();
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
                List<Korosztály> korosztalyok = Program.database.Korosztályok(_VEAZON);
                Verseny verseny = Program.database.Verseny(_VEAZON).Value;

                foreach( Eredmény eredmeny in eredmények ) {
                    foreach( Induló indulo in indulók ) {
                        if( eredmeny.Nev == indulo.Nev ) {
                            EREDMENY tempEredmeny = new EREDMENY(verseny.Azonosito, eredmeny.Osszpont.Value);
                            List<EREDMENY> tempEredmenyek = new List<EREDMENY>();
                            tempEredmenyek.Add( tempEredmeny );
                            foreach( Korosztály korosztaly in korosztalyok ) {
                                if( eredmeny.KorosztalyAzonosito == korosztaly.Azonosito ) {
                                    temp_indulok.Add( new INDULO( eredmeny.Ijtipus,
                                                                eredmeny.KorosztalyAzonosito,
                                                                korosztaly.Egyben,
                                                                indulo.Nem,
                                                                indulo.Nev,
                                                                indulo.Egyesulet,
                                                                0,
                                                                tempEredmenyek,
                                                                verseny.Azonosito ) );
                                }
                            }
                        }
                    }
                }

                return temp_indulok;
            }

            #region Compare
            public class Order : IComparer<INDULO> {
                public int Compare( INDULO i1, INDULO i2 ) {
                    int z = i1.OsszPont.CompareTo(i2.OsszPont);
                    return ( -1 * z );
                }
            }

            public class OrderVerseny : IComparer<Verseny> {
                public int Compare( Verseny i1, Verseny i2 ) {
                    int z = i1.Datum.CompareTo(i2.Datum);
                    return z;
                }
            }
            #endregion

            #endregion
            // a versenysorozat összes indulója versenyenként, azaz többször is
            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> IjTipusok;
        }

        public struct EREDMENYLAPVESENYSOROZATTELJES {
            public string Azonosito;
            public string Megnevezes;
            public int VersenyekSzama;

            public List<string> VersenyAzonositok;

            public struct INDULO {
                public string Ijtipus;
                public string Korosztaly;
                public bool KorosztalyEgyben;
                public string Nem;
                public string Nev;
                public string Egyesulet;
                public int EletKor;
                public List<EREDMENY> Eredmenyek;
                public int OsszPont;
                public int AtlagSzazalek;
                public string Verseny;

                public INDULO( string _Ijtipus,
                                string _Korosztaly,
                                bool _KorosztalyEgyben,
                                string _Nem,
                                string _Nev,
                                string _Egyesulet,
                                int _EletKor,
                                int _OsszPont,
                                int _AtlagSzazalek,
                                List<EREDMENY> _Eredmenyek,
                                string _Verseny ) {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    KorosztalyEgyben = _KorosztalyEgyben;
                    Nem = _Nem;
                    Nev = _Nev;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Eredmenyek = _Eredmenyek;
                    AtlagSzazalek = _AtlagSzazalek;
                    Verseny = _Verseny;
                    EletKor = _EletKor;
                }
            }

            public struct IJTIPUS {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VSAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    List<Verseny> versenyek = Program.database.Versenyek();
                    foreach( Verseny item in versenyek ) {
                        if( item.VersenySorozat == _VSAZON ) {
                            List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                            foreach( Korosztály item2 in temp ) {
                                Korosztalyok.Add( new KOROSZTALY( _Azonosito,
                                                                  item2.Azonosito,
                                                                  item2.Megnevezes,
                                                                  item.Azonosito,
                                                                  item2.Egyben,
                                                                  _OSSZESINDULO ) );
                            }
                        }
                    }

                }

            }

            public struct KOROSZTALY {
                public string Azonosito;
                public string Megnevezes;
                public string Verseny;
                public bool Egyben;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus,
                                   string _Azonosito,
                                   string _Megnevezes,
                                   string _Verseny,
                                   bool _Egyben,
                                   List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Verseny = _Verseny;
                    Egyben = _Egyben;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Nok = Nok( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Egyben = Egybe( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );

                }
            }

            public struct INDULOK {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak;
                public List<INDULO> Egyben;
            }

            public struct EREDMENY {
                public string Verseny;
                public int Pont;
                public int Szazalek;
                public EREDMENY( string _Verseny, int _Pont, int _Szazalek ) {
                    Verseny = _Verseny;
                    Pont = _Pont;
                    Szazalek = _Szazalek;
                }
            }

            public List<IJTIPUS>
            Ijtipusok( string _VSAZON, List<INDULO> _Indulok ) {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for( int i = IjtipusokSeged.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    for( int j = 0; j < _Indulok.Count; j++ ) {
                        if( IjtipusokSeged[i].Azonosito == _Indulok[j].Ijtipus ) {
                            found = true;
                        }
                    }
                    if( !found ) { IjtipusokSeged.RemoveAt( i ); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach( Íjtípus ijtipus in IjtipusokSeged ) {
                    IJTIPUS temp = new IJTIPUS(_VSAZON, ijtipus.Azonosito, ijtipus.Megnevezes, _Indulok);
                    Data.Add( temp );
                }
                return Data;
            }

            public List<KOROSZTALY>
            Korosztalyok( string _Ijtipus, string _VSAZON, string _Egyben, List<INDULO> _OSSZESINDULO ) {
                List<KOROSZTALY> Data = new List<KOROSZTALY>();
                //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                List<Verseny> versenyek = Program.database.Versenyek();
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                        foreach( Korosztály item2 in temp ) {
                            Data.Add( new KOROSZTALY( _Ijtipus,
                                                      item2.Azonosito,
                                                      item2.Megnevezes,
                                                      item.Azonosito,
                                                      item2.Egyben,
                                                      _OSSZESINDULO ) );
                        }
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {

                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }

                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Egybe( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( !_Egyben ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public
            EREDMENYLAPVESENYSOROZATTELJES( string _VSAZON )
                : this( ) {
                Azonosito = _VSAZON;
                Versenysorozat temp = Program.database.Versenysorozat(_VSAZON).Value;
                Megnevezes = temp.megnevezés;
                VersenyekSzama = temp.versenyek;

                VersenyAzonositok = new List<string>( );

                List<Verseny> versenyek = Program.database.Versenyek();

                IComparer<Verseny> Compare = new OrderVerseny();
                versenyek.Sort( Compare );
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        VersenyAzonositok.Add( item.Azonosito );
                    }
                }
                OsszesIndulo = Seged1( _VSAZON );
                IjTipusok = Ijtipusok( _VSAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order();

                foreach( IJTIPUS ijtipus in IjTipusok ) {
                    foreach( KOROSZTALY korosztaly in ijtipus.Korosztalyok ) {
                        korosztaly.Indulok.Ferfiak.Sort( Comparer );
                        korosztaly.Indulok.Nok.Sort( Comparer );
                        korosztaly.Indulok.Egyben.Sort( Comparer );
                    }
                }

            }

            #region Seged
            public List<INDULO> Seged1( string _VSAZON ) {
                int VersenyekSzama = 0;
                OsszesIndulo = new List<INDULO>( );
                List<Verseny> Versenyek = Program.database.Versenyek();
                foreach( Verseny verseny in Versenyek ) {
                    if( verseny.VersenySorozat == _VSAZON ) {
                        List<INDULO> temp = Seged2(verseny.Azonosito);
                        OsszesIndulo.AddRange( temp );
                        VersenyekSzama++;
                    }
                }

                for( int i = OsszesIndulo.Count - 1; i >= 0; i-- ) {
                    for( int j = i - 1; j >= 0; j-- ) {
                        if( OsszesIndulo[i].Verseny != OsszesIndulo[j].Verseny &&
                            OsszesIndulo[i].Nev == OsszesIndulo[j].Nev &&
                            OsszesIndulo[i].Korosztaly == OsszesIndulo[j].Korosztaly &&
                            OsszesIndulo[i].Ijtipus == OsszesIndulo[j].Ijtipus ) {
                            OsszesIndulo[i].Eredmenyek.Add( OsszesIndulo[j].Eredmenyek[0] );
                            OsszesIndulo.RemoveAt( j );
                            i--;
                        }
                    }
                }
                for( int i = OsszesIndulo.Count - 1; i >= 0; i-- ) {
                    int sum = 0;
                    int szazalek = 0;
                    foreach( EREDMENY item in OsszesIndulo[i].Eredmenyek ) {
                        sum += item.Pont;
                        szazalek += item.Szazalek;
                    }

                    INDULO tmp = OsszesIndulo[i];
                    tmp.OsszPont = sum;
                    tmp.AtlagSzazalek = szazalek / VersenyekSzama;
                    OsszesIndulo.RemoveAt( i );
                    OsszesIndulo.Add( tmp );

                }
                return OsszesIndulo;

            }

            public List<INDULO> Seged2( string _VEAZON ) {
                List<INDULO> temp_indulok = new List<INDULO>();

                List<Induló> indulók = Program.database.Indulók();
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
                List<Korosztály> korosztalyok = Program.database.Korosztályok(_VEAZON);

                foreach( Eredmény eredmeny in eredmények ) {
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    foreach( Induló indulo in indulók ) {
                        if( eredmeny.Nev == indulo.Nev ) {
                            EREDMENY tempEredmeny = new EREDMENY(verseny.Azonosito, eredmeny.Osszpont.Value, eredmeny.Szazalek.Value);
                            List<EREDMENY> tempEredmenyek = new List<EREDMENY>();
                            tempEredmenyek.Add( tempEredmeny );
                            foreach( Korosztály korosztaly in korosztalyok ) {
                                if( eredmeny.KorosztalyAzonosito == korosztaly.Azonosito ) {
                                    temp_indulok.Add( new INDULO( eredmeny.Ijtipus,
                                                                 eredmeny.KorosztalyAzonosito,
                                                                 korosztaly.Egyben,
                                                                 indulo.Nem,
                                                                 indulo.Nev,
                                                                 indulo.Egyesulet,
                                                                 Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                                                                 0,
                                                                 0,
                                                                 tempEredmenyek,
                                                                 verseny.Azonosito ) );
                                }
                            }
                        }
                    }
                }
                return temp_indulok;
            }

            #region Compare
            public class Order : IComparer<INDULO> {
                public int Compare( INDULO i1, INDULO i2 ) {
                    int z = i1.OsszPont.CompareTo(i2.OsszPont);
                    return ( -1 * z );
                }
            }

            public class OrderVerseny : IComparer<Verseny> {
                public int Compare( Verseny i1, Verseny i2 ) {
                    int z = i1.Datum.CompareTo(i2.Datum);
                    return z;
                }
            }
            #endregion

            #endregion

            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> IjTipusok;

        }

        public struct EREDMENYLAPVESENYSOROZATMISZ {
            public string Azonosito;
            public string Megnevezes;
            public int VersenyekSzama;

            public List<string> VersenyAzonositok;

            public struct INDULO {
                public string Ijtipus;
                public string Korosztaly;
                public bool KorosztalyEgyben;
                public string Nem;
                public string Nev;
                public string Egyesulet;
                public int EletKor;
                public List<EREDMENY> Eredmenyek;
                public int OsszPont;
                public int AtlagSzazalek;
                public string Verseny;

                public INDULO( string _Ijtipus,
                                string _Korosztaly,
                                bool _KorosztalyEgyben,
                                string _Nem,
                                string _Nev,
                                string _Egyesulet,
                                int _EletKor,
                                int _OsszPont,
                                int _AtlagSzazalek,
                                List<EREDMENY> _Eredmenyek,
                                string _Verseny ) {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    KorosztalyEgyben = _KorosztalyEgyben;
                    Nem = _Nem;
                    Nev = _Nev;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Eredmenyek = _Eredmenyek;
                    AtlagSzazalek = _AtlagSzazalek;
                    Verseny = _Verseny;
                    EletKor = _EletKor;
                }
            }

            public struct IJTIPUS {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VSAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    List<Verseny> versenyek = Program.database.Versenyek();
                    foreach( Verseny item in versenyek ) {
                        if( item.VersenySorozat == _VSAZON ) {
                            List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                            foreach( Korosztály item2 in temp ) {
                                Korosztalyok.Add( new KOROSZTALY( _Azonosito,
                                                                  item2.Azonosito,
                                                                  item2.Megnevezes,
                                                                  item.Azonosito,
                                                                  item2.Egyben,
                                                                  _OSSZESINDULO ) );
                            }
                        }
                    }

                }

            }

            public struct KOROSZTALY {
                public string Azonosito;
                public string Megnevezes;
                public string Verseny;
                public bool Egyben;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus,
                                   string _Azonosito,
                                   string _Megnevezes,
                                   string _Verseny,
                                   bool _Egyben,
                                   List<INDULO> _OSSZESINDULO ) {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Verseny = _Verseny;
                    Egyben = _Egyben;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Nok = Nok( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );
                    Indulok.Egyben = Egybe( _Ijtipus, _Azonosito, _Verseny, _Egyben, _OSSZESINDULO );

                }
            }

            public struct INDULOK {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak;
                public List<INDULO> Egyben;
            }

            public struct EREDMENY {
                public string Verseny;
                public int Pont;
                public int Szazalek;
                public EREDMENY( string _Verseny, int _Pont, int _Szazalek ) {
                    Verseny = _Verseny;
                    Pont = _Pont;
                    Szazalek = _Szazalek;
                }
            }

            public List<IJTIPUS>
            Ijtipusok( string _VSAZON, List<INDULO> _Indulok ) {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for( int i = IjtipusokSeged.Count - 1; i >= 0; i-- ) {
                    bool found = false;
                    for( int j = 0; j < _Indulok.Count; j++ ) {
                        if( IjtipusokSeged[i].Azonosito == _Indulok[j].Ijtipus ) {
                            found = true;
                        }
                    }
                    if( !found ) { IjtipusokSeged.RemoveAt( i ); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach( Íjtípus ijtipus in IjtipusokSeged ) {
                    IJTIPUS temp = new IJTIPUS(_VSAZON, ijtipus.Azonosito, ijtipus.Megnevezes, _Indulok);
                    Data.Add( temp );
                }
                return Data;
            }

            public List<KOROSZTALY>
            Korosztalyok( string _Ijtipus, string _VSAZON, string _Egyben, List<INDULO> _OSSZESINDULO ) {
                List<KOROSZTALY> Data = new List<KOROSZTALY>();
                //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                List<Verseny> versenyek = Program.database.Versenyek();
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                        foreach( Korosztály item2 in temp ) {
                            Data.Add( new KOROSZTALY( _Ijtipus,
                                                      item2.Azonosito,
                                                      item2.Megnevezes,
                                                      item.Azonosito,
                                                      item2.Egyben,
                                                      _OSSZESINDULO ) );
                        }
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {

                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }

                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( _Egyben == true ) { return Data; }

                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N" &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Egybe( string _Ijtipus, string _Korosztaly, string _Verseny, bool _Egyben, List<INDULO> _Indulok ) {
                List<INDULO> Data = new List<INDULO>();
                if( !_Egyben ) { return Data; }
                for( int i = _Indulok.Count - 1; i >= 0; i-- ) {
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].KorosztalyEgyben == _Egyben ) {
                        Data.Add( _Indulok[i] );
                        _Indulok.RemoveAt( i );
                    }
                }
                return Data;
            }

            public
            EREDMENYLAPVESENYSOROZATMISZ( string _VSAZON )
                : this( ) {
                Azonosito = _VSAZON;
                Versenysorozat temp = Program.database.Versenysorozat(_VSAZON).Value;
                Megnevezes = temp.megnevezés;
                VersenyekSzama = temp.versenyek;

                VersenyAzonositok = new List<string>( );

                List<Verseny> versenyek = Program.database.Versenyek();

                IComparer<Verseny> Compare = new OrderVerseny();
                versenyek.Sort( Compare );
                foreach( Verseny item in versenyek ) {
                    if( item.VersenySorozat == _VSAZON ) {
                        VersenyAzonositok.Add( item.Azonosito );
                    }
                }
                OsszesIndulo = Seged1( _VSAZON );
                IjTipusok = Ijtipusok( _VSAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order();

                foreach( IJTIPUS ijtipus in IjTipusok ) {
                    foreach( KOROSZTALY korosztaly in ijtipus.Korosztalyok ) {
                        korosztaly.Indulok.Ferfiak.Sort( Comparer );
                        korosztaly.Indulok.Nok.Sort( Comparer );
                        korosztaly.Indulok.Egyben.Sort( Comparer );
                    }
                }

            }

            #region Seged
            public List<INDULO> Seged1( string _VSAZON ) {
                int VersenyekSzama = 0;
                OsszesIndulo = new List<INDULO>( );
                List<Verseny> Versenyek = Program.database.Versenyek();
                foreach( Verseny verseny in Versenyek ) {
                    if( verseny.VersenySorozat == _VSAZON ) {
                        List<INDULO> temp = Seged2(verseny.Azonosito);
                        OsszesIndulo.AddRange( temp );
                        VersenyekSzama++;
                    }
                }

                for( int i = OsszesIndulo.Count - 1; i >= 0; i-- ) {
                    for( int j = i - 1; j >= 0; j-- ) {
                        if( OsszesIndulo[i].Verseny != OsszesIndulo[j].Verseny &&
                            OsszesIndulo[i].Nev == OsszesIndulo[j].Nev &&
                            OsszesIndulo[i].Korosztaly == OsszesIndulo[j].Korosztaly &&
                            OsszesIndulo[i].Ijtipus == OsszesIndulo[j].Ijtipus ) {
                            OsszesIndulo[i].Eredmenyek.Add( OsszesIndulo[j].Eredmenyek[0] );
                            OsszesIndulo.RemoveAt( j );
                            i--;
                        }
                    }
                }

                for( int i = OsszesIndulo.Count - 1; i >= 0; i-- ) {
                    int sum = 0;
                    int szazalek = 0;
                    foreach( EREDMENY item in OsszesIndulo[i].Eredmenyek ) {
                        sum += item.Pont;
                        szazalek += item.Szazalek;
                    }

                    INDULO tmp = OsszesIndulo[i];
                    tmp.OsszPont = sum;
                    tmp.AtlagSzazalek = szazalek / VersenyekSzama;
                    OsszesIndulo.RemoveAt( i );
                    OsszesIndulo.Add( tmp );

                }
                return OsszesIndulo;

            }

            public List<INDULO> Seged2( string _VEAZON ) {
                List<INDULO> temp_indulok = new List<INDULO>();

                List<Induló> indulók = Program.database.Indulók();
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
                List<Korosztály> korosztalyok = Program.database.Korosztályok(_VEAZON);

                foreach( Eredmény eredmeny in eredmények ) {
                    Verseny verseny = Program.database.Verseny(_VEAZON).Value;
                    foreach( Induló indulo in indulók ) {
                        if( eredmeny.Nev == indulo.Nev && indulo.Engedely != null ) {
                            EREDMENY tempEredmeny = new EREDMENY(verseny.Azonosito, eredmeny.Osszpont.Value, eredmeny.Szazalek.Value);
                            List<EREDMENY> tempEredmenyek = new List<EREDMENY>();
                            tempEredmenyek.Add( tempEredmeny );
                            foreach( Korosztály korosztaly in korosztalyok ) {
                                if( eredmeny.KorosztalyAzonosito == korosztaly.Azonosito ) {
                                    temp_indulok.Add( new INDULO( eredmeny.Ijtipus,
                                                                 eredmeny.KorosztalyAzonosito,
                                                                 korosztaly.Egyben,
                                                                 indulo.Nem,
                                                                 indulo.Nev,
                                                                 indulo.Egyesulet,
                                                                 Program.database.InduloKora( _VEAZON, eredmeny.Nev ),
                                                                 0,
                                                                 0,
                                                                 tempEredmenyek,
                                                                 verseny.Azonosito ) );
                                }
                            }
                        }
                    }
                }
                return temp_indulok;
            }

            #region Compare
            public class Order : IComparer<INDULO> {
                public int Compare( INDULO i1, INDULO i2 ) {
                    int z = i1.OsszPont.CompareTo(i2.OsszPont);
                    return ( -1 * z );
                }
            }

            public class OrderVerseny : IComparer<Verseny> {
                public int Compare( Verseny i1, Verseny i2 ) {
                    int z = i1.Datum.CompareTo(i2.Datum);
                    return z;
                }
            }
            #endregion

            #endregion

            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> IjTipusok;

        }

        public struct OKLEVELVERSENYZO {
            public string Verseny { get; set; }
            public string VersenySorozat { get; set; }
            public int Helyezes { get; set; }
            public string Indulo { get; set; }
            public string Egyesulet { get; set; }
            public string Ijtipus { get; set; }
            public string Korosztaly { get; set; }
            public string InduloNeme { get; set; }
            public string Datum{ get; set; }

            public OKLEVELVERSENYZO( string _Verseny, string _VersenySorozat, int _Helyezes, string _Indulo, string _Egyesulet, string _Ijtipus, string _Korosztaly, string _InduloNeme, string _Datum ) {
                Verseny = _Verseny;
                VersenySorozat = _VersenySorozat;
                Helyezes = _Helyezes;
                Indulo = _Indulo;
                Egyesulet = _Egyesulet;
                Ijtipus = _Ijtipus;
                Korosztaly = _Korosztaly;
                InduloNeme = _InduloNeme;
                Datum = _Datum;
            }
        }

        #region Tablazatok Formazas

        static public void
        NevezesiListaTablazatFormazas( Table _table ) {
            _table.Alignment = Alignment.center;

            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            _table.SetBorder( TableBorderType.InsideH, b );
            _table.SetBorder( TableBorderType.InsideV, b );
            _table.SetBorder( TableBorderType.Bottom, b );
            _table.SetBorder( TableBorderType.Top, b );
            _table.SetBorder( TableBorderType.Left, b );
            _table.SetBorder( TableBorderType.Right, b );

            for( int i = 0; i < 6; i++ ) {
                _table.Rows[0].Cells[i].SetBorder( TableCellBorderType.Bottom, c );
            }

            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 70;
                _table.Rows[i].Cells[1].Width = 200;
                _table.Rows[i].Cells[2].Width = 150;
                _table.Rows[i].Cells[3].Width = 40;
                _table.Rows[i].Cells[4].Width = 150;
                _table.Rows[i].Cells[5].Width = 70;
            }
            _table.AutoFit = AutoFit.ColumnWidth;
        }

        static public void
        CsapatlistaTablazatFormazas( Table _table ) {
            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            _table.SetBorder( TableBorderType.InsideH, b );
            _table.SetBorder( TableBorderType.InsideV, b );
            _table.SetBorder( TableBorderType.Bottom, b );
            _table.SetBorder( TableBorderType.Top, b );
            _table.SetBorder( TableBorderType.Left, b );
            _table.SetBorder( TableBorderType.Right, b );

            for( int i = 0; i < 6; i++ ) {
                _table.Rows[0].Cells[i].SetBorder( TableCellBorderType.Bottom, c );
            }

            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 70;
                _table.Rows[i].Cells[1].Width = 70;
                _table.Rows[i].Cells[2].Width = 200;
                _table.Rows[i].Cells[3].Width = 100;
                _table.Rows[i].Cells[4].Width = 70;
                _table.Rows[i].Cells[5].Width = 200;
            }
            _table.AutoFit = AutoFit.ColumnWidth;

        }

        static public void
        startlista_táblázat_formázás( Table _table ) {
            _table.AutoFit = AutoFit.Contents;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 50;
                _table.Rows[i].Cells[1].Width = 50;
                _table.Rows[i].Cells[2].Width = 50;
                _table.Rows[i].Cells[3].Width = 170;
                _table.Rows[i].Cells[4].Width = 230;
                _table.Rows[i].Cells[5].Width = 70;
                _table.Rows[i].Cells[6].Width = 80;
            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            //_table.SetBorder(TableBorderType.InsideH, c);
            _table.SetBorder( TableBorderType.InsideV, c );
            // _table.SetBorder(TableBorderType.Bottom, c);
            _table.SetBorder( TableBorderType.Top, c );
            _table.SetBorder( TableBorderType.Left, c );
            _table.SetBorder( TableBorderType.Right, c );
        }

        static public void
        EredmenyLapReszletesTablazatFormazas( Table _table ) {
            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 30;
                _table.Rows[i].Cells[1].Width = 50;
                _table.Rows[i].Cells[2].Width = 200;
                _table.Rows[i].Cells[3].Width = 300;
                for( int j = 4; j < _table.Rows[i].Cells.Count - 1; j++ ) {
                    _table.Rows[i].Cells[j].Width = 70;
                }

                _table.Rows[i].Cells[_table.Rows[i].Cells.Count - 1].Width = 100;

                _table.Rows[i].Height = 20;

            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
            _table.SetBorder( TableBorderType.InsideH, d );
            _table.SetBorder( TableBorderType.InsideV, c );
            _table.SetBorder( TableBorderType.Bottom, c );
            _table.SetBorder( TableBorderType.Top, c );
            _table.SetBorder( TableBorderType.Left, c );
            _table.SetBorder( TableBorderType.Right, c );

        }

        static public void
        EredmenyLapVersenyTablazatFormazas( Table _table ) {
            _table.AutoFit = AutoFit.Contents;
            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 30;
                _table.Rows[i].Cells[1].Width = 50;
                _table.Rows[i].Cells[2].Width = 50;
                _table.Rows[i].Cells[3].Width = 250;
                _table.Rows[i].Cells[4].Width = 150;
                _table.Rows[i].Cells[5].Width = 70;
                _table.Rows[i].Cells[6].Width = 70;
            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
            _table.SetBorder( TableBorderType.InsideH, d );
            _table.SetBorder( TableBorderType.InsideV, c );
            _table.SetBorder( TableBorderType.Bottom, c );
            _table.SetBorder( TableBorderType.Top, c );
            _table.SetBorder( TableBorderType.Left, c );
            _table.SetBorder( TableBorderType.Right, c );

        }

        static public void
        EredmenyLapVersenySorozatTablazatFormazas( Table _table ) {
            _table.AutoFit = AutoFit.Contents;
            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 30;
                _table.Rows[i].Cells[1].Width = 50;
                _table.Rows[i].Cells[2].Width = 250;
                _table.Rows[i].Cells[3].Width = 50;
                _table.Rows[i].Cells[4].Width = 150;
                _table.Rows[i].Cells[5].Width = 70;
                _table.Rows[i].Cells[6].Width = 70;
            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
            _table.SetBorder( TableBorderType.InsideH, d );
            _table.SetBorder( TableBorderType.InsideV, c );
            _table.SetBorder( TableBorderType.Bottom, c );
            _table.SetBorder( TableBorderType.Top, c );
            _table.SetBorder( TableBorderType.Left, c );
            _table.SetBorder( TableBorderType.Right, c );

        }

        static public void
        BeirolapHeaderTablazatFormazas( Table _table ) {
            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 350;
                _table.Rows[i].Cells[1].Width = 200;
                _table.Rows[i].Cells[2].Width = 100;
                _table.Rows[i].Height = 27;
            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            _table.SetBorder( TableBorderType.InsideH, c );
            _table.SetBorder( TableBorderType.InsideV, c );
            _table.SetBorder( TableBorderType.Bottom, c );
            _table.SetBorder( TableBorderType.Top, c );
            _table.SetBorder( TableBorderType.Left, c );
            _table.SetBorder( TableBorderType.Right, c );
        }

        static public void
        BeirolapTablazatFormazas( Table _table ) {
            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 70;
                _table.Rows[i].Cells[1].Width = 80;
                _table.Rows[i].Cells[2].Width = 80;
                _table.Rows[i].Cells[3].Width = 80;
                _table.Rows[i].Cells[4].Width = 80;
                _table.Rows[i].Cells[5].Width = 80;
                _table.Rows[i].Cells[6].Width = 80;
                _table.Rows[i].Cells[7].Width = 80;
                _table.Rows[i].Height = 25;
            }

            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Height = 20;
            }
        }

        static public void
        EgyesuletTablazatFormazas( Table _table ) {
            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            _table.SetBorder( TableBorderType.InsideH, b );
            _table.SetBorder( TableBorderType.InsideV, b );
            _table.SetBorder( TableBorderType.Bottom, b );
            _table.SetBorder( TableBorderType.Top, b );
            _table.SetBorder( TableBorderType.Left, b );
            _table.SetBorder( TableBorderType.Right, b );

            for( int i = 0; i < 4; i++ ) {
                _table.Rows[0].Cells[i].SetBorder( TableCellBorderType.Bottom, c );
            }

            _table.AutoFit = AutoFit.ColumnWidth;
            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Cells[0].Width = 100;
                _table.Rows[i].Cells[1].Width = 250;
                _table.Rows[i].Cells[2].Width = 250;
                _table.Rows[i].Cells[3].Width = 100;
            }

            for( int i = 0; i < _table.Rows.Count; i++ ) {
                _table.Rows[i].Height = 20;
            }
        }
        #endregion

        public static Footer
        PageNumber( DocX _document ) {
            _document.AddFooters( );
            Footer footer = _document.Footers.odd;

            Table FooterTable = footer.InsertTable(1, 2);
            FooterTable.Rows[0].Cells[1].Paragraphs[0].InsertPageNumber( PageNumberFormat.normal, 0 );
            FooterTable.Rows[0].Cells[1].Paragraphs[0].Append( ". oldal" );
            FooterTable.AutoFit = AutoFit.ColumnWidth;
            FooterTable.Rows[0].Cells[0].Width = _document.PageWidth - 200;
            FooterTable.Rows[0].Cells[1].Width = 60;

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            FooterTable.SetBorder( TableBorderType.InsideH, c );
            FooterTable.SetBorder( TableBorderType.InsideV, c );
            FooterTable.SetBorder( TableBorderType.Bottom, c );
            FooterTable.SetBorder( TableBorderType.Top, c );
            FooterTable.SetBorder( TableBorderType.Left, c );
            FooterTable.SetBorder( TableBorderType.Right, c );

            return footer;
        }

        static public void
        Print( string _FileName ) {
            ProcessStartInfo info = new ProcessStartInfo(_FileName.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start( info );
        }

        static public void Dialog( string _FileName ) {
            DialogFileName = _FileName;
            dialog = new Form( );
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.Width = 300;
            dialog.Height = 150;
            dialog.Text = "Nyomtatás";
            Label lblNyomtat = new Label() { Left = 70, Top = 20, Text = "Biztosan nyomtatni akar?", Width = 200 };
            Button btnIgen = new Button() { Text = "Igen", Left = 30, Top = 50, Width = 70 };
            Button btnNem = new Button() { Text = "Nem", Left = 110, Top = 50, Width = 70 };
            Button btnMegnyit = new Button() { Text = "Megnyitás", Left = 190, Top = 50, Width = 70 };
            dialog.Controls.Add( lblNyomtat );
            dialog.Controls.Add( btnIgen );
            dialog.Controls.Add( btnNem );
            dialog.Controls.Add( btnMegnyit );
            btnIgen.Click += btnIgen_Click;
            btnNem.Click += btnNem_Click;
            btnMegnyit.Click += btnMegnyit_Click;
            dialog.ShowDialog( );
        }

       
        #region EventHandlers
        static void btnIgen_Click( object _sender, EventArgs _event ) {
            Nyomtat.Print( DialogFileName );
            dialog.DialogResult = DialogResult.OK;
        }

        static void btnMegnyit_Click( object sender, EventArgs e ) {
            Process.Start( DialogFileName );
        }

        static void btnNem_Click( object sender, EventArgs e ) {
            dialog.DialogResult = DialogResult.OK;
            return;
        }

        #endregion
    }
}


