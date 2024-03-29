﻿using System;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;
using Input = UnityEngine.Input;
using KeyCode = UnityEngine.KeyCode;

namespace BepInEx
{
    /// <summary>
    /// Allow toggling full screen with alt+enter in games where that has been disabled
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class EnableFullScreenToggle : BasePlugin
    {
        internal const string GUID = "BepInEx.EnableFullScreenToggleIL2CPP_net6";
        internal const string PluginName = "Enable Full Screen Toggle";
        internal const string PluginVersion = "0.7";

        //Game Object shared between all BepInExUtility plugins
        public GameObject BepInExUtility;

        public override void Load()
        {
            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<FullScreenToggleComponent>();

            BepInExUtility = GameObject.Find("BepInExUtility");

            if (BepInExUtility == null)
            {
                BepInExUtility = new GameObject("BepInExUtility");
                GameObject.DontDestroyOnLoad(BepInExUtility);
                BepInExUtility.hideFlags = HideFlags.HideAndDontSave;
                BepInExUtility.AddComponent<FullScreenToggleComponent>();
            }
            else BepInExUtility.AddComponent<FullScreenToggleComponent>();
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
