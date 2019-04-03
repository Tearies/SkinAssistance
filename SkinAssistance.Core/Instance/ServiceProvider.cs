#region NS

using System;

#endregion

namespace SkinAssistance.Core.Instance
{
    public abstract class ServiceProvider :MarshalByRefObject, IServiceProvider
    {
        #region Override

        public virtual object GetService(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(GetType())) return this;
            return null;
        }

        #endregion
    }
}