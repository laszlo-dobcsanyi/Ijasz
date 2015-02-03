using Novacode;
using System.Drawing;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System;
using System.Collections;

namespace Íjász
{

    public static class Nyomtat
    {
        public static int ijtipus_sorszam;
        public static int korosztaly_sorszam;
        public static Form dialog;
        public static string DialogFileName;


        public struct CSAPATLISTA
        {
            public struct VERSENYADATOK
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;

                public
                VERSENYADATOK( string _VEAZON )
                {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
                    List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );
                    int MegjelentIndulok = 0;
                    foreach ( Eredmény item in eredmenyek ) { if ( item.megjelent == true ) { MegjelentIndulok++; } }

                    foreach ( Versenysorozat item in versenysorozatok )
                    {
                        if ( item.azonosító == verseny.VersenySorozat )
                        {
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

            public struct CSAPAT
            {
                public int INCSSZ;
                public List<VERSENYZOADAT> versenyzoadatok;
                
                public CSAPAT( int _INCSSZ )
                {
                    INCSSZ = _INCSSZ;
                    versenyzoadatok = new List<VERSENYZOADAT>( );
                }
            }

            public struct VERSENYZOADAT
            {
                public string INNEVE;
                public int INSOSZ;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;
                public string ITMEGN;


                public VERSENYZOADAT( string _INNEVE, int _INSOSZ, int _INSZUL, string _INEGYE, int _INCSSZ, string _ITMEGN )
                {
                    ITMEGN = _ITMEGN;
                    INNEVE = _INNEVE;
                    INSOSZ = _INSOSZ;
                    INSZUL = _INSZUL;
                    INEGYE = _INEGYE;
                    INCSSZ = _INCSSZ;
                }

                public class RendezCsapatSzam : IComparer<VERSENYZOADAT>
                {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 )
                    {
                        return v1.INCSSZ.CompareTo( v2.INCSSZ );
                    }
                }

                public class RendezNev : IComparer<VERSENYZOADAT>
                {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 )
                    {
                        if ( v1.INCSSZ.CompareTo( v2.INCSSZ ) == 0 )
                        {
                            return v1.INNEVE.CompareTo( v2.INNEVE );
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }
            }

            public List<CSAPAT> CsapatokLista(string _VEAZON)
            {
                List<CSAPAT> list_csapatok = Program.database.CsapatLista( _VEAZON );
                CSAPAT[] csapatok = list_csapatok.ToArray( );

                for ( int i = 0; i < csapatok.Length;i++ )
                {
                    csapatok[i].versenyzoadatok = VersenyzoAdatok( _VEAZON, csapatok[i].INCSSZ );
                }

                list_csapatok = new List<CSAPAT>(csapatok);
                return list_csapatok;
            }

            public List<VERSENYZOADAT>
            VersenyzoAdatok( string _VEAZON, int _Csapat )
            {
                List<VERSENYZOADAT> Data = new List<VERSENYZOADAT>( );

                List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );
                List<Induló> indulok = Program.database.Indulók( );
                List<Íjtípus> ijtipusok = Program.database.Íjtípusok( );
                foreach ( Eredmény eredmeny in eredmenyek )
                {
                    foreach ( Induló indulo in indulok )
                    {
                        if ( eredmeny.név == indulo.név )
                        {
                            if ( eredmeny.megjelent == true && eredmeny.csapat == _Csapat)
                            {
                                foreach ( Íjtípus ijtipus in ijtipusok )
                                {
                                    if ( ijtipus.azonosító == eredmeny.íjtípus )
                                    {
                                        Data.Add( new VERSENYZOADAT( eredmeny.név,
                                                                                (int)eredmeny.sorszám,
                                                                                Program.database.InduloKora( _VEAZON, eredmeny.név ),
                                                                                indulo.egyesület,
                                                                                eredmeny.csapat,
                                                                                ijtipus.megnevezés ) );
                                    }
                                }
                            }
                        }
                    }
                }

                IComparer<VERSENYZOADAT> Comparer1 = new CSAPATLISTA.VERSENYZOADAT.RendezCsapatSzam( );
                Data.Sort( Comparer1 );
                IComparer<VERSENYZOADAT> Comparer2 = new CSAPATLISTA.VERSENYZOADAT.RendezNev( );
                Data.Sort( Comparer2 );

                return Data;
            }

            public CSAPATLISTA( string _VEAZON )
                : this( )
            {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                Csapatok = CsapatokLista( _VEAZON );
            }

            public VERSENYADATOK VersenyAdatok;
            public List<CSAPAT> Csapatok;
        };

        public struct NEVEZESILISTA
        {
            public struct VERSENYADATOK
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;

                public
                VERSENYADATOK( string _VEAZON , bool _NemMegjelentNyomtat )
                {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
                    List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );
                    int MegjelentIndulok = 0;
                    foreach ( Eredmény item in eredmenyek ) 
                    {
                        if( _NemMegjelentNyomtat == true )
                        {
                            if ( item.megjelent == false ) 
                            {
                                MegjelentIndulok++; 
                            } 

                        }
                        else if ( _NemMegjelentNyomtat == false )
                        {
                            if ( item.megjelent == true )
                            {
                                MegjelentIndulok++;
                            } 
                        }
                    }

