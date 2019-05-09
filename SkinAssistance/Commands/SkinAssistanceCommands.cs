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
        public static readonly RelayCommand<IOperation> SwitchOperationCommand = new RelayCommand<IOperation>(nameof(SwitchOperationCommand), nameof(SwitchOperationCommand));

        public static readonly RelayCommand<object> SelecDirectoryCommand = new RelayCommand<object>(nameof(SelecDirectoryCommand), nameof(SelecDirectoryCommand));

        public static readonly RelayCommand<object> StartSearchCommand = new RelayCommand<object>(nameof(StartSearchCommand), nameof(StartSearchCommand));

        public static readonly RelayCommand<string> ShowInformationCommand = new RelayCommand<string>(nameof(ShowInformationCommand), nameof(ShowInformationCommand));

        public static readonly RelayCommand<string> ShowDetailsInformationCommand = new RelayCommand<string>(nameof(ShowDetailsInformationCommand), nameof(ShowDetailsInformationCommand));

        public static readonly RelayCommand<Tuple<string, Brush>> AddToGlobalRelinkReourceCommand =
            new RelayCommand<Tuple<string, Brush>>(nameof(AddToGlobalRelinkReourceCommand), nameof(AddToGlobalRelinkReourceCommand));

        public static readonly RelayCommand<bool> StartRealTimerCommand = new RelayCommand<bool>(nameof(StartRealTimerCommand), nameof(StartRealTimerCommand));
        public static readonly RelayCommand<long> UpdateMatchesCountCommand = new RelayCommand<long>(nameof(UpdateMatchesCountCommand), nameof(UpdateMatchesCountCommand));

    }
}
