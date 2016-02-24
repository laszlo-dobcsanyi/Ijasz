using System;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Íjász {
    public sealed partial class Database : IDisposable {

        public static int Verzió = 3;
        private SQLiteConnection connection;
        public Database( ) {
            connection = new SQLiteConnection( "Data Source=adat.db; Version=3; New=False; Compress=True;" );

            //Check if database not exists
            if( !File.Exists( "adat.db" ) ) {
                //Create database
                SQLiteConnection.CreateFile( "adat.db" );
                connection.Open( );

                //Create tables
                SQLiteCommand command = connection.CreateCommand( );

                command.CommandText =
                    "CREATE TABLE Verzió (PRVERZ int);" +
                    "CREATE TABLE Versenysorozat (VSAZON char(10) PRIMARY KEY, VSMEGN char(30), VSVESZ int);" +
                    "CREATE TABLE Verseny (VEAZON char(10) PRIMARY KEY, VEMEGN char(30), VEDATU char(20), VSAZON char(10), VEOSPO int NOT NULL, VEALSZ int, VEINSZ int, VELEZAR boolean, VEDUBE boolean);" +
                    "CREATE TABLE Korosztályok (VEAZON char(10) NOT NULL, KOAZON char(10) NOT NULL, KOMEGN char(30), KOEKMI int NOT NULL, KOEKMA int NOT NULL, KONOK boolean, KOFERF boolean, KOINSN int, KOINSF int, KOEGYB boolean);" +
                    "CREATE TABLE Íjtípusok (ITAZON char(10) PRIMARY KEY, ITMEGN char(30), ITLISO int, ITERSZ int);" +
                    "CREATE TABLE Egyesuletek (EGAZON char(30) PRIMARY KEY,EGCIME char(30),EGVENE char(30),EGVET1 char(30),EGVET2 char(30),EGVEM1 char(30),EGVEM2 char(30),EGLIST boolean,EGTASZ int);" +
                    "CREATE TABLE Indulók (INNEVE char(30) PRIMARY KEY, INNEME char(1) NOT NULL, INSZUL char(20) NOT NULL, INVEEN char(30),INERSZ int, EGAZON char(10));" +
                    "CREATE TABLE Oklevelek (" +
                                "OKAZON char(30) PRIMARY KEY, OKTIPU char(30)," +
                                "OKVENEX int, OKVENEY int, OKVENEH int, OKVENEF char(1), OKVENEB char(30), OKVENEM int, OKVENEI char(1)," +
                                "OKVSNEX int, OKVSNEY int, OKVSNEH int, OKVSNEF char(1), OKVSNEB char(30), OKVSNEM int, OKVSNEI char(1)," +
                                "OKHELYX int, OKHELYY int, OKHELYH int, OKHELYF char(1), OKHELYB char(30), OKHELYM int, OKHELYI char(1)," +
                                "OKNEVEX int, OKNEVEY int, OKNEVEH int, OKNEVEF char(1), OKNEVEB char(30), OKNEVEM int, OKNEVEI char(1)," +
                                "OKEGYEX int, OKEGYEY int, OKEGYEH int, OKEGYEF char(1), OKEGYEB char(30), OKEGYEM int, OKEGYEI char(1)," +
                                "OKIJTIX int, OKIJTIY int, OKIJTIH int, OKIJTIF char(1), OKIJTIB char(30), OKIJTIM int, OKIJTII char(1)," +
                                "OKKOROX int, OKKOROY int, OKKOROH int, OKKOROF char(1), OKKOROB char(30), OKKOROM int, OKKOROI char(1)," +
                                "OKNEMEX int, OKNEMEY int, OKNEMEH int, OKNEMEF char(1), OKNEMEB char(30), OKNEMEM int, OKNEMEI char(1)," +
                                "OKDATUX int, OKDATUY int, OKDATUH int, OKDATUF char(1), OKDATUB char(30), OKDATUM int, OKDATUI char(1));" +
                    "INSERT INTO Verzió (PRVERZ) VALUES (" + Verzió + ");";

                if( command.ExecuteNonQuery( ) != 0 ) { }// MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show( "Adatbázis létrehozva!", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information );
                command.Dispose( );
                connection.Close( );

                // Biztonsági mentések mappája

                if( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );
            }
            else {
                CreateBackup( "adat_indítás_" + DateTime.Now.ToString( ).Trim( new Char[ ] { '-' } ).Replace( ' ', '_' ).Replace( '.', '-' ).Replace( ':', '-' ) );
            }

        }

        public void CreateBackup( string _name ) {
            if( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );

            lock ( Program.datalock ) {
                //SQLiteConnection cnnOut = new SQLiteConnection("Data Source=backup\\" + _name + ".db;foreign keys=True");
                SQLiteConnection cnnOut = new SQLiteConnection( "Data Source=backup.db;foreign keys=True" );
                connection.Open( );
                cnnOut.Open( );
                connection.BackupDatabase( cnnOut, "main", "main", -1, null, -1 );
                connection.Close( );
                cnnOut.Close( );
            }
        }

        public static bool IsCorrectSQLText( string _text ) {
            if( _text.Contains( "'" ) || _text.Contains( "\"" ) || _text.Contains( "(" ) || _text.Contains( ")" ) || _text.Contains( ";" ) ) return false;
            return true;
        }

        public bool IsCorrectVersion( ) {
            int version = 0;
            SQLiteCommand command;

            connection.Open( );

            command = connection.CreateCommand( );
            command.CommandText = "SELECT PRVERZ FROM Verzió;";
            try {
                SQLiteDataReader reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
                    version = reader.GetInt32( 0 );
                }
            }
            catch( SQLiteException ) { version = 1; }
            catch( Exception ) { return false; }
            finally {
                command.Dispose( );
                connection.Close( );
            }

            if( Verzió == version ) return true;
            return false;
        }

        #region Nyomtat

        #region CsapatLista

        public List<Nyomtat.CSAPATLISTA.CSAPAT>
        CsapatLista( string _VEAZON ) {
            lock ( Program.datalock ) {
                List<Nyomtat.CSAPATLISTA.CSAPAT> Data = new List<Nyomtat.CSAPATLISTA.CSAPAT>( );
                connection.Open( );

                SQLiteCommand command = connection.CreateCommand( );

                command.CommandText = "select distinct Eredmények_" + _VEAZON + ".INCSSZ, count(Eredmények_" + _VEAZON + " .INCSSZ) " +
                                      "from Eredmények_" + _VEAZON + " group by INCSSZ order by INCSSZ;";
                SQLiteDataReader reader = command.ExecuteReader( );
                while( reader.Read( ) ) {
                    if( reader.GetInt32( 1 ) > 0 ) {
                        Data.Add( new Nyomtat.CSAPATLISTA.CSAPAT( reader.GetInt32( 0 ) ) );
                    }
                }

                command.Dispose( );
                connection.Close( );

                return Data;
            }
        }

        #endregion

        #region NevezesiLista

        #endregion

        #endregion

        public void Dispose( ) {
            connection.Close( );
        }
    }
}
