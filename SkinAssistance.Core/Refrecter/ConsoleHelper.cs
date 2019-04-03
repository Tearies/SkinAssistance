#region NS

using System;
using System.IO;
using System.Security;

#endregion

namespace SkinAssistance.Core.Refrecter
{
    internal class ConsoleHelper
    {
        #region Properties

        internal static bool IsConsole
        {
            [SecuritySafeCritical] get { return Console.In != StreamReader.Null; }
        }

        private static IntPtr _consoleOutputHandle;

        #endregion
    }

    //public static class EventLogHelper
    //{

    //    static EventLogHelper()
    //    {

    //        EventLogConfiguration config = new EventLogConfiguration(EventLogIsolation.Application.ToString());
    //        if (config.LogMode != EventLogMode.Circular)
    //        {
    //            config.LogMode = EventLogMode.Circular;
    //            config.MaximumSizeInBytes = 2048 * 1024;
    //            config.SaveChanges();
    //        }
    //    }

    //    public static void WriteEventLog(string message)
    //    {
    //        EventLog.WriteEntry("Tearies", message, EventLogEntryType.Information);
    //    }
    //}
}