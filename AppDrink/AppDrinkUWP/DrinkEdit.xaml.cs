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

        public DrinkEdit()
        {
            this.InitializeComponent();

            //Si no es un edit
            hubTitle.Header = "Agregar trago";

            //Si no existe imagen para ese trago, cargar imagen por defecto
            BitmapImage drinkImageDefault = new BitmapImage(new Uri(this.BaseUri, "/Assets/drink-default.jpg"));
            drinkImageCapture.Source = drinkImageDefault;
        }

        //Abre la cam para tomar foto y la muestra en un Image
        private async void captureBtn_Click(object sender, RoutedEventArgs e)
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

                //VER SI HACERLO ASI O COMO ESTA ARRIBA QUE ES MAS SENCILLO CREO
                /*
                 Call OpenAsync to get a stream from the image file. Call BitmapDecoder.CreateAsync 
                 to get a bitmap decoder for the stream. 
                 Then call GetSoftwareBitmap to get a SoftwareBitmap representation of the image.
                 The Image control requires that the image source be in BGRA8 format with
                 premultiplied alpha or no alpha, so call the static method SoftwareBitmap.Convert
                 to create a new software bitmap with the desired format. 
                 Next, create a new SoftwareBitmapSource object and call SetBitmapAsync 
                 to assign the software bitmap to the source. Finally, set the Image control's 
                 Source property to display the captured photo in the UI.
                 */
                /*
               IRandomAccessStream stream = await storeFile.OpenAsync(FileAccessMode.Read);
               BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
               SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
               SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
               BitmapPixelFormat.Bgra8, 
               BitmapAlphaMode.Premultiplied);

                SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                imageControl.Source = bitmapSource;
               */

                //VER SI HACE FALTA GUARDAR LA FOTO EN DISCO 
                /*
                El StorageFile que contiene la foto capturada recibe un nombre generado dinámicamente y 
                se guarda en la carpeta local de la aplicación. Para organizar mejor las fotos capturadas,
                puede que desee mover el archivo a otra carpeta.
                */
                //Guarda la foto en C:\Users\Dante\AppData\Local\Packages\884cbc0c-e00e-4cf6-994c-9e419a580f86_tn3w70na242fe\LocalState\DrinkPhotoFolder
                StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("DrinkPhotoFolder",
                CreationCollisionOption.OpenIfExists);

                await storeFile.CopyAsync(destinationFolder, "drinkPhoto.jpg", NameCollisionOption.ReplaceExisting);
                await storeFile.DeleteAsync();
            }


        }

        //Next goto savebutton clickevent and writethe followingcode to save the captured image.
        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(etNombre.Text) || string.IsNullOrEmpty(etIngredientes.Text) ||
                string.IsNullOrEmpty(comboBoxCategoria.SelectedItem.ToString()) )
            {

                Util.notificacionesAlUsuario("AppDrink", "No es posible completar la operación. Por favor, complete todos los campos. No pueden quedar campos vacíos.");
            }
            else
            {
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
            }



        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

       
    }
}
