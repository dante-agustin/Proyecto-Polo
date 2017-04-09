using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppDrinkUWP.Pantallas
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Welcome : Page
    {
        public Welcome()
        {
            this.InitializeComponent();
            
            //no anda bien, pasa que para buscarlo es asincronico sino, tengo que ver como solucionarlo
            /*string rootPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string filePath = Path.Combine(rootPath, "contrasena.txt");

            if (System.IO.File.Exists(filePath))
            {
                this.Frame.Navigate(typeof(MainPage));
            }*/
            
        }

        /* Esto se para hacer un Hiden Text que diga contraseña, igual acá como siempre arranca
         * Seleccionando el TextBox no funciona */
        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }

        private async void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtContrasena.Text))
            {
                var dialog = new MessageDialog("ERROR");
                dialog.Title = "Debe completar el campo Contraseña";
                dialog.Commands.Add(new UICommand { Label = "Aceptar", Id = 0 });
                var res = await dialog.ShowAsync();
            }
            else
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile contrasenaFile = await storageFolder.CreateFileAsync("contrasena.txt",
                        Windows.Storage.CreationCollisionOption.ReplaceExisting);

                await Windows.Storage.FileIO.WriteTextAsync(contrasenaFile, txtContrasena.Text);

                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
