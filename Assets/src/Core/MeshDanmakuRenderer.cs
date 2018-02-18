using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

internal sealed class MeshDanmakuRenderer : DanmakuRenderer {

  public MeshDanmakuRenderer(Material material, Mesh mesh) : base(material) {
    Mesh = mesh;
  }

}

}