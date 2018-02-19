using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DanmakU {

internal struct DestroyDanmaku : IJob {

  public AtomicCounter ActiveCount;  
  public NativeStack<int> Destroyed;

  internal NativeArray<float> Times;

  public NativeArray<DanmakuState> InitialStates;

  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;

  public NativeArray<float> Speeds;
  public NativeArray<float> AngularSpeeds;

  public NativeArray<Vector4> Colors;

  public void Execute() {
    while (Destroyed.Count > 0) {
      var index = Destroyed.Pop();
      var activeCount = ActiveCount.Decrement();

      InitialStates[index] = InitialStates[activeCount];
      Times[index] = Times[activeCount];
      Positions[index] = Positions[activeCount];
      Rotations[index] = Rotations[activeCount];
      Speeds[index] = Speeds[activeCount];
      AngularSpeeds[index] = AngularSpeeds[activeCount];
      Colors[index] = Colors[activeCount];
    }
  }

}

}