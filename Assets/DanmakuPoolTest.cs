using DanmakU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPoolTest : MonoBehaviour {

  DanmakuPool pool;
  Danmaku danmakuTest;

  public int PoolSize;
  public DanmakuRenderer Renderer;
  int counter;

	void Start () {
    pool = new DanmakuPool(PoolSize);
    for (int i = 0; i < PoolSize; i++) {
      var danmaku = pool.Get();
      danmaku.Position = 40 * Random.insideUnitCircle;
      danmaku.Speed = Random.value;
      danmaku.Rotation = Random.value * Mathf.PI * 2;
      danmaku.AngularSpeed = Random.Range(-1f, 1f) * Mathf.PI / 128;
      danmaku.Color = new Color(Random.value,Random.value, Random.value);
    }
    if (Renderer != null) {
      Renderer.Pool = pool;
    }
	}

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    pool.Update().Dispose();
    // for (int i = 0; i < objects.Length; i++) {
    //   objects[i].localPosition = pool.Positions[i];
    //   objects[i].localRotation = Quaternion.Euler(0, 0, pool.Rotations[i] * Mathf.Rad2Deg);
    // }
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() => pool.Dispose();

}
