namespace SkinAssistance.ViewModel
{
    [Match(typeof(CustermErrorStringMatch))]
    public class CustermErrorStringMatchOption : FileMatchOption
    {
        public CustermErrorStringMatchOption() : base("Error(\"*\",")
        {
        }
    }
}