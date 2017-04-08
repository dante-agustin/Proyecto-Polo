﻿
using AppDrinkUWP.DataHelper;
using AppDrinkUWP.Pantallas;
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

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppDrinkUWP
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DataBase db;
        public MainPage()
        {
            this.InitializeComponent();
            //crea la base de datos
            CreateDB();
        }

        public void CreateDB()
        {
            //Create DataBase
            db = new DataBase();
            db.CreateDatabase();
            
        }


        private void btnNuevoTrago_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrinkEdit));
        }

        private void btnCandado_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Contrasena));
        }

        private void btnTuerca_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Configuracion));
        }
    }
}