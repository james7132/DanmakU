using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace Danmaku2D.AttackPatterns {

	/// <summary>
	/// A Burst implementation that uses LinearProjectile as a projectile controller.
	/// </summary>
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Linear Burst")]
	public class LinearBurst : Burst {

		[SerializeField]
		private LinearProjectile LinearController;

		#region implemented abstract members of Burst

		protected override IProjectileController BurstController {
			get {
				return LinearController;
			}
		}

		#endregion
	}
}