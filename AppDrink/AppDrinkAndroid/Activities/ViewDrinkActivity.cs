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
using Android.Content.PM;

namespace AppDrinkAndroid.Activities
{
    [Activity(Label = "ViewDrinkActivity", ScreenOrientation = ScreenOrientation.Locked)]
    public class ViewDrinkActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewDrink);
            // Create your application here
        }
    }
}