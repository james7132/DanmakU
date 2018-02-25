using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace DanmakU {

/// <summary>
/// Represents a 2D axis aligned bounding box.
/// 
/// An axis-aligned bounding box, or AABB for short, is a box aligned with coordinate axes and fully 
/// enclosing some object. Because the box is never rotated with respect to the axes, it can be defined 
/// by just its <see cref="Center"/> and <see cref="Extents"/>, or alternatively by <see cref="Min"/> 
/// and <see cref="Max"/> points.
/// </summary>
[Serializable]
public struct Bounds2D {

  /// <summary>
  /// The center of the bounding box.
  /// </summary>
  public Vector2 Center;

  /// <summary>
  /// The extents of the Bounding Box. This is always half of the size of the Bounds.
  /// </summary>
  public Vector2 Extents;

  /// <summary>
  /// The minimal point of the box. This is always equal to Center-Extents.
  /// </summary>
  public Vector2 Min {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Center - Extents; }
    set { SetMinMax(value, Max); }
  }

  /// <summary>
  /// The maximal point of the box. This is always equal to Center+Extents.
  /// </summary>
  public Vector2 Max {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Center + Extents; }
    set { SetMinMax(Min, value); }
  }

  /// <summary>
  /// The total size of the box. This is always twice as large as the Extents.
  /// </summary>
  public Vector2 Size {
    get { return Extents * 2; }
    set { Extents = value * 0.5f; } 
  }

  /// <summary>
  /// Creates a new Bounds2D.
  /// </summary>
  /// <param name="center">The location of the origin of the Bounds2D.</param>
  /// <param name="size">The dimensions of the Bounds2D.</param>
  public Bounds2D(Vector2 center, Vector2 size) {
    Center = center;
    Extents = size * 0.5f;
  }

  /// <summary>
  /// Sets the bounds to the min and max value of the box.
  /// </summary>
  public void SetMinMax(Vector2 min, Vector2 max) {
    Extents = (max - min) * 0.5f;
    Center = min + Extents;
  }

  /// <summary>
  /// Grows the Bounds to include the <paramref cref="point"/>.
  /// </summary>
  /// <param name="point">the target point to expand to include.</param>
  public void Encapsulate(Vector2 point) {
    SetMinMax(Vector2.Min(Min, point), Vector2.Max(Max, point));
  }
  
  /// <summary>
  /// Grow the bounds to encapsulate the bounds.
  /// </summary>
  /// <param name="bounds">the new bounds to encapsulate.</param>
  public void Encapsulate(Bounds2D bounds) {
    Encapsulate(bounds.Center - bounds.Extents);
    Encapsulate(bounds.Center + bounds.Extents);
  }

  /// <summary>
  /// Expand the bounds by increasing its <see cref="Size"/> by <paramref cref="amount"/> along each side.
  /// </summary>
  public void Expand(float amount) {
    amount *= 0.5f;
    Extents.x += amount;
    Extents.y += amount;
  }

  /// <summary>
  /// Expand the bounds by increasing its <see cref="Size"/> by <paramref cref="amount"/> along each side.
  /// </summary>
  public void Expand(Vector2 amount) {
    amount *= 0.5f;
    Extents.x += amount.x;
    Extents.y += amount.y;
  }

  /// <summary>
  /// Does another bounding box intersect with this bounding box?
  /// 
  /// Check if the bounding box comes into contact with another bounding box. 
  /// </summary>
  /// <param name="bounds">the bounds to check against.</param>
  /// <returns>true if there is an intersection between bounds, false otherwise.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Intersects(Bounds2D bounds) {
    return (Math.Abs(Center.x - bounds.Center.x) < Extents.x + bounds.Size.x) &&
           (Math.Abs(Center.y - bounds.Center.y) < Extents.y + bounds.Extents.y);
  }

  /// <summary>
  /// Is <paramref cref="point"/> contained in the bounding box?
  /// </summary>
  /// <param name="point">the point to test.</param>
  /// <returns>true if <paramref cref="point"/> is inside the bounding box, false otherwise.</returns>
  /// <remarks>If any component of <see cref="Extents"/> is negative, this will always return false.</remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Contains(Vector2 point) {
    return Math.Abs(point.x - Center.x) <= Extents.x && Math.Abs(point.y - Center.y) <= Extents.y;
  }

  public static implicit operator Bounds2D(Bounds bounds) {
    return new Bounds2D(bounds.center, bounds.size);
  }

  public static implicit operator Bounds(Bounds2D bounds) {
    Vector3 size = bounds.Extents * 2;
    size.z = float.MaxValue;
    return new Bounds(bounds.Center, size);
  }

  public static bool operator ==(Bounds2D lhs, Bounds2D rhs) {
    return lhs.Center == rhs.Center && lhs.Extents == rhs.Extents;
  }

  public static bool operator !=(Bounds2D lhs, Bounds2D rhs) {
    return lhs.Center != rhs.Center ||  lhs.Extents != rhs.Extents;
  }

  public override bool Equals(object other) {
    if (!(other is Bounds)) return false;
    return ((Bounds2D)other) == this;
  }

  public override int GetHashCode() => Center.GetHashCode() ^ (Extents.GetHashCode() << 2);

  public override string ToString() => $"(Center: {Center}, Extents: {Extents})";

}

}