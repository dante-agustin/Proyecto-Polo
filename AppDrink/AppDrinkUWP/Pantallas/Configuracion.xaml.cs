using AppDrinkUWP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Configuracion : Page
    {

        UserConfig uc;

        public Configuracion()
        {
            this.InitializeComponent();
            uc = UserConfig.Instance();

            cbIngr.IsChecked = uc.showIngredientes;
            cbPrecio.IsChecked = uc.showIngredientes;

        }


        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (cbIngr.IsChecked == true)
                uc.showIngredientes = true;
            else
                uc.showIngredientes = false;

            if (cbPrecio.IsChecked == true)
                uc.showPrecio = true;
            else
                uc.showPrecio = false;

            this.Frame.Navigate(typeof(MainPage));
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
