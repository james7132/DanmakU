using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {

	public abstract class ProjectileEmitter : PausableGameObject {

		public DanmakuField field;
		public ProjectilePrefab prefab;

		[SerializeField]
		private FrameCounter delay;

		public override void NormalUpdate () {
			if(delay.Tick() && prefab != null) {
//				Fire(prefab, controller);
			}
		}

//		public abstract void FireSingle(
//
//		public void FireSingle(ProjectilePrefab prefab, IProjectileCollider controller) {
//			if (field != null) {
//				field.FireControlledProjectile(prefab, transform.position, transform.rotation.eulerAngles.z, controller, DanmakuField.CoordinateSystem.World);
//			}
//		}
	}
}