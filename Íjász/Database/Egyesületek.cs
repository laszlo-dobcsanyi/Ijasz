using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Íjász
{
   partial class Database
   {
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


   }
}
