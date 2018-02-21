using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DanmakU {

/// <summary>
/// A value type that represents a range of floating point values.
/// </summary>
/// <example>
/// Ranges are implicitly convertible to and from floats and support most basic
/// arithmetic with them.
/// <code>
/// Range r1 = 5f;                  // Converted to a range of [5, 5]
/// Range r2 = r1.Expanded(5);      // Expanded range to [0, 10]
/// Range r3 = r2 + 4;              // Shifted range to [4, 14]
/// Range r4 = r3 - 9;              // Shifted range to [-5, 5]
/// Range r5 = r4 * 20;             // Multiplied range to [-100, 100]
/// Range r6 = r5 / 10;             // Divided range to [-10, 10]
/// </code>
/// </example>
[Serializable]
public struct Range {

  [SerializeField] float _min;
  [SerializeField] float _max;

  /// <summary>
  /// Gets the smallest value in the range.
  /// </summary>
  public float Min => _min;

  /// <summary>
  /// Gets the largest value in the range.
  /// </summary>
  public float Max => _max;

  /// <summary>
  /// Gets the midpoint value of the range.
  /// </summary>
  public float Center => (Max + Min) / 2f;

  /// <summary>
  /// Gets the size of the range.
  /// </summary>
  public float Size => Max - Min;

  /// <summary>
  /// Creates a range scoped to the value of only one value.
  /// </summary>
  /// <param name="val">the center value of the range</param>
  /// <example>
  /// <code>
  /// new Range(0)         // A range of [0, 0]
  /// new Range(5)         // A range of [5, 5]
  /// new Range(1000, 100) // A range of [100, 100]
  /// </code>
  /// </example>
  public Range(float val) : this(val, val) {}

  /// <summary>
  /// Creates a range based on an interval of values.
  /// </summary>
  /// <param name="a">one extrema of the range.</param>
  /// <param name="b">one extrema of the range.</param>
  /// <example>
  /// <code>
  /// new Range(0, 5)   // A range of [0, 5]
  /// new Range(5, 0)   // Another way to express [0, 5]
  /// new Range(5, 100) // A range of [5, 100]
  /// new Range(100, 5) // Another way to express of [5, 100]
  /// </code>
  /// </example>
  public Range(float a, float b) {
    if (a > b) {
      _min = b;
      _max = a;
    } else {
      _max = b;
      _min = a;
    }
  }

  /// <summary>
  /// Creates a new Range expanded from the current range.
  /// </summary>
  /// <remarks>
  /// Can be used to shrink the new range by providiing a negative size.
  /// If the absolute value of <paramref cref="size"/> is larger than the
  /// <see cref="Size"/> of the Range, the range will
  /// </remarks>
  /// <param name="size">the size</param>
  /// <returns></returns>
  public Range Expanded(float size) {
    var extents = size / 2f;
    return new Range(Min - extents, Max + extents);
  }

  /// <summary>
  /// Gets whether the range is close to a single value and approximately the same
  /// as a given floating point value.
  /// </summary>
  /// <param name="value">the target value to check against.</param>
  /// <returns>true if the entire range is approximately the target value, false otherwise.</returns>
  public bool Approximately(float value) {
    return Mathf.Approximately(Size, 0f) && Mathf.Approximately(Center, value);
  }

  /// <summary>
  /// Clamps a value to the limits of the range.
  /// </summary>
  /// <param name="value">the value to clamp.</param>
  /// <returns>the clamped value.</returns>
  public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

  /// <summary>
  /// Uniformily samples a value from the region.
  /// </summary>
  /// <returns>the randomly sampled value.</returns>
  public float GetValue() => RandomUtility.Range(Min, Max);

  public static implicit operator Range(float val) => new Range(val);

  public static Range operator +(Range lhs, Range rhs) => new Range(lhs.Min + rhs.Min, lhs.Max + rhs.Max);
  public static Range operator -(Range lhs, Range rhs) => new Range(lhs.Min - rhs.Min, lhs.Max - rhs.Max);
  public static unsafe Range operator *(Range lhs, Range rhs) {
    float* values = stackalloc float[4];
    values[0] = lhs.Min * rhs.Min;
    values[1] = lhs.Max * rhs.Min;
    values[2] = lhs.Min * rhs.Max;
    values[3] = lhs.Max * rhs.Max;
    float min = float.MaxValue, max = float.MinValue;
    for (var i = 0; i < 4; i++) {
      min = Math.Min(min, values[i]);
      max = Math.Max(max, values[i]);
    }
    return new Range { _min = min, _max = max };
  }
  public static Range operator /(Range lhs, Range rhs) => lhs * (1f / rhs);
  public static Range operator *(Range lhs, float rhs) => new Range { _min = lhs._min * rhs, _max = lhs._max * rhs };
  public static Range operator /(Range lhs, float rhs) => new Range { _min = lhs._min / rhs, _max = lhs._max / rhs };
  public static Range operator /(float lhs, Range rhs)  {
    rhs._min = rhs._min == 0f ? float.NegativeInfinity : lhs / rhs._min;
    rhs._max = rhs._max == 0f ? float.PositiveInfinity : lhs / rhs._max;
    return rhs;
  }

}

}
