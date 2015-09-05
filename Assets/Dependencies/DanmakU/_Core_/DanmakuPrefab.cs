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

    [RequireComponent(typeof(ParticleSystem))]
    public abstract class DanmakuType : MonoBehaviour, IEnumerable<Danmaku>
    {
        private DanmakuPrefab prefab;

        public List<Danmaku> toDestroy;

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
        internal Material material;
        internal int sortingLayer;
        internal int sortingOrder;

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
                danmakuSystem.startColor = value;
            }
        }

        public Material Material {
            get { return material; }
        }
        #endregion

        #region Initialization
        void Awake()
        {
            DontDestroyOnLoad(this);
            danmakuSystem = GetComponent<ParticleSystem>();
            dRenderer = GetComponent<ParticleSystemRenderer>();
            
            Transform root = transform.root;
            transform.parent = null;
            transform.localPosition = Vector3.zero;

            danmakuSystem.simulationSpace = ParticleSystemSimulationSpace.World;
            danmakuSystem.startSize = 1;
            danmakuSystem.startLifetime = float.PositiveInfinity;
            danmakuSystem.gravityModifier = 0f;
            danmakuSystem.startSpeed = 0;
            danmakuSystem.enableEmission = false;

            particles = new ParticleSystem.Particle[danmakuSystem.particleCount];
            dRenderer.renderMode = ParticleSystemRenderMode.Mesh;

            dRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            dRenderer.receiveShadows = false;
            dRenderer.shadowCastingMode = ShadowCastingMode.Off;
            dRenderer.useLightProbes = false;

            //gameObject.hideFlags = HideFlags.HideInHierarchy;

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

            _totalCount = 0;
            _activeCount = 0;
            _spawnCount = prefab.spawnCount;
            if (_spawnCount <= 0)
                _spawnCount = 1;
            all = new Danmaku[prefab.initialCount * 2];
            Spawn(prefab.initialCount);
            toDestroy = new List<Danmaku>();

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
        public bool updating;

        void Update()
        {
            //--------------------------------------------------------------------
            //  Rendering Update
            //--------------------------------------------------------------------
            int particleCount = danmakuSystem.particleCount;
            if (_activeCount > particleCount)
            {
                danmakuSystem.maxParticles = Mathf.NextPowerOfTwo(_activeCount);
                danmakuSystem.Emit(_activeCount - particleCount);
            }
            if (_activeCount > particles.Length)
                Array.Resize(ref particles, Mathf.NextPowerOfTwo(_activeCount + 1));

            if (_activeCount <= 0 && particleCount <= 0)
                return;

            danmakuSystem.GetParticles(particles);
            int staticActiveCount = _activeCount;

            updating = true;

            // For some reason, new ParticleSystem.Particle breaks bullet systems
            // but accessing a 
            ParticleSystem.Particle particle = particles[0];
            particle.axisOfRotation = Vector3.forward;
            int i = 0;
            if (prefab.fixedAngle) {
                while (i < staticActiveCount) {
                    Danmaku danmaku = all[i];
                    danmaku.Update();
                    particle.position = danmaku.position;
                    particle.size = danmaku.Scale;
                    particle.color = danmaku.Color;
                    particles[i] = particle;
                    i++;
                }
            }
            else
            {
                while (i < staticActiveCount)
                {
                    Danmaku danmaku = all[i];
                    danmaku.Update();
                    particle.position = danmaku.position;
                    particle.rotation = danmaku.rotation;
                    particle.size = danmaku.Scale;
                    particle.color = danmaku.Color;
                    particles[i] = particle;
                    i++;
                }
            }
            updating = false;
            // Destroy extra particles
            while (i < particleCount) {
                particles[i].lifetime = 0;
                i++;
            }

            int destroyedCount = toDestroy.Count;
            // Remove destroyed particles
            for (i = 0; i < destroyedCount; i++)
                toDestroy[i].DestroyImpl();
            toDestroy.Clear();

            danmakuSystem.SetParticles(particles, _activeCount);
        }

        protected virtual void OnDestroy() {
            Destroy(mesh);
        }

        public void DestroyAll()
        {
            for (int i = 0; i < _activeCount; i++)
                all[i].Destroy();
        }

        void Spawn(int count)
        {
            int endCount = _totalCount + count;
            if (all.Length < endCount)
                Array.Resize(ref all, Mathf.NextPowerOfTwo(endCount + 1));
            for (int i = _totalCount; i < all.Length; i++)
                all[i] = new Danmaku(i);
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
            inactive.Scale = 1f;
            _releasedCount++;
            return inactive;
        }

        internal void Activate(Danmaku danmaku)
        {
            Danmaku active = all[_activeCount]; // This should be a released but not active bullet
            all[danmaku.PoolIndex] = active;
            all[active.PoolIndex] = danmaku;

            active.PoolIndex = danmaku.PoolIndex;
            danmaku.PoolIndex = _activeCount;
            _activeCount++;
        }

        internal void Return(Danmaku danmaku)
        {
            Danmaku released = all[_releasedCount];
            Danmaku active = all[_activeCount];

            all[_releasedCount] = danmaku;
            all[_activeCount] = released;
            all[danmaku.PoolIndex] = active;

            released.PoolIndex = active.PoolIndex;
            active.PoolIndex = danmaku.PoolIndex;
            danmaku.PoolIndex = _releasedCount;

            _activeCount--;
            _releasedCount--;
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
            Debug.Log(filter);
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
                danmaku.colliderOffset = cachedScale.Hadamard2(colliderOffset);
            }

            danmaku.Color = cachedColor;
            danmaku.Scale = 1f;
            danmaku.Layer = cachedLayer;
        }

        void Initialize()
        {
            cachedScale = transform.localScale;
            cachedTag = gameObject.tag;
            cachedLayer = gameObject.layer;
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
        internal string cachedTag;
        internal int cachedLayer;

        internal Sprite cachedSprite;
        internal Color cachedColor;
        internal Material cachedMaterial;
        internal int cachedSortingLayer;
        internal int cachedSortingOrder;

        #endregion

        #region Runtime fields

        internal DanmakuType type;
        private HashSet<Danmaku> currentDanmaku;
        internal DanmakuGroup currentGroup;

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