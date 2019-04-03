#region NS

using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    internal class InitailUpdateTask : InitializeTask
    {
        #region Properties

        /// <summary>
        ///     进度权重
        /// </summary>
        public override double ProgressWeight
        {
            get { return 1; }
        }

        public override string TaskName
        {
            get { return "注册系统服务"; }
        }

        #endregion

        #region Method

        protected override async Task<bool> InvokeExcute()
        {
            RasizeProgressChanged(ProgressWeight);

            return true;
        }

        #endregion
    }
}