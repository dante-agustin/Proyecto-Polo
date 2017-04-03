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
using AppDrinkProyectoCompartido;
using Android.Graphics;
using Android.Content.Res;

namespace AppDrinkAndroid
{
    
    public class DrinkAdapter : BaseAdapter<AppDrinkProyectoCompartido.Drink>
    {
        Activity context = null;
        IList<AppDrinkProyectoCompartido.Drink> drinks = new List<AppDrinkProyectoCompartido.Drink>();


        public DrinkAdapter(Activity context, IList<AppDrinkProyectoCompartido.Drink> drinks) : base()
        {
            this.context = context;
            this.drinks = drinks;
        }

        public override AppDrinkProyectoCompartido.Drink this[int position]
        {
            get { return drinks[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return drinks.Count; }
        }
        

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            // Get our object for position
            var item = drinks[position];

            // Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    context.LayoutInflater.Inflate(
                    Resource.Layout.DrinkListItem,
                    parent,
                    false)) as RelativeLayout;


            // Find references to each subview in the list item's view
            TextView tvNombre = view.FindViewById<TextView>(Resource.Id.tvNombre);
            TextView tvIngredientes = view.FindViewById<TextView>(Resource.Id.tvIngredientes);
            TextView tvCategoria = view.FindViewById<TextView>(Resource.Id.tvCategoria);
            TextView tvPrecio = view.FindViewById<TextView>(Resource.Id.tvPrecio);
            
            ImageView imgViewDrinkImage = view.FindViewById<ImageView>(Resource.Id.imgViewDrinkImage);

            UserConfig uc = UserConfig.Instance();
            if (uc.showIngredientes)
                tvIngredientes.Visibility  = ViewStates.Visible;
            else
                tvIngredientes.Visibility = ViewStates.Invisible;

            if (uc.showPrecio)
                tvPrecio.Visibility = ViewStates.Visible;
            else
                tvPrecio.Visibility = ViewStates.Invisible;


            //Assign item's values to the various subviews
            tvNombre.SetText(item.nombre, TextView.BufferType.Normal);
            tvCategoria.SetText(item.categoria, TextView.BufferType.Normal);
            tvIngredientes.SetText(item.ingredientes, TextView.BufferType.Normal);
            tvPrecio.SetText(item.precio, TextView.BufferType.Normal);

            //cargar la imagen del obj drink en imageview
            if ( item.imagePath!="default")
            {
                //error de memoria en tiempo de ejecucion
                //imgViewDrinkImage.SetImageBitmap(BitmapFactory.DecodeFile(item.imagePath));
                
                Bitmap bitmap = item.imagePath.LoadAndResizeBitmap(100, 100);

                if(bitmap != null)
                {
                    imgViewDrinkImage.SetImageBitmap(bitmap);
                    bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();
            }
            else{
              
                imgViewDrinkImage.SetImageResource(Resource.Drawable.drinkDefault);
            }
       
            
            


            //Finally return the view
            return view;
        }
    }
}