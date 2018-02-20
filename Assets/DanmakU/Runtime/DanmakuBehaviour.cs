using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

public abstract class DanmakuBehaviour : MonoBehaviour {

  List<DanmakuSet> OwnedDanmakuSets;

  protected DanmakuSet CreateSet(DanmakuPrefab prefab) {
    var pool = new DanmakuPool(prefab.DefaultPoolSize);
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