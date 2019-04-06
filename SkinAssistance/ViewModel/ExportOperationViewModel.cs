﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
    public class ExportOperationViewModel : ViewModelBase
    {
        private string _findDir;
        private ObservableCollection<FileMatchOption> _fileMatcheOptions;
        private ObservableCollection<string> _detailsInfo;
        private static readonly object locker = new object();

        public ExportOperationViewModel()
        {
            DetailsInfo = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(DetailsInfo, locker);
            FileMatcheOptions = new ObservableCollection<FileMatchOption>();
            FileMatcheOptions.Add(InstanseManager.ResolveService<BrushMatchOption>());
            FileMatcheOptions.Add(InstanseManager.ResolveService<ImageMatchOption>());
            SkinAssistanceCommands.SelecDirectoryCommands.RegistorCommand(this, OnSelecDirectoryCommandsExcuted, OnSelecDirectoryCommandsCanExuted);
            SkinAssistanceCommands.StartSearchCommands.RegistorCommand(this, OnStartSearchCommandsExcuted,
                OnStartSearchCommandsCanExcuted);
            SkinAssistanceCommands.ShowDetailsInformationCommands.RegistorCommand(this,
                OnShowDetailsInformationCommandsExcuted, OnShowDetailsInformationCommandsCanExcuted);
        }

        private bool OnShowDetailsInformationCommandsCanExcuted(string arg)
        {
            return true;
        }

        private void OnShowDetailsInformationCommandsExcuted(string obj)
        {
            DetailsInfo.Add(obj);
        }

        private bool OnStartSearchCommandsCanExcuted(object arg)
        {
            return !string.IsNullOrEmpty(FindDir.ToSafeString()) && FileMatcheOptions.Any(o => o.IsSelected);
        }

        private async void OnStartSearchCommandsExcuted(object obj)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var options = FileMatcheOptions.Where(o => o.IsSelected);
                    var fileList = Directory.GetFiles(FindDir, "*.xaml", SearchOption.AllDirectories);
                    foreach (var file in fileList)
                    {
                        SkinAssistanceCommands.ShowInformationCommands.ExcuteCommand<string>($"begin analyze {file}");
                        FileContentMatchEngine.Instance.Match(file, options);
                        SkinAssistanceCommands.ShowInformationCommands.ExcuteCommand<string>($"end analyze {file} ");
                    }
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

        public ObservableCollection<FileMatchOption> FileMatcheOptions
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