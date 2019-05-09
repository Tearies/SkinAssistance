namespace SkinAssistance.ViewModel
{
    public class ErrorStringMatchMatchOption : IMatchOption
    {
        #region Implementation of IMatchOption

        public T GetOption<T>(string optionName)
        {
            return default(T);
        }

        #endregion
    }
}