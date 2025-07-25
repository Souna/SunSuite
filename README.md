# Suite of game development tools

This GitHub project offers a collection of game development tools. Currently a work-in-progress, the project includes a custom binary file manager and map creator designed specifically for creating simple 2D sidescroller games.

The envisioned game project relies on binary files with a unique ".sun" extension to load all game data, such as maps, images, sounds, and player data. These binary files follow a custom format tailored exclusively for this project.

## ☀ SunFileManager
A sophisticated application designed to create, edit, and manage custom binary files with a .sun extension (similar to ZIP files but with custom data types). It's essentially a file archiver and editor specifically designed for game development, allowing users to store and organize game assets (images, sounds, data) in a hierarchical structure. This project is based on HaRepacker, which is a custom tool by Haha01Haha01, created for MapleStory file editing.

### Primary Use Cases
- **Game Asset Management:** Organize sprites, sounds, and data files
- **Level Design:** Store level layouts and object properties
- **Data Archiving:** Package related files into single archives

### Core Features
- Managing multiple open files simultaneously
- Handling file saving with temporary file creation for safety
- TreeView population and management
- **Image Support:** PNG format with automatic parsing
- **Audio Support:** MP3 files with built-in player
- **Data Properties:** Various numeric and string data types
- **Export Functionality:** Extract assets to desktop
- **User Preferences:** Dark mode, auto-parse images, UI options
- **Persistent Storage:** Settings saved to users' documents folder

### Context Menu System
- Dynamic right-click menus based on selected node type
- Property addition (String, Int, Float, Double, Canvas, Sound, Vector, etc.)
- File operations (New, Open, Save, Unload, Reload)
- Node manipulation (Rename, Remove, Expand/Collapse)

## ☀ SunEditor
With this drag-and-drop map creator, you can freely place game assets in any arrangement. It enables you to save asset positions and properties to the SunFiles, which in turn creates a playable game map. This project is also heavily based on HaCreator, which is a custom map editor for MapleStory, created by Haha01Haha01.

## ☀ Unnamed Game
The ultimate objective of this project is to develop a fully functional 2D sidescrolling game by utilizing the tools above and harnessing the capabilities of the [olc::PixelGameEngine](https://github.com/OneLoneCoder/olcPixelGameEngine) by OneLoneCoder.

## Contributing

This project is based on:
- [HaRepacker](https://github.com/Haha01Haha01/HaRepacker) by Haha01Haha01
- [HaCreator](https://github.com/Haha01Haha01/HaCreator) by Haha01Haha01

Contributions are welcome! Please feel free to submit issues and pull requests.
