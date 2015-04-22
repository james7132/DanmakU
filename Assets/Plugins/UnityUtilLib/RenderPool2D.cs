//using UnityEngine;
//using System.Collections.Generic;
//using UnityUtilLib.Pooling;
//
//namespace UnityUtilLib {
//
//	public sealed class RenderInstance : IPooledObject {
//
//		private RenderPool2D pool;
//
//		public Vector2 Position;
//		public float Rotation;
//		public Color32 Color;
//		public float Scale;
//
//		public Sprite Sprite {
//			get {
//				if(pool != null)
//					return pool.Sprite;
//				return null;
//			}
//			set {
//				if(pool.Sprite != null)
//					pool.Sprite = value;
//			}
//		}
//
//		private bool is_active;
//
//		internal RenderInstance(Color32 color) {
//			Color = color;
//			Scale = 1f;
//		}
//
//		#region IPooledObject implementation
//		public void Activate () {
//			is_active = true;
//		}
//		public void Deactivate () {
//			if (pool != null) {
//				pool.Return(this);
//			}
//		}
//		public IPool Pool {
//			get {
//				return pool;
//			}
//			set {
//				pool = value as RenderPool2D;
//			}
//		}
//		public bool IsActive {
//			get {
//				return is_active;
//			}
//		}
//		#endregion
//	}
//
//	[RequireComponent(typeof(SpriteRenderer))]
//	public sealed class RenderPool2D : IPool<RenderInstance> {
//
//		static RenderPool2D() {
//			RendererPrefabPath = "RenderObject System";
//		}
//
//		public static string RendererPrefabPath {
//			get;
//			set;
//		}
//
//		private HashSet<RenderInstance> active;
//
//		private SpriteRenderer referenceRenderer;
//
//		private Mesh spriteMesh;
//		private Sprite referenceSprite;
//		private Vector3 referenceScale;
//		private Material renderMaterial;
//
//		private ParticleSystem particleSystem;
//		private ParticleSystemRenderer renderer;
//		private ParticleSystem.Particle[] particles;
//
//		private int activeCount;
//		
//		public Sprite Sprite {
//			get {
//				return referenceSprite;
//			}
//			set {
//				referenceSprite = value;
//				if(value == null) {
//					renderer.mesh = null;
//				} else {
//					CreateSpriteMesh(referenceSprite, scale);
//				}
//			}
//		}
//
//		public Vector2 Scale {
//			get {
//				return scale;
//			}
//			set {
//				CreateSpriteMesh(referenceSprite, value);
//			}
//		}
//
//		public Material Material {
//			get {
//				return renderMaterial;
//			}
//			set {
//				if(value == null) {
//					renderer.material = null;
//					return;
//				}
//				
//				Material temp = renderMaterial;
//				renderMaterial = new Material(value);
//				if(temp != null) {
//					Object.Destroy(temp);
//				}
//
//				if(referenceSprite != null)
//					renderMaterial.mainTexture = referenceSprite.texture;
//
//				renderer.material = renderMaterial;
//			}
//		}
//
//		public Color DefaultColor {
//			get {
//				return referenceRenderer.color;
//			}
//			set {
//				referenceRenderer.color = value;
//			}
//		}
//		
//		public Vector3 AxisOfRotation;
//
//		void Awake() {
//		}
//
//		public RenderPool2D() {
//			Init(null);
//		}
//
//		public RenderPool2D(SpriteRenderer spriteRenderer, ParticleSystem prefab = null) {
//			if (spriteRenderer == null) {
//				throw new System.ArgumentException("Cannot use a null spriteRenderer");
//			}
//			Init(prefab);
//			CreateSpriteMesh(spriteRenderer.sprite, spriteRenderer.transform.localScale);
//			Material = spriteRenderer.sharedMaterial;
//			if (particleSystem != null) {
//				particles = new ParticleSystem.Particle[particleSystem.particleCount];
//			} else {
//				particles = new ParticleSystem.Particle[1000];
//			}
//		}
//
//		public RenderPool2D(Sprite sprite, Material targetMaterial, Vector2 scale, ParticleSystem prefab = null) {
//			if (sprite == null) {
//				throw new System.ArgumentException("Cannot use a null spriteRenderer");
//			}
//			Init(prefab);
//			CreateSpriteMesh(sprite, scale);
//			Material = targetMaterial;	
//		}
//
//		public RenderPool2D(Sprite sprite, Material targetMaterial, Transform transform, ParticleSystem prefab = null) {
//			if (sprite == null || transform == null) {
//				throw new System.ArgumentException("Cannot use a null Sprite or Transform");
//			}
//			Init(prefab);
//			CreateSpriteMesh(sprite, scale);
//			Material = targetMaterial;
//		}
//
//		private void CreateSpriteMesh (Sprite sprite, Vector3 scaleFactor) {
//			if (sprite == referenceSprite && scale == scaleFactor) {
//				return;
//			}
//			referenceSprite = sprite;
//			scale = scaleFactor;
//			if (sprite == null) {
//				renderer.mesh = null;
//				return;
//			}
//			
//			var verts = sprite.vertices;
//			var tris = sprite.triangles;
//			
//			Vector3[] vertexes = new Vector3[verts.Length];
//			int[] triangles = new int[tris.Length];
//			
//			Matrix4x4 transform = Matrix4x4.TRS(Vector3.one, Quaternion.identity, scale);
//			
//			for (int i = 0; i < verts.Length; i++) {
//				vertexes [i] = transform * ((Vector3)verts [i]);
//			}
//			
//			for (int i = 0; i < tris.Length; i++) {
//				triangles [i] = (int)tris [i];
//			}
//			
//			spriteMesh.vertices = vertexes;
//			spriteMesh.uv = sprite.uv;
//			spriteMesh.triangles = triangles;
//			
//			renderer.mesh = spriteMesh;
//		}
//
//		public void Update() {
//			int particleCount = particleSystem.particleCount;
//			if (activeCount > particleCount) {
//				//Debug.Log("hello");
//				particleSystem.maxParticles = Mathf.NextPowerOfTwo(activeCount);
//				particleSystem.Emit(activeCount - particleCount);
//				//Debug.Log(runtimeSystem.particleCount);
//				particleCount = activeCount;
//			}
//			if (activeCount > particles.Length) {
//				particles = new ParticleSystem.Particle[Mathf.NextPowerOfTwo(activeCount + 1)];
//			}
//			
//			int count2 = particleSystem.GetParticles(particles);
//			Debug.Log(count2);
//			bool done;
//			IEnumerator<RenderInstance> enumerator = active.GetEnumerator();
//			for(int i = 0; i < count2; i++) {
//				done = enumerator.MoveNext();
//				if(done) {
//					RenderInstance instance = enumerator.Current;
//					particles[i].position = instance.Position;
//					particles[i].rotation = instance.Rotation;
//					particles[i].size = instance.Scale;
//					particles[i].axisOfRotation = AxisOfRotation;
//					particles[i].lifetime = 1000;
//					particles[i].color = instance.Color;
//				} else {
//					particles[i].size = 0f;
//					particles[i].lifetime = -1;
//				}
//			}
//			particleSystem.SetParticles(particles, activeCount);
//		}
//		
//		#region IPool implementation
//		public RenderInstance Get () {
//			throw new System.NotImplementedException();
//		}
//		public void Return (RenderInstance obj) {
//			throw new System.NotImplementedException();
//		}
//		#endregion
//		#region IPool implementation
//		object IPool.Get () {
//			throw new System.NotImplementedException();
//		}
//		public void Return (object obj) {
//			throw new System.NotImplementedException();
//		}
//		#endregion
//	}
//
//
//}