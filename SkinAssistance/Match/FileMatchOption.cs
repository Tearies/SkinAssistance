using System;
using SkinAssistance.Core.MVVM;

namespace SkinAssistance.ViewModel
{
    public class FileMatchOption : ViewModelBase
    {
        private bool _isSelected;
        private string _matchName;
        private bool _isEnabled;
        private static EventHandler<bool> _selected;
        public static event EventHandler<bool> Selected
        {
            add { _selected += value; }
            remove { _selected -= value; }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnSelected(this, value);
                OnPropertyChanged();
            }
        }

        public string MatchName
        {
            get => _matchName;
            set
            {
                if (value == _matchName) return;
                _matchName = value;
                OnPropertyChanged();
            }
        }

        protected FileMatchOption(string matchName)
        {
            _matchName = matchName;
        }

        private static void OnSelected(FileMatchOption obj, bool e)
        {
            var handler = _selected;
            handler?.Invoke(obj, e);
        }
    }
}