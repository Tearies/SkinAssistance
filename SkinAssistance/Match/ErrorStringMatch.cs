using System;
using SkinAssistance.Commands;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public class ErrorStringMatch : IMatch
    {
        #region Implementation of IMatch

        /// <summary>
        /// 匹配文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        public void Match(string fileName, IMatchOption option)
        {
            SkinAssistanceCommands.ShowDetailsInformationCommands.ExcuteCommand<string>(fileName);
        }

        #endregion
    }
}