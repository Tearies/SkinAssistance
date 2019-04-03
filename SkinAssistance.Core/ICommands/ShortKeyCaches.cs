using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SkinAssistance.Core.Refrecter;

namespace SkinAssistance.Core.ICommands
{
    [XmlRoot("ShortKeys")]
    public class ShortKeyCaches
    {
        private static readonly Type thisType;
        static ShortKeyCaches()
        {
            thisType = typeof(ShortKeyCaches);
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ShortKeyCaches()
        {
            ShortKeys = new List<ShortKeyCache>();
        }

        [XmlAnyElement("ShortKeysXmlComment")]
        public XmlLinkedNode ShortKeysXmlComment { get { return thisType.GetXmlComment(); } set { } }

        [XmlElement(ElementName = "ShortKey")]
        [XmlComment("Name和Description 都不能修改! 列表项只能修改,不能新增和删除,定义的快捷键不能和其他程序所定义的重复,Key 有效值：System.Windows.Input.Key，详情参考:https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=netframework-4.7.2 ModifierKeys 有效值：System.Windows.Input.ModifierKeys，详情参考: https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.ModifierKeys?view=netframework-4.7.2")]
        public List<ShortKeyCache> ShortKeys { get; set; }

        public void Add(string name, string description, params ShortKey[] shortKeys)
        {
            ShortKeys.Add(new ShortKeyCache()
            {
                ShortKeyKey = name,
                Description = description,
                ShortKeys = new List<ShortKeyItem>(shortKeys.Select(o => new ShortKeyItem()
                {
                    Key = o.Key,
                    ModifierKey = o.ModifierKey
                }))
            });
        }
       
    }


}