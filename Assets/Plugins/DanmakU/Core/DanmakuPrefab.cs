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

		[Serialize, Default(Enum = 0)]
		internal Danmaku.ColliderType collisionType;

		[Serialize, Default(1f, 1f)]
		internal Vector2 colliderSize;

		[Serialize, Default(0f, 0f)]
		internal Vector2 colliderOffset;

		[Serialize, Default(Enum = 0)]
		private RenderingType renderingType;

		[Serialize]
		private Sprite sprite;
		
		[Serialize]
		private Mesh mesh;
		
		[Serialize]
		private Color color;
		
		[Serialize]
		private Material material;
		
		[Serialize, Default(0)]
		private int sortingLayer;
		
		[Serialize, Default(0)]
		private int sortingOrder;

		public override void Reset() {
			base.Reset ();
			danmakuSystemPrefab = (Resources.Load ("Danmaku Particle System") as GameObject).GetComponent<ParticleSystem> ();
			sprite = null;
			mesh = null;
			color = Color.white;
			material = null;

		}

		[Serialize]
		internal bool fixedAngle;

		[Serialize]
		private IDanmakuController[] extraControllers;
		
		internal Vector3 cachedScale;
		internal string cachedTag;
		internal int cachedLayer;

		private DanmakuController controllerAggregate;
		#endregion

		#region Runtime fields
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
				return renderingType;
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
				return sprite;
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
				return color;
			}
		}
		
		/// <summary>
		/// Gets the material used by the instance's SpriteRenderer
		/// </summary>
		/// <value>The material to be rendered with.</value>
		public Material Material {
			get {
				return material;
			}
		}
		
		/// <summary>
		/// Gets the sorting layer u
		/// </summary>
		/// <value>The sorting layer to be used when rendering.</value>
		public int SortingLayerID {
			get {
				return sortingLayer;
			}
		}
		
		public int SortingOrder {
			get {
				return sortingOrder;
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

//			Renderer singleRenderer;
//			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
//			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
//			if (spriteRenderer == null && meshRenderer == null) {
//				Debug.LogError("Danmaku Prefab (" + name + ") has neither SpriteRenderer or MeshRenderer. Attach one or the other to it.");
//				Destroy (this);
//				return;
//			}

			foreach (Component otherComponent in GetComponentsInChildren<Component>()) {
				if(otherComponent != this && !(otherComponent is Transform)) {
					Destroy(otherComponent);
				}
			}

			foreach (Transform child in transform) {
				Destroy (child.gameObject);
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

			if (renderingType == RenderingType.Sprite) {
				renderMaterial = new Material(material);
				renderMaterial.mainTexture = Sprite.texture;

				if(sprite == null)
					runtimeRenderer.mesh = null;
				else {
					var verts = sprite.vertices;
					var tris = sprite.triangles;
					
					vertexes = new Vector3[verts.Length];
					int[] triangles = new int[tris.Length];
					
					for (int i = 0; i < verts.Length; i++) {
						vertexes [i] =  (Vector3)verts [i];
					}
					
					for (int i = 0; i < tris.Length; i++) {
						triangles [i] = (int)tris [i];
					}
					
					renderMesh.vertices = vertexes;
					renderMesh.uv = sprite.uv;
					renderMesh.triangles = triangles;
				}
			} else {
				if(mesh != null) {
					renderMaterial = material;
					renderMesh.vertices = mesh.vertices;
					renderMesh.uv = mesh.uv;
					renderMesh.triangles = mesh.triangles;
					renderMesh.colors = mesh.colors;
					renderMesh.normals = mesh.normals;
					renderMesh.tangents = mesh.tangents;
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

			//singleRenderer.enabled = false;
			//GetComponent<CircleCollider2D>().enabled = false;
			//Disable all other components
			foreach (Behaviour comp in GetComponentsInChildren<Behaviour>()) {
				if(comp != this) {
					comp.enabled = false;
				}
			}

			runtimeSystem.simulationSpace = ParticleSystemSimulationSpace.World;
			//runtimeSystem.startColor = cachedColor;
			runtimeSystem.startColor = color;
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
			//runtimeRenderer.sortingLayerID = cachedSortingLayer;
			//runtimeRenderer.sortingOrder = cachedSortingOrder;
			runtimeRenderer.sortingLayerID = sortingLayer;
			runtimeRenderer.sortingOrder = sortingOrder;
			runtimeRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			runtimeRenderer.receiveShadows = false;
			runtimeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			runtimeRenderer.useLightProbes = false;
		}

		void OnDrawGizmosSelected() {

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
			DanmakuPrefab runtime = Instantiate (prefab);
			runtime.enabled = true;
			runtime.gameObject.SetActive (true);
			return runtime;
		}
	}
}