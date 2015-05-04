# DanmakU
DanmakU is an open source danmaku development kit for Unity3D.

# Getting Started
See the [wiki](https://github.com/rhythmia/DanmakU/wiki) on a number of tutorials on how to get started.  
See the [script documentation](http://rhythmia.org/DanmakU/Docs/html/annotated.html) to see a scripting reference.

# Contributing
DanmakU is open source under the MIT license. While using it, and find a bug that you fixed? See an issue in the issue tracker (of which there are many)? Have a new idea for a good feature for the development kit? Simply fork this repo, make your changes and submit a pull request. They will be merged in once it is confirmed that the changes indeed do help.

# Structure
DanmakU is split into multiple packages which can be used seperately or together depending on your needs:
* **Core** - The central core to the entire dev kit. **Requires [UnityUtilLib](https://github.com/james7132/UnityUtilLib), [Vexe Framework](http://forum.unity3d.com/threads/free-vfw-full-set-of-drawers-savesystem-serialize-interfaces-generics-auto-props-delegates.266165/), and Unity3D itself.**
* **Colliders** - A package that contains a number of useful Danmaku Colliders that can be used in a number of ways. **Requires: Core**
* **Controllers** - A package that contains a number of useful Danmaku Controllers that can be used in a number of ways. **Requires: Core**
* **Modifiers** - A package that contains a number of useful Fire Modifiers. **Requires: Core**
* **Player** - A package for creating player characters for danmaku games. **Requires: Core**
* **Enemy** - A package for creating enemies for Danmaku games, ranging from the standard cannon fodder to the stage bosses. **Requires: Core**
* **Etc** - A general package full of random useful utilities. **Requires: Core**
* **NoScript** - A general system for creating complex danmaku patterns without needing to code much. **Requires: Core, Modifiers** 
* **Touhou** - A specialized package for single player danmaku games with many of the same mechanics as many of the mainline Touhou games, **Requires: Core**
* **Phantasmagoria** - A specialized package for multiplayer versus danmaku games similar to Touhou Kaeidzuka ~ Phantasmagoria of Flower View and Touhou 3 - Touhou Yumejikuu ~ Phantasmagoria of Dim.Dream, **Requires: Core**

#Installation

To get DanmakU running in your Unity project, follow these instructions:

1. Go to the releases page for this repository ([link](https://github.com/Rhythmia/DanmakU/releases)) and find the appropriate release version (usually this is the latest release that is for your version of Unity).
2. Download the attached .unitypackage binary.
3. In Unity, open the project you wish to use the development kit with.
4. In the menu at the top, click Assets > Import Package > Custom Package.
5. Navigate to where you downloaded the unitypackage to and open it.
6. It will then prompt you which assets to add. Select all of them, and click Import.
7. Restart Unity. (this refreshes Unity, and pulls up all of the Unity Editor extensions created for this dev kit).

#Source Code Installation/Setup

To get the source code up and running, simply clone this repository using your prefered form of git (GUI, command line, etc).
