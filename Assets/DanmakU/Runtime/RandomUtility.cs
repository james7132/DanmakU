using System;
using UnityEngine;
using Random = System.Random;

namespace DanmakU {

internal static class RandomUtility {

  [ThreadStatic] static Random Random = new Random();

  public static float Range(float min, float max) {
    var random = Random ?? (Random = new Random());
    return Mathf.Lerp(min, max, (float)random.NextDouble());
  }

}

}