#region NS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace SkinAssistance.Core.Extensions
{
    #region Reference

    #endregion

    /// <summary>
    ///     Class AssemblyExtension.
    /// </summary>
    public static class AssemblyExtension
    {
        #region Nested Types

        private static class TypeExtensions
        {
            #region Method

            /// <summary>
            ///     Gets the custerm attribute.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="object">The object.</param>
            /// <returns>``0.</returns>
            internal static T GetCustermAttribute<T>(object @object) where T : Attribute
            {
                return GetTypeAttribute<T>(@object.GetType());
            }

            internal static T GetTypeAttribute<T>(Type t) where T : Attribute
            {
                return (T) t.GetCustomAttributes(true).FirstOrDefault(o => o.GetType() == typeof(T));
            }

            #endregion
        }

        #endregion

        #region Method

        /// <summary>
        ///     Gets the assembly custerm attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>``0.</returns>
        public static T GetAssemblyCustermAttribute<T>(this Assembly assembly) where T : Attribute
        {
            var attr = assembly.GetCustomAttributes(typeof(T), true);
            if (attr.Any())
                return (T) attr[0];
            return default(T);
        }

        /// <summary>
        ///     Gets the custerm assembly attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application">The application.</param>
        /// <returns>``0.</returns>
        public static T GetCustermAssemblyAttribute<T>(this object application) where T : Attribute
        {
            return Assembly.GetEntryAssembly().GetAssemblyCustermAttribute<T>();
        }

        /// <summary>
        ///     Gets the custerm attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application">The application.</param>
        /// <returns>``0.</returns>
        public static T GetCustermAttribute<T>(this object application) where T : Attribute
        {
            if (application is Type)
                return TypeExtensions.GetTypeAttribute<T>((Type) application);
            if (application is Enum)
                return ((Enum) application).GetCustermAttribute<T>();
            return TypeExtensions.GetCustermAttribute<T>(application);
        }

        /// <summary>
        ///     Gets the excuting assembly attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application">The application.</param>
        /// <returns>``0.</returns>
        public static T GetExcutingAssemblyAttribute<T>(this object application) where T : Attribute
        {
            return Assembly.GetExecutingAssembly().GetAssemblyCustermAttribute<T>();
        }

        /// <summary>
        ///     Gets the calling assembly attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application">The application.</param>
        /// <returns>``0.</returns>
        public static T GetCallingAssemblyAttribute<T>(this object application) where T : Attribute
        {
            return Assembly.GetCallingAssembly().GetAssemblyCustermAttribute<T>();
        }

        public static IEnumerable<object> ResolveParameter<T>(this T t, IEnumerable<object> properpties)
        {
            return properpties.Select(item => t.ResolveProperty(item));
        }

        private static object ResolveProperty<T>(this T type, object propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName.ToString());
            if (propertyInfo == null)
                return null;
            return propertyInfo.GetValue(type, null);
        }

        #endregion
    }
}