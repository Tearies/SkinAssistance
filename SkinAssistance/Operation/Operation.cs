using System;
using SkinAssistance.Core.MVVM;

namespace SkinAssistance.ViewModel
{
    public class Operation : ViewModelBase, IOperation
    {
        public Operation(string optionName, Type operationViewType)
        {
            _optionName = optionName;
            _operationViewType = operationViewType;
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
        private static EventHandler<bool> _selected;
        public static event EventHandler<bool> Selected
        {
            add { _selected += value; }
            remove { _selected -= value; }
        }

        private bool _isSelected;
        private bool _isEnabled;

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
        private string _optionName;
        private Type _operationViewType;

        public string OptionName
        {
            get => _optionName;
            set
            {
                if (value == _optionName) return;
                _optionName = value;
                OnPropertyChanged();
            }
        }

        public Type OperationViewType
        {
            get => _operationViewType;
            set
            {
                if (Equals(value, _operationViewType)) return;
                _operationViewType = value;
                OnPropertyChanged();
            }
        }
        private static void OnSelected(Operation obj, bool e)
        {
            var handler = _selected;
            handler?.Invoke(obj, e);
        }
    }
}