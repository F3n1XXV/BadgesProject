using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpCamButton.Class;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpCamButton.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public delegate void txtIpDel(string Ip, MainPage mp);
    public sealed partial class SettingPage : Page
    {
        //txtIpDel delTxtIp = new txtIpDel(ChangeTextIp);
        public string IpSetting = "000.000.000.000";
        MainPage mp;

        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void btnIp_Click(object sender, RoutedEventArgs e)
        {
            //IpSetting = txtIp.Text;
        }

        static public void ChangeTextIp(string ip, MainPage mp)
        {
            //IpSetting = txtIp.Text;
        }

        private void btnSettingInfoPage_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            mp.ChangePage(btn.Tag.ToString());
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
                //txtIp.Text = "IP:(" + mp.IpSetting + ")";
                //txtPort.Text = "Port:(" + mp.Port + ")";

                txbPort.Text = mp.Port.ToString();
                txbIp.Text = mp.IpSetting;
            }

        }

        private void txbPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mp.Port = int.Parse(txbPort.Text);
            }
            catch
            {
            }
        }

        private void txbIp_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(txbIp.Text);
                mp.IpSetting = ip.ToString();
            }
            catch 
            {
            }
        }
    }
}
