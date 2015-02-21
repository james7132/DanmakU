using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

public enum FieldCoordinateSystem { Screen, FieldRelative, AbsoluteWorld }

public abstract class AbstractDanmakuField : CachedObject {
	/// <summary>
	/// The player spawn location.
	/// </summary>
	[SerializeField]
	private Vector2 playerSpawnLocation = new Vector2(0.5f, 0.25f);

	public abstract AbstractDanmakuField TargetField { get; }

	private AbstractDanmakuGameController gameController;
	public AbstractDanmakuGameController GameController {
		get {
			if(gameController == null) {
				gameController = FindObjectOfType<AbstractDanmakuGameController>();
			}
			return gameController;
		}
	}

	private AbstractPlayableCharacter player;
	public AbstractPlayableCharacter Player {
		get {
			return player;
		}
	}

	public ProjectilePool BulletPool {
		get {
			return GameController.BulletPool;
		}
	}
	
	/// <summary>
	/// The field camera.
	/// </summary>
	[SerializeField]
	private Camera fieldCamera;
	
	private Transform cameraTransform;
	/// <summary>
	/// Gets or sets the camera transform.
	/// </summary>
	/// <value>The camera transform.</value>
	public Transform CameraTransform {
		get {
			return cameraTransform;
		}
		set {
			cameraTransform = value;
		}
	}

	/// <summary>
	/// Recomputes the bounding area for 
	/// </summary>
	public void RecomputeWorldPoints() {
		bottomLeft = fieldCamera.ViewportToWorldPoint (Vector3.zero);
		Vector3 UL = fieldCamera.ViewportToWorldPoint (Vector3.up);
		Vector3 BR = fieldCamera.ViewportToWorldPoint (Vector3.right);
		xDirection = (BR - bottomLeft);
		yDirection = (UL - bottomLeft);
		zDirection = Vector3.Cross (xDirection, yDirection).normalized;
	}

	private Vector3 xDirection;
	private Vector3 yDirection;
	private Vector3 zDirection;
	private Vector3 bottomLeft;
	/// <summary>
	/// The game plane distance.
	/// </summary>
	[SerializeField]
	private float gamePlaneDistance = 10;
	/// <summary>
	/// Gets the size of the X.
	/// </summary>
	/// <value>The size of the X.</value>
	public float XSize {
		get { return xDirection.magnitude; }
	}
	
	/// <summary>
	/// Gets the size of the Y.
	/// </summary>
	/// <value>The size of the Y.</value>
	public float YSize {
		get { return yDirection.magnitude; }
	}
	
