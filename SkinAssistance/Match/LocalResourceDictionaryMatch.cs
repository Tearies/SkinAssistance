using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Scripting;
using SkinAssistance.Commands;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public class LocalResourceDictionaryMatch : IMatch
    {
        private Regex errorStringRegx;
        public LocalResourceDictionaryMatch()
        {

            errorStringRegx = new Regex("ResourceManager.Default.GetResource\\((.*?)\"\\)");

        }

        #region Implementation of IMatch

        /// <summary>
        /// 匹配文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="option"></param>
        public void Match(IMatchCount matchCount, string fileName, IMatchOption option)
        {
            var allfileLines = File.ReadAllLines(fileName);
            var length = "ResourceManager.Default.GetResource(".Length;
            foreach (var line in allfileLines)
            {

                MatchCollection mc = errorStringRegx.Matches(line);
                foreach (Match m in mc)
                {
                    matchCount.MatchesCount++;
                    var tempValue = m.Value.Substring(length, m.Value.Length - length - 2);
                    int index = tempValue.IndexOf(',');
                    try
                    {
                        var key = "";
                        var value = "";
                        if (m.Value != "ResourceManager.Default.GetResource(\"\",\"\")")
                        {
                            key = tempValue.Substring(1, index - 2);
                            value = tempValue.Substring(index + 3, tempValue.Length - index - 3);
                            SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand($"{m.Value}=>{key}<-->{value}");
                        }
                        ResourceManager.Default.GetResource(key,value);
                    }
                    catch
                    {
                        SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand($"{m.Value}=>{tempValue}!!!!!!");
                    }


                }

            }

        }


    }

    #endregion

}
