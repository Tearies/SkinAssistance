using System;
using System.Collections.Generic;
using System.Linq;

namespace SkinAssistance.Core.Extensions
{
    public abstract class MethodInjector<T>
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected MethodInjector()
        {
            ActionList = new List<Action<T>>();
        }

        private List<Action<T>> ActionList { get; set; }

        internal void MethodInject(T tparam, Action<T> action,bool logResult)
        { 
            BuildMethodList(action,logResult);
            if (ActionList.Any())
            {
                ActionList.ForEach(o =>
                    {
                        try
                        {
                            o.Invoke(tparam);
                        }
                        finally
                        {

                        }
                    }
                );
            }
        }

        protected abstract void BuildMethodList(Action<T> action, bool logResult);
         
        internal MethodInjector<T> Registor(Action<T> action)
        {
            ActionList.Add(action);
            return this;
        } 
    }
}