using System.Threading.Tasks;
using SkinAssistance.Core.ApplicationInfo;
using SkinAssistance.Core.CommonInitialTask;
using SkinAssistance.Core.ICommands;
using SkinAssistance.Core.Theming;

namespace SkinAssistance
{
    class InitializeThemesTask: InitializeTask
    {
       
        public override double ProgressWeight => 0xff;

        public override string TaskName => "初始化主题";

        protected override async Task<bool> InvokeExcute()
        {
            TaskCommands.SplashMessageCommand.ExcuteCommand(string.Format("执行任务：{0}", TaskName));
            ThemeManager.LoadThemeResource("/SkinAssistance;component/Themes/Brushes.xaml",
                "/SkinAssistance;component/Themes/Generic.xaml",
                "/SkinAssistance.Core;component/Themes/Generic.xaml");
            RasizeProgressChanged(ProgressWeight);
            return true;
        }
    }
}
