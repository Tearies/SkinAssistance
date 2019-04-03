#region NS

using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    public class InitialThemeTask : InitializeTask
    {
        #region Properties

        private double _progressWeight;

        /// <summary>
        ///     进度权重
        /// </summary>
        public override double ProgressWeight
        {
            get { return 5; }
        }

        public override string TaskName
        {
            get { return "加载主题文件"; }
        }

        #endregion

        #region Method

        /// <summary>
        ///     Will be Do
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> InvokeExcute()
        {
            RasizeProgressChanged(ProgressWeight);
            return true;
        }

        #endregion
    }
}