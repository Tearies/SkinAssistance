using System;
using System.Collections.Generic;

namespace SkinAssistance.Core.ApplicationInfo
{
    public abstract class ApplicationEntry
    {
        #region Properties

        /// <summary>
        ///     主窗体类型
        /// </summary>
        public abstract Type MainWindowtype { get; }

        /// <summary>
        ///     是否启用Splash
        /// </summary>
        public abstract bool SplashEnabled { get; }

        /// <summary>
        ///     Splash类型
        /// </summary>
        public abstract Type SplashWindowType { get; }

        /// <summary>
        ///     初始化任务列表
        /// </summary>
        public abstract List<Type> InitializeTask { get; }

        #endregion
    }
}