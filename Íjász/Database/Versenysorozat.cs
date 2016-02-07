using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Íjász {
    partial class Database {

        // TODO EXTRA, CSAK SPAMERNEK KELL!!
        public Nullable<Versenysorozat> Versenysorozat( string _azonosító ) {
            lock ( Program.datalock ) {

                Nullable<Versenysorozat> data = null;
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
                SQLiteDataReader reader = command.ExecuteReader();
                while( reader.Read( ) ) {
                    data = new Versenysorozat( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ) );
                }

                command.Dispose( );
                connection.Close( );
                return data;
            }
        }

        public List<Versenysorozat> Versenysorozatok( ) {
            lock ( Program.datalock ) {

                List<Versenysorozat> data = new List<Versenysorozat>();

                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT VSAZON, VSMEGN, VSVESZ FROM Versenysorozat;";
                SQLiteDataReader reader = command.ExecuteReader();
                while( reader.Read( ) ) {
                    data.Add( new Versenysorozat( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetInt32( 2 ) ) );
                }

                command.Dispose( );
                connection.Close( );
                return data;
            }
        }

        public bool ÚjVersenysorozat( Versenysorozat versenysorozat ) {
            lock ( Program.datalock ) {
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                //command.CommandText = "INSERT INTO Versenysorozat (VSAZON, VSMEGN, VSVESZ) VALUES('" + _versenysorozat.azonosító + "', '" + _versenysorozat.megnevezés + "', 0);";

                command.CommandText = "INSERT INTO Versenysorozat (VSAZON, VSMEGN, VSVESZ) VALUES(@VSAZON, @VSMEGN, @VSVESZ) ;";
                command.Parameters.AddWithValue( "@VSAZON", versenysorozat.azonosító );
                command.Parameters.AddWithValue( "@VSMEGN", versenysorozat.megnevezés );
                command.Parameters.AddWithValue( "@VSVESZ", "0" );

                try {
                    command.ExecuteNonQuery( );
                } catch( SQLiteException exception ) {
                    new ErrorMessage( ErrorCode.VERSENYSOROZAT_CREATE, exception.Message, versenysorozat );
                    return false;
                } finally {
                    command.Dispose( );
                    connection.Close( );
                }

                Directory.CreateDirectory( versenysorozat.azonosító );

                return true;
            }
        }

        public bool VersenysorozatMódosítás( string azonosito, Versenysorozat versenysorozat ) {
            lock ( Program.datalock ) {
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                //command.CommandText = "UPDATE Versenysorozat SET VSAZON = '" + _versenysorozat.azonosító + "', VSMEGN = '" + _versenysorozat.megnevezés + "' WHERE VSAZON = '" + _azonosító + "';";
                command.CommandText = "UPDATE Versenysorozat SET VSAZON=@VSAZON_NEW, VSMEGN=@VSMEGN, WHERE VSAZON=@VSAZON_OLD;";
                command.Parameters.AddWithValue( "@VSAZON_NEW", versenysorozat.azonosító );
                command.Parameters.AddWithValue( "@VSMEGM", versenysorozat.megnevezés );
                command.Parameters.AddWithValue( "@VSAZON_OLD", azonosito );

                try {
                    command.ExecuteNonQuery( );
                } catch( SQLiteException exception ) {
                    new ErrorMessage( ErrorCode.VERSENYSOROZAT_MODIFY, exception.Message, versenysorozat );
                    return false;
                } finally {
                    command.Dispose( );
                    connection.Close( );
                    if( azonosito != versenysorozat.azonosító && Directory.GetFiles( azonosito ).Length == 0 ) {
                        Directory.Delete( azonosito );
                        Directory.CreateDirectory( versenysorozat.azonosító );
                    }
                }
                return true;
            }
        }

        public bool VersenysorozatTörlés( string azonosito ) {
            lock ( Program.datalock ) {
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Versenysorozat WHERE VSAZON=@VSAZON;";
                command.Parameters.AddWithValue( "@VSAZON", azonosito );

                command.ExecuteNonQuery( );

                command.Dispose( );
                connection.Close( );

                if( Directory.GetFiles( azonosito ).Length == 0 ) { Directory.Delete( azonosito ); }

                return true;
            }
        }

        ///

        public bool Versenysorozat_VersenyekNövel( string azonosito ) {
            lock ( Program.datalock ) {
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ + 1 WHERE WHERE VSAZON=@VSAZON;";
                command.Parameters.AddWithValue( "@VSAZON", azonosito );

                command.ExecuteNonQuery( );

                command.Dispose( );
                connection.Close( );
                return true;
            }
        }

        public bool Versenysorozat_VersenyekCsökkent( string azonosito ) {
            lock ( Program.datalock ) {

                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Versenysorozat SET VSVESZ = VSVESZ - 1 WHERE WHERE VSAZON=@VSAZON;";
                command.Parameters.AddWithValue( "@VSAZON", azonosito );

                command.ExecuteNonQuery( );

                command.Dispose( );
                connection.Close( );
                return true;
            }
        }
    }
}
