using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDrinkProyectoCompartido
{
    class Drink
    {
        public string nombre { set; get; }
        public string ingredientes { set; get; }
        //public virtual string imageURL { get; set; }

        public Drink(string nom, string ingred)
        {
            this.nombre = nom;
            this.ingredientes = ingred;
            
        }
    }
}
