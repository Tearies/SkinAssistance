using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SkinAssistance.Core.ICommands;
using SkinAssistance.ViewModel;

namespace SkinAssistance.Commands
{
    public static class SkinAssistanceCommands
    {
        public static readonly RelayCommand<IOperation> SwitchOperationCommands = new RelayCommand<IOperation>(nameof(SwitchOperationCommands),nameof(SwitchOperationCommands));

        public static readonly RelayCommand<object> SelecDirectoryCommands = new RelayCommand<object>(nameof(SelecDirectoryCommands), nameof(SelecDirectoryCommands));
    }
}
