using System;
using System.Diagnostics.Tracing;
using Windows.Foundation.Diagnostics;

namespace clEventLoggingUWP
{
 
    public class EventLogging 
    {
        static public string NameSource;
        //FileLogginSession je název souboru na disku (př. Log-nazev_souboru.etl)
   //C:\Users\<user>\AppData\Local\Packages\<file projectu>\LocalState\Logs\Log-<nameLogginSession>-1.etl

    //C:\Users\miros\AppData\Local\Packages\d5aa7e2e-00e9-47fd-adde-e0cc8b682787_05vkp36a4qba6\LocalState\Logs\Log-MIS_session-1.etl
        static private FileLoggingSession fileLoggingSession { get; set; }
        static private LoggingChannel loggingChannel { get; set; }
        #region  Messiges
        //if the order of the methods don’t match ordinal number position in the class it would fail generating ETW traces.
        //The EventSource has dependency on the order of the methods in the class.
        [Event(1, Level = EventLevel.Informational)]
        public void Info(string message)
        {
            writeTrace(1, message);
        }

        [Event(2, Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            writeTrace(2, message);
        }

        [Event(3, Level = EventLevel.Error)]
        public void Error(string message)
        {
            writeTrace(3, message);
        }

        [Event(4, Level = EventLevel.Informational)]
        public void SQLTrace(string message)
        {
            writeTrace(4, message);
        }
        #endregion


        public EventLogging(string  nameProject)
        {
            NameSource = nameProject;

            fileLoggingSession = new FileLoggingSession(NameSource + "_session" + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss"));
        }

        private async void writeTrace(int id, string message)
        {

            //test write data
            //for (int i = 0; i < 50; i++)
            //{
            //    loggingChannel = new LoggingChannel(DateTime.Now.ToString(), new LoggingChannelOptions());

            //    fileLoggingSession.AddLoggingChannel(loggingChannel, LoggingLevel.Warning);

            //    loggingChannel.LogMessage("error message" + i.ToString(), LoggingLevel.Error);
            //}
       
            await fileLoggingSession.CloseAndSaveToFileAsync();
        }
    }
}
