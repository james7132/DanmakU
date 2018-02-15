using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteDanmakuRenderer : DanmakuRenderer {

  public Sprite Sprite;

  Mesh spriteMesh;
  MaterialPropertyBlock propertyBlock;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    spriteMesh = new Mesh();
    if (Sprite == null) return;

    var spriteVerts = Sprite.vertices;
    spriteMesh.vertices = Sprite.vertices.Select(v => (Vector3)v).ToArray();
    spriteMesh.triangles = Sprite.triangles.Select(t => (int)t).ToArray();
    spriteMesh.uv = Sprite.uv;

    propertyBlock = new MaterialPropertyBlock();
    propertyBlock.SetTexture("_MainTex", Sprite.texture);
    Debug.Log(spriteMesh.vertices.Length);
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() => Destroy(spriteMesh);

  protected override Mesh GetMesh() => spriteMesh;
  protected override MaterialPropertyBlock GetMaterialPropertyBlock() => propertyBlock;

}
