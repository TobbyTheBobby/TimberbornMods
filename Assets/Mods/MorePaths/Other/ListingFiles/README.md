# More Platforms

You know what would be boring? Only having one path! With this mod u can use different paths to diversify your city!

## Shoutouts

A shoutout to Todor Alin on discord for his time testing versions of this mod!
And a shoutout to Knatte_Anka for helped out with making the icons and ironing out the details. So a big shoutout to him for the ideas and all the help!

## How to use

All paths can be found under the paths tab.

### Paths

These are the paths that are added with this mod. They have the same properties as normal path, but they just look different.

![Example Image](https://media.githubusercontent.com/media/TobbyTheBobby/TimberbornModsUnity/master/Assets/MorePaths/StaticFiles/Images/ExampleImage1.png)

### Adding custom paths

Steps to add your own path:
1. Add a folder in the "Paths" folder of this mod with the same name as the path that you want to add.
2. Add an image (jpg or png) and an icon that you would like to use for your custom path.
3. Make a copy of one of the existing PathSpecifications.
4. Replace the name of the path to be the same as the folder you created.
5. Replace the names of the textures with the name of your images in the Specification files.
6. Add a LocKey to the language file that you are using (probably enUS.txt) using the format: (LocKey),(Name),(comment)

If you have any questions, then send me a PM on discord and I can help or change the text above. 

## Installing

Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/).This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).

## Problems?

In case you experience problems, message me in the modding channel of the the [Timberborn discord](https://discord.gg/mfbBF4cWpX) or message me directly (Tobbert#1607). I will try to fix it as soon as possible. :D

## Known limitations

- Hovering next a driveway doesnt show the driveway.

## Planned features

- Fixing driveways not showing when a driveway is connected to a stairs.
- Enable or disable paths from in-game.
- Enable or disable the corners between paths. 

## Changelog

### 2.1.2 - 17.12.2022

- Fixed to now work with Game version v0.3.4.3.

### 2.1.1 - 20.11.2022

- Fixed a issue that resulted on crashes on MacOS.
- Updated German translations.

### 2.1.0 - 17.11.2022

- Added 2 new paths: Plastered Wood Folktails and Plastered Wood Iron Teeth. 
- Added settings so you can disable paths that you dont use often. 
- Fixed a issue where corners were active when a roof variant was active. 
- Massive code readability improvement.

### 2.0.1 - 12.10.2022

- Fixed MorePaths not working on Folktails.

### 2.0.0 - 12.10.2022

- Removed all assets and are now replaced by a 100% code based solution (possibility of paths disappearing when updating to this version).
- Added the possibility to add custom paths with specifications.
- Any specific path can now be disabled in the specifications. 
- Added corners between paths if they are linked in a square. 

### 1.1.1 - 23.9.2022

- Updated to work with TimberAPI (v0.5.0).
- Removed mentions of speeding up beavers, as this was removed in the previous update.
- Added Japanese translation that was provided by Sukunabikona.
- Added Russian translation that was provided by GinFuyou.
- Added German translation that was provided by juf0816.

### 1.1.0 - 2.9.2022

- Updated to work with latest experimental update (v0.2.6.2).
- Removed the bonus movement speed as the system it used, was removed from the game. As such, the science points were removed from the metal and wood paths.

### 1.0.0 - 16.8.2022

- Fixed a bug that showed paths in front of water, when build in the water.
- Decreased the walking speed buff from 100% to 50%.
- Changed Stone Path name to Gravel Path and this can cause existing paths to disappear. 
- Added more paths: Brick, Wood Folktails (inc. walking speed buff), Wood IronTeeth (inc. walking speed buff), Stone, Gravel 2.
- Changed the texture of the Metal Path. 
- Added category button to group all paths.
- Paths that boost walking speed now show that they do. 

### 0.1.3 - 11.8.2022

- Fixed a bug that prevented placing down stone path.

### 0.1.2 - 10.8.2022

- Fixed a bug that prevented placing buildings in the editor. 

### 0.1.1 - 9.8.2022

- Hotfix.

### 0.1.0 - 9.8.2022

- Released the plugin.