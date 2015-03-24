using UnityEngine;
using UnityUtilLib;
using System.Collections;
using Danmaku2D.NoScript;

namespace Danmaku2D {

	public abstract class ProjectileEmitter : PausableGameObject {

		[SerializeField]
		private ProjectileSource source;

		[SerializeField]
		private FrameCounter delay;

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
			print ("fire");
			SourcePoint[] points = source.SourcePoints;
			int size = points.Length;
			for(int i = 0; i < size; i++) {
				FireFromSource(points[i]);
			}
		}

		protected abstract void FireFromSource (SourcePoint source);
	}
}