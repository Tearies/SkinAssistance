namespace SkinAssistance.ViewModel
{
    [Match(typeof(LocalResourceDictionaryMatch))]
    public class LocalResourceDictionaryMatchOption : FileMatchOption{
        public LocalResourceDictionaryMatchOption() : base("资源抽取")
        {
           
        }
    }
}