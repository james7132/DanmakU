using UnityEngine;
using System.Collections;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An abstract generic superclass to mirror the functionality of any implementor of IPorjectileGroupController in a ProjectileControlBehavior.
	/// It can be used with other ProjectileControlBehavior implementations; however it is best used with IProjectileGroupController implementations that don't derive from ProjectileControlBehavior.
	/// </summary>
	public abstract class ControllerWrapperBehavior<T> : ProjectileControlBehavior where T : IDanmakuController, new() {

		[SerializeField]
		private T projectileController;
	
		/// <summary>
		/// Gets the underlying IProjectileGroupController.
		/// </summary>
		/// <value>The underlying controller.</value>
		public T Controller {
			get {
				return projectileController;
			}
		}

		public override void Awake () {
			if (projectileController == null) {
				projectileController = new T();
			}
		}

		#region implemented abstract members of ProjectileControlBehavior
		public sealed override void UpdateProjectile (Danmaku projectile, float dt) {
			projectileController.UpdateProjectile(projectile, dt);
		}
		#endregion
		
	}
}