using System;
using System.IO;
using System.Text;

namespace SkinAssistance.Core
{
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