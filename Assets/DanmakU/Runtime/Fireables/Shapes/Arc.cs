using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

[Serializable]
public class Arc : Fireable {

  public Range Count;
  public Range ArcLength;
  public Range Radius;

  public Arc(Range count, Range arcLength, Range radius) {
    Count = count;
    ArcLength = arcLength;
    Radius = radius;
  }

  public override void Fire(DanmakuConfig state) {
    float radius = Radius.GetValue();
    int count = Mathf.RoundToInt(Count.GetValue());
    float arcLength = ArcLength.GetValue();
    var rotation = state.Rotation.GetValue();
    var start = rotation - arcLength / 2;
    var currentState = state;
    for (int i = 0; i < count; i++) {
      var angle = start + i * (arcLength / count);
      state.Position = state.Position + (radius * RotationUtiliity.ToUnitVector(angle));
      state.Rotation = angle;
      Subfire(currentState);
    }
  }
}

}