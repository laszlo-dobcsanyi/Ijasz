using System;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Íjász
{
   public sealed partial class Database : IDisposable
   {

      public static int Verzió = 3;
      private SQLiteConnection connection;

      public Database( )
      {
         connection = new SQLiteConnection( "Data Source=adat.db; Version=3; New=False; Compress=True;" );

         //Check if database not exists
         if ( !File.Exists( "adat.db" ) )
         {
            //Create database
            SQLiteConnection.CreateFile( "adat.db" );
            connection.Open( );

            //Create tables
            SQLiteCommand command = connection.CreateCommand();

            command.CommandText =
                "CREATE TABLE Verzió (PRVERZ int);" +
                "CREATE TABLE Versenysorozat (VSAZON char(10) PRIMARY KEY, VSMEGN char(30), VSVESZ int);" +
                "CREATE TABLE Verseny (VEAZON char(10) PRIMARY KEY, VEMEGN char(30), VEDATU char(20), VSAZON char(10), VEOSPO int NOT NULL, VEALSZ int, VEINSZ int, VELEZAR boolean, VEDUBE boolean);" +
                "CREATE TABLE Korosztályok (VEAZON char(10) NOT NULL, KOAZON char(10) NOT NULL, KOMEGN char(30), KOEKMI int NOT NULL, KOEKMA int NOT NULL, KONOK boolean, KOFERF boolean, KOINSN int, KOINSF int, KOEGYB boolean);" +
                "CREATE TABLE Íjtípusok (ITAZON char(10) PRIMARY KEY, ITMEGN char(30), ITLISO int, ITERSZ int);" +
                "CREATE TABLE Egyesuletek (EGAZON char(30) PRIMARY KEY,EGCIME char(30),EGVENE char(30),EGVET1 char(30),EGVET2 char(30),EGVEM1 char(30),EGVEM2 char(30),EGLIST boolean,EGTASZ int);" +
                "CREATE TABLE Indulók (INNEVE char(30) PRIMARY KEY, INNEME char(1) NOT NULL, INSZUL char(20) NOT NULL, INVEEN char(30),INERSZ int, EGAZON char(10));" +
                "CREATE TABLE Oklevelek (OKAZON char(10) PRIMARY KEY, OKTIP char(30), OKNEVEX int, OKNEVEY int, OKHELYX int, OKHELYY int, OKKATEX int, OKKATEY int, OKHESZX int,OKHESZY int,  OKDATUX int, OKDATUY int, OKEGYEX int,OKEGYEY int  );" +
                "INSERT INTO Verzió (PRVERZ) VALUES (" + Verzió + ");";

            if ( command.ExecuteNonQuery( ) != 0 ) { }// MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else MessageBox.Show( "Adatbázis létrehozva!", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information );
            command.Dispose( );
            connection.Close( );

            // Biztonsági mentések mappája

            if ( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );
         }
         else
         {
            CreateBackup( "adat_indítás_" + DateTime.Now.ToString( ).Trim( new Char[ ] { '-' } ).Replace( ' ', '_' ).Replace( '.', '-' ).Replace( ':', '-' ) );
         }

      }

      public void CreateBackup( string _name )
      {
         if ( !Directory.Exists( @"backup" ) ) Directory.CreateDirectory( @"backup" );

         lock ( Program.datalock )
         {
            //SQLiteConnection cnnOut = new SQLiteConnection("Data Source=backup\\" + _name + ".db;foreign keys=True");
            SQLiteConnection cnnOut = new SQLiteConnection("Data Source=backup.db;foreign keys=True");
            connection.Open( );
            cnnOut.Open( );
            connection.BackupDatabase( cnnOut, "main", "main", -1, null, -1 );
            connection.Close( );
            cnnOut.Close( );
         }
      }

      public static bool IsCorrectSQLText( string _text )
      {
         if ( _text.Contains( "'" ) || _text.Contains( "\"" ) || _text.Contains( "(" ) || _text.Contains( ")" ) || _text.Contains( ";" ) ) return false;
         return true;
      }

      public bool IsCorrectVersion( )
      {
         int version = 0;
         SQLiteCommand command;

         connection.Open( );

         command = connection.CreateCommand( );
         command.CommandText = "SELECT PRVERZ FROM Verzió;";
         try
         {
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               version = reader.GetInt32( 0 );
            }
         }
         catch ( SQLiteException ) { version = 1; }
         catch ( Exception ) { return false; }
         finally
         {
            command.Dispose( );
            connection.Close( );
         }

         if ( Verzió == version ) return true;
         return false;
      }

      #region Nyomtat

      #region CsapatLista

      public List<Nyomtat.CSAPATLISTA.CSAPAT>
      CsapatLista( string _VEAZON )
      {
         lock ( Program.datalock )
         {
            List<Nyomtat.CSAPATLISTA.CSAPAT> Data = new List<Nyomtat.CSAPATLISTA.CSAPAT>( );
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "select distinct Eredmények_" + _VEAZON + ".INCSSZ, count(Eredmények_" + _VEAZON + " .INCSSZ) " +
                                  "from Eredmények_" + _VEAZON + " group by INCSSZ order by INCSSZ;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               if ( reader.GetInt32( 1 ) > 0 )
               {
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

      public void Dispose( )
      {
         connection.Close( );
      }
   }
}
