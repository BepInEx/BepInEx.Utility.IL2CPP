using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.IL2CPP.Utils.Collections;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace EnableResizeIL2CPP_netFm
{
    /// <summary>
    /// Enable window resizing in windowed mode.
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class EnableResize : BasePlugin
    {
        internal const string GUID = "SpockBauru.EnableResizeIL2CPP_netFm";
        internal const string PluginName = "Enable Resize";
        internal const string PluginVersion = "0.5";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockBauru;

        internal static ConfigEntry<bool> ConfigEnableResize { get; private set; }

        public override void Load()
        {
            ConfigEnableResize = Config.Bind("Config", "Enable Resize", true, "Whether to allow the game window to be resized. Requires game restart to take effect.");
            if (!ConfigEnableResize.Value) return;

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<EnableResizeComponent>();

            SpockBauru = GameObject.Find("SpockBauru");
            if (SpockBauru == null)
            {
                SpockBauru = new GameObject("SpockBauru");
                GameObject.DontDestroyOnLoad(SpockBauru);
                SpockBauru.hideFlags = HideFlags.HideAndDontSave;
                SpockBauru.AddComponent<EnableResizeComponent>();
            }
            else SpockBauru.AddComponent<EnableResizeComponent>();
        }
    }

    public class EnableResizeComponent : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public EnableResizeComponent(IntPtr handle) : base(handle) { }


        //Old code from mono version starts here
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // Almost the same: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlongptra
        private const int GWL_STYLE = -16;

        // https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles
        private const int WS_CAPTION = 0XC00000;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x80000;
        private const int WS_THICKFRAME = 0x40000;

        private const string GET_CLASS_NAME_MAGIC = "UnityWndClass"; //How Anon got this???
        private IntPtr WindowHandle = IntPtr.Zero;

        private int style = 0;
        private bool fs = false;
        private bool prevFS = true;
        private int res = 0;
        private int prevRes = 1;
        private int borderless = 1;
        private int prevBorderless = 0;
        private int borderlessMask = WS_CAPTION | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_SYSMENU | WS_THICKFRAME;
        private WaitForSecondsRealtime oneSecond = new WaitForSecondsRealtime(1f);

        internal void Awake()
        {
            var pid = Process.GetCurrentProcess().Id;
            EnumWindows((w, param) =>
            {
                if (w == IntPtr.Zero) return true;
                if (GetWindowThreadProcessId(w, out uint lpdwProcessId) == 0) return true;
                if (lpdwProcessId != pid) return true;
                var cn = new StringBuilder(256);
                if (GetClassName(w, cn, cn.Capacity) == 0) return true;
                if (cn.ToString() != GET_CLASS_NAME_MAGIC) return true;
                WindowHandle = w;
                return false;
            }, IntPtr.Zero);

            if (WindowHandle == IntPtr.Zero) return;

            StartCoroutine(TestScreen().WrapToIl2Cpp());
            EnableResize.ConfigEnableResize.SettingChanged += (sender, args) => StartCoroutine(TestScreen().WrapToIl2Cpp());
        }

        private IEnumerator TestScreen()
        {
            while (true)
            {
                if (!EnableResize.ConfigEnableResize.Value) yield break;

                fs = Screen.fullScreen;
                res = Screen.width + Screen.height;
                style = GetWindowLong(WindowHandle, GWL_STYLE);

                // If zero, is in borderless mode
                borderless = style & borderlessMask;

                if (!fs && prevFS ||
                    res != prevRes ||
                    borderless != 0 && prevBorderless == 0)
                {
                    ResizeWindow();
                }

                prevBorderless = borderless;
                prevFS = fs;
                prevRes = res;
                yield return oneSecond;
            }
        }

        private void ResizeWindow()
        {
            if (fs) return;
            if (borderless == 0) return;
            style = GetWindowLong(WindowHandle, GWL_STYLE);
            style |= WS_THICKFRAME | WS_MAXIMIZEBOX;
            SetWindowLong(WindowHandle, GWL_STYLE, style);
        }
    }
}
