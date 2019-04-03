#region NS

using System;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

#endregion

namespace SkinAssistance.Core.InstanseContext
{
    #region Reference

    #endregion

    public class InstanceContext<TApplication> where TApplication : Application
    {
        #region Method

        public static void InitializeAsFirstInstance(Action firstInstanceRunAction,
            Action<StartupNextInstanceEventArgs> nextInstanceRunAction)
        {
            var singleInstance = new SingleInstance<TApplication>(firstInstanceRunAction, nextInstanceRunAction);
            singleInstance.Run(Environment.GetCommandLineArgs());
        }

        #endregion
    }
}