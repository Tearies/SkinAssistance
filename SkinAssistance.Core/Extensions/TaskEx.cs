using System;
using System.Threading;
using System.Threading.Tasks;

namespace SkinAssistance.Core.Extensions
{
    public static class TaskEx
    {
        /// <summary>自动异步执行任务,超时后自动销毁</summary>
        /// <param name="action">需要执行的方法</param>
        /// <param name="cancleOrFalseAction">取消的时间</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static void RunTask(Action action, Action<string> cancleOrFalseAction = null)
        {
            RunTaskWithAutoCancle(action, cancleOrFalseAction, 1800);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <param name="token"></param>
        private static void InternalRunTask(Action action, Action<string> cancleOrFalseAction, CancellationToken token)
        {
            Task.Run(action, token).ContinueWith(p =>
            {
                string errorMessage = "";
                if (p.IsCanceled)
                    errorMessage = "Task Canceled!";
                if (p.IsFaulted)
                {
                    errorMessage = p.Exception?.ToString() ?? "发生未知异常!";
                }

                if ((p.IsCanceled || p.IsFaulted))
                {
                    if (cancleOrFalseAction != null)
                        cancleOrFalseAction(errorMessage);
                    else
                    {
                        Log.Error(errorMessage);
                    }
                }
            }, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <param name="autocancle"></param>
        public static void RunTaskWithAutoCancle(Action action, Action<string> cancleOrFalseAction = null, int autocancle = 60)
        {
            var cancle = new CancellationTokenSource(TimeSpan.FromSeconds(autocancle));
            InternalRunTask(action, cancleOrFalseAction, cancle.Token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <param name="autocancle"></param>
        /// <returns></returns>
        private static async Task<object> IntneralRunTaskWithAutoCancle(Func<object> action, Action<string> cancleOrFalseAction = null, int autocancle = 60)
        {
            var cancle = new CancellationTokenSource(TimeSpan.FromSeconds(autocancle));
            return await InternalRunTask(action, cancleOrFalseAction, cancle.Token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task<object> InternalRunTask(Func<object> action, Action<string> cancleOrFalseAction, CancellationToken token)
        {
            return await Task.Run(() => action.Invoke(), token).ContinueWith(p =>
            {
                string errorMessage = "";
                if (p.IsCanceled)
                    errorMessage = "Task Canceled!";
                if (p.IsFaulted)
                {
                    errorMessage = p.Exception?.ToString() ?? "发生未知异常!";
                }

                if ((p.IsCanceled || p.IsFaulted))
                {
                    if (cancleOrFalseAction != null)
                        cancleOrFalseAction(errorMessage);
                    else
                    {
                        Log.Error(errorMessage);
                    }
                    return string.Empty;
                }
                else
                {
                    return p.Result;
                }

            }, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <returns></returns>
        public static object RunTask(Func<object> action, Action<string> cancleOrFalseAction = null)
        {
            var result = Task.Run(async () => await IntneralRunTaskWithAutoCancle(action, cancleOrFalseAction, 1800)).Result;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancleOrFalseAction"></param>
        /// <param name="autocancle"></param>
        /// <returns></returns>
        public static object RunTaskWithAutoCancle(Func<object> action, Action<string> cancleOrFalseAction = null, int autocancle = 60)
        {
            var result = Task.Run(async () => await IntneralRunTaskWithAutoCancle(action, cancleOrFalseAction, autocancle)).Result;
            return result;
        }
    }
}