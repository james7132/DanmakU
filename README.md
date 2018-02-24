# DanmakU [![Docs](https://img.shields.io/badge/docs-passing-brightgreen.svg)](https://jamessliu.com/DanmakU) [![license](https://img.shields.io/github/license/james7132/DanmakU.svg)](./LICENSE) [![Discord](https://discordapp.com/api/guilds/346069036557271052/widget.png)](https://discordapp.com/invite/e9G43m2)
DanmakU is an high performance, open source development kit for Unity3D focused on simplifying the  development of 2D bullet hell games.

Check out the [documentation](https://jamessliu.com/DanmakU) or join the [Discord Server](https://discordapp.com/invite/e9G43m2) for more realtime support.

### Features

 * Comprehensive toolset for firing and managing large quantities of similar 
   objects.
 * Built for high multithreaded performance with the Unity C# Jobs system and 
   GPU instancing.
 * (Virtually) Zero garbage collection allocs.
 * Compatible with, and built on the Unity MonoBleedingEdge (.NET 4.6) runtime.
 * Easy and composable bullet pattern construction with the Fireables API.

### Requirements and Caveats

 * Unity 2018.1 or newer.
 * Support for [Procedural GPU Instancing](https://docs.unity3d.com/Manual/GPUInstancing.html). All shaders used to render bullets must have GPU Instancing enabled. Requires Shader Model 4.5 or newer.
