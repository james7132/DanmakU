using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanmakU {

internal sealed class SpriteDanmakuRenderer : DanmakuRenderer {

  static int MainTexPropertyId = Shader.PropertyToID("_MainTex");

  Sprite sprite;
  public Sprite Sprite {
    get { return sprite; }
    set {
      sprite = value;
      Mesh.vertices = Sprite.vertices.Select(v => (Vector3)v).ToArray();
      Mesh.triangles = Sprite.triangles.Select(t => (int)t).ToArray();
      Mesh.uv = Sprite.uv;
      if (renderMaterial != null) {
        PrepareMaterial(renderMaterial);
      }
    }
  }

  public SpriteDanmakuRenderer(Material material, Sprite sprite) : base(material, new Mesh()) {
    Sprite = sprite;
  }

  public override void Dispose() {
    if (Mesh != null) Object.DestroyImmediate(Mesh);
  }

  protected override void PrepareMaterial(Material material) {
    if (Sprite == null) return;
    material.SetTexture(MainTexPropertyId, Sprite.texture);
  }

}

}