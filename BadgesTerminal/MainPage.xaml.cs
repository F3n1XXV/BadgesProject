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
using clEventLoggingUWP;
using BadgesTerminal.Dialog;

namespace UwpCamButton
{
    public sealed partial class MainPage : Page
    {
        //static public SoftwareBitmap SoftwareBitmapPrint;
        public BitmapImage img = new BitmapImage();
        //*text, který by se měl zobrazit na oznáčku
        public static string TextUp;
        public static string TextCenter;
        public static string TextDown;
        //!text, který by se měl zobrazit na oznáčku
        //*nastavení obrázku pro fotku
        private static string _lastNamePicture;
        //v případě, že je vyžadováno posouvat obrázek, tak zde jsou souřadnice posunutí
        public static int TransferX;
        public static int TransferY;
        //!nastavení obrázku pro fotku
        public string IpSetting;
        public int Port = 49600;

        private EventLogging eventLog = new EventLogging("ButtonTerminal");

        public static string LastNamePicture
        {
            get=> _lastNamePicture;
            set
            {
                string fullPath = ApplicationData.Current.LocalCacheFolder.Path.ToString() + "\\" + _lastNamePicture;

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                _lastNamePicture = value;
            }
        }

        public static List<Activity> ListActivities = new List<Activity>();
        public static List<string> listLanguage = new List<string>()
        {
            "Czech","English","Deutch"
        };
        /// <summary>
        /// Přidá jednu novou aktivitu a zároveň,Smaže zaznamy activit, které jsou starší jak jedna hodina.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public static void ListActivitiesAdd(string name,string description)
        {
            ListActivities.RemoveAll(x=> x.DateAction<DateTime.Now.AddHours(-1));
            ListActivities.Add(new Activity(name, description));
        }

        public static readonly List<(string Tag, Type Page, string Name, int Id)> ListPages = new List<(string Tag, Type Page, string Name, int Id)>
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
            InitializeComponent();
            EventLogging.Info(1,"Start aplication");
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
        /// <summary>
        /// funkce pro přepínání stránek
        /// stačí zadat název stránky a procedura najde potřebnou stránku v obj. this.ListPages 
        /// </summary>
        /// <param name="NextPage"></param>
        public async void ChangePage(string NextPage)
        {
            if (NextPage== "Setting")
            {
                string psw = await InputTextDialogAsync();
                //*nastavení hesla z důvodu, aby se na danou stranku nedostal kde kdo
                if (psw == "button")
                {
                    Type page = ListPages.Find(x => x.Tag.ToString() == NextPage).Page;
                    frame1.Navigate(page, this);
                }
                //!nastavení hesla z důvodu, aby se na danou stranku nedostal kde kdo
            }
            else
            {
                Type page = ListPages.Find(x => x.Tag.ToString() == NextPage).Page;
                frame1.Navigate(page, this);
            }
        }
        /// <summary>
        /// dialog okno pro vkládání hesla
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        /// 
        //string pasword = await InputTextDialogAsync("Password");
        private async Task<string> InputTextDialogAsync()
        {
           
            Password_Dialog dialog = new Password_Dialog();
            //dialog.Content = inputTextBox;
            //dialog.Title = title;
            //dialog.IsSecondaryButtonEnabled = true;
            //dialog.PrimaryButtonText = "Ok";
            //dialog.SecondaryButtonText = "Cancel";

            await dialog.ShowAsync() ;


            return dialog.StrPassword;
        }

        //nastavení jazyku
        //prozatím tato funkce není dokončena
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
