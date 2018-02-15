using Unity.Collections;
using UnityEngine;

public struct Danmaku {

  internal readonly int Index;
  public readonly DanmakuPool Pool;

  internal Danmaku(DanmakuPool pool, int index) {
    Pool = pool;
    Index = index;
  }

  public Vector2 Position {
    get { return Pool.Positions[Index]; }
    set { Pool.Positions[Index] = value; }
  }

  public float Rotation {
    get { return Pool.Rotations[Index]; }
    set { Pool.Rotations[Index] = value; }
  }

  public float Speed {
    get { return Pool.Speeds[Index]; }
    set { Pool.Speeds[Index] = value; }
  }

  public float AngularSpeed {
    get { return Pool.AngularSpeeds[Index]; }
    set { Pool.AngularSpeeds[Index] = value; }
  }

  public Color Color {
    get { return Pool.Colors[Index]; }
    set { Pool.Colors[Index] = value; }
  }

  /// <summary>
  /// Destroys the danmaku object.
  /// </summary>
  /// <remarks>
  /// Calling this funciton simply queues the Danmaku for destruction. It will be
  /// recycled back into it's pool on the pool's next update cycle.
  ///
  /// Destroying the same danmaku more than once or attempting to edit an already
  /// destroyed Danmaku will likely result in undefined behavior.
  /// </remarks>
  public void Destroy() => Pool.Destroy(this);

}
