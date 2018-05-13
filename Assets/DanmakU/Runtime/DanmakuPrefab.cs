using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

internal struct DanmakuRendererConfig {
  public Mesh Mesh;
  public Sprite Sprite;
  public Material Material;
}

/// <summary>
/// A prototype base for <see cref="DanmakU.Danmaku"/>. A metaphorical "prefab" for creating
/// instances of <see cref="DanmakU.DanmakuSet"/>.
/// </summary>
/// <remarks>
/// <para>
/// Implemented as a MonoBehaviour. This can be attached to GameObject prefabs denote it is a base for
/// making. Any attached Components that inherit from <see cref="DanmakU.IDanmakuModifier"/> will be added as
/// modifiers to any <see cref="DanmakU.DanmakuSet"/> created from the prefab.
/// </para>
/// </remarks>
/// <example>
/// In scripts, the type DanmakuPrefab is mostly used as a serializable reference to a base for creating 
/// bullet types. This allows designers to change out the look and behaviour of bullet patterns without needing 
/// to modify the code.
/// <code>
/// public class CustomDanmakuPattern : DanmakuBehaviour {
///   // Serialize a reference to a DanmakuPrefab.
///   public DanmakuPrefab Prefab;
///   IFireable fireable;
/// 
///   void Start() {
///     // Create a DanmakuSet from the prefab.
///     fireable = CreateSet(Prefab);
///   }
///   
///   void Update() {
///     // Fire the bullets!
///     fireable.Fire();
///   }
/// }
/// </code>
/// </example>
[AddComponentMenu("DanmakU/Danmaku Prefab")]
public class DanmakuPrefab : MonoBehaviour {

  enum RendererType { Sprite, Mesh }

  [Header("Rendering")]
  [SerializeField] RendererType Type;
  [SerializeField] internal Material Material;
  internal Color Color = Color.red;
  [SerializeField] internal Mesh Mesh;
  [SerializeField] internal Sprite Sprite;
  [Header("Collision")]
  [SerializeField] internal float ColliderRadius = 1f;
  [SerializeField] internal Vector2 ColliderOffset;
  [Header("Pooling")]
  [SerializeField] internal int DefaultPoolSize = 1000;

  IDanmakuModifier[] Modifiers;

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    var center = transform.TransformPoint(ColliderOffset);
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(center, ColliderRadius);
  }

  internal IDanmakuModifier[] GetModifiers() {
    return Modifiers ?? (Modifiers = GetComponents<IDanmakuModifier>());
  }

  internal DanmakuRendererConfig GetRendererConfig() {
    return new DanmakuRendererConfig {
      Material = Material,
      Sprite = Sprite,
      Mesh = Mesh,
    };
  }

}

}