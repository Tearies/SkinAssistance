using System;
using SkinAssistance.Core.VBI.Presentation.Core;

namespace SkinAssistance.Core
{
    /// <summary>
    /// 异常的回调Handler
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="handle">The handle.</param>
    /// <param name="exSource">The ex source.</param>
    public delegate void ExceptionEventHandler(Exception innerException, EnumErrorHandle handle, ExceptionSources exSource);
}