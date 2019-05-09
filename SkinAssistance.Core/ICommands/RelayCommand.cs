
#region HeaderInfo

// **********************************************************************************************************
// FileName:  RelayCommandGeneric.cs
// Author:     Teries
// Email:  Teries@Tearies.com
// CopyRight:  2015© Tearies .All rights reserved.
// ChangeNote:
// 
// LastChange:   Teries
// ChangeDate:   2015-07-28-13:51
//  **********************************************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Windows.Input;
using SkinAssistance.Core.ICommands.Helper;

namespace SkinAssistance.Core.ICommands
{
    #region Reference

    #endregion

    /// <summary>
    /// Class RelayCommand. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IRegistorCommand{T}" />
    public sealed class RelayCommand<T> : IRegistorCommand<T>
    {
        /// <summary>
        /// The can actions action items
        /// </summary>
        private readonly List<WeakActionItem<T>> _canActionsActionItems;

        /// <summary>
        /// The excute item
        /// </summary>
        private WeakActionItem<T> excuteItem;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public RelayCommand()
        {
            _canActionsActionItems = new List<WeakActionItem<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="description"></param>
        /// <param name="shortKeys">The short cut.</param>
        public RelayCommand(string commandName,string description, params ShortKey[] shortKeys) : this()
        {
            this.RegistorShortCut(ShortKeyManager.RegistorShortKey(commandName, description, shortKeys).ToArray());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="description"></param>
        /// <param name="key">The key.</param>
        /// <param name="modifier">The modifier.</param>
        public RelayCommand(string commandName, string description, Key key, ModifierKeys modifier = ModifierKeys.None) : this(commandName, description, new ShortKey(key, modifier))
        {

        }


        #endregion

        #region  Implementation

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            var canExe = false;
            canExe = InternalCanExecute(parameter);
            return canExe;
        }

        /// <summary>
        /// Internals the can execute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [HandleProcessCorruptedStateExceptions]
        private bool InternalCanExecute(object parameter)
        {
            bool canExe = false;
            try
            {
                if (excuteItem == null || !excuteItem.IsAlive)
                    canExe = false;
                else if (excuteItem.CanExcuteAction == null)
                    canExe = true;


                else if (parameter == null && typeof(T).IsValueType)

                    canExe = excuteItem.CanExcuteAction.Execute(default(T));
                else
                    canExe = excuteItem.CanExcuteAction.Execute((T)parameter);
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }

            return canExe;
        }

        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [Obsolete("Please instead with CommandContext.ExcuteCommand", true)]
        public void Execute(object parameter)
        {
            InternalExcuteCommand(parameter);
        }

        /// <summary>
        /// Internals the excute command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [HandleProcessCorruptedStateExceptions]
        private void InternalExcuteCommand(object parameter)
        {
            try
            {
                var val = parameter;
                if (parameter != null
                    && parameter.GetType() != typeof(T))
                    if (parameter is IConvertible)
                        val = Convert.ChangeType(parameter, typeof(T), null);


                if (CanExecute(val) && excuteItem.ExcuteAction != null && (excuteItem.ExcuteAction.IsStatic || excuteItem.IsAlive))
                {
                    if (val == null)
                        if (typeof(T).IsValueType)

                            excuteItem.ExcuteAction.Execute(default(T));
                        else
                            excuteItem.ExcuteAction.Execute((T)val);
                    else
                        excuteItem.ExcuteAction.Execute((T)val);
                }

                RefreshCommandState();
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        /// <summary>
        /// Refreshes the state of the command.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void RefreshCommandState()
        {
            _canActionsActionItems.RemoveAll(o => !o.IsAlive);
        }

        /// <summary>
        /// 外界注册事件
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="excute">The excute.</param>
        /// <param name="canExecute">The can execute.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RegistorCommand(object target, Action<T> excute, Func<T, bool> canExecute)
        {
            _canActionsActionItems.Add(new WeakActionItem<T>(new WeakAction<T>(excute),
                new WeakFunc<T, bool>(canExecute), target));
            EnsureWeakActionItem();
        }

        /// <summary>
        /// 事件注销
        /// </summary>
        /// <param name="target">The target.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UnRegistorCommand(object target)
        {
            _canActionsActionItems.RemoveAll(o => o.Target.Target == target);
            EnsureWeakActionItem();
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Ensures the weak action item.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void EnsureWeakActionItem()
        {
            excuteItem = _canActionsActionItems.LastOrDefault(o => o.IsAlive);
        }

        #endregion
    }
}
