#region NS

using SkinAssistance.Core.Instance;
using SkinAssistance.Core.InstanseContext;
using SkinAssistance.Core.Native;

#endregion

namespace SkinAssistance.Core.ApplicationInfo
{
    #region Reference

    #endregion

    internal class AppTaskGenerator<T> where T : ApplicationEntry
    {
        #region Method

        public void Run()
        {
            InstanceContext<InternalApp<T>>.InitializeAsFirstInstance(StartAppTask,
                p => { p.BringToForeground = true; });
        }

        private void StartAppTask()
        {

            ActivatorWrapper.SolveInstance<InternalApp<T>>().Run();
        }

        #endregion
    }
}