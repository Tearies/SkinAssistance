using System;
using System.Management;
using System.Runtime.InteropServices;

namespace SkinAssistance.Core.Native
{
    public class MonitorManager
    {
        [DllImport("gdi32.dll")]
        private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 调整屏幕的亮度
        /// </summary>
        /// <param name="brightness">The brightness.</param>
        public static void SetBrightness(int brightness)
        {
            brightness = brightness % 101;
                ManagementScope scope = new ManagementScope("root\\WMI");
            SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        mObj.InvokeMethod("WmiSetBrightness",
                            new Object[] { UInt32.MaxValue, brightness});
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///  设置屏幕的Gama值
        /// </summary>
        /// <param name="gammaValue">The gamma value.</param>
        public static void SetGamma(int gammaValue)
        {
            RAMP _ramp;
            // since gamma value is max 44 ,, we need to take the percentage from this because 
            // it needed from 0 - 100%
            double gValue = gammaValue;
            gValue = Math.Floor(Convert.ToDouble((gValue / 2.27)));
            gammaValue = Convert.ToInt16(gValue);
            if (gammaValue != 0)
            {
                _ramp.Red = new ushort[256];
                _ramp.Green = new ushort[256];
                _ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    _ramp.Red[i] = _ramp.Green[i] = _ramp.Blue[i] =
                        (ushort)
                        (Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, (gammaValue + 5) * 0.1) * 65535 + 0.5)));
                }
                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref _ramp);
            }
        }

        #region Nested type: RAMP

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] public UInt16[] Blue;
        }
        #endregion
    }
}