using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DanmakU {

public class DanmakuCollider : MonoBehaviour {

  struct ColliderData {
    public Bounds Bounds;
    public int LayerMask;
  }

  static readonly List<DanmakuCollider> Colliders;
  static readonly List<Bounds>[] Data;
  static Bounds GlobalBounds;
  static int GlobalLayerMask;
  static int HighestLayer;

  static DanmakuCollider() {
    Colliders = new List<DanmakuCollider>();
    Data = new List<Bounds>[sizeof(int) * 8];
    for (var i = 0; i < Data.Length; i++) {
      Data[i] = new List<Bounds>();
    }
  }

  public static int ColliderCount => Colliders.Count;

  public static void RebuildSpatialHashes() {
    for (var i = 0; i < Data.Length; i++) {
      Data[i].Clear();
    }
    GlobalLayerMask = 0;
    HighestLayer = -1;
    bool globalInit = false;
    foreach (var collider in Colliders) {
      if (collider != null && collider.isActiveAndEnabled) {
        var layer = collider.gameObject.layer;
        Data[layer].Add(collider.data.Bounds);
        GlobalLayerMask |= 1 << layer;
        HighestLayer = Mathf.Max(layer + 1, HighestLayer);
        if (globalInit) {
          GlobalBounds.Encapsulate(collider.data.Bounds);
        } else {
          GlobalBounds = collider.data.Bounds;
          globalInit = true;
        }
      }
    }
    var center = GlobalBounds.center;
    center.z = 0;
    GlobalBounds.center = center;
  }

  Collider2D[] colliders;
  ColliderData data;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    colliders = GetComponents<Collider2D>();
    data = BuildData();
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
    data = BuildData(); 
  }

  internal static int TestCollisions(Bounds bounds)  {
    if (!GlobalBounds.Intersects(bounds)) return 0;
    int collisions = 0;
    for (var i = 0; i < HighestLayer; i++) {
      var mask = 1 << i;
      if ((GlobalLayerMask & mask) == 0) continue;
      var layerColliders = Data[i];
      for (var j = 0; j < layerColliders.Count; j++) {
        if (!layerColliders[j].Intersects(bounds)) continue;
        collisions |= mask;
        break;
      }
    }
    return collisions;
  }

  ColliderData BuildData() {
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
    extents.z = float.MaxValue;
    fullBounds.extents = extents;
    return new ColliderData {
      Bounds = fullBounds,
      LayerMask = 1 << gameObject.layer
    };
  }

}

}