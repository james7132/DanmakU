using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Object = UnityEngine.Object;

namespace DanmakU {

internal class DanmakuRenderer : IDisposable {

  const int kBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kBatchSize];
  static int ColorPropertyId = Shader.PropertyToID("_Color");

  public Color Color { get; set; } = Color.white;

  public readonly Mesh Mesh;

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

  public DanmakuRenderer(Material material, Mesh mesh) {
    propertyBlock = new MaterialPropertyBlock();
    Material = material;
    Mesh = mesh;
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

      var srcColors = (Vector4*)pool.Colors.GetUnsafeReadOnlyPtr();
      var srcTransforms = (Matrix4x4*)pool.Transforms.GetUnsafeReadOnlyPtr();

      int poolIndex = 0;
      while (poolIndex < pool.ActiveCount) {
        var count = Math.Min(kBatchSize - batchIndex, pool.ActiveCount - poolIndex);
        fixed (Vector4* colors = colorCache) {
          UnsafeUtility.MemCpy(colors + batchIndex, srcColors + poolIndex, sizeof(Vector4) * count);
        }
        fixed (Matrix4x4* transforms = transformCache) {
          UnsafeUtility.MemCpy(transforms + batchIndex, srcTransforms + poolIndex, sizeof(Matrix4x4) * count);
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
