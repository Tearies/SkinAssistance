﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
    public class ExportOperationViewModel : ViewModelBase
    {
        private string _findDir;
        private FileMatchOptionSource _fileMatcheOptions;
        private ObservableCollection<string> _detailsInfo;
        private static readonly object locker = new object();
        private GlobalRelinkSource globalRelink;
        private BrushMatchOptionOption matchOption;
        private string _resourcePrefix;

        public ExportOperationViewModel()
        {
            matchOption = InstanseManager.ResolveService<BrushMatchOptionOption>(isNew: false, initializeCallback: p =>
                  {
                      p.ReplaceInNewFile = false;
                  });
            DetailsInfo = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(DetailsInfo, locker);
            FileMatcheOptions = new FileMatchOptionSource();
            FileMatcheOptions.Add(InstanseManager.ResolveService<BrushMatchOption>(initializeCallback: p =>
            {
                p.IsEnabled = true;
            }));
            FileMatcheOptions.Add(InstanseManager.ResolveService<ImageMatchOption>());
            SkinAssistanceCommands.SelecDirectoryCommand.RegistorCommand(this, OnSelecDirectoryCommandsExcuted, OnSelecDirectoryCommandsCanExuted);
            SkinAssistanceCommands.StartSearchCommand.RegistorCommand(this, OnStartSearchCommandsExcuted,
                OnStartSearchCommandsCanExcuted);
            SkinAssistanceCommands.ShowDetailsInformationCommand.RegistorCommand(this,
                OnShowDetailsInformationCommandsExcuted, OnShowDetailsInformationCommandsCanExcuted);
            globalRelink = InstanseManager.ResolveService<GlobalRelinkSource>();

        }

        public string ResourcePrefix
        {
            get => _resourcePrefix;
            set
            {
                if (value == _resourcePrefix) return;
                _resourcePrefix = value;
                OnPropertyChanged();
                matchOption = InstanseManager.ResolveService<BrushMatchOptionOption>(isNew: false, initializeCallback: p =>
                {
                    p.ReplaceInNewFile = false;
                    p.ResourceKeyPrefix = value;
                });
            }
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

        private bool OnStartSearchCommandsCanExcuted(object arg)
        {
            return !string.IsNullOrEmpty(FindDir.ToSafeString()) && FileMatcheOptions.Any(o => o.IsSelected);
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
                    var fileList = Directory.GetFiles(FindDir, "*.xaml", SearchOption.AllDirectories);
                    SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"Analyze Start");

                    foreach (var file in fileList)
                    {
                        SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"analyze {file} start");
                        FileContentMatchEngine.Instance.Match(file, options, matchOption);
                        SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"analyze {file} end");
                    }

                    globalRelink.Save();
                    SkinAssistanceCommands.StartRealTimerCommand.ExcuteCommand(false);
                    SkinAssistanceCommands.ShowInformationCommand.ExcuteCommand<string>($"Analyze End");
                }
                catch (Exception e)
                {
                    this.Error(e);
                }
            });
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