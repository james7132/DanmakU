using UnityEngine;
using System.Collections;

namespace Danmaku2D.AttackPatterns {
	
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Curved Burst")]
	public class CurvedBurst : Burst {

		[SerializeField]
		private CurvedProjectile CurvedController;

		#region implemented abstract members of Burst

		protected override IProjectileGroupController BurstController {
			get {
				return CurvedController;
			}
		}

		#endregion
	}
}