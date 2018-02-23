using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

[Serializable]
public class Arc : Fireable {

  public Range Count = 1;
  [Radians] public Range ArcLength;
  public Range Radius;

  public Arc(Range count, Range arcLength, Range radius) {
    Count = count;
    ArcLength = arcLength;
    Radius = radius;
  }

  public override void Fire(DanmakuConfig state) {
    float radius = Radius.GetValue();
    int count = Mathf.RoundToInt(Count.GetValue());
    if (count == 0) return;
    if (count == 1) {
      Subfire(state);
      return;
    }
    float arcLength = ArcLength.GetValue();
    var rotation = state.Rotation.GetValue();
    var start = rotation - arcLength / 2;
    for (int i = 0; i < count; i++) {
      var angle = start + i * (arcLength / (count - 1));
      var currentState = state;
      currentState.Position = state.Position + (radius * RotationUtiliity.ToUnitVector(angle));
      currentState.Rotation = angle;
      Subfire(currentState);
    }
  }
}

}