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
                command.CommandText = "select OKAZON, OKTIP," +
                        " OKNEVEX, OKNEVEY, OKNEVEH," +
                        " OKHELYX, OKHELYY, OKHELYH," +
                        " OKKATEX, OKKATEY, OKKATEH," +
                        " OKHESZX, OKHESZY, OKHESZH," +
                        " OKDATUX, OKDATUY, OKDATUH," +
                        " OKEGYEX, OKEGYEY, OKEGYEH" +
                        " from Oklevelek;";
                var reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
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
                                            reader.GetInt32( 13 ),

                                            reader.GetInt32( 14 ),
                                            reader.GetInt32( 15 ),
                                            reader.GetInt32( 16 ),

                                            reader.GetInt32( 17 ),
                                            reader.GetInt32( 18 ),
                                            reader.GetInt32( 19 )
                                            ) );
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

                command.CommandText = "insert into Oklevelek(OKAZON, OKTIP," + 
                    " OKNEVEX, OKNEVEY, OKNEVEH," + 
                    " OKHELYX, OKHELYY, OKHELYH," + 
                    " OKKATEX, OKKATEY, OKKATEH," + 
                    " OKHESZX, OKHESZY, OKHESZH," + 
                    " OKDATUX, OKDATUY, OKDATUH," + 
                    " OKEGYEX, OKEGYEY, OKEGYEH) " +
                   "VALUES( '" + _Oklevel.Azonosito + "', '" + _Oklevel.Tipus + 
                   "'," + _Oklevel.NevX + ", " + _Oklevel.NevY + ", " + _Oklevel.NevH +
                   "," + _Oklevel.HelyezesX + "," + _Oklevel.HelyezesY + "," + _Oklevel.HelyezesH +
                   "," + _Oklevel.KategoriaX + "," + _Oklevel.KategoriaY + "," + _Oklevel.KategoriaH +
                   "," + _Oklevel.HelyszinX + "," + _Oklevel.HelyszinY + "," + _Oklevel.HelyszinH +
                   "," + _Oklevel.DatumX + "," + _Oklevel.DatumY + "," + _Oklevel.DatumH +
                   "," + _Oklevel.EgyesuletX + "," + _Oklevel.EgyesuletY + "," + _Oklevel.EgyesuletH + " );";

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
                   "', OKNEVEX = " + _Oklevel.NevX + ", OKNEVEY = " + _Oklevel.NevY + ", OKNEVEH = " + _Oklevel.NevH +
                   ", OKHELYX = " + _Oklevel.HelyezesX + ", OKHELYY = " + _Oklevel.HelyezesY + ", OKHELYH = " + _Oklevel.HelyezesH +
                   ", OKKATEX = " + _Oklevel.KategoriaX + ", OKKATEY = " + _Oklevel.KategoriaY + ", OKKATEH = " + _Oklevel.KategoriaH +
                   ", OKHESZX = " + _Oklevel.HelyszinX + ", OKHESZY = " + _Oklevel.HelyszinY + ", OKHESZH = " + _Oklevel.HelyszinH +
                   ", OKDATUX = " + _Oklevel.DatumX + ", OKDATUY = " + _Oklevel.DatumY + ", OKDATUH = " + _Oklevel.DatumH +
                   ", OKEGYEX = " + _Oklevel.EgyesuletX + ", OKEGYEY = " + _Oklevel.EgyesuletY + ", OKEGYEH = " + _Oklevel.EgyesuletH +
                   " WHERE OKAZON = '" + _Oklevel.Azonosito + "';";
                try {
                    command.ExecuteNonQuery( );
                }
                catch( SQLiteException ) {
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
