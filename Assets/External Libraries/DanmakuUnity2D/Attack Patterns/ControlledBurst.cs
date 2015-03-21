using UnityEngine;
using System.Collections;
using Danmaku2D.ProjectileControllers;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace Danmaku2D.AttackPatterns {
	
	/// <summary>
	/// A Burst implementation that uses CurvedProjectile as a projectile controller
	/// </summary>
	[AddComponentMenu("Danmaku 2D/Attack Patterns/Controlled Burst")]
	public class ControlledBurst : Burst {

		[SerializeField]
		private ProjectileControlBehavior controller;
		
		#region implemented abstract members of Burst
		
		protected override IProjectileController BurstController {
			get {
//				if(controller is ControllerWrapperBehavior<ProjectileControlBehavior>) {
//					return (controller as ControllerWrapperBehavior<ProjectileControlBehavior>).Controller;
//				}
				return controller;
			}
		}
		
		#endregion
	}
}