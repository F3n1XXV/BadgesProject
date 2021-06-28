using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Windows.Foundation.Diagnostics;

namespace clEventLoggingUWP
{
 
    public  class EventLogging 
    {
        readonly Dictionary<int, string> dictChannelName = new Dictionary<int, string>
        {
            { 1,"Info" },
            { 3,"Error"},
            { 2,"Warning"},
            { 4,"SQL" }
        };

        static public string NameSource;
        //FileLogginSession je název souboru na disku (př. Log-nazev_souboru.etl)
   //C:\Users\<user>\AppData\Local\Packages\<file projectu>\LocalState\Logs\Log-<nameLogginSession>-1.etl

    //C:\Users\miros\AppData\Local\Packages\d5aa7e2e-00e9-47fd-adde-e0cc8b682787_05vkp36a4qba6\LocalState\Logs\Log-MIS_session-1.etl
        private static FileLoggingSession fileLoggingSession { get; set; }
        private static LoggingChannel loggingChannel { get; set; }
        #region  Messiges
        //if the order of the methods don’t match ordinal number position in the class it would fail generating ETW traces.
        //The EventSource has dependency on the order of the methods in the class.

        [Event(1, Level = EventLevel.Informational)]
        public static void Info(int id, string message)
        {
            writeTrace("info","Info(" + id.ToString() + "):" + message, LoggingLevel.Information); ;
        }
        [Event(2, Level = EventLevel.Warning)]
        public static void Warning(int id, string message)
        {
            writeTrace("Warning","Warning(" + id.ToString() + "):" + message, LoggingLevel.Warning);
        }
        [Event(3, Level = EventLevel.Error)]
        public static void Error(int id, string message)
        {
            writeTrace("Error","Error(" + id.ToString() + "):" +message,LoggingLevel.Error);
        }
        [Event(4, Level = EventLevel.Informational)]
        public static void SQLTrace(int id, string message)
        {
            writeTrace("SQL error","SQL error(" + id.ToString() + "):" + message, LoggingLevel.Error);
        }
        #endregion


        static EventLogging()
        {
        
        }

        public EventLogging(string nameProject)
        {
            NameSource = nameProject;

            fileLoggingSession = new FileLoggingSession(NameSource + "_session" + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss"));
        }

        private static async void writeTrace(string channelName,string message,LoggingLevel logLevel)
        {
            try
            {

                 loggingChannel = new LoggingChannel(channelName, new LoggingChannelOptions());

                fileLoggingSession.AddLoggingChannel(loggingChannel, logLevel);

                loggingChannel.LogMessage(message, logLevel);

                await fileLoggingSession.CloseAndSaveToFileAsync();           }
            catch (Exception)
            {


            }
        }

        private static void clearFileEventLog()
        { 
        
        }
    }
}
