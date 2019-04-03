#region NS

using System;
using System.Reflection;

#endregion

namespace SkinAssistance.Core.Refrecter
{
    #region Reference

    #endregion

    internal class EntryAssemblyInfo : AssemblyInfos
    {
        #region Method

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public EntryAssemblyInfo()
            : base(GetEntryAssembly())
        {
        }

        private static Assembly GetEntryAssembly()
        {
            if (AppDomain.CurrentDomain != null && AppDomain.CurrentDomain.DomainManager != null)
                return AppDomain.CurrentDomain.DomainManager.EntryAssembly;


            var ma = new AppDomainManager();
            return ma.EntryAssembly;
        }

        #endregion
    }
}