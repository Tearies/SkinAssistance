#region NS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SkinAssistance.Core.CommonInitialTask;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.ApplicationInfo
{
    public class BootstrapFactory<T> where T : ApplicationEntry
    {
        #region Properties

        private static List<Type> _commonTask;

        #endregion

        #region Method

        internal static async void Initialize(string[] args)
        {
            try
            {
                var appentry = ActivatorWrapper.SolveInstance<T>();
                AdapterInitialize(appentry.InitializeTask);
                var isSucc = false;
                if (appentry.SplashEnabled)
                {
                    var splashWindow = (Window)ActivatorWrapper.SolveInstance(appentry.SplashWindowType);
                    splashWindow.Show();
                    isSucc = await Task.Run(() => InitializeTaskFatory.ExcuteInitialize());
                    splashWindow.Close();
                }
                else
                {
                    isSucc = await Task.Run(() => InitializeTaskFatory.ExcuteInitialize());
                }


                if (isSucc)
                {
                    var mainWindow = (BootstrapWindow)ActivatorWrapper.SolveInstance(appentry.MainWindowtype);
                    mainWindow.Closed += MainWindow_Closed;
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("系统任务初始化错误,程序关闭", "错误提示");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("系统初始化错误：" + ex.Message, "错误提示");
                LogExtensions.Critical(null, ex);
                Application.Current.Shutdown();
            }
        }

        private static void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        internal static void AdapterInitialize(List<Type> tasks)
        {
            var tmpTask = new List<Type>();

            if (tasks.Any())
                tmpTask.AddRange(tasks.Where(task => !_commonTask.Contains(task)));
            InitializeTaskFatory.Configurate(tmpTask.ToArray());
        }

        internal static async Task<bool> InitializeCommonTask()
        {
            _commonTask = CommandInitialTaskTypes.CommandInitialTask;
            InitializeTaskFatory.Configurate(_commonTask.ToArray());
            return await InitializeTaskFatory.ExcuteInitialize();
        }

        public static void StartSession()
        {
            ActivatorWrapper.SolveInstance<AppTaskGenerator<T>>().Run();
            InstanseManager.ResolveService<DisposeService>().Dispose();
            LogExtensions.Info(null,"Application Session End");
        }

        #endregion
    }
}