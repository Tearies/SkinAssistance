using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;
using SkinAssistance.Core.Refrecter;
using SkinAssistance.View;
[assembly: AssemblyDistinationVersion]
namespace SkinAssistance
{
    public class AppEntry : ApplicationEntry
    {
        #region Properties

        /// <summary>
        ///     主窗体类型
        /// </summary>
        public override Type MainWindowtype
        {
            get { return typeof(MainWindow); }
            //todo 测试时使用下面的测试窗体
            //get { return typeof(TestWindow); }
        }

        /// <summary>
        ///     是否启用Splash
        /// </summary>
        public override bool SplashEnabled
        {
            get { return true; }
        }

        /// <summary>
        ///     Splash类型
        /// </summary>
        public override Type SplashWindowType
        {
            get { return typeof(SplashWindow); }
        }

        /// <summary>
        ///     初始化任务列表
        /// </summary>
        public override List<Type> InitializeTask
        {
            get
            {
                return new List<Type>
                {
                    //Warning: Dont't Change the Order
                    typeof(InitializeThemesTask)
                };
            }
        }

        #endregion
    }
}
