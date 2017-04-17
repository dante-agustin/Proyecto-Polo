using AppDrinkProyectoCompartido;
using AppDrinkUWP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppDrinkUWP.Pantallas
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ViewDrink : Page
    {
        Drink trago;

        public ViewDrink()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = (Drink)e.Parameter;
            trago = (Drink)DataContext;

            LoadImageDrinkOnImageView(trago.imagePath);
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        
        private async void LoadImageDrinkOnImageView(string imageDrinkPath)
        {
            if (imageDrinkPath != "default")
            {
                //Buscar imagen en el disco y cargarla al image "drinkImageCapture"

                BitmapImage bimage = new BitmapImage();
                StorageFile file = await StorageFile.GetFileFromPathAsync(imageDrinkPath);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                bimage.SetSource(stream);
                drinkImageCapture.Source = bimage;

            }
            else
            {
                //Si no existe imagen para ese trago, cargar imagen por defecto
                BitmapImage drinkImageDefault = new BitmapImage(new Uri(this.BaseUri, "/Assets/drinkDefault.jpg"));
                drinkImageCapture.Source = drinkImageDefault;
            }
        }
    }
}
