using Unity.Jobs;

namespace DanmakU {

/// <summary>
/// A composable modifier to runtime behaviour of a set of bullets.
/// </summary>
/// <seealso cref="DanmakU.DanmakuSet"/>
public interface IDanmakuModifier {

  /// <summary>
  /// Adds a modification Unity Job to add 
  /// </summary>
  /// <param name="pool"></param>
  /// <param name="dependency"></param>
  /// <returns></returns>
  JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency);

}

}