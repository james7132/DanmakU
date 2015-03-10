using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public class ProjectileManager : SingletonBehavior<ProjectileManager>, IPausable {

		private static BasicPool<Projectile> projectilePool;

		[SerializeField]
		private int initialCount = 1000;

		[SerializeField]
		private int spawnOnEmpty = 1000;

		public override void Awake () {
			base.Awake ();
			projectilePool = new BasicPool<Projectile> (initialCount, spawnOnEmpty);
		}

		public int TotalCount {
			get {
				return (projectilePool != null) ? projectilePool.TotalCount : 0;
			}
		}

		public int ActiveCount {
			get {
				return (projectilePool != null) ? projectilePool.ActiveCount : 0;
			}
		}

		public void Update() {
			if (!Paused)
				NormalUpdate ();
		}

		public virtual void NormalUpdate () {
			Projectile[] active = projectilePool.Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].Update();
			}
		}

		public static void DeactivateAll() {
			Projectile[] active = projectilePool.Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].DeactivateImmediate();
			}
		}

		public static Projectile Get (ProjectilePrefab projectileType) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (projectileType);
			return proj;
		}

		public static Projectile Spawn(ProjectilePrefab bulletType, Vector2 location, float rotation) {
			Projectile bullet = Get (bulletType);
			bullet.Position = location;
			bullet.Rotation = rotation;
			bullet.Activate ();
			return bullet;
		}

		public static LinearProjectile FireLinearProjectile(ProjectilePrefab bulletType, 
		                                            Vector2 location, 
		                                            float rotation, 
		                                            float velocity) {
			LinearProjectile linearProjectile = new LinearProjectile (velocity);
			FireControlledProjectile (bulletType, location, rotation, linearProjectile);
			return linearProjectile;
		}
		
		public static CurvedProjectile FireCurvedProjectile(ProjectilePrefab bulletType,
		                                            Vector2 location,
		                                            float rotation,
		                                            float velocity,
		                                            float angularVelocity) {
			CurvedProjectile curvedProjectile = new CurvedProjectile (velocity, angularVelocity);
			FireControlledProjectile (bulletType, location, rotation, curvedProjectile);
			return curvedProjectile;
		}
		
		public static void FireControlledProjectile(ProjectilePrefab bulletType, 
				                                    Vector2 location, 
				                                    float rotation, 
				                                    IProjectileController controller) {
			Projectile bullet = Spawn (bulletType, location, rotation);
			bullet.Controller = controller;
		}

		#region IPausable implementation

		public bool Paused {
			get;
			set;
		}

		#endregion
	}
}