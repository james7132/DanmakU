using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

public class MeshDanmakuRenderer : DanmakuRenderer {

  public Mesh Mesh;
  MaterialPropertyBlock propertyBlock;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    propertyBlock = new MaterialPropertyBlock();
  }

  protected override Mesh GetMesh() => Mesh;
  protected override MaterialPropertyBlock GetMaterialPropertyBlock() {
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying && propertyBlock == null) {
      propertyBlock = new MaterialPropertyBlock();
    }
#endif
    return propertyBlock;
  } 

}

}