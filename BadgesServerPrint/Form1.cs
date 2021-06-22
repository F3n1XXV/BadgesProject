using System;
using System.Threading.Tasks;
using System.Windows.Forms;

//delegát pro zápis aktivit, ze všech vláken
delegate void DelDgwAddText(string textAddDgw);

namespace BadgesServerPrint
{
    public partial class Form1 : Form
    {
        //DelDgwAddText DelDgwAdd;
        const int portServer = 49600;
        const int countRowsDgw = 100;
        private SocketListener server;
        //nastaví maximální počet řádků v dgw

        private Task t1;
        public Form1()
        {
            InitializeComponent();

            //*vytvořá server a předá požadovaný parametr a následně pustí v jiném vlákně naslouchání
            server = new SocketListener(this);
            
            t1 = new Task(()=>server.StartListening (portServer));
            t1.Start();
            //!vytvořá server a předá požadovaný parametr a následně pustí v jiném vlákně naslouchání
        }
        //metoda při spuštění formuláře
        private void Form1_Load(object sender, EventArgs e)
        {
            //vypíše používaný IP a Port
            lblConnect.Text= "Info connect = Type: TCP / IP; IP: " + server.LocalIP.ToString() + "; Port: " + server.LocalPort;
            lblVersion.Text = "Version:" + Application.ProductVersion; 
        }

        /// <summary>
        ///zapíše záznam do datagridViewru
        /// </summary>
        public void AddDgwAction(string Description)
        {
            //*Spustí delegáta asynchronně ve vlákně, v němž byl vytvořen příslušný popisovač ovládacího prvku.
            this.BeginInvoke((Action)delegate ()
            {
                //*v případě, že v datagridviewru je více záznamů než je požadování, tak smaže první záznam
                if (dataGridView1.RowCount > countRowsDgw)
                    dataGridView1.Rows.RemoveAt(0);
                //!v případě, že v datagridviewru je více záznamů než je požadování, tak smaže první záznam

                //zapíše nový záznam do datagridviewru
                dataGridView1.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.sss"), Description);
            });
            //!Spustí delegáta asynchronně ve vlákně, v němž byl vytvořen příslušný popisovač ovládacího prvku.
        }

        //v případě ukončení programu vypne naslouchání a hlavní vlákno naslouchcího socketu
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.BblListen = false ;
            t1.Dispose();
        }
    }
}
