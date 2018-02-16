using System;
using UnityEngine;

namespace DanmakU.Fireables {

public class PoolFireable : Fireable {

  readonly DanmakuPool Pool;

  public PoolFireable(DanmakuPool pool) {
    if (pool == null) {
      throw new ArgumentNullException(nameof(pool));
    }
    Pool = pool;
  }

  public override void Fire(DanmakuState state) {
    var danmaku = Pool.Get();
    danmaku.Position = state.Position;
    danmaku.Rotation = state.Rotation.GetValue();
    danmaku.Speed = state.Speed.GetValue();
    danmaku.AngularSpeed = state.AngularVelocity.GetValue();
    danmaku.Color = state.Color;
  }

}

}