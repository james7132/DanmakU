using UnityEngine;
using UnityUtilLib;
using System.Collections;
using Danmaku2D.NoScript;

namespace Danmaku2D {

	[AddComponentMenu("Danmaku 2D/Danmaku Emitter")]
	public sealed class DanmakuEmitter : DanmakuTriggerReciever {

		[SerializeField]
		private DanmakuSource[] sources;

		[SerializeField]
		private FireBuilder fireData;

		[SerializeField]
		private Modifier modifier;

		[SerializeField]
		private DanmakuControlBehavior[] controllers;

		[SerializeField]
		private AudioClip fireSFX;

		[SerializeField]
		private float fireVolume = 1f;

		public FireModifier Modifier {
			get {
				if(modifier == null)
					return null;
				return modifier.WrappedModifier;
			}
		}

		public void Fire() {
			fireData.Controller = null;
			for(int i = 0; i < controllers.Length; i++)
				fireData.Controller += controllers[i].UpdateDanmaku;
			fireData.Modifier = Modifier;
			for(int i = 0; i < sources.Length; i++)
				sources[i].Fire (fireData);
			if (fireSFX != null)
				AudioManager.PlaySFX (fireSFX, fireVolume);
		}

		public void FireAtDanmaku(Danmaku danmaku) {
			FireBuilder copy = fireData.Clone ();
			copy.CoordinateSystem = DanmakuField.CoordinateSystem.World;
			copy.Position = danmaku.Position;
			copy.Rotation = danmaku.rotation;
			danmaku.Field.Fire(copy);
		}

		public void FireAtPoint(Vector2 position, DynamicFloat rotation, DanmakuField field) {
			FireBuilder copy = fireData.Clone ();
			copy.CoordinateSystem = DanmakuField.CoordinateSystem.World;
			copy.Position = position;
			copy.Rotation = rotation;
			field.Fire (copy);
		}

		#region implemented abstract members of DanmakuTriggerReciever

		public override void Trigger () {
			Fire ();
		}

		public override Color NodeColor {
			get {
				return Color.red;
			}
		}

		#endregion
	}
}