namespace SkinAssistance.ViewModel
{
    [Match(typeof(ImageMatch))]
    public class ImageMatchOption : FileMatchOption
    {
        public ImageMatchOption() : base("图片")
        {
        }
    }
}