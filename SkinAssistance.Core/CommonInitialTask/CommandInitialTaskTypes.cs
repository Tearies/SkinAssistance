#region NS

using System;
using System.Collections.Generic;
using SkinAssistance.Core.ICommands;

#endregion

namespace SkinAssistance.Core.CommonInitialTask
{
    #region Reference

    #endregion

    internal class CommandInitialTaskTypes
    {
        #region Properties

        public static List<Type> CommandInitialTask
        {
            get
            {
                return new List<Type>
                {
                    LogComponments,
                    InitialeGlobalCulturTask,
                    DispatcherContext,
                    RegistorExceptionHandlerTask,
                    ThemeTask,
                    UpdateTask
                };
            }
        }

        private static readonly Type InitialeGlobalCulturTask = typeof(InitialeGlobalCulturTask);
        private static readonly Type DispatcherContext = typeof(DispatcherContextTask);
        internal static readonly Type LogComponments = typeof(InitaileLogComponmentsTask);
        private static readonly Type UpdateTask = typeof(InitailUpdateTask);
        private static readonly Type ThemeTask = typeof(InitialThemeTask);
        private static readonly Type RegistorExceptionHandlerTask = typeof(RegistorExceptionHandlerTask);

        #endregion
    }

    public class TaskCommands
    {
        public static readonly RelayCommand<string> SplashMessageCommand = new RelayCommand<string>();
    }
}