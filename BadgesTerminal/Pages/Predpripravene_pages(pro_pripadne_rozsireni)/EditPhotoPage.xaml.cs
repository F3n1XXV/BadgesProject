using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace UwpCamButton
{
    public sealed partial class EditPhotoPage : Page
    {
        public EditPhotoPage()
        {
            this.InitializeComponent();
            cmbColors.ItemsSource = typeof(Colors).GetProperties();
            image.Source = new BitmapImage(new Uri(ApplicationData.Current.TemporaryFolder.Path.ToString() + "\\Temporary.png"));
        }

        public void updateText(object sender, TextChangedEventArgs args)
        {
            MainPage.TextDown = txtWriteTextUp.Text;
            MainPage.TextUp = txtWriteTextUp.Text;
            MainPage.TextCenter = txbTextCenter.Text;
        }
    }
}
