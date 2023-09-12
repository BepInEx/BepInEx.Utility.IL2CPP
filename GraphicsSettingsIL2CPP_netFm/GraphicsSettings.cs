using System;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BepInEx
{
    /// <summary>
    /// Change graphics settings like resolution, full screen and vSync in the Configuration Manager (F5)
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class GraphicsSettings : BasePlugin
    {
        internal const string GUID = "BepInEx.GraphicsSettingsIL2CPP_NetFm";
        internal const string PluginName = "Graphics Settings";
        internal const string PluginVersion = "0.7";

        private static ConfigEntry<int> Width;
        private static ConfigEntry<int> Height;
        private static ConfigEntry<DisplayModeList> DisplayMode;

        private static ConfigEntry<int> Framerate;
        private static ConfigEntry<vSyncList> vSync;

        private static ConfigEntry<bool> AutoApply;

        public override void Load()
        {
            AutoApply = Config.Bind("Apply on Startup", "Apply on Startup", false, "Apply graphics settings when you start the game. May also force the resolution when the game is running");
            AutoApply.SettingChanged += (sender, args) => ApplySettings();

            Width = Config.Bind("Resolution", "Width", 1280, "Set Resolution Width. Minimum is 800");
            Width.SettingChanged += (sender, args) => ApplySettings();
            Height = Config.Bind("Resolution", "Height", 720, "Set Resolution Height. Minimum is 600");
            Height.SettingChanged += (sender, args) => ApplySettings();
            DisplayMode = Config.Bind("Resolution", "Display Mode", DisplayModeList.Windowed);
            DisplayMode.SettingChanged += (sender, args) => ApplySettings();

            vSync = Config.Bind("Framerate", "vSync", vSyncList.On);
            vSync.SettingChanged += (sender, args) => ApplySettings();
            Framerate = Config.Bind("Framerate", "Target Framerate", -1, "Target Framerate only works if vSync is Off. Set -1 to unlimited");
            Framerate.SettingChanged += (sender, args) => ApplySettings();

            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((s, lsm) => 
            { 
                if (AutoApply.Value) ApplySettings(); 
            }));
        }

        private enum DisplayModeList
        {
            FullScreen,
            Windowed,
            Borderless_FullScreen
        }

        private enum vSyncList
        {
            On = 1,
            Off = 0,
            Half = 2
        }

        private void ApplySettings()
        {
            if (Width.Value < 800) Width.Value = 800;
            if (Height.Value < 600) Height.Value = 600;

            if (DisplayMode.Value == DisplayModeList.FullScreen)
                Screen.SetResolution(Width.Value, Height.Value, FullScreenMode.ExclusiveFullScreen);

            if (DisplayMode.Value == DisplayModeList.Windowed)
                Screen.SetResolution(Width.Value, Height.Value, FullScreenMode.Windowed);

            if (DisplayMode.Value == DisplayModeList.Borderless_FullScreen)
                Screen.SetResolution(Width.Value, Height.Value, FullScreenMode.FullScreenWindow);

            QualitySettings.vSyncCount = (int)vSync.Value;
            Application.targetFrameRate = Framerate.Value;
        }
    }
}

