using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D  {

	[RequireComponent(typeof(Collider2D))]
	public class ProjectileColliderTrigger : DanmakuTrigger, IDanmakuCollider {

		[SerializeField]
		private List<string> tagFilter;

		#region IDanmakuCollider implementation
		public void OnProjectileCollision (Danmaku proj) {
			for(int i = 0; i < tagFilter.Count; i++) {
				if(proj.CompareTag(tagFilter[i])) {
					FireTrigger();
					break;
				}
			}
		}
		#endregion




	}
}
