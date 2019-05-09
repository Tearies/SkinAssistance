using System;

namespace SkinAssistance.Core.ApplicationInfo
{
    internal interface IProgress
    {
        #region Properties

        /// <summary>
        ///     进度权重
        /// </summary>
        double ProgressWeight { get; }

        #endregion

        #region Method

        /// <summary>
        ///     配置文件进度汇报
        /// </summary>
        event EventHandler<ProgressChangedArgs> ProgressChanged;

        #endregion
    }
}