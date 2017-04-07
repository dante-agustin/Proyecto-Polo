using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDrinkProyectoCompartido;
using SQLite.Net;
using Windows.Foundation.Diagnostics;

namespace AppDrinkUWP.DataHelper
{
    class DataBase
    {
        string sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Drinks.db");
        public  bool CreateDatabase()
            {
                
                try
                {
                    using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
                    {
                        conn.CreateTable<Drink>();
                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                

                    return false;
                }
            
            }

        public bool insertIntoTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(),sqlpath))
                {
                    connection.Insert(trago);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public List<Drink> selectTableDrink()
        {
            try
            {
                using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
                {
                    return connection.Table<Drink>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
                {
                    connection.Query<Drink>("UPDATE Drink set nombre=?, ingredientes=?, categoria=?, imagePath=?, precio=? Where Id=?", trago.nombre, trago.ingredientes, trago.categoria, trago.imagePath, trago.precio, trago.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public bool deleteTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
                {
                    connection.Delete(trago);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableDrink(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath))
                {
                    connection.Query<Drink>("SELECT * FROM Drink Where Id=?", id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

    }
}
