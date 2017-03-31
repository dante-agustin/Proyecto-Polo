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
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }

    [Activity(Label = "DrinkEdit")]
    public class DrinkEdit : Activity
    {
        ImageButton imgBtnAgregarFoto;
        ImageView imgViewDrinkCapture;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DrinkEdit);

            //Cambiar si es un edit
            this.Title = "Agregar trago";
           
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                imgBtnAgregarFoto = FindViewById<ImageButton>(Resource.Id.imgBtnAgregarFoto);
                imgBtnAgregarFoto.Click += ImgBtnAgregarFoto_Click;
                imgViewDrinkCapture = FindViewById<ImageView>(Resource.Id.imgViewDrinkCapture);

            }

            Spinner spinnerCategoria = FindViewById<Spinner>(Resource.Id.spinnerCategoria);

            var list = new List<String>();
            list.Add("Vodka");
            list.Add("Whisky");
            list.Add("Basico");
            list.Add("Vinos");
            list.Add("Cocktails");
            list.Add("Cerveza");
            list.Add("Seleccionar la categoria");

            int listsize = list.Count - 1;

            var dataAdapter = new CustomAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, list, listsize);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerCategoria.Adapter = dataAdapter;
            spinnerCategoria.SetSelection(listsize);

        }

        private void ImgBtnAgregarFoto_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file)); 
            StartActivityForResult(intent, 0);  
                 
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "AppDrink Images");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
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
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imgViewDrinkCapture.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                imgViewDrinkCapture.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }
    }

    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}