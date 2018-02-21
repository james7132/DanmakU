using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Assertions;

namespace DanmakU {

/// <summary>
/// The singleton manager of all Danmaku-related activities.
/// </summary>
/// <remarks>
/// The singleton is responsible for managing the updating, rendering, collisions, and pooling of
/// all active Danmaku. Destroying the manager will shutdown all of the aformentioned processes and
/// globally destroy all actiive danmaku.
/// </remarks>
[DisallowMultipleComponent]
[AddComponentMenu("DanmakU/Danmaku Manager")]
public class DanmakuManager : MonoBehaviour {

  static RaycastHit2D[] raycastCache = new RaycastHit2D[256];

  /// <summary>
  /// Gets the singleton instance of the manager. Null if there is no active 
  /// manger.
  /// </summary>
  public static DanmakuManager Instance { get; private set; }

  /// <summary>
  /// The global bounds that all bullets are bound to.
  /// </summary>
  public Bounds2D Bounds = new Bounds2D(Vector2.zero, Vector2.one * 200);

  Dictionary<DanmakuRendererConfig, RendererGroup> RendererGroups;
  List<DanmakuRendererConfig> EmptyGroups;
  JobHandle UpdateHandle;

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
    UpdateHandle.Complete();
    if (DanmakuCollider.ColliderCount <= 0) return;
    foreach (var group in RendererGroups.Values) {
      foreach (var set in group.Sets) {
        var pool = set.Pool;
        foreach (var danmaku in pool) {
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
    UpdateHandle = default(JobHandle);
    foreach (var group in RendererGroups.Values) {
      UpdateHandle = JobHandle.CombineDependencies(UpdateHandle, group.StartUpdate());
      JobHandle.ScheduleBatchedJobs();
    }
  }

  /// <summary>
  /// Callback to draw gizmos that are pickable and always drawn.
  /// </summary>
  void OnDrawGizmos() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireCube(Bounds.Center, Bounds.Size);
  }

  /// <summary>
  /// Gets all <see cref="DanmakU.DanmakuSet"/>s associated with a prefab.
  /// </summary>
  /// <param name="prefab">the original prefab.</param>
  /// <returns>an enumeration of all associated sets.</returns>
  public IEnumerable<DanmakuSet> GetAllSets(DanmakuPrefab prefab) {
    var config = prefab.GetRendererConfig();
    RendererGroup group;
    if (RendererGroups.TryGetValue(config, out group)) {
      return group.Sets;
    }
    return Enumerable.Empty<DanmakuSet>();
  }

  void RenderBullets(Camera camera) {
    UpdateHandle.Complete();
    foreach (var group in RendererGroups.Values) {
      group.Render(gameObject.layer);
    }
  }

  internal DanmakuSet CreateDanmakuSet(DanmakuRendererConfig config, DanmakuPool pool) {
    var set = new DanmakuSet(pool);
    var group = GetOrCreateRendererGroup(config);
    group.AddSet(set);
    return set;
  }

  internal void DestroyDanmakuSet(DanmakuSet set) {
    EmptyGroups = EmptyGroups ?? new List<DanmakuRendererConfig>();
    foreach (var kvp in RendererGroups) {
      var group = kvp.Value;
      group.RemoveSet(set);
      if (group.Count <= 0) {
        EmptyGroups.Add(kvp.Key);
      }
    }
    foreach (var key in EmptyGroups) {
      RendererGroups.Remove(key);
    }
    EmptyGroups.Clear();
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

    public int Count => Sets.Count;
    
    public RendererGroup(DanmakuRenderer renderer) {
      Assert.IsNotNull(renderer);
      Renderer = renderer;
      Sets = new List<DanmakuSet>();
    }

    public void AddSet(DanmakuSet set) => Sets.Add(set);

    public bool RemoveSet(DanmakuSet set) => Sets.Remove(set);

    public JobHandle StartUpdate() {
      var update = default(JobHandle);
      for (var i = 0; i < Sets.Count; i++) {
        update = JobHandle.CombineDependencies(update, Sets[i].Update(default(JobHandle)));
      }
      return update;
    }

    public void Render(int layer) => Renderer.Render(Sets, layer);

    public void Dispose() {
      Sets.Clear();
      foreach (var set in Sets) {
        set.Dispose();
      }
    }

  }

}

}