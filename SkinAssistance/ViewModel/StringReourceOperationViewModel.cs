using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using SkinAssistance.Commands;
using SkinAssistance.Core;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.ICommands;
using SkinAssistance.Core.Instance;
using SkinAssistance.Core.MVVM;

namespace SkinAssistance.ViewModel
{
    public class StringReourceOperationViewModel : ViewModelBase
    {
        private string _findDir;
        private FileMatchOptionSource _fileMatcheOptions;
        private ObservableCollection<string> _detailsInfo;
        private static readonly object locker = new object();
        private ErrorStringMatchMatchOption matchoption;
        public StringReourceOperationViewModel()
        {
            matchoption = InstanseManager.ResolveService<ErrorStringMatchMatchOption>();
            DetailsInfo = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(DetailsInfo, locker);
            FileMatcheOptions = new FileMatchOptionSource();
            FileMatcheOptions.Add(InstanseManager.ResolveService<ErrorStringMatchOption>(initializeCallback: p =>
            {
                p.IsEnabled = true;
            }));
            SkinAssistanceCommands.SelecDirectoryCommand.RegistorCommand(this, OnSelecDirectoryCommandsExcuted, OnSelecDirectoryCommandsCanExuted);
            SkinAssistanceCommands.StartSearchCommand.RegistorCommand(this, OnStartSearchCommandsExcuted,
                OnStartSearchCommandsCanExcuted);
            SkinAssistanceCommands.ShowDetailsInformationCommand.RegistorCommand(this,
                OnShowDetailsInformationCommandsExcuted, OnShowDetailsInformationCommandsCanExcuted);
        
        }
        private bool OnSelecDirectoryCommandsCanExuted(object arg)
        {
            return true;
        }

        private void OnSelecDirectoryCommandsExcuted(object obj)
        {
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;
            commonOpenFileDialog.Multiselect = false;
            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FindDir = commonOpenFileDialog.FileName;
            }
        }

        private bool OnStartSearchCommandsCanExcuted(object arg)
        {
            return !string.IsNullOrEmpty(FindDir.ToSafeString()) && FileMatcheOptions.Any(o => o.IsSelected);
        }
        private bool OnShowDetailsInformationCommandsCanExcuted(string arg)
        {
            return true;
        }

        private void OnShowDetailsInformationCommandsExcuted(string obj)
        {
            this.Info(obj);
            DetailsInfo.Add(obj);
        }
        private async void OnStartSearchCommandsExcuted(object obj)
        {
            await Task.Run(() =>
            {
                try
                {
                    DetailsInfo.Clear();
                    FileContentMatchEngine.Instance.MatchesCount = 0;
                    SkinAssistanceCommands.StartRealTimerCommand.ExcuteCommand(true);
                    var options = FileMatcheOptions.Where(o => o.IsSelected);
                    var fileList = Directory.GetFiles(FindDir, "*.cs", SearchOption.AllDirectories);
                    SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"Analyze Start");

                    foreach (var file in fileList)
                    {
                        SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"analyze {file} start");
                        FileContentMatchEngine.Instance.Match(file, options, matchoption);
                        SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"analyze {file} end");
                    }

                    SkinAssistanceCommands.StartRealTimerCommand.ExcuteCommand(false);
                    SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"Analyze End");
                }
                catch (Exception e)
                {
                    this.Error(e);
                }
            });
        }

        public string FindDir
        {
            get => _findDir;
            set
            {
                if (value == _findDir) return;
                _findDir = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> DetailsInfo
        {
            get => _detailsInfo;
            set
            {
                if (Equals(value, _detailsInfo)) return;
                _detailsInfo = value;
                OnPropertyChanged();
            }
        }

        public FileMatchOptionSource FileMatcheOptions
        {
            get => _fileMatcheOptions;
            set
            {
                if (Equals(value, _fileMatcheOptions)) return;
                _fileMatcheOptions = value;
                OnPropertyChanged();
            }
        }
    }
}