                    foreach ( Versenysorozat item in versenysorozatok )
                    {
                        if ( item.azonosító == verseny.VersenySorozat )
                        {
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

            public struct VERSENYZOADAT
            {
                public string INNEVE;
                public int INSOSZ;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;
                public string ITMEGN;

                public VERSENYZOADAT( string _INNEVE, int _INSOSZ, int _INSZUL, string _INEGYE, int _INCSSZ, string _ITMEGN )
                {
                    INNEVE = _INNEVE;
                    ITMEGN = _ITMEGN;
                    INSOSZ = _INSOSZ;
                    INSZUL = _INSZUL;
                    INEGYE = _INEGYE;
                    INCSSZ = _INCSSZ;
                }

                public class RendezNev : IComparer<VERSENYZOADAT>
                {
                    public int Compare( VERSENYZOADAT v1, VERSENYZOADAT v2 )
                    {
                        return v1.INNEVE.CompareTo( v2.INNEVE );
                    }
                }
            }

            public List<VERSENYZOADAT>
            Versenyzodatok(string _VEAZON, bool _NemMegjelentNyomtat)
            {
                List<VERSENYZOADAT> Data = new List<VERSENYZOADAT>( );

                List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );
                List<Induló> indulok = Program.database.Indulók( );
                List<Íjtípus> ijtipusok = Program.database.Íjtípusok( );
                foreach ( Eredmény eredmeny in eredmenyek )
                {
                    foreach ( Induló indulo in indulok )
                    {
                        if ( eredmeny.név == indulo.név )
                        {
                            if(_NemMegjelentNyomtat == true)
                            {
                                if ( eredmeny.megjelent == false )
                                {
                                    foreach ( Íjtípus ijtipus in ijtipusok )
                                    {
                                        if ( ijtipus.azonosító == eredmeny.íjtípus )
                                        {
                                            Data.Add( new VERSENYZOADAT( eredmeny.név,
                                                                        (int)eredmeny.sorszám,
                                                                        Program.database.InduloKora( _VEAZON, eredmeny.név ),
                                                                        indulo.egyesület,
                                                                        eredmeny.csapat,
                                                                        ijtipus.megnevezés ) );
                                        }
                                    }
                                }
                            }
                            else if(_NemMegjelentNyomtat == false)
                            {
                                if ( eredmeny.megjelent == true )
                                {
                                    foreach ( Íjtípus ijtipus in ijtipusok )
                                    {
                                        if ( ijtipus.azonosító == eredmeny.íjtípus )
                                        {
                                            Data.Add( new VERSENYZOADAT( eredmeny.név,
                                                                        (int)eredmeny.sorszám,
                                                                        Program.database.InduloKora( _VEAZON, eredmeny.név ),
                                                                        indulo.egyesület,
                                                                        eredmeny.csapat,
                                                                        ijtipus.megnevezés ) );
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                }

                IComparer<VERSENYZOADAT> Comparer2 = new NEVEZESILISTA.VERSENYZOADAT.RendezNev( );
                Data.Sort( Comparer2 );

                return Data;
            }

            public NEVEZESILISTA( string _VEAZON, bool _NemMegjelentNyomtat )
                : this( )
            {
                VersenyAdatok = new VERSENYADATOK( _VEAZON, _NemMegjelentNyomtat );
                VersenyzoAdatok = Versenyzodatok( _VEAZON, _NemMegjelentNyomtat );
            }

            public VERSENYADATOK VersenyAdatok;
            public List<VERSENYZOADAT> VersenyzoAdatok;
        };
        
        public struct BEIROLAP
        {
            public struct VERSENYADATOK
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public string VSAZON;
                public string VSMEGN;
                public int VEALSZ;

                public
                VERSENYADATOK( string _VEAZON )
                {
                    VSAZON = null;
                    VSMEGN = null;
                    Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok( );
                    List<Eredmény> eredmenyek = Program.database.Eredmények( _VEAZON );
                    int MegjelentIndulok = 0;
                    foreach ( Eredmény item in eredmenyek ) { if ( item.megjelent == true ) { MegjelentIndulok++; } }

                    foreach ( Versenysorozat item in versenysorozatok )
                    {
                        if ( item.azonosító == verseny.VersenySorozat )
                        {
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
           
            public struct VERSENYZOADATOK
            {
                public string INNEVE;
                public string INBEEK;
                public string INNEME;
                public string INEGYE;
                public string INVEEN;

                public int INSOSZ;
                public int INCSSZ;
                public string ITMEGN;
                public string KOMEGN;

                public VERSENYZOADATOK(string _VEAZON, Eredmény _eredmeny )
                {
                    Induló? indulo = Program.database.Induló( _eredmeny.név );
                    List<Íjtípus> ijtipusok = Program.database.Íjtípusok( );

                    ITMEGN = null;
                    INSOSZ = (int)_eredmeny.sorszám;
                    INCSSZ = _eredmeny.csapat;
                    INNEVE = _eredmeny.név;
                    INBEEK = Program.database.InduloKora(_VEAZON,_eredmeny.név).ToString();
                    INNEME = indulo.Value.nem;    
                    INEGYE = indulo.Value.egyesület;
                    INVEEN = indulo.Value.engedély;
                    KOMEGN = _eredmeny.KorosztalyAzonosito;

                    foreach ( Íjtípus ijtipus in ijtipusok )
                    {
                        if( ijtipus.azonosító == _eredmeny.íjtípus )
                        {
                            ITMEGN = ijtipus.megnevezés;
                            break;
                        }
                    }
                }

            }

            public BEIROLAP(string _VEAZON, Eredmény _eredmeny) : this()
            {
                VersenyAdatok = new VERSENYADATOK( _VEAZON );
                VersenyzoAdatok = new VERSENYZOADATOK( _VEAZON, _eredmeny );
            }
            public VERSENYADATOK VersenyAdatok;
            public VERSENYZOADATOK VersenyzoAdatok;
        }

        public class Node_Eredménylap_Verseny_Teljes
        {
            public struct Versenyadatok
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEINSZ;
                public string VSAZON;
                public string VSMEGN;
                public Versenyadatok(Verseny _verseny, Versenysorozat _versenysorozat)
                {
                    VEAZON = _verseny.Azonosito;
                    VEMEGN = _verseny.Megnevezes;
                    VEDATU = _verseny.Datum;
                    VEOSPO = _verseny.Osszes;
                    VEINSZ = _verseny.Indulok;
                    VSAZON = _versenysorozat.azonosító;
                    VSMEGN = _versenysorozat.megnevezés;
                }
                public Versenyadatok(Verseny _verseny)
                {
                    VEAZON = _verseny.Azonosito;
                    VEMEGN = _verseny.Megnevezes;
                    VEDATU = _verseny.Datum;
                    VEOSPO = _verseny.Osszes;
                    VEINSZ = _verseny.Indulok;
                    VSAZON = null;
                    VSMEGN = null;
                }
            }

            public struct Node_Íjtípus
            {
                public Node_Íjtípus(string _azon, string _megn)
                {

                    azonosito = _azon;
                    megnevezés = _megn;
                    korosztályok = new List<Node_Korosztály>();
                    ijtipus_vanbenne = false;
                }

                public bool ijtipus_vanbenne;
                public void Vanbenne() { ijtipus_vanbenne = true; }
                public string azonosito;
                public string megnevezés;
                public List<Node_Korosztály> korosztályok;
                public struct Node_Korosztály
                {
                    public Node_Korosztály(int _also, int _felso, string _azonosito, string _megnevezés)
                    {
                        ferfiak = new List<Node_Indulo>();
                        nok = new List<Node_Indulo>();
                        also = _also;
                        felso = _felso;
                        korosztály_vanbenne = false;
                        kazon = _azonosito;
                        kmegn = _megnevezés;

                    }
                    public string kazon;
                    public string kmegn;
                    public int also;
                    public int felso;
                    public bool korosztály_vanbenne;
                    public void Vanbenne2() { korosztály_vanbenne = true; }
                    public List<Node_Indulo> ferfiak;
                    public List<Node_Indulo> nok;
                    public struct Node_Indulo
                    {
                        public Node_Indulo(string _nev, int _pont, string _nem, int _sorszam, int _kor, string _egyesulet, string _ijazon, int _szazalek, int _tiztalalat)
                        {
                            nev = _nev;
                            sorszam = _sorszam;
                            egyesulet = _egyesulet;
                            kor = _kor;
                            nem = _nem;
                            pont = _pont;
                            ijazon = _ijazon;
                            szazalek = _szazalek;
                            tiztalalat = _tiztalalat;
                            holtverseny = false;
                        }
                        public string nev;
                        public int sorszam;
                        public string egyesulet;
                        public int kor;
                        public string nem;
                        public int pont;
                        public string ijazon;
                        public int szazalek;
                        public int tiztalalat;
                        public bool holtverseny;

                    }
                }
            }

            public Versenyadatok versenyadatok;
            public List<Node_Íjtípus> íjtípus;
        }
      
        public struct Node_Eredménylap_VersenySorozat_Teljes
        {
            public struct Node_Íjtípus
            {
                public Node_Íjtípus(string _azon, string _megn)
                {
                    azonosito = _azon;
                    megnevezés = _megn;
                    korosztályok = new List<Node_Korosztály>();
                    ijtipus_vanbenne = false;
                }

                public bool ijtipus_vanbenne;
                public void Vanbenne() { ijtipus_vanbenne = true; }
                public string azonosito;
                public string megnevezés;
                public List<Node_Korosztály> korosztályok;
                public struct Node_Korosztály
                {
                    public Node_Korosztály(int _also, int _felso, string _azonosito, string _megnevezés)
                    {
                        ferfiak = new List<Node_Indulo>();
                        nok = new List<Node_Indulo>();
                        also = _also;
                        felso = _felso;
                        korosztály_vanbenne = false;
                        kazon = _azonosito;
                        kmegn = _megnevezés;

                    }
                    public string kazon;
                    public string kmegn;
                    public int also;
                    public int felso;
                    public bool korosztály_vanbenne;
                    public void Vanbenne2() { korosztály_vanbenne = true; }
                    public List<Node_Indulo> ferfiak;
                    public List<Node_Indulo> nok;
                    public struct Node_Indulo
                    {
                        public Node_Indulo(string _nev,string _nem, int _sorszam, int _kor, string _egyesulet, string _ijazon, int _pont, int _szazalek, int _tizpont)
                        {
                            nev = _nev;
                            sorszam = _sorszam;
                            egyesulet = _egyesulet;
                            kor = _kor;
                            nem = _nem;
                            ijazon = _ijazon;
                            eredmények = new List<Node_Eredmény>() ;
                            eredmények.Add( new Node_Eredmény(_pont,_szazalek,_tizpont));
                            holtverseny = false;
                        }
                        public void Holtverseny() { holtverseny = true; }
                        public string nev;
                        public int sorszam;
                        public string egyesulet;
                        public int kor;
                        public string nem;
                        public string ijazon;
                        public List<Node_Eredmény> eredmények;
                        public bool holtverseny;
                    }
                    public struct Node_Eredmény
                    {
                        public Node_Eredmény( int _pont, int _szazalek, int _tizpont )
                        {
                            pont = _pont;
                            szazalek = _szazalek;
                            tizpont = _tizpont;
                        }
                        public int pont;
                        public int szazalek;
                        public int tizpont;
                    }
                }
            }
            public List<Node_Íjtípus> íjtípus;
            public List<string> versenyazonosítók;
        }
        
        public struct EGYESULETADAT
        {
            public int Helyezes;
            public string Nev;
            public string Cim;
            public int OsszPont;

            public EGYESULETADAT( int _Helyezes, string _Nev, string _Cim, int _OsszPont )
            {
                Helyezes = _Helyezes;
                Nev = _Nev;
                Cim = _Cim;
                OsszPont = _OsszPont;
            }
        }

        public struct EREDMENYLAPVERSENYEGYESULET
        {
            public struct VERSENYADATOK
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public string VSAZON;
                public string VSMEGN;

                public VERSENYADATOK( string _VEAZON ): this( )
                {
                    List<Verseny> versenyek = Program.database.Versenyek();
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

                    foreach (Verseny verseny in versenyek)
                    {
                        if (verseny.Azonosito == _VEAZON)
                        {
                            VEAZON = verseny.Azonosito;
                            VEMEGN = verseny.Megnevezes;
                            VEDATU = verseny.Datum;
                            VEOSPO = verseny.Osszes;
                            VSAZON = verseny.VersenySorozat;
                        }
                    }
                    foreach (Versenysorozat versenysorozat in versenysorozatok)
                    {
                        if(versenysorozat.azonosító == VSAZON)
                        VSMEGN = versenysorozat.megnevezés;
                    }
                }
            }

            public List<EGYESULETADAT> Egyesuletek;
            public VERSENYADATOK VersenyAdatok;

            public EREDMENYLAPVERSENYEGYESULET( string _VEAZON )
            {
                Egyesuletek = Program.database.EredmenylapVersenyEgyesulet(_VEAZON);
                VersenyAdatok = new VERSENYADATOK( _VEAZON ); 
            }
        }

        public struct EREDMENYLAPVERSENYSOROZATEGYESULET
        {
            public string Azonosito;
            public string Megnevezes;
            public List<EGYESULETADAT> Egyesuletek;
            

            public EREDMENYLAPVERSENYSOROZATEGYESULET(string _VSAZON) :this()
            {
                Versenysorozat tmpvs = Program.database.Versenysorozat(_VSAZON).Value;
                Azonosito = _VSAZON;
                Megnevezes = tmpvs.megnevezés;

                Egyesuletek = new List<EGYESULETADAT>();
                List<Egyesulet> egyes = Program.database.Egyesuletek();
                foreach (Egyesulet item in egyes)
                {
                    if(item.Listazando==true)
                    Egyesuletek.Add(new EGYESULETADAT(0,item.Azonosito,item.Cim,0));
                }

               List<List<EGYESULETADAT>> temp = new List<List<EGYESULETADAT>>();

                List<Verseny> versenyek = Program.database.Versenyek();
                for( int i = versenyek.Count-1; i>=0;i-- )
                {
                    if(versenyek[i].VersenySorozat==_VSAZON)
                    {
                        temp.Add( Program.database.EredmenylapVersenyEgyesulet(versenyek[i].Azonosito) );
                    }
                }
                EGYESULETADAT[] Egyesuletek2 = Egyesuletek.ToArray();

                 foreach (List<EGYESULETADAT> item in temp)
                 {
                    foreach (EGYESULETADAT item2 in item)
                    {
                        for(int i=0;i<Egyesuletek2.Length;i++)
                        {
                            if(item2.Nev == Egyesuletek2[i].Nev)
                            {
                                Egyesuletek2[i].OsszPont += item2.OsszPont;
                            }
                        }
                    }
                 }
                 Egyesuletek = new List<EGYESULETADAT>( Egyesuletek2 );

                for (int i = 0; i < Egyesuletek.Count; i++)
                {
                    for (int j = 0; j < Egyesuletek.Count; j++)
                    {
                        if(Egyesuletek[i].OsszPont>Egyesuletek[j].OsszPont)
                        {
                            EGYESULETADAT temp3 = Egyesuletek[i];
                            Egyesuletek[i] = Egyesuletek[j];
                            Egyesuletek[j] = temp3;
                        }
                    }
                }
            }
        }

        public struct EREDMENYLAPVERSENYSOROZATRESZLETES
        {
            public string Azonosito;
            public string Megnevezes;
            public int VersenyekSzama;
            public List<string> VersenyAzonositok;

            public struct INDULO
            {
                public string Ijtipus;
                public string Korosztaly;
                public string Nem;
                public string Nev;
                public string Egyesulet;
                public List<EREDMENY> Eredmenyek;
                public int OsszPont;
                public string Verseny;

                public INDULO( string _Ijtipus, 
                                string _Korosztaly, 
                                string _Nem, 
                                string _Nev, 
                                string _Egyesulet, 
                                int _OsszPont,
                                List<EREDMENY> _Eredmenyek,
                                string _Verseny)
                {
                    Ijtipus = _Ijtipus;
                    Korosztaly = _Korosztaly;
                    Nem = _Nem;
                    Nev = _Nev;
                    Egyesulet = _Egyesulet;
                    OsszPont = _OsszPont;
                    Eredmenyek = _Eredmenyek;
                    Verseny = _Verseny;
                }
            }

            public struct IJTIPUS
            {
                public string Azonosito;
                public string Megnevezes;
                public List<KOROSZTALY> Korosztalyok;

                public IJTIPUS( string _VSAZON, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO )
                {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Korosztalyok = new List<KOROSZTALY>( );
                    //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                    List<Verseny> versenyek = Program.database.Versenyek();
                    foreach (Verseny item in versenyek)
                    {
                        if (item.VersenySorozat == _VSAZON)
                        {
                            List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                            foreach (Korosztály item2 in temp)
                            {
                                Korosztalyok.Add( new KOROSZTALY( _Azonosito, item2.azonosító, item2.megnevezés, _OSSZESINDULO ) );
                            }
                        }
                    }

                }

            }

            public struct KOROSZTALY
            {
                public string Azonosito;
                public string Megnevezes;
                public INDULOK Indulok;

                public KOROSZTALY( string _Ijtipus, string _Azonosito, string _Megnevezes, List<INDULO> _OSSZESINDULO )
                {
                    Azonosito = _Azonosito;
                    Megnevezes = _Megnevezes;
                    Indulok = new INDULOK( );
                    Indulok.Ferfiak = Ferfiak(_Ijtipus, _Azonosito, _OSSZESINDULO);
                    Indulok.Nok = Nok(_Ijtipus, _Azonosito, _OSSZESINDULO);
                }
            }

            public struct INDULOK
            {
                public List<INDULO> Nok;
                public List<INDULO> Ferfiak; 
            }

            public struct EREDMENY
            {
                public string Verseny;
                public int Pont;
                public EREDMENY( string _Verseny, int _Pont )
                {
                    Verseny = _Verseny;
                    Pont = _Pont;
                }
            }

            public List<IJTIPUS>
            Ijtipusok( string _VSAZON, List<INDULO> _Indulok )
            {

                List<IJTIPUS> Data = new List<IJTIPUS>();
                //lekérni az összes ijipust
                List<Íjtípus> IjtipusokSeged = Program.database.Íjtípusok();
                //kitörölni ami nincs
                for (int i = IjtipusokSeged.Count - 1; i >= 0; i--)
                {
                    bool found = false;
                    for (int j = 0; j < _Indulok.Count; j++)
                    {
                        if (IjtipusokSeged[i].azonosító == _Indulok[j].Ijtipus)
                        {
                            found = true;
                        }
                    }
                    if (!found) { IjtipusokSeged.RemoveAt(i); }
                }
                //a versenysorozaton szereplőket visszaadni
                foreach (Íjtípus ijtipus in IjtipusokSeged)
                {
                    IJTIPUS temp = new IJTIPUS( _VSAZON, ijtipus.azonosító, ijtipus.megnevezés, _Indulok );
                    Data.Add(temp);
                }
                return Data;
            }

            public List<KOROSZTALY>
            Korosztalyok( string _Ijtipus, string _VSAZON, List<INDULO> _OSSZESINDULO )
            {
                List<KOROSZTALY> Data = new List<KOROSZTALY>( );
                //végigmenni az összes versenyen, minden íjtípushoz hozzáadni a korosztályokat
                List<Verseny> versenyek = Program.database.Versenyek();
                foreach (Verseny item in versenyek)
                {
                    if (item.VersenySorozat == _VSAZON)
                    {
                        List<Korosztály> temp = Program.database.Korosztályok(item.Azonosito);
                        foreach (Korosztály item2 in temp)
                        {
                            Data.Add( new KOROSZTALY( _Ijtipus, item2.azonosító, item2.megnevezés, _OSSZESINDULO ) );    
                        }
                    }
                }
                return Data;
            }

            public static List<INDULO>
            Ferfiak( string _Ijtipus, string _Korosztaly, List<INDULO> _Indulok )
            {
                List<INDULO> Data = new List<INDULO>( );

                for(int i= _Indulok.Count-1;i>=0;i--)
                {
                    bool found = false;
                    if( _Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "F")
                    {
                        for(int j = 0; j< Data.Count;j++)
                        {
                            if( Data[j].Nev == _Indulok[i].Nev )
                            {
                                //hozzáadni az eredmenyt
                               found = true;
                            }
                        }
                        if(!found)
                        {
                            Data.Add(_Indulok[i]);
                            _Indulok.RemoveAt(i);
                        }

                    }
                }

                return Data;
            }

            public static List<INDULO>
            Nok( string _Ijtipus, string _Korosztaly, List<INDULO> _Indulok )
            {
                List<INDULO> Data = new List<INDULO>( );

                for (int i = _Indulok.Count - 1; i >= 0; i--)
                {
                    if (_Indulok[i].Ijtipus == _Ijtipus &&
                        _Indulok[i].Korosztaly == _Korosztaly &&
                        _Indulok[i].Nem == "N")
                    {
                        Data.Add(_Indulok[i]);
                        _Indulok.RemoveAt(i);
                    }
                }
                return Data;
            }

            public 
            EREDMENYLAPVERSENYSOROZATRESZLETES(string _VSAZON) :this()
            {
                Azonosito = _VSAZON;
                Versenysorozat temp = Program.database.Versenysorozat(_VSAZON).Value;
                Megnevezes = temp.megnevezés;
                VersenyekSzama = temp.versenyek;

                VersenyAzonositok = new List<string>();

                
                List<Verseny> versenyek = Program.database.Versenyek();

                IComparer<Verseny> Compare = new OrderVerseny();
                versenyek.Sort(Compare);
                foreach (Verseny item in versenyek)
	            {
		            if(item.VersenySorozat == _VSAZON)
                    {
                        VersenyAzonositok.Add(item.Azonosito);
                    }
	            }
                OsszesIndulo = Seged1( _VSAZON );
                IjTipusok = Ijtipusok( _VSAZON, OsszesIndulo );

                IComparer<INDULO> Comparer = new Order( );

                foreach ( IJTIPUS ijtipus in IjTipusok )
                {
                    foreach ( KOROSZTALY korosztaly in ijtipus.Korosztalyok )
                    {
                        korosztaly.Indulok.Ferfiak.Sort(Comparer);
                        korosztaly.Indulok.Nok.Sort(Comparer);
                    }
                }

            }

            #region Seged
            public List<INDULO> Seged1( string _VSAZON )
            {
                OsszesIndulo = new List<INDULO>( );
                List<Verseny> Versenyek = Program.database.Versenyek( );
                foreach ( Verseny verseny in Versenyek )
                {
                    if ( verseny.VersenySorozat == _VSAZON )
                    {
                        List<INDULO> temp = Seged2( verseny.Azonosito );
                        OsszesIndulo.AddRange( temp );

                    }
                }


                for ( int i = OsszesIndulo.Count - 1; i >= 0; i-- )
                {
                    for ( int j = OsszesIndulo.Count - 1; j >= 0; j-- )
                    {
                        if ( OsszesIndulo[i].Verseny != OsszesIndulo[j].Verseny &&
                            OsszesIndulo[i].Nev == OsszesIndulo[j].Nev &&
                            OsszesIndulo[i].Korosztaly == OsszesIndulo[j].Korosztaly &&
                            OsszesIndulo[i].Ijtipus == OsszesIndulo[j].Ijtipus )
                        {
                            foreach ( EREDMENY item in OsszesIndulo[j].Eredmenyek )
                            {
                                OsszesIndulo[i].Eredmenyek.Add( item );
                            }
                            OsszesIndulo.RemoveAt( j );
                        }
                    }
                }
                for ( int i = OsszesIndulo.Count - 1; i >= 0; i-- )
                {
                    int sum = 0;
                    foreach ( EREDMENY item in OsszesIndulo[i].Eredmenyek )
                    {
                        sum += item.Pont;
                    }

                    INDULO tmp = OsszesIndulo[i];
                    tmp.OsszPont = sum;
                    OsszesIndulo.RemoveAt( i );
                    OsszesIndulo.Add( tmp );

                }
                return OsszesIndulo;

            }

            public List<INDULO> Seged2( string _VEAZON )
            {
                List<INDULO> temp_indulok = new List<INDULO>( );

                List<Induló> indulók = Program.database.Indulók( );
                List<Eredmény> eredmények = Program.database.Eredmények( _VEAZON );
                List<Korosztály> korosztályok = Program.database.Korosztályok( _VEAZON );

                foreach ( Eredmény eredmeny in eredmények )
                {
                    Verseny verseny = Program.database.Verseny( _VEAZON ).Value;
                    foreach ( Induló indulo in indulók )
                    {
                        foreach ( Korosztály korosztaly in korosztályok )
                        {
                            if ( eredmeny.név == indulo.név )
                            {
                                if ( BetoltottEletkor( verseny.Datum, indulo.születés ) >= korosztaly.alsó_határ &&
                                    BetoltottEletkor( verseny.Datum, indulo.születés ) <= korosztaly.felső_határ
                                    )
                                {
                                    if ( indulo.nem == "F" && korosztaly.férfiak == true )
                                    {

                                        EREDMENY tempEredmeny = new EREDMENY( verseny.Azonosito, eredmeny.összpont.Value );
                                        List<EREDMENY> tempEredmenyek = new List<EREDMENY>( );
                                        tempEredmenyek.Add( tempEredmeny );
                                        INDULO temp = new INDULO( eredmeny.íjtípus,
                                                                    korosztaly.azonosító,
                                                                    indulo.nem,
                                                                    indulo.név,
                                                                    indulo.egyesület,
                                                                    0,
                                                                    tempEredmenyek,
                                                                    verseny.Azonosito );
                                        temp_indulok.Add( temp );
                                    }
                                    else if ( indulo.nem == "N" && korosztaly.nők == true )
                                    {

                                        EREDMENY tempEredmeny = new EREDMENY( verseny.Azonosito, eredmeny.összpont.Value );
                                        List<EREDMENY> tempEredmenyek = new List<EREDMENY>( );
                                        tempEredmenyek.Add( tempEredmeny );
                                        INDULO temp = new INDULO( eredmeny.íjtípus,
                                                                    korosztaly.azonosító,
                                                                    indulo.nem,
                                                                    indulo.név,
                                                                    indulo.egyesület,
                                                                    eredmeny.összpont.Value,
                                                                    tempEredmenyek,
                                                                    verseny.Azonosito );
                                        temp_indulok.Add( temp );
                                    }
                                }
                            }
                        }
                    }
                }

                return temp_indulok;
            }

            #region Compare
            public class Order : IComparer<INDULO>
            {
                public int Compare( INDULO i1, INDULO i2 )
                {
                    int z = i1.OsszPont.CompareTo( i2.OsszPont );
                    return ( -1 * z );
                }
            }

            public class OrderVerseny : IComparer<Verseny>
            {
                public int Compare( Verseny i1, Verseny i2 )
                {
                    int z = i1.Datum.CompareTo( i2.Datum );
                    return z;
                }
            }
            #endregion

            #endregion
            // a versenysorozat összes indulója versenyenként, azaz többször is
            public List<INDULO> OsszesIndulo;
            public List<IJTIPUS> IjTipusok;
        }



        static public string NyomtatCsapatlista(string _VEAZON)
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
            document.AddHeaders();
            PageNumber( document );
            
            #region címbekezdés

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(HeadLine);
            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés + "\n");
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region header

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            paragraph_1.Append(VersenyAzonosito);

            paragraph_1.Append( _VEAZON + ", " + Data.VersenyAdatok.VEMEGN );
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + VersenyIdo);
            paragraph_1.Append( Data.VersenyAdatok.VEDATU );
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + VersenyOsszPont);
            paragraph_1.Append( ( Data.VersenyAdatok.VEOSPO * 10 ).ToString( ) );
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + VersenyIndulokSzama);
            paragraph_1.Append( Data.VersenyAdatok.VEINSZ.ToString( ) );
            paragraph_1.Bold();
            paragraph_1.Append("\n" + VersenySorozat);
            paragraph_1.Append( Data.VersenyAdatok.VSAZON + ", " + Data.VersenyAdatok.VSMEGN );
            paragraph_1.Bold();
            paragraph_1.AppendLine();
            #endregion

            #region Data
            for ( int i = 0; i < Data.Csapatok.Count; i++ )
            {
                Table table = document.AddTable( Data.Csapatok[i].versenyzoadatok.Count + 1, 6 );
                table.Alignment = Alignment.center;

                table.Rows[0].Cells[0].Paragraphs[0].Append( "Csapat" ).Bold( );
                table.Rows[0].Cells[1].Paragraphs[0].Append( "Sorszám" ).Bold( );
                table.Rows[0].Cells[2].Paragraphs[0].Append( "Név" ).Bold( );
                table.Rows[0].Cells[3].Paragraphs[0].Append( "Íjtípus" ).Bold( );
                table.Rows[0].Cells[4].Paragraphs[0].Append( "Kor" ).Bold( );
                table.Rows[0].Cells[5].Paragraphs[0].Append( "Egyesület" ).Bold( );


                int q = 1;
                foreach ( CSAPATLISTA.VERSENYZOADAT versenyzo in Data.Csapatok[i].versenyzoadatok )
                {
                    table.Rows[q].Cells[0].Paragraphs[0].Append( versenyzo.INCSSZ.ToString( ) );
                    table.Rows[q].Cells[1].Paragraphs[0].Append( versenyzo.INSOSZ.ToString( ) );
                    table.Rows[q].Cells[2].Paragraphs[0].Append( versenyzo.INNEVE );
                    table.Rows[q].Cells[3].Paragraphs[0].Append( versenyzo.ITMEGN );
                    table.Rows[q].Cells[4].Paragraphs[0].Append( versenyzo.INSZUL.ToString( ) );
                    table.Rows[q].Cells[5].Paragraphs[0].Append( versenyzo.INEGYE );
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

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "CSAPATLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            return FileName;
        }

        static public string NyomtatBeirolap(string _VEAZON, Eredmény _eredmény) 
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

            if (Data.VersenyAdatok.VSAZON != null)
            {
                FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "BEIRLAP.docx";
            }
            else
            {
                 FileName = _VEAZON + "\\" + "BEIRLAP.docx";
            }

            var document = DocX.Create(FileName);
            document.MarginBottom = 10;
            document.AddHeaders();

            #region Title

            var titleFormat = new Formatting();
            titleFormat.Size = 10D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(HeadLine);
            title.Alignment = Alignment.center;
            title.AppendLine(Program.Tulajdonos_Megnevezés + "\n");
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region Data

            Table table = document.AddTable(Data.VersenyAdatok.VEALSZ + 3, 8);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Sorszám").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append( "Lőállás" ).Bold( );
            table.Rows[0].Cells[2].Paragraphs[0].Append( "10 pont" ).Bold( );
            table.Rows[0].Cells[3].Paragraphs[0].Append( "8 pont" ).Bold( );
            table.Rows[0].Cells[4].Paragraphs[0].Append( "5 pont" ).Bold( );
            table.Rows[0].Cells[5].Paragraphs[0].Append( "Mellé" ).Bold( );
            table.Rows[0].Cells[6].Paragraphs[0].Append( "Összesen" ).Bold( );
            table.Rows[0].Cells[7].Paragraphs[0].Append( "Göngyölt" ).Bold( );

            for (int i = 1; i <= Data.VersenyAdatok.VEALSZ; i++)
            {
                table.Rows[i].Cells[0].Paragraphs[0].Append((i).ToString());
            }

            table.Rows[Data.VersenyAdatok.VEALSZ + 1].Cells[1].Paragraphs[0].Append( "Össz darab" ).Bold( );
            table.Rows[Data.VersenyAdatok.VEALSZ + 2].Cells[1].Paragraphs[0].Append( "Össz pont" ).Bold( );
            #endregion

            #region Header

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = document.InsertParagraph(VersenyAzonosito, false, titleFormat2);

            Table header_table = document.AddTable(4,3);


            header_table.Rows[0].Cells[0].Paragraphs[0].Append(Sorszam);
            titleFormat2.Size = 18D;
            header_table.Rows[0].Cells[0].Paragraphs[0].InsertText(Data.VersenyzoAdatok.INSOSZ.ToString(),false,titleFormat2);

            header_table.Rows[0].Cells[1].Paragraphs[0].Append(Csapat);
            header_table.Rows[0].Cells[1].Paragraphs[0].Append(Data.VersenyzoAdatok.INCSSZ.ToString()).Bold();

            header_table.Rows[1].Cells[0].Paragraphs[0].Append(Nev);
            titleFormat2.Size = _eredmény.név.Length > 20 ? 14D : 18D;
            header_table.Rows[1].Cells[0].Paragraphs[0].InsertText(_eredmény.név.ToString(), false, titleFormat2);

            header_table.Rows[1].Cells[1].Paragraphs[0].Append(Kor);
            header_table.Rows[1].Cells[1].Paragraphs[0].Append(Data.VersenyzoAdatok.INBEEK.ToString()).Bold();

            header_table.Rows[1].Cells[2].Paragraphs[0].Append(Nem);
            header_table.Rows[1].Cells[2].Paragraphs[0].Append(Data.VersenyzoAdatok.INNEME.ToString()).Bold();

            header_table.Rows[2].Cells[0].Paragraphs[0].Append(Egyesulet);
            header_table.Rows[2].Cells[0].Paragraphs[0].Append(Data.VersenyzoAdatok.INEGYE.ToString()).Bold();

            header_table.Rows[2].Cells[1].Paragraphs[0].Append(Engedely);
            header_table.Rows[2].Cells[1].Paragraphs[0].Append(Data.VersenyzoAdatok.INVEEN.ToString()).Bold();

            header_table.Rows[3].Cells[0].Paragraphs[0].Append(Ijtipus);
            header_table.Rows[3].Cells[0].Paragraphs[0].Append(Data.VersenyzoAdatok.ITMEGN.ToString()).Bold();

            header_table.Rows[3].Cells[1].Paragraphs[0].Append(Korosztaly);
            header_table.Rows[3].Cells[1].Paragraphs[0].Append(Data.VersenyzoAdatok.KOMEGN.ToString()).Bold();

            header_táblázat_formázása(header_table);
            document.InsertTable(header_table);
            Paragraph temp = document.InsertParagraph();


            paragraph_1.Append(_VEAZON + ", " + Data.VersenyAdatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + VersenyIdo);
            paragraph_1.Append(Data.VersenyAdatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + VersenyOsszPont);
            paragraph_1.Append((Data.VersenyAdatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + VersenySorozat);

            if (Data.VersenyAdatok.VSAZON != null)
            {
                paragraph_1.Append(Data.VersenyAdatok.VSAZON + ", " + Data.VersenyAdatok.VSMEGN);
            }
            paragraph_1.Bold();
            paragraph_1.AppendLine();
            #endregion

            beirlap_táblázat_formázás(table);
            document.InsertTable(table);

            Paragraph paragraph_3 = document.InsertParagraph(Alairas1, false, titleFormat2);
            paragraph_3.AppendLine(Alairas2);

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "BEIRLAP.DOCX ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return FileName;
        }

        static public string NyomtatNevezesiLista( string _VEAZON, bool _NemMegjelentNyomtat )
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
            FooterTable.Rows[0].Cells[1].Paragraphs[0].Append( "1. oldal" );
            FooterTable.AutoFit = AutoFit.ColumnWidth;
            FooterTable.Rows[0].Cells[0].Width = document.PageWidth - 200;
            FooterTable.Rows[0].Cells[1].Width = 60;

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

            HeaderTable.Rows[0].Cells[0].Paragraphs[0].Append( "Sorszám" );
            HeaderTable.Rows[0].Cells[1].Paragraphs[0].Append( "Név" );
            HeaderTable.Rows[0].Cells[2].Paragraphs[0].Append( "Íjtípus" );
            HeaderTable.Rows[0].Cells[3].Paragraphs[0].Append( "Kor" );
            HeaderTable.Rows[0].Cells[4].Paragraphs[0].Append( "Egyesület" );
            HeaderTable.Rows[0].Cells[5].Paragraphs[0].Append( "Csapat" );

            NevezesiListaTablazatFormazas( HeaderTable );
            TablazatFejlec.InsertTable( HeaderTable );
            
            #endregion


            #region táblázat formázás

            Table table = document.AddTable( Data.VersenyAdatok.VEINSZ + 1, 6 );

            table.Rows[0].Cells[0].Paragraphs[0].Append( "Sorszám" );
            table.Rows[0].Cells[1].Paragraphs[0].Append( "Név" );
            table.Rows[0].Cells[2].Paragraphs[0].Append( "Íjtípus" );
            table.Rows[0].Cells[3].Paragraphs[0].Append( "Kor" );
            table.Rows[0].Cells[4].Paragraphs[0].Append( "Egyesület" );
            table.Rows[0].Cells[5].Paragraphs[0].Append( "Csapat" );
            #endregion

            #region táblázat adatok betöltése

            for ( int i = 0; i < Data.VersenyAdatok.VEINSZ; i++ )
            {
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append( Data.VersenyzoAdatok[i].INSOSZ.ToString( ) );
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append( Data.VersenyzoAdatok[i].INNEVE );
                table.Rows[i + 1].Cells[2].Paragraphs[0].Append( Data.VersenyzoAdatok[i].ITMEGN );
                table.Rows[i + 1].Cells[3].Paragraphs[0].Append( ( Data.VersenyzoAdatok[i].INSZUL.ToString( ) ) );
                table.Rows[i + 1].Cells[4].Paragraphs[0].Append( Data.VersenyzoAdatok[i].INEGYE );
                table.Rows[i + 1].Cells[5].Paragraphs[0].Append( Data.VersenyzoAdatok[i].INCSSZ.ToString( ) );
            }
            #endregion

            NevezesiListaTablazatFormazas( table );
            document.InsertTable( table );

            try { document.Save( ); }
            catch ( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "NEVEZLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
            return FileName;

        }


        static public string nyomtat_eredmenylap_verseny_teljes(string _VEAZON)
        {
            string filename = null;

            Node_Eredménylap_Verseny_Teljes verseny = new Node_Eredménylap_Verseny_Teljes();
            verseny.íjtípus = new List<Node_Eredménylap_Verseny_Teljes.Node_Íjtípus>();

            #region alap stringek
            string headline = "EREDMÉNYLAP";
            string típus = "***TELJES***";
            string st_vazon_vnev = "Verseny azonosítója, neve: ";
            string st_ido = "Verseny ideje: ";
            string st_vosszp = "Verseny összpontszáma: ";
            string st_insz = "Indulók száma: ";
            string st_vsorazon = "Versenysorozat azonosítója, neve: ";

            ijtipus_sorszam = 1;
            korosztaly_sorszam = 1;
            #endregion

            #region adat

            List<Verseny> versenyek = Program.database.Versenyek();
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Verseny outer in versenyek)
            {
                if (outer.Azonosito == _VEAZON && versenysorozatok.Count != 0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító == outer.VersenySorozat)
                        {
                            verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.Azonosito == _VEAZON)
                {
                    verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer);
                }
            }

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            foreach (Íjtípus item_íjtípusok in íjtípusok)
            {
                verseny.íjtípus.Add(new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus(item_íjtípusok.azonosító, item_íjtípusok.megnevezés));
            }

            List<Korosztály> korosztályok = Program.database.Korosztályok(_VEAZON);
            foreach (Korosztály item_korosztályok in korosztályok)
            {
                foreach (Node_Eredménylap_Verseny_Teljes.Node_Íjtípus item in verseny.íjtípus)
                {
                    item.korosztályok.Add(new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály(item_korosztályok.alsó_határ, item_korosztályok.felső_határ, item_korosztályok.azonosító, item_korosztályok.megnevezés));
                }
            }

            List<Induló> indulók = Program.database.Indulók();
            List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);

            foreach (Induló item_induló in indulók)
            {
                foreach (Eredmény item_eredmény in eredmények)
                {
                    if (item_induló.név==item_eredmény.név )
                    {
                        if (item_eredmény.megjelent == true)
                        {
                            int year = Convert.ToInt32((new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(item_induló.születés))).Year - 1);
                            Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo(
                                item_induló.név, (int)item_eredmény.összpont, item_induló.nem, (int)item_eredmény.sorszám, year, item_induló.egyesület, item_eredmény.íjtípus, (int)item_eredmény.százalék, (int)item_eredmény.találat_10);

                            for (int i = 0; i < verseny.íjtípus.Count; i++)
                            {
                                if (verseny.íjtípus[i].azonosito == temp.ijazon)
                                {
                                    for (int j = 0; j < verseny.íjtípus[i].korosztályok.Count; j++)
                                    {
                                        if (verseny.íjtípus[i].korosztályok[j].also <= temp.kor && verseny.íjtípus[i].korosztályok[j].felso >= temp.kor)
                                        {
                                            if (temp.nem == "F")
                                            {
                                                verseny.íjtípus[i].korosztályok[j].ferfiak.Add(temp);
                                                verseny.íjtípus[i].Vanbenne();
                                                verseny.íjtípus[i].korosztályok[j].Vanbenne2();
                                            }
                                            else
                                            {
                                                verseny.íjtípus[i].korosztályok[j].nok.Add(temp);
                                                verseny.íjtípus[i].Vanbenne();
                                                verseny.íjtípus[i].korosztályok[j].Vanbenne2();
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            verseny.versenyadatok.VEINSZ--;
                        }
                    }

                }
            }
            rendez_verseny_pont_alapján(verseny);

            #endregion

            if (verseny.versenyadatok.VSAZON!=null)
            {
                filename = verseny.versenyadatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVETELJ.docx";
            }
            else
            {
                filename = _VEAZON + "\\" + "ERLAPVETELJ.docx";
            }
            var document = DocX.Create(filename);
            document.AddHeaders();
            PageNumber( document );
            #region header

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(headline);
            title.AppendLine(típus);
            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés);
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            paragraph_1.AppendLine(st_vazon_vnev);

            paragraph_1.Append(_VEAZON + "," + verseny.versenyadatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + st_ido);
            paragraph_1.Append(verseny.versenyadatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t" + st_vosszp);
            paragraph_1.Append((verseny.versenyadatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\t" + st_insz);
            paragraph_1.Append(verseny.versenyadatok.VEINSZ.ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + st_vsorazon);
            paragraph_1.Append(verseny.versenyadatok.VSAZON + "," + verseny.versenyadatok.VSMEGN);
            paragraph_1.Bold();

            #region formázás

            for (int i = 0; i < verseny.íjtípus.Count; i++)
            {
                Table table = null;
                int ijtipus_count = 0;
                for (int j = 0; j < verseny.íjtípus[i].korosztályok.Count; j++)
                {
                    int korosztaly_count = 0;
                    if (verseny.íjtípus[i].korosztályok[j].ferfiak.Count > 0 || verseny.íjtípus[i].korosztályok[j].nok.Count > 0)
                    {
                        Paragraph adatok = document.InsertParagraph();
                        int ferfiak_count = 0;
                        int nok_count = 0;

                        if (ijtipus_count == 0)
                        {
                            ijtipus_count++;
                        }
                        if (korosztaly_count == 0)
                        {
                              korosztaly_count++;
                        }


                        if (verseny.íjtípus[i].korosztályok[j].nok.Count > 0)
                        {
                            Paragraph np = document.InsertParagraph();
                            if (nok_count == 0)
                            {
                                np.AppendLine("Íjtípus: ");
                                np.Append(verseny.íjtípus[i].megnevezés + "\n");
                                np.Bold();
                                np.Append("    Korosztály: ");
                                np.Append(verseny.íjtípus[i].korosztályok[j].kmegn);
                                np.Bold();
                                np.AppendLine("       Nők:");
                                nok_count++;
                            }

                            for (int q = 0; q < verseny.íjtípus[i].korosztályok[j].nok.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].sorszam + (verseny.íjtípus[i].korosztályok[j].nok[q].holtverseny ? "!" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].nev + (verseny.íjtípus[i].korosztályok[j].nok[q].holtverseny ? " (HOLTVERSENY)" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }

                        if (verseny.íjtípus[i].korosztályok[j].ferfiak.Count > 0)
                        {
                            Paragraph fp = document.InsertParagraph();
                            if (ferfiak_count == 0)
                            {
                                fp.AppendLine("Íjtípus: ");
                                fp.Append(verseny.íjtípus[i].megnevezés + "\n");
                                fp.Bold();
                                fp.Append("    Korosztály: ");
                                fp.Append(verseny.íjtípus[i].korosztályok[j].kmegn);
                                fp.Bold();
                                fp.AppendLine("        Férfiak:");
                                ferfiak_count++;
                            }

                            for (int q = 0; q < verseny.íjtípus[i].korosztályok[j].ferfiak.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].sorszam + (verseny.íjtípus[i].korosztályok[j].ferfiak[q].holtverseny ? "!" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].nev + (verseny.íjtípus[i].korosztályok[j].ferfiak[q].holtverseny ? " (HOLTVERSENY)" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }
                    }
                }

            }
            #endregion


            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVEMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return filename;
        }

        static public string nyomtat_eredmenylap_verseny_misz(string _VEAZON)
        {
            string filename = null;


            Node_Eredménylap_Verseny_Teljes verseny = new Node_Eredménylap_Verseny_Teljes();
            verseny.íjtípus = new List<Node_Eredménylap_Verseny_Teljes.Node_Íjtípus>();

            #region alap stringek
            string headline = "EREDMÉNYLAP";
            string típus = "***MISZ***";
            string st_vazon_vnev = "Verseny azonosítója, neve: ";
            string st_ido = "Verseny ideje: ";
            string st_vosszp = "Verseny összpontszáma: ";
            string st_insz = "Indulók száma: ";
            string st_vsorazon = "Versenysorozat azonosítója, neve: ";

            ijtipus_sorszam = 1;
            korosztaly_sorszam = 1;
            #endregion

            #region adat

            List<Verseny> versenyek = Program.database.Versenyek();
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Verseny outer in versenyek)
            {
                if (outer.Azonosito == _VEAZON && versenysorozatok.Count != 0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító == outer.VersenySorozat)
                        {
                            verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.Azonosito == _VEAZON)
                {
                    verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer);
                }
            }

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            foreach (Íjtípus item_íjtípusok in íjtípusok)
            {
                verseny.íjtípus.Add(new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus(item_íjtípusok.azonosító,item_íjtípusok.megnevezés));
            }

            List<Korosztály> korosztályok = Program.database.Korosztályok(_VEAZON);
            foreach (Korosztály item_korosztályok in korosztályok)
            {
                foreach (Node_Eredménylap_Verseny_Teljes.Node_Íjtípus item in verseny.íjtípus)
                {
                    item.korosztályok.Add(new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály(item_korosztályok.alsó_határ, item_korosztályok.felső_határ, item_korosztályok.azonosító,item_korosztályok.megnevezés));
                }
            }

            List<Induló> indulók = Program.database.Indulók();
            List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);


            foreach (Induló item_induló in indulók)
            {
                foreach (Eredmény item_eredmény in eredmények)
                {
                    if (item_induló.név == item_eredmény.név)
                    {
                        if (item_induló.engedély != "" && item_eredmény.megjelent == true)
                        {
                            int year = Convert.ToInt32((new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(item_induló.születés))).Year - 1);
                            Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = new Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo(
                                item_induló.név, (int)item_eredmény.összpont, item_induló.nem, (int)item_eredmény.sorszám, year, item_induló.egyesület, item_eredmény.íjtípus, (int)item_eredmény.százalék, (int)item_eredmény.találat_10);

                            for (int i = 0; i < verseny.íjtípus.Count; i++)
                            {
                                if (verseny.íjtípus[i].azonosito == temp.ijazon)
                                {
                                    for (int j = 0; j < verseny.íjtípus[i].korosztályok.Count; j++)
                                    {
                                        if (verseny.íjtípus[i].korosztályok[j].also <= temp.kor && verseny.íjtípus[i].korosztályok[j].felso >= temp.kor)
                                        {
                                            if (temp.nem == "F")
                                            {
                                                verseny.íjtípus[i].korosztályok[j].ferfiak.Add(temp);
                                                verseny.íjtípus[i].Vanbenne();
                                                verseny.íjtípus[i].korosztályok[j].Vanbenne2();
                                            }
                                            else
                                            {
                                                verseny.íjtípus[i].korosztályok[j].nok.Add(temp);
                                                verseny.íjtípus[i].Vanbenne();
                                                verseny.íjtípus[i].korosztályok[j].Vanbenne2();
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            verseny.versenyadatok.VEINSZ--;
                        }
                    }
                }
            }
            rendez_verseny_pont_alapján(verseny);
            #endregion

            if (verseny.versenyadatok.VSAZON!=null)
            {
                filename = verseny.versenyadatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVEMISZ.docx";
            }
            else
            {
                filename = _VEAZON + "\\" + "ERLAPVEMISZ.docx";
            
            }

            var document = DocX.Create(filename);
            document.AddHeaders();
            PageNumber( document );
            #region header

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(headline);
            title.AppendLine(típus);

            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés);
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            paragraph_1.AppendLine(st_vazon_vnev);

            paragraph_1.Append(_VEAZON + "," + verseny.versenyadatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + st_ido);
            paragraph_1.Append(verseny.versenyadatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t" + st_vosszp);
            paragraph_1.Append((verseny.versenyadatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\t" + st_insz);
            paragraph_1.Append((verseny.versenyadatok.VEINSZ).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + st_vsorazon);
            paragraph_1.Append(verseny.versenyadatok.VSAZON + "," + verseny.versenyadatok.VSMEGN);
            paragraph_1.Bold();
            #region formázás

            for (int i = 0; i < verseny.íjtípus.Count; i++)
            {
                Table table = null;
                int ijtipus_count = 0;
                for (int j = 0; j < verseny.íjtípus[i].korosztályok.Count; j++)
                {
                    int korosztaly_count = 0;
                    if (verseny.íjtípus[i].korosztályok[j].ferfiak.Count > 0 || verseny.íjtípus[i].korosztályok[j].nok.Count > 0)
                    {
                        Paragraph adatok = document.InsertParagraph();
                        int ferfiak_count = 0;
                        int nok_count = 0;

                        if (ijtipus_count == 0)
                        {
                            adatok.AppendLine("Íjtípus: ");
                            adatok.Append(verseny.íjtípus[i].megnevezés + "\n");
                            adatok.Bold();
                            ijtipus_count++;
                        }
                        if (korosztaly_count == 0)
                        {
                            adatok.Append("    Korosztály: ");
                            adatok.Append(verseny.íjtípus[i].korosztályok[j].kmegn);
                            adatok.Bold();
                            korosztaly_count++;
                        }

                      
                        if (verseny.íjtípus[i].korosztályok[j].nok.Count > 0)
                        {
                            Paragraph np = document.InsertParagraph();
                            if (nok_count == 0)
                            {
                                np.Append("       Nők:");
                                nok_count++;
                            }

                            for (int q = 0; q < verseny.íjtípus[i].korosztályok[j].nok.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].sorszam + (verseny.íjtípus[i].korosztályok[j].nok[q].holtverseny ? "!" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].nev + (verseny.íjtípus[i].korosztályok[j].nok[q].holtverseny ? " (HOLTVERSENY)" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].nok[q].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }

                        if (verseny.íjtípus[i].korosztályok[j].ferfiak.Count > 0)
                        {
                            Paragraph fp = document.InsertParagraph();
                            if (ferfiak_count == 0)
                            {
                                fp.Append("        Férfiak:");
                                ferfiak_count++;
                            }

                            for (int q = 0; q < verseny.íjtípus[i].korosztályok[j].ferfiak.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].sorszam + (verseny.íjtípus[i].korosztályok[j].ferfiak[q].holtverseny ? "!" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].nev + (verseny.íjtípus[i].korosztályok[j].ferfiak[q].holtverseny ? " (HOLTVERSENY)" : ""));
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(verseny.íjtípus[i].korosztályok[j].ferfiak[q].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }
                    }
                }

            }
            #endregion

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVEMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return filename;
        }

        static public string nyomtat_eredmenylap_versenysorozat_teljes(string _VSAZON, int _limit)
        {

            Node_Eredménylap_VersenySorozat_Teljes versenysorozat = versenysorozat_adatok(_VSAZON, _limit, true);
            versenysorozat_kiir(versenysorozat);

            string filename = _VSAZON + "\\" + "ERLAPVSTELJ.docx";
            var document = DocX.Create(filename);
            document.AddHeaders();
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
            title.Append(headline);
            title.AppendLine(típus);

            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés + "\n");
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            //megnevezés
            List<Versenysorozat> vsor = Program.database.Versenysorozatok();

            foreach (Versenysorozat item in vsor)
            {
                if (_VSAZON == item.azonosító)
                {
                    megnevezés = item.megnevezés;
                    break;
                }
            }

            paragraph_1.Append(st_vsorazon);
            paragraph_1.Append(_VSAZON + ", " + megnevezés);
            paragraph_1.Bold();
            paragraph_1.AppendLine();

            #region formázás

            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                Table table = null;
                int ijtipus_count = 0;
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    int korosztaly_count = 0;
                    if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count > 0 || versenysorozat.íjtípus[i].korosztályok[j].nok.Count > 0)
                    {
                        Paragraph adatok = document.InsertParagraph();
                        int ferfiak_count = 0;
                        int nok_count = 0;

                        if (ijtipus_count == 0)
                        {
                            adatok.Append("Íjtípus: ");
                            adatok.Append(versenysorozat.íjtípus[i].megnevezés + "\n");
                            adatok.Bold();
                            ijtipus_count++;
                        }
                        if (korosztaly_count == 0)
                        {
                            adatok.Append("    Korosztály: ");
                            adatok.Append(versenysorozat.íjtípus[i].korosztályok[j].kmegn);
                            adatok.Bold();
                            korosztaly_count++;
                        }


                       
                        if (versenysorozat.íjtípus[i].korosztályok[j].nok.Count > 0)
                        {
                            Paragraph np = document.InsertParagraph();
                            if (nok_count == 0)
                            {
                                np.Append("       Nők:");
                                nok_count++;
                            }

                            for (int q = 0; q < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].sorszam.ToString());
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].nev);
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].eredmények[0].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].eredmények[0].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }

                        if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count > 0)
                        {
                            Paragraph fp = document.InsertParagraph();
                            if (ferfiak_count == 0)
                            {
                                fp.Append("        Férfiak:");
                                ferfiak_count++;
                            }

                            for (int q = 0; q < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].sorszam.ToString());
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].nev);
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].eredmények[0].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].eredmények[0].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }


                    }
                }
            }
            #endregion

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVEMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return filename;
        }

