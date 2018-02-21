using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

internal abstract class DanmakuRenderer : IDisposable {

  const int kBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kBatchSize];
  static int ColorPropertyId = Shader.PropertyToID("_Color");

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

  internal unsafe void Render(List<DanmakuSet> sets, int layer) {
    var mesh = Mesh;
    int batchIndex = 0;
    foreach (var set in sets) {
      var pool = set.Pool;
      if (pool == null || pool.ActiveCount <= 0) return;

      var poolColors = pool.Colors;
      var poolTransforms = pool.Transforms;

      int poolIndex = 0;
      while (poolIndex < pool.ActiveCount) {
        var count = Math.Min(kBatchSize - batchIndex, pool.ActiveCount - poolIndex);
        fixed (void* colors = colorCache) {
          var srcPtr = ((Vector4*)poolColors.GetUnsafeReadOnlyPtr()) + poolIndex;
          UnsafeUtility.MemCpy(colors, srcPtr, sizeof(Vector4) * count);
        }
        fixed (void* transforms = transformCache) {
          var srcPtr = ((Matrix4x4*)poolTransforms.GetUnsafeReadOnlyPtr()) + poolIndex;
          UnsafeUtility.MemCpy(transforms, srcPtr, sizeof(Matrix4x4) * count);
        }
        batchIndex += count;
        poolIndex += count;
        batchIndex %= kBatchSize;
        if (batchIndex == 0) RenderBatch(mesh, kBatchSize, layer);
      }
    }
    if (batchIndex != 0) RenderBatch(mesh, batchIndex, layer);
  }

  void RenderBatch(Mesh mesh, int batchSize, int layer) {
    propertyBlock.SetVectorArray(ColorPropertyId, colorCache);
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
