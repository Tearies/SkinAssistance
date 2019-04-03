using System;
using System.Windows.Input;
using System.Windows.Interop;
using SkinAssistance.Core.ICommands;

namespace SkinAssistance.Core.HotKey
{

    public class HotkeyManager : HotkeyManagerBase
    {
        #region Singleton implementation

        public static readonly HotkeyManager Current = new Lazy<HotkeyManager>(() => new HotkeyManager()).Value;

        #endregion

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly HwndSource _source;
        private readonly WeakReferenceCollection<KeyBinding> _keyBindings;

        private HotkeyManager()
        {
            _keyBindings = new WeakReferenceCollection<KeyBinding>();

            var parameters = new HwndSourceParameters("Hotkey sink")
            {
                HwndSourceHook = HandleMessage,
                ParentWindow = HwndMessage
            };
            _source = new HwndSource(parameters);
            SetHwnd(_source.Handle);
        }

        public void AddOrReplace(string name, Key key, ModifierKeys modifiers)
        {
            AddOrReplace(name, key, modifiers, false);
        }

        public void AddOrReplace(string name, Key key, ModifierKeys modifiers, bool noRepeat)
        {
            var flags = GetFlags(modifiers, noRepeat);
            var vk = (uint)KeyInterop.VirtualKeyFromKey(key);
            base.AddOrReplace(name, vk, flags);
        }

        private static HotkeyFlags GetFlags(ModifierKeys modifiers, bool noRepeat)
        {
            var flags = HotkeyFlags.None;
            if (modifiers.HasFlag(ModifierKeys.Shift))
                flags |= HotkeyFlags.Shift;
            if (modifiers.HasFlag(ModifierKeys.Control))
                flags |= HotkeyFlags.Control;
            if (modifiers.HasFlag(ModifierKeys.Alt))
                flags |= HotkeyFlags.Alt;
            if (modifiers.HasFlag(ModifierKeys.Windows))
                flags |= HotkeyFlags.Windows;
            if (noRepeat)
                flags |= HotkeyFlags.NoRepeat;
            return flags;
        }

        private static ModifierKeys GetModifiers(HotkeyFlags flags)
        {
            var modifiers = ModifierKeys.None;
            if (flags.HasFlag(HotkeyFlags.Shift))
                modifiers |= ModifierKeys.Shift;
            if (flags.HasFlag(HotkeyFlags.Control))
                modifiers |= ModifierKeys.Control;
            if (flags.HasFlag(HotkeyFlags.Alt))
                modifiers |= ModifierKeys.Alt;
            if (flags.HasFlag(HotkeyFlags.Windows))
                modifiers |= ModifierKeys.Windows;
            return modifiers;
        }

        private IntPtr HandleMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            Hotkey hotkey;
            HandleHotkeyMessage(hwnd, msg, wparam, lparam, ref handled, out hotkey);
            if (!handled)
                return IntPtr.Zero;
            if (hotkey != null)
                handled = ExecuteBoundCommand(hotkey);
            return IntPtr.Zero;
        }

        private bool ExecuteBoundCommand(Hotkey hotkey)
        {
            var key = KeyInterop.KeyFromVirtualKey((int)hotkey.VirtualKey);
            var modifiers = GetModifiers(hotkey.Flags);
            return ShortCutContext.InvokeShortCuts(key, modifiers);
        }
    }
}