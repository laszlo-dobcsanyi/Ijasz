using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász {
    partial class Database {

        public List<Oklevel> Oklevelek( ) {
            List<Oklevel> Value = new List<Oklevel>( );

            lock ( Program.datalock ) {
                connection.Open( );
                SQLiteCommand command = connection.CreateCommand( );

                command.CommandText = "SELECT OKAZON, OKTIPU, " +
                  "OKVENEX, OKVENEY, OKVENEH, OKVENEF , OKVENEB , OKVENEM , OKVENEI ," +
                  "OKVSNEX, OKVSNEY, OKVSNEH, OKVSNEF , OKVSNEB , OKVSNEM , OKVSNEI ," +
                  "OKHELYX, OKHELYY, OKHELYH, OKHELYF , OKHELYB , OKHELYM , OKHELYI ," +
                  "OKNEVEX, OKNEVEY, OKNEVEH, OKNEVEF , OKNEVEB , OKNEVEM , OKNEVEI ," +
                  "OKEGYEX, OKEGYEY, OKEGYEH, OKEGYEF , OKEGYEB , OKEGYEM , OKEGYEI ," +
                  "OKIJTIX, OKIJTIY, OKIJTIH, OKIJTIF , OKIJTIB , OKIJTIM , OKIJTII ," +
                  "OKKOROX, OKKOROY, OKKOROH, OKKOROF , OKKOROB , OKKOROM , OKKOROI ," +
                  "OKNEMEX, OKNEMEY, OKNEMEH, OKNEMEF , OKNEMEB , OKNEMEM , OKNEMEI ," +
                  "OKDATUX, OKDATUY, OKDATUH, OKDATUF , OKDATUB , OKDATUM , OKDATUI " +
                  " FROM Oklevelek;";


                var reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
                    int index = -1;
                    Value.Add( new Oklevel( reader.GetString( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetString( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),

                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetInt32( ++index ),
                                            reader.GetString( ++index ),
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

        public bool UjOklevel( Oklevel oklevel ) {
            bool value = false;

            lock ( Program.datalock ) {
                connection.Open( );
                SQLiteCommand command = connection.CreateCommand( );

                command.CommandText = "INSERT INTO Oklevelek (" +
                                      "OKAZON, OKTIPU, " +
                                      "OKVENEX, OKVENEY, OKVENEH, OKVENEF , OKVENEB , OKVENEM , OKVENEI ," +
                                      "OKVSNEX, OKVSNEY, OKVSNEH, OKVSNEF , OKVSNEB , OKVSNEM , OKVSNEI ," +
                                      "OKHELYX, OKHELYY, OKHELYH, OKHELYF , OKHELYB , OKHELYM , OKHELYI ," +
                                      "OKNEVEX, OKNEVEY, OKNEVEH, OKNEVEF , OKNEVEB , OKNEVEM , OKNEVEI ," +
                                      "OKEGYEX, OKEGYEY, OKEGYEH, OKEGYEF , OKEGYEB , OKEGYEM , OKEGYEI ," +
                                      "OKIJTIX, OKIJTIY, OKIJTIH, OKIJTIF , OKIJTIB , OKIJTIM , OKIJTII ," +
                                      "OKKOROX, OKKOROY, OKKOROH, OKKOROF , OKKOROB , OKKOROM , OKKOROI ," +
                                      "OKNEMEX, OKNEMEY, OKNEMEH, OKNEMEF , OKNEMEB , OKNEMEM , OKNEMEI ," +
                                      "OKDATUX, OKDATUY, OKDATUH, OKDATUF , OKDATUB , OKDATUM , OKDATUI" +
                                      ") VALUES (" +
                                      "@OKAZON,  @OKTIPU, " +
                                      "@OKVENEX, @OKVENEY, @OKVENEH, @OKVENEF , @OKVENEB , @OKVENEM , @OKVENEI ," +
                                      "@OKVSNEX, @OKVSNEY, @OKVSNEH, @OKVSNEF , @OKVSNEB , @OKVSNEM , @OKVSNEI ," +
                                      "@OKHELYX, @OKHELYY, @OKHELYH, @OKHELYF , @OKHELYB , @OKHELYM , @OKHELYI ," +
                                      "@OKNEVEX, @OKNEVEY, @OKNEVEH, @OKNEVEF , @OKNEVEB , @OKNEVEM , @OKNEVEI ," +
                                      "@OKEGYEX, @OKEGYEY, @OKEGYEH, @OKEGYEF , @OKEGYEB , @OKEGYEM , @OKEGYEI ," +
                                      "@OKIJTIX, @OKIJTIY, @OKIJTIH, @OKIJTIF , @OKIJTIB , @OKIJTIM , @OKIJTII ," +
                                      "@OKKOROX, @OKKOROY, @OKKOROH, @OKKOROF , @OKKOROB , @OKKOROM , @OKKOROI ," +
                                      "@OKNEMEX, @OKNEMEY, @OKNEMEH, @OKNEMEF , @OKNEMEB , @OKNEMEM , @OKNEMEI ," +
                                      "@OKDATUX, @OKDATUY, @OKDATUH, @OKDATUF , @OKDATUB , @OKDATUM , @OKDATUI" +
                                      ");";

                command.Parameters.Add( new SQLiteParameter( "@OKAZON", oklevel.Azonosito ) );
                command.Parameters.Add( new SQLiteParameter( "@OKTIPU", oklevel.Tipus ) );

                command.Parameters.Add( new SQLiteParameter( "@OKVENEX", oklevel.VersenyX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEY", oklevel.VersenyY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEH", oklevel.VersenyH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEF", oklevel.VersenyF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEB", oklevel.VersenyB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEM", oklevel.VersenyM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEI", oklevel.VersenyI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKVSNEX", oklevel.VersenySorozatX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEY", oklevel.VersenySorozatY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEH", oklevel.VersenySorozatH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEF", oklevel.VersenySorozatF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEB", oklevel.VersenySorozatB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEM", oklevel.VersenySorozatM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEI", oklevel.VersenySorozatI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKHELYX", oklevel.HelyezesX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYY", oklevel.HelyezesY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYH", oklevel.HelyezesH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYF", oklevel.HelyezesF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYB", oklevel.HelyezesB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYM", oklevel.HelyezesM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYI", oklevel.HelyezesI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKNEVEX", oklevel.InduloX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEY", oklevel.InduloY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEH", oklevel.InduloH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEF", oklevel.InduloF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEB", oklevel.InduloB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEM", oklevel.InduloM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEI", oklevel.InduloI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKEGYEX", oklevel.EgyesuletX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEY", oklevel.EgyesuletY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEH", oklevel.EgyesuletH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEF", oklevel.EgyesuletF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEB", oklevel.EgyesuletB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEM", oklevel.EgyesuletM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEI", oklevel.EgyesuletI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKIJTIX", oklevel.IjtipusX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIY", oklevel.IjtipusY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIH", oklevel.IjtipusH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIF", oklevel.IjtipusF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIB", oklevel.IjtipusB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIM", oklevel.IjtipusM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTII", oklevel.IjtipusI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKKOROX", oklevel.KorosztalyX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROY", oklevel.KorosztalyY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROH", oklevel.KorosztalyH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROF", oklevel.KorosztalyF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROB", oklevel.KorosztalyB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROM", oklevel.KorosztalyM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROI", oklevel.KorosztalyI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKNEMEX", oklevel.InduloNemeX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEY", oklevel.InduloNemeY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEH", oklevel.InduloNemeH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEF", oklevel.InduloNemeF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEB", oklevel.InduloNemeB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEM", oklevel.InduloNemeM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEI", oklevel.InduloNemeI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKDATUX", oklevel.DatumX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUY", oklevel.DatumY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUH", oklevel.DatumH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUF", oklevel.DatumF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUB", oklevel.DatumB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUM", oklevel.DatumM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUI", oklevel.DatumI ) );

                try {
                    command.ExecuteNonQuery( );
                } catch( SQLiteException exception ) {
                    MessageBox.Show( exception.Message );
                } finally {
                    command.Dispose( );
                    connection.Close( );
                }
                value = true;
            }

            return value;
        }

        public bool OklevelModositas( string _Azonosito, Oklevel oklevel ) {
            lock ( Program.datalock ) {
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand( );

                command.CommandText = "UPDATE Oklevelek SET " +
                                                "OKAZON=@OKAZON, " +
                                                "OKTIPU=@OKTIPU, " +
                                                "OKVENEX=@OKVENEX, " +
                                                "OKVENEY=@OKVENEY, " +
                                                "OKVENEH=@OKVENEH, " +
                                                "OKVENEF=@OKVENEF, " +
                                                "OKVENEB=@OKVENEB, " +
                                                "OKVENEM=@OKVENEM, " +
                                                "OKVENEI=@OKVENEI, " +
                                                "OKVSNEX=@OKVSNEX, " +
                                                "OKVSNEY=@OKVSNEY, " +
                                                "OKVSNEH=@OKVSNEH, " +
                                                "OKVSNEF=@OKVSNEF, " +
                                                "OKVSNEB=@OKVSNEB, " +
                                                "OKVSNEM=@OKVSNEM, " +
                                                "OKVSNEI=@OKVSNEI, " +
                                                "OKHELYX=@OKHELYX, " +
                                                "OKHELYY=@OKHELYY, " +
                                                "OKHELYH=@OKHELYH, " +
                                                "OKHELYF=@OKHELYF, " +
                                                "OKHELYB=@OKHELYB, " +
                                                "OKHELYM=@OKHELYM, " +
                                                "OKHELYI=@OKHELYI, " +
                                                "OKNEVEX=@OKNEVEX, " +
                                                "OKNEVEY=@OKNEVEY, " +
                                                "OKNEVEH=@OKNEVEH, " +
                                                "OKNEVEF=@OKNEVEF, " +
                                                "OKNEVEB=@OKNEVEB, " +
                                                "OKNEVEM=@OKNEVEM, " +
                                                "OKNEVEI=@OKNEVEI, " +
                                                "OKEGYEX=@OKEGYEX, " +
                                                "OKEGYEY=@OKEGYEY, " +
                                                "OKEGYEH=@OKEGYEH, " +
                                                "OKEGYEF=@OKEGYEF, " +
                                                "OKEGYEB=@OKEGYEB, " +
                                                "OKEGYEM=@OKEGYEM, " +
                                                "OKEGYEI=@OKEGYEI, " +
                                                "OKIJTIX=@OKIJTIX, " +
                                                "OKIJTIY=@OKIJTIY, " +
                                                "OKIJTIH=@OKIJTIH, " +
                                                "OKIJTIF=@OKIJTIF, " +
                                                "OKIJTIB=@OKIJTIB, " +
                                                "OKIJTIM=@OKIJTIM, " +
                                                "OKIJTII=@OKIJTII, " +
                                                "OKKOROX=@OKKOROX, " +
                                                "OKKOROY=@OKKOROY, " +
                                                "OKKOROH=@OKKOROH, " +
                                                "OKKOROF=@OKKOROF, " +
                                                "OKKOROB=@OKKOROB, " +
                                                "OKKOROM=@OKKOROM, " +
                                                "OKKOROI=@OKKOROI, " +
                                                "OKNEMEX=@OKNEMEX, " +
                                                "OKNEMEY=@OKNEMEY, " +
                                                "OKNEMEH=@OKNEMEH, " +
                                                "OKNEMEF=@OKNEMEF, " +
                                                "OKNEMEB=@OKNEMEB, " +
                                                "OKNEMEM=@OKNEMEM, " +
                                                "OKNEMEI=@OKNEMEI, " +
                                                "OKDATUX=@OKDATUX, " +
                                                "OKDATUY=@OKDATUY, " +
                                                "OKDATUH=@OKDATUH, " +
                                                "OKDATUF=@OKDATUF, " +
                                                "OKDATUB=@OKDATUB, " +
                                                "OKDATUM=@OKDATUM, " +
                                                "OKDATUI=@OKDATUI " +
                                                "WHERE OKAZON=@OKAZON";

                command.Parameters.Add( new SQLiteParameter( "@OKAZON", oklevel.Azonosito ) );
                command.Parameters.Add( new SQLiteParameter( "@OKTIPU", oklevel.Tipus ) );

                command.Parameters.Add( new SQLiteParameter( "@OKVENEX", oklevel.VersenyX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEY", oklevel.VersenyY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEH", oklevel.VersenyH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEF", oklevel.VersenyF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEB", oklevel.VersenyB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEM", oklevel.VersenyM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVENEI", oklevel.VersenyI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKVSNEX", oklevel.VersenySorozatX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEY", oklevel.VersenySorozatY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEH", oklevel.VersenySorozatH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEF", oklevel.VersenySorozatF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEB", oklevel.VersenySorozatB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEM", oklevel.VersenySorozatM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKVSNEI", oklevel.VersenySorozatI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKHELYX", oklevel.HelyezesX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYY", oklevel.HelyezesY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYH", oklevel.HelyezesH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYF", oklevel.HelyezesF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYB", oklevel.HelyezesB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYM", oklevel.HelyezesM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKHELYI", oklevel.HelyezesI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKNEVEX", oklevel.InduloX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEY", oklevel.InduloY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEH", oklevel.InduloH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEF", oklevel.InduloF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEB", oklevel.InduloB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEM", oklevel.InduloM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEVEI", oklevel.InduloI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKEGYEX", oklevel.EgyesuletX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEY", oklevel.EgyesuletY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEH", oklevel.EgyesuletH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEF", oklevel.EgyesuletF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEB", oklevel.EgyesuletB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEM", oklevel.EgyesuletM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKEGYEI", oklevel.EgyesuletI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKIJTIX", oklevel.IjtipusX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIY", oklevel.IjtipusY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIH", oklevel.IjtipusH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIF", oklevel.IjtipusF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIB", oklevel.IjtipusB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTIM", oklevel.IjtipusM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKIJTII", oklevel.IjtipusI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKKOROX", oklevel.KorosztalyX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROY", oklevel.KorosztalyY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROH", oklevel.KorosztalyH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROF", oklevel.KorosztalyF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROB", oklevel.KorosztalyB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROM", oklevel.KorosztalyM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKKOROI", oklevel.KorosztalyI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKNEMEX", oklevel.InduloNemeX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEY", oklevel.InduloNemeY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEH", oklevel.InduloNemeH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEF", oklevel.InduloNemeF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEB", oklevel.InduloNemeB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEM", oklevel.InduloNemeM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKNEMEI", oklevel.InduloNemeI ) );

                command.Parameters.Add( new SQLiteParameter( "@OKDATUX", oklevel.DatumX ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUY", oklevel.DatumY ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUH", oklevel.DatumH ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUF", oklevel.DatumF ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUB", oklevel.DatumB ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUM", oklevel.DatumM ) );
                command.Parameters.Add( new SQLiteParameter( "@OKDATUI", oklevel.DatumI ) );



                try {
                    command.ExecuteNonQuery( );
                } catch( SQLiteException e ) {
                    return false;
                } finally {
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


                command.CommandText = "select OKAZON, OKTIPU, " +
                                      "OKVENEX, OKVENEY, OKVENEH, OKVENEF, OKVENEM, OKVENEI," +
                                      "OKVSNEX, OKVSNEY, OKVSNEH, OKVSNEF, OKVSNEM, OKVSNEI, " +
                                      "OKHELYX, OKHELYY, OKHELYH, OKHELYF, OKHELYM, OKHELYI, " +
                                      "OKINNEVEX, OKINNEVEY, OKINNEVEH, OKINNEVEF, OKINNEVEM, OKINNEVEI, " +
                                      "OKEGYEX, OKEGYEY, OKEGYEH, OKEGYEF, OKEGYEM, OKEGYEI, " +
                                      "OKIJTIX, OKIJTIY, OKIJTIH, OKIJTIF, OKIJTIM, OKIJTII, " +
                                      "OKKOROX, OKKOROY, OKKOROH, OKKOROF, OKKOROM, OKKOROI, " +
                                      "OKINNEMEX, OKINNEMEY, OKINNEMEH, OKINNEMEF, OKINNEMEM, OKINNEMEI, " +
                                      "OKDATUX, OKDATUY, OKDATUH, OKDATUF, OKDATUM, OKDATUI from Oklevelek WHERE OKAZON = '" + _azonosito + "';";

                var reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
                    int c = -1;
                    Value = new Oklevel( reader.GetString( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetString( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),

                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetInt32( ++c ),
                                             reader.GetString( ++c ),
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
