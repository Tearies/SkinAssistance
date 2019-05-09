namespace SkinAssistance.ViewModel
{
    [Match(typeof(ErrorStringMatch))]
    public class ErrorStringMatchOption : FileMatchOption
    {
        public ErrorStringMatchOption() : base("Error(\"")
        {
        }
    }
}