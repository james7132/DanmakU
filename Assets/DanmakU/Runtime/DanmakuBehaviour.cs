using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

/// <summary>
/// A base MonoBehaviour class for writing scripts that work with Danmaku.
/// </summary>
/// <remarks>
/// A given DanmakuBehaviour manages the lifetime of any of it's created <see cref="DanmakU.DanmakuSet"/>s.
/// Sets created with <see cref="CreateSet"/> will automatically be disposed of when the component is
/// destroyed. This means any bullets fired by these sets will be destroyed when the component is destroyed.
/// </remarks>
public abstract class DanmakuBehaviour : MonoBehaviour {

  List<DanmakuSet> OwnedDanmakuSets;

  /// <summary>
  /// Create a <see cref="DanmakU.DanmakuSet"/> from a prefab.
  /// </summary>
  /// <param name="prefab">the base prefab to create a set from.</param>
  /// <returns>the created DanmakuSet.</returns>
  protected DanmakuSet CreateSet(DanmakuPrefab prefab) {
    var pool = new DanmakuPool(prefab.DefaultPoolSize);
    pool.ColliderRadius = prefab.ColliderRadius;
    var set = DanmakuManager.Instance.CreateDanmakuSet(prefab.GetRendererConfig(), pool);
    (OwnedDanmakuSets ?? (OwnedDanmakuSets = new List<DanmakuSet>())).Add(set);
    set.AddModifiers(prefab.GetModifiers());
    return set;
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() {
    if (OwnedDanmakuSets == null) return;
    var manager = DanmakuManager.Instance;
    foreach (var danmakuSet in OwnedDanmakuSets) {
      if (manager != null) {
        manager.DestroyDanmakuSet(danmakuSet);
      }
      danmakuSet.Dispose();
    }
  }

}

}