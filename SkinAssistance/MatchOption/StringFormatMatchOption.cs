namespace SkinAssistance.ViewModel
{
    [Match(typeof(StringFormatStringMatch))]
    public class StringFormatMatchOption : FileMatchOption
    {
        public StringFormatMatchOption() : base("Format(\"*\",,")
        { 
        }
    }
}