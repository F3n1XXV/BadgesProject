using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BadgesTerminal.Dialog
{
    public sealed partial class Password_Dialog : ContentDialog
    {
        public string StrPassword;
  
        public Password_Dialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            StrPassword = psb.Password.ToString();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


        private void psb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    StrPassword = psb.ToString();

                    ContentDialog_PrimaryButtonClick(null,null);
                    Hide();
                    break;
                case VirtualKey.Escape:
                 
                    ContentDialog_SecondaryButtonClick(null,null);
                    Hide();
                    break;
                default:
                    base.OnKeyUp(e);
                    break;
            }
        }

       
    }
}
