using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkinAssistance.Core.Extensions;

namespace SkinAssistance.Core.ICommands
{
    public class ShortKeyManager
    {
        private static string _configFile = @"config\shortkey.config";
        private static List<ShortKeyCache> internalShortKeys;

        static ShortKeyManager()
        {
            var tempConfigPath = Extensions.Extensions.GetPhysicalPath(_configFile);
            string configContent = "";
            if (File.Exists(tempConfigPath))
                configContent = File.ReadAllText(tempConfigPath);
            if (string.IsNullOrEmpty(configContent))
            {
                Config = new ShortKeyCaches();
                UpdateDisplayConfig();
                return;
            }
            try
            {
                var config = configContent.ToObjectFromXml<ShortKeyCaches>();
                if (config != null)
                {
                    Config = config;
                }
            }
            catch (Exception e)
            {
                LogExtensions.Error(null, e);
                Config=new ShortKeyCaches();
            }
        }

        public static ShortKeyCaches Config { get; private set; }

        public static List<ShortKey> RegistorShortKey(string commandName, string description, params ShortKey[] keys)
        {
            var key = Config.ShortKeys.FirstOrDefault(o => o.ShortKeyKey == commandName);
            if (key == null)
            {
                Config.Add(commandName, description, keys);
                UpdateDisplayConfig();
                return new List<ShortKey>(keys);
            }
            return key.ShortKeys.Select(o => new ShortKey(o.Key, o.ModifierKey)).ToList();

        }

        private static void UpdateDisplayConfig()
        {
            Config.ToXmlFile(Extensions.Extensions.GetPhysicalPath(_configFile));
        }
    }
}