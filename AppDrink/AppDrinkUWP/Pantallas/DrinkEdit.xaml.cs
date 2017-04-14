using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;            //To use the camera capture UI
using Windows.Storage;                  //To do file operations with the returned image file
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
//using Windows.Graphics.Imaging;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AppDrinkProyectoCompartido;
using AppDrinkUWP.Classes;
using AppDrinkUWP.DataHelper;
using Microsoft.VisualBasic;
using System.Windows.Input;

// Package.appxmanifest -> View Designer -> Capabilities -> WebCam y Pictures Library
/*
 * First we need to enable the following capability’s “Pictures Library” and “Webcam” 
 * in the Package.appxmanifest file like the following image.
 * Next we need to add a one Button to capture Image and an Image control to show
 *  the captured image. Then we will save the image in the location of the current system.
 * Now go to MainPage.xaml and design your UI according to your wish here 
 * I create application bar with two buttons one is for capture image and one 
 * is for save the image. Create one image control to preview the captured image.
 */

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppDrinkUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    
    /// </summary>
    public sealed partial class DrinkEdit : Page
    {
        StorageFile storeFile;
        IRandomAccessStream stream;
        string drinkPhotoPath="default";
        DataBase db;
        private bool esAlta, tomoFoto=false;
        private Drink trago;

        public DrinkEdit()
        {
            this.InitializeComponent();
            CreateDB();
            Categories cat = new Categories();
            comboBoxCategoria.ItemsSource = cat.categoryList;
            /*
            if (esAlta){
                //Si no existe imagen para ese trago, cargar imagen por defecto
                BitmapImage drinkImageDefault = new BitmapImage(new Uri(this.BaseUri, "/Assets/drinkDefault.jpg"));
                drinkImageCapture.Source = drinkImageDefault;
                
            }*/
            
        }

        public void CreateDB()
        {
            //Create DataBase
            db = new DataBase();
            db.CreateDatabase();
            
        }

        //Abre la cam para tomar foto y la muestra en un Image
        private async void captureBtn_Click(object sender, RoutedEventArgs e)
        {
            
        string nombreTrago = etNombre.Text;
            if ( string.IsNullOrEmpty(nombreTrago))
            {
                Util.notificacionesAlUsuario("AppDrink", "No es posible completar la operación. Debe completar el campo nombre antes de poder tomar una foto del trago.");
            }
            else
            {
                //CameraCaptureUI class to capture photos or videos using the camera UI built into Windows. 
                CameraCaptureUI capture = new CameraCaptureUI();
                /* To capture a photo, create a new CameraCaptureUI object. 
                 * Using the object's PhotoSettings property you can specify properties 
                 * for the returned photo such as the image format of the photo. 
                 * By default, the camera capture UI allows the user to crop (recortar)
                 *  the photo before it is returned, although this can be disabled with the AllowCropping
                 *  property. 
                 * This example sets the CroppedSizeInPixels to request that the returned image be 200 x 200 in pixels.
                */
                capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                capture.PhotoSettings.CroppedAspectRatio = new Size(3, 5);
                //capture.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
                capture.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

                /*
                 Call CaptureFileAsync and specify CameraCaptureUIMode.Photo to specify that a photo should be 
                 captured. The method returns a StorageFile instance containing the image if the capture 
                 is successful. If the user cancels the capture, the returned object is null.
                 */
                storeFile = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

                if (storeFile != null)
                {
                    BitmapImage bimage = new BitmapImage();
                    stream = await storeFile.OpenAsync(FileAccessMode.Read); 
                    bimage.SetSource(stream);
                    drinkImageCapture.Source = bimage;
                    tomoFoto = true;


                    
                    /*
                    StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("DrinkPhotoFolder",
                    CreationCollisionOption.OpenIfExists);
                    
                    await storeFile.CopyAsync(destinationFolder, nombreTrago + ".jpg", NameCollisionOption.GenerateUniqueName);
                    //await storeFile.CopyAsync(destinationFolder, "drinkPhoto.jpg", NameCollisionOption.ReplaceExisting);
                    //await storeFile.DeleteAsync();

                */

                    //VER CON QUE PATH  ABRIR LOS ARCHIVOS
                    //drinkPhotoPath=storeFile.Path;

                }

            }


        }

        //Next goto savebutton clickevent and writethe followingcode to save the captured image.
        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {


            if (string.IsNullOrEmpty(etNombre.Text) || string.IsNullOrEmpty(etIngredientes.Text) ||
                string.IsNullOrEmpty(comboBoxCategoria.SelectedItem.ToString()) || string.IsNullOrEmpty(etPrecio.Text))
            {
                Util.notificacionesAlUsuario("AppDrink", "No es posible completar la operación. Por favor, complete todos los campos. No pueden quedar campos vacíos.");
            }
            else
            {
                int n;
                if (!int.TryParse(etPrecio.Text, out n))
                {
                    Util.notificacionesAlUsuario("AppDrink", "El Precio debe ser un valor numérico");
                }
                else
                {
                    
                    if (tomoFoto)
                    {
                        StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("DrinkPhotoFolder",
                    CreationCollisionOption.OpenIfExists);
                        string nameFile= etNombre.Text;
                        await storeFile.CopyAsync(destinationFolder, nameFile + ".jpg", NameCollisionOption.ReplaceExisting);                        
                        await storeFile.DeleteAsync();

                        drinkPhotoPath = destinationFolder.Path + "\\" + nameFile + ".jpg";
                        //drinkPhotoPath = storeFile.Path;
                        
                    }

                    if (!esAlta)
                    {
                        //Es un edit, no creo un nuevo trago

                        trago = (Drink)DataContext;
                        trago.nombre = etNombre.Text;
                        trago.ingredientes = etIngredientes.Text;
                        trago.precio = etPrecio.Text;
                        trago.categoria = comboBoxCategoria.SelectedItem.ToString();

                        
                        //drinkPhotoPath puede ser modificado si se toma una foto
                        trago.imagePath=drinkPhotoPath;

                        db.updateTableDrink(trago);

                    }
                    else
                    {
                        /*
                    try
                    {
                        FileSavePicker fs = new FileSavePicker();

                        fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
                            
                        fs.DefaultFileExtension = ".jpeg";
                        fs.SuggestedFileName = "Image" + DateTime.Today.ToString();
                        fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                        fs.SuggestedSaveFile = storeFile;
                        // Saving the file 
                        var s = await fs.PickSaveFileAsync();
                        if (s != null)
                        {
                            using (var dataReader = new DataReader(stream.GetInputStreamAt(0)))
                            {
                                await dataReader.LoadAsync((uint)stream.Size);
                                byte[] buffer = new byte[(int)stream.Size];
                                dataReader.ReadBytes(buffer);
                                await FileIO.WriteBytesAsync(s, buffer);
                            }
                        }                  

                    }
                    catch (Exception ex)
                    {
                        var messageDialog = new MessageDialog("Unable to save now. " + ex.Message);
                        await messageDialog.ShowAsync();
                    }
                    */
                        Drink newDrink = new Drink()
                        {
                            nombre = etNombre.Text,
                            categoria = comboBoxCategoria.SelectedItem.ToString(),
                            ingredientes = etIngredientes.Text,
                            precio = etPrecio.Text,
                            imagePath = drinkPhotoPath  //modificar esa variable si se toma una foto(guardar el path del jpg en disco)
                        };
                        db.insertIntoTableDrink(newDrink);
                    }
                    this.Frame.Navigate(typeof(MainPage));
                }

            }


        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //Then on the destination page you need to override OnNavigatedTo and get the parameter.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)  //es un edit
            {

                hubTitle.Header = "Modificar trago";

                //Como es un edit/modificacion, lleno los text box con la información ya almacenada
                //del participante que se selecciono

                DataContext = (Drink)e.Parameter;
                trago = (Drink)DataContext;

                esAlta = false;

                LoadImageDrinkOnImageView(trago.imagePath);
                //CON DataContext y Binding me ahorro de hacer todo esto de aca abajo
                /*
                p = (SharedProject2.Participante)e.Parameter;
                etNombre.Text = p.nombre;
                etApellido.Text = p.apellido;
                etMail.Text = p.email;
                */

            }
            else
            {
                hubTitle.Header = "Agregar trago";
                esAlta = true;
                BitmapImage drinkImageDefault = new BitmapImage(new Uri(this.BaseUri, "/Assets/drinkDefault.jpg"));
                drinkImageCapture.Source = drinkImageDefault;
            }

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
