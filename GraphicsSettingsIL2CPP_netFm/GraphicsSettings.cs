using System;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using BepInEx.Configuration;
using UnityEngine.SceneManagement;

namespace GraphicsSettingsIL2CPP_netFm
{
    /// <summary>
    /// Change graphics settings like resolution, full screen and vSync in the Configuration Manager (F5)
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class GraphicsSettings : BasePlugin
    {
        internal const string GUID = "SpockBauru.GraphicsSettingsIL2CPP_NetFm";
        internal const string PluginName = "Graphics Settings";
        internal const string PluginVersion = "0.5";

        private static ConfigEntry<int> Width;
        private static ConfigEntry<int> Height;
        private static ConfigEntry<DisplayModeList> DisplayMode;

        private static ConfigEntry<int> Framerate;
        private static ConfigEntry<vSyncList> vSync;

        private static ConfigEntry<bool> Apply;

        public override void Load()
        {
            Apply = Config.Bind("Apply Settings", "Apply All Graphics Settings Bellow", false, "Enable to apply the settings. This will be reset if anything changes.");
            Apply.SettingChanged += (sender, args) => ApplySettings();

            Width = Config.Bind("Resolution", "Width", 0);
            Width.SettingChanged += (sender, args) => Apply.Value = false;
            Height = Config.Bind("Resolution", "Height", 0);
            Height.SettingChanged += (sender, args) => Apply.Value = false;
            DisplayMode = Config.Bind("Resolution", "Display Mode", DisplayModeList.Windowed);
            DisplayMode.SettingChanged += (sender, args) => Apply.Value = false;

            vSync = Config.Bind("Framerate", "vSync", vSyncList.On);
            vSync.SettingChanged += (sender, args) => Apply.Value = false;
            Framerate = Config.Bind("Framerate", "Target Framerate", -1, "Target Framerate only works if vSync is Off. Set -1 to unlimited");
            Framerate.SettingChanged += (sender, args) => Apply.Value = false;

            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((s, lsm) => GetResolution()));
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

        private void GetResolution()
        {
            Width.Value = Screen.width;
            Height.Value = Screen.height;
            if (Screen.fullScreen) DisplayMode.Value = DisplayModeList.FullScreen;
            if (!Screen.fullScreen) DisplayMode.Value = DisplayModeList.Windowed;
            Apply.Value = false;
        }

        private void ApplySettings()
        {
            if (!Apply.Value) return;
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

