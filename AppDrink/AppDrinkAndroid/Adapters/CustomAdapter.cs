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

    public class CustomAdapter : ArrayAdapter<String>
    {      

        int _count;

        public CustomAdapter(Context context, int textViewResourceId, List<String> objects, int listCount)
            : base(context, textViewResourceId, objects)
        {
            _count = listCount;
        }

        public override int Count
        {
            get
            {
                return _count;
            }
        }
    }
}



