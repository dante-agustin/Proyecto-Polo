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
using Android.Graphics;

namespace AppDrinkAndroid.Activities
{
    [Activity(Label = "ViewDrinkActivity", ScreenOrientation = ScreenOrientation.Locked)]
    public class ViewDrinkActivity : Activity
    {
        TextView nombre;
        TextView ingredientes;
        TextView precio;
        TextView categoria;
        Button listo;
        string drinkPhotoPath = "default";
        int posicion;
        string category;
        static CustomAdapter dataAdapter;
        ImageView imgViewDrinkCapture;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewDrink);

            nombre = FindViewById<TextView>(Resource.Id.tvNombre);
            ingredientes = FindViewById<TextView>(Resource.Id.tvIngredientes);
            precio = FindViewById<TextView>(Resource.Id.tvPrecio);
            categoria = FindViewById<TextView>(Resource.Id.tvCategoria);
            listo = FindViewById<Button>(Resource.Id.btGuardar);
            imgViewDrinkCapture = FindViewById<ImageView>(Resource.Id.imgViewDrinkCapture);


            listo.Click += (e, o) =>
            {
                this.Finish();
            };

            //Si vino desde el botón "Agregar", posicion=-1
            posicion = Intent.GetIntExtra("posicion", -1);
            category = Intent.GetStringExtra("categoria");

            AppDrinkProyectoCompartido.Drink drinkAMostrar = AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(category)[posicion];
            nombre.Text = drinkAMostrar.nombre;
            ingredientes.Text = drinkAMostrar.ingredientes;
            precio.Text = drinkAMostrar.precio;
            categoria.Text = drinkAMostrar.categoria;
            drinkPhotoPath = drinkAMostrar.imagePath;
            //LoadImageDrinkOnListView(imgViewDrinkCapture, drinkAMostrar.imagePath);
        }
        
        private static void LoadImageDrinkOnListView(ImageView imgViewDrinkImage, string imageDrinkPath)
        {
            if (imageDrinkPath != "default")
            {
                //error de memoria en tiempo de ejecucion
                //imgViewDrinkImage.SetImageBitmap(BitmapFactory.DecodeFile(item.imagePath));

                Bitmap bitmap = imageDrinkPath.LoadAndResizeBitmap(100, 100);

                if (bitmap != null)
                {
                    imgViewDrinkImage.SetImageBitmap(bitmap);
                    bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();
            }
            else
            {
                imgViewDrinkImage.SetImageResource(Resource.Drawable.drinkDefault);
            }
        }
    }
}