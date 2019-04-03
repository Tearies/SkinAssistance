using System;
using System.IO;
using System.Text;
using NLog.Config;

namespace SkinAssistance.Core
{
    public static class FileHelper
    {
         
      

     

       
        
 
        #region 获取文件相对路径映射的物理路径
        /// <summary>
        /// 获取文件相对路径映射的物理路径，若文件为绝对路径则直接返回
        /// </summary>
        /// <param name="relativePath">文件的相对路径</param>        
        public static string GetPhysicalPath(string relativePath)
        {
            //有效性验证
            if (string.IsNullOrEmpty(relativePath))
            {
                return string.Empty;
            }
            //~,~/,/,\
            relativePath = relativePath.Replace("/", @"\").Replace("~", string.Empty).Replace("~/", string.Empty);
            //网络共享目录中的文件不应移除根路径
            if (!relativePath.StartsWith("\\\\"))
                relativePath = relativePath.StartsWith("\\") ? relativePath.Substring(1, relativePath.Length - 1) : relativePath;
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullPath = System.IO.Path.Combine(path, relativePath);
            return fullPath;
        }
        #endregion

        
 
    }
    public class NLogger : ILogger
    {
        private NLog.ILogger _DefaultLogger;

        public NLogger()
        {
            this.LoadLogConfig();
        }

        private void LoadLogConfig()
        {
            NLog.Config.XmlLoggingConfiguration config = new XmlLoggingConfiguration(FileHelper.GetPhysicalPath("config\\nlog.config"), false);
            var factory = new NLog.LogFactory(config);
            _DefaultLogger = factory.GetLogger("DefaultLog");
        }

        /// <summary>
        /// 更改日志存储路径
        /// </summary>
        /// <param name="name"></param>
        public void UpdateLogPath(Func<string, string> func)
        {
            NLog.Config.XmlLoggingConfiguration config = new XmlLoggingConfiguration(FileHelper.GetPhysicalPath("config\\nlog.config"), true);
            foreach (var item in config.AllTargets)
            {
                var ftarget = item as NLog.Targets.FileTarget;
                if (ftarget == null) continue;
                var lay = ftarget.FileName as NLog.Layouts.SimpleLayout;
                lay.Text = func(lay.Text);
            }
            var factory = new NLog.LogFactory(config);
            _DefaultLogger = factory.GetLogger("DefaultLog");
        }

        public void Info(string message)
        {
            _DefaultLogger.Info(message);
        }

        public void Warn(string message)
        {
            _DefaultLogger.Warn(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _DefaultLogger.Error(message + Environment.NewLine + ex.ToString());
        }

        public void Debug(string message)
        {
            _DefaultLogger.Debug(message);
        }

        /// <summary>
        /// 记录Trace信息
        /// </summary>
        /// <param name="message"></param>
        public void Trace(string message)
        {

            _DefaultLogger.Trace(message);
        }
    }
    public interface ILogger
    {
        /// <summary>
        /// 记录消息日志
        /// </summary>
        void Info(string message);

        /// <summary>
        /// 向日志文件中写入 “警告内容”。
        /// </summary>
        void Warn(string message);

        /// <summary>
        /// 记录错误消息
        /// </summary>
        void Error(string message, Exception ex = null);

        /// <summary>
        /// 记录调试信息
        /// </summary>
        void Debug(string message);

        /// <summary>
        /// 记录Trace信息
        /// </summary>
        /// <param name="message"></param>
        void Trace(string message);
    }
    public static class Log
    {
        private static ILogger InnerLoger;

        static Log()
        {
            InnerLoger = new NLogger();
        }

        /// <summary>
        /// 更改日志存储路径
        /// </summary>
        public static void UpdateLogPath(Func<string, string> func)
        {
            (InnerLoger as NLogger).UpdateLogPath(func);
        }

        /// <summary>
        /// 记录消息日志
        /// </summary>
        public static void Info(string message)
        {
            InnerLoger.Info(message);
        }
        /// <summary>
        /// 向日志文件中写入 “警告内容”。
        /// </summary>
        public static void Warn(string message)
        {
            InnerLoger.Warn(message);
        }
        /// <summary>
        /// 记录错误消息
        /// </summary>
        public static void Error(string message, Exception ex = null)
        {
            InnerLoger.Error(message, ex);
        }
        /// <summary>
        /// 记录调试信息
        /// </summary>
        public static void Debug(string message)
        {
            InnerLoger.Debug(message);
        }

        /// <summary>
        /// 向日志文件中写入Trace
        /// </summary>
        public static void Trace(string message)
        {
            InnerLoger.Trace(message);
        }
    }
}