using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

[ExecuteInEditMode]
public abstract class DanmakuRenderer : MonoBehaviour {

  const int kRenderBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kRenderBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kRenderBatchSize];
  static Vector4[] flipCache = new Vector4[kRenderBatchSize];

  public Material material;
  public DanmakuPool Pool;

  protected abstract Mesh GetMesh();
  protected abstract MaterialPropertyBlock GetMaterialPropertyBlock();
  protected virtual bool UsesSpriteRendering => false;

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  protected virtual void OnEnable() {
    Camera.onPreCull -= RenderForCamera;
    Camera.onPreCull += RenderForCamera;
  }

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  protected virtual void OnDisable() => Camera.onPreCull -= RenderForCamera;

  void RenderForCamera(Camera camera) => Render();

  void Render() {
    var mesh = GetMesh();
    var propertyBlock = GetMaterialPropertyBlock();
    if (mesh == null) return;
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying) {
      colorCache[0] = Color.white;
      if (UsesSpriteRendering) {
        propertyBlock.SetVectorArray("unity_SpriteRendererColorArray", colorCache);
        propertyBlock.SetVectorArray("unity_SpriteRendererFlipArray", flipCache);
      } else {
        propertyBlock.SetVectorArray("_Color", colorCache);
      }
      Graphics.DrawMesh(mesh, transform.position, transform.rotation, material, 
                        gameObject.layer, null, 0, propertyBlock);
      return;
    }
#endif
    if (Pool == null || Pool.ActiveCount <= 0) return;

    int i = 0;
    int batchEnd = Mathf.Min(Pool.ActiveCount, kRenderBatchSize);
    while (i < Pool.ActiveCount) {
      for (; i < batchEnd; i++) {
        colorCache[i % kRenderBatchSize] = Pool.Colors[i];
        transformCache[i % kRenderBatchSize] = Pool.Transforms[i];
      }
      int count = i % kRenderBatchSize;
      if (count == 0) count = kRenderBatchSize;
      if (UsesSpriteRendering) {
        propertyBlock.SetVectorArray("unity_SpriteRendererColorArray", colorCache);
        propertyBlock.SetVectorArray("unity_SpriteRendererFlipArray", flipCache);
      } else {
        propertyBlock.SetVectorArray("_Color", colorCache);
      }
      Graphics.DrawMeshInstanced(mesh, 0, material, transformCache,
          count: count,
          properties: propertyBlock,
          castShadows: ShadowCastingMode.Off,
          receiveShadows: false,
          layer: gameObject.layer);
      batchEnd = Mathf.Min(Pool.ActiveCount, batchEnd + kRenderBatchSize);
    }
  }

}

}