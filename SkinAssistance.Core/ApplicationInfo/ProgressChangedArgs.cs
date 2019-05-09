using System;

namespace SkinAssistance.Core.ApplicationInfo
{
    /// <summary>
    ///     汇报参数
    /// </summary>
    public class ProgressChangedArgs : EventArgs
    {
        #region Properties

        /// <summary>
        ///     当前进度
        /// </summary>
        public double Progress { get; private set; }

        #endregion

        #region Method

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.EventArgs" /> class.
        /// </summary>
        public ProgressChangedArgs(double progress)
        {
            Progress = progress;
        }

        #endregion
    }
}