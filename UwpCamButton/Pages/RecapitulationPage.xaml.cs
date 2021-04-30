using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
//using Windows.Graphics.Printing;
//using Windows.Media.Casting;
//using Windows.UI.Xaml.Printing;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

//using RawPrint;
using Windows.Graphics.Display;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using UwpCamButton.Class;
using Windows.UI.Core;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.Storage.Streams;
//using System.IO;
//using System.Drawing;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace UwpCamButton
{
    public sealed partial class RecapitulationPage : Page
    {
        BindingList<string> typeThings = new BindingList<string>();
        BindingList<string> countThings = new BindingList<string>();
        public MainPage mp;

        public RecapitulationPage()
        {
            this.InitializeComponent();
            //při přepínání stránek si pamatuje nastavené parametry (pokud je Enabled)
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Window.Current.Activated += Current_Activated;

            typeThings.Add("Magnet");
            //typeThings.Add("Oznak");

            countThings.Add("1");
            countThings.Add("2");
            countThings.Add("3");
            countThings.Add("4");
            countThings.Add("5");

            cmbType.SelectedIndex = 0;
            cmbCount.SelectedIndex = 0;

            UnregisterForPrinting();
            RegisterForPrinting();
            //printMan = PrintManager.GetForCurrentView();
            //printMan.PrintTaskRequested += PrintMan_PrintTaskRequested;

            //printDoc = new PrintDocument();
            //printDocSource = printDoc.DocumentSource;
            //printDoc.Paginate += PrintDoc_Paginate;
            //printDoc.GetPreviewPage += PrintDoc_GetPreviewPage;
            //printDoc.AddPages += PrintDoc_AddPages;

        }

        protected void RegisterForPrinting()
        {
                printMan = PrintManager.GetForCurrentView();
                printMan.PrintTaskRequested += PrintMan_PrintTaskRequested;
 
                printDoc = new PrintDocument();
                printDocSource = printDoc.DocumentSource;
                printDoc.Paginate += PrintDoc_Paginate;
                printDoc.GetPreviewPage += PrintDoc_GetPreviewPage;
                printDoc.AddPages += PrintDoc_AddPages;
        }

        protected void UnregisterForPrinting()
        {
            if (printMan == null)
                return;

            printMan.PrintTaskRequested -= PrintMan_PrintTaskRequested;
             if (printDoc == null)
                return;
   
            //printDocSource = printDoc.DocumentSource;
            printDoc.Paginate -= PrintDoc_Paginate;
            printDoc.GetPreviewPage -= PrintDoc_GetPreviewPage;
            printDoc.AddPages -= PrintDoc_AddPages;
        }

        private void loadImage()
        {
            PictureSave.Source = mp.img;
        }
        // toto funguje při activaci page
        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            //MainPage.ListActivitiesAdd("Page", "test3");
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                // do stuff
            }
            else
            {
                // do different stuff
            }
        }
        //vrátí stránku na stránku camery
        private void BtnCapture_Click(object sender, RoutedEventArgs e)
        {
            MainPage.ListActivitiesAdd("Camera", "Back page");
            mp.ChangePage("SelectionImport");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                //mp = MainPage(e.Parameter);
                //nameRecieved.Text = "Hi " + e.Parameter.ToString();
            }
            else if (e.Parameter is MainPage)
            {
                mp = (MainPage)e.Parameter;
            }

            loadImage();


        }
        //pošle obrázek na tiskárnu
        private async void printImage_click(object sender, RoutedEventArgs e)
        {

            //tuto funkci bych chtěl rozeběhnout
            //BtnPrint.IsEnabled = false;
            //btnBack.IsEnabled = false;

            //RenderTargetBitmap rtb = new RenderTargetBitmap();
            //await rtb.RenderAsync(SelectPrint);

            //byte[] bytes = await ImageToBytes(rtb);

            ////byte[] bytes = await ImageToBytes(image);
            //SocketClient socketTcp = new SocketClient(mp);
            //socketTcp.StartClient("192.168.0.107", cmbCount.SelectedItem.ToString(), bytes);

            //MainPage.ListActivitiesAdd("Camera", "Back page");

            ////PictureSave.Source = null;
            //mp.ChangePage("SelectionImport");
            //!tuto funkci bych chtěl rozeběhnout
            IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).First(IPA => IPA.AddressFamily == AddressFamily.InterNetwork);
            if (mp.IpSetting!= ip.ToString())
            {
                BtnPrint.IsEnabled = false;
                btnBack.IsEnabled = false;

                RenderTargetBitmap rtb = new RenderTargetBitmap();
                await rtb.RenderAsync(SelectPrint);

                var pixelBuffer = await rtb.GetPixelsAsync();
                var pixels = pixelBuffer.ToArray();
                var displayInformation = DisplayInformation.GetForCurrentView();
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImage" + ".jpg", CreationCollisionOption.ReplaceExisting);

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Ignore,
                                         (uint)rtb.PixelWidth,
                                         (uint)rtb.PixelHeight,
                                         displayInformation.RawDpiX,
                                         displayInformation.RawDpiY,
                                         pixels);

                    await encoder.FlushAsync();
                }

                SocketClient socketTcp = new SocketClient(mp);

                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] bytes = buffer.ToArray();

                socketTcp.StartClient(mp.IpSetting,mp.Port, cmbCount.SelectedItem.ToString(), bytes);

                MainPage.ListActivitiesAdd("Camera", "Back page");

                mp.ChangePage("SelectionImport");

            }
            else
            {
                if (PrintManager.IsSupported())
                {
              
                    await PrintManager.ShowPrintUIAsync();
                
                    //await Windows.Graphics.Printing.PrintManager.P;
                }                    
            }
           


           

        }
        //převede obrázek do bytu
        public async static Task<byte[]> ImageToBytes(BitmapImage image)
        {
            try
            {
                RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(image.UriSource);
                IRandomAccessStreamWithContentType streamWithContent = await streamRef.OpenReadAsync();
                byte[] buffer = new byte[streamWithContent.Size];
                await streamWithContent.ReadAsync(buffer.AsBuffer(), (uint)streamWithContent.Size, InputStreamOptions.None);
                return buffer;
            }
            catch (Exception ex)
            {
                MainPage.ListActivitiesAdd("Recapitulation", "Error:" + ex.ToString());
                return null;
            }
           
        }
        //převede byte do image
        public async static Task<BitmapImage> ImageFromBytes(byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }
        //vypočitá cenu oznáčků či magnetků
        private void calculatePrice(object sender, RoutedEventArgs e)
        {
            //přednastavená cena odznáčků
            int price=70;
            //pokud je nastavený magnet, tak nastaví cenu
            if ((cmbType.SelectedItem?.ToString() ?? "") == "Magnet")
            {price = 50;}

            //převede text v comboboxu na číslo(počet navolených oznáčků)
            int count = int.Parse(cmbCount.SelectedItem.ToString() );
            //vypíše vypočítanou cenu
            txbPriceVal.Text = (count*price).ToString() + " Kč";
        }

        #region Printer

        private PrintDocument printDoc;
        private PrintManager printMan;
        private IPrintDocumentSource printDocSource;

        private void PrintDoc_Paginate(object sender, PaginateEventArgs e)
        {
            printDoc.SetPreviewPageCount(int.Parse(cmbCount.SelectedItem.ToString()), PreviewPageCountType.Final);

        }

        private void PrintDoc_AddPages(object sender, AddPagesEventArgs e)
        {
            printDoc.AddPage(SelectPrint);
            printDoc.AddPagesComplete();

        }

        private void PrintDoc_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            printDoc.SetPreviewPage(e.PageNumber, SelectPrint);

            //printDoc.SetPreviewPage(e.PageNumber,BtnPrint);

        }

        private void PrintMan_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var printTask = args.Request.CreatePrintTask("Print completed", PrintTaskSourceRequested);
            printTask.Completed += PrintTask_Completed;
        }

        private void PrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
        {
            args.SetSource(printDocSource);
        }

        private void PrintTask_Completed(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            //Notifz user that printing has completed

            //UnregisterForPrinting();
        }

        #endregion
    }
}
