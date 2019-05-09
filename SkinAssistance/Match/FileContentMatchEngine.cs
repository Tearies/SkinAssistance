using System;
using System.Collections.Generic;
using SkinAssistance.Commands;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
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