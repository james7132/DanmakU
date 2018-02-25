using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace DanmakU {

/// <summary>
/// A single bullet.
/// </summary>
/// <remarks>
/// Internally implemented as a index and a reference to the pool that owns the Danmaku.
/// Does not actually hold any of the information about the Danmaku: the type is implemented
/// as a value-type struct (to avoid garbage allocation), but the semantics for editing any 
/// property is akin to that of a reference type. All the actual data is stored in the backing
/// DanmakuPool.
/// 
/// As the internal data of the backing DanmakuPool may be moved around after an update, the 
/// internal index of Danmaku may not point to the same logical bullet from update to update.
/// As a result, holding long term stores of Danmaku is ill-advised. Likewise editing Danmaku outside
/// of the scope from which they are read from the pool (i.e. <see cref="Danmaku.Get"/> or via enumeration)
/// is equally ill-advised and may lead to undefined behavior.
/// </remarks>
public struct Danmaku {

  /// <summary>
  /// The ID of the Danmaku. Guarenteed to be unique within the pool.
  /// </summary>
  public readonly int Id;

  /// <summary>
  /// The backing pool of the Danmaku.
  /// </summary>
  public readonly DanmakuPool Pool;

  internal Danmaku(DanmakuPool pool, int index) {
    Pool = pool;
    Id = index;
  }

  /// <summary>
  /// Gets the number of seconds since the Danmaku was created.
  /// </summary>
  public float Time => Pool.Times[Id];

  public DanmakuState InitialState => Pool.InitialStates[Id];

  /// <summary>
  /// Gets or sets the world position of the Danmaku.
  /// </summary>
  public Vector2 Position {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Pool.Positions[Id]; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set { Pool.Positions[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the rotation of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in radians. 0 points directly right. Pi points directly left.
  /// Controls both the graphical rotation as well as the direction of motion
  /// the Danmaku is moving in.
  /// </remarks>
  public float Rotation {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Pool.Rotations[Id]; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set { Pool.Rotations[Id] = value; }
  }

  /// <summary>
  /// Gets the direction the Danmaku is facing. Guarenteed to be a unit vector.
  /// </summary>
  public Vector2 Direction => GetDirection(Pool.Rotations[Id]);

  /// <summary>
  /// Gets or sets the speed of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in units per second. Movement direction is based on <see cref="Rotation"/>.
  /// Can be negative.
  /// </remarks>
  public float Speed {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Pool.Speeds[Id]; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set { Pool.Speeds[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the speed of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in radians per second. Can be negative.
  /// </remarks>
  public float AngularSpeed {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Pool.AngularSpeeds[Id]; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set { Pool.AngularSpeeds[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the Danmaku's rendering color.
  /// </summary>
  public Color Color {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return Pool.Colors[Id]; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set { Pool.Colors[Id] = value; }
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

  /// <summary>
  /// Replaces the current
  /// </summary>
  /// <remarks>
  /// This destroys the original bullet, rendering it unusable.
  /// </remarks>
  /// <param name="set">the prototype </param>
  /// <returns></returns>
  public Danmaku Replace(DanmakuSet set) {
    var danmaku = set.Fire(new DanmakuConfig {
      Position = Position,
      Rotation = Rotation,
      Speed = Speed,
      AngularSpeed = AngularSpeed,
      Color = Color
    });
    Destroy();
    return danmaku;
  }

  /// <summary>
  /// Gets the Danmaku's current state.
  /// </summary>
  /// <returns></returns>
  public DanmakuState GetState() {
    return new DanmakuState {
      Position = Position,
      Rotation = Rotation,
      Speed = Speed,
      AngularSpeed = AngularSpeed,
      Color = Color
    };
  }

  /// <summary>
  /// Applies a state to the Danmaku.
  /// </summary>
  /// <param name="state">the state to be applied.</param>
  public void ApplyState(DanmakuState state) {
    Position = state.Position;
    Rotation = state.Rotation;
    Speed = state.Speed;
    AngularSpeed = state.AngularSpeed;
    Color = state.Color;
  }

  /// <summary>
  /// Convert a Danmaku rotation into a unit vector.
  /// </summary>
  /// <param name="rotation">the rotation of the Danmaku.</param>
  /// <returns>the unit vector representing the way the bullet is facing.</returns>
  public static Vector2 GetDirection(float rotation) {
    return new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
  }

  public static bool operator ==(Danmaku lhs, Danmaku rhs) {
    return lhs.Id == rhs.Id && lhs.Pool == rhs.Pool;
  }

  public static bool operator !=(Danmaku lhs, Danmaku rhs) {
    return lhs.Id != rhs.Id || lhs.Pool != rhs.Pool;
  }

  public override bool Equals(object obj) {
    if (!(obj is Danmaku)) return false;
    return ((Danmaku)obj) == this;
  }

  public override int GetHashCode() => 31 * Pool.GetHashCode() + Id;

}

}