using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace DanmakU.Modifiers {

/// <summary>
/// A MonoBehaviour <see cref="DanmakU.IDanmakuModifier"/> that applies a constant
/// acceleration to all bullets.
/// </summary>
[AddComponentMenu("DanmakU/Modifiers/Danmaku Acceleration")]
public class DanmakuAcceleration : MonoBehaviour, IDanmakuModifier {

  /// <summary>
  /// The acceleration applied to bullets. Units is in game units per second per second.
  /// </summary>
  public Range Acceleration;

  public JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency = default(JobHandle)) {
    var acceleration = Acceleration * Time.deltaTime;
    if (acceleration.Approximately(0f)) return dependency;
    return new ApplyAcceleration {
      Acceleration = acceleration,
      Speeds = pool.Speeds
    }.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
  }

  struct ApplyAcceleration : IJobParallelFor {

    public Range Acceleration;
    public NativeArray<float> Speeds;
    
    public void Execute(int index) {
      Speeds[index] += Acceleration.GetValue();
    }

  }

}

}