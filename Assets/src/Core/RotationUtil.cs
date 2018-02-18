using System.Runtime.CompilerServices;
using UnityEngine;

public static class RotationUtil {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 ToUnitVector(float rotation) {
    const float HalfPI = Mathf.PI / 2;
    return new Vector2(SinHP(rotation + HalfPI), SinHP(rotation));
  }

  /// <summary>
  /// A fast approximate sine function.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float SinHP(float t) {
    const float TwoPI = Mathf.PI * 2;
    t = (t % TwoPI + TwoPI) % TwoPI;
    t = t - Mathf.PI;
    float sin = t * (1.27323954f - Mathf.Sign(t) * 0.405284735f * t);
    float sinSign = Mathf.Sign(sin);
    sin *= sinSign * 0.255f * (sin - sinSign) + 1;
		return sin;
  }

}
