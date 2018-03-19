using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

[Serializable]
public class Circle : Fireable {

  public Range Count = 1;
  public Range Radius;

  public Circle(Range count, Range radius) {
    Count = count;
    Radius = radius;
  }

  public override void Fire(DanmakuConfig state) {
    float radius = Radius.GetValue();
    int count = Mathf.RoundToInt(Count.GetValue());
    var rotation = state.Rotation.GetValue();
    var origin = state.Position;
    state.Rotation = rotation;
    for (int i = 0; i < count; i++) {
      var angle = rotation + i * (Mathf.PI * 2 / count);
      state.Position = origin + (radius * RotationUtiliity.ToUnitVector(angle));
      Subfire(state);
    }
  }

}

}

