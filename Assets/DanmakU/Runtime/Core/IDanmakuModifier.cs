using Unity.Jobs;

namespace DanmakU {

/// <summary>
/// A composable modifier to runtime behaviour of a set of bullets.
/// </summary>
/// <seealso cref="DanmakU.DanmakuSet"/>
public interface IDanmakuModifier {

  /// <summary>
  /// Adds a modification Unity Job to change a <see cref="DanmakU.DanmakuPool"/> and its
  /// bullets.
  /// </summary>
  /// <param name="pool">the pool to change with the </param>
  /// <param name="dependency">the most recent job changing the pool.</param>
  /// <returns>the new "latest" job working with the pool.</returns>
  /// <example>
  /// The simplest implementation is the no-op, which simply returns the dependency
  /// JobHandle back.
  /// <code>
  /// public JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency) => dependency;
  /// </code>
  /// </example>
  /// <example>
  /// The simplest non-trivial implementation is to add a single job that changes the way the
  /// way the DanmakuPool is updated.
  /// <code>
  /// public float acceleration;       // Member variable
  /// public JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency) {
  ///   if (Mathf.Approximately(acceleration, 0f)) return dependency; // Don't do work if you don't need to.
  ///   return new AccelerateJob {
  ///     Acceleration = acceleration,                                // Copy over the approriate data for processing.
  ///     Speeds = pool.Speeds                                        
  ///   }.Schedule(pool.ActiveCount, 32, dependency);                 // Make sure to wait for the previous job to finish.
  /// }
  /// 
  /// public struct AccelerateJob : IJobParallelFor {
  ///   public float Acceleration;
  ///   public NativeArray&lt;float&gt; Speeds;
  /// 
  ///   public void Execute(int indes) {
  ///     Speeds[index] += Acceleration;
  ///   }
  /// }
  /// </code>
  /// </example>
  JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency);

}

}