	/// <summary>
	/// Gets the bottom left.
	/// </summary>
	/// <value>The bottom left.</value>
	public Vector3 BottomLeft {
		get { return WorldPoint (new Vector3(0f, 0f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the bottom right.
	/// </summary>
	/// <value>The bottom right.</value>
	public Vector3 BottomRight {
		get { return WorldPoint (new Vector3(1f, 0f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the top left.
	/// </summary>
	/// <value>The top left.</value>
	public Vector3 TopLeft {
		get { return WorldPoint (new Vector3(0f, 1f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the top right.
	/// </summary>
	/// <value>The top right.</value>
	public Vector3 TopRight {
		get { return WorldPoint (new Vector3(1f, 1f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the center.
	/// </summary>
	/// <value>The center.</value>
	public Vector3 Center {
		get { return WorldPoint (new Vector3(0.5f, 0.5f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the top.
	/// </summary>
	/// <value>The top.</value>
	public Vector3 Top {
		get { return WorldPoint (new Vector3 (0.5f, 1f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the bottom.
	/// </summary>
	/// <value>The bottom.</value>
	public Vector3 Bottom {
		get { return WorldPoint (new Vector3 (0.5f, 0f, gamePlaneDistance));}
	}
	
	/// <summary>
	/// Gets the right.
	/// </summary>
	/// <value>The right.</value>
	public Vector3 Right {
		get { return WorldPoint (new Vector3 (1f, 0.5f, gamePlaneDistance)); }
	}
	
	/// <summary>
	/// Gets the left.
	/// </summary>
	/// <value>The left.</value>
	public Vector3 Left {
		get { return WorldPoint (new Vector3 (0f, 0.5f, gamePlaneDistance));}
	}

	/// <summary>
	/// Worlds the point.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="fieldPoint">Field point.</param>
	public Vector3 WorldPoint(Vector3 fieldPoint) {
		return bottomLeft + Relative2Absolute (fieldPoint);
	}
	
	/// <summary>
	/// Fields the point.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="worldPoint">World point.</param>
	public Vector3 FieldPoint(Vector3 worldPoint) {
		Vector3 offset = worldPoint - bottomLeft;
		return new Vector3 (Vector3.Project (offset, xDirection).magnitude, Vector3.Project (offset, yDirection).magnitude, Vector3.Project (offset, zDirection).magnitude);
	}
	
	/// <summary>
	/// Relative2s the absolute.
	/// </summary>
	/// <returns>The absolute.</returns>
	/// <param name="relativeVector">Relative vector.</param>
	public Vector3 Relative2Absolute(Vector3 relativeVector) {
		return relativeVector.x * xDirection + relativeVector.y * yDirection + relativeVector.z * zDirection;
	}

	/// <summary>
	/// Gets the lives remaining.
	/// </summary>
	/// <value>The lives remaining.</value>
	public int LivesRemaining {
		get {
			if(player != null) {
				return player.LivesRemaining;
			} else {
				Debug.Log("Player Field without Player");
				return int.MinValue;
			}
		}
	}
	
	/// <summary>
	/// Gets the player position.
	/// </summary>
	/// <value>The player position.</value>
	public Vector3 PlayerPosition {
		get {
			if(player != null) {
				return player.Transform.position;
			} else {
				Debug.Log("Player Field without Player");
				return Vector3.zero;
			}
		}
	}
	
	/// <summary>
	/// Angles the toward player.
	/// </summary>
	/// <returns>The toward player.</returns>
	/// <param name="startLocation">Start location.</param>
	public float AngleTowardPlayer(Vector3 startLocation) {
		return Util.AngleBetween2D (startLocation, PlayerPosition);
	}

	public override void Awake () {
		base.Awake ();
		fieldCamera.orthographic = true;
		cameraTransform = fieldCamera.transform;
		RecomputeWorldPoints ();
	}

	/// <summary>
	/// Spawns the player with the given controller
	/// </summary>
	/// <param name="character">Character prefab, defines character behavior and attack patterns.</param>
	/// <param name="controller">Controller for the player, allows for a user to manually control it or let an AI take over.</param>
	public AbstractPlayableCharacter SpawnPlayer(AbstractPlayableCharacter playerCharacter) {
		Vector3 spawnPos = WorldPoint(Util.To3D(playerSpawnLocation, gamePlaneDistance));
		player =  (AbstractPlayableCharacter) Instantiate(playerCharacter, spawnPos, Quaternion.identity);
		if(player != null) {
			player.Reset (1);
			player.Transform.parent = Transform;
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
	/// <param name="extraControllers">Extra ProjectileControllers to change the behavior of the projectile.</param>
	public Projectile SpawnProjectile(ProjectilePrefab prefab, Vector2 location, float rotation, FieldCoordinateSystem coordSys = FieldCoordinateSystem.Screen, ProjectileController[] extraControllers = null) {
		Vector3 worldLocation = Vector3.zero;
		switch(coordSys) {
			case FieldCoordinateSystem.Screen:
				worldLocation = WorldPoint(new Vector3(location.x, location.y, gamePlaneDistance));
				break;
			case FieldCoordinateSystem.FieldRelative:
				worldLocation = BottomLeft + new Vector3(location.x, location.y, 0f);
				break;
			case FieldCoordinateSystem.AbsoluteWorld:
				worldLocation = location;
				break;
		}
		Projectile projectile = (Projectile)BulletPool.Get (prefab);
		projectile.Transform.position = worldLocation;
		projectile.Transform.rotation = Quaternion.Euler(0f, 0f, rotation);
		projectile.Activate ();
		return projectile;
	}
	
	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	/// <param name="fieldLocation">Field location.</param>
	public void SpawnEnemy(AbstractEnemy prefab, Vector2 fieldLocation) {
		AbstractEnemy enemy = (AbstractEnemy)Instantiate(prefab, WorldPoint (Util.To3D (fieldLocation, gamePlaneDistance)), Quaternion.identity);
		enemy.Field = this;
	}
	
	/// <summary>
	/// Gets all bullets.
	/// </summary>
	/// <returns>The all bullets.</returns>
	/// <param name="position">Position.</param>
	/// <param name="radius">Radius.</param>
	/// <param name="layerMask">Layer mask.</param>
	public Projectile[] GetAllBullets(Vector3 position, float radius, int layerMask = 1 << 14) {
		Collider2D[] hits = Physics2D.OverlapCircleAll (position, radius, layerMask);
		List<Projectile> projectiles = new List<Projectile> ();
		for (int i = 0; i < hits.Length; i++) {
			Projectile proj = hits[i].GetComponent<Projectile>();
			if(proj != null) {
				projectiles.Add(proj);
			}
		}
		return projectiles.ToArray ();
	}
}
