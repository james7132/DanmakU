---
title: Custom Fireables
---

# Subclassing Fireable
The simplest way to fully integrate into the Fireables API is to create a
subclass of @DanmakU.Fireables.Fireable. The only required function that one is

Each Fireable instance contains a reference to its immediate child. As each
Fireable can thus have a hierarchy of descendants below it. Every call of `Fire`
thus traverses this hierarchy, with each parent Fireable calling `Fire` on it's
child zero or more times.

An important protected method is `Subfire` which checks if the Fireable has a
child or not, and then triggers a fire on it if and only if it is present.

The usual process is to take the DanmakuConfig provided as an argument to `Fire`
and change it to based on some parameters of the Fireable, then use the modified
config to fire the child. For example, if we wanted to make a line of bullets
that fire from the same location, but has

```csharp
public class CustomLine : Fireable {

  public int Count;
  public float DeltaSpeed;

  public void Fire(DanmakuConfig config) {
    for (var i = 0; i < Count; i++) {
      config.Speed += DeltaSpeed;
      Subfire(config);
    }
  }

}
```

It is important to remember that @DanmakU.DanmakuConfig is a struct and thus
uses value semantics. Modifying a config in a call to `Fire` will not modify the
original.

# Implementing IFireable
An alternative is to directly implement @DanmakU.IFireable. Most of the previous
comments hold true for the same; however, there is no support for further
composition as IFireable does not expose a method or property for managing
children. This is generally advised if and only if you are making an alternative
IFireable that doesn't create traditional @DanmakU.Danmaku via DanmakuSet and
instead want to output some other custom bullet type.

# Supporting Editor Serialization
To support editing Fireables in the Unity Editor, the Fireable implementation
must be serializable. For the most part, this involves adding the `Serializable`
attribute to the type:

```csharp
using System; // Required as the attribute is in the System namespace

[Serializable]
public class CustomLine : Fireable {
  ...
}
```
To make sure that the types and config you are serializing are valid, please
refer to the [Unity Manual](https://docs.unity3d.com/Manual/script-Serialization.html).

# Adding Randomization
Adding randomization to a Fireable is simple by using the @DanmakU.Range struct.
Ranges are a serializable value that can represent either a single floating
point value, or a interval on the real number line. Ranges support uniformly
sampling a value from within the range via @DanmakU.Range.GetValue, each call to
GetValue will return a random number from the range. If the range represents a
single value, GetValue will always return that value.  DanmakuConfig is
built upon Ranges so a Fireable may be passed a randomized configuration.

Depending on how you sample values, the behaviour of randomization may change.
Let's add randomization to CustomLine to demonstrate this:

```csharp
public class CustomLine : Fireable {

  public int Count;
  public Range DeltaSpeed;

  public void Fire(DanmakuConfig config) {
    for (var i = 0; i < Count; i++) {
      config.Speed += DeltaSpeed.GetValue();
      Subfire(config);
    }
  }

}
```

In this example, we are randomly sampling a new DeltaSpeed each step of the
generation. This will indeed create a line, but the difference in velocity may
not be uniform from one bullet to the next within a single line. If this is
undesirable, we need to sample DeltaSpeed only once for the entire line:

```csharp
public class CustomLine : Fireable {

  public int Count;
  public Range DeltaSpeed;

  public void Fire(DanmakuConfig config) {
    var deltaSpeed = DeltaSpeed.GetValue();
    for (var i = 0; i < Count; i++) {
      config.Speed += deltaSpeed;
      Subfire(config);
    }
  }

}
```

But what if we want to randomize the number of bullets within the line as well?
We can change Count to be a Range too! However, Range.GetValue returns a float,
not a integer! We can use Unity's math functions to remedy this.
`Mathf.FloorToInt`, `Mathf.RoundToInt`, and `Mathf.CeilToInt` can help us
discretize the results we get from GetValue:

```csharp
public class CustomLine : Fireable {

  public Range Count;
  public Range DeltaSpeed;

  public void Fire(DanmakuConfig config) {
    var deltaSpeed = DeltaSpeed.GetValue();
    for (var i = 0; i < Mathf.FloorToInt(Count.GetValue()); i++) {
      config.Speed += deltaSpeed;
      Subfire(config);
    }
  }

}
```

We now are able to make randomized lines of bullets now! But, we have a bug
here: Count's value is being resampled every iteration of the for loop: the
number of bullets we are expecting is not stable within a single fire. To remedy
this, we again move the sampling out of the loop and sample only once per
context where it's valid, this case being once per Fire:

```csharp
public class CustomLine : Fireable {

  public Range Count;
  public Range DeltaSpeed;

  public void Fire(DanmakuConfig config) {
    var count = < Mathf.FloorToInt(Count.GetValue());
    var deltaSpeed = DeltaSpeed.GetValue();
    for (var i = 0; i < count; i++) {
      config.Speed += deltaSpeed;
      Subfire(config);
    }
  }

}
```

## Interval Arithmetic
@DanmakU.Range is built upon interval arithmetic, a subset of computations that
focus on intervals of real numbers. For more information, see the [Wikipedia article](https://en.wikipedia.org/wiki/Interval_arithmetic).
