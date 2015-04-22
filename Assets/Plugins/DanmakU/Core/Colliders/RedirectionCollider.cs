using UnityEngine;
using UnityUtilLib;

namespace DanmakU {

	[AddComponentMenu("DanmakU/Colliders/Redirection Collider")]
	public class RedirectionCollider : DanmakuCollider {

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private DynamicFloat angle;

		private DanmakuGroup affected;

		public override void Awake () {
			base.Awake ();
			affected = new DanmakuGroup ();
		}

		#region implemented abstract members of DanmakuCollider
		protected override void ProcessDanmaku (Danmaku danmaku) {
			if (affected.Contains (danmaku))
				return;
			float baseAngle = angle.Value;
			switch(rotationMode) {
			case RotationMode.Relative:
				baseAngle += danmaku.Rotation;
				break;
			case RotationMode.Player:
				baseAngle += danmaku.Field.AngleTowardPlayer(danmaku.Position);
				break;
			case RotationMode.Absolute:
				break;
			}
			danmaku.Rotation = baseAngle;
			affected.Add (danmaku);
		}
		#endregion

	}

}
