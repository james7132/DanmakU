---
title: Fireables
---

90% of the scripting work in creating a bullet hell game is creating the initial conditions for bullets. The **Fireables API**
attempts to attempts to create decrease the amount of boilerplate code needed to create more complex patterns, and
encouranging reuse through composable firing patterns.

It should be noted that Fireables are meant to create bullet patterns that fire at the same time. Firing a Fireable 
immediately creates any bullets that might have come out of it. There is no way to introduce a wait time between fires.
This is by design. Creating time sequenced patterns is usually done by altering the parameters to Fireables over time.

# The IFireable Interface
The IFireable interface exposes only one function: `Fire(DanmakuConfig)`, during which it uses the provided 
DanmakuConfig to fire *something*, what that *something* is up to the implementation of the Fireable.

## DanmakuConfig
@DanmakU.DannmakuConfig is a simple data struct that contains information about how to fire a Fireable.

Many of it's fields are @DanmakU.Range which represents a either a single value, or range of values that are valid. 
When firing bullets, each Range field is evaluated, sampling a random value from that range. This allows bullet pattern 
designers to add randomization to the pattern without changing the code.

# Creating Bullets with Fireables
The type @DanmakU.DanmakuSet implements the IFireable interface and can be used as a standalone Fireable. In the original
Getting Started tutorial, you were already using the Fireables API!

Let's take a look at another Fireable: the `Ring`. The Ring fireabe creates a full circle of bullets based on a few 
parameters:
```
IFireable ring = new Ring(5, 2);
ring.Fire(...);
```
However, the above code does not fire any bullets! By themselves most Fireables do not create any bullets, but are rather
templates to create more complex patterns with other fireables. For this we must compose the Ring with the DanmakuSet we created:
```
IFireable bullets = CreateSet(danmakuPrefab);
IFireable ring = new Ring(5, 2).Of(bullets);
ring.Fire(...);
```
Now we finally get a set of bullets coming out! In the next section, we will explain exactly how this composition works.

# Composing Fireables
The most powerful feature behind Fireables are the fact that they are geometrically composable. This is done through
the `Of` extension method which composes two fireables together:

```
IFireable ring = new Ring(5, 2);
IFireable circles = new Circle(3, 3);
IFireable ringOfCircles = ring.Of(circles);
```

When a Fireable is created in the form of `f = x.Of(y)`, calling `f.Fire(...)` will replace every normal bullet fire in `x`
with a complete fire of `y`.

As the return of the `Of` call is a Fireable, `Of` calls can also be chained to create higher order compositions. This
continues the composition order `x.Of(y).Of(z)...` creates a Fireable that is composed of instances of `x` replaced by
`y`, which are in turn replaced by instances of `z`, and so on. For example:
```
IFireable higherOrderFireable = ring.Of(circles).Of(lines).Of(bullets);
```

# Configuring Fireables in the Editor
All of the built-n Fireables support being serialized as fields in Unity objects.

```
public class CustomBulletPattern : DanmakuBehaviour {
  public DanmakuPrefab Prefaab;
  // Serialize the fireables as a part of the behaviour
  public Ring Ring;
  public Circle Circles;
  IFireable fireable;
  
  void Start() {
    fireable = Ring.Of(Circles).Of(CreateSet(Prefab));
  }

  void Update() {
    fireable.Fire(new DanmakuConfig());
  }
}
```

This enables two important integrations with the Unity Engine: Inspector Editability and Animation. By doing this, developers
can edit the pattern at runtime, and also support animating the bullet pattern using Unity's animation system.

# Appendix: Built-In Fireables

## Ring
TODO(james7132): Document

## Circle
TODO(james7132): Document
