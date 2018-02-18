using DanmakU;
using DanmakU.Fireables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPoolTest : DanmakuBehaviour {

  Danmaku danmakuTest;

  public int PoolSize;
  public DanmakuPrefab prefab;
  public DanmakuRenderer Renderer;
  public DanamkuConfig State;
  public Ring Ring;
  public Circle Circle;
  IFireable fireable;
  float timer;
  RaycastHit2D[] raycastCache;

	void Start () {
    fireable = Ring.Of(Circle) .Of(CreateSet(prefab));
	}

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    timer += Time.deltaTime;
    if (timer > 1/20f) {
      State.Rotation += 20 * Mathf.Deg2Rad;
      fireable.Fire(State);
      timer = 0;
    }
  }

}
