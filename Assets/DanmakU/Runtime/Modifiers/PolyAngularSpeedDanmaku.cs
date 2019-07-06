using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU.Modifiers {
	
/// <summary>
/// A MonoBehaviour <see cref="DanmakU.IDanmakuModifier"/> that applies a constant
/// acceleration to all bullets.
/// </summary>
[AddComponentMenu("DanmakU/Modifiers/Danmaku Polynomial Speed")]
public class PolyAngularSpeedDanmaku : MonoBehaviour, IDanmakuModifier {

  /// <summary>
  /// changes angular speed with respect to time. Units is in radians per second.
  /// </summary>
  public float algorithmOffset;
  public float[] coefficients,powers;
  private NativeArray<float> nativeCoefficients,nativePowers;
  
  public void OnEnable(){ //converts float[] to native array
	  if(nativeCoefficients.IsCreated){
		nativeCoefficients.Dispose();
	  }
	  if(nativePowers.IsCreated){
		nativePowers.Dispose();
	  }
	  nativeCoefficients = new NativeArray<float>(coefficients,Allocator.Persistent);
	  nativePowers = new NativeArray<float>(powers,Allocator.Persistent);
  }
  
  public void OnDestroy(){
	  nativeCoefficients.Dispose();
	  nativePowers.Dispose();
  }
  

  public JobHandle UpdateDanmaku(DanmakuPool pool, JobHandle dependency = default(JobHandle)) {
    if (coefficients.Length < 1 || coefficients.Length > powers.Length) return dependency; //skip if constant speed, or if more coefficients than powers
	return new PolyAngularSpeed{
		Count = pool.ActiveCount,
		Times = pool.Times,
        AngularSpeeds = pool.AngularSpeeds,
		Coefficients = nativeCoefficients,
		Powers = nativePowers,
		offset = algorithmOffset
	}.Schedule(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
  }

  struct PolyAngularSpeed : IJob, IJobParallelFor {

    public int Count;
    public NativeArray<float> AngularSpeeds;
	[ReadOnly]
	public NativeArray<float> Times,Coefficients,Powers;
	public float offset;
	private float elapsedTime;
    
    public unsafe void Execute() {
      var ptr = (float*)(Times.GetUnsafePtr());
      var psr = (float*)(AngularSpeeds.GetUnsafePtr());
	  
      var timeEnd = ptr + Count;
	  var speedEnd = psr + Count;
	  
      while (ptr < timeEnd && psr < speedEnd) {
		elapsedTime = *ptr - offset;
		*psr = 0;
		for(int i=0;i < Coefficients.Length;i++){
			if(Coefficients[i] != 0 && !((System.Math.Abs(elapsedTime) < 0.0001f || elapsedTime > 5f) && Powers[i] < 0)){ // prevent giant numbers
				*psr += Coefficients[i] * (float)System.Math.Pow(elapsedTime,Powers[i]);
			}
		}
        ptr++;
		psr++;
      }
    }

    public void Execute(int index) {
		elapsedTime = Times[index] - offset;
		AngularSpeeds[index] = 0;
		for(int i=0;i < Coefficients.Length;i++){
			if(Coefficients[i] != 0 && !((System.Math.Abs(elapsedTime) < 0.0001f || elapsedTime > 5f) && Powers[i] < 0)){ // prevent divide giant numbers
				AngularSpeeds[index] += Coefficients[i] * (float)System.Math.Pow(elapsedTime,Powers[i]);
			}
		}
    }

  }
}

}