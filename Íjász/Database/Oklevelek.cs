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

                command.CommandText = "select OKAZON, OKTIPU, OKBETU, " +
                                      "OKVENEX, OKVENEY, OKVENEH, OKVENEF, OKVENEM, OKVENEI," +
                                      "OKVSNEX, OKVSNEY, OKVSNEH, OKVSNEF, OKVSNEM, OKVSNEI, " +
                                      "OKHELYX, OKHELYY, OKHELYH, OKHELYF, OKHELYM, OKHELYI, " +
                                      "OKINNEVEX, OKINNEVEY, OKINNEVEH, OKINNEVEF, OKINNEVEM, OKINNEVEI, " +
                                      "OKEGYEX, OKEGYEY, OKEGYEH, OKEGYEF, OKEGYEM, OKEGYEI, " +
                                      "OKIJTIX, OKIJTIY, OKIJTIH, OKIJTIF, OKIJTIM, OKIJTII, " +
                                      "OKKOROX, OKKOROY, OKKOROH, OKKOROF, OKKOROM, OKKOROI, " +
                                      "OKINNEMEX, OKINNEMEY, OKINNEMEH, OKINNEMEF, OKINNEMEM, OKINNEMEI, " +
                                      "OKDATUX, OKDATUY, OKDATUH, OKDATUF, OKDATUM, OKDATUI from Oklevelek;";



                var reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
                    int index = -1;
                        Value.Add( new Oklevel( reader.GetString( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),


                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),

                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index ),
                                                reader.GetInt32( ++index ),
                                                reader.GetString( ++index )
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

                command.CommandText =
                    "insert into Oklevelek(OKAZON,OKTIPU,OKBETU," +
                    "OKVENEX,OKVENEY,OKVENEH,OKVENEF,OKVENEM,OKVENEI," +
                    "OKVSNEX,OKVSNEY,OKVSNEH,OKVSNEF,OKVSNEM,OKVSNEI," +
                    "OKHELYX,OKHELYY,OKHELYH,OKHELYF,OKHELYM,OKHELYI," +
                    "OKINNEVEX,OKINNEVEY,OKINNEVEH,OKINNEVEF,OKINNEVEM,OKINNEVEI," +
                    "OKEGYEX,OKEGYEY,OKEGYEH,OKEGYEF,OKEGYEM,OKEGYEI," +
                    "OKIJTIX,OKIJTIY,OKIJTIH,OKIJTIF,OKIJTIM,OKIJTII," +
                    "OKKOROX,OKKOROY,OKKOROH,OKKOROF,OKKOROM,OKKOROI," +
                    "OKINNEMEX,OKINNEMEY,OKINNEMEH,OKINNEMEF,OKINNEMEM,OKINNEMEI," +
                    "OKDATUX,OKDATUY,OKDATUH,OKDATUF,OKDATUM,OKDATUI)" +
                    "VALUES( '" + _Oklevel.Azonosito + "','" + _Oklevel.Tipus + "','" + _Oklevel.BetuTipus + "', " +
                    _Oklevel.VersenyX + ", " + _Oklevel.VersenyY + ", " + _Oklevel.VersenyH + ", '" + _Oklevel.VersenyF +
                    "', " + _Oklevel.VersenyM + ", '" + _Oklevel.VersenyI + "', " +
                    _Oklevel.VersenySorozatX + ", " + _Oklevel.VersenySorozatY + ", " + _Oklevel.VersenySorozatH + ", '" +
                    _Oklevel.VersenySorozatF + "', " + _Oklevel.VersenySorozatM + ", '" + _Oklevel.VersenySorozatI +
                    "', " +
                    _Oklevel.HelyezesX + ", " + _Oklevel.HelyezesY + ", " + _Oklevel.HelyezesH + ", '" +
                    _Oklevel.HelyezesF + "', " + _Oklevel.HelyezesM + ", '" + _Oklevel.HelyezesI + "', " +
                    _Oklevel.InduloX + ", " + _Oklevel.InduloY + ", " + _Oklevel.InduloH + ", '" + _Oklevel.InduloF +
                    "', " + _Oklevel.InduloM + ", '" + _Oklevel.InduloI + "', " +
                    _Oklevel.EgyesuletX + ", " + _Oklevel.EgyesuletY + ", " + _Oklevel.EgyesuletH + ", '" +
                    _Oklevel.EgyesuletF + "', " + _Oklevel.EgyesuletM + ", '" + _Oklevel.EgyesuletI + "', " +
                    _Oklevel.IjtipusX + ", " + _Oklevel.IjtipusY + ", " + _Oklevel.IjtipusH + ", '" + _Oklevel.IjtipusF +
                    "', " + _Oklevel.IjtipusM + ", '" + _Oklevel.IjtipusI + "', " +
                    _Oklevel.KorosztalyX + ", " + _Oklevel.KorosztalyY + ", " + _Oklevel.KorosztalyH + ", '" +
                    _Oklevel.KorosztalyF + "', " + _Oklevel.KorosztalyM + ", '" + _Oklevel.KorosztalyI + "', " +
                    _Oklevel.InduloNemeX + ", " + _Oklevel.InduloNemeY + ", " + _Oklevel.InduloNemeH + ", '" +
                    _Oklevel.InduloNemeF + "', " + _Oklevel.InduloNemeM + ", '" + _Oklevel.InduloNemeI + "', " +
                    _Oklevel.DatumX + ", " + _Oklevel.DatumY + ", " + _Oklevel.DatumH + ", '" + _Oklevel.DatumF + "', " +
                    _Oklevel.DatumM + ", '" + _Oklevel.DatumI + "');";

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
                   "SET OKAZON= '" + _Oklevel.Azonosito + "', OKTIPU= '" + _Oklevel.Tipus + "', OKBETU= '" + _Oklevel.BetuTipus + "'" + 
                   ", OKVENEX= " + _Oklevel.VersenyX + ", OKVENEY= " + _Oklevel.VersenyY + ", OKVENEH= " + _Oklevel.VersenyH + ", OKVENEF= '" + _Oklevel.VersenyF + "', OKVENEM= " + _Oklevel.VersenyM + ", OKVENEI= '" + _Oklevel.VersenyI + "'" + 
                   ", OKVSNEX= " + _Oklevel.VersenySorozatX + ", OKVSNEY= " + _Oklevel.VersenySorozatY + ", OKVSNEH= " + _Oklevel.VersenySorozatH + ", OKVSNEF= '" + _Oklevel.VersenySorozatF + "', OKVSNEM= " + _Oklevel.VersenySorozatM + ", OKVSNEI= '" + _Oklevel.VersenySorozatI + "'" +
                   ", OKHELYX= " + _Oklevel.HelyezesX + ", OKHELYY= " + _Oklevel.HelyezesY + ", OKHELYH= " + _Oklevel.HelyezesH + ", OKHELYF= '" + _Oklevel.HelyezesF + "', OKHELYM= " + _Oklevel.HelyezesM + ", OKHELYI= '" + _Oklevel.HelyezesI + "'" +
                   ", OKINNEVEX= " + _Oklevel.InduloX + ", OKINNEVEY= " + _Oklevel.InduloY + ", OKINNEVEH= " + _Oklevel.InduloH + ", OKINNEVEF= '" + _Oklevel.InduloF + "', OKINNEVEM= " + _Oklevel.InduloM + ", OKINNEVEI= '" + _Oklevel.InduloI + "'" +
                   ", OKEGYEX= " + _Oklevel.EgyesuletX + ", OKEGYEY= " + _Oklevel.EgyesuletY + ", OKEGYEH= " + _Oklevel.EgyesuletH + ", OKEGYEF= '" + _Oklevel.EgyesuletF + "', OKEGYEM= " + _Oklevel.EgyesuletM + ", OKEGYEI= '" + _Oklevel.EgyesuletI + "'" +
                   ", OKIJTIX= " + _Oklevel.IjtipusX + ", OKIJTIY= " + _Oklevel.IjtipusY + ", OKIJTIH= " + _Oklevel.IjtipusH + ", OKIJTIF= '" + _Oklevel.IjtipusF + "', OKIJTIM= " + _Oklevel.IjtipusM + ", OKIJTII= '" + _Oklevel.IjtipusI + "'" +
                   ", OKKOROX= " + _Oklevel.KorosztalyX + ", OKKOROY= " + _Oklevel.KorosztalyY + ", OKKOROH= " + _Oklevel.KorosztalyH + ", OKKOROF= '" + _Oklevel.KorosztalyF + "', OKKOROM= " + _Oklevel.KorosztalyM + ", OKKOROI= '" + _Oklevel.KorosztalyI + "'" +
                   ", OKINNEMEX= " + _Oklevel.InduloNemeX + ", OKINNEMEY= " + _Oklevel.InduloNemeY + ", OKINNEMEH= " + _Oklevel.InduloNemeH + ", OKINNEMEF= '" + _Oklevel.InduloNemeF + "', OKINNEMEM= " + _Oklevel.InduloNemeM + ", OKINNEMEI= '" + _Oklevel.InduloNemeI + "'" +
                   ", OKDATUX= " + _Oklevel.DatumX + ", OKDATUY= " + _Oklevel.DatumY + ", OKDATUH= " + _Oklevel.DatumH + ", OKDATUF= '" + _Oklevel.DatumF + "', OKDATUM= " + _Oklevel.DatumM + ", OKDATUI= '" + _Oklevel.DatumI + "'" +
                   " WHERE OKAZON = '" + _Oklevel.Azonosito + "';";
                try {
                    command.ExecuteNonQuery( );
                }
                catch( SQLiteException e ) {
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

        public Oklevel Oklevel( string _azonosito ) {
            Oklevel Value = new Oklevel( );

            lock ( Program.datalock ) {
                connection.Open( );
                SQLiteCommand command = connection.CreateCommand( );
                command.CommandText = "select OKAZON,OKTIPU,OKBETU," +
                    "OKVENEX,OKVENEY,OKVENEH,OKVENEF,OKVENEM,OKVENEI," +
                    "OKVSNEX,OKVSNEY,OKVSNEH,OKVSNEF,OKVSNEM,OKVSNEI," +
                    "OKHELYX,OKHELYY,OKHELYH,OKHELYF,OKHELYM,OKHELYI," +
                    "OKINNEVEX,OKINNEVEY,OKINNEVEH,OKINNEVEF,OKINNEVEM,OKINNEVEI," +
                    "OKEGYEX,OKEGYEY,OKEGYEH,OKEGYEF,OKEGYEM,OKEGYEI," +
                    "OKIJTIX,OKIJTIY,OKIJTIH,OKIJTIF,OKIJTIM,OKIJTII," +
                    "OKKOROX,OKKOROY,OKKOROH,OKKOROF,OKKOROM,OKKOROI," +
                    "OKINNEMEX,OKINNEMEY,OKINNEMEH,OKINNEMEF,OKINNEMEM,OKINNEMEI," +
                    "OKDATUX,OKDATUY,OKDATUH,OKDATUF,OKDATUM,OKDATUI" +
                    " from Oklevelek where OKAZON = '" + _azonosito + "';";
                var reader = command.ExecuteReader( );
                while( reader.Read( ) )
                {
                    int c = -1;
                    Value = new Oklevel( reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c )
                                             );
                }
                command.Dispose( );
                connection.Close( );
            }
            return Value;
        }
    }
}
