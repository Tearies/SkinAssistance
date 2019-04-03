#region NS

using System;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.Parameter
{
    public class ParameterAttribute : Attribute
    {
        #region Properties

        public string ShortName { get; set; }

        public string Description { get; set; }

        public bool IsRequire { get; set; }

        public IValueConverter Converter { get; set; }

        #endregion

        #region Method

        public ParameterAttribute(string shortName, string description, bool isRequire = false, Type convertType = null)
        {
            ShortName = shortName;
            IsRequire = isRequire;
            Description = description;
            if (convertType != null)
                Converter = ActivatorWrapper.SolveInstance(convertType).ResolveService<IValueConverter>();
        }

        #endregion
    }
}