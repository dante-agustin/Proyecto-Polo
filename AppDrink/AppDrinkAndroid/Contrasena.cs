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
    [Activity(Label = "Contrasena")]
    public class Contrasena : Activity
    {
        EditText txtPass;
        Button btnIngresar;
        Button btnCancelar;
        TextView txtIncorrecta;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Contrasena);

            txtPass = FindViewById<EditText>(Resource.Id.txtPassword);
            btnIngresar = FindViewById<Button>(Resource.Id.btIngresar);
            btnCancelar = FindViewById<Button>(Resource.Id.btCancelar);
            txtIncorrecta = FindViewById<TextView>(Resource.Id.txtIncorrecta);

            /*
            if ( btnIngresar.onClick() )
            {
                //if ( txtPass.getText() == SQL SELECT password FROM usuario ) 
                {
                    StartActivity(typeof(MainActivity)); //   si esta todo bien, pasa a Main pero con poderes de mod
                }
                else
                {
                    txtIncorrecta.visibility = "visible"; //en el axml por defecto tiene "invisible"
                }
            }
           */
            /*
             if ( btnCancelar.onClick() ) //va al Main pero no pasa con poderes de mod
             {
            StartActivity(typeof(MainActivity));
             }
             */
        }
    }
}