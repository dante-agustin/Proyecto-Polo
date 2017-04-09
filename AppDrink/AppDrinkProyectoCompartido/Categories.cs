using System;
using System.Collections.Generic;
using System.Text;

namespace AppDrinkProyectoCompartido
{
    public class Categories
    {
        public List<string> categoryList { get; set; }

        public Categories()
        {
            categoryList = new List<string>();
            categoryList.Add("Vodkas");
            categoryList.Add("Whiskys");
            categoryList.Add("Basicos");
            categoryList.Add("Vinos");
            categoryList.Add("Cocktails");
            categoryList.Add("Cervezas");
        }
    }
}
