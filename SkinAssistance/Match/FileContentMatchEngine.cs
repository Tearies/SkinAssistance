using System;
using System.Collections.Generic;
using SkinAssistance.Commands;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public interface IMatchCount
    {
        /// <summary>
        /// 目前已匹配的数量
        /// </summary>
        long MatchesCount { get; set; }
    }
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
    internal class FileContentMatchEngine: IMatchEngine
    {
        public static readonly IMatchEngine Instance = new Lazy<IMatchEngine>(() => new FileContentMatchEngine()).Value;

        private long _matchesCount;

        public long MatchesCount
        {
            get => _matchesCount;
            set
            {
                _matchesCount = value;
                SkinAssistanceCommands.UpdateMatchesCountCommand.ExcuteCommand(value);
            }
        }

        private FileContentMatchEngine()
        {
           
        }
         
        public void Match(string file, IEnumerable<IFileMatchOption> options,IMatchOption matchOption)
        {
            foreach (var option in options)
            {
                var match =MatchInstanse.ResloveMatch(option);
                match?.Match(this,file, matchOption);
            }
        }

        
    }
}