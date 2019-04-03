using System;
using System.Collections.Generic;
using System.Linq;

namespace SkinAssistance.Core.HotKey
{
    public abstract class HotkeyManagerBase:IDisposable
    {
        private readonly Dictionary<int, string> _hotkeyNames = new Dictionary<int, string>();
        private readonly Dictionary<string, Hotkey> _hotkeys = new Dictionary<string, Hotkey>();
        private IntPtr _hwnd;
        internal static readonly IntPtr HwndMessage = (IntPtr)(-3);

        internal HotkeyManagerBase()
        {
        }

        internal void AddOrReplace(string name, uint virtualKey, HotkeyFlags flags)
        {
            var hotkey = new Hotkey(virtualKey, flags);
            lock (_hotkeys)
            {
                Remove(name);
                _hotkeys.Add(name, hotkey);
                _hotkeyNames.Add(hotkey.Id, name);
                if (_hwnd != IntPtr.Zero)
                    hotkey.Register(_hwnd, name);
            }
        }

        public void Remove(string name)
        {
            lock (_hotkeys)
            {
                Hotkey hotkey;
                if (_hotkeys.TryGetValue(name, out hotkey))
                {
                    _hotkeys.Remove(name);
                    _hotkeyNames.Remove(hotkey.Id);
                    if (_hwnd != IntPtr.Zero)
                        hotkey.Unregister();
                }
            }
        }

        internal void SetHwnd(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        private const int WmHotkey = 0x0312;

        internal IntPtr HandleHotkeyMessage(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled,
            out Hotkey hotkey)
        {
            hotkey = null;
            if (msg == WmHotkey)
            {
                int id = wParam.ToInt32();
                string name;
                if (_hotkeyNames.TryGetValue(id, out name))
                {
                    hotkey = _hotkeys[name];
                    handled = true;
                }
                else
                {
                    handled = false;
                }
            }

            return IntPtr.Zero;
        }
       

        #region IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        ~HotkeyManagerBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        private bool idDisposing;

        protected virtual void Dispose(bool idDispose)
        {
            if (!idDisposing)
            {
                if (idDispose)
                {
                    for (int i = 0; i < _hotkeyNames.Count; i++)
                    {
                        Remove(_hotkeyNames.ElementAt(i).Value);
                    }
                }
                idDisposing = true;
            }
        }

        #endregion
    }
}