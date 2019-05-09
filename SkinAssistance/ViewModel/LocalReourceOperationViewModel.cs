using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Scripting;
using Microsoft.WindowsAPICodePack.Dialogs;
using SkinAssistance.Commands;
using SkinAssistance.Core;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.ICommands;
using SkinAssistance.Core.Instance;
using SkinAssistance.Core.MVVM;

namespace SkinAssistance.ViewModel
{
    public class LocalReourceOperationViewModel : ViewModelBase
    {
        private string _findDir;
        private FileMatchOptionSource _fileMatcheOptions;
        private ObservableCollection<string> _detailsInfo;
        private static readonly object locker = new object();
        private LocalResourceDictionaryMatchOptionOption matchoption;
        public LocalReourceOperationViewModel()
        {
            matchoption = InstanseManager.ResolveService<LocalResourceDictionaryMatchOptionOption>();
            DetailsInfo = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(DetailsInfo, locker);
            FileMatcheOptions = new FileMatchOptionSource();
            FileMatcheOptions.Add(InstanseManager.ResolveService<LocalResourceDictionaryMatchOption>(initializeCallback: p =>
            {
                p.IsEnabled = true;
            }));

            UICultrues = new FileMatchOptionMultiSource();
            CultureInfo.GetCultures(CultureTypes.AllCultures).ToList().ForEach(o =>
            {
                if (!String.IsNullOrEmpty(o.Name))
                {
                    UICultrues.Add(InstanseManager.ResolveService<LocalCultureMatchOption>(isNew: true, initializeCallback: p =>
                    {
                        p.MatchName = o.Name;
                        p.IsEnabled = true;
                    }));
                }

            });

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
                    ResourceManager.Default.Initialize(AppName);
                    matchoption.ResourceCulture = UICultrues.Where(o => o.IsSelected).Select(o => o.MatchName).ToList();
                    DetailsInfo.Clear();
                    FileContentMatchEngine.Instance.MatchesCount = 0;
                    if (matchoption.ResourceCulture.Any())
                    {
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
                        matchoption.ResourceCulture.ForEach(o =>
                        {
                            SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand<string>($"build resource {o} start");
                            ResourceManager.Default.SaveAs(CultureInfo.GetCultureInfo(o));
                            SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand<string>($"build resource {o} end");
                        });
                        SkinAssistanceCommands.StartRealTimerCommand.ExcuteCommand(false);
                        SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"Analyze End");
                    }
                    
                    
                    
                  
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

        private FileMatchOptionMultiSource _uiCultrues;

        public FileMatchOptionMultiSource UICultrues
        {
            get => _uiCultrues;
            set
            {
                if (Equals(value, _uiCultrues)) return;
                _uiCultrues = value;
                OnPropertyChanged();
            }
        }

        private string _appName;

        public string AppName
        {
            get => _appName;
            set
            {
                if (value == _appName) return;
                _appName = value;
                matchoption.AppName = value;
                OnPropertyChanged();
            }
        }
    }
}