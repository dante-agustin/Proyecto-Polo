﻿using AppDrinkUWP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Contrasena : Page
    {
        UserConfig uc;

        public Contrasena()
        {
            this.InitializeComponent();
            uc = UserConfig.Instance();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("Password.txt");
            string fileText = await FileIO.ReadTextAsync(file); //try to read

            if (txtContrasena.Text.Equals(fileText))
            {
                uc.isAdmin = true;
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                uc.isAdmin = false;
                if (lblError.Text.ToString().Length < 1)
                    lblError.Text = "Por favor, ingrese una contraseña";
                else
                    lblError.Text = "Contraseña incorrecta";
                lblError.Visibility = Visibility.Visible;
            }
        }
    }
}
