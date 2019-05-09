using System;

namespace SkinAssistance.Core.VBI.Presentation.Core
{
    /// <summary>
    /// 错误处理模式
    /// </summary>
    [Flags]
    public enum EnumErrorHandle : uint
    {
        None = 0x01,
        Exit = 0x10,
        Restart = 0x100,

        Default = Exit | Restart,
    }
}