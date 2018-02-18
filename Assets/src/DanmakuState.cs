using System;
using UnityEngine;

namespace DanmakU {

[Serializable]
public struct DanamkuConfig {

  public Vector2 Position;
  public Range Rotation;
  public Range Speed;
  public Range AngularVelocity;
  public Color Color;

  public DanmakuState CreateState() {
    return new DanmakuState {
      Position = Position,
      Rotation = Rotation.GetValue(),
      Speed = Speed.GetValue(),
      AngularVelocity = AngularVelocity.GetValue(),
      Color = Color
    };
  }

}

[Serializable]
public struct DanmakuState {
  public Vector2 Position;
  public float Rotation;
  public float Speed;
  public float AngularVelocity;
  public Color Color;
}

}