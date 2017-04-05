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
    [Activity(Label = "AppDrinkAndroid", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ImageButton btnTuerca;
        ImageButton btnCandado;
        ImageButton btnAgregarTrago;
        ListView lvDrinks;
        DrinkAdapter drinkAdapter;
        string categoria;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            //SPINNER CATEGORIAS
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            DrinkEditActivity.SetDrinksOnSpinner(this, spinner);
            spinner.ItemSelected += Spinner_ItemSelected;        

            //BTN AGREGAR TRAGO

            btnAgregarTrago = FindViewById<ImageButton>(Resource.Id.imgBtnAgregarTrago);
            btnAgregarTrago.Click += (e, o) =>
            {
                Intent i = new Intent(this, typeof(DrinkEditActivity));
                StartActivity(i);
            };


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

            //LIST VIEW DRINKS
            lvDrinks = FindViewById<ListView>(Resource.Id.listViewDrinks);
            drinkAdapter = new DrinkAdapter(this, AppDrinkProyectoCompartido.ListDrinkHelper.getDrinks());
            lvDrinks.Adapter = drinkAdapter;
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            categoria=spinner.SelectedItem.ToString();
            //create our adapter
            drinkAdapter = new DrinkAdapter(this, AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(categoria));
            //Hook up our adapter to our ListView
            lvDrinks.Adapter = drinkAdapter;
        }


        //Cuando vuelve desde DrinkEdit refresca el listview
        protected override void OnResume()
        {
            base.OnResume();
            //create our adapter
            drinkAdapter = new DrinkAdapter(this, AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(categoria));
            //Hook up our adapter to our ListView
            lvDrinks.Adapter = drinkAdapter;
        }

        
    }
}