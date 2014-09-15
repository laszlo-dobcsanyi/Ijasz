using System;

namespace Íjász
{
    public static partial class Program
    {
        /*
        #region Spammer
        public static Random SpammerRandom = new Random();

        #region ValueGenerators
        public static Int32 RandomInt(int _max_value)
        {
            return SpammerRandom.Next(_max_value);
        }

        public static string RandomString(int _max_length)
        {
            string value = "";
            string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";

            int length = RandomInt(_max_length - 1) + 1;
            while (length-- >= 0)
            {
                value += charset[RandomInt(charset.Length)];
            }

            return value;
        }

        public static string RandomSQLString(int _max_length)
        {
            string value = "";

            do
            {
                value = RandomString(_max_length);
            } while (!Database.IsCorrectSQLText(value) || value.Contains(" "));

            return value;
        }

        public static string RandomDate()
        {
            return (SpammerRandom.Next(1900, 2014 + 1).ToString() + "." + SpammerRandom.Next(1, 12 + 1).ToString() + "." + SpammerRandom.Next(1, 28 + 1).ToString());
        }

        public static bool RandomBool()
        {
            return (SpammerRandom.Next(0, 1 + 1) == 0);
        }

        #endregion

        #region DataGenerators
        public static string RandomVersenysorozat()
        {
            List<Versenysorozat> versenysorozatok = database.Versenysorozatok();

            if (versenysorozatok.Count == 0)
                return "";
            else
                return versenysorozatok[RandomInt(versenysorozatok.Count)].azonosító;
        }

        public static string RandomVerseny()
        {
            string value = null;
            List<Verseny> versenyek = database.Versenyek_Aktív();

            if (versenyek.Count == 0)
                return value;
            else
            {
                value = versenyek[RandomInt(versenyek.Count)].azonosító;
                return value;
            }
        }

        public static string RandomÍjtípus()
        {
            string value = null;
            List<Íjtípus> íjtípusok = database.Íjtípusok();

            if (íjtípusok.Count == 0)
                return value;
            else
            {
                value = íjtípusok[RandomInt(íjtípusok.Count)].azonosító;
                return value;
            }
        }

        public static string RandomInduló()
        {
            string value = null;
            List<Induló> indulók = database.Indulók();

            if (indulók.Count == 0)
                return value;
            else
            {
                value = indulók[RandomInt(indulók.Count)].név;
                return value;
            }
        }
        #endregion

        public static int SpammerCount = 0;
        public static void Spam_Data()
        {
            SpammerCount++;

            if (SpammerCount <= 200)
            {
                int type = RandomInt(410 + 1);

                if (0 <= type && type <= 20) Spam_Versenysorozat_Hozzáadás();
                if (21 <= type && type <= 42) Spam_Verseny_Hozzáadás();
                if (43 <= type && type <= 64) Spam_Íjtípus_Hozzáadás();
                if (65 <= type && type <= 70) Spam_Induló_Hozzáadás();
                if (71 <= type && type <= 74) Spam_Eredmény_Hozzáadás();

                Spam_Eredmény_Módosítás();
            }
            else
                if (SpammerCount <= 500)
                {
                    int type = RandomInt(410 + 1);

                    if (0 <= type && type <= 1) Spam_Versenysorozat_Hozzáadás();
                    if (2 <= type && type <= 4) Spam_Verseny_Hozzáadás();
                    if (5 <= type && type <= 6) Spam_Íjtípus_Hozzáadás();
                    if (7 <= type && type <= 108) Spam_Induló_Hozzáadás();
                    if (109 <= type && type <= 410) Spam_Eredmény_Hozzáadás();
                }
                else
                {
                    int type = RandomInt(1010 + 1);

                    if (0 <= type && type <= 1) Spam_Versenysorozat_Hozzáadás();
                    if (2 <= type && type <= 4) Spam_Verseny_Hozzáadás();
                    if (5 <= type && type <= 6) Spam_Íjtípus_Hozzáadás();
                    if (7 <= type && type <= 108) Spam_Induló_Hozzáadás();
                    if (109 <= type && type <= 210) Spam_Eredmény_Hozzáadás();

                    if (211 <= type && type <= 410) Spam_Versenysorozat_Módosítás();
                    if (411 <= type && type <= 610) Spam_Verseny_Módosítás();
                    if (611 <= type && type <= 810) Spam_Induló_Módosítás();
                    if (811 <= type && type <= 1010) Spam_Eredmény_Módosítás();
                }
        }

        #region Spam_Hozzáadás
        public static void Spam_Versenysorozat_Hozzáadás()
        {
            mainform.versenysorozat_panel.Versenysorozat_Hozzáadás(new Versenysorozat(RandomString(10), RandomString(30), 0));
        }

        public static void Spam_Verseny_Hozzáadás()
        {
            // Versenysorozathoz tartozik, vagy nem!
            if (RandomBool())
                mainform.verseny_panel.Verseny_Hozzáadás(new Verseny(RandomString(10), RandomString(30), RandomDate(), "", RandomInt(300), 0, false));
            else
            {
                string versenysorozat = RandomVersenysorozat();
                mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(versenysorozat);
                mainform.verseny_panel.Verseny_Hozzáadás(new Verseny(RandomString(10), RandomString(30), RandomDate(), versenysorozat, RandomInt(300), 0, false));
            }
        }

        public static void Spam_Íjtípus_Hozzáadás()
        {
            mainform.íjtípusok_panel.Íjtípus_Hozzáadás(new Íjtípus(RandomString(10), RandomString(30), RandomInt(100000), 0));
        }

        public static void Spam_Induló_Hozzáadás()
        {
            mainform.indulók_panel.Induló_Hozzáadás(new Induló(RandomString(30), ((RandomBool()) ? "N" : "F"), RandomDate(), RandomString(30), RandomString(30), 0));
        }

        public static void Spam_Eredmény_Hozzáadás()
        {
            string verseny = RandomVerseny(); if (verseny == null) return;
            string íjtípus = RandomÍjtípus(); if (íjtípus == null) return;
            string induló = RandomInduló(); if (induló == null) return;

            int összespont = database.Verseny_Összespont(verseny);

            // 0-0-0-0 -val, vagy stimmeljen!!
            if (RandomBool())
                mainform.eredmények_panel.Eredmény_Hozzáadás(verseny, new Eredmény(induló, 0, íjtípus, RandomInt(35) + 1,
                    0, 0, 0, 0, 0, 0, RandomBool()));
            else
            {
                int pont_10 = RandomInt(összespont + 1);
                int pont_8 = RandomInt(összespont - pont_10 + 1);
                int pont_5 = RandomInt(összespont - pont_10 - pont_8 + 1);
                int mellé = összespont - pont_10 - pont_8 - pont_5;
                int pont = pont_10 * 10 + pont_8 * 8 + pont_5 * 5;
                int százalék = (int)(((double)pont / (összespont * 10)) * 100);
                mainform.eredmények_panel.Eredmény_Hozzáadás(verseny, new Eredmény(induló, 0, íjtípus, RandomInt(35) + 1,
                    pont_10, pont_8, pont_5, mellé, pont, százalék, RandomBool()));
            }
        }
        #endregion

        #region Spam_Módosítás
        public static void Spam_Versenysorozat_Módosítás()
        {
            string versenysorozat_azonosító = RandomVersenysorozat(); if (versenysorozat_azonosító == null) return;
            Nullable<Versenysorozat> versenysorozat = database.Versenysorozat(versenysorozat_azonosító); if (versenysorozat == null) return;

            if (versenysorozat.Value.versenyek == 0)
                mainform.versenysorozat_panel.Versenysorozat_Módosítás(versenysorozat_azonosító, new Versenysorozat(RandomSQLString(10), RandomString(30), versenysorozat.Value.versenyek));
            else
                mainform.versenysorozat_panel.Versenysorozat_Módosítás(versenysorozat_azonosító, new Versenysorozat(versenysorozat_azonosító, RandomString(30), versenysorozat.Value.versenyek));
        }

        public static void Spam_Verseny_Módosítás()
        {
            string verseny_azonosító = RandomVerseny(); if (verseny_azonosító == null) return;
            Nullable<Verseny> verseny = database.Verseny(verseny_azonosító); if (verseny == null) return;

            if (verseny.Value.indulók == 0)
            {
                string versenysorozat = RandomVersenysorozat(); if (versenysorozat == null) return;
                if (versenysorozat != verseny.Value.versenysorozat)
                {
                    Program.mainform.versenysorozat_panel.Versenysorozat_VersenyCsökkentés(verseny.Value.versenysorozat);
                    Program.mainform.versenysorozat_panel.Versenysorozat_VersenyNövelés(versenysorozat);

                    mainform.verseny_panel.Verseny_Módosítás(verseny.Value.azonosító, new Verseny(RandomSQLString(10), RandomString(30), RandomDate(),
                        versenysorozat, RandomInt(300), 0, RandomBool()));
                }
            }
            else
            {
                mainform.verseny_panel.Verseny_Módosítás(verseny.Value.azonosító, new Verseny(verseny.Value.azonosító, RandomString(30), RandomDate(),
                        verseny.Value.versenysorozat, verseny.Value.összes, verseny.Value.indulók, verseny.Value.lezárva));
            }
        }

        public static void Spam_Induló_Módosítás()
        {
            string induló_név = RandomInduló(); if (induló_név == null) return;
            Nullable<Induló> induló = database.Induló(induló_név); if (induló == null) return;

            if (induló.Value.eredmények == 0)
                mainform.indulók_panel.Induló_Módosítás(induló_név, new Induló(RandomString(30), RandomBool() ? "N" : "F", RandomDate(), RandomString(30), RandomString(30), 0));
            else
                mainform.indulók_panel.Induló_Módosítás(induló_név, new Induló(induló_név, RandomBool() ? "N" : "F", RandomDate(), RandomString(30), RandomString(30), induló.Value.eredmények));
        }

        public static void Spam_Eredmény_Módosítás()
        {
            string verseny = RandomVerseny(); if (verseny == null) return;
            List<Eredmény> eredmények = database.Eredmények(verseny);

            if (eredmények.Count == 0) return;
            Eredmény eredmény = eredmények[RandomInt(eredmények.Count)];

            string íjtípus = RandomÍjtípus(); if (íjtípus == null) return;
            // TODO ÍJTÍPUS MÓDOSÍTÁS!!

            int összespont = database.Verseny_Összespont(verseny);
            int pont_10 = RandomInt(összespont + 1);
            int pont_8 = RandomInt(összespont - pont_10 + 1);
            int pont_5 = RandomInt(összespont - pont_10 - pont_8 + 1);
            int mellé = összespont - pont_10 - pont_8 - pont_5;
            int pont = pont_10 * 10 + pont_8 * 8 + pont_5 * 5;
            int százalék = (int)(((double)pont / (összespont * 10)) * 100);
            mainform.eredmények_panel.Eredmény_Módosítás(verseny, eredmény, new Eredmény(eredmény.név, eredmény.sorszám, eredmény.íjtípus, RandomInt(35) + 1, pont_10, pont_8, pont_5, mellé, pont, százalék,RandomBool()));
        }
        #endregion
        #endregion
        */
    }
}
