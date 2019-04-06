﻿using System;
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
    }
}