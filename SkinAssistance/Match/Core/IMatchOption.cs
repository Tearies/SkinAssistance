namespace SkinAssistance.ViewModel
{
    public interface IMatchOption
    {
        T GetOption<T>(string optionName);
    }
}