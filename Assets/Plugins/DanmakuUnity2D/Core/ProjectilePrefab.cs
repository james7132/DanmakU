using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class ProjectilePrefab : DanmakuObjectPrefab {

		private ProjectilePrefab runtime;
		
		private ProjectileControlBehavior[] extraControllers;
		internal ProjectileControlBehavior[] ExtraControllers {
			get {
				return extraControllers;
			}
		}

		public override void Awake() {
			base.Awake ();
			extraControllers = GetComponents<ProjectileControlBehavior>();
		}

		internal ProjectilePrefab GetRuntime() {
			if(runtime == null)
				runtime = CreateRuntimeInstance(this);
			return runtime;
		}

		private static ProjectilePrefab CreateRuntimeInstance(ProjectilePrefab prefab) {
			ProjectilePrefab runtime = (ProjectilePrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}