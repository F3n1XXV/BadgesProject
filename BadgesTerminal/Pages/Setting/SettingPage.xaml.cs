using System;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpCamButton.Pages
{
    public delegate void txtIpDel(string Ip, MainPage mp);
    public sealed partial class SettingPage : Page
    {
        //txtIpDel delTxtIp = new txtIpDel(ChangeTextIp);
        public string IpSetting = "000.000.000.000";
        MainPage mainPage;

        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void btnIp_Click(object sender, RoutedEventArgs e)
        {
            //IpSetting = txtIp.Text;
        }

        public static void ChangeTextIp(string ip, MainPage mp)
        {
            //IpSetting = txtIp.Text;
        }

        private void btnSettingInfoPage_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            mainPage.ChangePage(btn.Tag.ToString());
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
                mainPage = (MainPage)e.Parameter;
                //txtIp.Text = "IP:(" + mp.IpSetting + ")";
                //txtPort.Text = "Port:(" + mp.Port + ")";

                txbPort.Text = mainPage.Port.ToString();
                txbIp.Text = mainPage.IpSetting;
            }

        }

        private void txbPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mainPage.Port = int.Parse(txbPort.Text);
            }
            catch (Exception)
            {}
        }

        private void txbIp_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(txbIp.Text);
                mainPage.IpSetting = ip.ToString();
            }
            catch (Exception)
            {}
        }
    }
}
