using System;

namespace SkinAssistance.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AspTypePropertyAttribute : Attribute
    {
        public bool CreateConstructorReferences { get; }

        public AspTypePropertyAttribute(bool createConstructorReferences)
        {
            CreateConstructorReferences = createConstructorReferences;
        }
    }
}