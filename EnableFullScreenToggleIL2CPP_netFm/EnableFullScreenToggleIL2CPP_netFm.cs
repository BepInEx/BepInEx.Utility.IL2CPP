using BepInEx;
using BepInEx.IL2CPP;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using Input = UnityEngine.Input;
using KeyCode = UnityEngine.KeyCode;

namespace EnableFullScreenToggleIL2CPP_netFm
{
    /// <summary>
    /// Allow toggling full screen with alt+enter in games where that has been disabled
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class EnableFullScreenToggleIL2CPP_netFm : BasePlugin
    {
        public const string GUID = "SpockBauru.EnableFullScreenToggle.IL2CPP_netFm";
        public const string PluginName = "Enable Full Screen Toggle IL2CPP_netFm";
        public const string PluginVersion = "1.0";

        public override void Load()
        {
            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<FullScreenToggleComponent>();
            GameObject EnableFullScreenToggle = new GameObject("EnableFullScreenToggle_IL2CPP");
            GameObject.DontDestroyOnLoad(EnableFullScreenToggle);
            EnableFullScreenToggle.hideFlags = HideFlags.HideAndDontSave;
            EnableFullScreenToggle.AddComponent<FullScreenToggleComponent>();
        }
    }

    public class FullScreenToggleComponent : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public FullScreenToggleComponent(IntPtr handle) : base(handle) { }

        internal void Update()
        {
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
                //This section of code is never reached on Unity builds where full screen can be toggled, it seems
                //We can safely toggle full screen without risk of it being toggled twice
                Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
