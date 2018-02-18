using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Assertions;

namespace DanmakU {

public class DanmakuManager : MonoBehaviour {

  public static DanmakuManager Instance;
  static RaycastHit2D[] raycastCache = new RaycastHit2D[256];

  public Bounds Bounds;
  public int DefaultPoolSize = 1000;

  Dictionary<DanmakuRendererConfig, RendererGroup> RendererGroups;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    Instance = this;
    RendererGroups = new Dictionary<DanmakuRendererConfig, RendererGroup>();
    Camera.onPreCull += RenderBullets;
  }

  /// <summary>
  /// This function is called when the MonoBehaviour will be destroyed.
  /// </summary>
  void OnDestroy() {
    Camera.onPreCull -= RenderBullets;
    foreach (var group in RendererGroups.Values) {
      group.Dispose();
    }
  }

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    WaitForUpdateComplete();
    var size = Bounds.extents;
    size.z = float.MaxValue;
    Bounds.extents = size;
    foreach (var group in RendererGroups.Values) {
      foreach (var set in group.Sets) {
        var pool = set.Pool;
        foreach (var danmaku in pool) {
          if (!Bounds.Contains(danmaku.Position)) {
            danmaku.Destroy();
            continue;
          }
          var layerMask = pool.CollisionMasks[danmaku.Id];
          if (layerMask == 0) continue; 
          var oldPosition = pool.OldPositions[danmaku.Id];
          var direction = oldPosition - danmaku.Position;
          var distance = direction.magnitude;
          var hits = Physics2D.CircleCastNonAlloc(oldPosition, pool.ColliderRadius, direction, raycastCache, distance, layerMask);
          if (hits <= 0) continue;
          danmaku.Destroy();
        }
      }
    }
  }

  /// <summary>
  /// LateUpdate is called every frame, if the Behaviour is enabled.
  /// It is called after all Update functions have been called.
  /// </summary>
  void LateUpdate() {
    DanmakuCollider.RebuildSpatialHashes();
    foreach (var group in RendererGroups.Values) {
      group.StartUpdate();
    }
  }

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireCube(Bounds.center, Bounds.size);
  }

  void WaitForUpdateComplete() {
    foreach (var group in RendererGroups.Values) {
      group.CompleteUpdate();
    }
  }

  void RenderBullets(Camera camera) {
    WaitForUpdateComplete();
    foreach (var group in RendererGroups.Values) {
      group.Render(gameObject.layer);
    }
  }

  internal DanmakuSet CreateDanmakuSet(DanmakuRendererConfig config) {
    var pool = new DanmakuPool(DefaultPoolSize);
    var set = new DanmakuSet(pool);
    var group = GetOrCreateRendererGroup(config);
    group.AddSet(set);
    return set;
  }

  RendererGroup GetOrCreateRendererGroup(DanmakuRendererConfig config) {
    RendererGroup group = null;
    if (!RendererGroups.TryGetValue(config, out group)) {
      group = CreateRendererGroup(config);
      RendererGroups[config] = group;
    }
    return group;
  }

  RendererGroup CreateRendererGroup(DanmakuRendererConfig config) {
    DanmakuRenderer renderer;
    if (config.Sprite != null) {
      renderer = new SpriteDanmakuRenderer(config.Material, config.Sprite);
    } else if (config.Mesh != null) {
      renderer = new MeshDanmakuRenderer(config.Material, config.Mesh);
    } else {
      throw new Exception("Attempted to create a DanmakuSet without valid renderer.");
    }
    return new RendererGroup(renderer);
  }

  class RendererGroup : IDisposable {

    public readonly DanmakuRenderer Renderer;
    public List<DanmakuSet> Sets;
    readonly List<JobHandle> UpdateHandles;

    public int Count => Sets.Count;
    
    public RendererGroup(DanmakuRenderer renderer) {
      Assert.IsNotNull(renderer);
      Renderer = renderer;
      Sets = new List<DanmakuSet>();
      UpdateHandles = new List<JobHandle>();
    }

    public void AddSet(DanmakuSet set) {
      Sets.Add(set);
      UpdateHandles.Add(default(JobHandle));
    }

    public bool RemoveSet(DanmakuSet set) {
      int index = Sets.IndexOf(set);
      if (index < 0) return false;
      Sets.RemoveAt(index);
      UpdateHandles.RemoveAt(index);
      return true;
    }

    public void StartUpdate() {
      for (var i = 0; i < Sets.Count; i++) {
        UpdateHandles[i] = Sets[i].Update(default(JobHandle));
      }
    }

    public void CompleteUpdate() {
      foreach (var handle in UpdateHandles) {
        if (!handle.IsCompleted) handle.Complete();
      }
    }

    public void Render(int layer) {
      Renderer.Render(Sets, layer);
    }

    public void Dispose() {
      Sets.Clear();
      UpdateHandles.Clear();
      foreach (var set in Sets) {
        set.Dispose();
      }
    }

  }


}

}