// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

		#if UNITY_EDITOR
		public override void Reset() {
			base.Reset ();
			danmakuSystemPrefab = (Resources.Load ("Danmaku Particle System") as GameObject).GetComponent<ParticleSystem> ();
			SpriteRenderer sprite = GetComponent<SpriteRenderer>();
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			if (sprite == null && mesh == null) {
				if( UnityEditor.EditorUtility.DisplayDialog ("Choose a Renderer", 
				                                             "Danmaku Prefab requires one and only one renderer, please chose one", 
				                                             "Sprite Renderer", 
				                                             "Mesh Renderer")) {
					gameObject.AddComponent<SpriteRenderer>();
				} else {
					gameObject.AddComponent<MeshFilter> ();
					gameObject.AddComponent<MeshRenderer> ();
				}
			}
		}
		#endif

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
		private DanmakuPrefab runtime;
		private Mesh renderMesh;
		private Material renderMaterial;
		private ParticleSystem runtimeSystem;
		private ParticleSystemRenderer runtimeRenderer;
		private ParticleSystem.Particle[] particles;
		private HashSet<Danmaku> currentDanmaku;
		private int danmakuCount;
		private RenderingType renderingType;
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

		//private MaterialPropertyBlock mpb;

		internal DanmakuController ExtraControllers {
			get {
				return controllerAggregate;
			}
		}

		internal void Add(Danmaku danmaku) {
			currentDanmaku.Add(danmaku);
		}

		internal void Remove(Danmaku danmaku) {
			currentDanmaku.Remove(danmaku);
		}

		void Update() {
			//runtimeRenderer.GetPropertyBlock(mpb);
			//mpb.SetTexture("_MainTexture", cachedSprite.texture);
			//runtimeRenderer.SetPropertyBlock(mpb);

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
			bool done;
			IEnumerator<Danmaku> enumerator = currentDanmaku.GetEnumerator();
			if (fixedAngle) {
				for(int i = 0; i < danmakuCount; i++) {
					done = enumerator.MoveNext();
					if(done) {
						Danmaku danmaku = enumerator.Current;
						particles[i].position = danmaku.position;
						particles[i].size = danmaku.Scale;
						//particles[i].axisOfRotation = forward;
						particles[i].lifetime = 1000;
						particles[i].color = danmaku.Color;
					} else {
						particles[i].size = 0f;
						particles[i].lifetime = -1;
					}
				}
			} else {
				Vector3 forward = Vector3.forward;
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

			//mpb = new MaterialPropertyBlock();
			Vector3[] vertexes;

			Renderer singleRenderer = GetComponent<Renderer> ();
			SpriteRenderer spriteRenderer = singleRenderer as SpriteRenderer;
			MeshRenderer meshRenderer = singleRenderer as MeshRenderer;
			if (singleRenderer == null || (spriteRenderer == null && meshRenderer == null)) {
				Debug.LogError("Danmaku Prefab (" + name + ") has neither SpriteRenderer or MeshRenderer. Attach one or the other to it.");
				Destroy (this);
				return;
			}

			singleRenderer.enabled = false;

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
			//runtimeTransform.parent = null;
			runtimeTransform.localPosition = Vector3.zero;

			runtimeRenderer = runtimeSystem.GetComponent<ParticleSystemRenderer> ();

			renderMesh = new Mesh();

			if (meshRenderer == null) {
				renderingType = RenderingType.Sprite;
				singleRenderer = spriteRenderer;
				cachedSprite = spriteRenderer.sprite;
				cachedColor = spriteRenderer.color;
				cachedMaterial = spriteRenderer.sharedMaterial;
				cachedSortingLayer = spriteRenderer.sortingLayerID;
				cachedSortingOrder = spriteRenderer.sortingOrder;

				//renderMaterial = cachedMaterial;

				renderMaterial = new Material(cachedMaterial);
				renderMaterial.mainTexture = Sprite.texture;

				if(cachedSprite == null)
					runtimeRenderer.mesh = null;
				else {
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
				renderingType = RenderingType.Mesh;
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
					//Debug.Log(vertexes[i]);
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
			runtimeSystem.startColor = cachedColor;
			runtimeSystem.startSize = 1;
			runtimeSystem.startLifetime = float.PositiveInfinity;
			runtimeSystem.gravityModifier = 0f;
			runtimeSystem.startSpeed = 0f;
			runtimeSystem.enableEmission = false;

			currentDanmaku = new HashSet<Danmaku> ();
			particles = new ParticleSystem.Particle[runtimeSystem.particleCount];
			for (int i = 0; i < extraControllers.Length; i++) {
				controllerAggregate += extraControllers[i].Update;
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

			gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtimeSystem.gameObject.hideFlags = HideFlags.HideInHierarchy;
		}

		#if UNITY_EDITOR
		void OnDrawGizmosSelected() {
			Matrix4x4 oldGizmoMatrix = Gizmos.matrix;
			Matrix4x4 oldHandlesMatrix = Handles.matrix;
			Color oldGizmosColor = Gizmos.color;
			Color oldHandlesColor = Handles.color;
			Matrix4x4 hitboxMatrix = Matrix4x4.TRS ((Vector2)transform.position, Quaternion.Euler(0f, 0f, transform.eulerAngles.z), transform.lossyScale);
			Gizmos.matrix = hitboxMatrix;
			Handles.matrix = hitboxMatrix;
			Handles.color = Color.green;
			Gizmos.color = Color.green;
			switch (collisionType) {
				case Danmaku.ColliderType.Point:
					//Handles.PositionHandle(Vector3.zero, Quaternion.identity);
					break;
				case Danmaku.ColliderType.Circle:
					hitboxMatrix = Matrix4x4.TRS ((Vector2)transform.position, Quaternion.Euler(0f, 0f, transform.Rotation2D()), transform.lossyScale.Max() * Vector3.one);
					Gizmos.matrix = hitboxMatrix;
					Handles.matrix = hitboxMatrix;
					Handles.DrawWireDisc(colliderOffset, Vector3.forward, colliderSize.Max());
					break;
				case Danmaku.ColliderType.Box:
					Gizmos.DrawWireCube(colliderOffset, colliderSize);
					break;
				case Danmaku.ColliderType.Line:
					Handles.DrawLine(colliderOffset, colliderOffset + new Vector2(0f, colliderSize.x));
				    break;
			}
			Gizmos.matrix = oldGizmoMatrix;
			Handles.matrix = oldHandlesMatrix;
			Gizmos.color = oldGizmosColor;
			Handles.color = oldHandlesColor;
		}
		#endif
		
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