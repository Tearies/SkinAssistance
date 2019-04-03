#region NS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SkinAssistance.Core.Instance;

#endregion

namespace SkinAssistance.Core.ApplicationInfo
{
    #region Reference

    #endregion

    public class InitializeTaskFatory
    {
        #region Properties

        private static double DoneProgress { get; set; }

        /// <summary>
        ///     进度权重
        /// </summary>
        public static double ProgressWeight
        {
            get { return Tasks.Sum(o => o.ProgressWeight); }
        }

        private static readonly List<InitializeTask> Tasks;
        private static readonly Type InitaializeType;

        #endregion

        #region Method

        static InitializeTaskFatory()
        {
            Tasks = new List<InitializeTask>();
            InitaializeType = typeof(InitializeTask);
        }

        private static event EventHandler<ProgressChangedArgs> _progressEventHandler;

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void Configurate(params Type[] tasks)
        {
            Tasks.Clear();
            foreach (var task in tasks)
                if (task.IsSubclassOf(InitaializeType))
                {
                    var taskinstance = (InitializeTask)ActivatorWrapper.SolveInstance(task, null);
                    taskinstance.ProgressChanged += (s, e) => { RasizeProgressChanged(e.Progress); };
                    Tasks.Add(taskinstance);
                }
        }


        public static async Task<bool> ExcuteInitialize()
        {
            DoneProgress = 0;
            var IniteSucc = true;
            foreach (var task in Tasks)
            {
                try
                {
                    IniteSucc = await task.Excute();
                }
                catch (Exception e)
                {
                    e.Critical(e);
                    IniteSucc = false;
                }

                if (!IniteSucc)
                    break;
            }
            return IniteSucc;
        }

        public static event EventHandler<ProgressChangedArgs> ProgressChanged
        {
            add { _progressEventHandler += value; }
            remove { _progressEventHandler -= value; }
        }

        private static void RasizeProgressChanged(double progress)
        {
            DoneProgress += progress;
            var handler = _progressEventHandler;
            if (handler != null)
                handler(null, new ProgressChangedArgs(Math.Round(DoneProgress)));
        }

        #endregion
    }
}