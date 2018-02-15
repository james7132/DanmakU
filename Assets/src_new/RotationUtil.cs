using System.Runtime.CompilerServices;

public static class RotationUtil {

  internal const float kRotationAccuracy = Mathf.PI / 10000;
  internal const int kRotationCacheSize = (int)(Mathf.PI * 2 / kRotationAccuracy);
  internal static Vector2[] RotationCache;

  static RotationUtil() {
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
    index = (Mathf.Abs(index * kRotationCacheSize) + index) % kRotationCacheSize;
    return RotationCache[index];
  }

}
