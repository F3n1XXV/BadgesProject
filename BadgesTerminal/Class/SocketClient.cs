using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UwpCamButton.Class
{
    public class SocketClientCamButton
    {
        static private MainPage _mainPage { get; set; }
        //komunikační protokol
        //private const int port = 49500;
        delegate void txtIpDel(string Ip, MainPage mp);

        public SocketClientCamButton(MainPage mainPage)
        {
            _mainPage = mainPage;
        }

        /// <summary>
        ///start metody pro poslání obrázku na server s počtem stránek
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        /// <param name="countPages"></param>
        /// <param name="buffer"></param>
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
