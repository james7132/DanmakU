using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

public class DestroyDanmakuCollider : MonoBehaviour {

  public DanmakuCollider Collider;

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  void OnEnable() {
    if (Collider != null) {
      Debug.Log("Subscribed");
      Collider.OnDanmakuCollision += OnDanmakuCollision;
    }
  }

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  void OnDisable() {
    if (Collider != null) {
      Collider.OnDanmakuCollision -= OnDanmakuCollision;
    }
  }

  void OnDanmakuCollision(DanmakuCollisionList collisions) {
    foreach (var collision in collisions) {
      collision.Danmaku.Destroy();
    }
  }

  /// <summary>
  /// Reset is called when the user hits the Reset button in the Inspector's
  /// context menu or when adding the component the first time.
  /// </summary>
  void Reset() {
    Collider = GetComponent<DanmakuCollider>();
  }

}

}