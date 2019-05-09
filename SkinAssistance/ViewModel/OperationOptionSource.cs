using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SkinAssistance.ViewModel
{
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