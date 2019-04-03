#region NS

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SkinAssistance.Core.Extensions;

#endregion

namespace SkinAssistance.Core.Instance
{
    public class InstanseManager : ServiceProvider
    {
        #region Properties

        private static readonly List<object> InstanceList;

        #endregion

        #region Method


        static InstanseManager()
        {
            InstanceList = new List<object>();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T ResolveService<T>(bool isNew = false) where T : class
        {
            T instance = null;
            if (!isNew && InstanceList.Any(o => o.IsAssignedFrom<T>()))
                instance = InstanceList.FirstOrDefault(o => o.IsAssignedFrom<T>()) as T;
            else
                instance = ActivatorWrapper.SolveInstance<T>();
            if (!isNew)
                InstanceList.Add(instance);
            return instance;
        }

        #endregion
    }
}