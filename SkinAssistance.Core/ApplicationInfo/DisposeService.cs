using System;
using System.Collections.Generic;
using System.Linq;
using SkinAssistance.Core.Instance;

namespace SkinAssistance.Core.ApplicationInfo
{
    public class DisposeService
    {
        public DisposeService()
        {
            Task = new List<DisposeTask>();
        }

        private List<DisposeTask> Task { get; set; }

        public void Dispose()
        {
            if (Task.Any())
            {
                foreach (var task in Task)
                {
                    try
                    {
                        task.Dispose();
                    }
                    catch (Exception ex)
                    {
                        this.Error(ex);
                    }
                    finally
                    {
                        this.Info("Dispose Service" + task.GetType().FullName);
                    }
                }
                Task.RemoveAll(p => true);
            }


        }

        public DisposeService RegisterTask<T>() where T : DisposeTask
        {
            var t = ActivatorWrapper.SolveInstance<T>();
            if (Task.Any(o => o.GetType() == typeof(T)))
            {
                this.Info(typeof(T).FullName + " is already registed!!");
            }
            this.Info("Registed Dispose Service" + typeof(T).FullName);
            Task.Add(t);
            return this;
        }
    }
}