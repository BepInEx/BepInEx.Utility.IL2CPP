# SpockPlugins_BepInEx IL2CPP
Various plugins for Unity games ported to IL2CPP<br>
Disclaimer: This is a temporary repository, it is subject to change without notice.

### Prerequisites
- [BepInEx 6.0.0-pre.1](https://github.com/BepInEx/BepInEx/releases/tag/v6.0.0-pre.1)
- [BepInExConfigManager.Il2Cpp](https://github.com/sinai-dev/BepInExConfigManager/releases) (open with F5)

### Installation
- Download the desired plugin from the [Releases Section](https://github.com/SpockBauru/SpockPlugins_BepInEx/releases).
- Extract the .zip file in the game folder (where `game.exe` is).

## Graphics Settings
Change graphics settings like resolution, full screen and vSync in the [Configuration Manager](https://github.com/sinai-dev/BepInExConfigManager/releases)

## Message Center
A simple plugin that shows any log entries marked as "Message" on screen. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility)<br>
Plugins generally use the "Message" log level for things that they want the user to read.

How to make my mod compatible?<br>
Use the Logger of your plugin and call its LogMessage method or Log method and pass in LogLevel.Message as a parameter. You don't have to reference this plugin, and everything will work fine if this plugin doesn't exist.

Please avoid abusing the messages! Only show short and clear messages that the user is likely to understand and find useful. Avoid showing many messages in a short succession.

### New Feature: Black List
Suppress console messages containing certain words. Available in Configuration Manager.

## Enable Resize
Enable window resizing when in windowed mode. An overhaul of [Enable Resize from BepInEx project](https://github.com/BepInEx/BepInEx.Utility) made to increase compatibility.

## Mute In Background
Mute a game when it loses focus, i.e. when alt-tabbed. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility).

## Enable Full Screen Toggle
Allow toggling full screen with alt+enter in games where that has been disabled. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility).
