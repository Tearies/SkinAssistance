namespace SkinAssistance.ViewModel
{
    public interface IMatch
    {
        /// <summary>
        /// 匹配文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        void Match(string fileName, IMatchOption option);
    }
}