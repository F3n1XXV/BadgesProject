using System;
using System.Diagnostics;

namespace BadgesServerPrint.Class
{
    public static class EventLoging
    {
        private static string source { get; set; }
        private static string sLog { get; set; }

        static EventLoging()
        {
            try
            {
                source = "BadgesServerPrint";
                sLog = "BadgesServerPrint";

                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, sLog);
            }
            catch (Exception)
            {}
        }


        public static void Info(int id, string message)
        {
            write(id, message,EventLogEntryType.Information);
        }

        public static void Warning(int id, string message)
        {
            write(id, message,EventLogEntryType.Warning);
        }

        public static  void Error(int id, string message)
        {
            write(id, message,EventLogEntryType.Error);
        }

        private static void write(int id,string message,EventLogEntryType eventType)
        {
            try
            {
                EventLog.WriteEntry(source, message, eventType, id);
            }
            catch (Exception)
            {}
        }
    }
}
