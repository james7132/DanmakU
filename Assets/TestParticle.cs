using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace DanmakU {
	
	public class TestParticle  : MonoBehaviour {

		private static GameObject targetGameObject;

		private ParticleSystem test;
		private ParticleSystemRenderer render;
		private ParticleSystem.Particle[] particles;
		private int j = 5000;
		private int count;
		private Stopwatch stopwatch;

		private ThreadTest[] tt;

		private class DanmakuParticleManager {

			private ParticleSystem particleSystem;
			private ParticleSystemRenderer renderer;
			private DanmakuPrefab prefab;

			public DanmakuParticleManager(DanmakuPrefab prefab) {
				if(targetGameObject == null)
					targetGameObject = new GameObject("Danmaku Systems");

				particleSystem = targetGameObject.AddComponent<ParticleSystem>();
				renderer = targetGameObject.GetComponent<ParticleSystemRenderer>();
				this.prefab = prefab;

				var sprite = prefab.Sprite;
				var material = new Material(prefab.Material);
				material.mainTexture = sprite.texture;

				particleSystem.startColor = prefab.Color;
				particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
				particleSystem.startSize = 2 * new Bounds2D(sprite.bounds).Extents.Max();
			}

		}
		
		void Start() {


			test = GetComponent<ParticleSystem>();
			render = GetComponent<ParticleSystemRenderer>();
			var renderSprite = GetComponent<SpriteRenderer>();
			var sprite = renderSprite.sprite;
			
			Material testMaterial = new Material(renderSprite.sharedMaterial);
			testMaterial.mainTexture = sprite.texture;
			test.startColor = renderSprite.color;
			test.simulationSpace = ParticleSystemSimulationSpace.World;
			render.sharedMaterial = testMaterial;

			var mesh = new Mesh();

			var verts = sprite.vertices;
			var tris = sprite.triangles;

			Vector3[] vertexes = new Vector3[verts.Length];
			int[] triangles = new int[tris.Length];

			for (int i = 0; i < verts.Length; i++) {
				vertexes[i] = verts[i];
			}

			for (int i = 0; i < tris.Length; i++) {
				triangles[i] = (int)tris[i];
			}
			
			mesh.vertices = vertexes;
			mesh.uv = sprite.uv;
			mesh.triangles = triangles;
			
			render.renderMode = ParticleSystemRenderMode.Mesh;
			render.mesh = mesh;

			render.sortingLayerID = renderSprite.sortingLayerID;
			render.sortingOrder = renderSprite.sortingOrder;

			test.startSize = 1; //2 * new Bounds2D(sprite.bounds).Extents.Max();

			//test.duration = float.PositiveInfinity;
			test.startLifetime = float.PositiveInfinity;
			test.startSpeed = 0;
			particles = new ParticleSystem.Particle[j];
			test.enableEmission = false;
			test.maxParticles = j;
			print(test.particleCount);
			test.Emit(j);
			print(test.particleCount);
			test.gravityModifier = 0f;
			tt = new ThreadTest[j];
			for (int i = 0; i < tt.Length; i++) {
				tt[i] = new ThreadTest();
			}
			stopwatch = new Stopwatch();
		}
		
		void Update() {
			if (test.particleCount < j) {
				print("Hello");
				test.Emit(j - test.particleCount);
			}
			count = test.GetParticles(particles);

			List<WaitHandle> doneEvents = new List<WaitHandle>();
			int subdivide = count / 10;
			for (int i = 0; i < count; i += subdivide) {
				ThreadTest tester = tt[i];
				tester.start = i;
				if(i + subdivide < count) {
					tester.size = subdivide;
				} else {
					tester.size = count - i;
				}
				//print(tester.start + ", " + tester.size);
				tester.particles = particles;
				doneEvents.Add(tester.doneEvent);
				ThreadPool.QueueUserWorkItem(tester.Callback);
			}

			//WaitHandle.WaitAll(doneEvents.ToArray());
			test.SetParticles(particles, count);
			//print(test.particleCount);
		}

		private class ThreadTest {

			public int start = 0;
			public int size;
			public ParticleSystem.Particle[] particles;
			public ManualResetEvent doneEvent;
			private static Vector3 forward = Vector3.forward;
			private static Vector3 one = Vector3.one * 10;

			public ThreadTest() {
				doneEvent = new ManualResetEvent(false);
			}

			public void Callback(System.Object o) {
				doneEvent.Reset();
				var count = 0;
				while (count < size) {
					int index = count + start;
					ParticleSystem.Particle part = particles[index];
					part.axisOfRotation = forward;
					part.rotation = index * Mathf.PI / 6;
					part.position = index * one / 2;
					particles[index] = part;
				}
				doneEvent.Set();
			}

		}
		
	}

}
