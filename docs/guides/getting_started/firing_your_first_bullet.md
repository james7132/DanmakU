
# Creating the Danmaku Manager singleton
> [!IMPORTANT]
> It is discouraged to call `DontDestoryOnLoad` on the Danmaku Manager
> component to allow it to survive a scene load. If you must keep it across scene
> loads, remember to release the data it owns periodically.

The power of **DanmakU** comes from the scripting together @DanmakU.Fireables 
and @DanmakU.Modifiers to create complex patterns. Included in the package is 
a simple emitter that is intended to be a jumping off point for more complex
behaviors. This guide walks you through setting up a scene with an emitter 
continually firing.

## Your First Bullet
1. Make sure you have completed the installation instructions. 

1. Select `File -> New Scene` to create a new scene

1. Create an empty game object in the scene. Click `Add Component` and add a 
@DanmakU.DanmakuManager component to the scene.

   Name it `Danmaku Manager` (the name isn't actually important, but it helps 
   disambiguate it in the scene.)

   The @DanmakU.DanmakuManager manages the danmaku (bullets) in your scene, and 
   it controls the bounds for danmaku..
   
   Keep everything default for now. Later, you can use the Center and Extents 
   properties to change the size of the area where bullets will automatically
   despawn. 

   Your scene must have a DanmakuManager object. **DanmakU will fail to run 
   without one.** 

1. Add another empty game object to the scene. This time, add a
@DanmakU.DanmakuPrefab component to the object.

   Name it `Danmaku Prefab`. This object will be the template we use for our bullets.

   @DanmakU.DanmakuPrefab defines a bullet type for your game. It contains a 
   reference to the shader, sprite (or mesh), and collision information for
   that bullet type. 

1. Included with the project is a sprite called `Big1`. Drag it onto the 
`Rendering->Sprite` field of `Danmaku Prefab`

1. In the `Danmaku\Runtime\Shaders` folder there is a file called 
`Default Sprite Danmaku`. Drag this to the `Rendering->Shader` property for the
`Danmaku Prefab` game object. Leave all the other fields their default values.

1. Add a third empty game object. Add a @DanmakU.DanmakuEmitter component to it.

   Name it `Danmaku Emitter`. Make sure it is positioned at (0, 0, 0), as bullets
   will be emitted from it.

   The @DanmakU.DanmakuEmitter class is a simple bullet emitter meant to be a 
   jumping off point for your own emitter types. It shows how you can compose 
   many behaviors together to create different patterns.

1. Drag `Danmaku Prefab` on the `Danmaku Type` field of `Danmaku Emitter`.

1. Expand `Arc`, if it is not already, and change `Arc->Count` to 1.

1. Press play. You should see a steady stream of bullets being fired to the right.

1. Stop the game and change `Fire Rate` to 1, `Arc->Count` to 3, 
`Arc-Length` to 30, `Line->Count` to 3, and `Line->Delta Speed` to 2.

1. Continue playing with the properties of `Danmaku Emitter`, including the 
position and rotation of the gameobject. 

> [!NOTE]
> You may need to adjust your Main Camera settings, and the import settings for the Big1 image to get the bullets to be the correct size.
