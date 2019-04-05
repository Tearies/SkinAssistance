#region NS

using System;
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
        public static T ResolveService<T>(Type targetType=null,bool isNew = false) where T : class
        {
            if (targetType == null)
                targetType = typeof(T);
            T instance = null;
            if (!isNew && InstanceList.Any(o => o.IsAssignedFrom(targetType)))
                instance = InstanceList.FirstOrDefault(o => o.IsAssignedFrom(targetType)) as T;
            else
                instance = ActivatorWrapper.SolveInstance(targetType) as T;
            if (!isNew)
                InstanceList.Add(instance);
            return instance;
        }

         
       
        #endregion
    }
}