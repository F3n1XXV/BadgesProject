using BadgesTerminal.Class;
using System;
using System.Collections.Generic;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpCamButton.Pages
{
    public delegate void txtIpDel(string Ip, MainPage mp);
    public sealed partial class SettingPage : Page
    {
        MainPage mainPage;

        public SettingPage()
        {
            this.InitializeComponent();

            cmbPrintType.SelectedItem = ValueAppLocalSetting.TypePrint;
        }

        private List<string> listTypePrint = new List<string>
        {
        "TCP/IP","Local"
        };


        private void btnSettingInfoPage_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            mainPage.ChangePage(btn.Tag.ToString());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MainPage)
            {
                mainPage = (MainPage)e.Parameter;

                cmbPrintType.SelectedItem = ValueAppLocalSetting.TypePrint;
                txbIp.Text = ValueAppLocalSetting.strIP;
                txbPort.Text = ValueAppLocalSetting.Port.ToString();          
            }
        }

        //uloží proměné do lokální paměti aplikace
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ValueAppLocalSetting.TypePrint = cmbPrintType.SelectedItem.ToString();

            if (cmbPrintType.SelectedItem=="TCP/IP")
            {
                //*port není jako IP, tak přeruší ukládání
                try
                {
                    IPAddress ip = IPAddress.Parse(txbIp.Text);
                    mainPage.IpSetting = ip.ToString();
                }
                catch (Exception)
                { 

                    return;
                }
                //!port není jako IP    , tak přeruší ukládání

                //*port není jako číslo, tak přeruší ukládání
                try
                {
                    mainPage.Port = int.Parse(txbPort.Text);
                }
                catch (Exception)
                {
                    return;
                }
                //!port není jako číslo, tak přeruší ukládání
                ValueAppLocalSetting.strIP =txtIp.ToString();
                ValueAppLocalSetting.Port = int.Parse(txbPort.Text);
            }
        }

        private void cmbPrintType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visibility visibleObj = Visibility.Visible;

            switch (cmbPrintType.SelectedItem.ToString())
            {
                case "Local":
                    visibleObj = Visibility.Collapsed;
                    break;
            }

            if (txtIp != null)
                txtIp.Visibility = visibleObj;

            if (txbIp != null)
                txbIp.Visibility = visibleObj;

            if (txtPort!=null)
                txtPort.Visibility = visibleObj;

            if (txbPort != null)
                txbPort.Visibility = visibleObj;
        }
    }
}
