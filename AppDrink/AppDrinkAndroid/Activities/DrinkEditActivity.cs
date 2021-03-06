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
using Android.Provider;
using Java.IO;
using Android.Graphics;
using Android.Media;
using JavaUri = Java.Net;
using Android.Content.PM;
using AppDrinkProyectoCompartido;
using AppDrinkAndroid.DataHelper;
using Android.Util;

namespace AppDrinkAndroid
{
    public static class App
    {
        public static File file;
        public static File dir;
        public static Bitmap bitmap;
    }

    [Activity(Label = "DrinkEdit", ScreenOrientation = ScreenOrientation.Locked)]
    public class DrinkEditActivity : Activity
    {
        ImageButton imgBtnAgregarFoto;
        ImageView imgViewDrinkCapture;
        EditText etNombre, etIngredientes, etPrecio;
        Button btGuardar, btCancelar;
        Spinner spinnerCategoria;
        string drinkPhotoPath="default";
        int posicion;
        string categoria;
        static CustomAdapter dataAdapter;
        DataBase db; 

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DrinkEdit);
            CreateDB();

            etNombre = FindViewById<EditText>(Resource.Id.etNombre);
            etIngredientes = FindViewById<EditText>(Resource.Id.etIngredientes);
            etPrecio = FindViewById<EditText>(Resource.Id.etPrecio);
            btGuardar = FindViewById<Button>(Resource.Id.btGuardar);
            btCancelar = FindViewById<Button>(Resource.Id.btCancelar);
            spinnerCategoria = FindViewById<Spinner>(Resource.Id.spinnerCategoria);
            SetDrinksOnSpinner(this,spinnerCategoria);

            btGuardar.Click += BtGuardar_Click;
            btCancelar.Click += BtCancelar_Click;

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                imgBtnAgregarFoto = FindViewById<ImageButton>(Resource.Id.imgBtnAgregarFoto);
                imgBtnAgregarFoto.Click += ImgBtnAgregarFoto_Click;
                imgViewDrinkCapture = FindViewById<ImageView>(Resource.Id.imgViewDrinkCapture);
            }

            //Si vino desde el bot�n "Agregar", posicion=-1
            posicion = Intent.GetIntExtra("posicion", -1);
            categoria = Intent.GetStringExtra("categoria");

            if (posicion > -1)   //Es un edit
            {
                this.Title = "Modificar trago";
                Drink drinkAModificar = ListDrinkHelper.getDrinksByCategory(categoria)[posicion];
                etNombre.Text = drinkAModificar.nombre;
                etIngredientes.Text = drinkAModificar.ingredientes;
                etPrecio.Text = drinkAModificar.precio;
                int spinnerPosition = dataAdapter.GetPosition(drinkAModificar.categoria);
                spinnerCategoria.SetSelection(spinnerPosition);

                drinkPhotoPath = drinkAModificar.imagePath;
                LoadImageDrinkOnImageView(imgViewDrinkCapture, drinkAModificar.imagePath);

            }
            else
            {//Es un alta de trago
                this.Title = "Agregar trago";
            }
        }

        private static void LoadImageDrinkOnImageView(ImageView imgViewDrinkImage ,string imageDrinkPath)
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

        public static  void SetDrinksOnSpinner(Context contexto, Spinner spnCategoria)
        {
            
            var list = new List<String>();
            if (contexto is MainActivity)
            {
                list.Add("Todas");
            }

            Categories cat = new Categories();
            list.AddRange(cat.categoryList);

            list.Add("Seleccionar categoria");
            

            int listsize = list.Count - 1;

            dataAdapter = new CustomAdapter(contexto, Android.Resource.Layout.SimpleSpinnerItem, list, listsize);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategoria.Adapter = dataAdapter;
            spnCategoria.SetSelection(listsize);

        }

        private void BtCancelar_Click(object sender, EventArgs e)
        {
            //base.OnBackPressed();
            Finish();
        }

        private void BtGuardar_Click(object sender, EventArgs e)
        {
            

            if (string.IsNullOrEmpty(etNombre.Text) || string.IsNullOrEmpty(etIngredientes.Text) ||
                spinnerCategoria.SelectedItem.ToString()=="Seleccionar categoria" || string.IsNullOrEmpty(etPrecio.Text) )
            {
                Toast.MakeText(this, "Por favor, complete todos los datos. Ning�n campo puede quedar vac�o.", ToastLength.Long).Show();
            }
            else
            {

                if (posicion > -1)   //es un edit, no debe crearse un nuevo objeto participante
                {
                    
                    Drink drinkAEditar=ListDrinkHelper.getDrinksByCategory(categoria)[posicion];
                    drinkAEditar.nombre = etNombre.Text;
                    drinkAEditar.ingredientes = etIngredientes.Text;
                    drinkAEditar.precio = etPrecio.Text;
                    drinkAEditar.categoria= spinnerCategoria.SelectedItem.ToString();
                    drinkAEditar.imagePath = drinkPhotoPath;
                    db.updateTableDrink(drinkAEditar);

                }
                else
                {

                    //drinkPhotoPath queda "default" cuando no se toma una foto

                    Drink newDrink = new Drink()
                    {
                        nombre = etNombre.Text,
                        categoria = spinnerCategoria.SelectedItem.ToString(),
                        ingredientes = etIngredientes.Text,
                        precio = etPrecio.Text,
                        imagePath= drinkPhotoPath
                    };
                    db.insertIntoTableDrink(newDrink);                  
                }
                
                Finish();
            }
        }

        public void CreateDB()
        {
            //Create DataBase
            db = new DataBase();
            db.createDataBase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);
        }

        private void ImgBtnAgregarFoto_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
            if( string.IsNullOrEmpty(etNombre.Text))
            {
                Toast.MakeText(this,"Debe escribir el nombre del trago antes de tomar una foto del mismo.", ToastLength.Short).Show();
            }else
            {
                string nombreFoto = etNombre.Text;
                App.file = new File(App.dir, String.Format(nombreFoto, Guid.NewGuid()));
                drinkPhotoPath = App.file.AbsolutePath;
                //App.file = new File(App.dir, String.Format("myPhoto_{0}", Guid.NewGuid()));

                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App.file));
                StartActivityForResult(intent, 0);
            }
                            
        }

        private void CreateDirectoryForPictures()
        {
            App.dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "AppDrink Images");
            if (!App.dir.Exists())
            {
                App.dir.Mkdirs();
            }
        }    

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App.file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imgViewDrinkCapture.Height;
            App.bitmap = App.file.Path.LoadAndResizeBitmap(width, height);
            
            if (App.bitmap != null)
            {
                imgViewDrinkCapture.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }
    }

    
}