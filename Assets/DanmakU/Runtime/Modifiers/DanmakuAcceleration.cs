using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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
    if (Mathf.Approximately(acceleration.Size, 0f)) {
      return new ApplyFixedAcceleration {
        Count = pool.ActiveCount,
        Acceleration = acceleration.Center,
        Speeds = pool.Speeds
      }.Schedule();
    } else {
      return new ApplyRandomAcceleration {
        Acceleration = acceleration.Center,
        Speeds = pool.Speeds
      }.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
    }
  }

  struct ApplyFixedAcceleration : IJob, IJobParallelFor {

    public int Count;
    public float Acceleration;
    public NativeArray<float> Speeds;
    
    public unsafe void Execute() {
      var ptr = (float*)(Speeds.GetUnsafePtr());
      var end = ptr + Count;
      while (ptr < end) {
        *ptr++ += Acceleration;
      }
    }

    public void Execute(int index) {
      Speeds[index] += Acceleration;
    }

  }

  struct ApplyRandomAcceleration : IJobParallelFor {

    public Range Acceleration;
    public NativeArray<float> Speeds;
    
    public void Execute(int index) {
      Speeds[index] += Acceleration.GetValue();
    }

  }

}

}