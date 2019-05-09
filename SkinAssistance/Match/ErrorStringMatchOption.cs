namespace SkinAssistance.ViewModel
{
    [Match(typeof(ErrorStringMatch))]
    public class ErrorStringMatchOption : FileMatchOption
    {
        public ErrorStringMatchOption() : base("Error(\"*\")")
        {
        }
    }

    [Match(typeof(CustermErrorStringMatch))]
    public class CustermErrorStringMatchOption : FileMatchOption
    {
        public CustermErrorStringMatchOption() : base("Error(\"*\",")
        {
        }
    }
}