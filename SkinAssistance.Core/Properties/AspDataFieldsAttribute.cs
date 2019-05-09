using System;

namespace SkinAssistance.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class AspDataFieldsAttribute : Attribute { }
}