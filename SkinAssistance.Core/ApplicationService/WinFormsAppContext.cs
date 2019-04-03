#region NS

using System;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

#endregion

namespace SkinAssistance.Core.ApplicationService
{
    #region Reference

    #endregion

    internal class WinFormsAppContext : ApplicationContext
    {
        #region Properties

        private readonly WindowsFormsApplicationBase m_App;

        #endregion

        #region Method

        public WinFormsAppContext(WindowsFormsApplicationBase App)
        {
            m_App = App;
        }

        [SecuritySafeCritical]
        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            if (m_App.ShutdownStyle == ShutdownMode.AfterMainFormCloses)
            {
                base.OnMainFormClosed(sender, e);
            }
            else
            {
                new UIPermission(UIPermissionWindow.AllWindows).Assert();
                var openForms = Application.OpenForms;
                PermissionSet.RevertAssert();
                if (openForms.Count > 0)
                    MainForm = openForms[0];
                else
                    base.OnMainFormClosed(sender, e);
            }
        }

        #endregion
    }
}