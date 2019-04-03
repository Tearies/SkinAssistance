using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SkinAssistance.Core.ApplicationInfo;
using SkinAssistance.Core.Instance;
using SkinAssistance.ViewModel;

namespace SkinAssistance.View
{
    public class SplashWindow : NativeWindow
    {
        #region Constructors

        static SplashWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplashWindow),
                new FrameworkPropertyMetadata(typeof(SplashWindow)));
        }


        public SplashWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = InstanseManager.ResolveService<SplashScreenViewModel>();
        }


        #endregion
    }
}
