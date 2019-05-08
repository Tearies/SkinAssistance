using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SkinAssistance.ViewModel
{
    public class FileMatchOptionSource : ObservableCollection<FileMatchOption>
    {
        private bool ChangedFromCode;
        public FileMatchOptionSource()
        {
            FileMatchOption.Selected += FileMatchOption_Selected;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void FileMatchOption_Selected(object sender, bool e)
        {
            if (ChangedFromCode)
                return;
            FileMatchOption option = sender as FileMatchOption;
            if (option != null && this.Items.Any())
            {
                ChangedFromCode = true;
                try
                {
                    foreach (FileMatchOption tmpOption in this.Items)
                    {
                        if (tmpOption.MatchName != option.MatchName)
                        {
                            tmpOption.IsSelected = false;
                        }
                    }
                }
                finally
                {
                    ChangedFromCode = false;
                }
             
             
            }
        }
    }

    public class OperationOptionSource : ObservableCollection<IOperation>
    {
        private bool ChangedFromCode;
        public OperationOptionSource()
        {
            Operation.Selected += FileMatchOption_Selected;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void FileMatchOption_Selected(object sender, bool e)
        {
            if (ChangedFromCode)
                return;
            IOperation option = sender as IOperation;
            if (option != null && this.Items.Any())
            {
                ChangedFromCode = true;
                try
                {
                    foreach (Operation tmpOption in this.Items)
                    {
                        if (tmpOption.OptionName != option.OptionName)
                        {
                            tmpOption.IsSelected = false;
                        }
                    }
                }
                finally
                {
                    ChangedFromCode = false;
                }


            }
        }
    }
}