
# Creating the Danmaku Manager singleton
> [!IMPORTANT]
> It is actually discouraged to call `DontDestoryOnLoad` on the Danmaku Manager
> component to allow it to survive a scene load. If you must keep it across scene
> loads, remember to release the data it owns periodically.

The main required component need for everything in DanmakU to function in the
**Danmaku Manager** component.
