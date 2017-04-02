using System;
using System.Collections.Generic;
using System.Text;

namespace AppDrinkProyectoCompartido
{
    class Prueba
    {
        static List<Drink> lista;

        static Prueba()
        {
            lista = new List<Drink>();
            Drink trago = new Drink("ASJ", "ASJAJSJAJSJA", "Vodka");
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
