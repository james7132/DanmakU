using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DanmakU {

public class DanmakuCollider : MonoBehaviour {

  static readonly List<DanmakuCollider> Colliders;

  static DanmakuCollider() {
    Colliders = new List<DanmakuCollider>();
  }

  public static int ColliderCount => Colliders.Count;

  Collider2D[] colliders;
  Bounds totalBounds;
  int layerMask;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    colliders = GetComponentsInChildren<Collider2D>();
    totalBounds = BuildBounds();
  }

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  void OnEnable() => Colliders.Add(this);

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  void OnDisable() => Colliders.Remove(this);

  /// <summary>
  /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
  /// </summary>
  void FixedUpdate()  {
    totalBounds = BuildBounds(); 
    layerMask = 1 << gameObject.layer;
  }

  internal static int TestCollisions(Bounds bounds)  {
    int collisions = 0;
    foreach (var collider in Colliders) {
      if (!collider.totalBounds.Intersects(bounds)) continue;
      collisions |= collider.layerMask;
    }
    return collisions;
  }

  public Bounds BuildBounds() {
    Bounds? bounds = null;
    foreach (var collider in colliders) {
      if (collider != null && collider.enabled && collider.gameObject.activeInHierarchy) {
        if (bounds == null) {
          bounds = collider.bounds;
        } else {
          bounds.Value.Encapsulate(collider.bounds);
        }
      }
    }
    var fullBounds = bounds ?? new Bounds(transform.position, Vector3.zero);
    var extents = fullBounds.extents;
    extents.z = float.PositiveInfinity;
    fullBounds.extents = extents;
    return fullBounds;
  }

}

}