#region HeaderInfo

// **********************************************************************************************************
// FileName:  WeakFunc.cs
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
using System.Reflection;

namespace SkinAssistance.Core.ICommands.Helper
{
    #region Reference

    #endregion

    /// <summary>
    ///     Stores a Func&lt;T&gt; without causing a hard reference
    ///     to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    ////[ClassInfo(typeof(WeakAction)]
    public class WeakFunc<TResult>
    {
        #region Fields

        private Func<TResult> _staticFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes an empty instance of the WeakFunc class.
        /// </summary>
        protected WeakFunc()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(Func<TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="target">The func's owner.</param>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<TResult> func)
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
        ///     Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakFunc's
        ///     method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        ///     Get a value indicating whether the WeakFunc is static or not.
        /// </summary>
        public bool IsStatic
        {
            get { return _staticFunc != null; }
        }

        /// <summary>
        ///     Gets the name of the method that this WeakFunc represents.
        /// </summary>
        public virtual string MethodName
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
        ///     Gets or sets a WeakReference to this WeakFunc's action's target.
        ///     This is not necessarily the same as
        ///     <see cref="Reference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference FuncReference { get; set; }

        /// <summary>
        ///     Gets or sets a WeakReference to the target passed when constructing
        ///     the WeakFunc. This is not necessarily the same as
        ///     <see cref="FuncReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the Func's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public virtual bool IsAlive
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

        /// <summary>
        ///     Gets the Func's owner. This object is stored as a
        ///     <see cref="WeakReference" />.
        /// </summary>
        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }

                return Reference.Target;
            }
        }

        /// <summary>
        ///     Gets the owner of the Func that was passed as parameter.
        ///     This is not necessarily the same as
        ///     <see cref="Target" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected object FuncTarget
        {
            get
            {
                if (FuncReference == null)
                {
                    return null;
                }

                return FuncReference.Target;
            }
        }

        #endregion

        /// <summary>
        ///     Executes the action. This only happens if the func's owner
        ///     is still alive.
        /// </summary>
        public TResult Execute()
        {
            if (_staticFunc != null)
            {
                return _staticFunc();
            }

            if (IsAlive)
            {
                if (Method != null
                    && FuncReference != null)
                {
                    return (TResult) Method.Invoke(FuncTarget, null);
                }
            }

            return default(TResult);
        }

        /// <summary>
        ///     Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            FuncReference = null;
            Method = null;
            _staticFunc = null;
        }
    }
}