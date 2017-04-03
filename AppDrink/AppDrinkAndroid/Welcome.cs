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
using System.IO;

namespace AppDrinkAndroid
{
    [Activity(Label = "DrinkDroid", MainLauncher = true)]
    public class Welcome : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //TO CHECK FIRST LOGIN
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "Password.txt");

            try
            {
                using (var streamReader = new StreamReader(filename))
                {
                    string content = streamReader.ReadToEnd();
                }
                StartActivity(typeof(MainActivity));
                this.Finish();
            }
            catch
            {
                SetContentView(Resource.Layout.Welcome);

                Button btnDone = FindViewById<Button>(Resource.Id.btnDone);
                EditText pass = FindViewById<EditText>(Resource.Id.txtPass);
                btnDone.Click += (e, o) =>
                {
                    string password = pass.Text.ToString();

                    using (var streamWriter = new StreamWriter(filename, true))
                    {
                        streamWriter.Write(password);
                    }
                    //Create file
                    StartActivity(typeof(MainActivity));
                };
            }
        }
    }
}