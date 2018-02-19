using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace DanmakU {

[AddComponentMenu("DanmakU/Modifiers/Acceleration")]
public class DanmakuAcceleration : MonoBehaviour, IDanmakuModifier {

  public float Acceleration;

  public JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency = default(JobHandle)) {
    var acceleration = Acceleration * Time.deltaTime;
    if (Mathf.Approximately(acceleration, 0f)) return dependency;
    return new ApplyAcceleration {
      Acceleration = Acceleration * Time.deltaTime,
      Speeds = pool.Speeds
    }.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
  }

  struct ApplyAcceleration : IJobParallelFor {

    public float Acceleration;
    public NativeArray<float> Speeds;
    
    public void Execute(int index) {
      Speeds[index] += Acceleration;
    }

  }

}

}