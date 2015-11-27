using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Íjász
{
   partial class Database
   {

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

   }
}
