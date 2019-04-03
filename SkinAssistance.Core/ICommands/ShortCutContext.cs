// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 08-21-2017
//
// Last Modified By : Tearies
// Last Modified On : 08-21-2017
// ***********************************************************************
// <copyright file="ShortCutContext.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Input;
using SkinAssistance.Core.HotKey;

namespace SkinAssistance.Core.ICommands
{
    /// <summary>
    /// Class ShortCutContext.
    /// </summary>
    internal static class ShortCutContext
    {
        /// <summary>
        /// The short command source
        /// </summary>
        private static readonly Dictionary<ShortKey, ICommand> shortCommandSource;

        private static long lastTimeSpan = 0;

        /// <summary>
        /// Initializes static members of the <see cref="ShortCutContext"/> class.
        /// </summary>
        static ShortCutContext()
        {
            shortCommandSource = new Dictionary<ShortKey, ICommand>();
        }

        /// <summary>
        /// Registors the short cut.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="shortKeys">The short cut.</param>
        public static void RegistorShortCut(this ICommand command, params ShortKey[] shortKeys)
        {
            if (shortKeys.Any())
            {
                foreach (var shortKey in shortKeys)
                {

                    if (!shortCommandSource.ContainsKey(shortKey))
                    {
                        HotkeyManager.Current.AddOrReplace($"{shortKey.ModifierKey}+{shortKey.Key}", shortKey.Key, shortKey.ModifierKey);
                        shortCommandSource.Add(shortKey, command);
                    }
                }
            }

        }

        [HandleProcessCorruptedStateExceptions]
        public static bool InvokeShortCuts(Key key, ModifierKeys modifiers)
        {
            var tempShortKey = new ShortKey(key, modifiers);
            if (shortCommandSource.ContainsKey(tempShortKey))
            {
                try
                {
                    var now = DateTime.Now.Ticks;
                    if (now - lastTimeSpan < TimeSpan.FromMilliseconds(800).Ticks)
                    {
                        LogExtensions.Info(null, $"Skip shrortCuts {tempShortKey} with duration too small");
                        return true;
                    }
                    lastTimeSpan = now;
                    LogExtensions.Info(null, "Invoke ShortKey:{0}", tempShortKey);
                    var tempCommand = shortCommandSource[tempShortKey];
                    tempCommand.Execute(null);
                    LogExtensions.Info(null, "Invoke ShortKey:{0} Succ", tempShortKey);
                }
                catch (Exception ex)
                {
                    LogExtensions.Error(null, ex, "Invoke ShortKey:{0} faild", tempShortKey);
                }
                return true;
            }
            return false;
        }

    }
}