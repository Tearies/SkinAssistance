using System;

namespace SkinAssistance.Core.Refrecter
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class XmlCommentAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute" /> class.</summary>
        public XmlCommentAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}