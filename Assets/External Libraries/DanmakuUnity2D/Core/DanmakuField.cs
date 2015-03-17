using UnityEngine;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	public abstract class DanmakuField : PausableGameObject {

		public enum CoordinateSystem { View, Relative, World }
		[SerializeField]
		private Vector2 playerSpawnLocation = new Vector2(0.5f, 0.25f);

		public abstract DanmakuField TargetField { get; }

		private DanmakuPlayer player;
		public DanmakuPlayer Player {
			get {
				return player;
			}
		}

		[SerializeField]
		private Camera fieldCamera;
		
		private Transform cameraTransform2D;
		public Transform CameraTransform2D {
			get {
				return cameraTransform2D;
			}
		}

		public void RecomputeWorldPoints() {
			Vector3 UL, BR;
			bottomLeft = fieldCamera.ViewportToWorldPoint (new Vector3(0f, 0f, gamePlaneDistance));
			UL = fieldCamera.ViewportToWorldPoint (Vector3.up);
			BR = fieldCamera.ViewportToWorldPoint (Vector3.right);
			x = (BR - bottomLeft);
			y = (UL - bottomLeft);
			z = transform.forward * gamePlaneDistance;

			scale = new Vector3 (x.magnitude, y.magnitude, gamePlaneDistance);
		}
	
		private Vector3 x, y, z, scale, bottomLeft;

		public float XSize {
			get { return scale.x; }
		}

		public float YSize {
			get { return scale.y; }
		}

		public Vector3 BottomLeft {
			get { return WorldPoint (new Vector3(0f, 0f, 0f)); }
		}

		public Vector3 BottomRight {
			get { return WorldPoint (new Vector3(1f, 0f, 0f)); }
		}

		public Vector3 TopLeft {
			get { return WorldPoint (new Vector3(0f, 1f, 0f)); }
		}

		public Vector3 TopRight {
			get { return WorldPoint (new Vector3(1f, 1f, 0f)); }
		}

		public Vector3 Center {
			get { return WorldPoint (new Vector3(0.5f, 0.5f, 0f)); }
		}

		public Vector3 Top {
			get { return WorldPoint (new Vector3 (0.5f, 1f, 0f)); }
		}

		public Vector3 Bottom {
			get { return WorldPoint (new Vector3 (0.5f, 0f, 0f));}
		}

		public Vector3 Right {
			get { return WorldPoint (new Vector3 (1f, 0.5f, 0f)); }
		}

		public Vector3 Left {
			get { return WorldPoint (new Vector3 (0f, 0.5f, 0f));}
		}

		public override void Awake () {
			base.Awake ();
			fieldCamera.orthographic = true;
			cameraTransform2D = fieldCamera.transform;
			RecomputeWorldPoints ();
		}

		[SerializeField]
		private float gamePlaneDistance = 10;
		public float GamePlaneDistance {
			get {
				return gamePlaneDistance;
			}
		}

		public Vector3 WorldPoint(Vector3 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point;
				case CoordinateSystem.Relative:
					return bottomLeft + point;
				default:
				case CoordinateSystem.View:
					return bottomLeft + point.x * x + point.y * y + point.z * z;
			}
		}

		public Vector3 RelativePoint(Vector3 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point - bottomLeft;
				case CoordinateSystem.Relative:
					return point;
				default:
				case CoordinateSystem.View:
					return point.x * x + point.y * y + point.z * z;
			}
		}

		public Vector3 ViewPoint(Vector3 point, CoordinateSystem coordSys = CoordinateSystem.World) {
			switch (coordSys) {
				default:
				case CoordinateSystem.World:
					Vector3 offset = point - bottomLeft;
					return new Vector3(
						Vector3.Project (offset, x).magnitude / scale.x,
						Vector3.Project (offset, y).magnitude / scale.y,
						Vector3.Project (offset, z).magnitude / scale.z);
				case CoordinateSystem.Relative:
					return new Vector3(
						Vector3.Project (point, x).magnitude / scale.x,
						Vector3.Project (point, y).magnitude / scale.y,
						Vector3.Project (point, z).magnitude / scale.z);
				case CoordinateSystem.View:
					return point;
			}
		}

		public float AngleTowardPlayer(Vector2 startLocation, CoordinateSystem coordinateSystem = CoordinateSystem.World) {
			return Util.AngleBetween2D (startLocation, Player.transform.position);
		}

		/// <summary>
		/// Spawns the player with the given controller
		/// </summary>
		/// <param name="character">Character prefab, defines character behavior and attack patterns.</param>
		/// <param name="controller">Controller for the player, allows for a user to manually control it or let an AI take over.</param>
		public DanmakuPlayer SpawnPlayer(DanmakuPlayer playerCharacter, CoordinateSystem coordSys = CoordinateSystem.View) {
			Vector3 spawnPos = WorldPoint((Vector3)playerSpawnLocation, coordSys);
			player =  (DanmakuPlayer) Instantiate(playerCharacter, spawnPos, Quaternion.identity);
			if(player != null) {
				player.Reset (5);
				player.transform.parent = transform;
				player.Field = this;
			}
			return player;
		}
		
		/// <summary>
		/// Spawns a projectile in the field.
		/// 
		/// If absoluteWorldCoord is set to false, location specifies a relative position in the field. 0.0 = left/bottom, 1.0 = right/top. Values greater than 1 or less than 0 spawn
		/// outside of of the camera view.
		/// </summary>
		/// <param name="prefab">Prefab for the spawned projectile, describes the visuals, size, and hitbox characteristics of the prefab.</param>
		/// <param name="location">The location within the field to spawn the projectile.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="absoluteWorldCoord">If set to <c>true</c>, <c>location</c> is in absolute world coordinates relative to the bottom right corner of the game plane.</param>
		public Projectile SpawnProjectile(ProjectilePrefab bulletType, Vector2 location, float rotation, CoordinateSystem coordSys = CoordinateSystem.View) {
			Projectile bullet = ProjectileManager.Get (bulletType);
			bullet.PositionImmediate = WorldPoint(location, coordSys);
			bullet.Rotation = rotation;
			bullet.Field = this;
			bullet.Activate ();
			return bullet;
		}

		public LinearProjectile FireLinearProjectile(ProjectilePrefab bulletType, 
		                                         Vector2 location, 
		                                         float rotation, 
		                                         float velocity,
		                                         CoordinateSystem coordSys = CoordinateSystem.View) {
			LinearProjectile linearProjectile = new LinearProjectile (velocity);
			Projectile bullet = ProjectileManager.Get (bulletType);
			bullet.PositionImmediate = WorldPoint(location, coordSys);
			bullet.Rotation = rotation;
			bullet.Field = this;
			bullet.Activate ();
			bullet.Controller = linearProjectile;
			return linearProjectile;
		}
		
		public CurvedProjectile FireCurvedProjectile(ProjectilePrefab bulletType,
		                                         Vector2 location,
		                                         float rotation,
		                                         float velocity,
		                                         float angularVelocity,
		                                         CoordinateSystem coordSys = CoordinateSystem.View) {
			CurvedProjectile curvedProjectile = new CurvedProjectile (velocity, angularVelocity);
			Projectile bullet = ProjectileManager.Get (bulletType);
			bullet.PositionImmediate = WorldPoint(location, coordSys);
			bullet.Rotation = rotation;
			bullet.Field = this;
			bullet.Activate ();
			bullet.Controller = curvedProjectile;
			return curvedProjectile;
		}
		
		public void FireControlledProjectile(ProjectilePrefab bulletType, 
		                                 Vector2 location, 
		                                 float rotation, 
		                                 IProjectileController controller,
		                                 CoordinateSystem coordSys = CoordinateSystem.View) {
			Projectile bullet = ProjectileManager.Get (bulletType);
			bullet.PositionImmediate = WorldPoint(location, coordSys);
			bullet.Rotation = rotation;
			bullet.Field = this;
			bullet.Activate ();
			bullet.Controller = controller;
		}

		public void SpawnEnemy(Enemy prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Enemy enemy = (Enemy)Instantiate(prefab);
			Transform transform = enemy.transform;
			transform.position = WorldPoint((Vector3)location, coordSys);
			enemy.Field = this;
		}
	}
}
