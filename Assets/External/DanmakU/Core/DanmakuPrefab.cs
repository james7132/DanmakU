// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;
using System.Collections.Generic;

namespace DanmakU {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(SpriteRenderer)), AddComponentMenu("Danmaku 2D/Danmaku Prefab")]
	public sealed class DanmakuPrefab : BetterBehaviour {

		#region Prefab Fields
		[Hide, Serialize]
		private CircleCollider2D circleCollider;
		
		[Hide, Serialize]
		private SpriteRenderer spriteRenderer;
		
		[Serialize]
		internal bool fixedAngle;

		[Serialize]
		private IDanmakuController[] extraControllers;
		
		internal Vector3 cachedScale;
		internal string cachedTag;
		internal int cachedLayer;
		
		internal float cachedColliderRadius;
		internal Vector2 cachedColliderOffset;
		
		internal Sprite cachedSprite;
		internal Color cachedColor;
		internal Material cachedMaterial;
		internal int cachedSortingLayer;
		internal int cachedSortingOrder;
		private DanmakuController controllerAggregate;
		#endregion

		#region Accessor Properties
		public bool FixedAngle {
			get {
				return fixedAngle;
			}
		}
		
		public Vector3 Scale {
			get {
				return cachedScale;
			}
		}
		
		public string Tag {
			get {
				return cachedTag;
			}
		}
		
		public int Layer {
			get {
				return cachedLayer;
			}
		}
		
		/// <summary>
		/// Gets the radius of the instance's collider
		/// </summary>
		/// <value>the radius of the collider.</value>
		public float ColliderRadius {
			get {
				return cachedColliderRadius;
			}
		}
		
		/// <summary>
		/// Gets the offset of the instance's collider
		/// </summary>
		/// <value>the offset of the collider.</value>
		public Vector2 ColliderOffset {
			get {
				return cachedColliderOffset;
			}
		}
		
		/// <summary>
		/// Gets the sprite of the instance's SpriteRenderer
		/// </summary>
		/// <value>The sprite to be rendered.</value>
		public Sprite Sprite {
			get {
				return cachedSprite;
			}
		}
		
		/// <summary>
		/// Gets the color of the instance's SpriteRenderer
		/// </summary>
		/// <value>The color to be rendered with.</value>
		public Color Color {
			get {
				return cachedColor;
			}
		}
		
		/// <summary>
		/// Gets the material used by the instance's SpriteRenderer
		/// </summary>
		/// <value>The material to be rendered with.</value>
		public Material Material {
			get {
				return cachedMaterial;
			}
		}
		
		/// <summary>
		/// Gets the sorting layer u
		/// </summary>
		/// <value>The sorting layer to be used when rendering.</value>
		public int SortingLayerID {
			get {
				return cachedSortingLayer;
			}
		}
		
		public int SortingOrder {
			get {
				return cachedSortingOrder;
			}
		}
		#endregion

		private DanmakuPrefab runtime;
		private Mesh spriteMesh;
		private ParticleSystem runtimeSystem;
		private ParticleSystemRenderer runtimeRenderer;
		private ParticleSystem.Particle[] particles;
		private HashSet<Danmaku> currentDanmaku;
		private int danmakuCount;

		[Hide]
		[Serialize]
		private ParticleSystem danmakuParticlePrefab;

		internal DanmakuController ExtraControllers {
			get {
				return controllerAggregate;
			}
		}

		public void Add(Danmaku danmaku) {
			currentDanmaku.Add(danmaku);
		}

		public void Remove(Danmaku danmaku) {
			currentDanmaku.Remove(danmaku);
		}

		void Update() {
			danmakuCount = currentDanmaku.Count;
			int count = runtimeSystem.particleCount;
			if (danmakuCount > count) {
				//Debug.Log("hello");
				runtimeSystem.maxParticles = Mathf.NextPowerOfTwo(danmakuCount);
				runtimeSystem.Emit(danmakuCount - count);
				//Debug.Log(runtimeSystem.particleCount);
				count = danmakuCount;
			}
			if (danmakuCount > particles.Length) {
				particles = new ParticleSystem.Particle[Mathf.NextPowerOfTwo(danmakuCount + 1)];
			}

			runtimeSystem.GetParticles(particles);
			//Debug.Log(count2);
			Vector3 forward = Vector3.forward;
			bool done;
			IEnumerator<Danmaku> enumerator = currentDanmaku.GetEnumerator();
			if (fixedAngle) {
				for(int i = 0; i < danmakuCount; i++) {
					done = enumerator.MoveNext();
					if(done) {
						Danmaku danmaku = enumerator.Current;
						particles[i].position = danmaku.position;
						particles[i].size = danmaku.Scale;
						particles[i].axisOfRotation = forward;
						particles[i].lifetime = 1000;
						particles[i].color = danmaku.Color;
					} else {
						particles[i].size = 0f;
						particles[i].lifetime = -1;
					}
				}
			} else {
				for(int i = 0; i < danmakuCount; i++) {
					done = enumerator.MoveNext();
					if(done) {
						Danmaku danmaku = enumerator.Current;
						particles[i].position = danmaku.position;
						particles[i].rotation = danmaku.rotation;
						particles[i].size = danmaku.Scale;
						particles[i].axisOfRotation = forward;
						particles[i].lifetime = 1000;
						particles[i].color = danmaku.Color;
					} else {
						particles[i].size = 0f;
						particles[i].lifetime = -1;
					}
				}
			}
			runtimeSystem.SetParticles(particles, danmakuCount);
		}

		public void Awake() {
			circleCollider = GetComponent<CircleCollider2D>();
			if(circleCollider == null) {
				throw new System.InvalidOperationException("ProjectilePrefab without a Collider! (" + name + ")");
			}
			spriteRenderer = GetComponent<SpriteRenderer>();
			if(spriteRenderer == null) {
				throw new System.InvalidOperationException("ProjectilePrefab without a SpriteRenderer (" + name + ")");
			}
			
			cachedScale = transform.localScale;
			cachedTag = gameObject.tag;
			cachedLayer = gameObject.layer;
			cachedColliderRadius = circleCollider.radius;
			cachedColliderOffset = circleCollider.offset;
			cachedSprite = spriteRenderer.sprite;
			cachedColor = spriteRenderer.color;
			cachedMaterial = spriteRenderer.sharedMaterial;
			cachedSortingLayer = spriteRenderer.sortingLayerID;
			cachedSortingOrder = spriteRenderer.sortingOrder;

			for (int i = 0; i < extraControllers.Length; i++) {
				controllerAggregate += extraControllers[i].UpdateDanmaku;
			}

			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;

			currentDanmaku = new HashSet<Danmaku>();
			//currentDanmaku = new Danmaku[128];

			particles = new ParticleSystem.Particle[128];

			//Disable all other components
			foreach (Behaviour comp in GetComponentsInChildren<Behaviour>()) {
				if(comp != this) {
					comp.enabled = false;
				}
			}
	
			runtimeSystem = Instantiate(danmakuParticlePrefab) as ParticleSystem;
			runtimeSystem.transform.position = Vector3.zero;
			runtimeRenderer = runtimeSystem.GetComponent<ParticleSystemRenderer> ();

			runtimeSystem.simulationSpace = ParticleSystemSimulationSpace.World;
			runtimeSystem.startColor = Color;
			runtimeSystem.startSize = 1;
			runtimeSystem.startLifetime = float.PositiveInfinity;
			runtimeSystem.gravityModifier = 0f;
			runtimeSystem.startSpeed = 0f;
			runtimeSystem.enableEmission = false;

			particles = new ParticleSystem.Particle[runtimeSystem.particleCount];

			Material renderMaterial = new Material(Material);
			renderMaterial.mainTexture = Sprite.texture;

			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
			propertyBlock.AddTexture("_MainTex", Sprite.texture);

			spriteMesh = new Mesh();
			
			var verts = Sprite.vertices;
			var tris = Sprite.triangles;
			
			Vector3[] vertexes = new Vector3[verts.Length];
			int[] triangles = new int[tris.Length];
			
			Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
			
			for (int i = 0; i < verts.Length; i++) {
				vertexes [i] = transformMatrix * ((Vector3)verts [i]);
				//vertexes[i] = verts[i];
			}
			
			for (int i = 0; i < tris.Length; i++) {
				triangles [i] = (int)tris [i];
			}
			
			spriteMesh.vertices = vertexes;
			spriteMesh.uv = Sprite.uv;
			spriteMesh.triangles = triangles;
			
			runtimeRenderer.mesh = spriteMesh;
			runtimeRenderer.renderMode = ParticleSystemRenderMode.Mesh;

			runtimeRenderer.sharedMaterial = renderMaterial;
			runtimeRenderer.SetPropertyBlock(propertyBlock);
			runtimeRenderer.sortingLayerID = cachedSortingLayer;
			runtimeRenderer.sortingOrder = cachedSortingOrder;
			runtimeRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			runtimeRenderer.receiveShadows = false;
			runtimeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			runtimeRenderer.useLightProbes = false;

		}

		void OnDestroy() {
			if (runtimeSystem != null) {
				Destroy(runtimeSystem.gameObject);
			}
			if (spriteMesh != null) {
				Destroy(spriteMesh);
			}
		}

		internal DanmakuPrefab GetRuntime() {
			if(runtime == null)
				runtime = CreateRuntimeInstance(this);
			return runtime;
		}

		private static DanmakuPrefab CreateRuntimeInstance(DanmakuPrefab prefab) {
			DanmakuPrefab runtime = (DanmakuPrefab)Instantiate (prefab);
			//runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			//runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}