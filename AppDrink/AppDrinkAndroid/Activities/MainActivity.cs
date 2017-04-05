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
using System.Reflection;
using AppDrinkProyectoCompartido;

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

            //Context menu
            RegisterForContextMenu(lvDrinks);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.listViewDrinks)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                //necesito acceder al nombre del trago
                Object trago=lvDrinks.GetItemAtPosition(info.Position);
                AppDrinkProyectoCompartido.Drink drink = trago as AppDrinkProyectoCompartido.Drink;
                if (drink != null)
                {
                    menu.SetHeaderTitle(drink.nombre);
                }
              
                var menuItems = Resources.GetStringArray(Resource.Array.menu);

                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var menuItemIndex = item.ItemId;
            var menuItems = Resources.GetStringArray(Resource.Array.menu);
            var menuItemName = menuItems[menuItemIndex];
           
            var listItemName = "";

            Object obj = lvDrinks.GetItemAtPosition(info.Position);
            var propertyInfo = obj.GetType().GetProperty("Instance");
            Drink trago = propertyInfo.GetValue(obj, null) as Drink;

            //En trago Instance tengo todo...

            /*
            AppDrinkProyectoCompartido.Drink drink = trago as AppDrinkProyectoCompartido.Drink;
            if (drink != null)
            {
                listItemName = drink.nombre;
            }
            */

            Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, listItemName), ToastLength.Short).Show();
            return true;
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