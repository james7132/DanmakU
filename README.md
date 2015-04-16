# DanmakU
DanmakU is an open source danmaku development kit for Unity3D.

# Getting Started
See the [wiki](https://github.com/rhythmia/DanmakU/wiki) on a number of tutorials on how to get started.  
See the [script documentation](http://rhythmia.org/DanmakU/Docs/html/annotated.html) to see a scripting reference.

# Dependencies
This development kit relies heavily on [UnityUtilLib](https://github.com/james7132/UnityUtilLib), a utility library.

All releases of DanmakU, including the source code releases, come packaged with the version of UnityUtilLib that is used.

It is also being tested with [Vexe Framework](http://forum.unity3d.com/threads/free-vfw-full-set-of-drawers-savesystem-serialize-interfaces-generics-auto-props-delegates.266165/) as a way to serialize a number of objects that are not currently easily serialized by Unity.

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

To get the source code up and running, execute the following command in your command line:

    git clone --recursive https://github.com/Rhythmia/DanmakU.git
    
Note: the "--recursive" parameter is required to successfully clone the right UnityUtilLib version. 

For older clones or git versions older than 1.6.5, use the following commands.

    git clone https://github.com/Rhythmia/DanmakU.git
    cd DanmakU
    git submodule update --init --recursive
