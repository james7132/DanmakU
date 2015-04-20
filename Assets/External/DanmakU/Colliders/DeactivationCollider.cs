using UnityEngine;
using System.Collections;

namespace DanmakU {

	public class DeactivationCollider : DanmakuCollider {

		#region implemented abstract members of DanmakuCollider
		protected override void ProcessDanmaku (Danmaku danmaku) {
			danmaku.Deactivate ();
		}
		#endregion
		
	}
}