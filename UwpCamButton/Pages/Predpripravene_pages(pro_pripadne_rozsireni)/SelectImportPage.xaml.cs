using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpCamButton
{
    public sealed partial class SelectImportPage : Page
    {
        public MainPage mainPage;
        public readonly List<(string Tag, Type Page, string Name, int Id)> Pages_list = new List<(string Tag, Type Page, string Name, int Id)>
        {
            ("Camera", typeof(CamPage),"LoadCamera",0),
            ("SDcard", typeof(SDcardPage),"LoadDataSDCard",1),
            ("Bluetooth", typeof(BluetoothPage), "LoadDataBluetooth",2),
            ("Gallery", typeof(GalleryPage), "Gallery",3),
        };

        public SelectImportPage()
        {
            this.InitializeComponent();
        }
        //metoda pro výběr možnosti, jak bude obrázek importován pro tisk
        private void btnSelectView_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Type page = Pages_list.Find(x => x.Tag.ToString() == btn.Tag.ToString()).Page;

            if (page != null)
                mainPage.frame1.Navigate(page, this);
        }
    }
}
