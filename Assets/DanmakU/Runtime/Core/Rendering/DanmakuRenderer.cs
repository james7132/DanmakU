using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Object = UnityEngine.Object;

namespace DanmakU {


internal class DanmakuRenderer : IDisposable {

  const int kBatchSize = 4096;

  static Vector4[] colorCache = new Vector4[kBatchSize];
  static Vector2[] positionCache = new Vector2[kBatchSize];
  static float[] rotationCache = new float[kBatchSize];
  static uint[] args = new uint[] {0, 0, 0, 0, 0};

  static int positionPropertyId = Shader.PropertyToID("positionBuffer");
  static int rotationPropertyId = Shader.PropertyToID("rotationBuffer");
  static int colorPropertyId = Shader.PropertyToID("colorBuffer");

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
  readonly ComputeBufferPool.Context StructuredBuffers;
  readonly ComputeBufferPool.Context ArgBuffers;

  public unsafe DanmakuRenderer(Material material, Mesh mesh) {
    propertyBlock = new MaterialPropertyBlock();
    StructuredBuffers = ComputeBufferPool.GetShared().CreateContext();
    ArgBuffers = ComputeBufferPool.GetShared(ComputeBufferType.IndirectArguments).CreateContext();
    Material = material;
    Mesh = mesh;
  }

  protected virtual void PrepareMaterial(Material material) {}

  public virtual void Dispose() {
    if (renderMaterial != null) {
      Object.DestroyImmediate(renderMaterial);
    }
    FlushBuffers();
  }

  public void FlushBuffers() {
    StructuredBuffers.Flush();
    ArgBuffers.Flush();
  }

  public unsafe void Render(List<DanmakuSet> sets, int layer) {
    var mesh = Mesh;
    int batchIndex = 0;

    foreach (var set in sets) {
      var pool = set.Pool;
      if (pool == null || pool.ActiveCount <= 0) continue;

      var srcPositions = (Vector2*)pool.Positions.GetUnsafeReadOnlyPtr();
      var srcRotations = (float*)pool.Rotations.GetUnsafeReadOnlyPtr();
      var srcColors = (Color*)pool.Colors.GetUnsafeReadOnlyPtr();

      int poolIndex = 0;
      while (poolIndex < pool.ActiveCount) {
        var count = Math.Min(kBatchSize - batchIndex, pool.ActiveCount - poolIndex);
        fixed (Vector2* colors = positionCache) {
          UnsafeUtility.MemCpy(colors + batchIndex, srcPositions+ poolIndex, sizeof(Vector2) * count);
        }
        fixed (float* rotations = rotationCache) {
          UnsafeUtility.MemCpy(rotations + batchIndex, srcRotations + poolIndex, sizeof(float) * count);
        }
        fixed (Vector4* colors = colorCache) {
          UnsafeUtility.MemCpy(colors + batchIndex, srcColors + poolIndex, sizeof(Vector4) * count);
        }
        batchIndex += count;
        poolIndex += count;
        batchIndex %= kBatchSize;
        if (batchIndex == 0) RenderBatch(mesh, kBatchSize, layer);
      }
    }
    if (batchIndex != 0) RenderBatch(mesh, batchIndex, layer);
  }

  unsafe void RenderBatch(Mesh mesh, int batchSize, int layer) {
    ComputeBuffer argsBuffer = ArgBuffers.Rent(1, args.Length * sizeof(uint));
    ComputeBuffer positionBuffer = StructuredBuffers.Rent(kBatchSize, sizeof(Vector2));
    ComputeBuffer colorBuffer = StructuredBuffers.Rent(kBatchSize, sizeof(Color));
    ComputeBuffer rotationBuffer = StructuredBuffers.Rent(kBatchSize, sizeof(float));

    colorBuffer.SetData(colorCache, 0, 0, batchSize);
    positionBuffer.SetData(positionCache, 0, 0, batchSize);
    rotationBuffer.SetData(rotationCache, 0, 0, batchSize);

    propertyBlock.SetBuffer(positionPropertyId, positionBuffer);
    propertyBlock.SetBuffer(rotationPropertyId, rotationBuffer);
    propertyBlock.SetBuffer(colorPropertyId, colorBuffer);

    args[0] = mesh.GetIndexCount(0);
    args[1] = (uint)batchSize;
    argsBuffer.SetData(args);

    Graphics.DrawMeshInstancedIndirect(mesh, 0, renderMaterial, 
      bounds: new Bounds(Vector3.zero, Vector3.one * 1000f), 
      bufferWithArgs: argsBuffer,
      argsOffset: 0,
      properties: propertyBlock,
      castShadows: ShadowCastingMode.Off,
      receiveShadows: false,
      layer: layer,
      camera: null);
  }

}

}
