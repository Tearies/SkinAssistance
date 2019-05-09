using System;

namespace SkinAssistance.Core.VBI.Presentation.Core
{
    /// <summary>
    /// <para>默认情况下，子进程继承父进程的错误模式标志。</para>
    /// <para>换句话说，如果一个进程SEM_NOGPFAULTERRORBOX标志已经打开，并且生成了一个子进程，该子进程也拥有这个打开的标志。</para>
    /// <para>但是，子进程并没有得到这一情况的通知，它可能尚未编写以便处理GP故障的错误。</para>
    /// <para>如果GP故障发生在子进程的某个线程中，该子进程就会终止运行，而不通知用户。</para>
    /// <para>父进程可以防止子进程继承它的错误模式，方法是在调用CreateProcess时设定CREATE_DEFAULT_ERROR_MODE标志</para>
    /// </summary>
    [Flags]
    public enum ErrorMode : int
    {
        /// <summary>
        /// 0x0 <para>使用系统默认的，既显示所有错误的对话框</para>
        /// </summary>
        SEM_Default = 0,
        /// <summary>
        /// 0x0001 <para>系统不显示关键错误处理消息框。 相反，系统发送错误给调用进程</para>
        /// </summary>
        SEM_FAILCRITICALERRORS = 0x0001,

        /// <summary>
        /// 0x0004 <para>系统会自动修复故障此功能只支持部分处理器架构</para>
        /// </summary>
        SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,

        /// <summary>
        /// 0x0002 <para>系统不显示Windows错误报告对话框</para>
        /// </summary>
        SEM_NOGPFAULTERRORBOX = 0x0002,

        /// <summary>
        /// 0x8000 <para>当无法找到文件时不弹出错误对话框。 相反，错误返回给调用进程</para>
        /// </summary>
        SEM_NOOPENFILEERRORBOX = 0x8000
    }
}