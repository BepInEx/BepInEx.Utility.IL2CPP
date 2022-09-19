using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using UnhollowerRuntimeLib;
using System;
using BepInEx.Configuration;

namespace MuteInBackgroundIL2CPP_netFm
{
    /// <summary>
    /// Mute the game when the screen is in background
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class MuteInBackground : BasePlugin
    {
        internal const string GUID = "SpockBauru.MuteInBackgroundIL2CPP_netFm";
        internal const string PluginName = "Mute In Background";
        internal const string PluginVersion = "0.6";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockBauru;


        internal static ConfigEntry<bool> ConfigMuteInBackground { get; private set; }

        public override void Load()
        {
            ConfigMuteInBackground = Config.Bind("Config", "Mute In Background", true, "Whether to mute the game when in the background, i.e. alt-tabbed.");

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MuteInBackgroundComponent>();

            SpockBauru = GameObject.Find("SpockBauru");

            if (SpockBauru == null)
            {
                SpockBauru = new GameObject("SpockBauru");
                GameObject.DontDestroyOnLoad(SpockBauru);
                SpockBauru.hideFlags = HideFlags.HideAndDontSave;
                SpockBauru.AddComponent<MuteInBackgroundComponent>();
            }
            else SpockBauru.AddComponent<MuteInBackgroundComponent>();
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
