using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DanmakU {

public abstract class DanmakuModifier {

  public virtual JobHandle PreUpdate(DanmakuPool pool, JobHandle dependency) => dependency;
  public virtual JobHandle PostUpdate(DanmakuPool pool, JobHandle dependency) => dependency;

}

public static class IDanmakuModifierExtensions {

  public static JobHandle RunUpdates(this List<DanmakuModifier> modifiers, 
                                     DanmakuPool pool,
                                     JobHandle dependency = default(JobHandle)) {
    foreach (var modifier in modifiers) {
      dependency = modifier.PreUpdate(pool, dependency);
    }
    dependency = pool.Update(dependency);
    foreach (var modifier in modifiers) {
      dependency = modifier.PostUpdate(pool, dependency);
    }
    return dependency;
  }


}

}