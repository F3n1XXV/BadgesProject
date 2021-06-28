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
            int selectIndex = cmbPrintType.SelectedIndex;
            ValueAppLocalSetting.TypePrint = listTypePrint[selectIndex].ToString();

            if (listTypePrint[selectIndex].ToString() == "TCP/IP")
            {
                //*port není jako IP, tak přeruší ukládání
                IPAddress ip;
                try
                {
                    ip = IPAddress.Parse(txbIp.Text);
                }
                catch (Exception)
                { 

                    return;
                }
                //!port není jako IP    , tak přeruší ukládání

                //*port není jako číslo, tak přeruší ukládání
                int port;
                try
                {
                    port = int.Parse(txbPort.Text);
                }
                catch (Exception)
                {
                    return;
                }
                //!port není jako číslo, tak přeruší ukládání
                ValueAppLocalSetting.strIP =ip.ToString();
                ValueAppLocalSetting.Port = port;
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
