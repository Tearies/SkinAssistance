using System;
using System.Windows.Input;
using SkinAssistance.Core.Native;

namespace SkinAssistance.Core.HotKey
{
    internal class Hotkey
    {
        private IntPtr _hwnd;


        public Hotkey(uint virtualKey, HotkeyFlags flags)
        {
            Id = NativeMethods.GlobalAddAtom(
                $"VBI_{KeyInterop.KeyFromVirtualKey((int) virtualKey)}_{flags}_{Environment.TickCount}");
            VirtualKey = virtualKey;
            Flags = flags;
        }

        public int Id { get; }

        public uint VirtualKey { get; }

        public HotkeyFlags Flags { get; }

        public void Register(IntPtr hwnd, string name)
        {
            if (!NativeMethods.RegisterHotKey(hwnd, Id, Flags, VirtualKey))
                LogExtensions.Info(null,
                    $"Registor Hot Key {Flags}+{KeyInterop.KeyFromVirtualKey((int) VirtualKey)} Failed");
            else
                LogExtensions.Info(null,
                    $"Registor Hot Key {Flags}+{KeyInterop.KeyFromVirtualKey((int) VirtualKey)} Succ");


            _hwnd = hwnd;
        }


        public void Unregister()
        {
            if (_hwnd != IntPtr.Zero)
            {
                NativeMethods.GlobalDeleteAtom(Id);
                if (!NativeMethods.UnregisterHotKey(_hwnd, Id))
                    LogExtensions.Info(null,
                        $"UnRegistor Hot Key  {Flags}+{KeyInterop.KeyFromVirtualKey((int) VirtualKey)} Failed");
                else
                    LogExtensions.Info(null,
                        $"UnRegistor Hot Key  {Flags}+{KeyInterop.KeyFromVirtualKey((int) VirtualKey)} Succ");
                _hwnd = IntPtr.Zero;
            }
        }
    }
}