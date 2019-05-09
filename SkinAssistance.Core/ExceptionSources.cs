namespace SkinAssistance.Core.VBI.Presentation.Core
{
    /// <summary>
    /// 异常来源
    /// </summary>
    public enum ExceptionSources : uint
    {
        /// <summary>
        /// The UI dispatcher
        /// </summary>
        Dispatcher,
        /// <summary>
        /// The task
        /// </summary>
        Task,
        /// <summary>
        /// The application domain
        /// </summary>
        AppDomain,
        /// <summary>
        /// The seh
        /// </summary>
        SEH
    }
}