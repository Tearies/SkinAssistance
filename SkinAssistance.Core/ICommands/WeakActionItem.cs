using System;
using SkinAssistance.Core.ICommands.Helper;

namespace SkinAssistance.Core.ICommands
{
    /// <summary>
    /// Class WeakActionItem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WeakActionItem<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakActionItem{T}"/> class.
        /// </summary>
        /// <param name="excuteAction">The excute action.</param>
        /// <param name="canExcuteAction">The can excute action.</param>
        /// <param name="target">The target.</param>
        public WeakActionItem(WeakAction<T> excuteAction, WeakFunc<T, bool> canExcuteAction, object target)
        {
            ExcuteAction = excuteAction;
            CanExcuteAction = canExcuteAction;
            Target = new WeakReference(target);
        }

        /// <summary>
        /// Gets the excute action.
        /// </summary>
        /// <value>The excute action.</value>
        public WeakAction<T> ExcuteAction { get; private set; }

        /// <summary>
        /// Gets the can excute action.
        /// </summary>
        /// <value>The can excute action.</value>
        public WeakFunc<T, bool> CanExcuteAction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is alive.
        /// </summary>
        /// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
        public bool IsAlive
        {
            get
            {
                if (ExcuteAction != null)
                    return ExcuteAction.IsAlive && !ExcuteAction.IsStatic;
                return false;
            }
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public WeakReference Target { get; private set; }
    }
}