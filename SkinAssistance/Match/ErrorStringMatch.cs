using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.Scripting;
using SkinAssistance.Commands;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public class ErrorStringMatch : IMatch
    {
        private Regex errorStringRegx;
        private Regex justStringRegx;
        public ErrorStringMatch()
        {
            errorStringRegx = new Regex("Error\\([\"](.*?)[\"]\\)");
            justStringRegx = new Regex("[a-zA-Z0-9]+");
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
            bool replaced = false;
            var newContent = new StringBuilder();
            var fileShortName = Path.GetFileNameWithoutExtension(fileName);
            bool replceContent = false;
            var newFile = fileName;
            foreach (var line in allfileLines)
            {
                string source = line;
                Analyze:
                {
                    replceContent = false;
                    MatchCollection mc = errorStringRegx.Matches(source);
                 
                    foreach (Match m in mc)
                    {
                        matchCount.MatchesCount++;
                        var strResource = m.Value.Substring(7, m.Length - 9);
                        var strResourceKey = justStringRegx.Match(strResource).Value.ToLower();
                        replceContent = true;
                        SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand($"{m.Value}->>{strResource}|{strResourceKey}");
                        var resourceid = $"ResourceManager.Default.GetResource(\"{strResourceKey}\", \"{strResource}\"));";
                        var replaceLink = $"\"Error{resourceid});";
                        source = source.Substring(0, m.Index) + replaceLink + source.Substring(m.Index + m.Length, source.Length - m.Index - m.Length);
                        replaced = true;
                    }
                    if (replceContent)
                        goto Analyze;
                }
                newContent.AppendLine(source);
            }

            if (replaced)
            {
                //newFile.PrepaireDictoryInfo();
                //File.WriteAllText(newFile, newContent.ToString());
            }
            newContent.Clear();
        }

        #endregion
    }
}