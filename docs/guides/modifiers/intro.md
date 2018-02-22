---
title: Modifiers
---

When we started off creating patterns we used Fireables to create bullet
patterns, but what if we want to modify the behavior of bullets after they've
been fired? **Danmaku Modifiers** are the answer to that. Danmaku Modifiers
alter the behavior of all bullets managed by @DanmakU.DanmakuPool objects.
For example, the modifier @DanmakU.Modiifers.DanmakuAcceleration applies a
constant acceleration to all bullets in the set.

# Adding Modifiers Programatically
Modifers can be added to and removed from @DanmakU.DanmakuSet objects:

```csharp
DanmakuSet bullets = CreateSet(bulletPrefab);
IDanmakuModifier modifier = new CustomModifier();
bullets.AddModifier(modifier)             // These APIs support chaining calls.
       .AddModifier(new CustomModiifer()) // Order matters. First added modifier is executed first in the loop.
       .RemoveModifier(modiifier);
```

# Adding Modifiers to Danmaku Prefabs
When creating a DanmakuSet from a @DanmakU.DanmakuPrefab, any component that
implements IDanmakuModifier will be added as a modifier to the set. This allows
designers to add, remove, and tweak modifiers without needing to touch the code
of the project.

# Appendix: Built In Modifiers
There is only one built in modifier right now:

 * @DanmakU.Modifiers.DanmakuAcceleration - Adds acceleration to bullets,
