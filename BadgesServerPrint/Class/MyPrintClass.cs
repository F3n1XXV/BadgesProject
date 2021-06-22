using System.Drawing;
using System.Drawing.Printing;

namespace BadgesServerPrint.Class
{
    class MyPrintClass
    {
        DelDgwAddText DelDgwAdd;

        //hlavní formulář
        private Form1 mainForm { get; set; }
        //dokument k tisku
        private PrintDocument printDocument { get; set; }
        //stažení obrázek ze zařízení
        private Image img { get; set; }

        public MyPrintClass(Form1 fm, Image _img,int countPages)
        {
            this.mainForm = fm;
            DelDgwAdd = new DelDgwAddText(fm.AddDgwAction);

            //* nastavení velikosti obrázku
            // obrázek se zmenšuje/zvětšuje oproti původnímu rozlišení
            int width = (int)(_img.Width/3.2);
            int height = (int)(_img.Height/3.2); //3.25 je 56 mm 3.2 je 57mm
            img = resizeImage(_img, new Size(width, height));
            //! nastavení velikosti obrázku

            //*nastavení print dokumentu
            printDocument = new PrintDocument();
            printDocument.DocumentName = "Button";
            printDocument.PrintPage += SettingPictureOnPage;
            printDocument.BeginPrint += new PrintEventHandler(pd_BeginPrint);
            printDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            printDocument.EndPrint += new PrintEventHandler(pd_EndPrint);
            //!nastavení print dokumentu
            
            settingPage();
            //*vytiskne určitý počet stránek
            for (int i = 0; i < countPages; i++)
            {
                //spustí tisk
                printDocument.Print();
            }
            //!vytiskne určitý počet stránek
        }
        //upraví velikost obrázku
        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        //nastavení umístění obrázku na papíru
        private void SettingPictureOnPage(object o, PrintPageEventArgs e)
        {
            //Image img = Image.FromFile("C:\\UwpCamButtonDemo\\A.jpg");
            //x ,y
            Point loc = new Point(-30, (e.PageBounds.Height- img.Height) / 2);
            e.Graphics.DrawImage(img, loc);
        }
        //nastavení paríru
        private void settingPage()
        {
            //stránky de jsou různé rozlišení papíru
            //https://www.prepressure.com/library/paper-size
            //https://toolstud.io/photo/dpi.php?width=10&width_unit=cm&height=15&height_unit=cm&dpi=360&bleed=0&bleed_unit=mm
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("100 x 150 mm", 822, 1182); //788;758;795
            //pd.DefaultPageSettings.PaperSize = new PaperSize("210 x 297 mm", 800, 800);
            //nastavení orientace pepíru
            printDocument.DefaultPageSettings.Landscape =true;
        }
        //vykoná činnost při začátku tisku
        private void pd_BeginPrint(object beginPrintSender, PrintEventArgs beginPrintE)
        {
            //zapíše vykonávanou činnost do listu 
            mainForm.AddDgwAction("Start printing.");
        }
        //vykoná činnost při ukončení tisku
        private void pd_EndPrint(object sender, PrintEventArgs endPrintE)
        {
                //zapíše vykonávanou činnost do listu 
                mainForm.AddDgwAction("End printing.");
        }
        //Událost PrintPage je vyvolána pro každou stránku, která má být vytištěna.
        private void pd_PrintPage(object printPageSender, PrintPageEventArgs printPageE)
        {
            // write your logic while printing  
            //Bitmap myBitmap1 = new Bitmap(myPicturebox.Width, myPicturebox.Height);
            ////myPicturebox.DrawToBitmap(myBitmap1, new Rectangle(0, 0, myPicturebox.Width, myPicturebox.Height));
            //printPageE.Graphics.DrawImage(myBitmap1, 0, 0);
            //myBitmap1.Dispose();
        }
    }
}
