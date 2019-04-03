#region HeaderInfo

// **********************************************************************************************************
// FileName:  MessageWeakAction.cs
// Author:     Teries
// Email:  Teries@Tearies.com
// CopyRight:  2015© Tearies .All rights reserved.
// ChangeNote:
// 
// LastChange:   Teries
// ChangeDate:   2015-07-14-12:04
//  **********************************************************************************************************

#endregion

using System;
using System.Reflection;

namespace SkinAssistance.Core.ICommands.Helper
{
    #region Reference

    #endregion

    public class MessageWeakAction
    {
        #region Fields

        private Action _staticAction;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes an empty instance of the <see cref="MessageWeakAction" /> class.
        /// </summary>
        protected MessageWeakAction()
        {
        }

        public MessageWeakAction(object target, MethodInfo action)
        {
            Method = action;

          ActionReference = new WeakReference(target);


          Reference = new WeakReference(target);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakAction's
        ///     method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        ///     Gets the name of the method that this WeakAction represents.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                if (_staticAction != null)
                {
                    return _staticAction.Method.Name;
                }


                return Method.Name;
            }
        }

        /// <summary>
        ///     Gets or sets a WeakReference to this WeakAction's action's target.
        ///     This is not necessarily the same as
        ///     <see cref="Reference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference ActionReference { get; set; }

        /// <summary>
        ///     Gets or sets a WeakReference to the target passed when constructing
        ///     the WeakAction. This is not necessarily the same as
        ///     <see cref="ActionReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the WeakAction is static or not.
        /// </summary>
        public bool IsStatic
        {
            get { return Method.IsStatic; }
        }

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

        protected object ActionTarget
        {
            get
            {
                if (ActionReference == null)
                {
                    return null;
                }

                return ActionReference.Target;
            }
        }

        #endregion

        public void Execute(object @params)
        {
            //if (_staticAction != null)
            //{
            //    _staticAction();
            //    return;
            //}

            var actionTarget = ActionTarget;


            if (Method != null )
            {
              
                Method.Invoke(actionTarget, new[] {@params});

                // ReSharper disable RedundantJumpStatement
                return;
                // ReSharper restore RedundantJumpStatement
            }
        }

        /// <summary>
        ///     Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            Method = null;
            _staticAction = null;
        }
    }
}