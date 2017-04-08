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

namespace AppDrinkAndroid
{
    [Activity(Label = "Configuracion", ScreenOrientation = ScreenOrientation.Locked)]
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
            checkboxIngred = FindViewById<CheckBox>(Resource.Id.checkBoxIngrendientes);
            btnGuardar = FindViewById<Button>(Resource.Id.btGuardar);
            btnCancelar = FindViewById<Button>(Resource.Id.btCancelar);

            UserConfig uc = UserConfig.Instance();

            //LOAD PREVIOUS CONFIG
            checkboxPrecio.Checked = uc.showPrecio;
            checkboxIngred.Checked = uc.showIngredientes;

            //ON CLICK METHODS
            btnGuardar.Click += (e, o) =>
            {
                if (checkboxPrecio.Checked)
                {
                    uc.showPrecio = true;
                }
                else
                {
                    uc.showPrecio = false;
                }

                if (checkboxIngred.Checked)
                {
                    uc.showIngredientes = true;
                    //si esta chequeado, el TextViewIngred tiene que estar en show
                }
                else
                {
                    uc.showIngredientes = false;
                    //sino, tiene que estar en hide
                }
                Finish();
            };

            btnCancelar.Click += (e, o) =>
            {
                //base.OnBackPressed();
                Finish();
            };
        }
    }
}