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

namespace AppDrinkAndroid
{
    public static class App
    {
        public static File file;
        public static File dir;
        public static Bitmap bitmap;
    }

    [Activity(Label = "DrinkEdit")]
    public class DrinkEditActivity : Activity
    {
        ImageButton imgBtnAgregarFoto;
        ImageView imgViewDrinkCapture;

        EditText etNombre, etIngredientes, etPrecio;
        Spinner spinnerCategoria;
        String drinkPhotoPath="default";


        Button btGuardar, btCancelar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DrinkEdit);

            //Cambiar si es un edit
            this.Title = "Agregar trago";

            
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
           

    }

        public static  void SetDrinksOnSpinner(Context contexto, Spinner spnCategoria)
        {
            
            var list = new List<String>();
            
            list.Add("Vodka");
            list.Add("Whisky");
            list.Add("Basico");
            list.Add("Vinos");
            list.Add("Cocktails");
            list.Add("Cerveza");
            if (contexto is MainActivity)
            {
                list.Add("Todas");
            }
            list.Add("Seleccionar categoria");
            

            int listsize = list.Count - 1;

            var dataAdapter = new CustomAdapter(contexto, Android.Resource.Layout.SimpleSpinnerItem, list, listsize);
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
                Toast.MakeText(this, "Por favor, complete todos los datos. Ningún campo puede quedar vacío.", ToastLength.Long).Show();
            }
            else
            {
                /*
                if (posicion > -1)   //es un edit, no debe crearse un nuevo objeto participante
                {
                    SharedProject2.Class1.getParticipantes()[posicion].nombre = etNombre.Text;
                    SharedProject2.Class1.getParticipantes()[posicion].apellido = etApellido.Text;
                    SharedProject2.Class1.getParticipantes()[posicion].email = etMail.Text;
                    SharedProject2.Class1.getParticipantes()[posicion].fechaNac = Convert.ToDateTime(etFechaNac.Text);

                }*/

                string nombre = etNombre.Text;
                string categoria = spinnerCategoria.SelectedItem.ToString();
                string ingredientes = etIngredientes.Text;
                string precio = "$ " + etPrecio.Text;


                //drinkPhotoPath queda "default" cuando no se toma una foto
                
                AppDrinkProyectoCompartido.Drink newDrink = new AppDrinkProyectoCompartido.Drink(nombre, ingredientes, categoria, drinkPhotoPath, precio);
                AppDrinkProyectoCompartido.ListDrinkHelper.agregarDrink(newDrink);
                Finish();
            }
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