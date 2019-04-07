using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using SkinAssistance.Core.ICommands;
using SkinAssistance.ViewModel;

namespace SkinAssistance.Commands
{
    public static class SkinAssistanceCommands
    {
        public static readonly RelayCommand<IOperation> SwitchOperationCommands = new RelayCommand<IOperation>(nameof(SwitchOperationCommands),nameof(SwitchOperationCommands));

        public static readonly RelayCommand<object> SelecDirectoryCommands = new RelayCommand<object>(nameof(SelecDirectoryCommands), nameof(SelecDirectoryCommands));

        public static readonly RelayCommand<object> StartSearchCommands = new RelayCommand<object>(nameof(StartSearchCommands), nameof(StartSearchCommands));

        public static readonly RelayCommand<string> ShowInformationCommands = new RelayCommand<string>(nameof(ShowInformationCommands), nameof(ShowInformationCommands));
         
        public static readonly RelayCommand<string> ShowDetailsInformationCommands = new RelayCommand<string>(nameof(ShowDetailsInformationCommands), nameof(ShowDetailsInformationCommands));

        public static readonly RelayCommand<Tuple<string, Brush>> AddToGlobalRelinkReourceCommand =
            new RelayCommand<Tuple<string, Brush>>(nameof(AddToGlobalRelinkReourceCommand), nameof(AddToGlobalRelinkReourceCommand));
    }
}
