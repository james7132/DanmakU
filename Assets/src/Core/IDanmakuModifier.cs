using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DanmakU {

public abstract class DanmakuModifier {

  public virtual JobHandle PreUpdate(DanmakuPool pool, JobHandle dependency) => dependency;
  public virtual JobHandle PostUpdate(DanmakuPool pool, JobHandle dependency) => dependency;

}

}