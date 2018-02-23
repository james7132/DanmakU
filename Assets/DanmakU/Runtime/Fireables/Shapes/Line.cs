using System;
using UnityEngine;

namespace DanmakU.Fireables {

[Serializable]
public class Line : Fireable {

  public Range Count = 1;
  public Range DeltaSpeed;

  public override void Fire(DanmakuConfig config) {
    var count = Mathf.RoundToInt(Count.GetValue());
    var deltaSpeed = DeltaSpeed.GetValue();
    for (var i = 0; i < count; i++) {
      config.Speed += deltaSpeed;
      Subfire(config);
    }
  }

}

}