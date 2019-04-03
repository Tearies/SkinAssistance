#region NS

using System;
using System.Diagnostics;
using SkinAssistance.Core.Extensions;

#endregion

namespace SkinAssistance.Core.Refrecter
{
    #region Reference

    #endregion

    public static class ProductInfo
    {
        #region Properties

        private static TimeSpan productStartTime;

        public static string Description { get; private set; }
        public static string CompanyName { get; private set; }
        public static string Title { get; private set; }
        public static string Copyright { get; private set; }
        public static string Trademark { get; private set; }
        public static string ProductName { get; private set; }
        public static Version Version { get; private set; }
        public static string AssemblyName { get; private set; }
        public static string DirectoryPath { get; private set; }
        public static string StackTrace { get; private set; }
        public static long WorkingSet { get; private set; }

        public static string VersionInfoString { get; private set; }
        public static string VersionString
        {
            get { return Version.ToUIString(); }
        }

        public static string ProductId { get; private set; }


        public static bool IsConsoleHost
        {
            get { return ConsoleHelper.IsConsole; }
        }

        /// <summary>统计程序的运行时间</summary>
        public static TimeSpan RunningTimes => TimeSpan.FromTicks(DateTime.Now.Subtract(productStartTime).Ticks);

        #endregion

        #region Method

        static ProductInfo()
        {
            var startTime = Process.GetCurrentProcess().StartTime;
            productStartTime = TimeSpan.FromTicks(startTime.Ticks);
            var enteryAssemblyInfo = new EntryAssemblyInfo();
            Description = enteryAssemblyInfo.Description;
            CompanyName = enteryAssemblyInfo.CompanyName;
            Copyright = enteryAssemblyInfo.Copyright;
            Trademark = enteryAssemblyInfo.Trademark;
            ProductName = enteryAssemblyInfo.ProductName;
            Version = enteryAssemblyInfo.Version;
            AssemblyName = enteryAssemblyInfo.AssemblyName;
            DirectoryPath = enteryAssemblyInfo.DirectoryPath;
            StackTrace = enteryAssemblyInfo.StackTrace;
            WorkingSet = enteryAssemblyInfo.WorkingSet;
            ProductId = enteryAssemblyInfo.ProductId;
            Title = enteryAssemblyInfo.Title;
            VersionInfoString = enteryAssemblyInfo.VersionInfoString;
        }

        #endregion
    }

    public static class BooleanBoxes
    {
        private static object TrueBox = (object)true;
        private static object FalseBox = (object)false;

        public static object Box(bool value)
        {
            if (value)
                return BooleanBoxes.TrueBox;
            return BooleanBoxes.FalseBox;
        }
    }

    
     
}