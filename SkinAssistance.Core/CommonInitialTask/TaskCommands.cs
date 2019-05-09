using SkinAssistance.Core.ICommands;

namespace SkinAssistance.Core.CommonInitialTask
{
    public class TaskCommands
    {
        public static readonly RelayCommand<string> SplashMessageCommand = new RelayCommand<string>();
    }
}