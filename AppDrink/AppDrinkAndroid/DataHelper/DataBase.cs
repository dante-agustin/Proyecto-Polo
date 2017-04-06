using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Android.Util;
using AppDrinkProyectoCompartido;
using System.IO;

namespace AppDrinkAndroid.DataHelper
{
    public class DataBase
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Drinks.db");

        public bool createDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.CreateTable<Drink>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public bool insertIntoTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Insert(trago);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public List<Drink> selectTableDrink()
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    return connection.Table<Drink>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Query<Drink>("UPDATE Drink set nombre=?, ingredientes=?, categoria=?, imagePath=?, precio=? Where Id=?", trago.nombre, trago.ingredientes, trago.categoria, trago.imagePath, trago.precio, trago.id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public bool deleteTableDrink(Drink trago)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Delete(trago);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableDrink(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Query<Drink>("SELECT * FROM Drink Where Id=?", id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx ", ex.Message);
                return false;
            }
        }

    }
}