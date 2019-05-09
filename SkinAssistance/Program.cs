using System;
using SkinAssistance.Core.ApplicationInfo;

namespace SkinAssistance
{
    internal class Program
    {
        #region Method

        [STAThread]
        private static void Main()
        {
            BootstrapFactory<AppEntry>.StartSession();
        }



        #endregion
    }
}