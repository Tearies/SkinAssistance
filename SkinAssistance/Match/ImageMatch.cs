namespace SkinAssistance.ViewModel
{
    public class ImageMatch : IMatch
    {
        

        public void Match(string fileName)
        {
            
        }

        public void Match(string fileName, IMatchOption option)
        {
           
        }

        #region Implementation of IMatch

        /// <summary>
        /// 匹配文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        public void Match(IMatchCount matchCount, string fileName, IMatchOption option)
        {
          
        }

        #endregion
    }
}