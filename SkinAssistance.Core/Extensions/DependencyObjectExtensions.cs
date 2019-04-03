using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace SkinAssistance.Core.Extensions
{
    public static class DependencyObjectExtensions
    {
        #region Fields

        private static readonly BindingFlags MethodFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags
                .Static;

        #endregion

        #region Members

      

        /// <summary>
        ///     Resolves the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="throwError">if set to <c>true</c> [throw error].</param>
        /// <returns>TResult.</returns>
        public static TResult ResolveService<T, TResult>(this object dependencyObject, Func<T, TResult> callBack,
            bool throwError = false) where T : DependencyObject
        {
            var tempObject = dependencyObject as T;
            if (tempObject != null)
                try
                {
                    return callBack(tempObject);
                }
                catch (Exception ex)
                {
                    if (throwError)
                        throw ex;
                }
            return default(TResult);
        }

        public static T ResolveService<T>(this DependencyObject dpobject) where T : class, new()
        {
            return dpobject as T;
        }
 

        public static T GetPropertyValue<T>(this object host, string propertyName)
        {
            var propertyInfo = host.GetType().GetProperty(propertyName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null)
                return (T) propertyInfo.GetValue(host);
            return default(T);
        }

        public static void SetPropertyValue<T>(this object host, string propertyName, T value)
        {
            var propertyInfo = host.GetType().GetProperty(propertyName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null)
                propertyInfo.SetValue(host, value);
        }

        public static IRawElementProviderSimple ProviderFromPeer(this AutomationPeer peer)
        {
            var type = peer.GetType();
            var baseMethod = type.GetMethod("ProviderFromPeer",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (baseMethod != null)
                return (IRawElementProviderSimple) baseMethod.Invoke(peer,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new object[]
                    {
                        peer
                    }, CultureInfo.CurrentCulture);
            return null;
        }

        public static T ExcuteMethod<T>(this object host, string methodName, bool methodisStatic, object[] param)
        {
            if (host == null || string.IsNullOrEmpty(methodName))
                return default(T);
            var type = host.GetType();
            var methodInfos = type.GetMethods(MethodFlags);
            var parmlength = param == null ? 0 : param.Length;
            var find = methodInfos.FirstOrDefault(o => o.Name == methodName && o.GetParameters().Length == parmlength);
            if (find != null)
                return (T) find.Invoke(methodisStatic ? null : host, MethodFlags, null, param,
                    CultureInfo.CurrentCulture);
            return default(T);
        }

        public static T ExcuteMethod<T>(this Type host, string methodName, bool methodisStatic, object[] param)
        {
            if (host == null || string.IsNullOrEmpty(methodName))
                return default(T);
            var methodInfos = host.GetMethods(MethodFlags);
            var parmlength = param == null ? 0 : param.Length;
            var find = methodInfos.FirstOrDefault(o => o.Name == methodName && o.GetParameters().Length == parmlength);
            if (find != null)
                return (T) find.Invoke(methodisStatic ? null : Activator.CreateInstance(host), MethodFlags, null, param,
                    CultureInfo.CurrentCulture);
            return default(T);
        }

        #endregion
    }
}