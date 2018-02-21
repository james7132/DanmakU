using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

public class RandomColor : Fireable {

  readonly Gradient _source;

  public RandomColor(Gradient source) {
    _source = source;
  }

  public override void Fire(DanmakuConfig state) {
    state.Color = _source.Evaluate(Random.value);
    Subfire(state);
  }

}

}