using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

namespace UwpCamButton.Class
{
    class SocketClient
    {
        static MainPage mp;
        //komunikační protokol
        //private const int port = 49500;
        delegate void txtIpDel(string Ip, MainPage mp);

        public SocketClient(MainPage _mp)
        {
            mp = _mp;
        }
        
        //ip adresa zařízení
        //public static IPAddress LocalIP
        //{
        //    get
        //    {
        //        ////první IP adresa zařízení
        //        //IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).First(IPA => IPA.AddressFamily == AddressFamily.InterNetwork);
        //        ////vypnutí automatické IP adresy
        //        ////v případě, že chceme nastavit IP adresu natvrdo, stačí zadat údaj natvrdo
        //        //if (mp.IpSetting!="000.000.000.000")
        //        //{
        //        //    ip= IPAddress.Parse(mp.IpSetting);
        //        //}
        //        ////!vypnutí automatické IP adresy

        //        //zapíše IP adresu do delegáta
        //        txtIpDel delTxtIp = new txtIpDel(Pages.SettingPage.ChangeTextIp);
        //        delTxtIp(ip.ToString(), mp);
        //        //!zapíše IP adresu do delegáta

        //        //vrací ip adresu
        //        return ip; 
        //    }
        //}
        //start metody pro poslání obrázku na server s počtem stránek
        public void StartClient(String serverIp,int port, String countPages, Byte[] buffer)
        {
            try
            {
                MainPage.ListActivitiesAdd("Socket", "Ip/Port:" + serverIp +"/"+ port.ToString()) ;
                //vytvoření socketu
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //propojení sokectu
                clientSocket.Connect(serverIp.ToString(), port);
                //převod a odeslání ztárvy na server
                byte[] dataMessange = Encoding.ASCII.GetBytes(countPages);
                clientSocket.Send(dataMessange);
                //!převod a odeslání ztárvy na server

                //prodleva mezi odeslání textu a obrázku
                //bezpečnostní pojistka, z důvodu, kdyby nedařilo odeslat text
                Thread.Sleep(1000);

                //odeslání obrázku
                clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
                //clientSocket.Close();
            }
            catch (ArgumentNullException e)
            {
                MainPage.ListActivitiesAdd("Socket", "ArgumentNullException:" + e);
            }
            catch (SocketException e)
            {
                MainPage.ListActivitiesAdd("Socket", "SocketException:" + e);
            }
            catch (Exception e)
            {
                MainPage.ListActivitiesAdd("Socket", "Exception:" + e);
            }
        }
    }
}
