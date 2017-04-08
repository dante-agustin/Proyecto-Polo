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
using System.IO;

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

            //TO CHECK FIRST LOGIN
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "Password.txt");
            string content = "";

            using (var streamReader = new StreamReader(filename))
            {
                content = streamReader.ReadToEnd();
            }
            
            btnIngresar.Click += (e, o) =>
            {
                if (txtPass.Text.ToString().Equals(content)) 
                {
                    /*
                    UserConfig uc = UserConfig.Instance();
                    uc.isAdmin = true;
                    StartActivity(typeof(MainActivity));
                    Finish();
                    */

                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    if (txtPass.Text.ToString().Length < 1)
                        txtIncorrecta.Text = "Por favor, ingrese una contraseña";
                    else
                        txtIncorrecta.Text = "Contraseña incorrecta";
                    txtIncorrecta.Visibility = ViewStates.Visible; 
                    //en el axml por defecto tiene "invisible"
                }
            };

            btnCancelar.Click += (e, o) =>
            {
                SetResult(Result.Canceled);
                Finish();
            };
        }
    }
}