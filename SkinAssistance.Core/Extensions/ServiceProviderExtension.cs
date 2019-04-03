#region NS

using System;

#endregion

namespace SkinAssistance.Core.Extensions
{
    public static class ServiceProviderExtension
    {
        #region Method

        public static T GetService<T>(this IServiceProvider provider) where T : class
        {
            return (T) provider.GetService(typeof(T));
        }

        #endregion
    }
}