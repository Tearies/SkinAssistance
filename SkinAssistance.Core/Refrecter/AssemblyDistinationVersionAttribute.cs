using System;

namespace SkinAssistance.Core.Refrecter
{
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple = false,Inherited = false)]
    internal class AssemblyDistinationVersionAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute" /> class.</summary>
        public AssemblyDistinationVersionAttribute()
        {
            Version=Version.Parse($"{5}.{0}.{1066}.{6666}");
        } 

        public Version Version { get; private set; }
    }
}