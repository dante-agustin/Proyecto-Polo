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
using Android.Content.PM;

namespace AppDrinkAndroid
{
    [Activity(Label = "AppDrinkAndroid", MainLauncher = false, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Locked)]
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
            UserConfig uc = UserConfig.Instance();

            //SPINNER CATEGORIAS
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            DrinkEditActivity.SetDrinksOnSpinner(this, spinner);
            spinner.ItemSelected += Spinner_ItemSelected;
            spinner.SetSelection(0);    //Para que por default sea Todos
                    

            //BTN AGREGAR TRAGO
            btnAgregarTrago = FindViewById<ImageButton>(Resource.Id.imgBtnAgregarTrago);
            if (uc.isAdmin == false)
                btnAgregarTrago.Visibility = ViewStates.Invisible;
            if (uc.isAdmin == true)
                btnAgregarTrago.Visibility = ViewStates.Visible;
            btnAgregarTrago.Click += (e, o) =>
            {
                Intent i = new Intent(this, typeof(DrinkEditActivity));
                StartActivity(i);
            };
            
            //BTN TUERCA
            btnTuerca = FindViewById<ImageButton>(Resource.Id.imgBtnTuerca);
            if (uc.isAdmin == false)
                btnTuerca.Visibility = ViewStates.Invisible;
            if (uc.isAdmin == true)
                btnTuerca.Visibility = ViewStates.Visible;
            btnTuerca.Click += (e, o) =>
            {
                StartActivity(typeof(Configuracion));
            };

            //BTN CANDADO
            btnCandado = FindViewById<ImageButton>(Resource.Id.imgBtnCandado);
            btnCandado.Click += (e, o) =>
            {
                if(uc.isAdmin == false)
                    StartActivity(typeof(Contrasena));
                if (uc.isAdmin == true)
                {
                    uc.isAdmin = false;
                    StartActivity(typeof(MainActivity));
                }
            };

            //LIST VIEW DRINKS
            lvDrinks = FindViewById<ListView>(Resource.Id.listViewDrinks);
            drinkAdapter = new DrinkAdapter(this, AppDrinkProyectoCompartido.ListDrinkHelper.getDrinks());
            lvDrinks.Adapter = drinkAdapter;

            //Context menu
            if (uc.isAdmin == true)
                RegisterForContextMenu(lvDrinks);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.listViewDrinks)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                //necesito acceder al nombre del trago
                Object obj = lvDrinks.GetItemAtPosition(info.Position);
                var propertyInfo = obj.GetType().GetProperty("Instance");
                AppDrinkProyectoCompartido.Drink trago = propertyInfo.GetValue(obj, null) as AppDrinkProyectoCompartido.Drink;

                if (trago != null)
                {
                    menu.SetHeaderTitle("Trago - " + trago.nombre);
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
            int esModificacion = 0;
           
            //var listItemName = "";
            Object obj = lvDrinks.GetItemAtPosition(info.Position);
            var propertyInfo = obj.GetType().GetProperty("Instance");
            AppDrinkProyectoCompartido.Drink trago = propertyInfo.GetValue(obj, null) as AppDrinkProyectoCompartido.Drink;

            if (trago != null)
            {
                //listItemName = trago.nombre;

                if (item.ItemId == esModificacion) //Se va a modificar el trago del list view
                {
                    ModificarTrago(info.Position);
                }
                else //Se va a eliminar el trago del list view
                {
                    EliminarTrago(info.Position);
                }
            }            

           
            return true;
        }

        private void ModificarTrago(int positionInListView)
        {
            Intent i = new Intent(this, typeof(DrinkEditActivity));
            i.PutExtra("posicion", positionInListView);
            i.PutExtra("categoria", categoria);
            StartActivity(i);
        }

        private void EliminarTrago(int positionInListView)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog msjConfirmDelete = builder.Create();
            msjConfirmDelete.SetTitle("Confirmar");
            msjConfirmDelete.SetMessage("¿Realmente deseas eliminar este trago de la lista?");
            msjConfirmDelete.SetIcon(Resource.Drawable.Icon);

            msjConfirmDelete.SetButton("Sí", (s, ev) =>
            {
                AppDrinkProyectoCompartido.Drink drinkAEliminar = AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(categoria)[positionInListView];
                AppDrinkProyectoCompartido.ListDrinkHelper.eliminarDrink(drinkAEliminar);
                refresh();

                Toast.MakeText(this, "Trago eliminado", ToastLength.Short).Show();

                //ACA FALTARIA AGREGAR ELIMINARLO DE LA BASE DE DATOS
                /////////////////
                /////////////
            });

            msjConfirmDelete.SetButton2("No", (s, ev) =>
            {
                msjConfirmDelete.Cancel();
            });

            msjConfirmDelete.Show();
        }

        private void refresh()
        {
            //create our adapter
            drinkAdapter = new DrinkAdapter(this, AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(categoria));
            //Hook up our adapter to our ListView
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