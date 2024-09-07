# BepInEx Utility IL2CPP
Various universal BepInEx utility plugins for Unity games compiled with IL2CPP. Projects marked with netFM are for BepInEx 6 pre1, while projects marked with net6 are for the latest BepInEx 6 builds.

If the game is not compiled with IL2CPP, use [BepInEx.Utility](https://github.com/BepInEx/BepInEx.Utility) instead.

### Prerequisites
#### netFM / BepInEx 6 pre1
- BepInEx 6.0 pre.1. Use [BepInEx_UnityIL2CPP_x64_6.0.0-pre.1.zip](https://github.com/BepInEx/BepInEx/releases/tag/v6.0.0-pre.1)
- Configuration Manager for IL2CPP. Use [BepInExConfigManager.Il2Cpp](https://github.com/sinai-dev/BepInExConfigManager/releases) (open with F5)
### net6 / BepInEx 6 latest
- Recent [nightly build of BepInEx 6.0](https://builds.bepinex.dev/projects/bepinex_be) (newer than pre.1)
- Configuration Manager for IL2CPP. Use [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager/releases) (open with F1, may require additional patches to make IMGUI work, [example](https://github.com/IllusionMods/BepisPlugins/tree/a27b6e4b4701eaff09bceb07f651fc2bacc82e29/src/IMGUIModule.Il2Cpp.CoreCLR.Patcher))

### Installation
- Download the desired plugin from the [Releases Section](https://github.com/SpockBauru/SpockPlugins_BepInEx/releases).
- Extract the .zip file in the game folder (where `game.exe` is).

## Graphics Settings
Change graphics settings like resolution, full screen and vSync in the [Configuration Manager](https://github.com/sinai-dev/BepInExConfigManager/releases). Press F5 to open.<br>
**New Feature:** Apply setings on startup. This will overwrite the game settings, use with caution.

## Enable Resize
Enable window resizing when in windowed mode. An overhaul of [Enable Resize from BepInEx project](https://github.com/BepInEx/BepInEx.Utility) made to increase compatibility.

## Mute In Background
Mute a game when it loses focus, i.e. when alt-tabbed. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility).

## Enable Full Screen Toggle
Allow toggling full screen with alt+enter in games where that has been disabled. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility).

## Message Center
A simple plugin that shows any log entries marked as "Message" on screen. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility)<br>
Plugins generally use the "Message" log level for things that they want the user to read.

How to make my mod compatible?<br>
Use the Logger of your plugin and call its LogMessage method or Log method and pass in LogLevel.Message as a parameter. You don't have to reference this plugin, and everything will work fine if this plugin doesn't exist.

Please avoid abusing the messages! Only show short and clear messages that the user is likely to understand and find useful. Avoid showing many messages in a short succession.

### New Feature: Black List
Suppress console messages containing certain words. Available in Configuration Manager.

## ByteFiddler (patcher)
This patcher allows you to replace a sequence of bytes in the memory of the current process. Use with caution!

1. Run the game once to generate `BepInEx\config\ByteFiddler.cfg` and edit it. 
2. Change the settings as needed, read setting descriptions for more info.
3. Run the game and the plugin will replace the found pattern with the replacement pattern. Check the log for errors or the success message.

## ProcessAffinityOverride (patcher)
This patcher allows you to override processor affinity of the current process.

1. Run the game once to generate `BepInEx\config\ProcessAffinityOverride.cfg` and edit it. 
2. Change the settings as needed, read setting descriptions for more info.
3. Run the game and the plugin should change the affinity. Check the log for errors or the success message. You can see if it worked by checking process affinity in Task Manager.
