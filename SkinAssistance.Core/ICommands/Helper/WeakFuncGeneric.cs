﻿#region HeaderInfo

// **********************************************************************************************************
// FileName:  WeakFuncGeneric.cs
// Author:     Teries
// Email:  Teries@Tearies.com
// CopyRight:  2015© Tearies .All rights reserved.
// ChangeNote:
// 
// LastChange:   Teries
// ChangeDate:   2015-07-28-13:55
//  **********************************************************************************************************

#endregion

using System;

namespace SkinAssistance.Core.ICommands.Helper
{
    #region Reference

    #endregion

    /// <summary>
    ///     Stores an Func without causing a hard reference
    ///     to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Func's parameter.</typeparam>
    /// <typeparam name="TResult">The type of the Func's return value.</typeparam>
    public class WeakFunc<T, TResult> : WeakFunc<TResult>, IExecuteWithObjectAndResult
    {
        #region Fields

        private Func<T, TResult> _staticFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(Func<T, TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="target">The func's owner.</param>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<T, TResult> func)
        {
            if (func.Method.IsStatic)
            {
                _staticFunc = func;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }


            Method = func.Method;
            FuncReference = new WeakReference(func.Target);


            Reference = new WeakReference(target);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the method that this WeakFunc represents.
        /// </summary>
        public override string MethodName
        {
            get
            {
                if (_staticFunc != null)
                {
                    return _staticFunc.Method.Name;
                }


                return Method.Name;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the Func's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (_staticFunc == null
                    && Reference == null)
                {
                    return false;
                }

                if (_staticFunc != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                return Reference.IsAlive;
            }
        }

        #endregion

        #region Implementations

        /// <summary>
        ///     Executes the func with a parameter of type object. This parameter
        ///     will be casted to T. This method implements <see cref="IExecuteWithObject.ExecuteWithObject" />
        ///     and can be useful if you store multiple WeakFunc{T} instances but don't know in advance
        ///     what type T represents.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter that will be passed to the func after
        ///     being casted to T.
        /// </param>
        /// <returns>The result of the execution as object, to be casted to T.</returns>
        public object ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T) parameter;
            return Execute(parameterCasted);
        }

        #endregion

        /// <summary>
        ///     Executes the func. This only happens if the func's owner
        ///     is still alive. The func's parameter is set to default(T).
        /// </summary>
        public new TResult Execute()
        {
            return Execute(default(T));
        }

        /// <summary>
        ///     Executes the func. This only happens if the func's owner
        ///     is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public TResult Execute(T parameter)
        {
            if (_staticFunc != null)
            {
                return _staticFunc(parameter);
            }

            if (IsAlive)
            {
                if (Method != null
                    && FuncReference != null)
                {
                    return (TResult) Method.Invoke(
                        FuncTarget,
                        new object[]
                        {
                            parameter
                        });
                }
            }

            return default(TResult);
        }

        /// <summary>
        ///     Sets all the funcs that this WeakFunc contains to null,
        ///     which is a signal for containing objects that this WeakFunc
        ///     should be deleted.
        /// </summary>
        public new void MarkForDeletion()
        {
            _staticFunc = null;
            base.MarkForDeletion();
        }
    }
}