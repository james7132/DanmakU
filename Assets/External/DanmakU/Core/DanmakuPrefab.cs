// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

namespace DanmakU {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(SpriteRenderer)), AddComponentMenu("Danmaku 2D/Danmaku Prefab")]
	public sealed class DanmakuPrefab : DanmakuObjectPrefab {

		private DanmakuPrefab runtime;

		private static ParticleSystem.Particle hiddenParticle;

		private Mesh renderMesh;
		private Material renderMaterial;
		private Mesh spriteMesh;
		private CombineInstance[] danmakuInstances;

		static DanmakuPrefab() {
			hiddenParticle = new ParticleSystem.Particle();
			hiddenParticle.position = new Vector3(20000000f, 0f, 0f);
			hiddenParticle.startLifetime = float.PositiveInfinity;
		}

		private ParticleSystem danmakuParticle;
		private ParticleSystemRenderer danmakuRenderer;
		private ParticleSystem.Particle[] particles;

		private HashSet<Danmaku> currentDanmaku;

		[SerializeField]
		private IDanmakuController[] extraControllers;
		internal IDanmakuController[] ExtraControllers {
			get {
				return extraControllers;
			}
		}

		public void Add(Danmaku danmaku) {
			currentDanmaku.Add(danmaku);
			danmakuParticle.Emit(danmaku.particle);
		}

		public void Remove(Danmaku danmaku) {
			currentDanmaku.Remove(danmaku);
		}

		void Update() {
//			int count = currentDanmaku.Count;
//			int particleCount = particles.Length;
//			var max = danmakuParticle.maxParticles;
//			if (max > particleCount) {
//				particles = new ParticleSystem.Particle[max];
//			}
//			particleCount = 0;
//			print(danmakuParticle.particleCount);
//			int currentCount = danmakuParticle.GetParticles(particles);
//			if (currentCount < count) {
//				//print("hello" + (count - currentCount));
//				danmakuParticle.Emit(count - currentCount);
//			}
//			foreach (Danmaku danmaku in currentDanmaku) {
//				particles[particleCount] = danmaku.particle;
//				particleCount++;
//			}
//			Vector3 zero = Vector2.one;
//			var forward = Vector3.forward;
//			for (int i = 0; i < max; i++) {
//				if(i > count) {
//					particles[i] = hiddenParticle;
//				} else {
//					ParticleSystem.Particle part = particles [i];
//					part.position = i * zero;
//					part.rotation = 0f;
//					part.axisOfRotation = forward;
//					part.startLifetime = float.PositiveInfinity;
//					particles [i] = part;
//				}
//			}
//			//danmakuParticle.SetParticles(particles, max);
//			print(danmakuParticle.particleCount);
//			danmakuParticle.Emit(20);
			Matrix4x4 test = Matrix4x4.identity;
			CombineInstance instance = danmakuInstances[0];
			for (int i = 0; i < danmakuInstances.Length; i++) {
				instance.mesh = spriteMesh;
				instance.transform = Matrix4x4.TRS(Vector2.one * i, Quaternion.identity, Vector3.one);
				danmakuInstances[i] = instance;
			}
			renderMesh.CombineMeshes(danmakuInstances, false, true);

			Graphics.DrawMesh(renderMesh, Matrix4x4.identity, renderMaterial, cachedLayer);
		}

		public override void Awake() {
			base.Awake();

			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;

			currentDanmaku = new HashSet<Danmaku> ();

			particles = new ParticleSystem.Particle[128];

			renderMesh = new Mesh();

			//Disable all other components
			foreach (Behaviour comp in GetComponentsInChildren<Behaviour>()) {
				if(comp != this) {
					comp.enabled = false;
				}
			}

			danmakuParticle = gameObject.AddComponent<ParticleSystem>();
			danmakuRenderer = GetComponent<ParticleSystemRenderer>();

			danmakuParticle.simulationSpace = ParticleSystemSimulationSpace.World;
			danmakuParticle.startColor = Color;
			danmakuParticle.startSize = 1;//new Bounds2D(Sprite.bounds).Size.Max();
			danmakuParticle.startLifetime = float.PositiveInfinity;
			danmakuParticle.gravityModifier = 0f;
			danmakuParticle.startSpeed = 0f;
			danmakuParticle.enableEmission = false;
			danmakuParticle.maxParticles = 10000;
			
			danmakuParticle.Emit(danmakuParticle.maxParticles);

			renderMaterial = new Material(Material);
			renderMaterial.mainTexture = Sprite.texture;

			spriteMesh = new Mesh();

			danmakuInstances = new CombineInstance[20];

			var verts = Sprite.vertices;
			var tris = Sprite.triangles;
			
			Vector3[] vertexes = new Vector3[verts.Length];
			int[] triangles = new int[tris.Length];

			Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
			
			for (int i = 0; i < verts.Length; i++) {
				vertexes[i] = transformMatrix * ((Vector3)verts[i]);
				//vertexes[i] = verts[i];
			}
			
			for (int i = 0; i < tris.Length; i++) {
				triangles[i] = (int)tris[i];
			}

			spriteMesh.vertices = vertexes;
			spriteMesh.uv = Sprite.uv;
			spriteMesh.triangles = triangles;

			danmakuRenderer.renderMode = ParticleSystemRenderMode.Mesh;
			danmakuRenderer.sharedMaterial = renderMaterial;
			danmakuRenderer.mesh = spriteMesh;
			danmakuRenderer.sortingLayerID = cachedSortingLayer;
			danmakuRenderer.sortingOrder = cachedSortingOrder;
			danmakuRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			danmakuRenderer.receiveShadows = false;
			danmakuRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			danmakuRenderer.useLightProbes = false;
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