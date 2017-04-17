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
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;


// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppDrinkUWP.Pantallas
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Welcome : Page
    {
        public Task InitTask { get; private set; }

        public Welcome()
        {
            this.InitializeComponent();

            Task InitTask = CheckIfFileExists();
            
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }


        private async void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if ( string.IsNullOrEmpty(txtContrasena.Text ))
            {
                lblError.Text = "Por favor, ingrese una contraseña";
            }
            else
            {
                //Create variables for file
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync("Password.txt", CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(file, txtContrasena.Text); //Store written password

                var frame = (Frame)Window.Current.Content;
                frame.Navigate(typeof(MainPage));
            }

        }



        public async Task CheckIfFileExists()
        {
            try
            {
                //Create variables for file
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.GetFileAsync("Password.txt");
                string text = await FileIO.ReadTextAsync(file); //try to read
                //IT IS NOT FIRST LOGIN

                GoToMain();

            }
            catch
            {
                //IT IS FIRST LOGIN!
            }
        }

        public async Task GoToMain()
        {
            var frame = (Frame)Window.Current.Content;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => frame.Navigate(typeof(MainPage)));
        }

    }
}


