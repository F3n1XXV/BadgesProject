using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Media;
using Windows.Graphics.Imaging;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace UwpCamButton
{
    public sealed partial class CamPage : Page
    {
        public MainPage mainPage;
        private RecapitulationPage recapitulationPage;
        private DispatcherTimer dispatcherTimer;
        private VideoFrame videoFrame;
   
        //aktuální čas/odpočet v časovači
        private int _timerTickInt = 0;
        private int timerTickInt
        {   
            set
            { 
                _timerTickInt = value;
                txbTimerCykle.Text = _timerTickInt.ToString();

                if (_timerTickInt==0)
                {
                    txbTimerCykle.Visibility = Visibility.Collapsed;
                    BtnCapture.Visibility = Visibility.Visible;
                }
                else
                {
                    txbTimerCykle.Visibility = Visibility.Visible;
                    BtnCapture.Visibility = Visibility.Collapsed;
                }
            } 
            get
            => _timerTickInt; 
        }

        public CamPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            OpenCamera();
        }

        //funkce, která se, pustí po změně stránky a nastaví parametry
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                //mp = MainPage(e.Parameter);
                //nameRecieved.Text = "Hi " + e.Parameter.ToString();
            }
            else if (e.Parameter is MainPage)
            {
                mainPage = (MainPage)e.Parameter;
            }
            else if (e.Parameter is RecapitulationPage)
            {
                recapitulationPage = (RecapitulationPage)e.Parameter;
            }
            else
            {
                //nameRecieved.Text = "Hi ";
            }
        }
        //tlačítko pro zapnutí kamery(není potřeba, jelikož se aplikace spouští puštěná)
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            OpenCamera();
        }
        //tlačítko pro vypnutí kamery
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            CameraPreview.Stop();
        }

        /// <summary>
        /// spustí webcameru
        /// </summary>
        public async void OpenCamera()
        {
            CameraPreview.PreviewFailed += CPTest_PreviewFailed;
            await CameraPreview.StartAsync();
            CameraPreview.CameraHelper.FrameArrived += CPTest_FrameArrived;

            //zapíše aktivitu do listu
            MainPage.ListActivitiesAdd("Camera", "Start camera");
        }

        private void CPTest_FrameArrived(object sender, FrameEventArgs e)
        {
            videoFrame = e.VideoFrame;
            var softwareBitmap = videoFrame.SoftwareBitmap;
        }
        //v případě, chyby kamery vypíše chybu
        private void CPTest_PreviewFailed(object sender, PreviewFailedEventArgs e)
        {
            //zapíše aktivitu do listu
            MainPage.ListActivitiesAdd("Camera", "Error:" + e.Error.ToString());
        }   
        //tlačítko pro vyfocení
        //spustí časovač, který po doběhnutí vytvoří fotku
        private void BtnCapture_Click(object sender, RoutedEventArgs e)
        {
            timerTickInt = 5;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += timerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            MainPage.ListActivitiesAdd("Camera", "Start timer");
        }
        //časovač pro focení
        private async void timerTick(object sender, object e)
        {
            timerTickInt -= 1;

            if (timerTickInt == 0)
            {
                dispatcherTimer.Stop();
                await startCapture();

                //přepne, stránku
                mainPage.ChangePage("Recapitulation");

                //zapíše aktivitu do listu
                MainPage.ListActivitiesAdd("Camera", "Create photo");
                //mp.ChangePage("EditPhoto");
            }
        }
        //zaznamená obrázek
        private async Task startCapture()
        {
            if (videoFrame!=null)
            {
                SoftwareBitmap softwareBitmap = videoFrame?.SoftwareBitmap;
                // Převeďte pixelový formát na Rgba16, abychom jej mohli uložit do souboru
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Rgba16);

                if (softwareBitmap != null)
                {
                    try
                    {
                        BitmapImage bmpImage2 = new BitmapImage();
                        //Poskytuje náhodný přístup k datům ve vstupních a výstupních streamech, které jsou uloženy v paměti místo na disku.
                        InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
                        {
                            // Vytvořte kodér v požadovaném formátu
                            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                            //encoder.BitmapTransform.Rotation = BitmapRotation.Clockwise90Degrees;
                            // Nastavte bitmapu softwaru
                            encoder.SetSoftwareBitmap(softwareBitmap);
                            await encoder.FlushAsync();


                            await bmpImage2.SetSourceAsync(stream);
                            mainPage.img= bmpImage2;
                        }
                    }
                    catch (Exception ex)
                    {
                        MainPage.ListActivitiesAdd("Camera", "Error:" + ex.ToString());
                    }
                }

                //zapíše aktivitu do listu
                MainPage.ListActivitiesAdd("Camera", "Save photo:" + ApplicationData.Current.LocalCacheFolder.Path.ToString());
            }
            else
            {
                //zapíše aktivitu do listu
                MainPage.ListActivitiesAdd("Camera", "Pokus o vyfocení.Kamera není připojena.");
            }

        }
        //uloží obrázek na disk
        //již tato funkce není potřeba(zachovávám v případě, kdyby byla potřeba)
        //private static async Task<FileUpdateStatus> WriteToStorageFile(SoftwareBitmap bitmap, StorageFile file)
        //{
        //    StorageFile sFile = file;
        //    if (sFile != null)
        //    {
        //        CachedFileManager.DeferUpdates(sFile);

        //        using (var fileStream = await sFile.OpenAsync(FileAccessMode.ReadWrite))
        //        {
        //            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
        //            encoder.SetSoftwareBitmap(bitmap);
        //            await encoder.FlushAsync();
        //        }

        //        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(sFile);
        //        return status;
        //    }
        //    return FileUpdateStatus.Failed;
        //}
    }
}
