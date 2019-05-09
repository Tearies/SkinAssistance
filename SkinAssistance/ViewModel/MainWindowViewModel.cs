using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
        private OperationOptionSource _operationSource;
        private string _information;
        private string _runTimes;
        private Stopwatch RunTimerSource;
        private CancellationTokenSource CancellationTokenSource;
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

        public OperationOptionSource OperationSource
        {
            get => _operationSource;
            set
            {
                if (Equals(value, _operationSource)) return;
                _operationSource = value;
                OnPropertyChanged();
            }
        }

        private long _matchsCount;

        public long MatchsCount
        { 
            get => _matchsCount;
            set
            {
                if (value == _matchsCount) return;
                _matchsCount = value;
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

        public string RunTimes
        {
            get => _runTimes;
            set
            {
                if (value.Equals(_runTimes)) return;
                _runTimes = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            RunTimes = TimeSpan.FromMilliseconds(0).ToString(@"hh\:mm\:ss\.ff");
            RunTimerSource = new Stopwatch();
            OperationSource = new OperationOptionSource();
            OperationSource.Add(new Operation("提取颜色", typeof(ExportOperationView))
            {
                IsEnabled = true
            });
            OperationSource.Add(new Operation("皮肤制作", typeof(SkinOperationView)));
            OperationSource.Add(new Operation("字符串资源提取", typeof(StringReourceOperationView))
            {
                IsEnabled = true
            });
            SkinAssistanceCommands.SwitchOperationCommand.RegistorCommand(this, OnSwitchOperationCommandsExcuted,
                OnSwitchOperationCommandsCanExcuted);
            SkinAssistanceCommands.ShowInformationCommand.RegistorCommand(this, OnShowInformationCommandsExcuted,
                OnShowInformationCommandsCanExcuted);
            SkinAssistanceCommands.StartRealTimerCommand.RegistorCommand(this, OnStartRealTimerExcuted, OnStartRealTimerCanExcuted);
            SkinAssistanceCommands.UpdateMatchesCountCommand.RegistorCommand(this,
                OnUpdateMatchesCountCommandExcuted, OnUpdateMatchesCountCommandCanExcuted);
        }

        private bool OnUpdateMatchesCountCommandCanExcuted(long arg)
        {
            return true;
        }

        private void OnUpdateMatchesCountCommandExcuted(long obj)
        {
            MatchsCount = obj;
        }

        private bool OnStartRealTimerCanExcuted(bool arg)
        {
            return true;
        }

        private void OnStartRealTimerExcuted(bool obj)
        {

            if (RunTimerSource.IsRunning)
            {
                RunTimerSource.Stop();
            }

            if (CancellationTokenSource != null)
            {
                if (!CancellationTokenSource.IsCancellationRequested)
                {
                    CancellationTokenSource.Cancel(false);
                    CancellationTokenSource.Dispose();
                }
            }
            if (obj)
            {
                CancellationTokenSource = new CancellationTokenSource();
                RunTimerSource.Start();
                ThreadPool.QueueUserWorkItem((p) =>
                {
                    while (!CancellationTokenSource.IsCancellationRequested)
                    {
                        Thread.Sleep(30);
                        RunTimes = RunTimerSource.Elapsed.ToString(@"hh\:mm\:ss\.ff");
                    }
                }, CancellationTokenSource.Token);
            }
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
            obj.IsSelected = true;
            Content = InstanseManager.ResolveService<FrameworkElement>(obj.OperationViewType);
        }
    }
}