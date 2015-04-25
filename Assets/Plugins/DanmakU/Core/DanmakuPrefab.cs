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
	[DisallowMultipleComponent]
	[AddComponentMenu("DanmakU/Danmaku Prefab")]
	public sealed class DanmakuPrefab : BetterBehaviour {

		public enum RenderingType { Sprite, Mesh }

		#region Prefab Fields
		[SerializeField]
		private ParticleSystem danmakuSystemPrefab;

		[SerializeField]
		internal Danmaku.ColliderType collisionType;

		[SerializeField]
		internal Vector2 colliderSize;

		[SerializeField]
		internal Vector2 colliderOffset;

		[SerializeField]
		private RenderingType renderingType;

		[SerializeField]
		private Sprite sprite;

		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private Color color;
		
		[SerializeField]
		private Material material;

		[SerializeField]
		private int sortingLayer;

		[SerializeField]
		private int sortingOrder;

		private void Reset() {
			danmakuSystemPrefab = (Resources.Load ("Danmaku Particle System") as GameObject).GetComponent<ParticleSystem> ();
			collisionType = Danmaku.ColliderType.Point;
			colliderSize = Vector2.zero;
			colliderOffset = Vector2.zero;
			renderingType = RenderingType.Sprite;
			sprite = null;
			mesh = null;
			color = Color.white;
			sortingLayer = 0;
			sortingOrder = 0;
			SpriteRenderer temp = GetComponent<SpriteRenderer> ();
			if (temp == null) {
				gameObject.AddComponent<SpriteRenderer> ();
				material = temp.sharedMaterial;
				Object.DestroyImmediate (temp);
			} else {
				material = temp.sharedMaterial;
			}

		}

		[Serialize]
		internal bool fixedAngle;

		[Serialize]
		private IDanmakuController[] extraControllers;
		
		internal Vector3 cachedScale;
		internal string cachedTag;
		internal int cachedLayer;
		
		internal Sprite cachedSprite;
		internal Color cachedColor;
		internal Material cachedMaterial;
		internal int cachedSortingLayer;
		internal int cachedSortingOrder;
		private DanmakuController controllerAggregate;
		#endregion

		#region Runtime fields
		private RenderingType renderType;
		private DanmakuPrefab runtime;
		private Mesh renderMesh;
		private Material renderMaterial;
		private ParticleSystem runtimeSystem;
		private ParticleSystemRenderer runtimeRenderer;
		private ParticleSystem.Particle[] particles;
		private HashSet<Danmaku> currentDanmaku;
		private int danmakuCount;
		#endregion

		#region Accessor Properties
		public RenderingType RenderType {
			get {
				return renderType;
			}
		}

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
		public Vector2 ColliderSize {
			get {
				return colliderSize;
			}
		}
		
		/// <summary>
		/// Gets the offset of the instance's collider
		/// </summary>
		/// <value>the offset of the collider.</value>
		public Vector2 ColliderOffset {
			get {
				return colliderOffset;
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

		public Mesh Mesh {
			get {
				return renderMesh;
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
						particles[i].color = danmaku.Color;
						if(particles[i].lifetime <= 1)
							particles[i].lifetime = 1000;
					} else {
						particles[i].size = 0f;
						particles[i].lifetime = -1;
					}
				}
			}
			runtimeSystem.SetParticles(particles, danmakuCount);
		}

		public void Awake() {
			Vector3[] vertexes;

			Renderer singleRenderer;
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
			if (spriteRenderer == null && meshRenderer == null) {
				Debug.LogError("Danmaku Prefab (" + name + ") has neither SpriteRenderer or MeshRenderer. Attach one or the other to it.");
				Destroy (this);
				return;
			}
			
			cachedScale = transform.localScale;
			cachedTag = gameObject.tag;
			cachedLayer = gameObject.layer;

			if(danmakuSystemPrefab != null)
				runtimeSystem = Instantiate(danmakuSystemPrefab);
			if (runtimeSystem == null) {
				GameObject runtimeObject = Instantiate (Resources.Load ("Danmaku Particle System")) as GameObject;
				if(runtimeObject == null) {
					runtimeObject = new GameObject(name);
				}
				if (runtimeSystem == null) {
					runtimeSystem = runtimeObject.GetComponent<ParticleSystem>();
				}
				if (runtimeSystem == null)
					runtimeSystem = runtimeObject.AddComponent<ParticleSystem> ();
			}
			Transform runtimeTransform = runtimeSystem.transform;
//			Transform root = runtimeTransform.root;
			runtimeTransform.parent = null;
			runtimeTransform.localPosition = Vector3.zero;

//			foreach (Transform sibling in root) {
//				Destroy (sibling.gameObject);
//			}
//			foreach (Transform child in runtimeTransform) {
//				if(child != runtimeTransform)
//					Destroy (child.gameObject);
//			}

			runtimeRenderer = runtimeSystem.GetComponent<ParticleSystemRenderer> ();
//			foreach (Component componentCheck in runtimeSystem.GetComponentsInChildren<Component>()) {
//				if(componentCheck == runtimeSystem)
//					continue;
//				if(componentCheck == runtimeRenderer)
//					continue;
//				if(componentCheck == runtimeTransform)
//					continue;
//				Destroy (componentCheck);
//			}
			
			renderMesh = new Mesh();

			if (meshRenderer == null) {
				renderType = RenderingType.Sprite;
				singleRenderer = spriteRenderer;
				cachedSprite = spriteRenderer.sprite;
				cachedColor = spriteRenderer.color;
				cachedMaterial = spriteRenderer.sharedMaterial;
				cachedSortingLayer = spriteRenderer.sortingLayerID;
				cachedSortingOrder = spriteRenderer.sortingOrder;

				renderMaterial = new Material(cachedMaterial);
				renderMaterial.mainTexture = Sprite.texture;

				if(cachedSprite == null)
					runtimeRenderer.mesh = null;
				else {
					renderType = RenderingType.Sprite;
					var verts = cachedSprite.vertices;
					var tris = cachedSprite.triangles;
					
					vertexes = new Vector3[verts.Length];
					int[] triangles = new int[tris.Length];
					
					for (int i = 0; i < verts.Length; i++) {
						vertexes [i] =  (Vector3)verts [i];
					}
					
					for (int i = 0; i < tris.Length; i++) {
						triangles [i] = (int)tris [i];
					}
					
					renderMesh.vertices = vertexes;
					renderMesh.uv = cachedSprite.uv;
					renderMesh.triangles = triangles;
				}
			} else {
				renderType = RenderingType.Mesh;
				singleRenderer = meshRenderer;
				cachedSprite = null;
				cachedColor = Color.white;
				cachedMaterial = meshRenderer.sharedMaterial;
				cachedSortingLayer = meshRenderer.sortingLayerID;
				cachedSortingOrder = meshRenderer.sortingOrder;

				renderMaterial = meshRenderer.sharedMaterial;
				MeshFilter filter = meshRenderer.GetComponent<MeshFilter>();
				if(filter == null) {
					Debug.LogError("Danmaku Prefab (" + name + ") is trying to use a MeshRenderer as a base, but no MeshFilter is found. Please add one.");
				} else {
					Mesh filterMesh = filter.mesh;
					renderMesh.vertices = filterMesh.vertices;
					renderMesh.uv = filterMesh.uv;
					renderMesh.triangles = filterMesh.triangles;
					renderMesh.colors = filterMesh.colors;
					renderMesh.normals = filterMesh.normals;
					renderMesh.tangents = filterMesh.tangents;
				}
			}

			if (renderMesh != null) {

				//Scale the mesh as necessary based on prefab's scale

				Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
				vertexes = renderMesh.vertices;
				for(int i = 0; i < vertexes.Length; i++) {
					vertexes[i] = transformMatrix * vertexes[i];
				}
				renderMesh.vertices = vertexes;
				renderMesh.Optimize();
			}
			
			runtimeRenderer.mesh = renderMesh;

			singleRenderer.enabled = false;
			//GetComponent<CircleCollider2D>().enabled = false;
			//Disable all other components
			foreach (Behaviour comp in GetComponentsInChildren<Behaviour>()) {
				if(comp != this) {
					comp.enabled = false;
				}
			}

			runtimeSystem.simulationSpace = ParticleSystemSimulationSpace.World;
			runtimeSystem.startColor = cachedColor;
			runtimeSystem.startSize = 1;
			runtimeSystem.startLifetime = float.PositiveInfinity;
			runtimeSystem.gravityModifier = 0f;
			runtimeSystem.startSpeed = 0f;
			runtimeSystem.enableEmission = false;

			currentDanmaku = new HashSet<Danmaku> ();
			particles = new ParticleSystem.Particle[runtimeSystem.particleCount];
			for (int i = 0; i < extraControllers.Length; i++) {
				controllerAggregate += extraControllers[i].UpdateDanmaku;
			}
			
			runtimeRenderer.mesh = renderMesh;
			runtimeRenderer.renderMode = ParticleSystemRenderMode.Mesh;
			
			runtimeRenderer.sharedMaterial = renderMaterial;
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
			if (renderMesh != null) {
				Destroy(renderMesh);
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