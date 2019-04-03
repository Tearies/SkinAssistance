#region NS

using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    public class RegistorExceptionHandlerTask : InitializeTask
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
            get { return "注册全局异常处理组件"; }
        }

        #endregion

        #region Method

        protected override async Task<bool> InvokeExcute()
        {
            UnhandledExceptionHander.RegisterGlobalException();
            RasizeProgressChanged(ProgressWeight);
            return true;
        }

        #endregion
    }
}