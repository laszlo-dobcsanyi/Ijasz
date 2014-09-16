using System;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
    public sealed class Database :IDisposable
    {
        public static int Verzió = 2;
        private SQLiteConnection connection;

        public Database()
        {
            connection = new SQLiteConnection("Data Source=adat.db; Version=3; New=False; Compress=True;");

            //Check if database not exists
            if (!File.Exists("adat.db"))
            {
                //Create database
                SQLiteConnection.CreateFile("adat.db");
                connection.Open();

                //Create tables
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE Verzió (PRVERZ int);" +
                    "CREATE TABLE Versenysorozat (VSAZON char(10) PRIMARY KEY, VSMEGN char(30), VSVESZ int);" +
                    "CREATE TABLE Verseny (VEAZON char(10) PRIMARY KEY, VEMEGN char(30), VEDATU char(20), VSAZON char(10), VEOSPO int NOT NULL, VEALSZ int, VEINSZ int, VELEZAR boolean);" +
                    "CREATE TABLE Korosztályok (VEAZON char(10) NOT NULL, KOAZON char(10) NOT NULL, KOMEGN char(30), KOEKMI int NOT NULL, KOEKMA int NOT NULL, KOINSF int, KOINSN int);" +
                    "CREATE TABLE Íjtípusok (ITAZON char(10) PRIMARY KEY, ITMEGN char(30), ITLISO int, ITERSZ int);" +
                    "CREATE TABLE Indulók (INNEVE char(30) PRIMARY KEY, INNEME char(1) NOT NULL, INSZUL char(20) NOT NULL, INVEEN char(30), INEGYE char(30), INERSZ int);" +

                    "INSERT INTO Verzió (PRVERZ) VALUES (" + Verzió + ");";

                if (command.ExecuteNonQuery() != 0) MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show("Adatbázis létrehozva!", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                command.Dispose();
                connection.Close();

                // Biztonsági mentések mappája

                if (!Directory.Exists(@"backup")) Directory.CreateDirectory(@"backup");
            }
            else
            {
                CreateBackup("adat_indítás_" + DateTime.Now.ToString().Trim(new Char[] { '-' }).Replace(' ', '_').Replace('.', '-').Replace(':', '-'));
            }
        }

        public void CreateBackup(string _name)
        {
            if (!Directory.Exists(@"backup")) Directory.CreateDirectory(@"backup");

            lock (Program.datalock)
            {
                SQLiteConnection cnnOut = new SQLiteConnection("Data Source=backup\\" + _name + ".db;foreign keys=True");
                connection.Open();
                cnnOut.Open();
                connection.BackupDatabase(cnnOut, "main", "main", -1, null, -1);
                connection.Close();
                cnnOut.Close();
            }
        }

        public static bool IsCorrectSQLText(string _text)
        {
            if (_text.Contains("'") || _text.Contains("\"") || _text.Contains("(") || _text.Contains(")") || _text.Contains(";") ) return false;
            return true;
        }

        public bool IsCorrectVersion()
        {
            int version = 0;
            SQLiteCommand command;

            connection.Open();

            command = connection.CreateCommand();
            command.CommandText = "SELECT PRVERZ FROM Verzió;";
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    version = reader.GetInt32(0);
                }
            }
            catch (SQLiteException) { version = 1; }
            catch (Exception excp) { MessageBox.Show(excp.Message); return false; }
            finally
            {
                command.Dispose();
                connection.Close();
            }

            if (Verzió == version) return true;
            return false;
        }

        #region Versenysorozat

        // TODO EXTRA, CSAK SPAMERNEK KELL!!
        public Nullable<Versenysorozat> Versenysorozat(string _azonosító)
        {
            lock (Program.datalock)
            {

                Nullable<Versenysorozat> data = null;
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data = new Versenysorozat(reader.GetString(0), reader.GetString(1), reader.GetInt32(2));
                }

                command.Dispose();
                connection.Close();
                return data;
            }
        }

        public List<Versenysorozat> Versenysorozatok()
        {
            lock (Program.datalock)
            {

                List<Versenysorozat> data = new List<Versenysorozat>();

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Versenysorozat(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
                }

                command.Dispose();
                connection.Close();
                return data;
            }
        }

        public bool ÚjVersenysorozat(Versenysorozat _versenysorozat)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Versenysorozat (VSAZON, VSMEGN, VSVESZ) VALUES('" + _versenysorozat.azonosító + "', '" + _versenysorozat.megnevezés + "', 0);";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                Directory.CreateDirectory(_versenysorozat.azonosító);

                return true;
            }
        }

        public bool VersenysorozatMódosítás(string _azonosító, Versenysorozat _versenysorozat)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Versenysorozat SET VSAZON = '" + _versenysorozat.azonosító + "', VSMEGN = '" + _versenysorozat.megnevezés + "' WHERE VSAZON = '" + _azonosító + "';";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if (_azonosító != _versenysorozat.azonosító && Directory.GetFiles(_azonosító).Length == 0)
                    {
                        Directory.Delete(_azonosító);
                        Directory.CreateDirectory(_versenysorozat.azonosító);
                    }
                }
                return true;
            }
        }

        public bool VersenysorozatTörlés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Versenysorozat WHERE VSAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                if (Directory.GetFiles(_azonosító).Length == 0) { Directory.Delete(_azonosító); }

                return true;
            }
        }

        ///

        public bool Versenysorozat_VersenyekNövel(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ + 1 WHERE VSAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        public bool Versenysorozat_VersenyekCsökkent(string _azonosító)
        {
            lock (Program.datalock)
            {

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ - 1 WHERE VSAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }
        #endregion

        #region Verseny
        // TODO CSAK A SPAMMERNEK KELL!!
        public Nullable<Verseny> Verseny(string _azonosító)
        {
            lock (Program.datalock)
            {
                Nullable<Verseny> data = null;

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data = new Verseny(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetBoolean(7));
                }

                command.Dispose();
                connection.Close();
                return data;
            }
        }
        public List<Verseny> Versenyek()
        {
            lock (Program.datalock)
            {
                List<Verseny> data = new List<Verseny>();

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR FROM Verseny;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Verseny(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetBoolean(7)));
                }

                command.Dispose();
                connection.Close();
                return data;
            }
        }

        public List<Verseny> Versenyek_Aktív()
        {
            lock (Program.datalock)
            {
                List<Verseny> data = new List<Verseny>();

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR FROM Verseny WHERE VELEZAR = 0;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Verseny(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetBoolean(7)));
                }

                command.Dispose();
                connection.Close();
                return data;
            }
        }

        public bool ÚjVerseny(Verseny _verseny)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Verseny (VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR) VALUES('" + _verseny.azonosító + "', '" + _verseny.megnevezés + "', " +
                    "'" + _verseny.dátum + "', '" + _verseny.versenysorozat + "', " + _verseny.összes + ", " + _verseny.állomások + ", 0, 0);";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    command.Dispose();
                    connection.Close();
                    return false;
                }


                command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KOINSF, KOINSN) VALUES" +
                    "('" + _verseny.azonosító + "', 'K10', '0-10', 1, 9, 0, 0)," +
                    "('" + _verseny.azonosító + "', 'K14', '10-14', 10, 13, 0, 0)," +
                    "('" + _verseny.azonosító + "', 'K18', '14-18', 14, 17, 0, 0)," +
                    "('" + _verseny.azonosító + "', 'K50', '18-50', 18, 49, 0, 0)," +
                    "('" + _verseny.azonosító + "', 'K100', '50-100', 50, 99, 0, 0);";
                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE Eredmények_" + _verseny.azonosító + " (INNEVE  char(30) NOT NULL, INSOSZ INTEGER PRIMARY KEY AUTOINCREMENT, ITAZON char(10), INCSSZ int, IN10TA int," +
                    "IN08TA int, IN05TA int, INMETA int, INOSZP int, INERSZ int, INMEGJ boolean);";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                if (_verseny.versenysorozat != "") { Directory.CreateDirectory(_verseny.versenysorozat + "\\" + _verseny.azonosító); }
                else { Directory.CreateDirectory(_verseny.azonosító); }

                return true;
            }
        }

        public bool VersenyMódosítás(string _azonosító, Verseny _verseny)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Verseny SET VEAZON = '" + _verseny.azonosító + "', VEMEGN = '" + _verseny.megnevezés + "', VEDATU = '" + _verseny.dátum + "' ," +
                    "VSAZON = '" + _verseny.versenysorozat + "', VEOSPO = " + _verseny.összes + ", VEALSZ = " + _verseny.állomások + " WHERE VEAZON = '" + _azonosító + "';";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    command.Dispose();
                    connection.Close();
                    return false;
                }

                if (_azonosító != _verseny.azonosító)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "UPDATE Korosztályok SET VEAZON = '" + _verseny.azonosító + "' WHERE VEAZON = '" + _azonosító + "';";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = "ALTER TABLE Eredmények_" + _azonosító + " RENAME TO Eredmények_" + _verseny.azonosító + ";";
                    command.ExecuteNonQuery();
                }

                command.Dispose();
                connection.Close();
                if (_azonosító != _verseny.azonosító)
                {
                    try
                    {
                        Directory.Delete(_azonosító);
                        Directory.CreateDirectory(_verseny.azonosító);
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                }
                return true;
            }
        }

        public bool VersenyTörlés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Korosztályok WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = "DROP TABLE Eredmények_" + _azonosító + ";";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                try
                {
                    Directory.Delete(_azonosító);
                }
                catch (Exception)
                {
                    return true;
                }

                return true;
            }
        }

        ///

        public bool Verseny_IndulókNövelés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ + 1 WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        public bool Verseny_IndulókCsökkentés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ - 1 WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        public int? Verseny_Állomások(string _azonosító)
        {
            lock (Program.datalock)
            {
                int? value = null;
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VEALSZ FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    value = reader.GetInt32(0);
                }

                command.Dispose();
                connection.Close();
                return value;
            }
        }

        public int Verseny_Összespont(string _azonosító)
        {
            lock (Program.datalock)
            {
                int összpont = -666;
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    összpont = reader.GetInt32(0);
                }

                command.Dispose();
                connection.Close();
                return összpont;
            }
        }

        public bool Verseny_Lezárva(string _azonosító)
        {
            lock (Program.datalock)
            {
                bool lezárva = false;
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VELEZAR FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lezárva = reader.GetBoolean(0);
                }

                command.Dispose();
                connection.Close();
                return lezárva;
            }
        }
        
        public bool Verseny_Lezárás(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Verseny SET VELEZAR = 1 WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        public bool Verseny_Megnyitás(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Verseny SET VELEZAR = 0 WHERE VEAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        #endregion

        #region Korosztályok
        public List<Korosztály> Korosztályok(string _verseny)
        {
            lock (Program.datalock)
            {
                List<Korosztály> data = new List<Korosztály>();
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KOINSF, KOINSN FROM Korosztályok WHERE VEAZON = '" + _verseny + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Korosztály(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6)));
                }

                command.Dispose();
                connection.Close();

                return data;
            }
        }

        public bool ÚjKorosztály(Korosztály _korosztály)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.verseny + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                bool found = false;
                while (reader.Read())
                {
                    if (_korosztály.azonosító == reader.GetString(0))
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    command.Dispose();
                    connection.Close();
                    return false;
                }

                command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KOINSF, KOINSN) VALUES('" + _korosztály.verseny + "', '" + _korosztály.azonosító + "', '" + _korosztály.megnevezés + "', " +
                     +_korosztály.alsó_határ + ", " + _korosztály.felső_határ + ", " + _korosztály.indulók_férfiak + ", " + _korosztály.indulók_nők + ");";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

                return true;
            }
        }

        public bool KorosztályMódosítás(string _azonosító, Korosztály _korosztály)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                if (_azonosító != _korosztály.azonosító)
                {
                    command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.verseny + "';";
                    SQLiteDataReader reader = command.ExecuteReader();
                    bool found = false;
                    while (reader.Read())
                    {
                        if (_korosztály.azonosító == reader.GetString(0))
                        {
                            found = true;
                        }
                    }

                    command.Dispose();

                    if (found)
                    {
                        connection.Close();
                        return false;
                    }
                }

                command = connection.CreateCommand();
                command.CommandText = "UPDATE Korosztályok SET KOAZON = '" + _korosztály.azonosító + "', KOMEGN = '" + _korosztály.megnevezés + "', " +
                    "KOEKMI = " + _korosztály.alsó_határ + ", KOEKMA = " + _korosztály.felső_határ + ", KOINSF = " + _korosztály.indulók_férfiak + ", KOINSN = " + _korosztály.indulók_nők + " " +
                    "WHERE KOAZON = '" + _azonosító + "' AND VEAZON = '" + _korosztály.verseny + "';";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }

        public bool KorosztályTörlés(string _verseny, string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Korosztályok WHERE VEAZON = '" + _verseny + "' AND KOAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                return true;
            }
        }

        public struct CountPair
        {
            public int férfiak;
            public int nők;
        };

        public bool KorosztálySzámolás(string _verseny)
        {
            List<Korosztály> korosztályok = Korosztályok(_verseny);

            lock (Program.datalock)
            {
                foreach (Korosztály current in korosztályok)
                {
                    CountPair indulók = KorosztálySzámolás(_verseny, current.alsó_határ, current.felső_határ, true);

                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Korosztályok SET KOINSF = " + indulók.férfiak + ", KOINSN = " + indulók.nők + " WHERE KOAZON = '" + current.azonosító + "' AND VEAZON = '" + current.verseny + "';";
                    command.ExecuteNonQuery();

                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }

        public CountPair KorosztálySzámolás(string _azonosító, int _alsó, int _felső, bool _internal)
        {
            if (!_internal) lock (Program.datalock) { return KorosztálySzámolás_Segéd(_azonosító, _alsó, _felső); }
            else return KorosztálySzámolás_Segéd(_azonosító, _alsó, _felső);
        }

        /// <summary>
        /// Nincs lockolva, csak belső használatra!
        /// </summary>
        private CountPair KorosztálySzámolás_Segéd(string _azonosító, int _alsó, int _felső)
        {
            CountPair count = new CountPair();

            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT INSZUL, INNEME, INMEGJ FROM Indulók, Eredmények_" + _azonosító + " WHERE Eredmények_" + _azonosító + ".INNEVE= Indulók.INNEVE AND Eredmények_" + _azonosító + ".INMEGJ= '" + 1 + "';";

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int year = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(reader.GetString(0)))).Year - 1;
                //MessageBox.Show("also: " + _alsó + " felso: " + _felső + " year: " + year);
                if (_alsó <= year && year <= _felső)
                {
                    if (reader.GetString(1) == "N") count.nők++;
                    else count.férfiak++;
                }
                //MessageBox.Show(reader.GetBoolean(2).ToString());
            }
            // MessageBox.Show(count.nők.ToString() + " " + count.férfiak.ToString());
            command.Dispose();
            connection.Close();

            return count;
        }
        #endregion

        #region Íjtípusok
        public List<Íjtípus> Íjtípusok()
        {
            lock (Program.datalock)
            {
                List<Íjtípus> data = new List<Íjtípus>();
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT ITAZON, ITMEGN, ITLISO, ITERSZ FROM Íjtípusok;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Íjtípus(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
                }

                command.Dispose();
                connection.Close();

                return data;
            }
        }

        public bool ÚjÍjtípus(Íjtípus _íjtípus)
        {
            lock (Program.datalock)
            {
                if (Íjtípus_SorszámLétezik(_íjtípus.sorszám, _íjtípus.azonosító)) return false;

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Íjtípusok (ITAZON, ITMEGN, ITLISO, ITERSZ) VALUES('" + _íjtípus.azonosító + "', '" + _íjtípus.megnevezés + "', " + _íjtípus.sorszám + ", 0);";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }

        public bool ÍjtípusMódosítás(string _azonosító, Íjtípus _íjtípus)
        {
            lock (Program.datalock)
            {
                if (Íjtípus_SorszámLétezik(_íjtípus.sorszám, _íjtípus.azonosító)) return false;

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Íjtípusok SET ITAZON = '" + _íjtípus.azonosító + "', ITMEGN = '" + _íjtípus.megnevezés + "', ITLISO = " + _íjtípus.sorszám + " WHERE ITAZON = '" + _azonosító + "';";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }

        public bool ÍjtípusTörlés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Íjtípusok WHERE ITAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                return true;
            }
        }

        ///

        public bool Íjtípus_EredményekNövelés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Íjtípusok SET ITERSZ = ITERSZ + 1 WHERE ITAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        public bool Íjtípus_EredményekCsökkentés(string _azonosító)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Íjtípusok SET ITERSZ = ITERSZ - 1 WHERE ITAZON = '" + _azonosító + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return true;
            }
        }

        /// <summary>
        /// Nincs lockolva, csak belső használatra!
        /// </summary>
        private bool Íjtípus_SorszámLétezik(int _sorszám, string _itazon)
        {
            bool exists = false;
            connection.Open();

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT ITAZON FROM Íjtípusok WHERE ITLISO = " + _sorszám + ";";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) != _itazon)
                {
                    exists = true;
                }
            }

            command.Dispose();
            connection.Close();

            return exists;
        }
        #endregion

        #region Induló
        public List<Induló> Indulók()
        {
            lock (Program.datalock)
            {
                List<Induló> data = new List<Induló>();
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, INEGYE, INERSZ FROM Indulók;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Induló(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5)));
                }

                command.Dispose();
                connection.Close();

                return data;
            }
        }

        public Nullable<Induló> Induló(string _név)
        {
            lock (Program.datalock)
            {
                Nullable<Induló> induló = null;
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, INEGYE, INERSZ FROM Indulók WHERE INNEVE = '" + _név + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    induló = new Induló(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5));
                }

                command.Dispose();
                connection.Close();

                return induló;
            }
        }

        public bool ÚjInduló(Induló _induló)
        {
            lock (Program.datalock)
            {
                if (Induló_EngedélyLétezik("", _induló.engedély)) { return false; }

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Indulók (INNEVE, INNEME, INSZUL, INVEEN, INEGYE, INERSZ) VALUES('" + _induló.név + "', '" + _induló.nem + "', '" + _induló.születés + "', " +
                    "'" + _induló.engedély + "', '" + _induló.egyesület + "', 0);";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return false;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return true;
            }
        }

        public bool IndulóMódosítás(string _név, Induló _induló)
        {
            lock (Program.datalock)
            {
                if (Induló_EngedélyLétezik(_név, _induló.engedély)) { return false; }

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Indulók SET INNEVE = '" + _induló.név + "', INNEME = '" + _induló.nem + "', INSZUL = '" + _induló.születés + "', " +
                    "INVEEN = '" + _induló.engedély + "', INEGYE = '" + _induló.egyesület + "' WHERE INNEVE = '" + _név + "';";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

                return true;
            }
        }

        public bool Induló_EredményekÁtnevezése(string _eredeti_név, string _új_név)
        {
            lock (Program.datalock)
            {
                List<Verseny> versenyek = Versenyek();

                connection.Open();

                foreach (Verseny _verseny in versenyek)
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Eredmények_" + _verseny.azonosító + " SET INNEVE = '" + _új_név + "' WHERE INNEVE = '" + _eredeti_név + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                connection.Close();

                return true;
            }
        }

        public bool IndulóTörlés(string _név)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Indulók WHERE INNEVE= '" + _név + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                return true;
            }
        }


        /// <summary>
        /// Nincs lockolva, csak belső használatra!
        /// </summary>
        public bool Induló_EngedélyLétezik(string _név, string _engedély)
        {
            if (_engedély.Length == 0) { return false; }
            bool exists = false;
            connection.Open();

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT INNEVE FROM Indulók WHERE INVEEN = '" + _engedély + "' AND INNEVE <> '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                exists = true;
            }

            command.Dispose();
            connection.Close();

            if (exists == true)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Eredmények
        public List<Eredmény> Eredmények(string _azonosító)
        {
            lock (Program.datalock)
            {
                List<Eredmény> data = new List<Eredmény>();

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _azonosító + ";";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10)));
                }


                command.Dispose();
                connection.Close();
                return data;
            }
        }

        public Nullable<Eredmény> Eredmény(string _verseny, string _név)
        {
            lock (Program.datalock)
            {
                Nullable<Eredmény> eredmény = null;

                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eredmény = new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10));
                }

                command.Dispose();
                connection.Close();

                return eredmény;
            }
        }

        public class BeírásEredmény
        {
            public enum Flag
            {
                NINCS,

                HOZZÁADOTT,
                MÓDOSÍTOTT
            }

            public Flag flag;
            public Eredmény? eredmény;
            public Eredmény? eredeti;

            public BeírásEredmény(Eredmény? _eredmény, Eredmény? _eredeti, Flag _flag)
            {
                flag = _flag;
                eredeti = _eredeti;
                eredmény = _eredmény;
            }

            public BeírásEredmény()
            {
                flag = Flag.NINCS;
                eredeti = null;
                eredmény = null;
            }
        }
        public BeírásEredmény EredményBeírás(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent)
        {
            lock (Program.datalock)
            {
                connection.Open();
                SQLiteCommand command;

                // Név meglétének ellenőrzése a versenyen
                Eredmény? eredeti = null;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eredeti = new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10));
                }
                command.Dispose();

                if (eredeti != null)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + (_megjelent ? "1" : "0") + " WHERE INNEVE = '" + _név + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ)" +
                       "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + (_megjelent ? "1" : "0") + ");";
                    command.ExecuteNonQuery();
                    command.Dispose();

                    // Induló eredmények számának növelése
                    command = connection.CreateCommand();
                    command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                Eredmény? eredmény = null;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eredmény = new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10));
                }
                command.Dispose();

                connection.Close();
                return new BeírásEredmény(eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT);
            }
        }

        public BeírásEredmény EredményBeírás_Ellenőrzött(string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent)
        {
            lock (Program.datalock)
            {
                bool found;

                connection.Open();
                SQLiteCommand command;

                // Verseny meglétének és lezárásának ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { if (_verseny == reader.GetString(0) && reader.GetBoolean(1) == false) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return new BeírásEredmény(); };

                // Induló meglétének ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE FROM Indulók;";
                reader = command.ExecuteReader();
                while (reader.Read()) { if (_név == reader.GetString(0)) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return new BeírásEredmény(); ; };

                // Íjtípus meglétének ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
                reader = command.ExecuteReader();
                while (reader.Read()) { if (_íjtípus == reader.GetString(0)) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return new BeírásEredmény(); };

                // Név meglétének ellenőrzése a versenyen
                Eredmény? eredeti = null;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eredeti = new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10));
                }
                command.Dispose();

                if (eredeti != null)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + (_megjelent ? "1" : "0") + " WHERE INNEVE = '" + _név + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ)" +
                       "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + (_megjelent ? "1" : "0") + ");";
                    command.ExecuteNonQuery();
                    command.Dispose();

                    // Induló eredmények számának növelése
                    command = connection.CreateCommand();
                    command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                Eredmény? eredmény = null;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eredmény = new Eredmény(reader.GetString(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                        reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetBoolean(10));
                }
                command.Dispose();

                connection.Close();
                return new BeírásEredmény(eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT);
            }
        }

        public Int64 EredményMódosítás(string _azonosító, Eredmény _eredeti, Eredmény _eredmény)
        {
            lock (Program.datalock)
            {
                connection.Open();
                SQLiteCommand command;
                if (_eredeti.név != _eredmény.név)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT INNEVE FROM Eredmények_" + _azonosító + ";";

                    bool found = false;
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (_eredmény.név == reader.GetString(0)) found = true;
                    }

                    if (found)
                    {
                        command.Dispose();
                        connection.Close();
                        return -666;
                    }
                }

                command = connection.CreateCommand();
                command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET INNEVE = '" + _eredmény.név + "', ITAZON = '" + _eredmény.íjtípus + "', INCSSZ = '" + _eredmény.csapat + "', " +
                    "IN10TA = " + _eredmény.találat_10 + ", IN08TA = " + _eredmény.találat_08 + ", IN05TA = " + _eredmény.találat_05 + ", INMETA = " + _eredmény.mellé + ", " +
                    "INOSZP = " + _eredmény.összpont + ", INERSZ = " + _eredmény.százalék + ", INMEGJ = " + (_eredmény.megjelent ? "1" : "0") + " WHERE INNEVE = '" + _eredeti.név + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                return 1;
            }
        }

        public Nullable<Eredmény> EredményMódosítás_Ellenőrzött(string _azonosító, Eredmény _eredeti, Eredmény _eredmény)
        {
            lock (Program.datalock)
            {
                bool found;

                connection.Open();
                SQLiteCommand command;

                // Verseny meglétének és lezárásának ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) { if (_azonosító == reader.GetString(0) && reader.GetBoolean(1) == false) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return null; };

                // Induló meglétének ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INNEVE FROM Indulók;";
                reader = command.ExecuteReader();
                while (reader.Read()) { if (_eredmény.név == reader.GetString(0)) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return null; };

                // Íjtípus meglétének ellenőrzése
                found = false;
                command = connection.CreateCommand();
                command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
                reader = command.ExecuteReader();
                while (reader.Read()) { if (_eredmény.íjtípus == reader.GetString(0)) found = true; }
                command.Dispose();

                if (!found) { connection.Close(); return null; };


                // Százalék számítás összespont lekérdezése után
                int összespont = -666;
                command = connection.CreateCommand();
                command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    összespont = reader.GetInt32(0);
                }
                command.Dispose();

                if (!((_eredmény.találat_10 == 0 && _eredmény.találat_08 == 0 && _eredmény.találat_05 == 0 && _eredmény.mellé == 0)
                    || (összespont == _eredmény.találat_10 + _eredmény.találat_08 + _eredmény.találat_05 + _eredmény.mellé)))
                {
                    connection.Close(); return null;
                }

                int összes = _eredmény.találat_10 * 10 + _eredmény.találat_08 * 8 + _eredmény.találat_05 * 5;
                int százalék = (int)(((double)összes / (összespont * 10)) * 100);

                // Adatbázis módosítás
                command = connection.CreateCommand();
                command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET ITAZON = '" + _eredmény.íjtípus + "', INCSSZ = '" + _eredmény.csapat + "', " +
                    "IN10TA = " + _eredmény.találat_10 + ", IN08TA = " + _eredmény.találat_08 + ", IN05TA = " + _eredmény.találat_05 + ", INMETA = " + _eredmény.mellé + ", " +
                    "INOSZP = " + összes + ", INERSZ = " + százalék + ", INMEGJ = " + (_eredmény.megjelent ? "1" : "0") + " WHERE INNEVE = '" + _eredeti.név + "';";
                command.ExecuteNonQuery();

                // Sorszám, megjelent lekérdezése
                Int64 sorszám = -666;
                command = connection.CreateCommand();
                command.CommandText = "SELECT INSOSZ FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.név + "';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sorszám = reader.GetInt64(0);
                }
                command.Dispose();

                command.Dispose();
                connection.Close();
                return new Eredmény(_eredmény.név, sorszám, _eredmény.íjtípus, _eredmény.csapat, _eredmény.találat_10, _eredmény.találat_08, _eredmény.találat_05, _eredmény.mellé, összes, százalék, _eredmény.megjelent);
            }
        }

        public bool EredményTörlés(string _azonosító, Eredmény _eredmény)
        {
            lock (Program.datalock)
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.név + "';";
                command.ExecuteNonQuery();

                // Induló eredmények számának csökkentése
                command = connection.CreateCommand();
                command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ - 1 WHERE INNEVE = '" + _eredmény.név + "';";
                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                return true;
            }
        }
        #endregion

        public void Dispose()
        {
            connection.Close();
        }
    }
}
