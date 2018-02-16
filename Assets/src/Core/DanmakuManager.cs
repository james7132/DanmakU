using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

public class DanmakuManager : MonoBehaviour {

  public static DanmakuManager Instance;

  public Bounds Bounds;

  public static bool InBounds(Vector2 position) {
    return Instance.Bounds.Contains(position);
  }

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    Instance = this;
  }

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireCube(Bounds.center, Bounds.size);
  }

}

}