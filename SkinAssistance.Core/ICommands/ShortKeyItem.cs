using System.Windows.Input;
using System.Xml.Serialization;

namespace SkinAssistance.Core.ICommands
{
    [XmlRoot("HotKey")]
    public class ShortKeyItem
    {
        [XmlElement("Key", Order = 1)]
        public Key Key { get; set; }
        [XmlElement("ModifierKey", Order = 4)]
        public ModifierKeys ModifierKey { get; set; }
    }
}