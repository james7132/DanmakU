// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	public delegate void DanmakuEvent (Danmaku danmaku);

	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a bullet hell game.
	/// </summary>
	public sealed partial class Danmaku {

		/// <summary>
		/// The supported collider shapes used by danmaku.
		/// </summary>
		public enum ColliderType { 
			Circle, 
			Box, 
			Point, 
			Line 
		}
		
		#region Private and Internal Fields

		/// <summary>
		/// The Danmaku instance's index within the DanmakuPool.
		/// </summary>
		internal int poolIndex;
		//internal int renderIndex;

		internal Vector2 direction;

		//Cached information about the Danmaku from its prefab
		internal ColliderType colliderType = ColliderType.Circle;
		internal Vector2 colliderOffset = Vector2.zero; 
		internal Vector2 colliderSize = Vector2.zero;
		private float sizeSquared;
		internal int layer;
		internal int frames;
		internal float time;

		//Prefab information
		private DanmakuPrefab prefab;
		private DanmakuPrefab runtime;

		//Collision related variables
		private int colliderMask;

		private bool to_deactivate;

		internal HashSet<DanmakuGroup> groups;

		internal Vector3 position;
		internal float rotation;

		//Preallocated variables to avoid allocation in Update
		private Vector2 originalPosition;
		private RaycastHit2D[] raycastHits;
		private Vector2 collisionCenter;

		//Cached check for controllers to avoid needing to calculate them in Update
		private bool controllerCheck;

		#endregion

		/// <summary>
		/// Occurs when the Danmaku instance is activated.
		/// </summary>
		public event DanmakuEvent OnActivate;

		/// <summary>
		/// Occurs when the Danmaku instance is deactivated.
		/// </summary>
		public event DanmakuEvent OnDeactivate;

		private DanmakuController onUpdate;

		public event DanmakuController ControllerUpdate {
			add {
				onUpdate += value;
				controllerCheck = onUpdate != null;
			}
			remove {
				onUpdate -= value;
				controllerCheck = onUpdate != null;
			}
		}
		
		#region Public Properties

		/// <summary>
		/// The vertex color to use when rendering
		/// </summary>
		public Color32 Color {
			get;
			set;
		}

		public int Damage {
			get;
			set;
		}

		/// <summary>
		/// Whether or not to perform collision detection with the Danmaku instance.
		/// </summary>
		public bool CollisionCheck {
			get;
			set;
		}

		public float Speed {
			get;
			set;
		}

		public float AngularSpeed {
			get;
			set;
		}

		public DanmakuPrefab Prefab {
			get {
				return runtime;
			}
		}

		public Sprite Sprite {
			get {
				return runtime.Sprite;
			}
		}

		public Mesh Mesh {
			get {
				return runtime.Mesh;
			}
		}

		public Material Material {
			get {
				//return material;
				return runtime.Material;
			}
		}

		/// <summary>
		/// Gets or sets the position, in world space, of the projectile.
		/// </summary>
		/// <value>The position of the projectile.</value>
		public Vector2 Position {
			get {
				return position;
			}
			set {
				position.x = value.x;
				position.y = value.y;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the projectile, in degrees.
		/// </summary>
		/// <remarks>
		/// If viewed from a unrotated orthographic camera:
		/// 0 - Straight up
		/// 90 - Straight Left
		/// 180 - Straight Down
		/// 270 -  Straight Right
		/// </remarks>
		/// <value>The rotation of the bullet in degrees.</value>
		public float Rotation {
			get {
				return rotation;
			}
			set {
				rotation = value;
				direction = UnitCircle(value);
			}
		}
		
		/// <summary>
		/// Gets the direction vector the projectile is facing.
		/// </summary>
		/// <remarks>
		/// It is a unit vector.
		/// Changing <see cref="Rotation"/> will change this vector.
		/// </remarks>
		/// <value>The direction vector the projectile is facing toward.</value>
		public Vector2 Direction {
			get {
				return direction;
			}
			set {
				direction = value.normalized;
				rotation = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90f;
			}
		}

		public float Scale {
			get;
			set;
		}

		/// <summary>
		/// The amount of time that has passed since this bullet has been fired.
		/// </summary>
		/// <value>The time since the projectile has been fired, in seconds.</value>
		public float Time {
			get {
				return time;
			}
		}
		
		/// <summary>
		/// The number of framesfieldBoundshave passed since this bullet has been fired.
		/// </summary>
		/// <value>The number of frames that have passed since this bullet has been fired.</value>
		public int Frames {
			get {
				return frames;
			}
		}
		
		/// <summary>
		/// Gets the instance's tag.
		/// </summary>
		/// <remarks>
		/// This is initialzied to the tag on the DanmakuPrefab, but can be changed to any string.
		/// </remarks>
		/// <value>The tag of the projectile.</value>
		public string Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the instance's layer.
		/// </summary>
		/// <remarks>
		/// Unlike GameObject's layers, this layer value only affects collision behavior.
		/// </remarks>
		/// <value>The layer used for collision detection.</value>
		public int Layer {
			get {
				return layer;
			}
			set {
				layer = value;
				colliderMask = collisionMask[layer];
			}
		}
		
		public DanmakuField Field {
			get;
			set;
		}

		#endregion

		public void AddController(IDanmakuController controller) {
			ControllerUpdate += controller.Update;
		}

		public void AddController(DanmakuController controller) {
			ControllerUpdate += controller;
		}

		public void RemoveController(IDanmakuController controller) {
			ControllerUpdate -= controller.Update;
		}

		public void RemoveController(DanmakuController controller) {
			ControllerUpdate -= controller;
		}

		public void ClearControllers() {
			controllerCheck = true;
			onUpdate = null;
		}

		#region Position Functions

		/// <summary>
		/// Moves the bullet closer to the specified target point.
		/// 
		/// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
		/// </summary>
		/// <param name="target">The target position to move towards in absolute world coordinates.</param>
		/// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
		public void MoveTowards (Vector2 target, float maxDistanceDelta) {
			Position = Vector2.MoveTowards (position, target, maxDistanceDelta);
		}

		/// <summary>
		/// Moves the bullet closer to the specified target Transform's position.
		/// 
		/// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the target Transform is null.</exception>
		/// <param name="target">The Transform of the object to move towards.</param>
		/// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
		public void MoveTowards (Transform target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException();
			Position = Vector2.MoveTowards (position, target.position, maxDistanceDelta);
		}

		/// <summary>
		/// Moves the bullet closer to the specified target Component's position.
		/// 
		/// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the target Component is null.</exception>
		/// <param name="target">The Component of the object to move towards.</param>
		/// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
		public void MoveTowards (Component target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException();
			Position = Vector2.MoveTowards (position, target.transform.position, maxDistanceDelta);
		}

		/// <summary>
		/// Moves the bullet closer to the specified target GameObject's position.
		/// 
		/// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the target GameObject is null.</exception>
		/// <param name="target">The GameObject of the object to move towards.</param>
		/// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
		public void MoveTowards (GameObject target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException();
			Position = Vector2.MoveTowards (position, target.transform.position, maxDistanceDelta);
		}

		public void Translate (Vector2 deltaPos) {
			Position += deltaPos;
		}

		#endregion

		#region Rotation Functions

		public void Rotate (float deltaTheta) {
			Rotation += deltaTheta;
		}

		#endregion

		#region Speed Functions

		public void Accelerate(DynamicFloat dv) {
			Speed += dv;
		}

		#endregion

		#region Angular Speed Functions 

		public void AngularAcclerate(DynamicFloat dav) {
			AngularSpeed += dav;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="DanmakU.Danmaku"/> class.
		/// </summary>
		internal Danmaku() {
			groups = new HashSet<DanmakuGroup> ();
			raycastHits = new RaycastHit2D[5];
		}

		internal void Update() {

			int i, j, count;
			originalPosition.x = position.x;
			originalPosition.y = position.y;
			Vector2 movementVector;

			if (controllerCheck) {
				onUpdate(this, dt);
			}

			if(AngularSpeed != 0f) {
				float rotationChange = AngularSpeed * dt;
				rotation += rotationChange;
				direction = UnitCircle(rotation);
			}

			if (Speed != 0) {
				float movementChange = Speed * dt;
				position.x += direction.x * movementChange;
				position.y += direction.y * movementChange;
			}
			
			movementVector.x = position.x - originalPosition.x;
			movementVector.y = position.y - originalPosition.y;

			//Debug.DrawRay(originalPosition, movementVector);

			if(CollisionCheck) {
				float sqrDistance = movementVector.sqrMagnitude;
				float cx = colliderOffset.x;
				float cy = colliderOffset.y;
				if(cx == 0 && cy == 0) {
					collisionCenter = originalPosition;
				} else {
					float c = direction.x;
					float s = direction.y;
					collisionCenter.x = originalPosition.x + c * cx - s * cy;
					collisionCenter.y = originalPosition.y + s * cx + c * cy;
				}
				//Check if the collision detection should be continuous or not
				count = 0;
				switch(colliderType) {
					default:
					case ColliderType.Point:
						if(sqrDistance > sizeSquared || Physics2D.OverlapPoint(collisionCenter, colliderMask) != null) {
							count = Physics2D.RaycastNonAlloc(collisionCenter,
							                                  movementVector,
							                                  raycastHits,
							                                  Mathf.Sqrt(sqrDistance),
							                                  colliderMask);
						}
						break;
					case ColliderType.Line:
						float length = Mathf.Sqrt(sqrDistance) + colliderSize.x;
						if(sqrDistance > sizeSquared || Physics2D.Raycast(collisionCenter, movementVector, length, colliderMask).collider != null) {
							count = Physics2D.RaycastNonAlloc(collisionCenter,
							                                  movementVector,
							                                  raycastHits,
							                                  Mathf.Sqrt(sqrDistance) + colliderSize.x,
							                                  colliderMask);
						}
						break;
					case ColliderType.Circle:
						if(sqrDistance > sizeSquared || Physics2D.OverlapCircle(collisionCenter, colliderSize.x, colliderMask) != null) {
							count = Physics2D.CircleCastNonAlloc(collisionCenter, 
								                                 colliderSize.x,
							                                     movementVector,
							                                     raycastHits,
							                                     sqrDistance,
							                                     colliderMask);
						}
						break;
					case ColliderType.Box:
						count = Physics2D.BoxCastNonAlloc(collisionCenter,
					                                      colliderSize,
					                                      rotation,
					                                      movementVector,
					                                  	  raycastHits,
					                                      colliderMask);
						break;
				}
				if(count > 0) {
					IDanmakuCollider[] scripts;
					for (i = 0; i < count; i++) {
						RaycastHit2D hit = raycastHits [i];
						Collider2D collider = hit.collider;
						if(collider == null)
							continue;
						if(colliderMap.ContainsKey(collider)) {
							scripts = colliderMap[collider];
							if(scripts == null) {
								scripts = Util.GetComponents<IDanmakuCollider>(collider);
								colliderMap[collider] = scripts;
							}
						} else {
							scripts = Util.GetComponents<IDanmakuCollider>(collider);
							colliderMap[collider] = scripts;
						}
						for (j = 0; j < scripts.Length; j++) {
							scripts [j].OnDanmakuCollision (this, hit);
						}
						if (to_deactivate) {
							position.x = hit.point.x;
							position.y = hit.point.y;
							DeactivateImmediate();
							return;
						}
					}
				}
			}

			if (!is_active || (Field != null && !Field.bounds.Contains (position))) {
				DeactivateImmediate();
				return;
			}
			
			frames++;
			time += dt;
		}

		public void MatchPrefab(DanmakuPrefab prefab) {
			if (prefab == null) {
				Debug.LogError("Tried to match a null prefab");
				return;
			}
			if (this.prefab != prefab) {
				this.prefab = prefab;

				if(is_active) {
					runtime.Remove(this);
					runtime = prefab.GetRuntime ();
					runtime.Add(this);
				} else {
					runtime = prefab.GetRuntime ();
				}

				Vector2 scale = runtime.cachedScale;
				colliderType = runtime.collisionType;
				switch(colliderType) {
					default:
					case ColliderType.Point:
						colliderSize = Vector2.zero;
						sizeSquared = 0;
						break;
					case ColliderType.Circle:
						colliderSize = runtime.colliderSize * scale.Max();
						break;
					case ColliderType.Line:
						colliderSize = runtime.colliderSize;
						break;
				}
				sizeSquared = colliderSize.y * colliderSize.y;
				colliderOffset = scale.Hadamard2(runtime.colliderOffset);

			}

			Tag = runtime.cachedTag;

			Color = runtime.Color;
			Scale = 1f;
			layer = runtime.cachedLayer;
			colliderMask = collisionMask [layer];

			AddController (runtime.ExtraControllers);
		}

		internal bool is_active;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active.
		/// </summary>
		/// <remarks>
		/// Setting it to true while inactive is equal to calling Activate.
		/// Setting it to false while active is equal to calling DeactivateImmediate.
		/// </remarks>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive {
			get {
				return is_active;
			}
			set {
				if(is_active) {
					if(!value)
						DeactivateImmediate();
				} else {
					if(value)
						Activate();
				}
				is_active = value;
			}
		}

		public static implicit operator FireData(Danmaku danmaku) {
			FireData data = new FireData();
			data.Position = danmaku.Position;
			data.Rotation = danmaku.Rotation;
			data.AngularSpeed = danmaku.AngularSpeed;
			data.Speed = danmaku.Speed;
			data.Prefab = danmaku.Prefab;
			data.Controller = danmaku.onUpdate;
			data.Damage = danmaku.Damage;
			data.Field = danmaku.Field;
			return data;
		}

		/// <summary>
		/// Fires a single bullet from the bullet's current position.
		/// </summary>
		/// <remarks>
		/// By default, firing using this method also uses the rotation of the bullet to fire the bullet.
		/// Set <c>useRotation</c> to false to disable this.
		/// </remarks>
		/// <param name="data">the data used to create the .</param>
		/// <param name="useRotation">If set to <c>true</c>, the bullet will use the current rotation of the bullet to fire with.</param>
		public Danmaku Fire (FireData data, bool useRotation = true) {
			Vector2 tempPos = data.Position;
			DynamicFloat tempRot = data.Rotation;
			data.Position = Position;
			if(useRotation)
				data.Rotation = Rotation;
			Danmaku danmaku = data.Fire();
			data.Position = tempPos;
			data.Rotation = tempRot;
			return danmaku;
		}

		public void Fire (FireBuilder builder, bool useRotation = true) {
			Vector2 tempPos = builder.Position;
			DynamicFloat tempRot = builder.Rotation;
			builder.Position = Position;
			if(useRotation)
				builder.Rotation = Rotation;
			builder.Fire();
			builder.Position = tempPos;
			builder.Rotation = tempRot;
		}

		/// <summary>
		/// Activates the Danmaku instance.
		/// </summary>
		/// <remarks>
		/// Calling this on a already active instance does nothing.
		/// Calling this on a instance marked for deactivation will unmark the projectile and keep it from deactivating.
		/// </remarks>
		public void Activate () {
			to_deactivate = false;
			runtime.Add(this);
			if (!is_active && OnActivate != null)
				OnActivate (this);
			is_active = true;
			frames = 0;
			time = 0f;
		}
		
		/// <summary>
		/// Marks the instance for deactivation.
		/// </summary>
		/// <remarks>
		/// Deactivated bullets are removed from the active set, and all 
		/// The instance  removed from the active set and all bullet functionality will cease after current 
		/// If Danmaku needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
		/// </remarks>
		public void Deactivate()  {
			to_deactivate = true;
		}

		/// <summary>
		/// Immediately deactivates this Danmaku and ceases all processing done on it.
		/// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
		/// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
		/// </summary>
		public void DeactivateImmediate() {
			if (is_active && OnDeactivate != null)
				OnDeactivate (this);

			onUpdate = null;
			OnActivate = null;
			OnDeactivate = null;
			Field = null;
			controllerCheck = false;
			Damage = 0;
			runtime.Remove(this);
			CollisionCheck = true;
			is_active = false;
			danmakuPool.Return (this);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="DanmakU.Danmaku"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode () {
			return poolIndex;
		}
	}
}
