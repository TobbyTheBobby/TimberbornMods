# Category Button

This mod adds the ability for mod makers to add buttons that have multiple buttons. Also groups the bridges in the paths menu.

## How to use

The category buttons works as you think it would, it adds another layer of buttons to use. But u can also use SHIFT + MOUSE SCROLL to switch between buttons of the same group. 

## Modders: how to use

1. Add the CategoryButton package to the the project.
2. Create a specification which looks like the following:
```json
{
  "Name": "CategoryBridges",
  "ToolGroup": "Paths",
  "ToolOrder": 300,
  "ButtonIcon": "Bridges.png",
  "Buildings": [
    "SuspensionBridge1x1.Folktails",
    "SuspensionBridge1x1.IronTeeth",
    "SuspensionBridge2x1.Folktails",
    "SuspensionBridge2x1.IronTeeth",
    "SuspensionBridge3x1.Folktails",
    "SuspensionBridge3x1.IronTeeth",
    "SuspensionBridge4x1.Folktails",
    "SuspensionBridge4x1.IronTeeth",
    "SuspensionBridge5x1.Folktails",
    "SuspensionBridge5x1.IronTeeth",
    "SuspensionBridge6x1.Folktails",
    "SuspensionBridge6x1.IronTeeth"
  ],
  "DisplayNameLocKey": "Tobbert.CategoryBridges.DisplayName"
}
```
3. Change the values based on your category. 
4. Add an image with the name that you filled in under "ButtonIcon" and place it in "assets\CategorySprites\".
5. Done!

## Installing

Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/). You can install this plugin manually by cloning the repo, building it
and adding the dll to your bepinex plugins folder. This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).

## Problems?

In case you experience problems, message me in the modding channel of the the [Timberborn discord](https://discord.gg/mfbBF4cWpX) or message me directly (Tobbert#1607). I will try to fix it as soon as possible. :D

## Changelog

### 1.4.1 - 16.12.2022

- Fixed to now work with Game version v0.3.4.3.

### 1.4.0 - 2.12.2022

- Added the ability to add Planting buttons as part of a CategoryButton.
- Now disables any Group Button that doesn't have any buttons anymore.
- Fixed a bug that prevented the Bridge Category to be overwritten.

### 1.3.0 - 26.11.2022

- Added a feature that any button can be added anywhere, regardless of order. 

### 1.2.2 - 18.11.2022

- Reverted a change that was incompatible with the old system before 1.2.0

### 1.2.1 - 17.11.2022

- Fixed a issue where the mod crashed on MacOS.
- Now always is in the middle even after the size changes. 

### 1.2.0 - 29.10.2022

- Changed so the buttons are now based on specifications instead of prefabs. 

### 1.1.2 - 23.9.2022

- Updated to work with TimberAPI (v0.5.0).
- Added Japanese translation that was provided by Sukunabikona.
- Added Russian translation that was provided by GinFuyou.
- Added German translation that was provided by juf0816.

### 1.1.1 - 29.8.2022

- Removed unnecessary console logs. 
- Fixed a bug that prevented water from appearing when exiting a category button that had no button inside the category selected.

### 1.1.0 - 18.8.2022

- The last used button will now be selected upon reopen.
- Added the possibility to use shift+scroll to switch between buttons of the same group. 

### 1.0.4 - 16.8.2022

- Fixed a bug that prevented the button from working if it was first in the row.

### 1.0.3 - 15.8.2022

- Fixed a bug that the place of the group was incorrect on non-native resolutions.

### 1.0.2 - 14.8.2022

- Fixed a bug that closed the category window when clicking the category button twice. 
- Fixed a bug that caused a crash when opening the editor.

### 1.0.1 - 13.8.2022

- Fixed a bug that prevented loading the mod. 

### 1.0.0 - 13.8.2022

- Released the plugin.
