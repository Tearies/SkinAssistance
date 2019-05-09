using System.Collections.Generic;

namespace SkinAssistance.ViewModel
{
    public interface IMatchEngine: IMatchCount
    {
       

        /// <summary>
        /// 匹配文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="options"></param>
        /// <param name="matchOption"></param>
        void Match(string file, IEnumerable<IFileMatchOption> options, IMatchOption matchOption);
    }
}