using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

public sealed class SpriteDanmakuRenderer : DanmakuRenderer {

  public Sprite Sprite;

  Mesh spriteMesh;

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  protected override void OnEnable() {
    base.OnEnable();
    spriteMesh = new Mesh();
    if (Sprite == null) return;

    spriteMesh.vertices = Sprite.vertices.Select(v => (Vector3)v).ToArray();
    spriteMesh.triangles = Sprite.triangles.Select(t => (int)t).ToArray();
    spriteMesh.uv = Sprite.uv;
  }

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  protected override void OnDisable() {
    base.OnDisable();
#if UNITY_EDITOR
    if (EditorApplication.isPlaying) {
      Destroy(spriteMesh);
    } else {
#else
    {
#endif
      DestroyImmediate(spriteMesh);
    }
  }

  protected override Mesh GetMesh() {
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying && Sprite != null) {
      if (spriteMesh == null) {
        spriteMesh = new Mesh();
      }
      spriteMesh.vertices = Sprite.vertices.Select(v => (Vector3)v).ToArray();
      spriteMesh.triangles = Sprite.triangles.Select(t => (int)t).ToArray();
      spriteMesh.uv = Sprite.uv;
      return spriteMesh;
    }
#endif
    return spriteMesh;
  } 

  protected override void PrepareMaterial(Material material) {
    if (Sprite == null) return;
    material.SetTexture("_MainTex", Sprite.texture);
  }

}

}