
namespace DanmakU {

public class DanmakuRenderer {

  const int kRenderBatchSize = 1023;

  Vector4[] colorCache = new Vector4[kRenderBatchSize];
  Matrix4x4[] transformCache = new Matrix4x4[kRenderBatchSize]
  MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

  public Mesh mesh;
  public Material material;
  public DanmakuPool Pool;

  public void Render() {
    if (pool?.ActiveCount <= 0) return;

    int i = 0;
    int batchEnd = Mathf.Min(pool.ActiveCount, kRenderBatchSize);
    while (i < pool.ActiveCount) {
      for (; i < batchEnd; i++) {
        colorCache[i % kRenderBatchSize] = pool.Colors[i];
        transformCache[i % kRenderBatchSize] = pool.Transforms[i];
      }
      int count = i % kRenderBatchSize;
      if (count == 0) count = kRenderBatchSize;
      propertyBlock.SetVectorArray("_Color", colorCache);
      Graphics.DrawMeshInstanced(mesh, 0, material, transformCache,
          count: count,
          properties: propertyBlock,
          castShadows: ShadowCastingMode.Off,
          layer: gameObject.layer)
      batchEnd = Mathf.Min(pool.ActiveCount, batchEnd + kRenderBatchSize);
    }
  }

}

}
