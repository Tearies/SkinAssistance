using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;

namespace SkinAssistance.Core.CommonInitialTask
{
    internal class InitialeGlobalCulturTask : InitializeTask
    {
        private double _progressWeight;
        private string _taskName;

        #region Overrides of InitializeTask

        /// <summary>
        ///     进度权重
        /// </summary>
        public override double ProgressWeight
        {
            get { return 0xff; }
        }

        public override string TaskName
        {
            get { return "语言设置"; }
        }

        /// <summary>
        ///     Will be Do
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> InvokeExcute()
        {
            //CultureInfoManager.ApplyGlobalCultureInfo();//sgh于2018.1.23注释，主要用于使用默认操作系统的语言和区域设置，而不是像程序里面那样强制使用英语区域设置
            base.RasizeProgressChanged(0xff);
            return true;
        }

        #endregion
    }
}