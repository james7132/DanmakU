using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU.Modifiers {
	
/// <summary>
/// A MonoBehaviour <see cref="DanmakU.IDanmakuModifier"/> that applies a constant
/// acceleration to all bullets.
/// </summary>
[AddComponentMenu("DanmakU/Modifiers/Danmaku Exponential Speed")]
public class ExpSpeedDanmaku : MonoBehaviour, IDanmakuModifier {

  /// <summary>
  /// changes speed with respect to time. Units is in game units per second.
  /// </summary>
  public float algorithmOffset,coefficient, powBase, plusC;

  public JobHandle UpdateDanmaku(DanmakuPool pool, JobHandle dependency = default(JobHandle)) {
    if (Mathf.Approximately(coefficient,0f)) return dependency; //skip if no speed
		return new ExpSpeed{
			Count = pool.ActiveCount,
			Times = pool.Times,
			Speeds = pool.Speeds,
			coefficient = coefficient,
			powBase = powBase,
			plusC = plusC,
			offset = algorithmOffset
		}.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
  }
  
  struct ExpSpeed : IJob, IJobParallelFor {

    public int Count;
    public NativeArray<float> Speeds;
	[ReadOnly]
    public NativeArray<float> Times;
	public float coefficient, powBase, plusC, offset;
	private float elapsedTime;
    
    public unsafe void Execute() {
      var ptr = (float*)(Times.GetUnsafePtr());
      var psr = (float*)(Speeds.GetUnsafePtr());
	  
      var timeEnd = ptr + Count;
	  var speedEnd = psr + Count;
	  
      while (ptr < timeEnd && psr < speedEnd) {
		elapsedTime = *ptr - offset;
		*psr = coefficient * (float)System.Math.Pow(powBase,elapsedTime) + plusC;
        ptr++;
		psr++;
      }
    }

    public void Execute(int index) {
		elapsedTime = Times[index] - offset;
		Speeds[index] = coefficient * (float)System.Math.Pow(powBase,elapsedTime) + plusC;
    }

  }
}

}