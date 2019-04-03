#region NS

using System.Collections.Generic;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.Parameter
{
    public class Parameter : ServiceProvider
    {
        #region Method

        public List<T> Validate<T>(string[] args) where T : class, IParameter
        {
            return args.ResolveService<T>();
        }

        #endregion
    }
}