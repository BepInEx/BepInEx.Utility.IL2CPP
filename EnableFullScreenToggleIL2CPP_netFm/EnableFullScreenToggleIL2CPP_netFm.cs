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
        public const string GUID = "SpockBauru.EnableFullScreenToggleIL2CPP_netFm";
        public const string PluginName = "Enable Full Screen Toggle";
        public const string PluginVersion = "0.1";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockPlugins_BepInEx;

        public override void Load()
        {
            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<FullScreenToggleComponent>();

            SpockPlugins_BepInEx = GameObject.Find("SpockPlugins_BepInEx");

            if (SpockPlugins_BepInEx == null)
            {
                SpockPlugins_BepInEx = new GameObject("SpockPlugins_BepInEx");
                GameObject.DontDestroyOnLoad(SpockPlugins_BepInEx);
                SpockPlugins_BepInEx.hideFlags = HideFlags.HideAndDontSave;
                SpockPlugins_BepInEx.AddComponent<FullScreenToggleComponent>();
            }
            else SpockPlugins_BepInEx.AddComponent<FullScreenToggleComponent>();
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
