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
        private StorageFile storeFile;
        private IRandomAccessStream stream;
        string drinkPhotoPath="default";
        DataBase db;

        public DrinkEdit()
        {
            this.InitializeComponent();
            CreateDB();
            Categories cat = new Categories();
            comboBoxCategoria.ItemsSource = cat.categoryList;
            
            //Si no es un edit
            hubTitle.Header = "Agregar trago";

            //Si no existe imagen para ese trago, cargar imagen por defecto
            BitmapImage drinkImageDefault = new BitmapImage(new Uri(this.BaseUri, "/Assets/drinkDefault.jpg"));
            drinkImageCapture.Source = drinkImageDefault;
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
                    stream = await storeFile.OpenAsync(FileAccessMode.Read); ;
                    bimage.SetSource(stream);
                    drinkImageCapture.Source = bimage;

                    
                    StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("DrinkPhotoFolder",
                    CreationCollisionOption.OpenIfExists);
                    
                    await storeFile.CopyAsync(destinationFolder, nombreTrago + ".jpg", NameCollisionOption.GenerateUniqueName);
                    // await storeFile.CopyAsync(destinationFolder, "drinkPhoto.jpg", NameCollisionOption.ReplaceExisting);
                    //await storeFile.DeleteAsync();

                //VER CON QUE PATH  ABRIR LOS ARCHIVOS
                    //string prueba=storeFile.Path;
                    
                }
            
            }


        }

        //Next goto savebutton clickevent and writethe followingcode to save the captured image.
        private  void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(etNombre.Text) || string.IsNullOrEmpty(etIngredientes.Text) ||
                string.IsNullOrEmpty(comboBoxCategoria.SelectedItem.ToString()) || string.IsNullOrEmpty(etPrecio.Text))
            {

                Util.notificacionesAlUsuario("AppDrink", "No es posible completar la operación. Por favor, complete todos los campos. No pueden quedar campos vacíos.");
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
                }*/

                Drink newDrink = new Drink()
                {
                    nombre = etNombre.Text,
                    categoria = comboBoxCategoria.SelectedItem.ToString(),
                    ingredientes = etIngredientes.Text,
                    precio = etPrecio.Text,
                    imagePath = drinkPhotoPath  //modificar esa variable cuando el path de la foto en disco
                };
                db.insertIntoTableDrink(newDrink);
                
            }
            this.Frame.Navigate(typeof(MainPage));


        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

       
    }
}
