using System;
using System.ComponentModel;
using System.Windows;
using SkinAssistance.Core.Extensions;

namespace SkinAssistance.Core.ApplicationInfo
{
    public class BootstrapWindow : Window
    {

        protected IntPtr Handle
        {
            get { return this.GetPropertyValue<IntPtr>("CriticalHandle"); }
        }

        public BootstrapWindow()
        {

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            ShowInTaskbar = false;
            ShowActivated = true;
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