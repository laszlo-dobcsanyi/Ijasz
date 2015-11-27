using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace Íjász {
   partial class Database {
      // TODO CSAK A SPAMMERNEK KELL!!
      public Nullable<Verseny> Verseny( string _azonosító ) {
         lock ( Program.datalock ) {
            Nullable<Verseny> data = null;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               data = new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public List<Verseny> Versenyek( ) {
         lock ( Program.datalock ) {
            List<Verseny> data = new List<Verseny>( );

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               data.Add( new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public List<Verseny> Versenyek_Aktív( ) {
         lock ( Program.datalock ) {
            List<Verseny> data = new List<Verseny>( );

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE FROM Verseny WHERE VELEZAR = 0;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               data.Add( new Verseny( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetInt32( 4 ), reader.GetInt32( 5 ), reader.GetInt32( 6 ), reader.GetBoolean( 7 ), reader.GetBoolean( 8 ) ) );
            }

            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public bool ÚjVerseny( Verseny _verseny ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "INSERT INTO Verseny (VEAZON, VEMEGN, VEDATU, VSAZON, VEOSPO, VEALSZ, VEINSZ, VELEZAR, VEDUBE) VALUES('" + _verseny.Azonosito + "', '" + _verseny.Megnevezes + "', " +
                "'" + _verseny.Datum + "', '" + _verseny.VersenySorozat + "', " + _verseny.Osszes + ", " + _verseny.Allomasok + ", 0, 0, " + Convert.ToInt32( _verseny.DublaBeirlap ) + ");";
            try {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException ) {
               command.Dispose( );
               connection.Close( );
               return false;
            }

            command = connection.CreateCommand( );
            command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSF, KOINSN,KOEGYB) VALUES" +
                "('" + _verseny.Azonosito + "', 'K10', '0-10', 1, 9, 1, 1, 0, 0,0)," +
                "('" + _verseny.Azonosito + "', 'K14', '10-14', 10, 13, 1, 1, 0, 0,0)," +
                "('" + _verseny.Azonosito + "', 'K18', '14-18', 14, 17, 1, 1, 0, 0,0)," +
                "('" + _verseny.Azonosito + "', 'K50', '18-50', 18, 49, 1, 1, 0, 0,0)," +
                "('" + _verseny.Azonosito + "', 'K100', '50-100', 50, 99, 1, 1, 0, 0,0);";
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

      public bool VersenyMódosítás( string _azonosító, Verseny _verseny ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "UPDATE Verseny SET VEAZON = '" + _verseny.Azonosito + "', VEMEGN = '" + _verseny.Megnevezes + "', VEDATU = '" + _verseny.Datum + "' ," +
                "VSAZON = '" + _verseny.VersenySorozat + "', VEOSPO = " + _verseny.Osszes + ", VEALSZ = " + _verseny.Allomasok + ", VEDUBE = " + Convert.ToInt32( _verseny.DublaBeirlap ) + " WHERE VEAZON = '" + _azonosító + "';";
            try {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException ) {
               command.Dispose( );
               connection.Close( );
               return false;
            }

            if ( _azonosító != _verseny.Azonosito ) {
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Korosztályok SET VEAZON = '" + _verseny.Azonosito + "' WHERE VEAZON = '" + _azonosító + "';";
               command.ExecuteNonQuery( );

               command = connection.CreateCommand( );
               command.CommandText = "ALTER TABLE Eredmények_" + _azonosító + " RENAME TO Eredmények_" + _verseny.Azonosito + ";";
               command.ExecuteNonQuery( );
            }

            command.Dispose( );
            connection.Close( );
            if ( _azonosító != _verseny.Azonosito ) {
               try {
                  Directory.Delete( _azonosító );
                  Directory.CreateDirectory( _verseny.Azonosito );
               }
               catch ( Exception ) {
                  return true;
               }
            }
            return true;
         }
      }

      public bool VersenyTörlés( string _azonosító ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
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

            try {
               Directory.Delete( _azonosító );
            }
            catch ( Exception ) {
               return true;
            }

            return true;
         }
      }

      ///

      public bool Verseny_IndulókNövelés( string _azonosító ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ + 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Verseny_IndulókCsökkentés( string _azonosító ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "UPDATE Verseny SET VEINSZ = VEINSZ - 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public int? Verseny_Állomások( string _azonosító ) {
         lock ( Program.datalock ) {
            int? value = null;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VEALSZ FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               value = reader.GetInt32( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return value;
         }
      }

      public int Verseny_Összespont( string _azonosító ) {
         lock ( Program.datalock ) {
            int összpont = -666;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               összpont = reader.GetInt32( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return összpont;
         }
      }

      public bool Verseny_Lezárva( string _azonosító ) {
         lock ( Program.datalock ) {
            bool lezárva = false;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT VELEZAR FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               lezárva = reader.GetBoolean( 0 );
            }

            command.Dispose( );
            connection.Close( );
            return lezárva;
         }
      }

      public bool Verseny_Lezárás( string _azonosító ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "UPDATE Verseny SET VELEZAR = 1 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

      public bool Verseny_Megnyitás( string _azonosító ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "UPDATE Verseny SET VELEZAR = 0 WHERE VEAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return true;
         }
      }

   }

}