        static public string nyomtat_eredmenylap_versenysorozat_misz(string _VSAZON, int _limit)
        {
            
           Node_Eredménylap_VersenySorozat_Teljes versenysorozat = versenysorozat_adatok(_VSAZON,_limit,false);
           versenysorozat_kiir(versenysorozat);

            string filename = _VSAZON + "\\" + "ERLAPVSMISZ.docx";
            var document = DocX.Create(filename);
            document.AddHeaders();
            PageNumber( document );

            #region alap stringek
            string headline = "EREDMÉNYLAP";
            string típus = "***MISZ***";
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
            title.Append(headline);
            title.AppendLine(típus);

            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés + "\n");
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            //megnevezés
            List<Versenysorozat> vsor = Program.database.Versenysorozatok();

            foreach (Versenysorozat item in vsor)
            {
                if (_VSAZON == item.azonosító)
                {
                    megnevezés = item.megnevezés;
                    break;
                }
            }

            paragraph_1.Append("\n" + st_vsorazon);
            paragraph_1.Append(_VSAZON + ", " + megnevezés);
            paragraph_1.Bold();
            paragraph_1.AppendLine();

            #region formázás

            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                Table table = null;
                int ijtipus_count = 0;
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    int korosztaly_count = 0;
                    if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count > 0 || versenysorozat.íjtípus[i].korosztályok[j].nok.Count > 0)
                    {
                        Paragraph adatok = document.InsertParagraph();
                        int ferfiak_count = 0;
                        int nok_count = 0;

                        if (ijtipus_count == 0)
                        {
                            adatok.Append("Íjtípus: ");
                            adatok.Append(versenysorozat.íjtípus[i].megnevezés + "\n");
                            adatok.Bold();
                            ijtipus_count++;
                        }
                        if (korosztaly_count == 0)
                        {
                            adatok.Append("    Korosztály: ");
                            adatok.Append(versenysorozat.íjtípus[i].korosztályok[j].kmegn);
                            adatok.Bold();
                            korosztaly_count++;
                        }


                      
                        if (versenysorozat.íjtípus[i].korosztályok[j].nok.Count > 0)
                        {
                            Paragraph np = document.InsertParagraph();
                            if (nok_count == 0)
                            {
                                np.Append("       Nők:");
                                nok_count++;
                            }

                            for (int q = 0; q < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].sorszam.ToString());
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].nev);
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].eredmények[0].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].nok[q].eredmények[0].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }

                        if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count > 0)
                        {
                            Paragraph fp = document.InsertParagraph();
                            if (ferfiak_count == 0)
                            {
                                fp.Append("        Férfiak:");
                                ferfiak_count++;
                            }

                            for (int q = 0; q < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; q++)
                            {
                                table = document.AddTable(1, 7);

                                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((q + 1) + ".");
                                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].sorszam.ToString());
                                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].nev);
                                table.Rows[table.Rows.Count - 1].Cells[4].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].egyesulet);
                                table.Rows[table.Rows.Count - 1].Cells[5].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].eredmények[0].pont.ToString() + " pont");
                                table.Rows[table.Rows.Count - 1].Cells[6].Paragraphs[0].Append(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[q].eredmények[0].szazalek.ToString() + "%");
                                startlista_táblázat_formázás(table);
                                document.InsertTable(table);
                            }
                        }
                    }
                }
            }
            #endregion

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVEMISZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return filename;
        }

        static public string NyomtatEredmenylapVersenyEgyesulet(string _VEAZON)
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

            if (Data.VersenyAdatok.VSAZON != null)
            {
                FileName = Data.VersenyAdatok.VSAZON + "\\" + _VEAZON + "\\" + "ERLAPVEEGYE.docx";
            }
            else
            {
                FileName = _VEAZON + "\\" + "ERLAPVEEGYE.docx";
            }

            var document = DocX.Create(FileName);
            document.AddHeaders();
            PageNumber( document );

            #region header

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(Cim);
            title.AppendLine(Tipus);
            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés);
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region Title

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            paragraph_1.AppendLine(Verseny);

            paragraph_1.Append(_VEAZON + ", " + Data.VersenyAdatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + Ido);
            paragraph_1.Append(Data.VersenyAdatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t" + Osszpont);
            paragraph_1.Append((Data.VersenyAdatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + VersenySorozat);
            paragraph_1.Append(Data.VersenyAdatok.VSAZON + "," + Data.VersenyAdatok.VSMEGN);
            paragraph_1.AppendLine();
            paragraph_1.Bold();
            #endregion

            Table table = document.AddTable(1, 4);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Sorrend").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Egyesület neve").Bold();
            table.Rows[0].Cells[2].Paragraphs[0].Append("Egyesület címe").Bold();
            table.Rows[0].Cells[3].Paragraphs[0].Append("ÖsszPont").Bold();
            

            
            for(int i = 0;i<Data.Egyesuletek.Count;i++)
            {
                table.InsertRow();
                table.Rows[table.Rows.Count - 1].Cells[0].Paragraphs[0].Append( i+1 + "."  );
                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append(Data.Egyesuletek[i].Nev);
                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(Data.Egyesuletek[i].Cim);
                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(Data.Egyesuletek[i].OsszPont.ToString());
            }
            EgyesuletTablazatFormazas(table);

            document.InsertTable(table);

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "CSAPATLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            
            return FileName;
        }

        static public string NyomtatEredmenylapVersenysorozatEgyesulet( string _VSAZON )
        {
            string FileName = _VSAZON + "\\" + "ERLAPVSEGYE.docx";
            EREDMENYLAPVERSENYSOROZATEGYESULET Data = new EREDMENYLAPVERSENYSOROZATEGYESULET(_VSAZON);


            #region alap stringek
            string Cim = "EREDMÉNYLAP";
            string Tipus = "***egyesület***";
            string VersenySorozat = "Versenysorozat azonosítója, neve: ";
            #endregion



            var document = DocX.Create(FileName);
            document.AddHeaders();
            PageNumber( document );

            #region header

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(Cim);
            title.AppendLine(Tipus);
            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés);
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region Title

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();
            
            paragraph_1.Append("\n" + VersenySorozat);
            paragraph_1.Append(Data.Azonosito + "," + Data.Megnevezes);
            paragraph_1.AppendLine();
            paragraph_1.Bold();
              
            #endregion

            Table table = document.AddTable(1, 4);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Sorrend").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Egyesület neve").Bold();
            table.Rows[0].Cells[2].Paragraphs[0].Append("Egyesület címe").Bold();
            table.Rows[0].Cells[3].Paragraphs[0].Append("ÖsszPont").Bold();



            for (int i = 0; i < Data.Egyesuletek.Count; i++)
            {
                table.InsertRow();
                table.Rows[table.Rows.Count - 1].Cells[0].Paragraphs[0].Append(i + 1 + ".");
                table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append(Data.Egyesuletek[i].Nev);
                table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append(Data.Egyesuletek[i].Cim);
                table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append(Data.Egyesuletek[i].OsszPont.ToString());
            }
            EgyesuletTablazatFormazas(table);

            document.InsertTable(table);

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVSEGYE.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            return FileName;
        }

        static public string NyomtatEredmenylapVersenySorozatReszletes( string _VSAZON )
        {
            string FileName = _VSAZON + "\\" + "ERLAPVSRESZ.docx";
            EREDMENYLAPVERSENYSOROZATRESZLETES Data = new EREDMENYLAPVERSENYSOROZATRESZLETES(_VSAZON);

            #region test
            /*teszt
            int count = 0;
            foreach (EREDMENYLAPVERSENYSOROZATRESZLETES.Ijtipus ijtipus in Data.IJTIPUSOK)
            {
                foreach (EREDMENYLAPVERSENYSOROZATRESZLETES.Korosztaly korosztaly in ijtipus.Korosztalyok)
                {
                    for(int i = 0;i<korosztaly.Indulok.Ferfiak.Count;i++)
                    {
                        if( korosztaly.Indulok.Ferfiak[i].Nev == "DINNYÉS GYULA" )
                        {
                            count++;
                        }
                    }

                    for(int j = 0;j<korosztaly.Indulok.Nok.Count;j++)
                    {
                        count++;
                    }
                }
            }
            */

            #endregion

            var document = DocX.Create(FileName);
            document.AddHeaders();
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
            title.Append(Cim);
            title.AppendLine(Tipus);
            title.Alignment = Alignment.center;

            titleFormat.Size = 10D;
            title.AppendLine(Program.Tulajdonos_Megnevezés);
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region Title

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = header.InsertParagraph();

            paragraph_1.Append("\n" + VersenySorozat);
            paragraph_1.Append(Data.Azonosito + "," + Data.Megnevezes).Bold();
            paragraph_1.Bold();

            #endregion

            #region HeaderTable
            Table HeaderTable = document.AddTable(1, Data.VersenyekSzama + 5);
            HeaderTable.AutoFit = AutoFit.ColumnWidth;
            for (int z = 0; z < HeaderTable.Rows.Count; z++)
            {
                HeaderTable.Rows[z].Cells[0].Width = 30;
                HeaderTable.Rows[z].Cells[1].Width = 50;
                HeaderTable.Rows[z].Cells[2].Width = 200;
                HeaderTable.Rows[z].Cells[3].Width = 300;
                for (int q = 4; q < HeaderTable.Rows[z].Cells.Count - 1; q++)
                {
                    HeaderTable.Rows[z].Cells[q].Width = 70;
                }
                HeaderTable.Rows[z].Cells[HeaderTable.Rows[0].Cells.Count - 1].Width = 100;
                HeaderTable.Rows[z].Height = 27;
            }

            for (int z = 0; z < Data.VersenyAzonositok.Count; z++)
            {
                HeaderTable.Rows[0].Cells[z + 4].Paragraphs[0].Append(Data.VersenyAzonositok[z]).Bold();
                HeaderTable.Rows[0].Cells[z + 4].Paragraphs[0].FontSize(8D);

            }
            HeaderTable.Rows[0].Cells[HeaderTable.Rows[0].Cells.Count - 1].Paragraphs[0].Append("Összesen").Bold();;
            HeaderTable.Rows[0].Cells[HeaderTable.Rows[0].Cells.Count - 1].Paragraphs[0].FontSize(8D);

            HeaderTable.AutoFit = AutoFit.ColumnWidth;
            EredmenyLapReszletesTablazatFormazas(HeaderTable);

            header.InsertTable(HeaderTable);
            #endregion

            for ( int i = 0; i < Data.IjTipusok.Count; i++ )
            {
                Table table = null;
                for ( int j = 0; j < Data.IjTipusok[i].Korosztalyok.Count; j++ )
                {
                    if ( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak.Count != 0 || Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok.Count != 0 )
                    {
                        Paragraph adatok = document.InsertParagraph();
                        adatok.Append("Íjtípus: ");
                        adatok.Append( Data.IjTipusok[i].Megnevezes );
                        adatok.Bold();
                        adatok.AppendLine("    Korosztály: ");
                        adatok.Append( Data.IjTipusok[i].Korosztalyok[j].Megnevezes );
                        adatok.Bold();

                        if ( Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok.Count != 0 )
                        {
                            Paragraph np = document.InsertParagraph();
                            np.Append("      Nők: ");
                            np.Bold();
                        }

                        for ( int k = 0; k < Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok.Count; k++ )
                        {
                            table = document.AddTable(1, Data.VersenyekSzama + 5);
                            table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((k + 1) + ".");
                            table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].Nev );
                            table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].Egyesulet );
                            for ( int m = 0; m < Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].Eredmenyek.Count; m++ )
                            {
                                for(int n = 0; n< Data.VersenyekSzama;n++)
                                {
                                    if ( HeaderTable.Rows[0].Cells[4 + n].Paragraphs[0].Text == Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].Eredmenyek[m].Verseny )
                                    {
                                        table.Rows[table.Rows.Count - 1].Cells[n + 4].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].Eredmenyek[m].Pont.ToString( ) );
                                    }
                                }
                            }
                            table.Rows[table.Rows.Count - 1].Cells[Data.VersenyekSzama + 4].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Nok[k].OsszPont.ToString( ) + " pont" ).Bold( );

                            for (int ii = 0; ii < table.Rows.Count; ii++)
                            {
                                for (int jj = 0; jj < Data.VersenyekSzama; jj++)
                                {
                                    if(table.Rows[ii].Cells[jj + 4].Paragraphs[0].Text == "")
                                    {
                                        table.Rows[ii].Cells[jj + 4].Paragraphs[0].Append("0");
                                    }
                                }
                            }

                            EredmenyLapReszletesTablazatFormazas(table);
                            document.InsertTable(table);
                        }

                        if ( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak.Count != 0 )
                        {
                            Paragraph fp = document.InsertParagraph();
                            fp.Append("      Férfiak: ");
                            fp.Bold();
                        }
                        for ( int k = 0; k < Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak.Count; k++ )
                        {
                            table = document.AddTable(1, Data.VersenyekSzama + 5);
                            table.Rows[table.Rows.Count - 1].Cells[1].Paragraphs[0].Append((k + 1) + ".");
                            table.Rows[table.Rows.Count - 1].Cells[2].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].Nev );
                            table.Rows[table.Rows.Count - 1].Cells[3].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].Egyesulet );
                            for ( int m = 0; m < Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].Eredmenyek.Count; m++ )
                            {
                                for (int n = 0; n < Data.VersenyekSzama; n++)
                                {
                                    if ( HeaderTable.Rows[0].Cells[4 + n].Paragraphs[0].Text == Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].Eredmenyek[m].Verseny )
                                    {
                                        table.Rows[table.Rows.Count - 1].Cells[n + 4].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].Eredmenyek[m].Pont.ToString( ) );
                                    }
                                }
                            }
                            table.Rows[table.Rows.Count - 1].Cells[Data.VersenyekSzama + 4].Paragraphs[0].Append( Data.IjTipusok[i].Korosztalyok[j].Indulok.Ferfiak[k].OsszPont.ToString( ) + " pont" ).Bold( );
                            for (int ii = 0; ii < table.Rows.Count; ii++)
                            {
                                for (int jj = 0; jj < Data.VersenyekSzama; jj++)
                                {
                                    if (table.Rows[ii].Cells[jj + 4].Paragraphs[0].Text == "")
                                    {
                                        table.Rows[ii].Cells[jj + 4].Paragraphs[0].Append("0");
                                    }
                                }
                            }
                            EredmenyLapReszletesTablazatFormazas(table);
                            document.InsertTable(table);
                        } 
                    }
                             
                }
                
            }



            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVSRESZ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            return FileName;
        }

        static public Node_Eredménylap_VersenySorozat_Teljes versenysorozat_adatok(string _VSAZON, int _limit, bool _teljes)
        {
            Node_Eredménylap_VersenySorozat_Teljes versenysorozat = new Node_Eredménylap_VersenySorozat_Teljes();
            versenysorozat.versenyazonosítók = new List<string>();
            versenysorozat.íjtípus = new List<Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus>();

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny item in versenyek)
            {
                if (item.VersenySorozat == _VSAZON){ versenysorozat.versenyazonosítók.Add(item.Azonosito);}
            }

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            foreach (Íjtípus item_íjtípusok in íjtípusok){ versenysorozat.íjtípus.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus(item_íjtípusok.azonosító,item_íjtípusok.megnevezés));}

            List<Korosztály> korosztályok = Program.database.Korosztályok(versenysorozat.versenyazonosítók[0]);
            foreach (Korosztály item_korosztályok in korosztályok)
            {
                foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus item in versenysorozat.íjtípus)
                {
                    item.korosztályok.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály(item_korosztályok.alsó_határ, item_korosztályok.felső_határ, item_korosztályok.azonosító,item_korosztályok.megnevezés));
                }
            }

            #region struktúra feltöltése
            
            List<Induló> indulók = Program.database.Indulók();
            foreach (string _VEAZON in versenysorozat.versenyazonosítók)
            {
                List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);

                foreach (Induló item_induló in indulók)
                {
                    bool found = false;

                    foreach (Eredmény item_eredmény in eredmények)
                    {
                        if (_teljes == true)
                        {
                            if (item_eredmény.név == item_induló.név)
                            {
                                if (item_eredmény.megjelent == true)
                                {
                                    found = false;
                                    //ha már meglévő csak az eredményt adom hozzá, ha más az íjtípus akkor külön veszem
                                    for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
                                    {
                                        for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                                        {
                                            if (item_induló.nem == "F")
                                            {
                                                for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                                                {
                                                    //férfi
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].nev == item_induló.név && found == false && versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].ijazon == item_eredmény.íjtípus)
                                                    {
                                                        found = true;
                                                        versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény((int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10));
                                                        //Console.WriteLine("eredmény hozzáadva meglévő férfiversenyzőhöz: " + versenysorozat.íjtípus[i]._korosztályok[j]._ferfiak[k]._nev + " found: " + found);
                                                    }
                                                }
                                            }
                                            else if (item_induló.nem == "N")
                                            {
                                                for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                                                {
                                                    //nő
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].nok[k].nev == item_induló.név && found == false && versenysorozat.íjtípus[i].korosztályok[j].nok[k].ijazon == item_eredmény.íjtípus)//ennyit elég csekkelni??
                                                    {
                                                        found = true;
                                                        versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény((int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10 ));
                                                        //Console.WriteLine("eredmény hozzáadva meglévő nő versenyzőhöz: " + versenysorozat.íjtípus[i]._korosztályok[j]._nok[k]._nev + " found: " + found);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("ERROR");
                                            }
                                        }
                                    }
                                    //vagy új versenyző, vagy meglévő versenyző más íjtípussal
                                    if (found == false)
                                    {
                                        int year = Convert.ToInt32((new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(item_induló.születés))).Year - 1);
                                        Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo(
                                        item_induló.név, item_induló.nem, (int)item_eredmény.sorszám, year, item_induló.egyesület, item_eredmény.íjtípus, (int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10);
                                        for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
                                        {
                                            if (versenysorozat.íjtípus[i].azonosito == temp.ijazon)
                                            {
                                                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                                                {
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].also <= temp.kor && versenysorozat.íjtípus[i].korosztályok[j].felso >= temp.kor)
                                                    {
                                                        if (temp.nem == "F")
                                                        {
                                                            versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Add(temp);
                                                            versenysorozat.íjtípus[i].Vanbenne();
                                                            versenysorozat.íjtípus[i].korosztályok[j].Vanbenne2();
                                                        }
                                                        else
                                                        {
                                                            versenysorozat.íjtípus[i].korosztályok[j].nok.Add(temp);
                                                            versenysorozat.íjtípus[i].Vanbenne();
                                                            versenysorozat.íjtípus[i].korosztályok[j].Vanbenne2();
                                                        }
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item_eredmény.név == item_induló.név && item_induló.engedély!="")
                            {
                                if (item_eredmény.megjelent == true)
                                {
                                    found = false;
                                    //ha már meglévő csak az eredményt adom hozzá, ha más az íjtípus akkor külön veszem
                                    for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
                                    {
                                        for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                                        {
                                            if (item_induló.nem == "F")
                                            {
                                                for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                                                {
                                                    //férfi
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].nev == item_induló.név && found == false && versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].ijazon == item_eredmény.íjtípus)
                                                    {
                                                        found = true;
                                                        versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény((int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10));
                                                        //Console.WriteLine("eredmény hozzáadva meglévő férfi versenyzőhöz: " + versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].nev + " found: " + found);
                                                    }
                                                }
                                            }
                                            else if (item_induló.nem == "N")
                                            {
                                                for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                                                {
                                                    //nő
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].nok[k].nev == item_induló.név && found == false && versenysorozat.íjtípus[i].korosztályok[j].nok[k].ijazon == item_eredmény.íjtípus)//ennyit elég csekkelni??
                                                    {
                                                        found = true;
                                                        versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény((int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10));
                                                        //Console.WriteLine("eredmény hozzáadva meglévő nő versenyzőhöz: " + versenysorozat.íjtípus[i].korosztályok[j].nok[k].nev + " found: " + found);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("ERROR");
                                            }
                                        }
                                    }
                                    //vagy új versenyző, vagy meglévő versenyző más íjtípussal
                                    if (found == false)
                                    {
                                        int year = Convert.ToInt32((new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(item_induló.születés))).Year - 1);
                                        Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo(
                                        item_induló.név, item_induló.nem, (int)item_eredmény.sorszám, year, item_induló.egyesület, item_eredmény.íjtípus, (int)item_eredmény.összpont, (int)item_eredmény.százalék, (int)item_eredmény.találat_10);
                                        for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
                                        {
                                            if (versenysorozat.íjtípus[i].azonosito == temp.ijazon)
                                            {
                                                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                                                {
                                                    if (versenysorozat.íjtípus[i].korosztályok[j].also <= temp.kor && versenysorozat.íjtípus[i].korosztályok[j].felso > temp.kor)
                                                    {
                                                        if (temp.nem == "F")
                                                        {
                                                            versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Add(temp);
                                                            versenysorozat.íjtípus[i].Vanbenne();
                                                            versenysorozat.íjtípus[i].korosztályok[j].Vanbenne2();
                                                        }
                                                        else
                                                        {
                                                            versenysorozat.íjtípus[i].korosztályok[j].nok.Add(temp);
                                                            versenysorozat.íjtípus[i].Vanbenne();
                                                            versenysorozat.íjtípus[i].korosztályok[j].Vanbenne2();
                                                        }
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region rendezni kell az eredményeket
            
            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        for (int l = 0; l < versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count; l++)
                        {
                            for (int m = 0; m <  versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count; m++)
                            {
                                if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[l].szazalek > versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[m].szazalek)
                                {
                                    Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény temp = versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[l];
                                    versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[l] = versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[m];
                                    versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[m] = temp;
                                }
                            }
                        }
                    }
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        for (int l = 0; l < versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count; l++)
                        {
                            for (int m = 0; m < versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count; m++)
                            {
                                if (versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[l].szazalek > versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[m].szazalek)
                                {
                                    Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény temp = versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[l];
                                    versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[l] = versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[m];
                                    versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[m] = temp;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region limit számú eredmény kell
            
            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        if(versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count > _limit)
                        {
                            while (versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count != _limit)
                            {
                                int q = versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count-1;
                                versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.RemoveAt(q);
                            }
                        }
                        if (versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count < _limit)
                        {
                            versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény(0, 0,0));
                        }
                    }
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        if (versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count > _limit)
                        {
                            while (versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count != _limit)
                            {
                                int q = versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count - 1;
                                versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.RemoveAt(q);
                            }
                        }
                        if (versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count < _limit)
                        {
                            versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Add(new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény(0, 0,0));
                        }
                    }
                }
            }
            #endregion

            #region a 0.-ik eredmény legyen a végleges eredmény -> pontok összeadása százalék átlagolás
            
            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény temp = new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény();
                        for (int l = 0; l < versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count; l++)
                        {
                            temp.pont += versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[l].pont;
                            temp.szazalek += versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[l].szazalek;
                            temp.szazalek /= l + 1;
                        }
                        versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[0] = temp;
                    }
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény temp = new Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény();
                        for (int l = 0; l < versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count; l++)
                        {
                            temp.pont += versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[l].pont;
                            temp.szazalek += versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[l].szazalek;
                            temp.szazalek /= l + 1;
                        }
                        versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények[0] = temp;
                    }
                }
            }
            #endregion

            #region többi eredmény törlése
            
            for (int i = 0; i < versenysorozat.íjtípus.Count; i++)
            {
                for (int j = 0; j < versenysorozat.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        for (int l = 1; l < versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.Count; l++)
                        {
                            versenysorozat.íjtípus[i].korosztályok[j].ferfiak[k].eredmények.RemoveAt(l);
                        }
                    }
                    for (int k = 0; k < versenysorozat.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        for (int l = 1; l < versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.Count; l++)
                        {
                            versenysorozat.íjtípus[i].korosztályok[j].nok[k].eredmények.RemoveAt(l);
                        }
                    }
                }
            }
            #endregion

            rendez_versenysorozat_pont_alapján(versenysorozat);

            return versenysorozat;
        }

        static public void versenysorozat_kiir(Node_Eredménylap_VersenySorozat_Teljes _versenysorozat)
        {
            Console.WriteLine("---------------------------------------");

            foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus íjtípus_item in _versenysorozat.íjtípus)
            {
                if (íjtípus_item.korosztályok.Count != 0)//van benne
                {
                    Console.WriteLine("ij: " + íjtípus_item.azonosito);
                }
                foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály korosztály_item in íjtípus_item.korosztályok)
                {
                    if (korosztály_item.ferfiak.Count != 0 || korosztály_item.nok.Count != 0)
                    {
                        Console.WriteLine("   korosztály: " + korosztály_item.kazon);
                    }
                    foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo indulo_item in korosztály_item.ferfiak)
                    {
                        Console.WriteLine("      " + indulo_item.nev + " " + indulo_item.ijazon);//+ " " + indulo_item._nem + " " + indulo_item._kor + " " + indulo_item._sorszam);

                        foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény eredmény_item in indulo_item.eredmények)
                        {
                            Console.WriteLine("         " + eredmény_item.pont + " " + eredmény_item.szazalek + "%");
                        }
                    }

                    foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo indulo_item in korosztály_item.nok)
                    {
                        Console.WriteLine("      " + indulo_item.nev + " " +  indulo_item.ijazon);//+ " " + indulo_item._nem + " " + indulo_item._kor + " " + indulo_item._sorszam);

                        foreach (Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Eredmény eredmény_item in indulo_item.eredmények)
                        {
                            Console.WriteLine("         " + eredmény_item.pont + " " + eredmény_item.szazalek + "%");
                        }
                    }
                }
            }
            Console.WriteLine("---------------------------------------");
        }

        #region rendezés

        static public void rendez_verseny_pont_alapján( Node_Eredménylap_Verseny_Teljes eredmény )
        {
            for (int i = 0; i < eredmény.íjtípus.Count; i++)
            {
                for (int j = 0; j < eredmény.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < eredmény.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        for (int l = k+1; l < eredmény.íjtípus[i].korosztályok[j].ferfiak.Count; l++)
                        {
                            if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].pont < eredmény.íjtípus[i].korosztályok[j].ferfiak[l].pont)
                            {
                                Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].ferfiak[k];
                                eredmény.íjtípus[i].korosztályok[j].ferfiak[k] = eredmény.íjtípus[i].korosztályok[j].ferfiak[l];
                                eredmény.íjtípus[i].korosztályok[j].ferfiak[l] = temp;
                            }

                            if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].pont == eredmény.íjtípus[i].korosztályok[j].ferfiak[l].pont)
                            {
                                if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].tiztalalat < eredmény.íjtípus[i].korosztályok[j].ferfiak[l].tiztalalat)
                                {
                                    //MessageBox.Show( "csere:  " + eredmény.íjtípus[i].korosztályok[j].ferfiak[k].tiztalalat + "  ----    " + eredmény.íjtípus[i].korosztályok[j].ferfiak[l].tiztalalat);
                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].ferfiak[k];
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[k] = eredmény.íjtípus[i].korosztályok[j].ferfiak[l];
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[l] = temp;
                                }
                                else if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].tiztalalat == eredmény.íjtípus[i].korosztályok[j].ferfiak[l].tiztalalat)
                                {
                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].ferfiak[k];
                                    temp.holtverseny = true;
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[k] = temp;

                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp2 = eredmény.íjtípus[i].korosztályok[j].ferfiak[l];
                                    temp2.holtverseny = true;
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[l] = temp2;
                                }
                            }
                        }
                    }

                    for (int k = 0; k < eredmény.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        for (int l = k+1; l < eredmény.íjtípus[i].korosztályok[j].nok.Count; l++)
                        {
                            if (eredmény.íjtípus[i].korosztályok[j].nok[k].pont < eredmény.íjtípus[i].korosztályok[j].nok[l].pont)
                            {
                                Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].nok[k];
                                eredmény.íjtípus[i].korosztályok[j].nok[k] = eredmény.íjtípus[i].korosztályok[j].nok[l];
                                eredmény.íjtípus[i].korosztályok[j].nok[l] = temp;
                            }
                            if (eredmény.íjtípus[i].korosztályok[j].nok[k].pont == eredmény.íjtípus[i].korosztályok[j].nok[l].pont)
                            {
                                if (eredmény.íjtípus[i].korosztályok[j].nok[k].tiztalalat < eredmény.íjtípus[i].korosztályok[j].nok[l].tiztalalat)
                                {
                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].nok[k];
                                    eredmény.íjtípus[i].korosztályok[j].nok[k] = eredmény.íjtípus[i].korosztályok[j].nok[l];
                                    eredmény.íjtípus[i].korosztályok[j].nok[l] = temp;
                                }
                                else if (eredmény.íjtípus[i].korosztályok[j].nok[k].tiztalalat == eredmény.íjtípus[i].korosztályok[j].nok[l].tiztalalat)
                                {
                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].nok[k];
                                    temp.holtverseny = true;
                                    eredmény.íjtípus[i].korosztályok[j].nok[k] = temp;

                                    Node_Eredménylap_Verseny_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp2 = eredmény.íjtípus[i].korosztályok[j].nok[l];
                                    temp2.holtverseny = true;
                                    eredmény.íjtípus[i].korosztályok[j].nok[l] = temp2;
                                }
                            }
                        }
                    }
                }
            }
           
        }

        static public void rendez_versenysorozat_pont_alapján(Node_Eredménylap_VersenySorozat_Teljes eredmény)
        {
            for (int i = 0; i < eredmény.íjtípus.Count; i++)
            {
                for (int j = 0; j < eredmény.íjtípus[i].korosztályok.Count; j++)
                {
                    for (int k = 0; k < eredmény.íjtípus[i].korosztályok[j].ferfiak.Count; k++)
                    {
                        for (int l = 0; l < eredmény.íjtípus[i].korosztályok[j].ferfiak.Count; l++)
                        {
                            if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[0].pont > eredmény.íjtípus[i].korosztályok[j].ferfiak[l].eredmények[0].pont)
                            {
                                Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].ferfiak[k];
                                eredmény.íjtípus[i].korosztályok[j].ferfiak[k] = eredmény.íjtípus[i].korosztályok[j].ferfiak[l];
                                eredmény.íjtípus[i].korosztályok[j].ferfiak[l] = temp;
                            }

                            if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[0].pont == eredmény.íjtípus[i].korosztályok[j].ferfiak[l].eredmények[0].pont)
                            {
                                if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[0].tizpont > eredmény.íjtípus[i].korosztályok[j].ferfiak[l].eredmények[0].tizpont)
                                {
                                    Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].ferfiak[k];
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[k] = eredmény.íjtípus[i].korosztályok[j].ferfiak[l];
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[l] = temp;
                                }
                                else if (eredmény.íjtípus[i].korosztályok[j].ferfiak[k].eredmények[0].tizpont == eredmény.íjtípus[i].korosztályok[j].ferfiak[l].eredmények[0].tizpont)
                                {
                                    eredmény.íjtípus[i].korosztályok[j].ferfiak[k].Holtverseny();
                                }
                            }
                        }
                    }

                    for (int k = 0; k < eredmény.íjtípus[i].korosztályok[j].nok.Count; k++)
                    {
                        for (int l = 0; l < eredmény.íjtípus[i].korosztályok[j].nok.Count; l++)
                        {
                            if (eredmény.íjtípus[i].korosztályok[j].nok[k].eredmények[0].pont > eredmény.íjtípus[i].korosztályok[j].nok[l].eredmények[0].pont)
                            {
                                Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].nok[k];
                                eredmény.íjtípus[i].korosztályok[j].nok[k] = eredmény.íjtípus[i].korosztályok[j].nok[l];
                                eredmény.íjtípus[i].korosztályok[j].nok[l] = temp;
                            }

                            if (eredmény.íjtípus[i].korosztályok[j].nok[k].eredmények[0].pont == eredmény.íjtípus[i].korosztályok[j].nok[l].eredmények[0].pont)
                            {
                                if (eredmény.íjtípus[i].korosztályok[j].nok[k].eredmények[0].tizpont > eredmény.íjtípus[i].korosztályok[j].nok[l].eredmények[0].tizpont)
                                {
                                    Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus.Node_Korosztály.Node_Indulo temp = eredmény.íjtípus[i].korosztályok[j].nok[k];
                                    eredmény.íjtípus[i].korosztályok[j].nok[k] = eredmény.íjtípus[i].korosztályok[j].nok[l];
                                    eredmény.íjtípus[i].korosztályok[j].nok[l] = temp;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region táblázatok

        static public void
        NevezesiListaTablazatFormazas( Table _table )
        {
            _table.Alignment = Alignment.center;

            Border b = new Border( Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue );
            Border c = new Border( Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black );

            _table.SetBorder( TableBorderType.InsideH, b );
            _table.SetBorder( TableBorderType.InsideV, b );
            _table.SetBorder( TableBorderType.Bottom, b );
            _table.SetBorder( TableBorderType.Top, b );
            _table.SetBorder( TableBorderType.Left, b );
            _table.SetBorder( TableBorderType.Right, b );

            for ( int i = 0; i < 6; i++ )
            {
                _table.Rows[0].Cells[i].SetBorder( TableCellBorderType.Bottom, c );
            }

            for ( int i = 0; i < _table.Rows.Count; i++ )
            {
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
        CsapatlistaTablazatFormazas( Table _table )
        {
            Border b = new Border( Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue );
            Border c = new Border( Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black );

            _table.SetBorder( TableBorderType.InsideH, b );
            _table.SetBorder( TableBorderType.InsideV, b );
            _table.SetBorder( TableBorderType.Bottom, b );
            _table.SetBorder( TableBorderType.Top, b );
            _table.SetBorder( TableBorderType.Left, b );
            _table.SetBorder( TableBorderType.Right, b );

            for ( int i = 0; i < 6; i++ )
            {
                _table.Rows[0].Cells[i].SetBorder( TableCellBorderType.Bottom, c );
            }

            for ( int i = 0; i < _table.Rows.Count; i++ )
            {
                _table.Rows[i].Cells[0].Width = 70;
                _table.Rows[i].Cells[1].Width = 70;
                _table.Rows[i].Cells[2].Width = 200;
                _table.Rows[i].Cells[3].Width = 100;
                _table.Rows[i].Cells[4].Width = 70;
                _table.Rows[i].Cells[5].Width = 200;
            }
            _table.AutoFit = AutoFit.ColumnWidth;

        }


        static public void startlista_táblázat_formázás(Table _table)
        {
            _table.AutoFit = AutoFit.Contents;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
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
            _table.SetBorder(TableBorderType.InsideV, c);
            // _table.SetBorder(TableBorderType.Bottom, c);
            _table.SetBorder(TableBorderType.Top, c);
            _table.SetBorder(TableBorderType.Left, c);
            _table.SetBorder(TableBorderType.Right, c);
        }

        static public void EredmenyLapReszletesTablazatFormazas(Table _table)
        {
            _table.AutoFit = AutoFit.ColumnWidth;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _table.Rows[i].Cells[0].Width = 30;
                _table.Rows[i].Cells[1].Width = 50;
                _table.Rows[i].Cells[2].Width = 200;
                _table.Rows[i].Cells[3].Width = 300;
                for (int j = 4; j < _table.Rows[i].Cells.Count-1; j++)
                {
                    _table.Rows[i].Cells[j].Width = 70;
                }

                _table.Rows[i].Cells[_table.Rows[i].Cells.Count-1].Width = 100;

                _table.Rows[i].Height = 20;

            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
            _table.SetBorder(TableBorderType.InsideH, d);
            _table.SetBorder(TableBorderType.InsideV, c);
             _table.SetBorder(TableBorderType.Bottom, c);
            _table.SetBorder(TableBorderType.Top, c);
            _table.SetBorder(TableBorderType.Left, c);
            _table.SetBorder(TableBorderType.Right, c);
  
        }

        static public void header_táblázat_formázása(Table _table)
        {
            _table.AutoFit = AutoFit.ColumnWidth;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _table.Rows[i].Cells[0].Width = 350;
                _table.Rows[i].Cells[1].Width = 200;
                _table.Rows[i].Cells[2].Width = 100;
                _table.Rows[i].Height = 27;
            }

            Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black);
            _table.SetBorder(TableBorderType.InsideH, c);
            _table.SetBorder(TableBorderType.InsideV, c);
            _table.SetBorder(TableBorderType.Bottom, c);
            _table.SetBorder(TableBorderType.Top, c);
            _table.SetBorder(TableBorderType.Left, c);
            _table.SetBorder(TableBorderType.Right, c);
        }

        static public void beirlap_táblázat_formázás(Table _table)
        {
            _table.AutoFit = AutoFit.ColumnWidth;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
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

            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _table.Rows[i].Height = 20;
            }
        }

        static public void EgyesuletTablazatFormazas(Table _table)
        {
            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            _table.SetBorder(TableBorderType.InsideH, b);
            _table.SetBorder(TableBorderType.InsideV, b);
            _table.SetBorder(TableBorderType.Bottom, b);
            _table.SetBorder(TableBorderType.Top, b);
            _table.SetBorder(TableBorderType.Left, b);
            _table.SetBorder(TableBorderType.Right, b);

            for (int i = 0; i < 4; i++)
            {
                _table.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, c);
            }

            _table.AutoFit = AutoFit.ColumnWidth;
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _table.Rows[i].Cells[0].Width = 100;
                _table.Rows[i].Cells[1].Width = 250;
                _table.Rows[i].Cells[2].Width = 250;
                _table.Rows[i].Cells[3].Width = 100;
            }

            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _table.Rows[i].Height = 20;
            }
        }

        #endregion

        public static Footer PageNumber( DocX _document )
        {
            _document.AddFooters( );
            Footer footer = _document.Footers.odd;

            Table FooterTable = footer.InsertTable( 1, 2 );
            FooterTable.Rows[0].Cells[1].Paragraphs[0].InsertPageNumber( PageNumberFormat.normal, 0 );
            FooterTable.Rows[0].Cells[1].Paragraphs[0].Append( ". oldal" );
            FooterTable.AutoFit = AutoFit.ColumnWidth;
            FooterTable.Rows[0].Cells[0].Width = _document.PageWidth - 200;
            FooterTable.Rows[0].Cells[1].Width = 60;

            Border c = new Border( Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Black );
            FooterTable.SetBorder( TableBorderType.InsideH, c );
            FooterTable.SetBorder( TableBorderType.InsideV, c );
            FooterTable.SetBorder( TableBorderType.Bottom, c );
            FooterTable.SetBorder( TableBorderType.Top, c );
            FooterTable.SetBorder( TableBorderType.Left, c );
            FooterTable.SetBorder( TableBorderType.Right, c );

            return footer;
        }


        static public void print(string _filename)
        {
            ProcessStartInfo info = new ProcessStartInfo(_filename.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(info);
        }


        static public void Dialog(string _filename) 
        {
            DialogFileName = _filename;
            dialog = new Form();
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.Width = 300;
            dialog.Height = 150;
            dialog.Text = "Nyomtatás";
            Label text = new Label() { Left = 70, Top = 20, Text = "Biztosan nyomtatni akar?", Width = 200 };
            Button igen = new Button() { Text = "Igen", Left = 30, Top = 50, Width = 70 };
            Button nem = new Button() { Text = "Nem", Left = 110, Top = 50, Width = 70 };
            Button megnyit = new Button() { Text = "Megnyitás", Left = 190, Top = 50, Width = 70 };
            dialog.Controls.Add(text);
            dialog.Controls.Add(igen);
            dialog.Controls.Add(nem);
            dialog.Controls.Add(megnyit);
            igen.Click += igen_Click;
            nem.Click += nem_Click;
            megnyit.Click += megnyit_Click;
            dialog.ShowDialog();
        }

        public static int BetoltottEletkor( string _VersenyDatum, string _InduloSzuletes )
        {
            return ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( _VersenyDatum ) - DateTime.Parse( _InduloSzuletes ) ) ).Year - 1;
        }


        #region event handlers
        static void igen_Click(object sender, EventArgs e)
        {
            Nyomtat.print(DialogFileName);
            dialog.DialogResult = DialogResult.OK;
        }
        static void megnyit_Click(object sender, EventArgs e)
        {
            Process.Start( DialogFileName );
        }
        static void nem_Click(object sender, EventArgs e)
        {
            dialog.DialogResult = DialogResult.OK;
            return;
        }
        #endregion
    }
}
    

