// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 04-19-2017
//
// Last Modified By : Tearies
// Last Modified On : 08-21-2017
// ***********************************************************************
// <copyright file="CommandContext.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Runtime.ExceptionServices;
using System.Windows.Threading;

namespace SkinAssistance.Core.ICommands
{
    /// <summary>
    /// Class CommandContext.
    /// </summary>
    public static class CommandContext
    {
        #region Other

        /// <summary>
        /// Registors the specified command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="target">The target.</param>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecuteFunc">The can execute function.</param>
        public static void Registor<T>(IRegistorCommand<T> command, object target, Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            command.RegistorCommand(target, executeAction, canExecuteFunc);
        }

        #endregion

        /// <summary>
        /// Uns the registor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="target">The target.</param>
        public static void UnRegistor<T>(IRegistorCommand<T> command, object target)
        {
            command.UnRegistorCommand(target);
        }

        /// <summary>
        /// Excutes the command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="param">The parameter.</param>
        [HandleProcessCorruptedStateExceptions]
        public static void ExcuteCommand<T>(this IRegistorCommand<T> command, object param)
        {
            ExcuteCommandWithoutDispatcher(command, param); 

        }

        [HandleProcessCorruptedStateExceptions]
        public static void ExcuteCommandWithoutDispatcher<T>(this IRegistorCommand<T> command, object param)
        {
            try
            {
                if (command.CanExecute(param))
                    command.Execute(param);
            }
            catch (Exception ex)
            {
                LogExtensions.Critical(null, ex);
            }
        }
       
    }
}