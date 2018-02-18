using DanmakU;
using DanmakU.Fireables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPoolTest : MonoBehaviour {

  DanmakuPool pool;
  Danmaku danmakuTest;

  public int PoolSize;
  public DanmakuRenderer Renderer;
  public DanmakuState State;
  IFireable fireable;
  float timer;
  RaycastHit2D[] raycastCache;

	void Start () {
    pool = new DanmakuPool(PoolSize);
    fireable = 
      new Ring(5, 0)
        .Of(new Circle(8, 3))
        .Of(new PoolFireable(pool));
    if (Renderer != null) {
      Renderer.Pool = pool;
    }
    raycastCache = new RaycastHit2D[256];
	}

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    pool.ColliderRadius = 1;
    DanmakuCollider.RebuildSpatialHashes();
    pool.Update().Complete();
    timer += Time.deltaTime;
    if (timer > 1/20f) {
      State.Rotation += 20 * Mathf.Deg2Rad;
      fireable.Fire(State);
      timer = 0;
    }
    var bounds = DanmakuManager.Instance.Bounds;
    var size = bounds.extents;
    size.z = float.MaxValue;
    bounds.extents = size;
    foreach (var danmaku in pool) {
      if (!bounds.Contains(danmaku.Position)) {
        danmaku.Destroy();
        continue;
      }
      var layerMask = pool.CollisionMasks[danmaku.Id];
      if (layerMask == 0) continue; 
      var oldPosition = pool.OldPositions[danmaku.Id];
      var direction = oldPosition - danmaku.Position;
      var distance = direction.magnitude;
      var hits = Physics2D.CircleCastNonAlloc(oldPosition, pool.ColliderRadius, direction, raycastCache, distance, layerMask);
      if (hits <= 0) continue;
      danmaku.Destroy();
    }
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() => pool.Dispose();

}
