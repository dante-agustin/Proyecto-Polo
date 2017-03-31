using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;

namespace AppDrinkAndroid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        ImageButton btnTuerca;
        ImageButton btnCandado;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            //SPINNER CATEGORIAS
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            //BTN TUERCA
            btnTuerca = FindViewById<ImageButton>(Resource.Id.imgBtnTuerca);
            btnTuerca.Click += (e, o) =>
            {
                StartActivity(typeof(Configuracion));
            };

            //BTN CANDADO
            btnCandado = FindViewById<ImageButton>(Resource.Id.imgBtnCandado);
            btnCandado.Click += (e, o) =>
            {
                StartActivity(typeof(Contrasena));
            };


        }
        

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
    }
}

