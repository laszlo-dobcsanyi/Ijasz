using Novacode;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Íjász {
    public partial class Nyomtat {

        /// <summary>kiszedi az első _Limit eredményt, meghívja rájuk a NyomtatOklevelVersenyVersenyzo fgv-t </summary>
        static public void NyomtatOklevelVerseny( string _VEAZON, Oklevel _Oklevel, int _Limit ) {
            List<OKLEVELVERSENYZO> versenyzok = new List<OKLEVELVERSENYZO>();
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

        static public string NyomtatOklevelVersenyVersenyzo( Oklevel _Oklevel, OKLEVELVERSENYZO _Versenyzo ) {
            //TODO(mate): egyedi filename? egyesével lehessen nyomtatni....?????
            //NOTE(mate): _oklevelből: 4 * (x,y)
            //NOTE(mate): az oklevel sablont le kell sortolni y szerint!
            string filename = null;
            var document = DocX.Create("oklevel_" + _Versenyzo.Nev + ".docx");
            //MessageBox.Show("width: " + document.PageWidth + "\nheight: " + document.PageHeight);


            /* oldal ||  w(cm)  |  h(cm)  ||  w  |  h   ||
                A4   ||   210   |   297   || 793 | 1122 ||
                A5   ||   148   |   210   || 558 | 793  ||

            */

            //A5
            document.PageHeight = 793;
            document.PageWidth = 558;

            document.MarginBottom = 1;
            document.MarginLeft = 9;
            document.MarginRight = 9;
            document.MarginTop = 1;
            


            Table table = document.AddTable(4, 2);

            const int MM_OLDAL_SZELESSEG = 210;
            const int MM_OLDAL_MAGASSAG = 297;

            const int OLDAL_SZELESSEG = 558;
            const int OLDAL_MAGASSAG = 793;


            table.Rows[0].Cells[0].Width = 0;
            table.Rows[0].Cells[1].Width = 0;

            table.Rows[1].Cells[0].Width = 0;
            table.Rows[1].Cells[1].Width = 0;

            table.Rows[2].Cells[0].Width = 0;
            table.Rows[2].Cells[1].Width = 0;

            table.Rows[3].Cells[0].Width = 0;
            table.Rows[3].Cells[1].Width = 0;



            table.Rows[0].Cells[0].Width = OLDAL_SZELESSEG;
            table.Rows[0].Height = OLDAL_MAGASSAG;

            table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Bottom;

            document.InsertTable( table );
            try { document.Save( ); }
            catch( System.Exception ) { MessageBox.Show( "A dokumentum meg van nyitva!", "ERLAPVETELJ.DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error ); }

            return filename;
        }
    }
}
