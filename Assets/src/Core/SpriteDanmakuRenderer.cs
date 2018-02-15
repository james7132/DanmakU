using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

public class SpriteDanmakuRenderer : DanmakuRenderer {

  public Sprite Sprite;

  Mesh spriteMesh;
  MaterialPropertyBlock propertyBlock;

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

    propertyBlock = new MaterialPropertyBlock();
    propertyBlock.SetTexture("_MainTex", Sprite.texture);
    Debug.Log(spriteMesh.vertices.Length);
  }

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  protected override void OnDisable() {
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
      spriteMesh = new Mesh();
      spriteMesh.vertices = Sprite.vertices.Select(v => (Vector3)v).ToArray();
      spriteMesh.triangles = Sprite.triangles.Select(t => (int)t).ToArray();
      spriteMesh.uv = Sprite.uv;
    }
#endif
    return spriteMesh;
  } 
  protected override MaterialPropertyBlock GetMaterialPropertyBlock() {
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying && Sprite != null) {
      propertyBlock = new MaterialPropertyBlock();
      propertyBlock.SetTexture("_MainTex", Sprite.texture);
    }
#endif
    return propertyBlock;
  } 
  protected override bool UsesSpriteRendering => true;

}

}