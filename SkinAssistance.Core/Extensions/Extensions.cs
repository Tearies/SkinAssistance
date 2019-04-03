#region NS

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace SkinAssistance.Core.Extensions
{
    #region Reference

    #endregion

    public static partial class Extensions
    {

        public static string GetPhysicalPath(string relativePath)
        {
            //有效性验证
            if (string.IsNullOrEmpty(relativePath))
            {
                return string.Empty;
            }
            //~,~/,/,\
            relativePath = relativePath.Replace("/", @"\").Replace("~", string.Empty).Replace("~/", string.Empty);
            relativePath = relativePath.StartsWith("\\") ? relativePath.Substring(1, relativePath.Length - 1) : relativePath;
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullPath = System.IO.Path.Combine(path, relativePath);
            return fullPath;
        }

        #region Method
        public static T Invoke<T>(this DispatcherObject dispatcherObject, Func<T> func)
        {

            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                return func();
            }
            else
            {
                return (T)dispatcherObject.Dispatcher.Invoke(new Func<T>(func));
            }
        }

        public static void Invoke(this DispatcherObject dispatcherObject, Action invokeAction)
        {

            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                invokeAction();
            }
            else
            {
                dispatcherObject.Dispatcher.Invoke(invokeAction);
            }
        }

        /// <summary> 
        ///   Executes the specified action asynchronously with the DispatcherPriority.Background on the thread that the Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcherObject">The dispatcher object where the action runs.</param>
        /// <param name="invokeAction">An action that takes no parameters.</param>
        /// <param name="priority">The dispatcher priority.</param> 
        public static void BeginInvoke(this DispatcherObject dispatcherObject, Action invokeAction, DispatcherPriority priority = DispatcherPriority.Background)
        {

            dispatcherObject.Dispatcher.BeginInvoke(priority, invokeAction);
        }
        /// <summary>
        ///     获取枚举的Custorm属性
        /// </summary>
        /// <typeparam name="T">CustermAttribute的值</typeparam>
        /// <param name="em">枚举值</param>
        /// <returns></returns>
        public static T GetCustermAttribute<T>(this Enum em) where T : Attribute
        {
            var type = em.GetType();
            var fd = type.GetField(em.ToString());
            if (fd == null)
                return default(T);
            var attrs = fd.GetCustomAttributes(typeof(T), false);
            return attrs.Any() ? (T)attrs[0] : default(T);
        }
        public static string SafeFormat(this string stringFormat, params object[] @params)
        {
            return (@params.Length > 0 ? string.Format(stringFormat, @params) : stringFormat);
        }
        //public static T ToExactlyEnum<T>(this object data)
        //{
        //    if (data == null)
        //        return default(T);
        //    var type = typeof(T);
        //    if (!type.IsEnum)
        //    {
        //        LogExtensions.Error(null, "type is not a Enum. Type:" + type);
        //        return default(T);
        //        //throw new ArgumentException("type is not a Enum. Type:" + type);
        //    }
        //    if (Enum.IsDefined(type, data))
        //        return (T)Enum.ToObject(type, data);
        //    LogExtensions.Error(null, "invalid data to be converted to enum");
        //    return default(T);
        //    //throw new ArgumentException("invalid data to be converted to enum");
        //}
        public static T ToExactlyEnum<T>(this object name)
        {
            if (name == null)
            {
                return default(T);
            }
            var type = typeof(T);
            if (!type.IsEnum)
            {
                LogExtensions.Error(null, "type is not a Enum. Type:" + type);
                return default(T);
            }
            if (Enum.IsDefined(type, name))
            {
                return (T)Enum.Parse(type, name.ToString());
            }
            LogExtensions.Error(null, "invalid data to be converted to enum");
            return default(T);
        }

        public static bool ToBoolean<T>(this T obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return default(bool);
            }
        }

        public static string ToUIString(this Version version)
        {
            return string.Format(@"v {0:D2}.{1:D2}.{2:D2}.{3:D2}", version.Major, version.Minor, version.Build,
                version.Revision);
        }


        public static void Restart(this Application application)
        {
            if (Assembly.GetEntryAssembly() == null)
                throw new NotSupportedException("RestartNotSupported");
            var flag = false;

            var commandLineArgs = Environment.GetCommandLineArgs();
            var stringBuilder1 = new StringBuilder((commandLineArgs.Length - 1) * 16);
            for (var index = 1; index < commandLineArgs.Length - 1; ++index)
            {
                stringBuilder1.Append('"');
                stringBuilder1.Append(commandLineArgs[index]);
                stringBuilder1.Append("\" ");
            }
            if (commandLineArgs.Length > 1)
            {
                stringBuilder1.Append('"');
                var stringBuilder2 = stringBuilder1;
                var strArray = commandLineArgs;
                var index = strArray.Length - 1;
                var str = strArray[index];
                stringBuilder2.Append(str);
                stringBuilder1.Append('"');
            }
            var startInfo = Process.GetCurrentProcess().StartInfo;
            startInfo.FileName = Assembly.GetEntryAssembly().Location;
            if (stringBuilder1.Length > 0)
                startInfo.Arguments = stringBuilder1.ToString();
            application.Shutdown();
            Process.Start(startInfo);
        }

        /// <summary>
        /// 将控件打印到ImageSource
        /// </summary>
        /// <param name="elemntHost"></param>
        /// <returns></returns>
        public static BitmapSource ExportToImageSource(this FrameworkElement element)
        {
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)element.DesiredSize.Width, (int)element.DesiredSize.Height,
                96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);

            return renderTargetBitmap;
        }
        #endregion

        #region 序列化与反序列化

        /// <summary>
        ///     将XML字符串转为对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TObject ToObjectFromXml<TObject>(this string str) where TObject : class
        {
            var temp = Encoding.UTF8.GetBytes(str);
            using (var mstream = new MemoryStream(temp))
            {
                XmlTextReader xr = new XmlTextReader(mstream); xr.Namespaces = false;
                var serializer = new XmlSerializer(typeof(TObject));
                return serializer.Deserialize(xr) as TObject;
            }
        }

        /// <summary>
        ///     将XML文件内容字符串转为对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TObject ToObjectFromXmlFile<TObject>(this string filePath) where TObject : class
        {
            using (var mstream = new StreamReader(filePath, Encoding.UTF8))
            {
                XmlTextReader xr = new XmlTextReader(mstream); xr.Namespaces = false;
                var serializer = new XmlSerializer(typeof(TObject));
                return serializer.Deserialize(xr) as TObject;
            }
        }

        /// <summary>
        ///     将XML字符串转为对象(struct)
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TObject ToObjectAsStructFromXml<TObject>(this string str) where TObject : struct
        {
            var temp = Encoding.UTF8.GetBytes(str);
            using (var mstream = new MemoryStream(temp))
            {
                XmlTextReader xr = new XmlTextReader(mstream); xr.Namespaces = false;
                var serializer = new XmlSerializer(typeof(TObject));
                return (TObject)serializer.Deserialize(xr);

            }
        }

        /// <summary>
        ///     将对象转为XML字符串
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<TObject>(this TObject obj)
        {
            using (var mstream = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var serializer = new XmlSerializer(typeof(TObject));
                serializer.Serialize(mstream, obj, ns);
                return Encoding.UTF8.GetString(mstream.ToArray());
            }
        }

        /// <summary>
        ///     将对象转为XML字符串
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToXml<TObject>(this TObject obj, Encoding encoding)
        {
            var settings = new XmlWriterSettings { Encoding = encoding, Indent = true };
            var mstream = new MemoryStream();
            using (var writer = XmlWriter.Create(mstream, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var serializer = new XmlSerializer(typeof(TObject));
                serializer.Serialize(writer, obj, ns);
                return Encoding.UTF8.GetString(mstream.ToArray());
            }
        }

        /// <summary>
        ///     将对象转为XML字符串并存入XML文件中
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void ToXmlFile<TObject>(this TObject obj, string filePath)
        {
            using (var fileStream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    var serializer = new XmlSerializer(typeof(TObject));
                    serializer.Serialize(fileStream, obj, ns);
                }
                finally
                {

                }
            }
        }
 

        #endregion
    }


}