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

namespace AppDrinkAndroid
{
    class UserConfig
    {
        private static UserConfig _instance = new UserConfig(); //Singleton

        public bool showPrecio { get; set; }
        public bool showIngredientes { get; set; }

        static internal UserConfig Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }

    }
}