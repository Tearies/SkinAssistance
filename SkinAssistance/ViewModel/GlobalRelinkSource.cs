using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using SkinAssistance.Commands;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Refrecter;

namespace SkinAssistance.ViewModel
{
    public class GlobalRelinkSource
    {
        private ResourceDictionary rdic;
        public GlobalRelinkSource()
        {
            rdic = new ResourceDictionary();
            SkinAssistanceCommands.AddToGlobalRelinkReourceCommand.RegistorCommand(this, OnAddToGlobalRelinkReourceCommandExcuted, OnAddToGlobalRelinkReourceCommandCanExcuted);
        }

        private bool OnAddToGlobalRelinkReourceCommandCanExcuted(Tuple<string, Brush> arg)
        {
            return true;
        }

        private void OnAddToGlobalRelinkReourceCommandExcuted(Tuple<string, Brush> obj)
        {
            if (!rdic.Contains(obj.Item1))
            {
                rdic.Add(obj.Item1, obj.Item2);
            }
        }

        public void Save()
        {
            var filePath = ProductInfo.DirectoryPath + @"\Relink\" + "GlobalResource.xaml";
            filePath.PrepairDicInfo();
            using (var xmlWriter = new XmlTextWriter(filePath,null))
            {
                xmlWriter.Formatting = Formatting.Indented;
                XamlWriter.Save(rdic, xmlWriter);
            }

        }
    }
}