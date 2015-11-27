using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace Íjász
{
   partial class Database
   {

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

   }
}
