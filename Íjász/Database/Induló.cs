using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Íjász {
    partial class Database {
        public List<Induló> Indulók( ) {
            lock ( Program.datalock ) {
                List<Induló> data = new List<Induló>();
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ FROM Indulók;";
                SQLiteDataReader reader = command.ExecuteReader();
                while( reader.Read( ) ) {
                    data.Add( new Induló( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetString( 4 ), reader.GetInt32( 5 ) ) );
                }

                command.Dispose( );
                connection.Close( );

                return data;
            }
        }

        public Nullable<Induló> Induló( string _név ) {
            lock ( Program.datalock ) {
                Nullable<Induló> induló = null;
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ FROM Indulók WHERE INNEVE = '" + _név + "';";
                SQLiteDataReader reader = command.ExecuteReader();
                while( reader.Read( ) ) {
                    induló = new Induló( reader.GetString( 0 ), reader.GetString( 1 ), reader.GetString( 2 ), reader.GetString( 3 ), reader.GetString( 4 ), reader.GetInt32( 5 ) );
                }

                command.Dispose( );
                connection.Close( );

                return induló;
            }
        }

        public bool ÚjInduló( Induló indulo ) {
            lock ( Program.datalock ) {
                if( Induló_EngedélyLétezik( "", indulo.Engedely ) ) { return false; }

                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Indulók (INNEVE, INNEME, INSZUL, INVEEN, EGAZON, INERSZ) VALUES(@INNEVE, @INNEME, @INSZUL, @INVEEN, @EGAZON, @INERSZ);";

                command.Parameters.Add( new SQLiteParameter( "@INNEVE", indulo.Nev ) );
                command.Parameters.Add( new SQLiteParameter( "@INNEME", indulo.Nem ) );
                command.Parameters.Add( new SQLiteParameter( "@INSZUL", indulo.SzuletesiDatum ) );
                command.Parameters.Add( new SQLiteParameter( "@INVEEN", indulo.Engedely ) );
                command.Parameters.Add( new SQLiteParameter( "@EGAZON", indulo.Egyesulet ) );
                command.Parameters.Add( new SQLiteParameter( "@INERSZ", "0" ) );

                try {
                    command.ExecuteNonQuery( );
                }
                catch( SQLiteException exception ) {
                    new ErrorMessage( ErrorCode.INDULO_CREATE, exception.Message, indulo );
                    return false;
                }
                finally {
                    command.Dispose( );
                    connection.Close( );
                }
                return true;
            }
        }

        public bool IndulóMódosítás( string nev, Induló indulo ) {
            lock ( Program.datalock ) {
                if( Induló_EngedélyLétezik( nev, indulo.Engedely ) ) { return false; }

                connection.Open( );

                SQLiteCommand command = connection.CreateCommand();
               // command.CommandText = "UPDATE Indulók SET INNEVE = '" + _induló.Nev + "', INNEME = '" + _induló.Nem + "', INSZUL = '" + _induló.SzuletesiDatum + "', " +
                //    "INVEEN = '" + _induló.Engedely + "', EGAZON = '" + _induló.Egyesulet + "' WHERE INNEVE = '" + _név + "';";

                command.CommandText = "UPDATE Indulók SET INNEVE = @INNEVE_NEV, INNEME = @INNEME, INSZUL = @INSZUL, INVEEN = @INVEEN, EGAZON = @EGAZON WHERE INNEVE = @INNEVE_OLD";
                command.Parameters.Add( new SQLiteParameter( "@INNEVE_NEV",indulo.Nev) );
                command.Parameters.Add( new SQLiteParameter( "@INNEME", indulo.Nem) );
                command.Parameters.Add( new SQLiteParameter( "@INSZUL", indulo.SzuletesiDatum) );
                command.Parameters.Add( new SQLiteParameter( "@INVEEN",indulo.Engedely) );
                command.Parameters.Add( new SQLiteParameter( "@EGAZON",indulo.Egyesulet) );
                command.Parameters.Add( new SQLiteParameter( "@INNEVE_OLD",nev) );

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
                return true;
            }
        }

        public bool Induló_EredményekÁtnevezése( string _eredeti_név, string _új_név ) {
            lock ( Program.datalock ) {
                List<Verseny> versenyek = Versenyek();

                connection.Open( );

                foreach( Verseny _verseny in versenyek ) {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Eredmények_" + _verseny.Azonosito + " SET INNEVE = '" + _új_név + "' WHERE INNEVE = '" + _eredeti_név + "';";
                    command.ExecuteNonQuery( );
                    command.Dispose( );
                }

                connection.Close( );

                return true;
            }
        }

        public bool IndulóTörlés( string _név ) {
            lock ( Program.datalock ) {
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
        public bool Induló_EngedélyLétezik( string _név, string _engedély ) {
            if( _engedély.Length == 0 ) { return false; }
            bool exists = false;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT INNEVE FROM Indulók WHERE INVEEN = '" + _engedély + "' AND INNEVE <> '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while( reader.Read( ) ) {
                exists = true;
            }

            command.Dispose( );
            connection.Close( );

            if( exists == true ) {
                return true;
            }
            return false;
        }

    }
}
