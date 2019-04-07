using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using SkinAssistance.Commands;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.ICommands;
using SkinAssistance.Core.Refrecter;

namespace SkinAssistance.ViewModel
{
    public class BrushMatch : IMatch
    {

        private BrushConverter converter;
        public BrushMatch()
        {
            converter = new BrushConverter();
        }

        public void Match(string fileName)
        {
            var allfileLines = File.ReadAllLines(fileName);
            var newFile = ProductInfo.DirectoryPath + @"\Relink\" + Path.GetFileName(fileName) + "_Relink" + Path.GetExtension(fileName);
            var newContent = new StringBuilder();
            foreach (var line in allfileLines)
            {
                string source = line;
                Regex reg = new Regex("[\"](.*?)[\"]");
                MatchCollection mc = reg.Matches(source);
                foreach (Match m in mc)
                {
                    var value = m.Value.Trim('"');
                    Brush brush;
                    if (MatchBrushes<Brush>(value, out brush))
                    {
                        var resourceid = Guid.NewGuid().ToString("N");
                        var resourceLink = $"<SolidColorBrush x:Key=\"{resourceid}\" Color=\"{value}\" />";
                        var replaceLink = $"\"{{DynamicResource {resourceid}}}\"";
                        try
                        {
                            source = source.Substring(0, m.Index) + replaceLink +
                                     source.Substring(m.Index + m.Length, source.Length - m.Index - m.Length);
                            SkinAssistanceCommands.AddToGlobalRelinkReourceCommand.ExcuteCommand(new Tuple<string, Brush>(resourceid, brush));
                        }
                        catch (Exception e)
                        {

                        }

                        SkinAssistanceCommands.ShowDetailsInformationCommands.ExcuteCommand($"{line}|{source}");
                    }
                }

                newContent.AppendLine(source);
            }

            newFile.PrepaireDictoryInfo();
            File.WriteAllText(newFile, newContent.ToString());
        }

        private bool MatchBrushes<T>(string value, out T outBack) where T : Brush
        {
            try
            {
                outBack = converter.ConvertFromString(value) as T;
                return true;
            }
            catch
            {
                outBack = default(T);
                return false;
            }
        }
    }
}