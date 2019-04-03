#region NS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using SkinAssistance.Core.Parameter;

#endregion

namespace SkinAssistance.Core.Extensions
{
    /// <summary>
    /// Class CoreExtension.
    /// </summary>
    public static class CoreExtension
    {
        #region Method
        /// <summary>
        /// To the type of the log.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentOutOfRangeException">eventType</exception>
        public static string ToLogType(this TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    return "C";
                case TraceEventType.Error:
                    return "E";
                case TraceEventType.Warning:
                    return "W";
                case TraceEventType.Information:
                    return "I";
                case TraceEventType.Verbose:
                    return "V";
                case TraceEventType.Start:
                    return "B"; //Begin
                case TraceEventType.Stop:
                    return "S";
                case TraceEventType.Suspend:
                    return "H"; //Hang
                case TraceEventType.Resume:
                    return "R";
                case TraceEventType.Transfer:
                    return "T";
                default:
                    throw new ArgumentOutOfRangeException("eventType");
            }
        }

        /// <summary>
        /// To the log time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.String.</returns>
        public static string ToLogTime(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss ffffff");
        }

        /// <summary>
        /// Prepaires the dictory information.
        /// </summary>
        /// <param name="dicPath">The dic path.</param>
        public static void PrepaireDictoryInfo(this string dicPath)
        {
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
        }
        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>T.</returns>
        public static T ResolveService<T>(this IServiceProvider provider) where T : class
        {
            return provider.GetService(typeof(T)) as T;
        }

        ///// <summary>
        ///// Resolves the service.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="provider">The provider.</param>
        ///// <returns>T.</returns>
        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>T.</returns>
        public static T ResolveService<T>(this object provider) where T : class
        {
            return provider as T;
        }

        /// <summary>
        /// Resolves the excute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="throwError">if set to <c>true</c> [throw error].</param>
        public static void ResolveService<T>(this object dependencyObject, Action<T> callBack, bool throwError = false) where T : DependencyObject
        {
            var tempObject = dependencyObject as T;
            if (tempObject != null)
            {
                try
                {
                    callBack(tempObject);
                }
                catch (Exception ex)
                {
                    if (throwError)
                        throw ex;
                }
            }
        }

        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="throwError">if set to <c>true</c> [throw error].</param>
        /// <returns>TResult.</returns>
        public static TResult ResolveService<T, TResult>(this object dependencyObject, Func<T, TResult> callBack, bool throwError = false) where T : DependencyObject
        {
            var tempObject = dependencyObject as T;
            if (tempObject != null)
            {
                try
                {
                    return callBack(tempObject);
                }
                catch (Exception ex)
                {
                    if (throwError)
                        throw ex;
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// Resolves the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public static T ResolveValue<T>(this IServiceProvider provider, string key)
        {
            var keyValueCache = provider.ResolveService<IKeyValueCache>();
            if (keyValueCache == null)
                return default(T);
            return keyValueCache.ResolveValue<T>(key);
        }

        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>T.</returns>
        public static List<T> ResolveService<T>(this string[] provider) where T : class, IParameter
        {
            if (provider == null)
                return new List<T>();
            return ParameterLine.Parse<T>(provider);
        }


        /// <summary>
        /// Resolves the propertis.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <returns>Dictionary&lt;T, PropertyInfo&gt;.</returns>
        public static Dictionary<T, PropertyInfo> ResolvePropertis<T>(this object container) where T : Attribute
        {
            var result = new Dictionary<T, PropertyInfo>();
            if (container != null)
            {
                var properties = container.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                foreach (var property in properties)
                {
                    var tempAttr = property.GetCustomAttribute<T>();
                    if (tempAttr != null)
                        result.Add(tempAttr, property);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether [is assigned from] [the specified o].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <returns><c>true</c> if [is assigned from] [the specified o]; otherwise, <c>false</c>.</returns>
        public static bool IsAssignedFrom<T>(this object o)
        {
            if (o == null)
                return false;
            return o.GetType().FullName == typeof(T).FullName;
        }



        #endregion

    }
}