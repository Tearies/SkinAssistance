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
        private List<string> SkipFile;
        public BrushMatch()
        {
            converter = new BrushConverter();
            SkipFile = new List<string>();
            SkipFile.Add("Colors");
            SkipFile.Add("Brushes");
        }

        public void Match(string fileName)
        {
            if (SkipFile.Contains(Path.GetFileNameWithoutExtension(fileName)))
            {
                return;
            }
            var allfileLines = File.ReadAllLines(fileName);
            var newFile = ProductInfo.DirectoryPath + @"\Relink\" + Path.GetFileName(fileName) + "_Relink" + Path.GetExtension(fileName);
            var newContent = new StringBuilder();
            Regex reg = new Regex("[\"](.*?)[\"]");
            Brush brush;
            bool replceContent = false;
            foreach (var line in allfileLines)
            {
                string source = line;
                MatchCollection mc = reg.Matches(source);
                foreach (Match m in mc)
                {
                    var value = m.Value.Trim('"');
                    if (MatchBrushes<Brush>(value, out brush))
                    {
                        replceContent = true;
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

            if (replceContent)
            {
                newFile.PrepaireDictoryInfo();
                File.WriteAllText(newFile, newContent.ToString());
            }
            newContent.Clear();
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