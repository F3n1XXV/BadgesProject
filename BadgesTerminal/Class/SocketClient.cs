using clEventLoggingUWP;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UwpCamButton.Class
{
    public class SocketClientCamButton
    {
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
                //*zaznamenávání aktivity
                MainPage.ListActivitiesAdd("Socket", "Ip/Port:" + serverIp +"/"+ port.ToString()) ;
                EventLogging.Info(2, "Socket:Ip/Port:" + serverIp + "/" + port.ToString());
                //!zaznamenávání aktivity

                //vytvoření socketu
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //propojení sokectu
                clientSocket.Connect(serverIp.ToString(), port);
                //převod a odeslání zprávy na server
                byte[] dataMessange = Encoding.ASCII.GetBytes(countPages);
                clientSocket.Send(dataMessange);
                //!převod a odeslání zprávy na server

                //prodleva mezi odeslání textu a obrázku
                //bezpečnostní pojistka, z důvodu, kdyby nedařilo odeslat text
                Thread.Sleep(1000);

                //odeslání obrázku
                clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
                clientSocket.Close();
            }
            catch (ArgumentNullException ex)
            {
                MainPage.ListActivitiesAdd("Socket", "ArgumentNullException:" + ex);
                EventLogging.Error(2, "ArgumentNullException:" + ex.ToString());
            }
            catch (SocketException ex)
            {
                MainPage.ListActivitiesAdd("Socket", "SocketException:" + ex);
                EventLogging.Error(3, "SocketException:" + ex.ToString());
            }
            catch (Exception ex)
            {
                MainPage.ListActivitiesAdd("Socket", "Exception:" + ex);
                EventLogging.Error(4, "Exception:" + ex.ToString());
            }
        }
    }
}
