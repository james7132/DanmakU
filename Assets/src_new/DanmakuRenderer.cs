using UnityEngine;
using UnityEngine.Rendering;

public abstract class DanmakuRenderer : MonoBehaviour {

  const int kRenderBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kRenderBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kRenderBatchSize];

  public Material material;
  protected abstract Mesh GetMesh();
  protected abstract MaterialPropertyBlock GetMaterialPropertyBlock();
  public DanmakuPool Pool;

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    if (Pool == null || Pool.ActiveCount <= 0) return;

    int i = 0;
    int batchEnd = Mathf.Min(Pool.ActiveCount, kRenderBatchSize);
    var propertyBlock = GetMaterialPropertyBlock();
    while (i < Pool.ActiveCount) {
      for (; i < batchEnd; i++) {
        colorCache[i % kRenderBatchSize] = Pool.Colors[i];
        transformCache[i % kRenderBatchSize] = Pool.Transforms[i];
      }
      int count = i % kRenderBatchSize;
      if (count == 0) count = kRenderBatchSize;
      propertyBlock.SetVectorArray("_Color", colorCache);
      Graphics.DrawMeshInstanced(GetMesh(), 0, material, transformCache,
          count: count,
          properties: propertyBlock,
          castShadows: ShadowCastingMode.Off,
          receiveShadows: false,
          layer: gameObject.layer);
      batchEnd = Mathf.Min(Pool.ActiveCount, batchEnd + kRenderBatchSize);
    }
  }

}