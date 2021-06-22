using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace UwpCamButton.Pages.Setting
{
    public sealed partial class SettingInfoPage : Page
    {
        private List<Activity> listActivities2 = MainPage.ListActivities;
        public SettingInfoPage()
        {
            InitializeComponent();
        }
    }
}
