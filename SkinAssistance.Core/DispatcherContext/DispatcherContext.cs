#region NS

using System;
using System.Threading;
using System.Windows.Threading;

#endregion

namespace SkinAssistance.Core.DispatcherContext
{
    #region Reference

    #endregion

    public static class DispatcherContext
    {
        #region Properties

        private static Dispatcher RenderDispatcher { get; set; }
        /// <summary>默认UI操作的超时时间 30s</summary>
        private const int DefaultDispatcherTimeout = 30;
        #endregion

        #region Method

        public static void Initialize()
        {
            if (RenderDispatcher != null && RenderDispatcher.Thread.IsAlive)
                return;

            RenderDispatcher = Dispatcher.CurrentDispatcher;
        }

        public static void Reset()
        {
            RenderDispatcher = null;
        }

        private static CancellationTokenSource BuildCancellationTokenSource(int duration)
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(duration));
        }

        public static void InvokeOnRender(Action action, DispatcherPriority priority = DispatcherPriority.Background)
        {
            CheckRenderDispatcher();
            try
            {
                RenderDispatcher.Invoke(action, priority, BuildCancellationTokenSource(DefaultDispatcherTimeout).Token);
            }
            catch (Exception e)
            {

            }


        }

        public static DispatcherOperation InvokeAsync(Action ation, CancellationToken token, DispatcherPriority priority = DispatcherPriority.Background)
        {
            return RenderDispatcher.InvokeAsync(ation, priority, token);
        }

        /// <summary>所有异步UI操作30秒内必须执行完毕, 不然会卡UI</summary>
        public static DispatcherOperation InvokeAsync(Action ation, DispatcherPriority priority = DispatcherPriority.Background)
        {
            return InvokeAsync(ation, BuildCancellationTokenSource(DefaultDispatcherTimeout).Token, priority);
        }

        public static void CheckBeginInvokeOnRender(Action action,
            DispatcherPriority priority = DispatcherPriority.Normal, params object[] @params)
        {
            CheckRenderDispatcher();
            if (RenderDispatcher.CheckAccess())
                action();
            else
                RenderDispatcher.BeginInvoke(action, priority, @params, BuildCancellationTokenSource(DefaultDispatcherTimeout).Token);
        }


        private static void CheckRenderDispatcher()
        {
            if (RenderDispatcher == null)
                throw new InvalidOperationException();
        }

        #endregion
    }
}