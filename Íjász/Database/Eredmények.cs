using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Íjász
{
   partial class Database
   {

      public List<Eredmény> Eredmények( string _azonosító )
      {
         lock ( Program.datalock )
         {
            List<Eredmény> data = new List<Eredmény>( );

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _azonosító + ";";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               data.Add( new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) ) );
            }
            command.Dispose( );
            connection.Close( );
            return data;
         }
      }

      public Nullable<Eredmény> Eredmény( string _verseny, string _név )
      {
         lock ( Program.datalock )
         {
            Nullable<Eredmény> Data = null;

            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               Data = new Eredmény( reader.GetString( 0 ),
                                        reader.GetInt64( 1 ),
                                        reader.GetString( 2 ),
                                        reader.GetInt32( 3 ),
                                        reader.GetInt32( 4 ),
                                        reader.GetInt32( 5 ),
                                        reader.GetInt32( 6 ),
                                        reader.GetInt32( 7 ),
                                        reader.GetInt32( 8 ),
                                        reader.GetInt32( 9 ),
                                        reader.GetBoolean( 10 ),
                                        reader.GetBoolean( 11 ),
                                        reader.GetString( 12 ) );
            }

            command.Dispose( );
            connection.Close( );

            return Data;
         }
      }

      public class BeírásEredmény
      {
         public enum Flag
         {
            NINCS,

            HOZZÁADOTT,
            MÓDOSÍTOTT
         }

         public Flag flag;
         public Eredmény? eredmény;
         public Eredmény? eredeti;

         public BeírásEredmény( Eredmény? _eredmény, Eredmény? _eredeti, Flag _flag )
         {
            flag = _flag;
            eredeti = _eredeti;
            eredmény = _eredmény;
         }

         public BeírásEredmény( )
         {
            flag = Flag.NINCS;
            eredeti = null;
            eredmény = null;
         }
      }

      //ha a korosztálymódodított = true akkor be kell írni a korosztályt is, különben nem
      //ELLENŐRIZVE JÓL MŰKÖDIK
      public BeírásEredmény EredményBeírás( string _név,
                                          string _verseny,
                                          string _íjtípus,
                                          int _csapat,
                                          bool _megjelent,
                                          bool _korosztalymodositott,
                                          string _korosztalyazonosito )
      {
         lock ( Program.datalock )
         {
            connection.Open( );
            SQLiteCommand command;

            // Név meglétének ellenőrzése a versenyen
            Eredmény? eredeti = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               eredeti = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            //eredmény módosítás
            if ( eredeti != null )
            {
               command = connection.CreateCommand( );
               //módoítom a korosztályt
               command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + ( _megjelent ? "1" : "0" ) + ", INKOMO = '" + ( _korosztalymodositott ? "1" : "0" ) + "', KOAZON = '" + _korosztalyazonosito + "' WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }
            //új eredmény beírás -> nincs korosztályazonosító,csak ha korosztálymódosított = true
            else
            {
               command = connection.CreateCommand( );
               command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON) " +
                  "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + ( _megjelent ? "1" : "0" ) + ( _korosztalymodositott ? ",1" : ",0" ) + ",'" + _korosztalyazonosito + "');";
               command.ExecuteNonQuery( );
               command.Dispose( );

               // Induló eredmények számának növelése
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }

            Eredmény? eredmény = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ, INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredmény = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            connection.Close( );
            return new BeírásEredmény( eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT );
         }
      }

      public BeírásEredmény EredményBeírás_Ellenőrzött( string _név, string _verseny, string _íjtípus, int _csapat, bool _megjelent, bool _korosztalymodositott )
      {
         lock ( Program.datalock )
         {
            bool found;

            connection.Open( );
            SQLiteCommand command;

            // Verseny meglétének és lezárásának ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) ) { if ( _verseny == reader.GetString( 0 ) && reader.GetBoolean( 1 ) == false ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); };

            // Induló meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE FROM Indulók;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _név == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); ; };

            // Íjtípus meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _íjtípus == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return new BeírásEredmény( ); };

            // Név meglétének ellenőrzése a versenyen
            Eredmény? eredeti = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredeti = new Eredmény( reader.GetString( 0 ),
                   reader.GetInt64( 1 ),
                   reader.GetString( 2 ),
                   reader.GetInt32( 3 ),
                   reader.GetInt32( 4 ),
                   reader.GetInt32( 5 ),
                   reader.GetInt32( 6 ),
                   reader.GetInt32( 7 ),
                   reader.GetInt32( 8 ),
                   reader.GetInt32( 9 ),
                   reader.GetBoolean( 10 ),
                   reader.GetBoolean( 11 ),
                   reader.GetString( 12 ) );
            }
            command.Dispose( );

            if ( eredeti != null )
            {
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Eredmények_" + _verseny + " SET ITAZON = '" + _íjtípus + "', INCSSZ = " + _csapat + ", INMEGJ = " + ( _megjelent ? "1" : "0" ) + ", INKOMO = " + ( _korosztalymodositott ? "1" : "0" ) + " WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }
            else
            {
               command = connection.CreateCommand( );
               command.CommandText = "INSERT INTO Eredmények_" + _verseny + " (INNEVE, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ,INKOMO)" +
                  "VALUES('" + _név + "', '" + _íjtípus + "', '" + _csapat + "', 0, 0, 0, 0, 0, 0, " + ( _megjelent ? "1" : "0" ) + ( _korosztalymodositott ? "1" : "0" ) + ");";
               command.ExecuteNonQuery( );
               command.Dispose( );

               // Induló eredmények számának növelése
               command = connection.CreateCommand( );
               command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ + 1 WHERE INNEVE = '" + _név + "';";
               command.ExecuteNonQuery( );
               command.Dispose( );
            }

            Eredmény? eredmény = null;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE, INSOSZ, ITAZON, INCSSZ, IN10TA, IN08TA, IN05TA, INMETA, INOSZP, INERSZ, INMEGJ,INKOMO, KOAZON FROM Eredmények_" + _verseny + " WHERE INNEVE = '" + _név + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               eredmény = new Eredmény( reader.GetString( 0 ),
                                       reader.GetInt64( 1 ),
                                       reader.GetString( 2 ),
                                       reader.GetInt32( 3 ),
                                       reader.GetInt32( 4 ),
                                       reader.GetInt32( 5 ),
                                       reader.GetInt32( 6 ),
                                       reader.GetInt32( 7 ),
                                       reader.GetInt32( 8 ),
                                       reader.GetInt32( 9 ),
                                       reader.GetBoolean( 10 ),
                                       reader.GetBoolean( 11 ),
                                       reader.GetString( 12 ) );
            }
            command.Dispose( );

            connection.Close( );
            return new BeírásEredmény( eredmény, eredeti, eredeti != null ? BeírásEredmény.Flag.MÓDOSÍTOTT : BeírásEredmény.Flag.HOZZÁADOTT );
         }
      }

      public Int64 EredményMódosítás( string _azonosító, Eredmény _eredeti, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            connection.Open( );
            SQLiteCommand command;
            if ( _eredeti.Nev != _eredmény.Nev )
            {
               command = connection.CreateCommand( );
               command.CommandText = "SELECT INNEVE FROM Eredmények_" + _azonosító + ";";

               bool found = false;
               SQLiteDataReader reader = command.ExecuteReader();
               while ( reader.Read( ) )
               {
                  if ( _eredmény.Nev == reader.GetString( 0 ) ) found = true;
               }

               if ( found )
               {
                  command.Dispose( );
                  connection.Close( );
                  return -666;
               }
            }

            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET INNEVE = '" + _eredmény.Nev + "', ITAZON = '" + _eredmény.Ijtipus + "', INCSSZ = '" + _eredmény.Csapat + "', " +
                "IN10TA = " + _eredmény.Talalat10 + ", IN08TA = " + _eredmény.Talalat8 + ", IN05TA = " + _eredmény.Talalat5 + ", INMETA = " + _eredmény.Melle + ", " +
                "INOSZP = " + _eredmény.Osszpont + ", INERSZ = " + _eredmény.Szazalek + ", INMEGJ = " + ( _eredmény.Megjelent ? "1" : "0" ) + ", INKOMO = " + ( _eredmény.KorosztalyModositott ? "'1'" : "'0'" ) + ", KOAZON= '" + _eredmény.KorosztalyAzonosito + "' WHERE INNEVE = '" + _eredeti.Nev + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );
            return 1;
         }
      }

      public Nullable<Eredmény> EredményMódosítás_Ellenőrzött( string _azonosító, Eredmény _eredeti, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            bool found;

            connection.Open( );
            SQLiteCommand command;

            // Verseny meglétének és lezárásának ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEAZON, VELEZAR FROM Verseny;";
            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _azonosító == reader.GetString( 0 ) && reader.GetBoolean( 1 ) == false ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };

            // Induló meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INNEVE FROM Indulók;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _eredmény.Nev == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };

            // Íjtípus meglétének ellenőrzése
            found = false;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT ITAZON FROM Íjtípusok;";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) ) { if ( _eredmény.Ijtipus == reader.GetString( 0 ) ) found = true; }
            command.Dispose( );

            if ( !found ) { connection.Close( ); return null; };

            // Százalék számítás összespont lekérdezése után
            int összespont = -666;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT VEOSPO FROM Verseny WHERE VEAZON = '" + _azonosító + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               összespont = reader.GetInt32( 0 );
            }
            command.Dispose( );

            if ( !( ( _eredmény.Talalat10 == 0 && _eredmény.Talalat8 == 0 && _eredmény.Talalat5 == 0 && _eredmény.Melle == 0 )
                || ( összespont == _eredmény.Talalat10 + _eredmény.Talalat8 + _eredmény.Talalat5 + _eredmény.Melle ) ) )
            {
               connection.Close( ); return null;
            }

            int összes = _eredmény.Talalat10 * 10 + _eredmény.Talalat8 * 8 + _eredmény.Talalat5 * 5;
            int százalék = (int)( ( (double)összes / ( összespont * 10 ) ) * 100 );

            // Adatbázis módosítás
            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Eredmények_" + _azonosító + " SET ITAZON = '" + _eredmény.Ijtipus + "', INCSSZ = '" + _eredmény.Csapat + "', " +
                "IN10TA = " + _eredmény.Talalat10 + ", IN08TA = " + _eredmény.Talalat8 + ", IN05TA = " + _eredmény.Talalat5 + ", INMETA = " + _eredmény.Melle + ", " +
                "INOSZP = " + összes + ", INERSZ = " + százalék + ", INMEGJ = " + ( _eredmény.Megjelent ? "1" : "0" ) + ", INKOMO = " + ( _eredmény.KorosztalyModositott ? "1" : "0" ) + " SET KOAZON = '" + _eredmény.KorosztalyAzonosito + "' WHERE INNEVE = '" + _eredeti.Nev + "';";
            command.ExecuteNonQuery( );

            // Sorszám, megjelent lekérdezése
            Int64 sorszám = -666;
            command = connection.CreateCommand( );
            command.CommandText = "SELECT INSOSZ FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.Nev + "';";
            reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               sorszám = reader.GetInt64( 0 );
            }
            command.Dispose( );

            command.Dispose( );
            connection.Close( );
            return new Eredmény( _eredmény.Nev,
                                sorszám,
                                _eredmény.Ijtipus,
                                _eredmény.Csapat,
                                _eredmény.Talalat10,
                                _eredmény.Talalat8,
                                _eredmény.Talalat5,
                                _eredmény.Melle,
                                összes,
                                százalék,
                                _eredmény.Megjelent,
                                _eredmény.KorosztalyModositott,
                                _eredmény.KorosztalyAzonosito );
         }
      }

      public bool EredményTörlés( string _azonosító, Eredmény _eredmény )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Eredmények_" + _azonosító + " WHERE INNEVE = '" + _eredmény.Nev + "';";
            command.ExecuteNonQuery( );

            // Induló eredmények számának csökkentése
            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Indulók SET INERSZ = INERSZ - 1 WHERE INNEVE = '" + _eredmény.Nev + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

   }
}
