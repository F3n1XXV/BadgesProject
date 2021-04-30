using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UwpCamButton.Pages.Setting;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Sockets;
using System.Net;
using System.Linq;

namespace UwpCamButton
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        //static public SoftwareBitmap SoftwareBitmapPrint;
        public BitmapImage img = new BitmapImage();
        // text, který by se měl zobrazit na oznáčku
        static public string TextUp;
        static public string TextCenter;
        static public string TextDown;
        //!text, který by se měl zobrazit na oznáčku
        //nastavení obrázku pro fotku
        static private string _lastNamePicture;
        //v případě, že je vyžadováno posouvat obrázek, tak zde jsou souřadnice posunutí
        static public int TransferX;
        static public int TransferY;
        //!nastavení obrázku pro fotku
        public string IpSetting;
        public int Port = 49600;

        static public string LastNamePicture
        {
            get
            {
                return _lastNamePicture;
            }
            set
            {
                string fullPath = ApplicationData.Current.LocalCacheFolder.Path.ToString() + "\\" + _lastNamePicture;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _lastNamePicture = value;
            }
        }

        static public List<Activity> ListActivities = new List<Activity>();
        static public List<string> listLanguage = new List<string>()
        {
            "Czech","English","Deutch"
        };

        static public void ListActivitiesAdd(string name,string description)
        {
            ListActivities.RemoveAll(x=> x.DateAction<DateTime.Now.AddHours(-1));
                ListActivities.Add(new Activity(name, description));
        }

        static public readonly List<(string Tag, Type Page, string Name, int Id)> ListPages = new List<(string Tag, Type Page, string Name, int Id)>
        {
            //("Camera", typeof(CamPage),"LoadCamera",0),
            //("SDcard", typeof(SDcardPage),"LoadDataSDCard",1),
            ("SelectionImport", typeof(CamPage), "SelectionImport",2),
            ("EditPhoto", typeof(EditPhotoPage), "EditPhoto",3),
            ("Recapitulation", typeof(RecapitulationPage), "Recapitulation",4),
            ("Info", typeof(InfoPage), "Info",5),
            ("Tutorial", typeof(InfoPage), "Tutorial",6),
            //("SettingPrint", typeof(Pages.SettingPage), "Setting",6),
            ("SettingInfo", typeof(SettingInfoPage), "SettingInfo",7),
            ("Setting", typeof(Pages.SettingPage), "Setting",8) 
        };


        public MainPage()
        {
            this.InitializeComponent();
            //vybere první defaultní jazyk
            cmbLanguage.SelectedIndex = 1;
            
            //při startu nastaví předefinovanou stránku
            ChangePage("SelectionImport");
            IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).First(IPA => IPA.AddressFamily == AddressFamily.InterNetwork);
            IpSetting = ip.ToString();
        }

        //funkce pro přepínání stránek
        private void btnSelectView_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ChangePage(btn.Tag.ToString());
        }
        //funkce pro přepínání stránek
        public async void ChangePage(string NextPage)
        {
            //Type page = Pages_list.Find(x => x.Tag.ToString() == NextPage).Page;
            //changePage(NextPage);

            if (NextPage== "Setting")
            {
                string pasword= await InputTextDialogAsync("Password");
                if (pasword == "button")
                {
                    Type page = ListPages.Find(x => x.Tag.ToString() == NextPage).Page;
                    frame1.Navigate(page, this);
                }
            }
            else
            {
                Type page = ListPages.Find(x => x.Tag.ToString() == NextPage).Page;
                frame1.Navigate(page, this);
            }
        }

        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            PasswordDialog dialog = new PasswordDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }


        //nastavení lazyku
        // prozatím tato funkce není dokončena
        private void ChangeLanguage(object sender, SelectionChangedEventArgs e)
        {
            var newlySelected = e.AddedItems[0] as ComboBoxItem;
            string newLanguage = newlySelected.Content.ToString();
            switch (newLanguage)
            {
                case "Czech":
                    {
                        //CultureInfo.CurrentCulture = new CultureInfo("en");
                        //CultureInfo.CurrentUICulture = new CultureInfo("en");
                        break;
                    }
                case "English":
                    {
                        //CultureInfo.CurrentCulture = new CultureInfo("es");
                        //CultureInfo.CurrentUICulture = new CultureInfo("es");
                        break;
                    }
                case "Deutch":
                    {
                        //CultureInfo.CurrentCulture = new CultureInfo("fr");
                        //CultureInfo.CurrentUICulture = new CultureInfo("fr");
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Unidentified Language");
                    }
            }
        }

    }

}
