using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class ProjectilePrefab : MonoBehaviour {

		private static Dictionary<ProjectilePrefab, ProjectilePrefab> runtimeInstances;

		[SerializeField]
		private CircleCollider2D circleCollider;
		public CircleCollider2D CircleCollider {
			get {
				if(circleCollider == null)
					circleCollider = GetComponent<CircleCollider2D> ();
				return circleCollider;
			}
		}

		[SerializeField]
		private SpriteRenderer spriteRenderer;
		public SpriteRenderer SpriteRenderer {
			get {
				if(spriteRenderer == null)
					spriteRenderer = GetComponent<SpriteRenderer> ();
				return spriteRenderer;
			}
		}

		[SerializeField]
		private ProjectileControlBehavior[] extraControllers;
		public ProjectileControlBehavior[] ExtraControllers {
			get {
				if(extraControllers == null)
					extraControllers = GetComponents<ProjectileControlBehavior>();
				return extraControllers;
			}
		}

		public ProjectilePrefab GetRuntime() {
			if(runtimeInstances == null)
				runtimeInstances = new Dictionary<ProjectilePrefab, ProjectilePrefab>();
			if(!runtimeInstances.ContainsKey (this))
				runtimeInstances[this] = CreateRuntimeInstance(this);
			return runtimeInstances [this];
		}

		private static ProjectilePrefab CreateRuntimeInstance(ProjectilePrefab prefab) {
			ProjectilePrefab runtime = (ProjectilePrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}

		public void Reinit() {
			circleCollider = GetComponent<CircleCollider2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			extraControllers = GetComponents<ProjectileControlBehavior> ();
		}
	}
}