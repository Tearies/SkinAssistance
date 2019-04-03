using System;
using System.Windows.Input;

namespace SkinAssistance.Core.ICommands
{
    public interface IRegistorCommand<T> : ICommand
    {
        #region Other

        void RegistorCommand(object tartget,Action<T> canAction, Func<T, bool> _canExecute);

        void UnRegistorCommand(object tartget);
         
        #endregion


    }
}