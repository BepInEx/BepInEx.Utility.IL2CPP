using System;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BepInEx
{
    /// <summary>
    /// Change graphics settings like resolution, full screen and vSync in the Configuration Manager (F5)
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public class GraphicsSettings : BasePlugin
    {
        internal const string GUID = "BepInEx.GraphicsSettingsIL2CPP_net6";
        internal const string PluginName = "Graphics Settings";
        internal const string PluginVersion = "0.8";

        private static ConfigEntry<int> Width;
        private static ConfigEntry<int> Height;
        private static ConfigEntry<DisplayModeList> DisplayMode;

        private static ConfigEntry<int> Framerate;
        private static ConfigEntry<vSyncList> vSync;

        private static ConfigEntry<bool> AutoApply;

        public override void Load()
        {
            AutoApply = Config.Bind("General", "Auto Apply", false, "Apply all graphics settings automatically whenever active scene changes. Otherwise settings are only applied as they are changed.");
            var applySettingsAction = (UnityAction<Scene, Scene>)new Action<Scene, Scene>((f, t) => ApplySettings());
            var applySettingsAction2 = (UnityAction<Scene, LoadSceneMode>)new Action<Scene, LoadSceneMode>((f, t) => ApplySettings());
            void ApplySceneHook(bool showWarning)
            {
                try
                {
                    if (AutoApply.Value)
                        SceneManager.add_activeSceneChanged(applySettingsAction);
                    else
                        SceneManager.remove_activeSceneChanged(applySettingsAction);
                }
                catch (NotSupportedException)
                {
                    try
                    {
                        if (AutoApply.Value)
                            SceneManager.add_sceneLoaded(applySettingsAction2);
                        else
                            SceneManager.remove_sceneLoaded(applySettingsAction2);
                    }
                    catch (NotSupportedException)
                    {
                        Log.Log(showWarning ? LogLevel.Message | LogLevel.Warning : LogLevel.Warning, "Could not hook the scene change event handler, changes will not be applied automatically");
                    }
                }
            }
            AutoApply.SettingChanged += (sender, args) => { ApplySceneHook(true); };

            Width = Config.Bind("Resolution Override", "Width", 0, "Force resolution width to this value. Minimum is 800. Set to 0 to disable this feature.");
            Width.SettingChanged += (sender, args) => ApplySettings();
            Height = Config.Bind("Resolution Override", "Height", 0, "Force resolution height to this value. Minimum is 600. Set to 0 to disable this feature.");
            Height.SettingChanged += (sender, args) => ApplySettings();
            DisplayMode = Config.Bind("Resolution Override", "Display Mode", DisplayModeList.Default, "Force specified window mode.");
            DisplayMode.SettingChanged += (sender, args) => ApplySettings();

            vSync = Config.Bind("Framerate Override", "vSync", vSyncList.Default, "Force specified vsync mode.");
            vSync.SettingChanged += (sender, args) => ApplySettings();
            Framerate = Config.Bind("Framerate Override", "Target Framerate", Application.targetFrameRate, "Force specified target Framerate. Only works if vSync is Off. Set -1 for unlimited.");
            Framerate.SettingChanged += (sender, args) => ApplySettings();

            if (AutoApply.Value)
                ApplySceneHook(false);
        }

        private enum DisplayModeList
        {
            Default = 0,
            FullScreen,
            Windowed,
            Borderless_FullScreen
        }

        private enum vSyncList
        {
            Default = -1,
            On = 1,
            Off = 0,
            Half = 2
        }

        private static int _originalWidth, _originalHeight;
        private static vSyncList _originalVsync = vSyncList.Default;
        private static DisplayModeList _originalDisplayMode = DisplayModeList.Default;

        private static void ApplySettings()
        {
            if (_originalWidth <= 0)
                _originalWidth = Screen.width;
            if (_originalHeight <= 0)
                _originalHeight = Screen.height;
            if (_originalVsync == vSyncList.Default)
                _originalVsync = (vSyncList)QualitySettings.vSyncCount;
            if (_originalDisplayMode == DisplayModeList.Default)
                _originalDisplayMode = (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen) ? DisplayModeList.FullScreen :
                                       (Screen.fullScreenMode == FullScreenMode.Windowed) ? DisplayModeList.Windowed :
                                       (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) ? DisplayModeList.Borderless_FullScreen :
                                       DisplayModeList.Windowed;

            var width = Width.Value;
            if (width <= 0) width = _originalWidth;
            else if (width < 800) width = Width.Value = 800;

            var height = Height.Value;
            if (height <= 0) height = _originalHeight;
            else if (height < 600) height = Height.Value = 600;

            var displayMode = DisplayMode.Value;
            if (displayMode == DisplayModeList.Default)
                displayMode = _originalDisplayMode;

            if (displayMode == DisplayModeList.FullScreen)
                Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
            else if (displayMode == DisplayModeList.Windowed)
                Screen.SetResolution(width, height, FullScreenMode.Windowed);
            else if (displayMode == DisplayModeList.Borderless_FullScreen)
                Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);

            if (vSync.Value == vSyncList.Default)
                QualitySettings.vSyncCount = (int)_originalVsync;
            else
                QualitySettings.vSyncCount = (int)vSync.Value;

            if (QualitySettings.vSyncCount == 0)
                Application.targetFrameRate = Framerate.Value;
        }
    }
}

