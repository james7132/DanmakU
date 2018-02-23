using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// A set of lower-precision functions for assisting in working with Danmaku rotations.
/// </summary>
public static class RotationUtiliity {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 ToUnitVector(float rotation) {
    return new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
  }

}
