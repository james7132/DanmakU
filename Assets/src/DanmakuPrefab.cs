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

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    var center = transform.TransformPoint(ColliderOffset);
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(center, ColliderRadius);
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