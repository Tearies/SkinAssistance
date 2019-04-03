namespace SkinAssistance.Core.Native
{
    public class ThreadHelper
    {
        public static string GetCurrentThreadID()
        {
            return NativeMethods.GetCurrentThreadId().ToString().PadLeft(5, ' ');
        }
    }
}