namespace SkinAssistance.Core.Refrecter
{
    internal class TypeInstanceInfo
    {
        #region Properties

        internal bool IsSingleInstance { get; private set; }
        internal string InstanceName { get; private set; }

        #endregion

        #region Method

        public TypeInstanceInfo(bool isSingleInstance, string instanceName)
        {
            IsSingleInstance = isSingleInstance;
            InstanceName = instanceName;
        }

        #endregion
    }
}