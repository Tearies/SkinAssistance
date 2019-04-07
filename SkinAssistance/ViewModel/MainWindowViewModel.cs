using System.Collections.ObjectModel;
using System.Windows;
using SkinAssistance.Commands;
using SkinAssistance.Core.Instance;
using SkinAssistance.Core.MVVM;
using SkinAssistance.View;

namespace SkinAssistance.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private FrameworkElement _content;
        private ObservableCollection<IOperation> _operationSource;
        private string _information;

        public FrameworkElement Content
        {
            get => _content;
            set
            {
                if (Equals(value, _content)) return;
                _content = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IOperation> OperationSource
        {
            get => _operationSource;
            set
            {
                if (Equals(value, _operationSource)) return;
                _operationSource = value;
                OnPropertyChanged();
            }
        }

        public string Information
        {
            get => _information;
            set
            {
                if (value == _information) return;
                _information = value;
                OnPropertyChanged();
            }
        }
        public MainWindowViewModel()
        {
            OperationSource = new ObservableCollection<IOperation>();
            OperationSource.Add(new Operation("提取资源", typeof(ExportOperationView))
            {
                IsEnabled = true
            });
            OperationSource.Add(new Operation("皮肤制作", typeof(SkinOperationView))
           );
            SkinAssistanceCommands.SwitchOperationCommands.RegistorCommand(this, OnSwitchOperationCommandsExcuted,
                OnSwitchOperationCommandsCanExcuted);
            SkinAssistanceCommands.ShowInformationCommands.RegistorCommand(this, OnShowInformationCommandsExcuted,
                OnShowInformationCommandsCanExcuted);
        }

        private bool OnShowInformationCommandsCanExcuted(string arg)
        {
            return true;
        }

        private void OnShowInformationCommandsExcuted(string obj)
        {
            Information = obj;
        }

        private bool OnSwitchOperationCommandsCanExcuted(IOperation arg)
        {
            return true;
        }

        private void OnSwitchOperationCommandsExcuted(IOperation obj)
        {
            Content = InstanseManager.ResolveService<FrameworkElement>(obj.OperationViewType);
        }
    }
}