using Windows.Storage;

namespace BadgesTerminal.Class
{
    class ValueAppLocalSetting
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        static public string TypePrint
        {
            get => localSettings.Values["TypePrint"] == null ? "": localSettings.Values["TypePrint"].ToString();
            set => localSettings.Values["TypePrint"] = value;
        }

        static public string strIP
        {
            get => localSettings.Values["strIP"] == null ? "" : localSettings.Values["strIP"].ToString();
            set => localSettings.Values["strIP"] = value;
        }

        static public int Port
        {
            get => (int)(localSettings.Values["Port"] == null ? 0 : localSettings.Values["Port"]);
            set => localSettings.Values["Port"] = value;
        }
    }
}
