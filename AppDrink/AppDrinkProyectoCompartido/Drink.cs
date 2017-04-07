
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
//using SQLite.Net.Attributes;

namespace AppDrinkProyectoCompartido
{
    public class Drink
    {

        #if __ANDROID__
                        [PrimaryKey, AutoIncrement]
                        public int id { set; get; }
         
        #else
                        //UWP
                        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
                        public int id { set; get; }
        #endif


        public string nombre { set; get; }
        public string ingredientes { set; get; }
        public string categoria { set; get; }
        public string precio { set; get; }
        public string imagePath { get; set; }
        
        /*
        public Drink(string nom, string ingred, string cate, string imgPath, string precio)
        {
            this.nombre = nom;
            this.ingredientes = ingred;
            this.categoria = cate;
            this.imagePath = imgPath;
            this.precio = precio;
        }*/

       
    }
}
