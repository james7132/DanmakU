// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Vexe.Runtime.Types;
using Vexe.Runtime.Extensions;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hourai.DanmakU {

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class DanmakuType : MonoBehaviour, IEnumerable<Danmaku>
    {
        internal static List<DanmakuType> activeTypes;

        internal DanmakuPrefab prefab;
        public DanmakuPrefab Prefab
        {
            get { return prefab; }
        }

        // Pool variables
        internal Danmaku[] all;
        internal int _totalCount;
        internal int _spawnCount;
        internal int _activeCount;
        internal int _releasedCount;

        // Particle System variables
        private ParticleSystem danmakuSystem;
        private ParticleSystemRenderer dRenderer;
        private ParticleSystem.Particle[] particles;

        // Rendedring variables
        internal Mesh mesh;
        internal Color color;
        internal float size;
        internal Material material;
        internal int sortingLayer;
        internal int sortingOrder;
        
        private int updateIndex;
        private ParticleSystem.Particle particle;

        Action PostUpdate;

        #region Public Properties
        public int IotalCount {
            get { return _totalCount; }
        }

        public int ActiveCount {
            get { return _activeCount; }
        }

        public int SpawnCount {
            get { return _spawnCount; }
            set { _spawnCount = value; }
        }

        public int SortingLayer {
            get { return sortingLayer; }
            set {
                sortingLayer = value;
                dRenderer.sortingLayerID = value;
            }
        }

        public int SortingOrder {
            get { return sortingOrder; }
            set {
                sortingOrder = value;
                dRenderer.sortingOrder = value;
            }
        }

        public Mesh Mesh {
            get { return mesh; }
        }

        public Color Color {
            get { return color; }
            set {
                color = value;
                if(danmakuSystem)
                    danmakuSystem.startColor = value;
            }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public Material Material {
            get { return material; }
        }
        #endregion

        #region Initialization
        void Awake()
        {
            if (activeTypes == null)
                activeTypes = new List<DanmakuType>();
            activeTypes.Add(this);

            DontDestroyOnLoad(this);

            danmakuSystem = GetComponent<ParticleSystem>();
            dRenderer = GetComponent<ParticleSystemRenderer>();

            Transform root = transform.root;
            transform.parent = null;
            transform.localPosition = Vector3.zero;

            danmakuSystem.simulationSpace = ParticleSystemSimulationSpace.World;
            danmakuSystem.startSize = 1;
            danmakuSystem.startLifetime = Mathf.Infinity;
            danmakuSystem.maxParticles = int.MaxValue;
            danmakuSystem.gravityModifier = 0f;
            danmakuSystem.startSpeed = 0;
            danmakuSystem.enableEmission = false;

            particles = new ParticleSystem.Particle[danmakuSystem.particleCount];
            dRenderer.renderMode = ParticleSystemRenderMode.Mesh;

            dRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            dRenderer.receiveShadows = false;
            dRenderer.shadowCastingMode = ShadowCastingMode.Off;
            dRenderer.useLightProbes = false;

            gameObject.hideFlags = HideFlags.HideInHierarchy;

            // Destroy rest of hiearchy
            if(root != transform)
                Destroy(root.gameObject);
            gameObject.Children().Destroy();

            Component[] whitelist = new Component[] { this, danmakuSystem, dRenderer, transform };
            foreach (Component comp in GetComponents<Component>())
                if (!whitelist.Contains(comp))
                    Destroy(comp);
        }

        // MUST be called from a DanmakuPrefab
        internal void Init(Renderer renderer, DanmakuPrefab prefab)
        {
            if (!prefab)
                throw new ArgumentNullException("prefab");

            this.prefab = prefab;

            size = prefab.cachedScale.Max();
            _totalCount = 0;
            _activeCount = 0;
            _spawnCount = prefab.spawnCount;
            if (_spawnCount <= 0)
                _spawnCount = 1;
            all = new Danmaku[prefab.initialCount * 2];
            Spawn(prefab.initialCount);

            material = renderer.sharedMaterial;
            sortingLayer = renderer.sortingLayerID;
            sortingOrder = renderer.sortingOrder;
            mesh = new Mesh();

            OnInit(renderer);

            Matrix4x4 rot = Matrix4x4.TRS(Vector3.zero,
                                            Quaternion.Euler(0f, 0f, prefab.rotationOffset),
                                            prefab.transform.localScale);
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] = rot * verts[i];
            mesh.vertices = verts;

            dRenderer.mesh = mesh;

            danmakuSystem.startColor = color;
            dRenderer.sharedMaterial = material;
            dRenderer.sortingLayerID = sortingLayer;
            dRenderer.sortingOrder = sortingOrder;
        }

        internal abstract void OnInit(Renderer renderer);
        #endregion

        void Update()
        {
            if (_activeCount <= 0)
                return;

            int particleCount = danmakuSystem.particleCount;
            if (_activeCount > particleCount)
                danmakuSystem.Emit(_activeCount - particleCount);
            if (_activeCount > particles.Length)
                Array.Resize(ref particles, Mathf.NextPowerOfTwo(_activeCount + 1));

            danmakuSystem.GetParticles(particles);

            // For some reason, new ParticleSystem.Particle breaks bullet systems
            // but accessing the first element like this and reusing it for each is OK.
            particle = particles[0];
            particle.axisOfRotation = Vector3.forward;
            updateIndex = 0;
            if (prefab.fixedAngle) {
                while (updateIndex < _activeCount) {
                    Danmaku danmaku = all[updateIndex];
                    danmaku.Update();
                    particle.position = danmaku.position;
                    particle.size = danmaku.Size;
                    particle.color = danmaku.Color;
                    particles[updateIndex++] = particle;
                }
            }
            else
            {
                while (updateIndex < _activeCount)
                {
                    Danmaku danmaku = all[updateIndex];
                    danmaku.Update();
                    particle.position = danmaku.position;
                    particle.rotation = danmaku.rotation;
                    particle.size = danmaku.Size;
                    particle.color = danmaku.Color;
                    particles[updateIndex++] = particle;
                }
            }

            updateIndex = -1;

            danmakuSystem.SetParticles(particles, _activeCount);

            if(PostUpdate != null) {
                PostUpdate();
                PostUpdate = null;
            }
        }

        protected virtual void OnDestroy() {
            activeTypes.Remove(this);
            Destroy(mesh);
        }

        void OnLevelWasLoaded(int level) {
            _activeCount = 0;
            _releasedCount = 0;
        }

        public void DestroyAll()
        {
            PostUpdate += delegate()
            {
                for (int i = 0; i < _activeCount; i++)
                    all[i].DestroyImpl();
                _activeCount = 0;
                _releasedCount = 0;
            };
        }

        void Spawn(int count)
        {
            int endCount = _totalCount + count;
            if (all.Length < endCount)
                Array.Resize(ref all, Mathf.NextPowerOfTwo(endCount + 1));
            for (int i = _totalCount; i < all.Length; i++)
                all[i] = new Danmaku(i, this);
            _totalCount = endCount;
        }

        #region Pooling Functions
        /// <summary>
        /// Retrievese a inactive Danmaku from the pool.
        /// The Danmaku wil not be rendered or be updated until Activate is called on it.
        /// </summary>
        /// <returns></returns>
        public Danmaku Get()
        {
            if (_releasedCount >= _totalCount)
                Spawn(_spawnCount);
            Danmaku inactive = all[_releasedCount];
            inactive.Color = color;
            inactive.prefab = prefab;
            inactive.Size = size;
            inactive.Layer = prefab.cachedLayer;
            inactive.ColliderSize = prefab.colliderSize;
            inactive.colliderOffset = prefab.colliderOffset;
            _releasedCount++;
            return inactive;
        }

        internal void Activate(Danmaku danmaku)
        {
            Danmaku active = all[_activeCount];

            active.PoolIndex = danmaku.PoolIndex;
            danmaku.PoolIndex = _activeCount;

            all[_activeCount] = danmaku;
            all[active.PoolIndex] = active;

            _activeCount++;
        }

        internal void Return(Danmaku danmaku)
        {
            int index = danmaku.PoolIndex;

            _activeCount--;
            _releasedCount--;

            Danmaku active = all[_activeCount];     // Last active Danmaku
            if(_activeCount == _releasedCount)
            {
                all[danmaku.PoolIndex = _activeCount] = danmaku;
                all[active.PoolIndex = index] = active;
            }
            else
            {
                Danmaku released = all[_releasedCount]; // Last released Danmaku
                all[danmaku.PoolIndex = _releasedCount] = danmaku;
                all[released.PoolIndex = _activeCount] = released;
                all[active.PoolIndex = index] = active;
            }
            if (index <= updateIndex)
            {
                active.Update();
                particle.position = active.position;
                particle.rotation = active.rotation;
                particle.size = active.Size;
                particle.color = active.Color;
                particles[index] = particle;
            }
        }
        #endregion

        #region IEnumerable Implementation
        public IEnumerator<Danmaku> GetEnumerator()
        {
            for (var i = 0; i < _activeCount; i++)
                yield return all[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    internal sealed class SpriteDanmaku : DanmakuType {

        internal override void OnInit(Renderer renderer)
        {
            SpriteRenderer sr = renderer as SpriteRenderer;
            if (sr == null)
                return;
            //cachedSprite = renderer.sprite;
            color = sr.color;

            Sprite sprite = sr.sprite;
            material = new Material(material);
            material.mainTexture = sprite.texture;

            if (sprite == null)
                Destroy(mesh);
            else
            {
                var verts = sprite.vertices;
                var tris = sprite.triangles;

                var vertexes = new Vector3[verts.Length];
                int[] triangles = new int[tris.Length];

                for (int i = 0; i < verts.Length; i++)
                    vertexes[i] = verts[i];

                for (int i = 0; i < tris.Length; i++)
                    triangles[i] = tris[i];

                mesh.vertices = vertexes;
                mesh.uv = sprite.uv;
                mesh.triangles = triangles;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(material);
        }
    }

    internal sealed class MeshDanmaku : DanmakuType
    {
        internal override void OnInit(Renderer renderer)
        {
            var mr = renderer as MeshRenderer;
            if (mr == null)
                return;
            color = Color.white;

            MeshFilter filter = mr.GetComponent<MeshFilter>();
            if (filter == null)
            {
                Debug.LogError("Danmaku Prefab (" + name +
                               ") is trying to use a MeshRenderer as a base, but no MeshFilter is found. Please add one.");
            }
            else
            {
                Mesh fMesh = filter.sharedMesh;
                mesh.vertices = fMesh.vertices;
                mesh.uv = fMesh.uv;
                mesh.triangles = fMesh.triangles;
                mesh.colors = fMesh.colors;
                mesh.normals = fMesh.normals;
                mesh.tangents = fMesh.tangents;
            }
        }
    }

    /// <summary>
    /// A container behavior used on prefabs to define how a bullet looks or behaves
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Hourai.DanmakU/Danmaku Prefab")]
    public sealed class DanmakuPrefab : BetterBehaviour, IEnumerable<FireData> {

        public enum RenderingType {

            Sprite,
            Mesh

        }

        void Awake()
        {
            Destroy(gameObject);
        }

        void Init()
        {
            cachedScale = transform.localScale;
            cachedLayer = gameObject.layer;

            var singleRenderer = GetComponent<Renderer>();
            var spriteRenderer = singleRenderer as SpriteRenderer;
            var meshRenderer = singleRenderer as MeshRenderer;
            if (spriteRenderer == null && meshRenderer == null)
            {
                Debug.LogError("Danmaku Prefab (" + name +
                               ") has neither SpriteRenderer or MeshRenderer. Attach one or the other to it.");
                Destroy(this);
                return;
            }

            ParticleSystem system = null;
            if (danmakuSystemPrefab != null)
                system = Instantiate(danmakuSystemPrefab);
            if (system == null)
            {
                GameObject runtimeObject =
                    Instantiate(Resources.Load("Danmaku Particle System")) as
                    GameObject ?? new GameObject(name);
                system = runtimeObject.GetOrAddComponent<ParticleSystem>();
            }
            if(meshRenderer)
                type = system.gameObject.AddComponent<MeshDanmaku>();
            else
                type = system.gameObject.AddComponent<SpriteDanmaku>();
            type.Init(singleRenderer, this);
            type.gameObject.SetActive(true);
        }

        public Danmaku Get()
        {
            if (!type)
                Init();
            return type.Get();
        }

        public void Match(Danmaku danmaku)
        {
            throw new System.Exception();
            if (danmaku.prefab != this)
            {
                danmaku.prefab = this;

                // TODO: Figure out how to transfer Danmaku from pool to pool here

                danmaku.colliderType = collisionType;
                switch (collisionType)
                {
                    default:
                        danmaku.colliderSize = Vector2.zero;
                        break;
                    case Danmaku.ColliderType.Circle:
                        danmaku.colliderSize = colliderSize * Mathf.Max(cachedScale.x, cachedScale.y);
                        break;
                    case Danmaku.ColliderType.Line:
                        danmaku.colliderSize = colliderSize;
                        break;
                }
                danmaku.sizeSquared = colliderSize.y * colliderSize.y;
                danmaku.colliderOffset = Vector2.Scale(cachedScale, colliderOffset);
            }

            danmaku.Color = type.Color;
            danmaku.Size = 1f;
            danmaku.Layer = cachedLayer;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Matrix4x4 oldGizmoMatrix = Gizmos.matrix;
            Matrix4x4 oldHandlesMatrix = Handles.matrix;
            Color oldGizmosColor = Gizmos.color;
            Color oldHandlesColor = Handles.color;
            Matrix4x4 hitboxMatrix = Matrix4x4.TRS((Vector2) transform.position,
                                                   Quaternion.Euler(0f, 0f, transform.eulerAngles.z),
                                                   transform.lossyScale);
            Gizmos.matrix = hitboxMatrix;
            Handles.matrix = hitboxMatrix;
            Handles.color = Color.green;
            Gizmos.color = Color.green;
            switch (collisionType) {
                case Danmaku.ColliderType.Point:
                    //Handles.PositionHandle(Vector3.zero, Quaternion.identity);
                    break;
                case Danmaku.ColliderType.Circle:
                    hitboxMatrix = Matrix4x4.TRS((Vector2) transform.position,
                                                 Quaternion.Euler(0f, 0f, transform.Rotation2D()),
                                                 transform.lossyScale.Max()*Vector3.one);
                    Gizmos.matrix = hitboxMatrix;
                    Handles.matrix = hitboxMatrix;
                    Handles.DrawWireDisc(colliderOffset,
                                         Vector3.forward,
                                         colliderSize.Max());
                    break;
                case Danmaku.ColliderType.Box:
                    Gizmos.DrawWireCube(colliderOffset, colliderSize);
                    break;
                case Danmaku.ColliderType.Line:
                    Handles.DrawLine(colliderOffset,
                                     colliderOffset + new Vector2(0f, colliderSize.x));
                    break;
            }
            Gizmos.matrix = oldGizmoMatrix;
            Handles.matrix = oldHandlesMatrix;
            Gizmos.color = oldGizmosColor;
            Handles.color = oldHandlesColor;
        }
        #endif

        private void OnDestroy() {
            if (type)
                Destroy(type.gameObject);
        }

        public static implicit operator FireData(DanmakuPrefab prefab) {
            return prefab ? new FireData() {Prefab = prefab} : null;
        }

        #region Prefab Fields

        [SerializeField]
        private ParticleSystem danmakuSystemPrefab;

        [Serialize, Default(Enum = 0)]
        internal Danmaku.ColliderType collisionType;

        [Serialize, Default(1f, 1f)]
        internal Vector2 colliderSize;

        [Serialize, Default(0f, 0f)]
        internal Vector2 colliderOffset;

        [Serialize, fSlider(-180f, 180f)]
        internal float rotationOffset;

#if UNITY_EDITOR
        public override void Reset() {
            base.Reset();
            danmakuSystemPrefab =
                (Resources.Load("Danmaku Particle System") as GameObject)
                    .GetComponent<ParticleSystem>();
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (sprite == null && mesh == null) {
                if (EditorUtility.DisplayDialog("Choose a Renderer",
                                                "Danmaku Prefab requires one and only one renderer, please chose one",
                                                "Sprite Renderer",
                                                "Mesh Renderer"))
                    gameObject.AddComponent<SpriteRenderer>();
                else {
                    gameObject.AddComponent<MeshFilter>();
                    gameObject.AddComponent<MeshRenderer>();
                }
            }
        }
#endif

        [Serialize]
        internal bool fixedAngle;

        [Serialize, Default(1000)]
        internal int initialCount;

        [Serialize, Default(100)]
        internal int spawnCount;

        internal Vector3 cachedScale;
        internal int cachedLayer;

        #endregion

        #region Runtime fields

        internal DanmakuType type;

        #endregion

        #region Accessor Properties

        public bool FixedAngle {
            get { return fixedAngle; }
        }

        /// <summary>
        /// Gets the radius of the instance's collider
        /// </summary>
        /// <value>the radius of the collider.</value>
        public Vector2 ColliderSize {
            get { return colliderSize; }
        }

        /// <summary>
        /// Gets the offset of the instance's collider
        /// </summary>
        /// <value>the offset of the collider.</value>
        public Vector2 ColliderOffset {
            get { return colliderOffset; }
        }

        public DanmakuType Renderer
        {
            get { return type; }
        }

        #endregion

        public IEnumerable Infinite() {
            return new FireData() { Prefab = this }.Infinite();
        } 

        public IEnumerator<FireData> GetEnumerator() {
            yield return new FireData() { Prefab = this };
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}