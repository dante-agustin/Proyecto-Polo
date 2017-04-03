using System;
using System.Collections.Generic;
using System.Text;

namespace AppDrinkProyectoCompartido
{
    class ListDrinkHelper
    {
        static List<Drink> lista;

        static ListDrinkHelper()
        {
            lista = new List<Drink>();
            Drink trago = new Drink("ASJ", "ASJAJSJAJSJA", "Vodka","default");
            lista.Add(trago);
                     
        }

        public static List<Drink>getDrinks()
        {
            return lista;
        }

        public static void agregarDrink(Drink newDrink)
        {
            lista.Add(newDrink);
        }

        public static void eliminarDrink(Drink d)
        {
            lista.Remove(d);
        }
    }
}
