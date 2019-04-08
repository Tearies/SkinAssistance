using System;
using System.Collections.Generic;

namespace SkinAssistance.ViewModel
{
    internal class FileContentMatchEngine
    {
        public static readonly FileContentMatchEngine Instance = new Lazy<FileContentMatchEngine>(() => new FileContentMatchEngine()).Value;
     
        private FileContentMatchEngine()
        {
           
        }
         
        public void Match(string file, IEnumerable<FileMatchOption> options,MatchOption matchOption)
        {
            foreach (var option in options)
            {
                var match =MatchInstanse.ResloveMatch(option);
                match?.Match(file, matchOption);
            }
        }

        
    }
}