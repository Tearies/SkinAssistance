using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;

namespace SkinAssistance.Core.RemoteInfo
{
    public class WMIEntityItemsQuery<T>
    {
        private static readonly Dictionary<string, PropertyInfo> PropertyCache;
        private readonly ManagementClass managementClass;

        static WMIEntityItemsQuery()
        {
            PropertyCache = new Dictionary<string, PropertyInfo>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Array.ForEach(properties, o => { PropertyCache.Add(o.Name, o); });
        }

        public WMIEntityItemsQuery()
        {
            if (WMIClass.Exits<T>(out var className))
                managementClass = new ManagementClass(className);
            else
                throw new Exception("Unkown WMI Class");
        }

        public List<T> QueryItem(Predicate<T> searchFilter)
        {
            var tempItems = new List<T>();
            var collection = managementClass.GetInstances();
            foreach (var c in collection)
            {
                var tobj = Activator.CreateInstance<T>();
                foreach (var property in c.Properties)
                    if (PropertyCache.ContainsKey(property.Name))
                        PropertyCache[property.Name].SetValue(tobj, property.Value);
                if (searchFilter != null)
                {
                    if (searchFilter(tobj))
                        tempItems.Add(tobj);
                }
                else
                {
                    tempItems.Add(tobj);
                }
            }

            return tempItems;
        }

        private class WMIClass
        {
            private static readonly List<string> WMICLASSS = new List<string>();

            static WMIClass()
            {
                var searcher =
                    new ManagementObjectSearcher(new WqlObjectQuery(
                        "select * from meta_class"));
                foreach (ManagementClass wmiClass in
                    searcher.Get())
                    WMICLASSS.Add(
                        wmiClass["__CLASS"].ToString());
            }

            public static bool Exits<T>(out string className)
            {
                className = typeof(T).Name;
                var tempClassName = className;
                return WMICLASSS.Any(o => o == tempClassName);
            }
        }
    }
}