using System.Runtime.CompilerServices;
using UnityEngine;

public static class RotationUtil {

  internal const float kRotationAccuracy = Mathf.PI / 10000;
  internal const int kRotationCacheSize = (int)(Mathf.PI * 2 / kRotationAccuracy);
  internal static Vector2[] RotationCache;

  static RotationUtil() {
    // TODO(james7132): This sadly promotes cache misses, can we do better than caching unit vectors?
    RotationCache = new Vector2[kRotationCacheSize];
    for (var i = 0; i < RotationCache.Length; i++) {
      var angle = kRotationAccuracy * i;
      RotationCache[i] = new Vector2 {
        x = Mathf.Cos(angle),
        y = Mathf.Sin(angle)
      };
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 ToUnitVector(float rotation) {
    int index = (int)(rotation / kRotationAccuracy);
    index = (index %= kRotationCacheSize) < 0 ? index + kRotationCacheSize : index;
    return RotationCache[index];
  }

}
