#region NS

using System;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Native;

#endregion

namespace SkinAssistance.Core
{
    #region Reference

    #endregion

    /// <summary>
    /// Class LogExtensions.
    /// </summary>
    public static class LogExtensions
    {
        #region Method
        /// <summary>
        /// Informations the specified string format.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Info(this object logSource, string stringFormat, params object[] @params)
        {
            Log.Info(GetThreadInfo() + stringFormat.SafeFormat(@params));
        }

        /// <summary>
        /// Debugs the specified string format.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Debug(this object logSource, string stringFormat, params object[] @params)
        {
            Log.Debug(GetThreadInfo() + stringFormat.SafeFormat(@params));
        }

        /// <summary>
        /// Errors the specified string format.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Error(this object logSource, string stringFormat,
            params object[] @params)
        {
            Log.Error(GetThreadInfo() + stringFormat.SafeFormat(@params));
        }

        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Error(this object logSource, Exception ex, string stringFormat, params object[] @params)
        {
            Log.Error(GetThreadInfo() + stringFormat.SafeFormat(@params), ex);
        }

        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="ex">The ex.</param>
        public static void Error(this object logSource, Exception ex)
        {
            Log.Error(GetThreadInfo(), ex);
        }

        /// <summary>
        /// Criticals the specified string format.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Critical(this object logSource, string stringFormat, params object[] @params)
        {
            Log.Error(GetThreadInfo() + stringFormat.SafeFormat(@params));
        }

        /// <summary>
        /// Criticals the specified exception.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="exception">The exception.</param>
        public static void Critical(this object logSource, Exception exception)
        {
            Log.Error(GetThreadInfo() , exception);
        }

        /// <summary>
        /// Warnings the specified string format.
        /// </summary>
        /// <param name="logSource">The log source.</param>
        /// <param name="stringFormat">The string format.</param>
        /// <param name="params">The parameters.</param>
        public static void Warning(this object logSource, string stringFormat, params object[] @params)
        {
            Log.Warn(GetThreadInfo() + stringFormat.SafeFormat(@params));
        }


        /// <summary>
        /// Gets the thread information.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetThreadInfo()
        {
            return "UnManaged ID:[" + ThreadHelper.GetCurrentThreadID() + "]| ";
        }
        #endregion
    }
}