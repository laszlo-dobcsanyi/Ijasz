using Novacode;
using System.Drawing;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System;

namespace Íjász
{
    public static class Nyomtat
    {
        public static int ijtipus_sorszam;
        public static int korosztaly_sorszam;
        public static string filename;
        public static Form dialog;


        public struct Node_Csapatlista
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
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
                    VSAZON = _versenysorozat.azonosító;
                    VSMEGN = _versenysorozat.megnevezés;
                }
                public Versenyadatok(Verseny _verseny)
                {
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
                    VSAZON = null;
                    VSMEGN = null;
                }
            }

            public struct Versenyzoadatok
            {
                public string ITMEGN;
                public string INNEVE;
                public int INSOSZ;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;

                public Versenyzoadatok( Íjtípus _íjtípus, Induló _induló, Eredmény _eredmény )
                {
                    ITMEGN = _íjtípus.megnevezés;
                    INNEVE = _eredmény.név;
                    INSOSZ = (int)_eredmény.sorszám;
                    INSZUL = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(_induló.születés))).Year - 1;
                    INEGYE = _induló.egyesület;
                    INCSSZ = _eredmény.csapat;
                }
            }

            public Versenyadatok versenyadatok;
            public List<Versenyzoadatok> versenyzoadatok;
        };

        public struct Node_NevezésiLista
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
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
                    VSAZON = _versenysorozat.azonosító;
                    VSMEGN = _versenysorozat.megnevezés;
                }
                public Versenyadatok(Verseny _verseny)
                {
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
                    VSAZON = null;
                    VSMEGN = null;
                }
            }
            public struct Versenyzoadatok
            {
                public int INSOSZ;
                public string INNEVE;
                public string ITMEGN;
                public int INSZUL;
                public string INEGYE;
                public int INCSSZ;

                public Versenyzoadatok( Induló _induló, Eredmény _eredmény, Íjtípus _íjtípus )
                {
                    INSOSZ = (int)_eredmény.sorszám;
                    INNEVE = _eredmény.név;
                    ITMEGN = _íjtípus.megnevezés;
                    INSZUL = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(_induló.születés))).Year - 1;
                    INEGYE = _induló.egyesület;
                    INCSSZ = _eredmény.csapat;
                }
            }

            public Versenyadatok versenyadatok;
            public List<Versenyzoadatok> versenyzoadatok;
        };
        
        public struct Node_Beírólap
        {
           
            public struct Versenyadatok
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public int VEALSZ;
                public string VSAZON;
                public string VSMEGN;
                public Versenyadatok(Verseny _verseny, Versenysorozat _versenysorozat)
                {
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEALSZ = _verseny.állomások;
                    VSAZON = _versenysorozat.azonosító;
                    VSMEGN = _versenysorozat.megnevezés;
                }
                public Versenyadatok(Verseny _verseny)
                {
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEALSZ = _verseny.állomások;
                    VSAZON = null;
                    VSMEGN = null;
                }
            }

            public struct Versenyzoadatok
            {
                public int INSOSZ;
                public int INCSSZ;
                public string INNEVE;
                public string INBEEK;
                public string INNEME;
                public string INEGYE;
                public string INVEEN;
                public string ITMEGN;
                public string KOMEGN;

                public Versenyzoadatok( Eredmény _eredmény, Íjtípus _íjtípus, Korosztály _korosztály, Induló _induló )
                {
                    INSOSZ = (int)_eredmény.sorszám;
                    INCSSZ = _eredmény.csapat;
                    INNEVE = _eredmény.név;
                    INBEEK = _induló.születés;
                    INNEME = _induló.nem;
                    INEGYE = _induló.egyesület;
                    INVEEN = _induló.engedély;
                    ITMEGN = _íjtípus.megnevezés;
                    KOMEGN = _korosztály.megnevezés;
                }

            }

            public Versenyadatok versenyadatok;
            public Versenyzoadatok versenyzoadatok;
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
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
                    VSAZON = _versenysorozat.azonosító;
                    VSMEGN = _versenysorozat.megnevezés;
                }
                public Versenyadatok(Verseny _verseny)
                {
                    VEAZON = _verseny.azonosító;
                    VEMEGN = _verseny.megnevezés;
                    VEDATU = _verseny.dátum;
                    VEOSPO = _verseny.összes;
                    VEINSZ = _verseny.indulók;
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

        public struct EredmenylapVersenyEgyesulet
        {
            public struct Versenyadatok
            {
                public string VEAZON;
                public string VEMEGN;
                public string VEDATU;
                public int VEOSPO;
                public string VSAZON;
                public string VSMEGN;

                public Versenyadatok(string _VEAZON ) : this()
                {
                    List<Verseny> versenyek = Program.database.Versenyek();
                    List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

                    foreach (Verseny verseny in versenyek)
                    {
                        if (verseny.azonosító == _VEAZON)
                        {
                            VEAZON = verseny.azonosító;
                            VEMEGN = verseny.megnevezés;
                            VEDATU = verseny.dátum;
                            VEOSPO = verseny.összes;
                            VSAZON = verseny.versenysorozat;
                        }
                        
                    }
                    foreach (Versenysorozat versenysorozat in versenysorozatok)
                    {
                        if(versenysorozat.azonosító == VSAZON)
                        VSMEGN = versenysorozat.megnevezés;
                    }
                }
            }

            public struct InduloAdat
            {
                public string Nev;
                public string Egyesulet;
                public int Pont;

                public InduloAdat( string _Nev, string _Egyesulet, int _Pont )
                {
                    Nev = _Nev;
                    Egyesulet = _Egyesulet;
                    Pont = _Pont;
                }
            }

            public struct EgyesuletAdat
            {
                public int Helyezes;
                public string Nev;
                public string Cim;
                public int OsszPont;


                public EgyesuletAdat(int _Helyezes, string _Nev, string _Cim, int _OsszPont)
                {
                    Helyezes = _Helyezes;
                    Nev = _Nev;
                    Cim = _Cim;
                    OsszPont = _OsszPont;
                }
            }

            public static List<InduloAdat> InduloAdatok(string _VEAZON)
            {
                List<Eredmény> eredmenyek = Program.database.Eredmények(_VEAZON);
                List<Induló> indulok = new List<Induló>();
                List<InduloAdat> induloadatok = new List<InduloAdat>();

                for (int i = eredmenyek.Count - 1; i >= 0; i--)
                {
                    if (eredmenyek[i].megjelent == false)
                    {
                        eredmenyek.Remove(eredmenyek[i]);
                    }
                    else
                    {
                        Induló temp = Program.database.Induló(eredmenyek[i].név).Value;
                        Egyesulet tempegyesulet = Program.database.Egyesulet(temp.egyesület);
                        if (tempegyesulet.Listazando==true)
                        {
                            indulok.Add(temp);
                        }
                        else
                        {
                            eredmenyek.Remove(eredmenyek[i]);
                        }
                    }
                }

                for (int i = eredmenyek.Count - 1; i >= 0; i--)
                {
                    for (int j = indulok.Count - 1; j >= 0; j--)
                    {
                        if (eredmenyek[i].név == indulok[j].név)
                        {
                            InduloAdat indulo = new InduloAdat(indulok[j].név, indulok[j].egyesület, eredmenyek[i].összpont.Value);
                            induloadatok.Add(indulo);
                            eredmenyek.RemoveAt(i);
                            indulok.RemoveAt(j);
                        }
                    }
                }

                if (eredmenyek.Count != 0 || indulok.Count != 0)
                {
                    MessageBox.Show("FAIL");
                }
                return induloadatok;
            }

            public List<EgyesuletAdat> EgyesuletAdatok(string _VEAZON)
            {
                List<InduloAdat> induloadatok = InduloAdatok(_VEAZON);
                List<EgyesuletAdat> egyesuletadatok = new List<EgyesuletAdat>();
                //List<Egyesulet> egyesuletek = Program.database.Egyesuletek();

                List<Egyesulet> egyesuletek = Program.database.Egyesuletek();
                foreach(Egyesulet egyesulet in egyesuletek)
                {
                    egyesuletadatok.Add(new EgyesuletAdat(0,egyesulet.Azonosito,egyesulet.Cim,0));
                }

                for (int i =0; i< induloadatok.Count ; i++)
                {
                    for (int j = 0; j < egyesuletadatok.Count; j++)
                    {
                        if( induloadatok[i].Egyesulet == egyesuletadatok[j].Nev )
                        {
                            EgyesuletAdat temp = egyesuletadatok[j];
                            temp.OsszPont += induloadatok[i].Pont;
                            egyesuletadatok.RemoveAt(j);
                            egyesuletadatok.Add(temp);
                        }
                    }
                }

                for(int i= egyesuletadatok.Count-1;i>=0;i--)
                {
                    if(egyesuletadatok[i].OsszPont == 0)
                    {
                        egyesuletadatok.RemoveAt(i);
                    }
                }

                for (int i=0;i<egyesuletadatok.Count;i++)
                {
                    for (int j = 0; j < egyesuletadatok.Count; j++)
                    {
                        if(egyesuletadatok[i].OsszPont > egyesuletadatok[j].OsszPont)
                        {
                            EgyesuletAdat temp = egyesuletadatok[i];
                            egyesuletadatok[i] = egyesuletadatok[j];
                            egyesuletadatok[j] = temp;
                        }
                    }
                }

                /*
                {
                    EgyesuletAdat egyesuletadat = new EgyesuletAdat(0,induloadatok[i].Egyesulet,"",0);
                    induloadatok.RemoveAt(i);
                    for( int j = induloadatok.Count-1; j>=1; j-- )
                    {
                        if(induloadatok[j].Egyesulet == egyesuletadat.Nev)
                        {
                            egyesuletadat.OsszPont += induloadatok[j].Pont;
                            induloadatok.RemoveAt(j);
                        }
                    }
                    egyesuletadatok.Add(egyesuletadat);

                }
                */
                return egyesuletadatok;
            }

            public EredmenylapVersenyEgyesulet(string _VEAZON) : this()
            {
                Egyesuletek = EgyesuletAdatok( _VEAZON);
                VersenyAdatok = new EredmenylapVersenyEgyesulet.Versenyadatok(_VEAZON);
            }

            public List<EgyesuletAdat> Egyesuletek;
            public Versenyadatok VersenyAdatok; 
        }

        static public string nyomtat_beirlap(string _VEAZON, Eredmény _eredmény) 
        {
            #region alap stringek
            string st_pont = "\n\n\n       ------------------------------      ------------------------------";
            string st_beiro = "                                 Beíró aláírása                                               Versenyző aláírása";

            string headline = "B E Í R Ó L A P";
            string st_vazon_vnev = "Verseny azonosító, név: ";
            string st_ido = "Verseny ideje: ";
            string st_vosszp = "Verseny össz pontszám: ";
            string st_vsorazon = "Versenysorozat azonosító, név: ";
            string st_sorszam = "Versenyző nevezés sorszám: ";
            string st_csapat = "Csapatszám: ";
            string st_neve = "Név: ";
            string st_kor = "Betöltött kor: ";
            string st_nem = "Nem: ";
            string st_egyesulet = "Egyesület: ";
            string st_engedely = "Versenyengedélyszám: ";
            string st_ijtipus = "Íj típus: ";
            string st_korosztaly = "Korosztály: ";
            #endregion

            #region adatok

            Node_Beírólap beirlap = new Node_Beírólap();

            List<Verseny> versenyek = Program.database.Versenyek();
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Verseny outer in versenyek)
            {
                if (outer.azonosító == _VEAZON && versenysorozatok.Count!=0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító==outer.versenysorozat)
                        {
                            beirlap.versenyadatok = new Node_Beírólap.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.azonosító == _VEAZON)
                {
                    beirlap.versenyadatok = new Node_Beírólap.Versenyadatok(outer);
                }
            }

            List<Induló> indulók = Program.database.Indulók();
            List<Korosztály> korosztályok = Program.database.Korosztályok(_VEAZON);
            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();

                    foreach (Induló item_indulo in indulók)
                    {
                        if (item_indulo.név == _eredmény.név)
                        {
                            int year = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(item_indulo.születés))).Year - 1;
                            foreach (Íjtípus item_ijtipus in íjtípusok)
                            {
                                if (item_ijtipus.azonosító == _eredmény.íjtípus)
                                {
                                    foreach (Korosztály item_korosztály in korosztályok)
                                    {
                                        if (year>=item_korosztály.alsó_határ && year <= item_korosztály.felső_határ)
                                        {
                                            beirlap.versenyzoadatok = new Node_Beírólap.Versenyzoadatok(_eredmény, item_ijtipus, item_korosztály, item_indulo);
                                            beirlap.versenyzoadatok.INBEEK = year.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
            #endregion

            string filename=null;

            if (beirlap.versenyadatok.VSAZON != null)
            {
                filename = beirlap.versenyadatok.VSAZON + "\\" + _VEAZON + "\\" + "BEIRLAP.docx";
            }
            else
            {
                 filename = _VEAZON + "\\" + "BEIRLAP.docx";
            }

            var document = DocX.Create(filename);
            document.AddHeaders();

            #region címbekezdés

            var titleFormat = new Formatting();
            titleFormat.Size = 10D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;
            Paragraph title = header.InsertParagraph();
            title.Append(headline);
            title.Alignment = Alignment.center;
            title.AppendLine(Program.Tulajdonos_Megnevezés + "\n");
            title.Bold();
            titleFormat.Position = 12;
            #endregion

            #region táblázat formázás

            Table table = document.AddTable(beirlap.versenyadatok.VEALSZ + 3, 8);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Sorszám");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Lőállás");
            table.Rows[0].Cells[2].Paragraphs[0].Append("10 pont");
            table.Rows[0].Cells[3].Paragraphs[0].Append("8 pont");
            table.Rows[0].Cells[4].Paragraphs[0].Append("5 pont");
            table.Rows[0].Cells[5].Paragraphs[0].Append("Mellé");
            table.Rows[0].Cells[6].Paragraphs[0].Append("Összesen");
            table.Rows[0].Cells[7].Paragraphs[0].Append("Göngyölt");

            for (int i = 1; i <= beirlap.versenyadatok.VEALSZ; i++)
            {
                table.Rows[i].Cells[0].Paragraphs[0].Append((i).ToString());
            }
            
            table.Rows[beirlap.versenyadatok.VEALSZ + 1].Cells[1].Paragraphs[0].Append("Össz darab");
            table.Rows[beirlap.versenyadatok.VEALSZ + 2].Cells[1].Paragraphs[0].Append("Össz pont");
            


            #endregion

            #region header

            var titleFormat2 = new Formatting();
            titleFormat2.Size = 10D;
            titleFormat2.Position = 1;

            Paragraph paragraph_1 = document.InsertParagraph(st_vazon_vnev, false, titleFormat2);

            Table header_table = document.AddTable(4,3);


            header_table.Rows[0].Cells[0].Paragraphs[0].Append(st_sorszam);
            titleFormat2.Size = 18D;
            header_table.Rows[0].Cells[0].Paragraphs[0].InsertText(beirlap.versenyzoadatok.INSOSZ.ToString(),false,titleFormat2);

            header_table.Rows[0].Cells[1].Paragraphs[0].Append(st_csapat);
            header_table.Rows[0].Cells[1].Paragraphs[0].Append(beirlap.versenyzoadatok.INCSSZ.ToString()).Bold();

            header_table.Rows[1].Cells[0].Paragraphs[0].Append(st_neve);
            titleFormat2.Size = _eredmény.név.Length > 20 ? 14D : 18D;
            header_table.Rows[1].Cells[0].Paragraphs[0].InsertText(_eredmény.név.ToString(), false, titleFormat2);

            header_table.Rows[1].Cells[1].Paragraphs[0].Append(st_kor);
            header_table.Rows[1].Cells[1].Paragraphs[0].Append(beirlap.versenyzoadatok.INBEEK.ToString()).Bold();

            header_table.Rows[1].Cells[2].Paragraphs[0].Append(st_nem);
            header_table.Rows[1].Cells[2].Paragraphs[0].Append(beirlap.versenyzoadatok.INNEME.ToString()).Bold();

            header_table.Rows[2].Cells[0].Paragraphs[0].Append(st_egyesulet);
            header_table.Rows[2].Cells[0].Paragraphs[0].Append(beirlap.versenyzoadatok.INEGYE.ToString()).Bold();

            header_table.Rows[2].Cells[1].Paragraphs[0].Append(st_engedely);
            header_table.Rows[2].Cells[1].Paragraphs[0].Append(beirlap.versenyzoadatok.INVEEN.ToString()).Bold();

            header_table.Rows[3].Cells[0].Paragraphs[0].Append(st_ijtipus);
            header_table.Rows[3].Cells[0].Paragraphs[0].Append(beirlap.versenyzoadatok.ITMEGN.ToString()).Bold();

            header_table.Rows[3].Cells[1].Paragraphs[0].Append(st_korosztaly);
            header_table.Rows[3].Cells[1].Paragraphs[0].Append(beirlap.versenyzoadatok.KOMEGN.ToString()).Bold();

            header_táblázat_formázása(header_table);
            document.InsertTable(header_table);
            Paragraph temp = document.InsertParagraph();


            paragraph_1.Append(_VEAZON + ", " + beirlap.versenyadatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + st_ido);
            paragraph_1.Append(beirlap.versenyadatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + st_vosszp);
            paragraph_1.Append((beirlap.versenyadatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + st_vsorazon);
            if (beirlap.versenyadatok.VSAZON != null)
            {
                paragraph_1.Append(beirlap.versenyadatok.VSAZON + ", " + beirlap.versenyadatok.VSMEGN);
            }
            paragraph_1.Bold();
            paragraph_1.AppendLine();
            #endregion

            beirlap_táblázat_formázás(table);
            document.InsertTable(table);

            //TODO SZÉTCSÚSZIK!!!4NÉGY
            Paragraph paragraph_3 = document.InsertParagraph(st_pont, false, titleFormat2);
            paragraph_3.AppendLine(st_beiro);

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "BEIRLAP.DOCX ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return filename;
        }

        static public string nyomtat_nevezesilista(string _VEAZON , bool _nemmegjelent_nyomtat)
        {
            Node_NevezésiLista nevezlista = new Node_NevezésiLista();
            nevezlista.versenyzoadatok = new List<Node_NevezésiLista.Versenyzoadatok>();
            


            #region alap stringek
            string headline;
            string st_ind;

            if (_nemmegjelent_nyomtat)
            {
                 headline = "H I Á N Y Z Ó K  L I S T A";
                 st_ind = "Hiányzók száma: ";

            }
            else
            {
                 headline = "N E V E Z É S I  L I S T A";
                 st_ind = "Indulók száma: ";

            } 
            string st_vazon_vnev = "Verseny azonosító, név: ";
            string st_ido = "Verseny ideje: ";
            string st_vosszp = "Verseny össz pontszám: ";
            string st_vsorazon = "Versenysorozat azonosító, név: ";
            #endregion

            #region adatok

            List<Verseny> versenyek = Program.database.Versenyek();
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Verseny outer in versenyek)
            {
                if (outer.azonosító==_VEAZON && versenysorozatok.Count!=0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító ==outer.versenysorozat )
                        {
                            nevezlista.versenyadatok = new Node_NevezésiLista.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if(outer.azonosító == _VEAZON)
                {
                    nevezlista.versenyadatok = new Node_NevezésiLista.Versenyadatok(outer);
                }
            }


            List<Induló> indulók = Program.database.Indulók();
            List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);
            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            int nemmegjelent = 0;
            int megjelent = 0;

            foreach (Eredmény item_eredmény in eredmények)
            {
                if (_nemmegjelent_nyomtat == true )
                {
                    if (item_eredmény.megjelent == false)
                    {
                        nemmegjelent++;
                        foreach (Induló item_induló in indulók)
                        {
                            if (item_induló.név == item_eredmény.név)
                            {
                                foreach (Íjtípus item_íjtípus in íjtípusok)
                                {
                                    if (item_eredmény.íjtípus == item_íjtípus.azonosító)
                                    {
                                        nevezlista.versenyzoadatok.Add(new Node_NevezésiLista.Versenyzoadatok(item_induló, item_eredmény, item_íjtípus));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (item_eredmény.megjelent == true)
                    {
                        megjelent++;
                        foreach (Induló item_induló in indulók)
                        {
                            if (item_induló.név == item_eredmény.név)
                            {
                                foreach (Íjtípus item_íjtípus in íjtípusok)
                                {
                                    if (item_eredmény.íjtípus == item_íjtípus.azonosító)
                                    {
                                        nevezlista.versenyzoadatok.Add(new Node_NevezésiLista.Versenyzoadatok(item_induló, item_eredmény, item_íjtípus));
                                    }
                                }
                            }
                        }
                    }
                }
                
            }

            if (_nemmegjelent_nyomtat==true)
            {
                nevezlista.versenyadatok.VEINSZ = nemmegjelent;
            }
            else
            {
                nevezlista.versenyadatok.VEINSZ = megjelent;
            }


            for (int i = 0; i < nevezlista.versenyzoadatok.Count-1; i++)
            {
                for (int j = nevezlista.versenyzoadatok.Count - 1; j > i; j-- )
                {
                    if (nevezlista.versenyzoadatok[j-1].INNEVE[0] < nevezlista.versenyzoadatok[j].INNEVE[0])
                    {
                        Node_NevezésiLista.Versenyzoadatok temp = nevezlista.versenyzoadatok[j];
                        nevezlista.versenyzoadatok[j] = nevezlista.versenyzoadatok[j - 1];
                        nevezlista.versenyzoadatok[j - 1] = temp;
                    }
                }
            }

            #endregion
            
            string filename;
            
            if (_nemmegjelent_nyomtat)
            {
                if (nevezlista.versenyadatok.VSAZON!=null)
                {
                    filename = nevezlista.versenyadatok.VSAZON + "\\" + _VEAZON + "\\"  + "NEVEZLISTANEMMEGJELENT.docx";
                }
                else
                {
                    filename = _VEAZON + "\\" + "NEVEZLISTANEMMEGJELENT.docx";
                }
            }
            else
            {
                if (nevezlista.versenyadatok.VSAZON != null)
                {
                    filename = nevezlista.versenyadatok.VSAZON + "\\" + nevezlista.versenyadatok.VEAZON + "\\" + "NEVEZLISTA.docx";
                }
                else
                {
                    filename = _VEAZON + "\\" + "NEVEZLISTA.docx";
                }
            }
            var document = DocX.Create(filename);
            document.AddHeaders();

            #region címbekezdés

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(headline);
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

            Paragraph paragraph_1 = document.InsertParagraph(st_vazon_vnev, false, titleFormat2);

            paragraph_1.Append(nevezlista.versenyadatok.VEAZON + ", " + nevezlista.versenyadatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + st_ido);
            paragraph_1.Append(nevezlista.versenyadatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + st_vosszp);
            paragraph_1.Append((nevezlista.versenyadatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + st_ind);
            paragraph_1.Append(nevezlista.versenyadatok.VEINSZ.ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + st_vsorazon);
            paragraph_1.Append(nevezlista.versenyadatok.VSAZON + ", " + nevezlista.versenyadatok.VSMEGN);
            paragraph_1.Bold();
            paragraph_1.AppendLine();
            #endregion

            #region táblázat formázás

            Table table = document.AddTable(nevezlista.versenyadatok.VEINSZ + 1, 6);
            table.Alignment = Alignment.center;

            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            table.SetBorder(TableBorderType.InsideH, b);
            table.SetBorder(TableBorderType.InsideV, b);
            table.SetBorder(TableBorderType.Bottom, b);
            table.SetBorder(TableBorderType.Top, b);
            table.SetBorder(TableBorderType.Left, b);
            table.SetBorder(TableBorderType.Right, b);

            for (int i = 0; i < 6; i++)
            {
                table.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, c);
            }

            for (int i = 0; i < nevezlista.versenyadatok.VEINSZ + 1; i++)
            {
                table.Rows[i].Cells[0].Width = 70;
                table.Rows[i].Cells[1].Width = 200;
                table.Rows[i].Cells[2].Width = 100;
                table.Rows[i].Cells[3].Width = 40;
                table.Rows[i].Cells[4].Width = 150;
                table.Rows[i].Cells[5].Width = 150;
            }
            table.AutoFit = AutoFit.ColumnWidth;


            table.Rows[0].Cells[0].Paragraphs[0].Append("Sorszám");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Név");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Íjtípus");
            table.Rows[0].Cells[3].Paragraphs[0].Append("Kor");
            table.Rows[0].Cells[4].Paragraphs[0].Append("Egyesület");
            table.Rows[0].Cells[5].Paragraphs[0].Append("Csapat");
            #endregion

            #region táblázat adatok betöltése

            for (int i = 0; i < nevezlista.versenyzoadatok.Count; i++)
                for (int j = 0; j < nevezlista.versenyzoadatok.Count-1; j++)
                    if (nevezlista.versenyzoadatok[i].INNEVE.CompareTo(nevezlista.versenyzoadatok[j].INNEVE)!=-1)
                    {
                        Node_NevezésiLista.Versenyzoadatok temp = nevezlista.versenyzoadatok[i];
                        nevezlista.versenyzoadatok[i] = nevezlista.versenyzoadatok[j];
                        nevezlista.versenyzoadatok[j] = temp;
                    }

                    for (int i = 1; i < nevezlista.versenyadatok.VEINSZ + 1; ++i)
                    {
                        table.Rows[table.Rows.Count - i].Cells[1].Paragraphs[0].Append(nevezlista.versenyzoadatok[i - 1].INNEVE);
                        table.Rows[table.Rows.Count - i].Cells[0].Paragraphs[0].Append(nevezlista.versenyzoadatok[i - 1].INSOSZ.ToString());
                        table.Rows[table.Rows.Count - i].Cells[2].Paragraphs[0].Append(nevezlista.versenyzoadatok[i - 1].ITMEGN);
                        table.Rows[table.Rows.Count - i].Cells[3].Paragraphs[0].Append((nevezlista.versenyzoadatok[i - 1].INSZUL.ToString()));
                        table.Rows[table.Rows.Count - i].Cells[4].Paragraphs[0].Append(nevezlista.versenyzoadatok[i - 1].INEGYE);
                        table.Rows[table.Rows.Count - i].Cells[5].Paragraphs[0].Append(nevezlista.versenyzoadatok[i - 1].INCSSZ.ToString());
                    }
            #endregion

            document.InsertTable(table);

                try { document.Save(); }
                catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "NEVEZLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error);}
                return filename;
            
        }

        static public string nyomtat_csapatlista(string _VEAZON)
        {
            string filename = null;
            Node_Csapatlista csapatlista = new Node_Csapatlista();
            csapatlista.versenyzoadatok = new List<Node_Csapatlista.Versenyzoadatok>();

            #region alap stringek
            string headline = "C S A P A T  L I S T A";
            string st_vazon_vnev = "Verseny azonosító, név: ";
            string st_ido = "Verseny ideje: ";
            string st_vosszp = "Verseny össz pontszám: ";
            string st_insz = "Indulók száma: ";
            string st_vsorazon = "Versenysorozat azonosító, név: ";
            #endregion

            #region adat

            List<Verseny> versenyek = Program.database.Versenyek();
            List<Versenysorozat> versenysorozatok = Program.database.Versenysorozatok();

            foreach (Verseny outer in versenyek)
            {
                if (outer.azonosító == _VEAZON && versenysorozatok.Count!=0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (outer.versenysorozat==inner.azonosító )
                        {
                            csapatlista.versenyadatok = new Node_Csapatlista.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.azonosító == _VEAZON)
                {
                    csapatlista.versenyadatok = new Node_Csapatlista.Versenyadatok(outer);
                }
            }

            List<Íjtípus> íjtípusok = Program.database.Íjtípusok();
            List<Induló> indulók = Program.database.Indulók();
            List<Eredmény> eredmények = Program.database.Eredmények(_VEAZON);

            int megjelent = 0;

            foreach (Eredmény item_eredmény in eredmények)
            {
                if (item_eredmény.megjelent == true)
                {
                    megjelent++;
                    foreach (Induló item_indulók in indulók)
                    {
                        if (item_indulók.név == item_eredmény.név)
                        {
                            foreach (Íjtípus item_íjtípus in íjtípusok)
                            {
                                if (item_eredmény.íjtípus == item_íjtípus.azonosító)
                                {
                                    csapatlista.versenyzoadatok.Add(new Node_Csapatlista.Versenyzoadatok(item_íjtípus, item_indulók, item_eredmény));
                                }
                            }
                        }
                    }
                }
            }
            csapatlista.versenyadatok.VEINSZ = megjelent;

            for (int i = 0; i < csapatlista.versenyzoadatok.Count; i++)
                for (int j = 0; j < i; j++)
                    if (csapatlista.versenyzoadatok[i].INCSSZ < csapatlista.versenyzoadatok[j].INCSSZ)
                    {
                        Node_Csapatlista.Versenyzoadatok temp = csapatlista.versenyzoadatok[i];
                        csapatlista.versenyzoadatok[i] = csapatlista.versenyzoadatok[j];
                        csapatlista.versenyzoadatok[j] = temp;
                    }
            #endregion

            if (csapatlista.versenyadatok.VSAZON!=null)
            {
                filename = csapatlista.versenyadatok.VSAZON + "\\" + _VEAZON + "\\" + "CSAPATLISTA.docx";
            }
            else
            {
                filename = _VEAZON + "\\" + "CSAPATLISTA.docx";
            }

            var document = DocX.Create(filename);
            document.AddHeaders();
            
            #region címbekezdés

            var titleFormat = new Formatting();
            titleFormat.Size = 14D;
            titleFormat.Position = 1;
            titleFormat.Spacing = 5;
            titleFormat.Bold = true;

            Header header = document.Headers.odd;

            Paragraph title = header.InsertParagraph();
            title.Append(headline);
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
            paragraph_1.Append(st_vazon_vnev);

            paragraph_1.Append(_VEAZON + ", " + csapatlista.versenyadatok.VEMEGN);
            paragraph_1.Bold();
            titleFormat2.Bold = false;
            paragraph_1.Append("\n" + st_ido);
            paragraph_1.Append(csapatlista.versenyadatok.VEDATU);
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + st_vosszp);
            paragraph_1.Append((csapatlista.versenyadatok.VEOSPO * 10).ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\t\t" + st_insz);
            paragraph_1.Append(csapatlista.versenyadatok.VEINSZ.ToString());
            paragraph_1.Bold();
            paragraph_1.Append("\n" + st_vsorazon);
            paragraph_1.Append(csapatlista.versenyadatok.VSAZON + ", " + csapatlista.versenyadatok.VSMEGN);
            paragraph_1.Bold();
            paragraph_1.AppendLine();
            #endregion

            #region táblázat formázás

            Table table = document.AddTable(1, 6);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Csapat");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Sorszám");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Név");
            table.Rows[0].Cells[3].Paragraphs[0].Append("Íjtípus");
            table.Rows[0].Cells[4].Paragraphs[0].Append("Kor");
            table.Rows[0].Cells[5].Paragraphs[0].Append("Egyesület");

            Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Blue);
            Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.seven, 0, Color.Black);

            table.SetBorder(TableBorderType.InsideH, b);
            table.SetBorder(TableBorderType.InsideV, b);
            table.SetBorder(TableBorderType.Bottom, b);
            table.SetBorder(TableBorderType.Top, b);
            table.SetBorder(TableBorderType.Left, b);
            table.SetBorder(TableBorderType.Right, b);


            for (int i = 0; i < 6; i++)
            {
                table.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, c);
            }
            #endregion

            #region táblázat adatok betöltése
            int q = 1;
            int tempcs = 0;

            for (int i = 0; i < csapatlista.versenyadatok.VEINSZ; i++)
                for (int j = 0; j < csapatlista.versenyadatok.VEINSZ-1; j++)
                    if (csapatlista.versenyzoadatok[i].INNEVE.CompareTo(csapatlista.versenyzoadatok[j].INNEVE) == -1 && csapatlista.versenyzoadatok[i].INCSSZ == csapatlista.versenyzoadatok[j].INCSSZ)
                    {
                        Node_Csapatlista.Versenyzoadatok temp = csapatlista.versenyzoadatok[i];
                        csapatlista.versenyzoadatok[i] = csapatlista.versenyzoadatok[j];
                        csapatlista.versenyzoadatok[j] = temp;
                    }


            foreach (Node_Csapatlista.Versenyzoadatok  item in csapatlista.versenyzoadatok)
            {
                if (tempcs == 0 || tempcs == item.INCSSZ)
                {
                    table.InsertRow();
                    table.Rows[q].Cells[0].Paragraphs[0].Append(item.INCSSZ.ToString());
                    table.Rows[q].Cells[1].Paragraphs[0].Append(item.INSOSZ.ToString());
                    table.Rows[q].Cells[2].Paragraphs[0].Append(item.INNEVE);
                    table.Rows[q].Cells[3].Paragraphs[0].Append(item.ITMEGN);
                    table.Rows[q].Cells[4].Paragraphs[0].Append(item.INSZUL.ToString());
                    table.Rows[q].Cells[5].Paragraphs[0].Append(item.INEGYE);
                    tempcs = item.INCSSZ;
                    q++;
                }
                else
                {
                    document.InsertTable(table);
                    document.InsertSectionPageBreak();
                    table = document.AddTable(1, 6);
                    #region táblázat 1.sor

                    table.Rows[0].Cells[0].Paragraphs[0].Append("Csapat");
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Sorszám");
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Név");
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Íjtípus");
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Kor");
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Egyesület");
                    #endregion
                    #region táblázat formázás
                    table.SetBorder(TableBorderType.InsideH, b);
                    table.SetBorder(TableBorderType.InsideV, b);
                    table.SetBorder(TableBorderType.Bottom, b);
                    table.SetBorder(TableBorderType.Top, b);
                    table.SetBorder(TableBorderType.Left, b);
                    table.SetBorder(TableBorderType.Right, b);

                    for (int i = 0; i < 6; i++)
                    {
                        table.Rows[0].Cells[i].SetBorder(TableCellBorderType.Bottom, c);
                    }
                    #endregion

                    table.Alignment = Alignment.center;
                    table.InsertRow();
                    q = 1;
                    table.Rows[q].Cells[0].Paragraphs[0].Append(item.INCSSZ.ToString());
                    table.Rows[q].Cells[1].Paragraphs[0].Append(item.INSOSZ.ToString());
                    table.Rows[q].Cells[2].Paragraphs[0].Append(item.INNEVE);
                    table.Rows[q].Cells[3].Paragraphs[0].Append(item.ITMEGN);
                    table.Rows[q].Cells[4].Paragraphs[0].Append(item.INSZUL.ToString());
                    table.Rows[q].Cells[5].Paragraphs[0].Append(item.INEGYE);
                    tempcs = item.INCSSZ;
                    q++;
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i].Cells[0].Width = 150;
                    table.Rows[i].Cells[1].Width = 70;
                    table.Rows[i].Cells[2].Width = 200;
                    table.Rows[i].Cells[3].Width = 100;
                    table.Rows[i].Cells[4].Width = 40;
                    table.Rows[i].Cells[5].Width = 150;
                }
                table.AutoFit = AutoFit.ColumnWidth;

            }
            document.InsertTable(table);
            #endregion

            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "CSAPATLISTA.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            return filename;
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
                if (outer.azonosító == _VEAZON && versenysorozatok.Count != 0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító == outer.versenysorozat)
                        {
                            verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.azonosító == _VEAZON)
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
                if (outer.azonosító == _VEAZON && versenysorozatok.Count != 0)
                {
                    foreach (Versenysorozat inner in versenysorozatok)
                    {
                        if (inner.azonosító == outer.versenysorozat)
                        {
                            verseny.versenyadatok = new Node_Eredménylap_Verseny_Teljes.Versenyadatok(outer, inner);
                        }
                    }
                }
                else if (outer.azonosító == _VEAZON)
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
            EredmenylapVersenyEgyesulet Data = new EredmenylapVersenyEgyesulet(_VEAZON);
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

        static public Node_Eredménylap_VersenySorozat_Teljes versenysorozat_adatok(string _VSAZON, int _limit, bool _teljes)
        {
            Node_Eredménylap_VersenySorozat_Teljes versenysorozat = new Node_Eredménylap_VersenySorozat_Teljes();
            versenysorozat.versenyazonosítók = new List<string>();
            versenysorozat.íjtípus = new List<Node_Eredménylap_VersenySorozat_Teljes.Node_Íjtípus>();

            List<Verseny> versenyek = Program.database.Versenyek();
            foreach (Verseny item in versenyek)
            {
                if (item.versenysorozat == _VSAZON){ versenysorozat.versenyazonosítók.Add(item.azonosító);}
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
            filename = _filename;
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

        #region event handlers
        static void igen_Click(object sender, EventArgs e)
        {
            Nyomtat.print(filename);
            dialog.DialogResult = DialogResult.OK;
        }
        static void megnyit_Click(object sender, EventArgs e)
        {
            Process.Start(filename);
        }
        static void nem_Click(object sender, EventArgs e)
        {
            dialog.DialogResult = DialogResult.OK;
            return;
        }
        #endregion
    }
}
    

