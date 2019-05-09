namespace SkinAssistance.Core.VBI.Presentation.Core
{
    public enum ExceptionMode : uint
    {
        /// <summary>
        /// 表示我已经处理了异常,可以优雅地结束了  
        /// </summary>
        EXCEPTION_EXECUTE_HANDLER = 1,
        /// <summary>
        /// 表示我不处理,其他人来吧,于是windows调用默认的处理程序显示一个错误框,并结束  
        /// </summary>
        EXCEPTION_CONTINUE_SEARCH = 0,
        /// <summary>
        /// 表示错误已经被修复,请从异常发生处继续执行
        /// </summary>
        EXCEPTION_CONTINUE_EXECUTION = 0xffffffff
    }
}