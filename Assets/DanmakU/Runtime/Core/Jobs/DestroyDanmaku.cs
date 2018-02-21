using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

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

  public void Execute() {
    var activeCount = ActiveCountArray[0];
    for (var i = 0; i < activeCount; i++) {
      if (Times[i] > 0) continue;
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