
using AppDrinkProyectoCompartido;
using AppDrinkUWP.Classes;
using AppDrinkUWP.DataHelper;
using AppDrinkUWP.Pantallas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        UserConfig uc;

        public MainPage()
        {
            this.InitializeComponent();
            uc = UserConfig.Instance();
            //crea la base de datos
            CreateDB();

            //cuando apenas carga te muestra todo, y cuando elegis una categoria va al metodo cbCategorias_Seleccion
            lvTragos.ItemsSource = AppDrinkProyectoCompartido.ListDrinkHelper.getDrinks();

            

            if (uc.isAdmin)
            {
                btnCandado.Content = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx://AppDrinkUWP/Assets/candadoAbierto.png")),
                    Stretch = Stretch.Fill
                };

                btnTuerca.Visibility = Visibility.Visible;
                btnNuevoTrago.Visibility = Visibility.Visible;
            }
            else
            {
                btnCandado.Content = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx://AppDrinkUWP/Assets/candado.png")),
                    Stretch = Stretch.Fill
                };
                btnTuerca.Visibility = Visibility.Collapsed;
                btnNuevoTrago.Visibility = Visibility.Collapsed;
            }


            Categories cat = new Categories();
            List<string> cl = new List<string>();
            cl.Add("Todos");
            cl.AddRange(cat.categoryList);
            cbCategorias.ItemsSource = cl;
            cbCategorias.SelectedIndex = 0;
        }

       

        public void CreateDB()
        {
            //Create DataBase
            db = new DataBase();
            db.CreateDatabase();
            
        }

        private void cbCategorias_Seleccion(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string catSeleccionada = cmb.SelectedValue.ToString();
            lvTragos.ItemsSource = AppDrinkProyectoCompartido.ListDrinkHelper.getDrinksByCategory(catSeleccionada);
        }

        private void btnNuevoTrago_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrinkEdit));
        }

        private void btnCandado_Click(object sender, RoutedEventArgs e)
        {
            if (uc.isAdmin)
            {
                uc.isAdmin = false;
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                this.Frame.Navigate(typeof(Contrasena));
            }
        }

        private void btnTuerca_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Configuracion));
        }

        private void refresh()
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //Se ejecuta al hacer click derecho sobre el item del list view
        private async void LvTragos_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("¿Realmente desea borrar el trago?");
            showDialog.Commands.Add(new UICommand("Sí") { Id = 0 });
            showDialog.Commands.Add(new UICommand("No") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();

            if ((int)result.Id == 0)
            {
                /**
                ListView lv = (ListView)sender;
                ////////////////VER
                SharedProject2.Participante p = (SharedProject2.Participante)lv.SelectedValue;
                SharedProject2.Class1.eliminarParticipante(p);
                lvParticipantes.ItemsSource = SharedProject2.Class1.getParticipantes();**/
            }
            else
            {
                //skip your task 
            }
        }
    }
}
