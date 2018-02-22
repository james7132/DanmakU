---
title: Installing DanmakU
---

DanmakU only works in context of the Unity3D engine. Right now, it is required to
use the Unity 2018.1 Beta to use DanmakU, as there are a number of core APIs that
are only available in the beta currently. It you do not already have Unity3D
installed, it is suggested you install it using the following steps:

## Installing Unity
1. Install the [Unity Hub](https://forum.unity.com/threads/unity-hub-preview-0-12-0-is-now-available.516479/).
1. Run the Unity Hub.
1. Install the latest 2018.1b version (Install > Beta Releases)
1. Create a new project (New > Select 2018.1b)
1. Open the project (Projects > Double Click Project)

## Adding DanmakU to a project
1. Be sure you are using the .NET 4.6 Equivalent Mono Runtime.
   (Edit > Project Settings > Player)
1. Go to the [Github Releases](https://github.com/james7132/DanmakU/releases).
1. Download the latest release's \*.unitypackage.
1. In Unity, load the package (Assets > Import Package > Custom Pacakge).
1. Import all assets.

> [!NOTE]
> Moving the DanmakU directory does not affect any of the package at all. Feel free to move it anywhere it is approriate.
