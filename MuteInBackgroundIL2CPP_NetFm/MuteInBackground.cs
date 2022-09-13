using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using UnhollowerRuntimeLib;
using System;
using BepInEx.Configuration;

namespace MuteInBackgroundIL2CPP_NetFm
{
    /// <summary>
    /// Mute the game in background
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class MuteInBackground : BasePlugin
    {
        internal const string GUID = "SpockBauru.MuteInBackgroundIL2CPP_NetFm";
        internal const string PluginName = "Mute In Background";
        internal const string PluginVersion = "0.1";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockPlugins_BepInEx;


        internal static ConfigEntry<bool> ConfigMuteInBackground { get; private set; }

        public override void Load()
        {
            ConfigMuteInBackground = Config.Bind("Config", "Mute In Background", true, "Whether to mute the game when in the background, i.e. alt-tabbed.");

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MuteInBackgroundComponent>();

            SpockPlugins_BepInEx = GameObject.Find("SpockPlugins_BepInEx");

            if (SpockPlugins_BepInEx == null)
            {
                SpockPlugins_BepInEx = new GameObject("SpockPlugins_BepInEx");
                GameObject.DontDestroyOnLoad(SpockPlugins_BepInEx);
                SpockPlugins_BepInEx.hideFlags = HideFlags.HideAndDontSave;
                SpockPlugins_BepInEx.AddComponent<MuteInBackgroundComponent>();
            }
            else SpockPlugins_BepInEx.AddComponent<MuteInBackgroundComponent>();
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
