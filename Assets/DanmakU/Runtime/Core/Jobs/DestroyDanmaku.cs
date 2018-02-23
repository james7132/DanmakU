using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace DanmakU {

internal struct DestroyDanmaku : IJob {

  public NativeArray<int> ActiveCountArray;
  public NativeArray<float> Times;
  public NativeArray<DanmakuState> InitialStates;
  public NativeArray<Vector2> Positions;
  public NativeArray<float> Rotations;
  public NativeArray<float> Speeds;
  public NativeArray<float> AngularSpeeds;
  public NativeArray<Vector4> Colors;

  public DestroyDanmaku(DanmakuPool pool) {
    ActiveCountArray = pool.activeCountArray;
    Times = pool.Times;
    InitialStates = pool.InitialStates;
    Positions = pool.Positions;
    Rotations = pool.Rotations;
    Times = pool.Times;
    Speeds = pool.Speeds;
    AngularSpeeds = pool.AngularSpeeds;
    Colors = pool.Colors;
  }

  public unsafe void Execute() {
    var activeCount = Mathf.Max(0, ActiveCountArray[0]);
    var timePtr = (float*)Times.GetUnsafeReadOnlyPtr();
    for (var i = 0; i < activeCount; i++) {
      if (*(timePtr++) >= 0) continue;
      activeCount--;
      InitialStates[i] = InitialStates[activeCount];
      Times[i] = Times[activeCount];
      Positions[i] = Positions[activeCount];
      Rotations[i] = Rotations[activeCount];
      Speeds[i] = Speeds[activeCount];
      AngularSpeeds[i] = AngularSpeeds[activeCount];
      Colors[i] = Colors[activeCount];
    }
    ActiveCountArray[0] = activeCount;
  }

}

}