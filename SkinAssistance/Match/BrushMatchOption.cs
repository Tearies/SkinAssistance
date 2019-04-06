namespace SkinAssistance.ViewModel
{
    [Match(typeof(BrushMatch))]
    public class BrushMatchOption : FileMatchOption
    {
        public BrushMatchOption() : base("颜色")
        {
        }
    }
}