#region NS

using System;
using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;
using SkinAssistance.Core.Refrecter;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    internal class InitaileLogComponmentsTask : InitializeTask
    {
        #region Properties

        /// <summary>
        ///     进度权重
        /// </summary>
        public override double ProgressWeight
        {
            get { return 5; }
        }

        public override string TaskName
        {
            get { return "注册日志组件"; }
        }

        #endregion

        #region Method

        protected override async Task<bool> InvokeExcute()

        {
            try
            {
                string applicationHeader = string.Format("{0} .Player Version[{1}] .Net Version[{2}] .OS Version[{3}]", ProductInfo.ProductName, ProductInfo.VersionInfoString, Environment.Version.ToString(4), Environment
                    .OSVersion.VersionString);
                this.Info("".PadLeft(156, '*'));
                this.Info(applicationHeader);
                this.Info("".PadLeft(156, '*'));
                this.Info("Application Session Start");
              
                RasizeProgressChanged(ProgressWeight);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
    }

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