using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DanmakU {

[ExecuteInEditMode]
public abstract class DanmakuRenderer : MonoBehaviour {

  const int kRenderBatchSize = 1023;

  static Vector4[] colorCache = new Vector4[kRenderBatchSize];
  static Matrix4x4[] transformCache = new Matrix4x4[kRenderBatchSize];

  public Color Color = Color.white;
  public Material material;
  public DanmakuPool Pool;

  Material renderMaterial;
  Material oldMaterial;
  MaterialPropertyBlock propertyBlock;

  protected abstract Mesh GetMesh();
  protected virtual void PrepareMaterial(Material material) {}

  Material GetRenderMaterial() {
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying) {
      renderMaterial = material;
      return renderMaterial;
    }
#endif
    if (material == null) {
      renderMaterial = null;
    } else if (renderMaterial == null) {
      renderMaterial = Instantiate(material);
    }
    return renderMaterial;
  }

  MaterialPropertyBlock GetMaterialPropertyBlock() {
    if (propertyBlock == null) {
      propertyBlock = new MaterialPropertyBlock();
    }
    return propertyBlock;
  }

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  protected virtual void OnEnable() {
    Camera.onPreCull -= Render;
    Camera.onPreCull += Render;
  }

  /// <summary>
  /// This function is called when the behaviour becomes disabled or inactive.
  /// </summary>
  protected virtual void OnDisable() => Camera.onPreCull -= Render;

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() {
    OnDisable();
    if (renderMaterial != null && renderMaterial != material) {
      DestroyImmediate(renderMaterial);
    }
  }

  void Render(Camera camera) {
    var mesh = GetMesh();
    var propertyBlock = GetMaterialPropertyBlock();
    PrepareMaterial(GetRenderMaterial());
    if (mesh == null || renderMaterial == null) return;
    material.enableInstancing = true;
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying && this != null) {
      colorCache[0] = Color;
      transformCache[0] = transform.worldToLocalMatrix;
      propertyBlock.SetVectorArray("_Color", colorCache);
      Graphics.DrawMeshInstanced(mesh, 0, renderMaterial, transformCache, 
          count: 1,
          properties: propertyBlock,
          castShadows: ShadowCastingMode.Off,
          receiveShadows: false,
          layer: gameObject.layer);
      return;
    }
#endif
    if (Pool == null || Pool.ActiveCount <= 0) return;

    int i = 0;
    int batchEnd = Mathf.Min(Pool.ActiveCount, kRenderBatchSize);
    while (i < Pool.ActiveCount) {
      var count = Mathf.Min(kRenderBatchSize, Pool.ActiveCount - i);
      if (count == kRenderBatchSize) {
        new NativeSlice<Vector4>(Pool.Colors, i, count).CopyTo(colorCache);
        new NativeSlice<Matrix4x4>(Pool.Transforms, i, count).CopyTo(transformCache);
      } else {
        // This is only because CopyTo requires the array and slice to be the same size.
        for (var j = 0; j < count; j++) {
          colorCache[j] = Pool.Colors[i + j];
          transformCache[j] = Pool.Transforms[i + j];
        }
      }
      propertyBlock.SetVectorArray("_Color", colorCache);
      Graphics.DrawMeshInstanced(mesh, 0, renderMaterial, transformCache,
          count: count,
          properties: propertyBlock,
          castShadows: ShadowCastingMode.Off,
          receiveShadows: false,
          layer: gameObject.layer);
      batchEnd = Mathf.Min(Pool.ActiveCount, batchEnd + kRenderBatchSize);
      i += count;
    }
  }

}

}