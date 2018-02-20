# DanmakU Documentation

**DanmakU** is a high performance engine for simulating and rendering bullet-like objects for the
Unity3D engine.

If this is your first time using DanmakU, you should refer to the Intro for tutorials. More experienced users might 
refer to the API Documentation for a breakdown of the individuals objects in the library.

For more realtime support, it is suggested to join the [DanmakU Discord Server](https://discordapp.com/invite/e9G43m2).

## Why another bullet hell engine?

There are quite a few bullet hell engines out there, including a few Unity Asset Store Packages:

  * [Touhou Danmakufu](https://dmf.shrinemaiden.org/wiki/Main_Page)
  * [BulletML](http://www.asahi-net.or.jp/~cs8k-cyu/bulletml/index_e.html)
  * [Uni Bullet Hell](https://assetstore.unity.com/packages/tools/integration/uni-bullet-hell-19088)
  * [Danmaku Engine](https://assetstore.unity.com/packages/templates/systems/danmaku-engine-29855)
  * [BulletML for Unity](https://assetstore.unity.com/packages/tools/bulletml-for-unity-16206)

The big question is why add to this already large list of working solutions? Many of these engines for 
building bullet hell games fall short in dealing with the following observations:

**In bullet hell games, there can be hundreds, if not thousands, of active entities simulating and
rendering to the screen at any given time.** This presents a twofold performance issue: processing
a large number of objects eats up lots of CPU time, and traditionally rendering each bullet seperately
can result in a very high number of GPU draw calls, bottlenecking the game on lower end hardware. DanmakU
resolves these issues with a triangle of performance optimizations:

* **Full Multithreaded Simulation** - The entire simulation system is built to fully utilize all of
  the CPU cores on the machine. 
* [**Data Oriented Programming**](https://android-developers.googleblog.com/2015/07/game-performance-data-oriented.html) - 
  CPU cache misses rapidly degrade computational performance. By keeping all bullet data tightly packed in 
  contiguous memory, we minimize the number of cache misses, keeping the CPU as busy as possible.
* [**GPU Instancing**](https://docs.unity3d.com/Manual/GPUInstancing.html) - Unity3D supports rendering large batches of 
  similar objects in one call to the GPU. DanmakU makes only one draw call to the GPU for every 1023 bullets on the screen, 
  keeping the GPU as active as possible while minimizing CPU overhead.

With the rise of higher framerate machines and monitors, the amount of time in between each update tick of a
game shrinks with each coming year. Minimizing the performance impact of managing and rendering bullets opens much
more headroom for developers to expand upon their games in more meaningful ways. DanmakU has been viciously optimized
to minimize the update latency, even at very high bullet counts.

**99.9% of bullets are "fire and forget"**. Processing and tracking bullets should be focused on 
and optimized for applying broad attributes and rules that hold true throughout a bullets's lifetime.
The remaining 0.1% are relative few in number and can controlled through much more specialized routines.
DanmakU's core concept of *Danmaku Modifiers* focus on optimizing the general case and giving the performance
headroom for the developer to further specialize for that last 0.1%.

**Bullet patterns are geometrically composable**. A major pain point of many other bullet hell engines is the
sheer amount of work to get rather simple patterns. As a generalization, most provide primitive commands
for creating a single bullet or perhaps a simple geometric shape. Composing more complex patterns requires 
explicit scripting of the pattern with code or markup. With DanmakU's *Fireables API*, firing a 
ring of bullets, circle of bullets, line of bullets, or even a ring of circles of bullets should be as simple
as firing a single bullet.

**Allocating garbage on every update can result in massive lag spikes that ruin the game's experience.** 
Many bullet hell engines were designed to be object-oriented (i.e. a bullet is an object), or focus on using
coroutines as the main form of scripting (i.e. Touhou Danmakufu). Allocating these objects and contexts on the 
heap can be quite expensive.

**Higher quality games are developed faster when the iteration time to implement and test changes is shorter.**
This has been a keystone behind Unity for the past decade, but has generally been slow in adoption in 
bullet hell engines. Many require either long recompilation times, editing some text file (like BulletML), or require 
parsing text scripts using a built binary (i.e. Touhou Danmakufu). Many DanmakU elements are serializable and support
being edited in the Unity Editor while the game is running for realtime feedback on the bullet designs created.

Finally, DanmakU is and always will be fully open source, unlike all of the aforementioned solutions. You are free
to use and extend DanmakU for any project, whether that be a commercial game or a hobbyist fan project.