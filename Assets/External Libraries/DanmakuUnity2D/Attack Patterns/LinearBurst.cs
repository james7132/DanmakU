using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.AttackPatterns {

	/// <summary>
	/// Circular linear burst.
	/// </summary>
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Linear Burst")]
	public class LinearBurst : Burst {

		[SerializeField]
		private LinearProjectile LinearController;

		#region implemented abstract members of Burst

		protected override IProjectileGroupController BurstController {
			get {
				return LinearController;
			}
		}

		#endregion
	}
}