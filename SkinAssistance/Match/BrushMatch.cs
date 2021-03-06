﻿using System;
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
        private Regex brushesRegx;
        public BrushMatch()
        {
            brushesRegx = new Regex("[\"](.*?)[\"]");
            converter = new BrushConverter();
            SkipFile = new List<string>();
            SkipFile.Add("Colors");
            SkipFile.Add("Brushes");
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

        public void Match(IMatchCount matchCount,string fileName, IMatchOption option)
        {
            if (SkipFile.Contains(Path.GetFileNameWithoutExtension(fileName)))
            {
                return;
            }
            var allfileLines = File.ReadAllLines(fileName);
            var newFile = fileName;
            if (option.GetOption<bool>("ReplaceInNewFile"))
            {
                newFile = ProductInfo.DirectoryPath + @"\Relink\" + Path.GetFileName(fileName) + "_Relink" + Path.GetExtension(fileName);
            }

            var newContent = new StringBuilder();

            Brush brush;
            bool replceContent = false;
            bool replaced = false;
            var fileShortName = Path.GetFileNameWithoutExtension(fileName);
            foreach (var line in allfileLines)
            {
                string source = line;
            Analyze:
                {
                    replceContent = false;
                    MatchCollection mc = brushesRegx.Matches(source);
                    foreach (Match m in mc)
                    {
                        var value = m.Value.Trim('"');
                        if (MatchBrushes<Brush>(value, out brush))
                        {
                            matchCount.MatchesCount++;
                            replaced = true;
                            replceContent = true;
                            var resourceid = option.GetOption<string>("ResourceKeyPrefix") + "_"+fileShortName+"_" + Guid.NewGuid().ToString("N");
                            var replaceLink = $"\"{{DynamicResource {resourceid}}}\"";
                            source = source.Substring(0, m.Index) + replaceLink + source.Substring(m.Index + m.Length, source.Length - m.Index - m.Length);
                            SkinAssistanceCommands.AddToGlobalRelinkReourceCommand.ExcuteCommand(new Tuple<string, Brush>(resourceid, brush));
                            SkinAssistanceCommands.ShowDetailsInformationCommand.ExcuteCommand($"{fileName}->>{line}|{source}");
                            break;
                        }
                    }
                    if (replceContent)
                        goto Analyze;
                }
                newContent.AppendLine(source);
            }

            if (replaced)
            {
                newFile.PrepaireDictoryInfo();
                File.WriteAllText(newFile, newContent.ToString());
            }
            newContent.Clear();
        }
    }
}