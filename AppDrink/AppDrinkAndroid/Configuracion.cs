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
    [Activity(Label = "Configuracion")]
    public class Configuracion : Activity
    {
        CheckBox checkboxPrecio;
        CheckBox checkboxIngred;
        Button btnGuardar;
        Button btnCancelar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Configuracion);

            checkboxPrecio = FindViewById<CheckBox>(Resource.Id.checkBoxPrecio);
            checkboxIngred = FindViewById<CheckBox>(Resource.Id.checkBoxIngredientes);
            btnGuardar = FindViewById<Button>(Resource.Id.btGuardar);
            btnCancelar = FindViewById<Button>(Resource.Id.btCancelar);

            if ( btnGuardar.onClick() ) //si das click en guardar, va mirando los checkbox y hace los cambios
            {
                if (checkboxPrecio.isChecked())
                {
                    //si esta chequeado, el TextViewPrecio tiene que estar en show
                }
                else
                {
                    //sino, tiene que estar en hide
                }

                if (checkboxIngred.isChecked())
                {
                    //si esta chequeado, el TextViewIngred tiene que estar en show
                }
                else
                {
                    //sino, tiene que estar en hide
                }
                StartActivity(typeof(MainActivity));
            }
            
            if ( btnCancelar.onClick() ) //si das click en cancelar no hace nada
            {
                StartActivity(typeof(MainActivity));
            }


        }
    }
}