#region NS

using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    internal class DispatcherContextTask : InitializeTask
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
            get { return "注册UI线程上下文"; }
        }

        #endregion

        #region Method

        protected override async Task<bool> InvokeExcute()
        {
            DispatcherContext.DispatcherContext.Initialize();
            RasizeProgressChanged(ProgressWeight);
            return true;
        }

        #endregion
    }
}