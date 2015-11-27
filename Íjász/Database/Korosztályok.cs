using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Íjász
{
   partial class Database
   {
      
      public List<Korosztály> Korosztályok( string _verseny )
      {
         lock ( Program.datalock )
         {
            List<Korosztály> data = new List<Korosztály>();
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSN, KOINSF, KOEGYB FROM Korosztályok WHERE VEAZON = '" + _verseny + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            while ( reader.Read( ) )
            {
               data.Add( new Korosztály( reader.GetString( 0 ),
                   reader.GetString( 1 ),
                   reader.GetString( 2 ),
                   reader.GetInt32( 3 ),
                   reader.GetInt32( 4 ),
                   reader.GetBoolean( 5 ),
                   reader.GetBoolean( 6 ),
                   reader.GetInt32( 7 ),
                   reader.GetInt32( 8 ),
                   reader.GetBoolean( 9 ) ) );
            }

            command.Dispose( );
            connection.Close( );

            return data;
         }
      }

      public bool ÚjKorosztály( Korosztály _korosztály )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.Verseny + "';";
            SQLiteDataReader reader = command.ExecuteReader();
            bool found = false;
            while ( reader.Read( ) )
            {
               if ( _korosztály.Azonosito == reader.GetString( 0 ) )
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
            command.CommandText = "INSERT INTO Korosztályok (VEAZON, KOAZON, KOMEGN, KOEKMI, KOEKMA, KONOK, KOFERF, KOINSF, KOINSN, KOEGYB) VALUES('" + _korosztály.Verseny + "', '" + _korosztály.Azonosito + "', '" + _korosztály.Megnevezes + "', " +
                +_korosztály.AlsoHatar + ", " + _korosztály.FelsoHatar + ", " + ( _korosztály.Nokre ? "1" : "0" ) + ", " + ( _korosztály.Ferfiakra ? "1" : "0" ) + ", " + _korosztály.InduloFerfiak + ", " + _korosztály.InduloFerfiak + ( _korosztály.Egyben ? " ,1" : " ,0" ) + ");";

            command.ExecuteNonQuery( );
            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public bool KorosztályMódosítás( string _azonosító, Korosztály _korosztály )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();

            if ( _azonosító != _korosztály.Azonosito )
            {
               command.CommandText = "SELECT KOAZON FROM Korosztályok WHERE VEAZON = '" + _korosztály.Verseny + "';";
               SQLiteDataReader reader = command.ExecuteReader();
               bool found = false;
               while ( reader.Read( ) )
               {
                  if ( _korosztály.Azonosito == reader.GetString( 0 ) )
                  {
                     found = true;
                  }
               }

               command.Dispose( );

               if ( found )
               {
                  connection.Close( );
                  return false;
               }
            }

            command = connection.CreateCommand( );
            command.CommandText = "UPDATE Korosztályok SET KOAZON = '" + _korosztály.Azonosito + "', KOMEGN = '" + _korosztály.Megnevezes + "', " +
                "KOEKMI = " + _korosztály.AlsoHatar + ", KOEKMA = " + _korosztály.FelsoHatar + ", KONOK = " + ( _korosztály.Nokre ? "1" : "0" ) + ", KOFERF = " + ( _korosztály.Ferfiakra ? "1" : "0" ) +
                ", KOINSF = " + _korosztály.InduloFerfiak + ", KOINSN = " + _korosztály.InduloNok + ", KOEGYB = " + ( _korosztály.Egyben ? "1" : "0" ) + " WHERE KOAZON = '" + _azonosító + "' AND VEAZON = '" + _korosztály.Verseny + "';";
            try
            {
               command.ExecuteNonQuery( );
            }
            catch ( System.Data.SQLite.SQLiteException )
            {
               return false;
            }
            finally
            {
               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public bool KorosztályTörlés( string _verseny, string _azonosító )
      {
         lock ( Program.datalock )
         {
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Korosztályok WHERE VEAZON = '" + _verseny + "' AND KOAZON = '" + _azonosító + "';";
            command.ExecuteNonQuery( );

            command.Dispose( );
            connection.Close( );

            return true;
         }
      }

      public struct CountPair
      {
         public int férfiak;
         public int nők;
      };

      public bool
      KorosztálySzámolás( string _verseny )
      {
         List<Korosztály> korosztályok = Korosztályok(_verseny);

         lock ( Program.datalock )
         {
            foreach ( Korosztály current in korosztályok )
            {
               CountPair indulók = KorosztálySzámolás(_verseny,
                                                            current.AlsoHatar,
                                                            current.FelsoHatar,
                                                            current.Nokre,
                                                            current.Ferfiakra,
                                                            true);

               connection.Open( );
               SQLiteCommand command = connection.CreateCommand();
               command.CommandText = "UPDATE Korosztályok SET KOINSF = " + indulók.férfiak + ", KOINSN = " + indulók.nők + " WHERE KOAZON = '" + current.Azonosito + "' AND VEAZON = '" + current.Verseny + "';";
               command.ExecuteNonQuery( );

               command.Dispose( );
               connection.Close( );
            }

            return true;
         }
      }

      public CountPair
      KorosztálySzámolás( string _azonosító, int _alsó, int _felső, bool _nők, bool _férfiak, bool _internal )
      {
         if ( !_internal )
            lock ( Program.datalock )
            {
               return KorosztálySzámolás_Segéd( _azonosító, _alsó, _felső, _nők, _férfiak );
            }
         else return KorosztálySzámolás_Segéd( _azonosító, _alsó, _felső, _nők, _férfiak );
      }

      /// <summary>
      /// Nincs lockolva, csak belső használatra!
      /// </summary>
      private CountPair
      KorosztálySzámolás_Segéd( string _azonosító, int _alsó, int _felső, bool _nők, bool _férfiak )
      {
         CountPair count = new CountPair();

         connection.Open( );
         SQLiteCommand command = connection.CreateCommand();
         command.CommandText = "SELECT INSZUL, INNEME, INMEGJ FROM Indulók, Eredmények_" + _azonosító + " WHERE Eredmények_" + _azonosító + ".INNEVE= Indulók.INNEVE AND Eredmények_" + _azonosító + ".INMEGJ= '" + 1 + "';";

         SQLiteDataReader reader = command.ExecuteReader();
         while ( reader.Read( ) )
         {
            int year = (new DateTime(1, 1, 1) + (DateTime.Now - DateTime.Parse(reader.GetString(0)))).Year - 1;
            //MessageBox.Show("also: " + _alsó + " felso: " + _felső + " year: " + year);
            if ( _alsó <= year && year <= _felső )
            {
               if ( reader.GetString( 1 ) == "N" )
               {
                  if ( _nők ) count.nők++;
               }
               else
               {
                  if ( _férfiak ) count.férfiak++;
               }
            }
            //MessageBox.Show(reader.GetBoolean(2).ToString());
         }
         // MessageBox.Show(count.nők.ToString() + " " + count.férfiak.ToString());
         command.Dispose( );
         connection.Close( );

         return count;
      }

      public int InduloKora( string _VEAZON, string _INNEVE )
      {
         lock ( Program.datalock )
         {
            int Value = 0;
            connection.Open( );

            SQLiteCommand command = connection.CreateCommand( );

            command.CommandText = "select min(verseny.VEDATU), Indulók.INSZUL from verseny, indulók " +
                                  "where verseny.VSAZON = (select verseny.vsazon from verseny " +
                                  "where verseny.veazon = '" + _VEAZON + "') and indulók.inneve = '" + _INNEVE + "';";

            SQLiteDataReader reader = command.ExecuteReader( );
            while ( reader.Read( ) )
            {
               string q = reader.GetString( 0 );
               string qq = reader.GetString( 1 );
               Value = ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( q ) - DateTime.Parse( qq ) ) ).Year - 1;
            }

            command.Dispose( );
            connection.Close( );
            return Value;
         }
      }

      //teszt
      //működik de lassú ,a végén a módosítást egyszerre
      /// <summary>
      /// egy eredmenylap osszes indulojanak koazon-jat módosítja + visszaadja a korosztaly indulok szamat
      /// régi korosztalyt törölni???!!!!4négy
      /// </summary>
      public bool
      KorosztalyModositas( string _VEAZON )
      {
         /*  tudom az új korosztályt
          *  tudom a versenyazonosító
          *  végig kell menni az eredménylapon , updatelni akit lehet
          *  ha kész ki kell számolni a korosztály indulókat
          * 
          */

         try
         {
            List<Korosztály> list_korosztalyok = Program.database.Korosztályok( _VEAZON );
            List<Eredmény> list_eredmenyek = Program.database.Eredmények( _VEAZON );
            List<Induló> list_indulok = Program.database.Indulók( );

            // a korosztályokat tuti módosítom -> array könnyebb
            //kinullázom
            Korosztály[] korosztalyok = list_korosztalyok.ToArray( );
            for ( int i = 0 ; i < korosztalyok.Length ; i++ )
            {
               korosztalyok[ i ].InduloNok = 0;
               korosztalyok[ i ].InduloFerfiak = 0;
            }

            //eredmenyeket tuti modosítóm -> array könnyebb
            Eredmény[] eredmenyek = list_eredmenyek.ToArray( );

            //kell a verseny az időpont miatt
            Verseny? verseny = Program.database.Verseny( _VEAZON );

            Induló[] indulok = list_indulok.ToArray( );

            int countNemModosithato = 0;
            int countModositott = 0;
            int countMegjelentek = 0;
            foreach ( Eredmény item in list_eredmenyek ) { if ( item.Megjelent == true ) { countMegjelentek++; } }

            for ( int k = 0 ; k < indulok.Length ; k++ )
            {
               //a versenykor ennyi éves volt
               int year = Program.database.InduloKora( verseny.Value.Azonosito, indulok[k].Nev );// ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( verseny.Value.Datum ) - DateTime.Parse( indulok[k].születés ) ) ).Year - 1;
               for ( int j = 0 ; j < eredmenyek.Length ; j++ )
               {
                  if ( indulok[ k ].Nev == eredmenyek[ j ].Nev )
                  {
                     //ha felülírt a korosztálya nem foglalkozok vele
                     if ( eredmenyek[ j ].KorosztalyModositott == false )
                     {
                        for ( int i = 0 ; i < korosztalyok.Length ; i++ )
                        {
                           //férfi, férfi korosztály
                           if ( indulok[ k ].Nem == "F" && korosztalyok[ i ].Ferfiakra == true &&
                               ( year >= korosztalyok[ i ].AlsoHatar && year <= korosztalyok[ i ].FelsoHatar ) )
                           {
                              //csak a megjelentek???????
                              if ( eredmenyek[ j ].Megjelent == true )
                              {
                                 korosztalyok[ i ].InduloFerfiak++;
                              }

                              eredmenyek[ j ].KorosztalyAzonosito = korosztalyok[ i ].Azonosito;
                              countModositott++;
                           }
                           //no, no korosztaly
                           else if ( indulok[ k ].Nem == "N" && korosztalyok[ i ].Ferfiakra == true &&
                           ( year >= korosztalyok[ i ].AlsoHatar && year <= korosztalyok[ i ].FelsoHatar ) )
                           {
                              if ( eredmenyek[ j ].Megjelent == true )
                              {
                                 korosztalyok[ i ].InduloNok++;
                              }

                              eredmenyek[ j ].KorosztalyAzonosito = korosztalyok[ i ].Azonosito;
                              countModositott++;
                           }
                        }
                     }
                     else
                     {
                        for ( int q = 0 ; q < korosztalyok.Length ; q++ )
                        {
                           if ( korosztalyok[ q ].Azonosito == eredmenyek[ j ].KorosztalyAzonosito &&
                              eredmenyek[ j ].Megjelent == true )
                           {
                              if ( indulok[ k ].Nem == "F" ) { korosztalyok[ q ].InduloFerfiak++; }
                              else if ( indulok[ k ].Nem == "N" ) { korosztalyok[ q ].InduloNok++; }
                           }
                        }
                        countNemModosithato++;
                     }
                  }
               }
            }
            if ( countModositott + countNemModosithato != verseny.Value.Indulok )
            {
               MessageBox.Show( "A korosztályok nem megfelelőek!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error );
               return false;
            }
            //ha minden ok, akkor az új eredményeket, és korosztályokat el kell menteni
            //átírni a database fgv-eket. hogy egyben 
            for ( int i = 0 ; i < korosztalyok.Length ; i++ )
            {
               Program.database.KorosztályMódosítás( korosztalyok[ i ].Azonosito, korosztalyok[ i ] );
            }

            for ( int i = 0 ; i < eredmenyek.Length ; i++ )
            {
               for ( int j = 0 ; j < list_eredmenyek.Count ; j++ )
               {
                  if ( eredmenyek[ i ].Nev == list_eredmenyek[ j ].Nev )
                  {
                     Program.database.EredményMódosítás( verseny.Value.Azonosito, list_eredmenyek[ j ], eredmenyek[ i ] );
                  }
               }
            }
         }
         catch ( Exception e )
         {

            MessageBox.Show( e.Message );
         }


         return true;
      }

      //NOTE(mate): ez váltja ki az InduloKora számolást
      public struct KOROSZTALYSEGED
      {
         public List<string> Nevek;
         public string Datum;

         public KOROSZTALYSEGED( List<string> _Nevek, string _Datum )
         {
            Nevek = _Nevek;
            Datum = _Datum;
         }
      }

      public int LINQ_InduloKora( List<KOROSZTALYSEGED> KorosztalySeged, Induló _indulo )
      {
         int Value = -666;

         foreach ( var indulo in KorosztalySeged )
         {
            foreach ( var nev in indulo.Nevek )
            {
               if ( _indulo.Nev == nev )
               {
                  Value = ( new DateTime( 1, 1, 1 ) + ( Convert.ToDateTime( indulo.Datum ) - DateTime.Parse( _indulo.SzuletesiDatum ) ) ).Year - 1;
                  return Value;
               }
            }
         }
         return Value;
      }

      public void LINQ_KorosztalyModositas( string _VEAZON )
      {
         var Korosztalyok_ = Program.database.Korosztályok(_VEAZON);
         var Eredmenyek = Program.database.Eredmények(_VEAZON);
         var Indulok_ = Program.database.Indulók();
         var verseny = Program.database.Verseny(_VEAZON);
         var versenyek = Program.database.Versenyek();
         var KorosztalySeged = new List<KOROSZTALYSEGED>();

         //NOTE(mate): minden versenydatumhoz kiszedem az indulok nevet az eredmenyekbol
         foreach ( var item in versenyek )
         {
            var nevek = Program.database.Eredmények(item.Azonosito).Select( q=>q.Nev );
            KorosztalySeged.Add( new KOROSZTALYSEGED( nevek.ToList( ), item.Datum ) );
         }

         //NOTE(mate): lerendezem datum szerint, és minden névhez csak a legkorábbi dátumot hagyom benne 
         KorosztalySeged.OrderBy( q => q.Datum );
         KorosztalySeged.GroupBy( q => q.Nevek ).Select( grp => grp.First( ) );

         //NOTE(mate): összerakom az indulókat

         var Indulok = from indulo in Indulok_
                       from korosztaly in Korosztalyok_
                       join eredmeny in Eredmenyek on indulo.Nev equals eredmeny.Nev
                       let Kor = LINQ_InduloKora(KorosztalySeged,indulo)
                       where ( Kor >= korosztaly.AlsoHatar && Kor <= korosztaly.FelsoHatar &&
                           ( ( indulo.Nem == "F" && korosztaly.Ferfiakra == true ) || ( indulo.Nem == "N" && korosztaly.Nokre == true ) ) )
                       select new
                       {
                          indulo.Nev,
                          indulo.Nem,
                          eredmeny.KorosztalyModositott,
                          eredmeny.KorosztalyAzonosito,
                          eredmeny.Megjelent,
                          KOAZON = eredmeny.KorosztalyModositott == true ? eredmeny.KorosztalyAzonosito : korosztaly.Azonosito
                       };

         //NOTE(mate): kiszámolom az induloférfiakat/indulonoket
         var Korosztalyok = Korosztalyok_.ToArray();
         string commandText = null;

         for ( int i = 0 ; i < Korosztalyok.Count( ) ; i++ )
         {
            Korosztalyok[ i ].InduloFerfiak = Indulok.Count( indulo => indulo.Nem.Equals( "F" ) && indulo.KOAZON.Equals( Korosztalyok[ i ].Azonosito ) && indulo.Megjelent.Equals( true ) );
            Korosztalyok[ i ].InduloNok = Indulok.Count( indulo => indulo.Nem.Equals( "N" ) && indulo.KOAZON.Equals( Korosztalyok[ i ].Azonosito ) && indulo.Megjelent.Equals( true ) );
            commandText += "UPDATE Korosztályok SET KOINSF = " + Korosztalyok[ i ].InduloFerfiak + ", KOINSN = " + Korosztalyok[ i ].InduloNok + " WHERE KOAZON = '" + Korosztalyok[ i ].Azonosito + "' AND VEAZON = '" + _VEAZON + "';";

         }

         //NOTE(mate): frissítem az eredményeket
         foreach ( var item in Indulok )
         {
            commandText += "UPDATE Eredmények_" + _VEAZON + " SET KOAZON= '" + item.KOAZON + "' WHERE INNEVE = '" + item.Nev + "';";
         }

         lock ( Program.datalock )
         {
            connection.Open( );
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            try { command.ExecuteNonQuery( ); }
            catch ( System.Data.SQLite.SQLiteException ) { return; }
            finally { command.Dispose( ); connection.Close( ); }
         }
      }


   }
}
