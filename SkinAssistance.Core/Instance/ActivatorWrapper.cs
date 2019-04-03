#region NS

using System;

#endregion

namespace SkinAssistance.Core.Instance
{
    public class ActivatorWrapper
    {
        #region Method

        private static object CreateInstance(Type type, params object[] @params)
        {
            var instanceInfo = InstanseCore.GetTypeInstanceInfo(type);
            return (instanceInfo.IsSingleInstance
                ? type.GetProperty(instanceInfo.InstanceName).GetValue(null, null)
                : Activator.CreateInstance(type, @params));
        }

        public static T SolveInstance<T>(object[] @params = null) where T : class
        {
            return SolveInstance(typeof(T), @params) as T;
        }

        public static object SolveInstance(Type bindingType, params object[] @params)
        {
            var t = @params == null ? CreateInstance(bindingType) : CreateInstance(bindingType, @params);
            return t;
        }

        #endregion
    }
}