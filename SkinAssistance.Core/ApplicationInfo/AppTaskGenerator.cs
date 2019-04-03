#region NS

using System;
using System.ComponentModel;
using System.Windows;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Instance;
using SkinAssistance.Core.InstanseContext;
using SkinAssistance.Core.Native;

#endregion

namespace SkinAssistance.Core.ApplicationInfo
{
    #region Reference

    #endregion

    internal class AppTaskGenerator<T> where T : ApplicationEntry
    {
        #region Method

        public void Run()
        {
            InstanceContext<InternalApp<T>>.InitializeAsFirstInstance(StartAppTask,
                p => { p.BringToForeground = true; });
        }

        private void StartAppTask()
        {

            ActivatorWrapper.SolveInstance<InternalApp<T>>().Run();
        }

        #endregion
    }

    public class BootstrapWindow : Window
    {

        protected IntPtr Handle
        {
            get { return this.GetPropertyValue<IntPtr>("CriticalHandle"); }
        }

        public BootstrapWindow()
        {

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            ShowActivated = true;
            SizeToContent = SizeToContent.WidthAndHeight;
            Application.Current.MainWindow = this;
        }

        public new void Show()
        {
            base.Show();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
        }


        #region Overrides of Window

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closing" /> event.</summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        #endregion
    }
}