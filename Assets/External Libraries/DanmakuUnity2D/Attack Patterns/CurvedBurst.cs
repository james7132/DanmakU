using UnityEngine;
using System.Collections;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace Danmaku2D.AttackPatterns {

	/// <summary>
	/// A Burst implementation that uses CurvedProjectile as a projectile controller
	/// </summary>
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Curved Burst")]
	public class CurvedBurst : Burst {

		[SerializeField]
		private CurvedProjectile CurvedController;

		#region implemented abstract members of Burst

		protected override IProjectileController BurstController {
			get {
				return CurvedController;
			}
		}

		#endregion
	}
}