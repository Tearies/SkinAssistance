using System.Collections.Generic;
using System.Xml.Serialization;

namespace SkinAssistance.Core.ICommands
{
    [XmlRoot("ShortKey")]
    public class ShortKeyCache
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ShortKeyCache()
        {
            ShortKeys = new List<ShortKeyItem>();
        }
        [XmlAttribute("Description")]
        public string Description { get; set; }
        [XmlAttribute("Name")]
        public string ShortKeyKey { get; set; }
        [XmlElement("Keys",typeof(ShortKeyItem))]
        public List<ShortKeyItem> ShortKeys { get; set; }
    }
}