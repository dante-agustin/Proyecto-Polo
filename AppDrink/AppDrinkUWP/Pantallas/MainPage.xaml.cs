
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
        string categoria = "Todas";
        List<Drink> lstSource;
        UserConfig uc;

        public MainPage()
        {
            this.InitializeComponent();
            uc = UserConfig.Instance();
            //crea la base de datos
            CreateDB();        

            if (uc.isAdmin)
            {
                btnCandado.Content = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx://AppDrinkUWP/Assets/candadoAbierto.png")),
                    Stretch = Stretch.Fill
                };

                btnTuerca.Visibility = Visibility.Visible;
                btnNuevoTrago.Visibility = Visibility.Visible;
                lvTragos.RightTapped += LvTragos_RightTapped;
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
            cl.Add("Todas");
            cl.AddRange(cat.categoryList);
            cbCategorias.ItemsSource = cl;
            cbCategorias.SelectedIndex = 0;

            //cuando apenas carga te muestra todo, y cuando elegis una categoria va al metodo cbCategorias_Seleccion
            LoadAndRefreshListView();
            
        }

        private void LvTragos_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {            
            Object obj = e.OriginalSource;
            var s = (FrameworkElement)e.OriginalSource;
            Drink tragoSeleccionado = (Drink)s.DataContext;
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem itemModificar = new MenuFlyoutItem { Text = "Modificar" };
            MenuFlyoutItem itemEliminar = new MenuFlyoutItem { Text = "Eliminar" };
            itemModificar.Tag = tragoSeleccionado;
            itemEliminar.Tag = tragoSeleccionado;
            myFlyout.Items.Add(itemModificar);
            myFlyout.Items.Add(itemEliminar);
            myFlyout.ShowAt(lvTragos, e.GetPosition(lvTragos));

            itemModificar.Click += ItemModificar_Click;
            itemEliminar.Click += ItemEliminar_Click;
            
        }

        private async void ItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            var trago = ((MenuFlyoutItem)sender).Tag;
            Drink tragoAEliminar = (Drink)trago;
            if (tragoAEliminar != null)
            {
                MessageDialog showDialog = new MessageDialog("¿Realmente deseas eliminar ese trago de la lista?");
                showDialog.Commands.Add(new UICommand("Sí") { Id = 0 });
                showDialog.Commands.Add(new UICommand("No") { Id = 1 });
                showDialog.DefaultCommandIndex = 0;
                showDialog.CancelCommandIndex = 1;
                
                var result = await showDialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    //ListDrinkHelper.eliminarDrink(tragoAEliminar);
                    db.deleteTableDrink(tragoAEliminar);
                    LoadAndRefreshListView();
                    Util.notificacionesAlUsuario("App participantes - Confirmacion", "Trago eliminado existosamente.");

                }
            }
        }

        private void ItemModificar_Click(object sender, RoutedEventArgs e)
        {
            var trago = ((MenuFlyoutItem)sender).Tag;
            Drink tragoAModificar = (Drink)trago;
            this.Frame.Navigate(typeof(DrinkEdit), tragoAModificar);
        }


        public void CreateDB()
        {
            //Create DataBase
            db = new DataBase();
            db.CreateDatabase();
            
        }

        private void LoadAndRefreshListView()
        {
            lstSource = db.selectTableDrink();
            ListDrinkHelper.SetList(lstSource);
            lvTragos.ItemsSource = null;
            lvTragos.ItemsSource = ListDrinkHelper.getDrinksByCategory(categoria);          
        }

        private void cbCategorias_Seleccion(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            categoria = cmb.SelectedValue.ToString();
            lvTragos.ItemsSource = null;
            lvTragos.ItemsSource = ListDrinkHelper.getDrinksByCategory(categoria);
        }

        private void btnNuevoTrago_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrinkEdit),null);
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

        private void Refresh()
        {
            //this.Frame.Navigate(typeof(MainPage));
            lvTragos.ItemsSource = null;
            lvTragos.ItemsSource = ListDrinkHelper.getDrinksByCategory(categoria);
        }


    }
}
