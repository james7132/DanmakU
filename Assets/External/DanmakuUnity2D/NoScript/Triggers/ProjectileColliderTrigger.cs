using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D  {

	[RequireComponent(typeof(Collider2D)), AddComponentMenu("Danmaku 2D/Triggers/Projectile Collider Trigger")]
	public class ProjectileColliderTrigger : DanmakuTrigger, IDanmakuCollider {

		[SerializeField]
		private List<string> tagFilter;

		#region IDanmakuCollider implementation
		public void OnDanmakuCollision (Danmaku proj) {
			for(int i = 0; i < tagFilter.Count; i++) {
				if(proj.CompareTag(tagFilter[i])) {
					Trigger();
					break;
				}
			}
		}
		#endregion




	}
}
