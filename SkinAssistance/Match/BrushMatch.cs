using System.IO;
using SkinAssistance.Commands;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.ViewModel
{
    public class BrushMatch : IMatch
    {
        public void Match(string fileName)
        {
            var allfileLines = File.ReadAllLines(fileName);
            foreach (var line in allfileLines)
            {
                SkinAssistanceCommands.ShowDetailsInformationCommands.ExcuteCommand(line);
            }
        }
    }
}