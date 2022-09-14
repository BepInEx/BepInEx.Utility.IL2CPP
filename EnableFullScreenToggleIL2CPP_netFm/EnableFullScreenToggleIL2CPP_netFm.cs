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
    public class EnableFullScreenToggle : BasePlugin
    {
        internal const string GUID = "SpockBauru.EnableFullScreenToggleIL2CPP_netFm";
        internal const string PluginName = "Enable Full Screen Toggle";
        internal const string PluginVersion = "0.5";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockBauru;

        public override void Load()
        {
            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<FullScreenToggleComponent>();

            SpockBauru = GameObject.Find("SpockBauru");

            if (SpockBauru == null)
            {
                SpockBauru = new GameObject("SpockBauru");
                GameObject.DontDestroyOnLoad(SpockBauru);
                SpockBauru.hideFlags = HideFlags.HideAndDontSave;
                SpockBauru.AddComponent<FullScreenToggleComponent>();
            }
            else SpockBauru.AddComponent<FullScreenToggleComponent>();
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
