using System;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace BepInEx
{
    /// <summary>
    /// Mute the game when the screen is in background
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class MuteInBackground : BasePlugin
    {
        internal const string GUID = "BepInEx.MuteInBackgroundIL2CPP_netFm";
        internal const string PluginName = "Mute In Background";
        internal const string PluginVersion = "0.7";

        //Game Object shared between all BepInExUtility plugins
        public GameObject BepInExUtility;


        internal static ConfigEntry<bool> ConfigMuteInBackground { get; private set; }

        public override void Load()
        {
            ConfigMuteInBackground = Config.Bind("Config", "Mute In Background", true, "Whether to mute the game when in the background, i.e. alt-tabbed.");

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MuteInBackgroundComponent>();

            BepInExUtility = GameObject.Find("BepInExUtility");

            if (BepInExUtility == null)
            {
                BepInExUtility = new GameObject("BepInExUtility");
                GameObject.DontDestroyOnLoad(BepInExUtility);
                BepInExUtility.hideFlags = HideFlags.HideAndDontSave;
                BepInExUtility.AddComponent<MuteInBackgroundComponent>();
            }
            else BepInExUtility.AddComponent<MuteInBackgroundComponent>();
        }
    }

    public class MuteInBackgroundComponent : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public MuteInBackgroundComponent(IntPtr handle) : base(handle) { }

        internal static float? OriginalVolume = null;

        internal void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                //Restore the original volume if one was previously set
                if (OriginalVolume != null)
                    AudioListener.volume = (float)OriginalVolume;
                OriginalVolume = null;
            }
            else if (MuteInBackground.ConfigMuteInBackground.Value)
            {
                //Store the original volume and set the volume to zero
                OriginalVolume = AudioListener.volume;
                AudioListener.volume = 0;
            }
        }
    }
}
