using System;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Íjász
{
   public sealed class Database : IDisposable
   {
      public static int Verzió = 3;
      private SQLiteConnection connection;

      public Database( )
      {
         connection = new SQLiteConnection( "Data Source=adat.db; Version=3; New=False; Compress=True;" );

         //Check if database not exists
         if ( !File.Exists( "adat.db" ) )
         {
            //Create database
            SQLiteConnection.CreateFile( "adat.db" );
            connection.Open( );

            //Create tables
            SQLiteCommand command = connection.CreateCommand();

            command.CommandText =
                "CREATE TABLE Verzió (PRVERZ int);" +
                "CREATE TABLE Versenysorozat (VSAZON char(10) PRIMARY KEY, VSMEGN char(30), VSVESZ int);" +
                "CREATE TABLE Verseny (VEAZON char(10) PRIMARY KEY, VEMEGN char(30), VEDATU char(20), VSAZON char(10), VEOSPO int NOT NULL, VEALSZ int, VEINSZ int, VELEZAR boolean, VEDUBE boolean);" +
                "CREATE TABLE Korosztályok (VEAZON char(10) NOT NULL, KOAZON char(10) NOT NULL, KOMEGN char(30), KOEKMI int NOT NULL, KOEKMA int NOT NULL, KONOK boolean, KOFERF boolean, KOINSN int, KOINSF int, KOEGYB boolean);" +
                "CREATE TABLE Íjtípusok (ITAZON char(10) PRIMARY KEY, ITMEGN char(30), ITLISO int, ITERSZ int);" +
                "CREATE TABLE Egyesuletek (EGAZON char(30) PRIMARY KEY,EGCIME char(30),EGVENE char(30),EGVET1 char(30),EGVET2 char(30),EGVEM1 char(30),EGVEM2 char(30),EGLIST boolean,EGTASZ int);" +
                "CREATE TABLE Indulók (INNEVE char(30) PRIMARY KEY, INNEME char(1) NOT NULL, INSZUL char(20) NOT NULL, INVEEN char(30),INERSZ int, EGAZON char(10));" +
                "INSERT INTO Verzió (PRVERZ) VALUES (" + Verzió + ");";


            if ( command.ExecuteNonQuery( ) != 0 ) { }// MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else MessageBox.Show( "Adatbázis létrehozva!", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information );
            command.Dispose( );
            connection.Close( );

            // Biztonsági mentések mappája

            if ( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );
         }
         else
         {
            CreateBackup( "adat_indítás_" + DateTime.Now.ToString( ).Trim( new Char[ ] { '-' } ).Replace( ' ', '_' ).Replace( '.', '-' ).Replace( ':', '-' ) );
         }

      }

      public void CreateBackup( string _name )
      {
         if ( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );

         lock ( Program.datalock )
         {
            SQLiteConnection cnnOut = new SQLiteConnection("Data Source=backup\\" + _name + ".db;foreign keys=True");
            connection.Open( );
            cnnOut.Open( );
            connection.BackupDatabase( cnnOut, "main", "main", -1, null, -1 );
            connection.Close( );
            cnnOut.Close( );
         }
      }

      public static bool IsCorrectSQLText( string _text )
      {
         if ( _text.Contains( "'" ) || _text.Contains( "\"" ) || _text.Contains( "(" ) || _text.Contains( ")" ) || _text.Contains( ";" ) ) return false;
         return true;
      }

      public bool IsCorrectVersion( )
      {
         int version = 0;
         SQLiteCommand command;

         connection.Open( );

         command = connection.CreateCommand( );
         command.CommandText = "SELECT PRVERZ FROM Verzió;";
         try
         {
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               version = reader.GetInt32( 0 );
            }
         }
         catch ( SQLiteException ) { version = 1; }
         catch ( Exception ) { return false; }
         finally
         {
            command.Dispose( );
            connection.Close( );
         }

         if ( Verzió == version ) return true;
         return false;
      }

      public void torol( )
      {
         SQLiteCommand command;
         connection = new SQLiteConnection( "Data Source=adat.db; Version=3; New=False; Compress=True;" );
         connection.Open( );

         command = connection.CreateCommand( );
         command.CommandText = "UPDATE Eredmények_2014KVS2 SET KOAzON = 'FAIL';";
         command.ExecuteNonQuery( );
         command.Dispose( );
         connection.Close( );
      }

      #region Versenysorozat

      // TODO EXTRA, CSAK SPAMERNEK KELL!!
      public Nullable<Versenysorozat> Versenysorozat( string _azonosító )
      {
         lock ( Program.datalock )
         {

            Nullable<Versenysorozat> data = null;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data = new Versenysorozat( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public List<Versenysorozat> Versenysorozatok( )
      {
         lock ( Program.datalock )
         {

            List<Versenysorozat> data = new List<Versenysorozat>();

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Versenysorozat( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ) ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public bool ÚjVersenysorozat( Versenysorozat _versenysorozat )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Versenysorozat (VSAZON, VSMEGN, VSVESZ) VALUES('" + _versenysorozat.azonosító + "', '" + _versenysorozat.megnevezés + "', 0);";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            Directory.CreateDirectory( _versenysorozat.azonosító );

            return true;
         }
      }

      public bool VersenysorozatMódosítás( string _azonosító, Versenysorozat _versenysorozat )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Versenysorozat SET VSAZON = '" + _versenysorozat.azonosító + "', VSMEGN = '" + _versenysorozat.megnevezés + "' WHERE VSAZON = '" + _azonosító + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
               if ( _azonosító != _versenysorozat.azonosító && Directory.GetFiles( _azonosító ).Length == 0 )
               {
                  Directory.Delete( _azonosító );
                  Directory.CreateDirectory( _versenysorozat.azonosító );
               }
            }
            return true;
         }
      }

      public bool VersenysorozatTörlés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Versenysorozat WHERE VSAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            if ( Directory.GetFiles( _azonosító ).Length == 0 ) { Directory.Delete( _azonosító ); }

            return true;
         }
      }

      ///

      public bool Versenysorozat_VersenyekNövel( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ + 1 WHERE VSAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Versenysorozat_VersenyekCsökkent( string _azonosító )
      {
         lock ( Program.datalock )
         {

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ - 1 WHERE VSAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }
      #endregion

      #region Verseny
      // TODO CSAK A SPAMMERNEK KELL!!
      public Nullable<Verseny> Verseny( string _azonosító )
      {
         lock ( Program.datalock )
         {
            Nullable<Verseny> data = null;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data = new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public List<Verseny> Versenyek( )
      {
         lock ( Program.datalock )
         {
            List<Verseny> data = new List<Verseny>();

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public List<Verseny> Versenyek_Aktív( )
      {
         lock ( Program.datalock )
         {
            List<Verseny> data = new List<Verseny>();

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny WHERE VELEZAR = 0;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public bool ÚjVerseny( Verseny _verseny )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Verseny (VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE) VALUES('" + _verseny.Azonosito + "', '" + _verseny.Megnevezes + "', " +
                "'" + _verseny.Datum + "', '" + _verseny.VersenySorozat + "', " + _verseny.Osszes + ", " + _verseny.Allomasok + ", 0, 0, " + Convert.ToInt32( _verseny.DublaBeirlap ) + ");";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               command.Dispose( );
               connection.Close( );
               return false;
            }


            command = connection.CreateCommand( );
            command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSF, KOINSN) VALUES" +
                "('" + _verseny.Azonosito + "', 'K10', '0-10', 1, 9, 1, 1, 0, 0)," +
                "('" + _verseny.Azonosito + "', 'K14', '10-14', 10, 13, 1, 1, 0, 0)," +
                "('" + _verseny.Azonosito + "', 'K18', '14-18', 14, 17, 1, 1, 0, 0)," +
                "('" + _verseny.Azonosito + "', 'K50', '18-50', 18, 49, 1, 1, 0, 0)," +
                "('" + _verseny.Azonosito + "', 'K100', '50-100', 50, 99, 1, 1, 0, 0);";
            command.ExecuteNonQuery( );

            command = connection.CreateCommand( );
            command.CommandText = "CREATE TABLE Eredmények_" + _verseny.Azonosito + " (INNEVE  char(30) NOT NULL, INSOSZ INTEGER PRIMARY KEY AUTOINCREMENT, ITAZON char(10), INCSSZ int, IN10TA int," +
                "IN08TA int, IN05TA int, INMETA int, INOSZP int, INERSZ int, INMEGJ boolean, INKOMO boolean, KOAZON char(10));";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            if ( _verseny.VersenySorozat != "" ) { Directory.CreateDirectory( _verseny.VersenySorozat + "\\" + _verseny.Azonosito ); }
            else { Directory.CreateDirectory( _verseny.Azonosito ); }

            return true;
         }
      }

      public bool VersenyMódosítás( string _azonosító, Verseny _verseny )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Verseny SET VEAZON = '" + _verseny.Azonosito + "', VEMEGN = '" + _verseny.Megnevezes + "', VEDATU = '" + _verseny.Datum + "' ," +
                "VSAZON = '" + _verseny.VersenySorozat + "', VEOSPO = " + _verseny.Osszes + ", VEALSZ = " + _verseny.Allomasok + ", VEDUBE = " + Convert.ToInt32( _verseny.DublaBeirlap ) + " WHERE VEAZON = '" + _azonosító + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               command.Dispose( );
               connection.Close( );
               return false;
            }

            if ( _azonosító != _verseny.Azonosito )
            {
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Korosztályok SET VEAZON = '" + _verseny.Azonosito + "' WHERE VEAZON = '" + _azonosító + "';";
               command.ExecuteNonQuery( );

               command = connection.CreateCommand( );
               command.CommandText = "ALTER TABLE Eredmények_" + _azonosító + " RENAME TO Eredmények_" + _verseny.Azonosito + ";";
               command.ExecuteNonQuery( );
            }

            command.Dispose( );
            connection.Close( );
            if ( _azonosító != _verseny.Azonosito )
            {
               try
               {
                  Directory.Delete( _azonosító );
                  Directory.CreateDirectory( _verseny.Azonosito );
               }
               catch ( Exception )
               {
                  return true;
               }
            }
            return true;
         }
      }

      public bool VersenyTörlés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command = connection.CreateCommand( );
            command.CommandText = "DELETE FROM Korosztályok WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command = connection.CreateCommand( );
            command.CommandText = "DROP TABLE Eredmények_" + _azonosító + ";";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            try
            {
               Directory.Delete( _azonosító );
            }
            catch ( Exception )
            {
               return true;
            }

            return true;
         }
      }

      ///

      public bool Verseny_IndulókNövelés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ + 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Verseny_IndulókCsökkentés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ - 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public int? Verseny_Állomások( string _azonosító )
      {
         lock ( Program.datalock )
         {
            int? value = null;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VEALSZ FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               value = reader.GetInt32( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return value;
         }
      }

      public int Verseny_Összespont( string _azonosító )
      {
         lock ( Program.datalock )
         {
            int összpont = -666;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               összpont = reader.GetInt32( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return összpont;
         }
      }

      public bool Verseny_Lezárva( string _azonosító )
      {
         lock ( Program.datalock )
         {
            bool lezárva = false;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VELEZAR FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               lezárva = reader.GetBoolean( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return lezárva;
         }
      }

      public bool Verseny_Lezárás( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Verseny SET VELEZAR = 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Verseny_Megnyitás( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Verseny SET VELEZAR = 0 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      #endregion

      #region Korosztályok
      public List<Korosztály> Korosztályok( string _verseny )
      {
         lock ( Program.datalock )
         {
            List<Korosztály> data = new List<Korosztály>();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSN, KOINSF, KOEGYB FROM Korosztályok WHERE VEAZON = '" + _verseny + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Korosztály( reader.GetString( 0 ),
                   reader.GetString( 1 ),
                   reader.GetString( 2 ),
                   reader.GetInt32( 3 ),
                   reader.GetInt32( 4 ),
                   reader.GetBoolean( 5 ),
                   reader.GetBoolean( 6 ),
                   reader.GetInt32( 7 ),
                   reader.GetInt32( 8 ),
                   reader.GetBoolean( 9 ) ) );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public bool ÚjKorosztály( Korosztály _korosztály )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.Verseny + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            bool found = false;
            while ( reader.Read( ) )
            {
               if ( _korosztály.Azonosito == reader.GetString( 0 ) )
               {
                  found = true;
               }
            }
            if ( found )
            {
               command.Dispose( );
               connection.Close( );
               return false;
            }

            command = connection.CreateCommand( );
            command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSF, KOINSN, KOEGYB) VALUES('" + _korosztály.Verseny + "', '" + _korosztály.Azonosito + "', '" + _korosztály.Megnevezes + "', " +
                +_korosztály.AlsoHatar + ", " + _korosztály.FelsoHatar + ", " + ( _korosztály.Nokre ? "1" : "0" ) + ", " + ( _korosztály.Ferfiakra ? "1" : "0" ) + ", " + _korosztály.InduloFerfiak + ", " + _korosztály.InduloFerfiak + ( _korosztály.Egyben ? " ,1" : " ,0" ) + ");";

            command.ExecuteNonQuery( );
            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public bool KorosztályMódosítás( string _azonosító, Korosztály _korosztály )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            if ( _azonosító != _korosztály.Azonosito )
            {
               command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.Verseny + "';";
               SQLiteDataReader reader = command.ExecuteReader();
               bool found = false;
               while ( reader.Read( ) )
               {
                  if ( _korosztály.Azonosito == reader.GetString( 0 ) )
                  {
                     found = true;
                  }
               }

               command.Dispose( );

               if ( found )
               {
                  connection.Close( );
                  return false;
               }
            }

            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Korosztályok SET KOAZON = '" + _korosztály.Azonosito + "', KOMEGN = '" + _korosztály.Megnevezes + "', " +
                "KOEKMI = " + _korosztály.AlsoHatar + ", KOEKMA = " + _korosztály.FelsoHatar + ", KONOK = " + ( _korosztály.Nokre ? "1" : "0" ) + ", KOFERF = " + ( _korosztály.Ferfiakra ? "1" : "0" ) +
                ", KOINSF = " + _korosztály.InduloFerfiak + ", KOINSN = " + _korosztály.InduloNok + ", KOEGYB = " + ( _korosztály.Egyben ? "1" : "0" ) + " WHERE KOAZON = '" + _azonosító + "' AND VEAZON = '" + _korosztály.Verseny + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public bool KorosztályTörlés( string _verseny, string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Korosztályok WHERE VEAZON = '" + _verseny + "' AND KOAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public struct CountPair
      {
         public int férfiak;
         public int nők;
      };

      public bool
      KorosztálySzámolás( string _verseny )
      {
         List<Korosztály> korosztályok = Korosztályok(_verseny);

         lock ( Program.datalock )
         {
            foreach ( Korosztály current in korosztályok )
            {
               CountPair indulók = KorosztálySzámolás(_verseny,
                                                            current.AlsoHatar,
                                                            current.FelsoHatar,
                                                            current.Nokre,
                                                            current.Ferfiakra,
                                                            true);

               connection.Open( );
               SQLiteCommand command = connection.CreateCommand();
               command.CommandText = "UPDATE Korosztályok SET KOINSF = " + indulók.férfiak + ", KOINSN = " + indulók.nők + " WHERE KOAZON = '" + current.Azonosito + "' AND VEAZON = '" + current.Verseny + "';";
               command.ExecuteNonQuery( );

               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public CountPair
      KorosztálySzámolás( string _azonosító, int _alsó, int _felső, bool _nők, bool _férfiak, bool _internal )
      {
         if ( !_internal )
            lock ( Program.datalock )
            {
               return KorosztálySzámolás_Segéd( _azonosító, _alsó, _felső, _nők, _férfiak );
            }
         else return KorosztálySzámolás_Segéd( _azonosító, _alsó, _felső, _nők, _férfiak );
      }

      /// <summary>
      /// Nincs lockolva, csak belső használatra!
      /// </summary>
      private CountPair
      KorosztálySzámolás_Segéd( string _azonosító, int _alsó, int _felső, bool _nők, bool _férfiak )
      {
         CountPair count = new CountPair();

         connection.Open( );
         SQLiteCommand command = connection.CreateCommand();
         command.CommandText = "SELECT INSZUL, INNEME, INMEGJ FROM Indulók, Eredmények_" + _azonosító + " WHERE Eredmények_" + _azonosító + ".INNEVE= Indulók.INNEVE AND Eredmények_" + _azonosító + ".INMEGJ= '" + 1 + "';";

         SQLiteDataReader reader = command.ExecuteReader();
         while ( reader.Read( ) )
         {
            int year = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(reader.GetString(0)))).Year - 1;
            //MessageBox.Show("also: " + _alsó + " felso: " + _felső + " year: " + year);
            if ( _alsó <= year && year <= _felső )
            {
               if ( reader.GetString( 1 ) == "N" )
               {
                  if ( _nők ) count.nők++;
               }
               else
               {
                  if ( _férfiak ) count.férfiak++;
               }
            }
            //MessageBox.Show(reader.GetBoolean(2).ToString());
         }
         // MessageBox.Show(count.nők.ToString() + " " + count.férfiak.ToString());
         command.Dispose( );
         connection.Close( );

         return count;
      }

      public int InduloKora( string _VEAZON, string _INNEVE )
      {
         lock ( Program.datalock )
         {
            int Value = 0;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "select min(verseny.VEDATU), Indulók.INSZUL from verseny, indulók " +
                                  "where verseny.VSAZON = (select verseny.vsazon from verseny " +
                                  "where verseny.veazon = '" + _VEAZON + "') and indulók.inneve = '" + _INNEVE + "';";

            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               string q = reader.GetString( 0 );
               string qq = reader.GetString( 1 );
               Value = ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( q ) - DateTime.Parse( qq ) ) ).Year - 1;
            }

            command.Dispose( );
            connection.Close( );
            return Value;
         }
      }

      //teszt
      //működik de lassú ,a végén a módosítást egyszerre
      /// <summary>
      /// egy eredmenylap osszes indulojanak koazon-jat módosítja + visszaadja a korosztaly indulok szamat
      /// régi korosztalyt törölni???!!!!4négy
      /// </summary>
      public bool
      KorosztalyModositas( string _VEAZON )
      {
         /*  tudom az új korosztályt
          *  tudom a versenyazonosító
          *  végig kell menni az eredménylapon , updatelni akit lehet
          *  ha kész ki kell számolni a korosztály indulókat
          * 
          */

         try
         {
            List<Korosztály> list_korosztalyok = Program.database.Korosztályok( _VEAZON );
            List<Eredmény> list_eredmenyek = Program.database.Eredmények( _VEAZON );
            List<Induló> list_indulok = Program.database.Indulók( );

            // a korosztályokat tuti módosítom -> array könnyebb
            //kinullázom
            Korosztály[] korosztalyok = list_korosztalyok.ToArray( );
            for ( int i = 0 ; i < korosztalyok.Length ; i++ )
            {
               korosztalyok[ i ].InduloNok = 0;
               korosztalyok[ i ].InduloFerfiak = 0;
            }

            //eredmenyeket tuti modosítóm -> array könnyebb
            Eredmény[] eredmenyek = list_eredmenyek.ToArray( );

            //kell a verseny az időpont miatt
            Verseny? verseny = Program.database.Verseny( _VEAZON );


            Induló[] indulok = list_indulok.ToArray( );

            int countNemModosithato = 0;
            int countModositott = 0;
            int countMegjelentek = 0;
            foreach ( Eredmény item in list_eredmenyek ) { if ( item.Megjelent == true ) { countMegjelentek++; } }

            for ( int k = 0 ; k < indulok.Length ; k++ )
            {
               //a versenykor ennyi éves volt
               int year = Program.database.InduloKora( verseny.Value.Azonosito, indulok[k].Nev );// ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( verseny.Value.Datum ) - DateTime.Parse( indulok[k].születés ) ) ).Year - 1;
               for ( int j = 0 ; j < eredmenyek.Length ; j++ )
               {
                  if ( indulok[ k ].Nev == eredmenyek[ j ].Nev )
                  {
                     //ha felülírt a korosztálya nem foglalkozok vele
                     if ( eredmenyek[ j ].KorosztalyModositott == false )
                     {
                        for ( int i = 0 ; i < korosztalyok.Length ; i++ )
                        {
                           //férfi, férfi korosztály
                           if ( indulok[ k ].Nem == "F" && korosztalyok[ i ].Ferfiakra == true &&
                               ( year >= korosztalyok[ i ].AlsoHatar && year <= korosztalyok[ i ].FelsoHatar ) )
                           {
                              //csak a megjelentek???????
                              if ( eredmenyek[ j ].Megjelent == true )
                              {
                                 korosztalyok[ i ].InduloFerfiak++;
                              }

                              eredmenyek[ j ].KorosztalyAzonosito = korosztalyok[ i ].Azonosito;
                              countModositott++;
                           }
                           //no, no korosztaly
                           else if ( indulok[ k ].Nem == "N" && korosztalyok[ i ].Ferfiakra == true &&
                           ( year >= korosztalyok[ i ].AlsoHatar && year <= korosztalyok[ i ].FelsoHatar ) )
                           {
                              if ( eredmenyek[ j ].Megjelent == true )
                              {
                                 korosztalyok[ i ].InduloNok++;
                              }

                              eredmenyek[ j ].KorosztalyAzonosito = korosztalyok[ i ].Azonosito;
                              countModositott++;
                           }
                        }
                     }
                     else
                     {
                        for ( int q = 0 ; q < korosztalyok.Length ; q++ )
                        {
                           if ( korosztalyok[ q ].Azonosito == eredmenyek[ j ].KorosztalyAzonosito && eredmenyek[ j ].Megjelent == true )
                           {
                              if ( indulok[ k ].Nem == "F" ) { korosztalyok[ q ].InduloFerfiak++; }
                              else if ( indulok[ k ].Nem == "N" ) { korosztalyok[ q ].InduloNok++; }
                           }
                        }
                        countNemModosithato++;
                     }
                  }
               }
            }
            if ( countModositott + countNemModosithato != verseny.Value.Indulok )
            {
               MessageBox.Show( "A korosztályok nem megfelelőek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
               return false;
            }
            //ha minden ok, akkor az új eredményeket, és korosztályokat el kell menteni
            //átírni a database fgv-eket. hogy egyben 
            for ( int i = 0 ; i < korosztalyok.Length ; i++ )
            {
               Program.database.KorosztályMódosítás( korosztalyok[ i ].Azonosito, korosztalyok[ i ] );
            }

            for ( int i = 0 ; i < eredmenyek.Length ; i++ )
            {
               for ( int j = 0 ; j < list_eredmenyek.Count ; j++ )
               {
                  if ( eredmenyek[ i ].Nev == list_eredmenyek[ j ].Nev )
                  {
                     Program.database.EredményMódosítás( verseny.Value.Azonosito, list_eredmenyek[ j ], eredmenyek[ i ] );
                  }
               }
            }
         }
         catch ( Exception e )
         {

            MessageBox.Show( e.Message );
         }



         return true;
      }

      #endregion

      #region Íjtípusok
      public List<Íjtípus> Íjtípusok( )
      {
         lock ( Program.datalock )
         {
            List<Íjtípus> data = new List<Íjtípus>();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT ITAZON, ITMEGN, ITLISO, ITERSZ FROM Íjtípusok;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Íjtípus( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ), reader.GetInt32( 3 ) ) );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public bool ÚjÍjtípus( Íjtípus _íjtípus )
      {
         lock ( Program.datalock )
         {
            if ( Íjtípus_SorszámLétezik( _íjtípus.Sorszam, _íjtípus.Azonosito ) ) return false;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Íjtípusok (ITAZON, ITMEGN, ITLISO, ITERSZ) VALUES('" + _íjtípus.Azonosito + "', '" + _íjtípus.Megnevezes + "', " + _íjtípus.Sorszam + ", 0);";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public bool ÍjtípusMódosítás( string _azonosító, Íjtípus _íjtípus )
      {
         lock ( Program.datalock )
         {
            if ( Íjtípus_SorszámLétezik( _íjtípus.Sorszam, _íjtípus.Azonosito ) ) return false;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Íjtípusok SET ITAZON = '" + _íjtípus.Azonosito + "', ITMEGN = '" + _íjtípus.Megnevezes + "', ITLISO = " + _íjtípus.Sorszam + " WHERE ITAZON = '" + _azonosító + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public bool ÍjtípusTörlés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Íjtípusok WHERE ITAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      ///

      public bool Íjtípus_EredményekNövelés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Íjtípusok SET ITERSZ = ITERSZ + 1 WHERE ITAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Íjtípus_EredményekCsökkentés( string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Íjtípusok SET ITERSZ = ITERSZ - 1 WHERE ITAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      /// <summary>
      /// Nincs lockolva, csak belső használatra!
      /// </summary>
      private bool Íjtípus_SorszámLétezik( int _sorszám, string _itazon )
      {
         bool exists = false;
         connection.Open( );

         SQLiteCommand command = connection.CreateCommand();

         command.CommandText = "SELECT ITAZON FROM Íjtípusok WHERE ITLISO = " + _sorszám + ";";
         SQLiteDataReader reader = command.ExecuteReader();
         while ( reader.Read( ) )
         {
            if ( reader.GetString( 0 ) != _itazon )
            {
               exists = true;
            }
         }

         command.Dispose( );
         connection.Close( );

         return exists;
      }
      #endregion

      #region Egyesuletek

      public static string GetNullableString( SQLiteDataReader _reader, int _column )
      {
         if ( !_reader.IsDBNull( _column ) ) return _reader.GetString( _column );
         return null;
      }
      public static bool GetNullableBool( SQLiteDataReader _reader, int _column )
      {
         if ( !_reader.IsDBNull( _column ) ) return _reader.GetBoolean( _column );
         return false;
      }

      public static int GetNullableInt( SQLiteDataReader _reader, int _column )
      {
         if ( !_reader.IsDBNull( _column ) ) return _reader.GetInt32( _column );
         return 0;
      }

      public Egyesulet
      Egyesulet( string _azonosito )
      {
         lock ( Program.datalock )
         {
            Egyesulet data = new Egyesulet();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT EGAZON,EGCIME,EGVENE,EGVET1,EGVET2,EGVEM1,EGVEM2,EGLIST,EGTASZ FROM Egyesuletek WHERE Egyesuletek.EGAZON='" + _azonosito + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data = new Egyesulet( reader.GetString( 0 ),
                                           GetNullableString( reader, 1 ),
                                           GetNullableString( reader, 2 ),
                                           GetNullableString( reader, 3 ),
                                           GetNullableString( reader, 4 ),
                                           GetNullableString( reader, 5 ),
                                           GetNullableString( reader, 6 ),
                                           GetNullableBool( reader, 7 ),
                                           GetNullableInt( reader, 8 ) );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public List<Egyesulet>
      Egyesuletek( )
      {
         lock ( Program.datalock )
         {
            List<Egyesulet> data = new List<Egyesulet>();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT EGAZON,EGCIME,EGVENE,EGVET1,EGVET2,EGVEM1,EGVEM2,EGLIST,EGTASZ FROM Egyesuletek ;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               Egyesulet temp = new Egyesulet( reader.GetString(0),
                                                GetNullableString(reader, 1),
                                                GetNullableString(reader,2),
                                                GetNullableString(reader,3),
                                                GetNullableString(reader,4),
                                                GetNullableString(reader,5),
                                                GetNullableString(reader,6),
                                                GetNullableBool(reader, 7),
                                                GetNullableInt(reader, 8));
               data.Add( temp );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public bool
      UjEgyesulet( Egyesulet _egyesulet )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT EGAZON FROM Egyesuletek WHERE EGAZON = '" + _egyesulet.Azonosito + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            bool found = false;
            while ( reader.Read( ) )
            {
               if ( _egyesulet.Azonosito == reader.GetString( 0 ) )
               {
                  found = true;
               }
            }
            if ( found )
            {
               command.Dispose( );
               connection.Close( );
               return false;
            }

            command = connection.CreateCommand( );
            command.CommandText = "INSERT INTO Egyesuletek (EGAZON,EGCIME,EGVENE,EGVET1,EGVET2,EGVEM1,EGVEM2,EGLIST,EGTASZ)" +
                                  " VALUES('" + _egyesulet.Azonosito + "', '" +
                                   _egyesulet.Cim + "', '" +
                                   _egyesulet.Vezeto + "', '" +
                                   _egyesulet.Telefon1 + "', '" +
                                   _egyesulet.Telefon2 + "', '" +
                                   _egyesulet.Email1 + "', '" +
                                   _egyesulet.Email2 + "', '" +
                                   Convert.ToInt32( _egyesulet.Listazando ) + "'," +
                                   _egyesulet.TagokSzama + ");";

            command.ExecuteNonQuery( );
            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public bool
      EgyesuletModositas( Egyesulet _Regi, Egyesulet _Uj )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Egyesuletek SET EGAZON= '" + _Uj.Azonosito +
                                                        "', EGCIME = '" + _Uj.Cim +
                                                        "', EGVENE = '" + _Uj.Vezeto +
                                                        "', EGVET1 = '" + _Uj.Telefon1 +
                                                        "', EGVET2 = '" + _Uj.Telefon2 +
                                                        "', EGVEM1 = '" + _Uj.Email1 +
                                                        "', EGVEM2 = '" + _Uj.Email2 +
                                                        "', EGLIST = " + ( _Uj.Listazando ? "1" : "0" ) +
                                                        ", EGTASZ = " + _Uj.TagokSzama +
                                                        " WHERE EGAZON = '" + _Regi.Azonosito + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               command.Dispose( );
               connection.Close( );
               return false;
            }
            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool
      EgyesuletTorles( string _Azonosito )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Egyesuletek WHERE EGAZON = '" + _Azonosito + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public int
      EgyesuletTagokSzama( string _azonosito )
      {
         lock ( Program.datalock )
         {
            int value = 0;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT COUNT(EGAZON) FROM Indulók WHERE EGAZON= '" + _azonosito + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               value++;
            }

            command.Dispose( );
            connection.Close( );

            return value;
         }
      }

      public bool
      EgyesuletTagokNoveles( string _azonosito )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Egyesuletek SET EGTASZ = EGTASZ + 1 WHERE EGAZON = '" + _azonosito + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool
      EgyesuletTagokCsokkentes( string _azonosito )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Egyesuletek SET EGTASZ = EGTASZ - 1 WHERE EGAZON = '" + _azonosito + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public List<Nyomtat.EGYESULETADAT> EredmenylapVersenyEgyesulet( string _VEAZON )
      {
         lock ( Program.datalock )
         {
            int c=1;
            List<Nyomtat.EGYESULETADAT> egyesuletadatok = new List<Nyomtat.EGYESULETADAT>();
            connection.Open( );
            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT Egyesuletek.EGAZON, Egyesuletek.EGCIME, SUM(Eredmények_" + _VEAZON + ".INOSZP) " +
                                  "FROM Indulók " +
                                  "INNER JOIN Eredmények_" + _VEAZON +
                                  " ON Eredmények_" + _VEAZON + ".INNEVE=Indulók.INNEVE " +
                                  "INNER JOIN Egyesuletek" +
                                  " ON Indulók.EGAZON=Egyesuletek.EGAZON " +
                                  "WHERE Egyesuletek.EGLIST=1 AND Eredmények_" + _VEAZON + ".INMEGJ=1 " +
                                  "GROUP BY Indulók.EGAZON " +
                                  "ORDER BY SUM(Eredmények_" + _VEAZON + ".INOSZP) DESC" + ";";

            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               egyesuletadatok.Add( new Nyomtat.EGYESULETADAT( c,
                                                                                          reader.GetString( 0 ),
                                                                                          reader.GetString( 1 ),
                                                                                          reader.GetInt32( 2 ) ) );
               c++;
            }

            command.Dispose( );
            connection.Close( );
            return egyesuletadatok;
         }
      }

      #endregion

      #region Induló
      public List<Induló> Indulók( )
      {
         lock ( Program.datalock )
         {
            List<Induló> data = new List<Induló>();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ FROM Indulók;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Induló( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetString( 4 ), reader.GetInt32( 5 ) ) );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public Nullable<Induló> Induló( string _név )
      {
         lock ( Program.datalock )
         {
            Nullable<Induló> induló = null;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ FROM Indulók WHERE INNEVE = '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               induló = new Induló( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetString( 4 ), reader.GetInt32( 5 ) );
            }

            command.Dispose( );
            connection.Close( );

            return induló;
         }
      }

      public bool ÚjInduló( Induló _induló )
      {
         lock ( Program.datalock )
         {
            if ( Induló_EngedélyLétezik( "", _induló.Engedely ) ) { return false; }

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Indulók (INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ) VALUES('" + _induló.Nev + "', '" + _induló.Nem + "', '" + _induló.SzuletesiDatum + "', " +
                "'" + _induló.Engedely + "', '" + _induló.Egyesulet + "', 0);";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public bool IndulóMódosítás( string _név, Induló _induló )
      {
         lock ( Program.datalock )
         {
            if ( Induló_EngedélyLétezik( _név, _induló.Engedely ) ) { return false; }

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Indulók SET INNEVE = '" + _induló.Nev + "', INNEME = '" + _induló.Nem + "', INSZUL = '" + _induló.SzuletesiDatum + "', " +
                "INVEEN = '" + _induló.Engedely + "', EGAZON = '" + _induló.Egyesulet + "' WHERE INNEVE = '" + _név + "';";

            command.ExecuteNonQuery( );
            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public bool Induló_EredményekÁtnevezése( string _eredeti_név, string _új_név )
      {
         lock ( Program.datalock )
         {
            List<Verseny> versenyek = Versenyek();

            connection.Open( );

            foreach ( Verseny _verseny in versenyek )
            {
               SQLiteCommand command = connection.CreateCommand();
               command.CommandText = "UPDATE Eredmények_" + _verseny.Azonosito + " SET INNEVE = '" + _új_név + "' WHERE INNEVE = '" + _eredeti_név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }

            connection.Close( );

            return true;
         }
      }

      public bool IndulóTörlés( string _név )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Indulók WHERE INNEVE= '" + _név + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }


      /// <summary>
      /// Nincs lockolva, csak belső használatra!
      /// </summary>
      public bool Induló_EngedélyLétezik( string _név, string _engedély )
      {
         if ( _engedély.Length == 0 ) { return false; }
         bool exists = false;
         connection.Open( );

         SQLiteCommand command = connection.CreateCommand();

         command.CommandText = "SELECT INNEVE FROM Indulók WHERE INVEEN = '" + _engedély + "' AND INNEVE <> '" + _név + "';";
         SQLiteDataReader reader = command.ExecuteReader();
         while ( reader.Read( ) )
         {
            exists = true;
         }

         command.Dispose( );
         connection.Close( );

         if ( exists == true )
         {
            return true;
         }
         return false;
      }
      #endregion

      #region Eredmények
      public List<Eredmény> Eredmények( string _azonosító )
      {
         lock ( Program.datalock )
         {
            List<Eredmény> data = new List<Eredmény>( );

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _azonosító + ";";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               data.Add( new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) ) );
            }
            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public Nullable<Eredmény> Eredmény( string _verseny, string _név )
      {
         lock ( Program.datalock )
         {
            Nullable<Eredmény> Data = null;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               Data = new Eredmény( reader.GetString( 0 ),
                                        reader.GetInt64( 1 ),
                                        reader.GetString( 2 ),
                                        reader.GetInt32( 3 ),
                                        reader.GetInt32( 4 ),
                                        reader.GetInt32( 5 ),
                                        reader.GetInt32( 6 ),
                                        reader.GetInt32( 7 ),
                                        reader.GetInt32( 8 ),
                                        reader.GetInt32( 9 ),
                                        reader.GetBoolean( 10 ),
                                        reader.GetBoolean( 11 ),
                                        reader.GetString( 12 ) );
            }

            command.Dispose( );
            connection.Close( );

            return Data;
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

         public BeírásEredmény( Eredmény? _eredmény, Eredmény? _eredeti, Flag _flag )
         {
            flag = _flag;
            eredeti = _eredeti;
            eredmény = _eredmény;
         }

         public BeírásEredmény( )
         {
            flag = Flag.NINCS;
            eredeti = null;
            eredmény = null;
         }
      }

      //ha a korosztálymódodított = true akkor be kell írni a korosztályt is, különben nem
      //ELLENŐRIZVE JÓL MŰKÖDIK
      public BeírásEredmény EredményBeírás( string _név,
                                          string _verseny,
                                          string _íjtípus,
                                          int _csapat,
                                          bool _megjelent,
                                          bool _korosztalymodositott,
                                          string _korosztalyazonosito )
      {
         lock ( Program.datalock )
         {
            connection.Open( );
            SQLiteCommand command;

            // Név meglétének ellenőrzése a versenyen
            Eredmény? eredeti = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               eredeti = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            //eredmény módosítás
            if ( eredeti != null )
            {
               command = connection.CreateCommand( );
               //módoítom a korosztályt
               command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + ( _megjelent ? "1" : "0" ) + ", INKOMO = '" + ( _korosztalymodositott ? "1" : "0" ) + "', KOAZON = '" + _korosztalyazonosito + "' WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }
            //új eredmény beírás -> nincs korosztályazonosító,csak ha korosztálymódosított = true
            else
            {
               command = connection.CreateCommand( );
               command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON) " +
                  "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + ( _megjelent ? "1" : "0" ) + ( _korosztalymodositott ? ",1" : ",0" ) + ",'" + _korosztalyazonosito + "');";
               command.ExecuteNonQuery( );
               command.Dispose( );

               // Induló eredmények számának növelése
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }

            Eredmény? eredmény = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredmény = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            connection.Close( );
            return new BeírásEredmény( eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT );
         }
      }

      public BeírásEredmény EredményBeírás_Ellenőrzött( string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _korosztalymodositott )
      {
         lock ( Program.datalock )
         {
            bool found;

            connection.Open( );
            SQLiteCommand command;

            // Verseny meglétének és lezárásának ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) ) { if ( _verseny == reader.GetString( 0 ) && reader.GetBoolean( 1 ) == false ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); };

            // Induló meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE FROM Indulók;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _név == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); ; };

            // Íjtípus meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _íjtípus == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); };

            // Név meglétének ellenőrzése a versenyen
            Eredmény? eredeti = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredeti = new Eredmény( reader.GetString( 0 ),
                   reader.GetInt64( 1 ),
                   reader.GetString( 2 ),
                   reader.GetInt32( 3 ),
                   reader.GetInt32( 4 ),
                   reader.GetInt32( 5 ),
                   reader.GetInt32( 6 ),
                   reader.GetInt32( 7 ),
                   reader.GetInt32( 8 ),
                   reader.GetInt32( 9 ),
                   reader.GetBoolean( 10 ),
                   reader.GetBoolean( 11 ),
                   reader.GetString( 12 ) );
            }
            command.Dispose( );

            if ( eredeti != null )
            {
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + ( _megjelent ? "1" : "0" ) + ", INKOMO = " + ( _korosztalymodositott ? "1" : "0" ) + " WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }
            else
            {
               command = connection.CreateCommand( );
               command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ,INKOMO)" +
                  "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + ( _megjelent ? "1" : "0" ) + ( _korosztalymodositott ? "1" : "0" ) + ");";
               command.ExecuteNonQuery( );
               command.Dispose( );

               // Induló eredmények számának növelése
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }

            Eredmény? eredmény = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ,INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredmény = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            connection.Close( );
            return new BeírásEredmény( eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT );
         }
      }


      public Int64 EredményMódosítás( string _azonosító, Eredmény _eredeti, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            connection.Open( );
            SQLiteCommand command;
            if ( _eredeti.Nev != _eredmény.Nev )
            {
               command = connection.CreateCommand( );
               command.CommandText = "SELECT INNEVE FROM Eredmények_" + _azonosító + ";";

               bool found = false;
               SQLiteDataReader reader = command.ExecuteReader();
               while ( reader.Read( ) )
               {
                  if ( _eredmény.Nev == reader.GetString( 0 ) ) found = true;
               }

               if ( found )
               {
                  command.Dispose( );
                  connection.Close( );
                  return -666;
               }
            }

            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET INNEVE = '" + _eredmény.Nev + "', ITAZON = '" + _eredmény.Ijtipus + "', INCSSZ = '" + _eredmény.Csapat + "', " +
                "IN10TA = " + _eredmény.Talalat10 + ", IN08TA = " + _eredmény.Talalat8 + ", IN05TA = " + _eredmény.Talalat5 + ", INMETA = " + _eredmény.Melle + ", " +
                "INOSZP = " + _eredmény.Osszpont + ", INERSZ = " + _eredmény.Szazalek + ", INMEGJ = " + ( _eredmény.Megjelent ? "1" : "0" ) + ", INKOMO = " + ( _eredmény.KorosztalyModositott ? "'1'" : "'0'" ) + ", KOAZON= '" + _eredmény.KorosztalyAzonosito + "' WHERE INNEVE = '" + _eredeti.Nev + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return 1;
         }
      }

      public Nullable<Eredmény> EredményMódosítás_Ellenőrzött( string _azonosító, Eredmény _eredeti, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            bool found;

            connection.Open( );
            SQLiteCommand command;

            // Verseny meglétének és lezárásának ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _azonosító == reader.GetString( 0 ) && reader.GetBoolean( 1 ) == false ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };

            // Induló meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE FROM Indulók;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _eredmény.Nev == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };

            // Íjtípus meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _eredmény.Ijtipus == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };


            // Százalék számítás összespont lekérdezése után
            int összespont = -666;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               összespont = reader.GetInt32( 0 );
            }
            command.Dispose( );

            if ( !( ( _eredmény.Talalat10 == 0 && _eredmény.Talalat8 == 0 && _eredmény.Talalat5 == 0 && _eredmény.Melle == 0 )
                || ( összespont == _eredmény.Talalat10 + _eredmény.Talalat8 + _eredmény.Talalat5 + _eredmény.Melle ) ) )
            {
               connection.Close( ); return null;
            }

            int összes = _eredmény.Talalat10 * 10 + _eredmény.Talalat8 * 8 + _eredmény.Talalat5 * 5;
            int százalék = (int)( ( (double)összes / ( összespont * 10 ) ) * 100 );

            // Adatbázis módosítás
            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET ITAZON = '" + _eredmény.Ijtipus + "', INCSSZ = '" + _eredmény.Csapat + "', " +
                "IN10TA = " + _eredmény.Talalat10 + ", IN08TA = " + _eredmény.Talalat8 + ", IN05TA = " + _eredmény.Talalat5 + ", INMETA = " + _eredmény.Melle + ", " +
                "INOSZP = " + összes + ", INERSZ = " + százalék + ", INMEGJ = " + ( _eredmény.Megjelent ? "1" : "0" ) + ", INKOMO = " + ( _eredmény.KorosztalyModositott ? "1" : "0" ) + " SET KOAZON = '" + _eredmény.KorosztalyAzonosito + "' WHERE INNEVE = '" + _eredeti.Nev + "';";
            command.ExecuteNonQuery( );

            // Sorszám, megjelent lekérdezése
            Int64 sorszám = -666;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INSOSZ FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.Nev + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               sorszám = reader.GetInt64( 0 );
            }
            command.Dispose( );

            command.Dispose( );
            connection.Close( );
            return new Eredmény( _eredmény.Nev,
                                sorszám,
                                _eredmény.Ijtipus,
                                _eredmény.Csapat,
                                _eredmény.Talalat10,
                                _eredmény.Talalat8,
                                _eredmény.Talalat5,
                                _eredmény.Melle,
                                összes,
                                százalék,
                                _eredmény.Megjelent,
                                _eredmény.KorosztalyModositott,
                                _eredmény.KorosztalyAzonosito );
         }
      }

      public bool EredményTörlés( string _azonosító, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.Nev + "';";
            command.ExecuteNonQuery( );

            // Induló eredmények számának csökkentése
            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ - 1 WHERE INNEVE = '" + _eredmény.Nev + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }
      #endregion


      #region Nyomtat

      #region CsapatLista

      public List<Nyomtat.CSAPATLISTA.CSAPAT>
      CsapatLista( string _VEAZON )
      {
         lock ( Program.datalock )
         {
            List<Nyomtat.CSAPATLISTA.CSAPAT> Data = new List<Nyomtat.CSAPATLISTA.CSAPAT>( );
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "select distinct Eredmények_" + _VEAZON + ".INCSSZ, count(Eredmények_" + _VEAZON + " .INCSSZ) " +
                                  "from Eredmények_" + _VEAZON + " group by INCSSZ order by INCSSZ;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               if ( reader.GetInt32( 1 ) > 0 )
               {
                  Data.Add( new Nyomtat.CSAPATLISTA.CSAPAT( reader.GetInt32( 0 ) ) );
               }
            }

            command.Dispose( );
            connection.Close( );

            return Data;
         }
      }

      #endregion

      #region NevezesiLista

      #endregion

      #endregion
      public void Dispose( )
      {
         connection.Close( );
      }
   }
}
