
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
            
            /*
            Drink trago = new Drink()
            {
                nombre="ABC",
                ingredientes="EFGH",
                categoria="Vodka",
                imagePath="default",
                precio="$ 125"
            };
            lista.Add(trago);*/
                     
        }

        public static void SetList(List<Drink> list)
        {
            lista = null;
            lista = list;
        }

        public static List<Drink>getDrinks()
        {
            
            return lista;
        }

        public static List<Drink> getDrinksByCategory(string category)
        {
            List<Drink> listaCategorizada= new List<Drink>();
            string categoryDefault = "Todas";

            if(category != categoryDefault)
            {
                foreach (Drink obj in lista)
                {
                    if (obj.categoria == category)
                    {
                        listaCategorizada.Add(obj);
                    }
                }

                return listaCategorizada;
            }
            else{
                return lista; //Si seleccion "Todas" las categoria, devuelve la lista original completa
            }
            
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
