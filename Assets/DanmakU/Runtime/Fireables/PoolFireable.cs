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

  public override void Fire(DanamkuConfig config) {
    Pool.Get(config);
  }

}

}