using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

public sealed class MeshDanmakuRenderer : DanmakuRenderer {

  public Mesh Mesh;

  protected override Mesh GetMesh() => Mesh;

}

}