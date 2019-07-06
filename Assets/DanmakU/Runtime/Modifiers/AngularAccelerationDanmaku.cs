using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU.Modifiers {

/// <summary>
/// A MonoBehaviour <see cref="DanmakU.IDanmakuModifier"/> that applies a constant
/// acceleration to all bullets.
/// </summary>
[AddComponentMenu("DanmakU/Modifiers/Danmaku Angular Acceleration")]
public class AngularAccelerationDanmaku : MonoBehaviour, IDanmakuModifier {

  /// <summary>
  /// The angular acceleration applied to bullets. Units is in radians per second per second.
  /// </summary>
  public Range AngularAcceleration, angularSpeedFactor; //the power to which angular speed affects angular acceleration (speed^factor)

  public JobHandle UpdateDanmaku(DanmakuPool pool, JobHandle dependency = default(JobHandle)) {
	var angularAcceleration = AngularAcceleration * Time.deltaTime;
	if (angularAcceleration.Approximately(0f)) return dependency;
    if (Mathf.Approximately(angularAcceleration.Size, 0f)) {
      return new ApplyFixedAngularAcceleration {
        Count = pool.ActiveCount,
        AngularAcceleration = angularAcceleration.Center,
		AngularSpeedFactor = angularSpeedFactor.Center,
        AngularSpeeds = pool.AngularSpeeds
      }.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
    } else {
      return new ApplyRandomAngularAcceleration {
        AngularAcceleration = angularAcceleration.GetValue(),
		AngularSpeedFactor = angularSpeedFactor.GetValue(),
        AngularSpeeds = pool.AngularSpeeds
      }.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
    }
  }

  struct ApplyFixedAngularAcceleration : IJob, IJobParallelFor {

    public int Count;
    public float AngularAcceleration, AngularSpeedFactor;
    public NativeArray<float> AngularSpeeds;
	private float realAngularAcceleration;
    
    public unsafe void Execute() {
      var ptr = (float*)(AngularSpeeds.GetUnsafePtr());
      var end = ptr + Count;
      while (ptr < end) {
		realAngularAcceleration = (AngularSpeedFactor != 0) ? AngularAcceleration * ((float)System.Math.Pow(System.Math.Abs(*ptr),AngularSpeedFactor) + 1f) : AngularAcceleration;
        *ptr++ += realAngularAcceleration;
      }
    }

    public void Execute(int index) {
	  realAngularAcceleration = (AngularSpeedFactor != 0) ? AngularAcceleration * ((float)System.Math.Pow(System.Math.Abs(AngularSpeeds[index]),AngularSpeedFactor) + 1f) : AngularAcceleration;
      AngularSpeeds[index] += AngularAcceleration;
    }

  }

  struct ApplyRandomAngularAcceleration : IJobParallelFor {

    public float AngularAcceleration, AngularSpeedFactor;
    public NativeArray<float> AngularSpeeds;
	private float realAngularAcceleration;
    
    public void Execute(int index) {
	  realAngularAcceleration = (AngularSpeedFactor != 0) ? AngularAcceleration * ((float)System.Math.Pow(System.Math.Abs(AngularSpeeds[index]),AngularSpeedFactor) + 1f) : AngularAcceleration;
      AngularSpeeds[index] += realAngularAcceleration;
    }

  }

}

}