using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

public abstract class DanmakuBehaviour : MonoBehaviour {

  List<DanmakuSet> OwnedDanmakuSets;

  protected DanmakuSet CreateSet(DanmakuPrefab prefab) {
    var set = DanmakuManager.Instance.CreateDanmakuSet(prefab.GetRendererConfig());
    // TODO(james7132): Add fetching modifier components here;
    (OwnedDanmakuSets ?? (OwnedDanmakuSets = new List<DanmakuSet>())).Add(set);
    return set;
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() {
    if (OwnedDanmakuSets == null) return;
    foreach (var danmakuSet in OwnedDanmakuSets) {
      danmakuSet.Dispose();
    }
  }

}

}