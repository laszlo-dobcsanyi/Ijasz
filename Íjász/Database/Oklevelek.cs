using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Íjász {
   partial class Database {

      public List<Oklevel> Oklevelek( ) {
         List<Oklevel> Value = new List<Oklevel>( );

         lock ( Program.datalock ) {
            connection.Open( );
            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "select OKAZON, OKTIP, OKNEVEX, OKNEVEY, OKHELYX, OKHELYY, OKKATEX, OKKATEY, OKHESZX, OKHESZY, OKDATUX, OKDATUY, OKEGYEX, OKEGYEY from Oklevelek;";
            var reader = command.ExecuteReader( );
            while ( reader.Read( ) ) {
               Value.Add( new Oklevel( reader.GetString( 0 ),
                                       reader.GetString( 1 ),

                                       reader.GetInt32( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetInt32( 10 ),
                                       reader.GetInt32( 11 ),
                                       reader.GetInt32( 12 ),
                                       reader.GetInt32( 13 ) ) );
            }

            command.Dispose( );
            connection.Close( );
         }
         return Value;
      }

      public bool UjOklevel( Oklevel _Oklevel ) {
         bool value = false;

         lock ( Program.datalock ) {
            connection.Open( );
            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "insert into Oklevelek(OKAZON, OKTIP, OKNEVEX, OKNEVEY, OKHELYX, OKHELYY, OKKATEX, OKKATEY, OKHESZX, OKHESZY, OKDATUX, OKDATUY, OKEGYEX, OKEGYEY) " +
               "VALUES( '" + _Oklevel.Azonosito + "', '" + _Oklevel.Tipus + "', " + _Oklevel.NevX + ", " + _Oklevel.NevY + 
               "," + _Oklevel.HelyezesX + "," + _Oklevel.HelyezesY + 
               "," + _Oklevel.KategoriaX + "," + _Oklevel.KategoriaY + 
               "," + _Oklevel.HelyszinX + "," + _Oklevel.HelyszinY + 
               "," + _Oklevel.DatumX + "," + _Oklevel.DatumY +
               "," + _Oklevel.EgyesuletX + "," + _Oklevel.EgyesuletY + " );";

            command.ExecuteNonQuery( );
            command.Dispose( );
            connection.Close( );
            value = true;
         }

         return value;
      }

      public bool OklevelModositas( string _Azonosito, Oklevel _Oklevel ) {

         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "UPDATE Oklevelek " +
               "SET OKAZON = '" + _Oklevel.Azonosito + "', OKTIP = '" + _Oklevel.Tipus +
               "', OKNEVEX = " + _Oklevel.NevX + ", OKNEVEY = " + _Oklevel.NevY +
               ", OKHELYX = " + _Oklevel.HelyezesX + ", OKHELYY = " + _Oklevel.HelyezesY +
               ", OKKATEX = " + _Oklevel.KategoriaX + ", OKKATEY = " + _Oklevel.KategoriaY +
               ", OKHESZX = " + _Oklevel.HelyszinX + ", OKHESZY = " + _Oklevel.HelyszinY +
               ", OKDATUX = " + _Oklevel.DatumX + ", OKDATUY = " + _Oklevel.DatumY +
               ", OKEGYEX = " + _Oklevel.EgyesuletX + ", OKEGYEY = " + _Oklevel.EgyesuletY +
               " WHERE OKAZON = '" + _Oklevel.Azonosito + "';";
            try {
               command.ExecuteNonQuery( );
            }
            catch ( SQLiteException ) {
               return false;
            }
            finally {
               command.Dispose( );
               connection.Close( );
            }
         }
         return true;
      }

      public bool OklevelTorles( string _azonosito ) {
         lock ( Program.datalock ) {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "DELETE FROM Oklevelek WHERE OKAZON = '" + _azonosito + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }
   }
}
