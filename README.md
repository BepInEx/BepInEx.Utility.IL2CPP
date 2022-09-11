# SpockPlugins_BepInEx IL2CPP
Various plugins for Unity games ported to IL2CPP

### Prerequisites

- [BepInEx 6.0.0-pre.1](https://github.com/BepInEx/BepInEx/releases/tag/v6.0.0-pre.1)
- [BepInExConfigManager.Il2Cpp](https://github.com/sinai-dev/BepInExConfigManager/releases)

### Installation
- Download the desired plugin from the [Releases Section](https://github.com/SpockBauru/SpockPlugins_BepInEx/releases).
- Extract the .zip file in the game folder (where `game.exe` is).

## Message Center
A simple plugin that shows any log entries marked as "Message" on screen. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility)<br>
Plugins generally use the "Message" log level for things that they want the user to read.

How to make my mod compatible?<br>
Use the Logger of your plugin and call its LogMessage method or Log method and pass in LogLevel.Message as a parameter. You don't have to reference this plugin, and everything will work fine if this plugin doesn't exist.

Please avoid abusing the messages! Only show short and clear messages that the user is likely to understand and find useful. Avoid showing many messages in a short succession.

### New Feature: Black List
Suppress console messages containing certain words. Available in Configuration Manager.

## Enable Resize
An overhaul of [Enable Resize from BepInEx project](https://github.com/BepInEx/BepInEx.Utility) made to increase compatibility

## Enable Full Screen Toggle ILCPP_netFM
Enables toggling full screen with alt+enter on games with it disabled. Ported form [BepInEx Utility](https://github.com/BepInEx/BepInEx.Utility)


