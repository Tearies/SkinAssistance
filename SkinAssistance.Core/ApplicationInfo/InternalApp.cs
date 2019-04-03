#region NS

#endregion

using System.Windows;

namespace SkinAssistance.Core.ApplicationInfo
{
    #region Reference

    #endregion

    internal class InternalApp<T> : Application where T : ApplicationEntry
    {
        #region Method

        public InternalApp()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!BootstrapFactory<T>.InitializeCommonTask().Result)
                Current.Shutdown();

            BootstrapFactory<T>.Initialize(e.Args);
        }

        #endregion
    }
}