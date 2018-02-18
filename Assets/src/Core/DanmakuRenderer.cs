using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

public abstract class DanmakuRenderer : IDisposable {

  const int kBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kBatchSize];

  public Color Color { get; set; } = Color.white;

  public virtual Mesh Mesh { get; set; }

  Material sharedMaterial;
  protected Material renderMaterial;
  public Material Material {
    get { return sharedMaterial; }
    set {
      if (renderMaterial != null) Object.DestroyImmediate(renderMaterial);
      sharedMaterial = value;
      if (sharedMaterial != null) {
        renderMaterial = Object.Instantiate(sharedMaterial);
        renderMaterial.enableInstancing = true;
        PrepareMaterial(renderMaterial);
      }
    }
  }

  readonly MaterialPropertyBlock propertyBlock;

  protected DanmakuRenderer(Material material) {
    propertyBlock = new MaterialPropertyBlock();
    Material = material;
  }

  protected virtual void PrepareMaterial(Material material) {}

  public virtual void Dispose() {
    if (renderMaterial == null) return;
    Object.DestroyImmediate(renderMaterial);
  }

  internal void Render(List<DanmakuSet> sets, int layer) {
    var mesh = Mesh;
    int batchIndex = 0;
    foreach (var set in sets) {
      var pool = set.Pool;
      if (pool == null || pool.ActiveCount <= 0) return;

      var poolColors = pool.Colors;
      var poolTransforms = pool.Transforms;

      int poolIndex = 0;
      while (poolIndex < pool.ActiveCount) {
        var count = Mathf.Min(kBatchSize - batchIndex, pool.ActiveCount - poolIndex);
        if (count == kBatchSize) {
          new NativeSlice<Vector4>(poolColors, poolIndex, count).CopyTo(colorCache);
          new NativeSlice<Matrix4x4>(poolTransforms, poolIndex, count).CopyTo(transformCache);
          batchIndex = 0;
          poolIndex += count;
        } else {
          // This is only because CopyTo requires the array and slice to be the same size.
          for (; poolIndex < pool.ActiveCount && batchIndex < kBatchSize; batchIndex++, poolIndex++) {
            colorCache[batchIndex] = poolColors[poolIndex];
            transformCache[batchIndex] = poolTransforms[poolIndex];
          }
        }
        batchIndex %= kBatchSize;
        if (batchIndex == 0) RenderBatch(mesh, kBatchSize, layer);
      }
    }
    if (batchIndex != 0) RenderBatch(mesh, batchIndex, layer);
  }

  void RenderBatch(Mesh mesh, int batchSize, int layer) {
    propertyBlock.SetVectorArray("_Color", colorCache);
    Graphics.DrawMeshInstanced(mesh, 0, renderMaterial, transformCache,
      count: batchSize,
      properties: propertyBlock,
      castShadows: ShadowCastingMode.Off,
      receiveShadows: false,
      layer: layer);
  }

  // internal void Render(Camera camera) {
  //   var mesh = GetMesh();
  //   var propertyBlock = GetMaterialPropertyBlock();
  //   PrepareMaterial(GetRenderMaterial());
  //   if (mesh == null || renderMaterial == null) return;
  // #if UNITY_EDITOR
  //     if (!EditorApplication.isPlaying && this != null) {
  //       colorCache[0] = Color;
  //       transformCache[0] = transform.worldToLocalMatrix;
  //       propertyBlock.SetVectorArray("_Color", colorCache);
  //       Graphics.DrawMeshInstanced(mesh, 0, renderMaterial, transformCache, 
  //           count: 1,
  //           properties: propertyBlock,
  //           castShadows: ShadowCastingMode.Off,
  //           receiveShadows: false,
  //           layer: gameObject.layer);
  //       return;
  //     }
  // #endif
}

}
