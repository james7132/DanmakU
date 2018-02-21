using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DanmakU {

/// <summary>
/// A enumerable set of bullets that supports runtime modification of behaviors through
/// zero or more <see cref="DanmakU.IDanmakuModifier"/>.
/// </summary>
/// <example>
/// Many of the methods of DanmakuSet follow a fluent API for changing the modifiers on the set.
/// Multiple commands can be chained one after another in a fluent manner.
/// <code>
/// DanmakuSet danmakuSet;
/// IDanmakuModifier customModifier1 = new CustomModifier1();
/// danmakuSet.AddModifier(customModifer1)          // Add the first modifier
///           .AddModifier(new CustomModifier2())   // Add the second modifier inline
///           .RemoveModifier(customModifer1);      // Remove the first modifier
/// </code>
/// </example>
public class DanmakuSet : IEnumerable<Danmaku>, IFireable, IDisposable {

  /// <summary>
  /// The underlying pool of bullets.
  /// </summary>
  public readonly DanmakuPool Pool;
  readonly List<IDanmakuModifier> Modifiers;

  internal DanmakuSet(DanmakuPool pool) {
    Pool = pool;
    Modifiers = new List<IDanmakuModifier>();
  }

  /// <summary>
  /// Adds a modifier to the set.
  /// </summary>
  /// <param name="modifier">the modifier to add</param>
  /// <returns>the original set with the modifier added.</returns>
  public DanmakuSet AddModifier(IDanmakuModifier modifier) {
    Modifiers.Add(modifier);
    return this;
  }

  /// <summary>
  /// Adds a set of modifiers to the set.
  /// </summary>
  /// <param name="modifiers">the range of modifiers to add to the set.</param>
  /// <returns>the original set with the modifiers added.</returns>
  public DanmakuSet AddModifiers(IEnumerable<IDanmakuModifier> modifiers) {
    Modifiers.AddRange(modifiers);
    return this;
  }

  /// <summary>
  /// Removes a modifier from the set.
  /// </summary>
  /// <param name="modifier">the modifier to remove from the set.</param>
  /// <returns>the original set with the modifiers removed.</returns>  
  public DanmakuSet RemoveModifier(IDanmakuModifier modifier) {
    Modifiers.Remove(modifier);
    return this;
  }

  /// <summary>
  /// Checks if a <see cref="DanmakU.Danmaku"/> is contained within the set.
  /// </summary>
  /// <param name="danmaku">the bullet to check the membership of.</param>
  /// <returns>true if the bullet belongs to the set, false otherwise.</returns>  
  public bool Contains(Danmaku danmaku) {
    return Pool == danmaku.Pool && danmaku.Id > 0 && danmaku.Id < Pool.ActiveCount;
  }

  /// <inheritdoc/>
  public DanmakuEnumerator GetEnumerator() => Pool.GetEnumerator();
  /// <inheritdoc/>
  IEnumerator<Danmaku> IEnumerable<Danmaku>.GetEnumerator() => GetEnumerator();
  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  /// <summary>
  /// Clears all modifiers from the set.
  /// </summary>
  public void ClearModifiers() => Modifiers.Clear();

  internal JobHandle Update(JobHandle dependency) {
    foreach (var modifier in Modifiers) {
      dependency = modifier.UpdateDannmaku(Pool, dependency);
    }
    dependency = Pool.Update(dependency);
    return dependency;
  }

  /// <summary>
  /// Disposes of the set. This destroys all of the bullets in the set and disposes of
  /// the underlying pool backing the set.
  /// </summary>
  public void Dispose() => Pool.Dispose();

  /// <summary>
  /// Creates and fires a single bullet based on a given config.
  /// </summary>
  /// <param name="config">the config for creating the bullet</param>
  /// <returns>the created bullet</returns>
  public Danmaku Fire(DanmakuConfig config) => Pool.Get(config);

  /// <inheritdoc/>
  void IFireable.Fire(DanmakuConfig config) => Fire(config);

}

}