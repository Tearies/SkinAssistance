using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SkinAssistance.Core.Theming
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public static class ThemeManager
    {
        private static List<string> DependencyResource { get; set; }
        private static Uri DefaultAccentes { get; set; }
        static ThemeManager()
        {
            DependencyResource = new List<string>();
        }
         

        private static void LoadThemeAccent()
        {
          
            var accentUri = DefaultAccentes;
            var accentResource = LoadResource<AccentResourceDictionary>(accentUri);
            if (accentResource != null)
            {
                var oldThemes = AppResource.MergedDictionaries.OfType<AccentResourceDictionary>().ToList();
                AppResource.MergedDictionaries.Add(accentResource);
                if (oldThemes.Any())
                {
                    foreach (var theme in oldThemes)
                    {
                        AppResource.MergedDictionaries.Remove(theme);
                    }
                }
            }
             
        }

        /// <summary>
        /// 只修改配套颜色, 如需动态切换,需要得到控件的支持使用 DynamicResource, 而不是 StaticResource
        /// </summary>
        /// <param name="accentes"></param>
        public static void ChangeAccentes(string accentes)
        {
            DefaultAccentes = new Uri(accentes, UriKind.RelativeOrAbsolute);
            LoadThemeAccent();
        }

        /// <summary>
        /// 加载主依赖项,会清空 App.Xaml 里面的Resource.MergedDictionaries 里面的东西, 请不要再放置 rd到App.Xaml 下.
        /// </summary>
        /// <param name="accentes"></param>
        /// <param name="resources"></param>
        public static void LoadThemeResource(string accentes,params string[] resources)
        {
            DefaultAccentes = new Uri(accentes,UriKind.RelativeOrAbsolute);
            DependencyResource.Clear();
            DependencyResource.AddRange(resources);
            ClearMergedDictionaries();
            LoadThemeAccent();
            LoadResource();
        }

        /// <summary>
        /// 清空Resource.MergedDictionaries
        /// </summary>
        private static void ClearMergedDictionaries()
        {
            AppResource.MergedDictionaries.Clear();
        }

        /// <summary>
        /// 加载依赖资源
        /// </summary>
        /// <param name="resources"></param>
        private static void LoadResource()
        {
            if (DependencyResource.Any())
                foreach (var resource in DependencyResource)
                {
                    var r = LoadResource<ResourceDictionary>(new Uri(resource, UriKind.RelativeOrAbsolute));
                    if (r != null)
                        AppResource.MergedDictionaries.Add(r);
                }
        }

        private static ResourceDictionary AppResource
        {
            get { return Application.Current.Resources; }
        }

        /// <summary>
        /// 加载资源并返回相应的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static T LoadResource<T>(Uri uri) where T : ResourceDictionary
        {
            try
            {
                if (Application.LoadComponent(uri) is T accentResource)
                {
                    return accentResource;
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

    }
}
