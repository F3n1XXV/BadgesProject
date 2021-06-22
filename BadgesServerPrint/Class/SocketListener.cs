using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Linq;
using BadgesServerPrint.Class;

namespace BadgesServerPrint
{
    class SocketListener
    {
        private DelDgwAddText DelDgwAdd{get; set;}
        private Form1 mainForm{get; set;}
        //MyPrintClass mPrint;
        private Socket hostSocket{get; set;}
        //ovladací bool pro cyklus naslouchání
        public bool BblListen{get;set;}
        //nastaví první lokální IP adresu zařízení 
        public IPAddress LocalIP
        {
            get=> Dns.GetHostAddresses(Dns.GetHostName()).First(IPA=>IPA.AddressFamily==AddressFamily.InterNetwork);
        }

        private int _localPort { get; set; }
        /// <summary>
        ///nastaví preferovaný naslouchací port zařízení
        ///defaultně je nastavený port 49600- (v rozsahu 49152 až 65535, vyhrazené pro dynamické přidělování a soukromé využití, nejsou pevně přiděleny žádné aplikaci)
        /// </summary>
        public int LocalPort
        {
            get => _localPort;
        }

        public SocketListener(Form1 fm)
        {
            BblListen = true;
            this.mainForm = fm;
            //vytvoří delegáta pro předávání informací do datagriedviewru
            DelDgwAdd = new DelDgwAddText(fm.AddDgwAction);
        }
        //naslouchání příchozích zpráv
        public void StartListening(int localPort)
        {
            _localPort = localPort;
            //socket služba
            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //nastavení připojovacího uzlu
            IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse(LocalIP.ToString()), LocalPort);
            //připojovací uzel
            receiveSocket.Bind(hostIpEndPoint);
            //nastaví počet, kolik může být připojených/naslouchaných zařízení
            receiveSocket.Listen(1);
            EventLoging.Info(1, "Start server." + LocalIP.ToString() + ":" + LocalPort.ToString());
            DelDgwAdd("Start server:" + LocalIP.ToString() +":" + LocalPort.ToString());
            

            while (BblListen)
            {
                //čeká na vytvořené připojení
                hostSocket = receiveSocket.Accept();
                //*vytvoří vlákno na naslouchání
                Thread thread = new Thread(new ThreadStart(threadImage));
                thread.IsBackground = true;
                thread.Start();
                //!vytvoří vlákno na naslouchání
            }
        }
        //čtení obrázku
        private void threadImage()
        {
            try
            {
                //nastaví po jak dlouhé době má přestat naslouchání
                hostSocket.ReceiveTimeout=1000;
                //*naslouchání od klienta
                //prvně naslouchá string(počet stránek)
                byte[] m = new byte[17];// velikost chartu se udává podle poštu znaků ((počet znaků*7bytů)+10)
                int dataSizeMessangeClient = hostSocket.Receive(m, 0, m.Length, SocketFlags.None);
                //přeloží byty do stringu
                string messangeClient = Encoding.ASCII.GetString(m, 0, dataSizeMessangeClient);
                //přeloží string do čísla
                int countPagesPrint = int.Parse(messangeClient);
                //!naslouchání od klienta

                //*naslouchání od klienta
                //nastavení maximální velikosti příchozí zprávy
                byte[] c = new byte[1024*1024*2];
                //velikost naslouchaného souboru(obrázku)
                int dataSize = dataSize = hostSocket.Receive(c, 0, c.Length, SocketFlags.None);
                
                if (dataSize > 0)
                {
                    //přeložení bytů do memory streamu
                    MemoryStream ms = new MemoryStream(c, 0, dataSize, true);
                    //přeloží paměť bytů do obrázku
                    Image img = Image.FromStream(ms);

                    ms.Close();
                    DelDgwAdd("Load image.");
                  
                    EventLoging.Info(2, "Load image.");
                    MyPrintClass mPrint = new MyPrintClass(mainForm,img, countPagesPrint);
                    DelDgwAdd("Start print.");
                    EventLoging.Info(3, "Start print.");
                }
                //!naslouchání od klienta
            }
            catch (Exception ex)
            {
                DelDgwAdd("Error:"+ ex);

                EventLoging.Error(1, ex.ToString());
            }
        }
    }
}
