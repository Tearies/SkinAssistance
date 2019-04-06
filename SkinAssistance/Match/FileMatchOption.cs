using SkinAssistance.Core.MVVM;

namespace SkinAssistance.ViewModel
{
    public class FileMatchOption : ViewModelBase
    {
        private bool _isSelected;
        private string _matchName;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
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
    }
}