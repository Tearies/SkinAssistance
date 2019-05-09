#region NS

using System;
using System.Threading.Tasks;
using SkinAssistance.Core.CommonInitialTask;

#endregion

namespace SkinAssistance.Core.ApplicationInfo
{
    #region Reference

    #endregion


    public abstract class InitializeTask : IProgress
    {
        #region Properties

        private readonly Type thisType;

        protected string ErrorMessage { get; set; }

        /// <summary>
        ///     进度权重
        /// </summary>
        public abstract double ProgressWeight { get; }

        public abstract string TaskName { get; }
        #endregion

        #region Override

        event EventHandler<ProgressChangedArgs> IProgress.ProgressChanged
        {
            add { throw new NotImplementedException(); }

            remove { throw new NotImplementedException(); }
        }

        #endregion

        #region Method

        protected InitializeTask()
        {
            thisType = GetType();
        }

        private event EventHandler<ProgressChangedArgs> _progressEventHandler;

        public event EventHandler<ProgressChangedArgs> ProgressChanged
        {
            add { _progressEventHandler += value; }
            remove { _progressEventHandler -= value; }
        }

        /// <summary>
        ///     Will be Do
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> InvokeExcute();

        private void LogErrorInfo()
        {
            if (thisType != CommandInitialTaskTypes.LogComponments)
                this.Info("Invoke InitializeTask:{0} Faild. Cause By:{1}",
                    thisType.Name, ErrorMessage);
        }

        public Task<bool> Excute()
        {
            if (thisType != CommandInitialTaskTypes.LogComponments)
                this.Info("Start InitializeTask:{0}", thisType.Name);
            var result = InvokeExcute();
            if (!result.Result)
                LogErrorInfo();
            if (thisType != CommandInitialTaskTypes.LogComponments)
                this.Info("End InitializeTask:{0}", thisType.Name);
            return result;
        }


        protected virtual void RasizeProgressChanged(double progress)
        {
            var handler = _progressEventHandler;
            if (handler != null)
                handler(this, new ProgressChangedArgs(progress));
        }

        #endregion
    }
}