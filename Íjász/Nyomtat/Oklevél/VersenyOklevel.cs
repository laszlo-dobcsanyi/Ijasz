using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Novacode;
using System.Windows.Forms;
using System.IO;

namespace Íjász {
    public partial class Nyomtat {

        /// <summary>kiszedi az első _Limit eredményt, meghívja rájuk a NyomtatOklevelVersenyVersenyzo fgv-t </summary>
        static public void NyomtatOklevelVerseny(string _VEAZON, Oklevel _Oklevel, int _Limit) {
            List<OKLEVELVERSENYZO> versenyzok = new List<OKLEVELVERSENYZO>();
            List<Eredmény> eredmenyek = (Program.database.Eredmények(_VEAZON).OrderBy(eredmeny => eredmeny.Osszpont).Take(_Limit)).ToList();
            var indulok = Program.database.Indulók();
            string versenydatum = Program.database.Verseny(_VEAZON).Value.Datum;
            List<OKLEVELVERSENYZO> eredmenyek2 = (from indulo in indulok
                                                  join eredmeny in eredmenyek on indulo.Nev equals eredmeny.Nev
                                                  select new OKLEVELVERSENYZO {
                                                      Nev = indulo.Nev,
                                                      Egyesulet = indulo.Egyesulet,
                                                      Helyezes = 0,
                                                      Datum = versenydatum
                                                  }).ToList();

            var q = eredmenyek2.ToArray();

            for (int i = 0; i < q.Count(); ++i) {
                q[i].Helyezes = i + 1;
                NyomtatOklevelVersenyVersenyzo(_Oklevel, q[i]);
            }
        }

        static public string NyomtatOklevelVersenyVersenyzo(Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo) {
            //TODO(mate): egyedi filename? egyesével lehessen nyomtatni....?????
            //NOTE(mate): _oklevelből: 4 * (x,y)
            string filename = null;
            var document = DocX.Create("oklevel_" + _Versenyzo.Nev + ".docx");

            document.MarginBottom   = 1;
            document.MarginLeft     = 9;
            document.MarginRight    = 9;
            document.MarginTop      = 1;

            Table table = document.AddTable(1, 2);
            table.Rows[0].Height = 100;
            table.Rows[0].Cells[0].Width = 100;
            table.Rows[0].Cells[1].Width = 200;
            table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            
            try { document.Save(); }
            catch (System.Exception) { MessageBox.Show("A dokumentum meg van nyitva!", "ERLAPVETELJ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error); }
           
            return filename;
        }
    }
}
