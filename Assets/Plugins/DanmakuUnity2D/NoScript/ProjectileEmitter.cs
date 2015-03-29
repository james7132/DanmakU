using UnityEngine;
using UnityUtilLib;
using System.Collections;
using Danmaku2D.NoScript;

namespace Danmaku2D {

	public abstract class ProjectileEmitter : PausableGameObject {

		[SerializeField]
		private ProjectileSource source;

		protected ProjectileSource Source {
			get {
				return source;
			}
		}

		[SerializeField]
		private FrameCounter delay;

		[SerializeField]
		private ModifierWrapper modifier;

		public FireModifier Modifier {
			get {
				if(modifier == null)
					return null;
				return modifier.Modifier;
			}
		}

		public DanmakuField TargetField {
			get {
				return source.TargetField;
			}
			set {
				source.TargetField = value;
			}
		}

		public override void Awake () {
			base.Awake ();
			if (source == null)
				source = GetComponent<ProjectileSource> ();
		}

		public override void NormalUpdate () {
			if(delay.Tick()) {
				Fire ();
			}
		}

		public void Fire() {
			if (source == null)
				source = gameObject.AddComponent<PointSource> ();
//			print ("fire");
			FireProjectiles ();
		}

		protected abstract void FireProjectiles();
	}
}