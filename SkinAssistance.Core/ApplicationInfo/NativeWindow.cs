using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SkinAssistance.Core.Extensions;
using SkinAssistance.Core.Native;

namespace SkinAssistance.Core.ApplicationInfo
{
    public class NativeWindow : Window
    {
        static NativeWindow()
        {
            EventManager.RegisterClassHandler(typeof(NativeWindow), Mouse.LostMouseCaptureEvent, (Delegate)new MouseEventHandler(NativeWindow.OnLostMouseCapture));
        }

        private static void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            sender.ResolveService<NativeWindow>((p) => p.OnLostMouseCapture());
        }

        private void OnLostMouseCapture()
        {
            if (!StyOpen)
                Close();
        }

        public NativeWindow()
        {
            Background = Brushes.Transparent;

            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.Manual;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            Topmost = true;
            ShowActivated = false;
        }



        public bool StyOpen
        {
            get { return (bool)GetValue(StyOpenProperty); }
            set { SetValue(StyOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StyOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StyOpenProperty =
            DependencyProperty.Register("StyOpen", typeof(bool), typeof(NativeWindow), new FrameworkPropertyMetadata(BooleanBoxes.Box(true)));



    }
}