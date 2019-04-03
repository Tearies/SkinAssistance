using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkinAssistance.Core;
using SkinAssistance.Core.ApplicationInfo;
using SkinAssistance.Core.CommonInitialTask;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public class SplashScreenViewModel : INotifyPropertyChanged
    {
        #region Methods

        public SplashScreenViewModel()
        {
            InitializeTaskFatory.ProgressChanged += InitializeTaskFatory_ProgressChanged;
            TotalProgress = InitializeTaskFatory.ProgressWeight;
            CommandContext.Registor(TaskCommands.SplashMessageCommand, this, OnSplashMessageCommandExcuted,
                OnSplashMessageCommandCanExcuted);
        }

        private void OnSplashMessageCommandExcuted(string obj)
        {
            CurrentInfomation = obj;
        }

        private bool OnSplashMessageCommandCanExcuted(string arg)
        {
            return true;
        }

        private void InitializeTaskFatory_ProgressChanged(object sender, ProgressChangedArgs e)
        {
            CurrentProgress = e.Progress;
        }



        #endregion

        #region CurrentInfomation	

        private string _currentInfomation;

        [Description("CurrentInfomation")]
        public string CurrentInfomation
        {
            get { return _currentInfomation; }
            set
            {
                if (_currentInfomation == value)
                    return;
                _currentInfomation = value;
                OnPropertyChanged();
                this.Info(value);
            }
        }

        #endregion

        private double _currentProgress;

        public double CurrentProgress
        {
            get { return _currentProgress; }
            set { _currentProgress = value; OnPropertyChanged(); }
        }

        private double _totalProgress;

        public double TotalProgress
        {
            get { return _totalProgress; }
            set { _totalProgress = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged
                    .Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
