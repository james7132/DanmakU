using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DanmakU {

public class DanmakuSet : IEnumerable<Danmaku>, IFireable, IDisposable {

  public readonly DanmakuPool Pool;
  readonly List<DanmakuModifier> Modifiers;

  internal DanmakuSet(DanmakuPool pool) {
    Pool = pool;
    Modifiers = new List<DanmakuModifier>();
  }

  public DanmakuSet AddModifier(DanmakuModifier modifier) {
    Modifiers.Add(modifier);
    return this;
  }

  public DanmakuSet AddModifiers(IEnumerable<DanmakuModifier> modifiers) {
    Modifiers.AddRange(modifiers);
    return this;
  }

  public DanmakuSet RemoveModifiers(DanmakuModifier modifier) {
    Modifiers.Remove(modifier);
    return this;
  }

  public bool Contains(Danmaku danmaku) {
    return Pool == danmaku.Pool && danmaku.Id > 0 && danmaku.Id < Pool.ActiveCount;
  }

  public DanmakuEnumerator GetEnumerator() => Pool.GetEnumerator();
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public void ClearModifiers() => Modifiers.Clear();

  internal JobHandle Update(JobHandle dependency) {
    foreach (var modifier in Modifiers) {
      dependency = modifier.PreUpdate(Pool, dependency);
    }
    dependency = Pool.Update(dependency);
    foreach (var modifier in Modifiers) {
      dependency = modifier.PostUpdate(Pool, dependency);
    }
    return dependency;
  }

  public void Dispose() => Pool.Dispose();

  public void Fire(DanamkuConfig config) {
    Pool.Get(config);
  }

}

}