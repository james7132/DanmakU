using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;
using System.Collections.Generic;
using System;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Danmaku : IPooledObject, IColorable, IPrefabed<DanmakuPrefab> {
		
		internal int index;

		private GameObject gameObject;
		private Transform transform;
		private SpriteRenderer renderer;

		internal float rotation;
		internal Vector2 direction;

		//Cached information about the Projectile from its prefab
		internal Vector2 circleCenter = Vector2.zero; 
		private float circleRaidus = 1f;
		internal Sprite sprite;
		internal Material material;
		internal Color color;
		internal string tag;
		internal int layer;
		internal int frames;
		internal string cachedTag;
		internal int cachedLayer;
		internal bool symmetric;

		//Prefab information
		private DanmakuPrefab prefab;
		private DanmakuPrefab runtime;

		//Collision related variables
		private int colliderMask;

		private bool to_deactivate;
		
		private DanmakuController controllerUpdate;
		internal List<DanmakuGroup> groups;
		private DanmakuField field;
		private Bounds2D bounds;

		//Preallocated variables to avoid allocation in Update
		private int count, count2;
		private float distance;
		private Vector2 originalPosition, movementVector;
		private IDanmakuCollider[] scripts;
		private RaycastHit2D[] raycastHits;
		private Collider2D[] colliders;

		//Cached check for controllers to avoid needing to calculate them in Update
		internal bool groupCheck;
		private bool controllerCheck;
		internal int groupCountCache;

		public float Velocity;
		public float AngularVelocity;

		public DanmakuPrefab Prefab {
			get {
				return runtime;
			}
		}

		/// <summary>
		/// Gets or sets the damage this projectile does to entities.
		/// Generally speaking, this is only used for projectiles fired by the player at enemies
		/// </summary>
		/// <value>The damage this projectile does.</value>
		public int Damage;
		
		/// <summary>
		/// Gets the renderer sprite of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-sprite.html">SpriteRenderer.sprite</see>
		/// </summary>
		/// <value>The sprite.</value>
		public Sprite Sprite {
			get {
				return sprite;
			}
			set {
				sprite = value;
				renderer.sprite = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the renderer color of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-color.html">SpriteRenderer.color</see>
		/// </summary>
		/// <value>The renderer color.</value>
		public Color Color {
			get {
				return color;
			}
			set {
				renderer.color = value;
				color = value;
			}
		}

		public Material Material {
			get {
				return material;
			}
			set {
				material = value;
				renderer.material = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the position, in world space, of the projectile.
		/// </summary>
		/// <value>The position of the projectile.</value>
		public Vector2 Position;
		
		public Vector2 PositionImmediate {
			get {
				return Position;
			}
			set {
				transform.localPosition = value;
				Position = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the rotation of the projectile, in degrees.
		/// If viewed from a unrotated orthographic camera:
		/// 0 - Straight up
		/// 90 - Straight Left
		/// 180 - Straight Down
		/// 270 -  Straight Right
		/// </summary>
		/// <value>The rotation of the bullet in degrees.</value>
		public float Rotation {
			get {
				return rotation;
			}
			set {
				if(!symmetric)
					transform.localRotation = Quaternion.Euler(0f, 0f, value);
				rotation = value;
				direction = UnitCircle(rotation);
			}
		}
		
		/// <summary>
		/// Gets the direction vector the projectile is facing.
		/// It is a unit vector.
		/// Changing <see cref="Rotation"/> will change this vector.
		/// </summary>
		/// <value>The direction vector the projectile is facing toward.</value>
		public Vector2 Direction {
			get {
				return direction;
			}
		}
		
		/// <summary>
		/// The amount of time, in seconds,that has passed since this bullet has been fired.
		/// This is calculated based on the number of AbstractProjectileControllerd frames that has passed since the bullet has fired
		/// Pausing will cause this value to stop increasing
		/// </summary>
		/// <value>The time since the projectile has been fired.</value>
		public float Time {
			get {
				return frames * Util.TargetDeltaTime;
			}
		}
		
		/// <summary>
		/// The number of frames that have passed since this bullet has been fired.
		/// </summary>
		/// <value>The frame count since this bullet has been fired.</value>
		public int Frames {
			get {
				return frames;
			}
		}
		
		/// <summary>
		/// Gets the projectile's tag.
		/// </summary>
		/// <value>The tag of the projectile.</value>
		public string Tag {
			get {
				return tag;
			}
			set {
				tag = value;
				gameObject.tag = value;
			}
		}
		
		/// <summary>
		/// Gets the projectile's layer.
		/// </summary>
		/// <value>The layer of the projectile.</value>
		public int Layer {
			get {
				return layer;
			}
			set {
				layer = value;
				colliderMask = collisionMask[layer];
			}
		}

		public bool BoundsCheck {
			get;
			set;
		}

		public bool CollisionCheck {
			get;
			set;
		}

		/// <summary>
		/// Gets the DanmakuField this instance was fired from.
		/// </summary>
		/// <value>The field the projectile was fired from.</value>
		public DanmakuField Field {
			get {
				return field;
			}
		}

		public void AddController(IDanmakuController controller) {
			if(controller != null) {
				controllerUpdate += controller.UpdateProjectile;
				controllerCheck = controllerUpdate != null;
			}
		}

		public void AddController(DanmakuController controller) {
			controllerUpdate += controller;
			controllerCheck = controllerUpdate != null;
		}

		public void RemoveController(IDanmakuController controller) {
			if(controller != null) {
				controllerUpdate -= controller.UpdateProjectile;
				controllerCheck = controllerUpdate != null;
			}
		}

		public void RemoveController(DanmakuController controller) {
			controllerUpdate -= controller;
			controllerCheck = controllerUpdate != null;
		}

		public void ClearControllers() {
			controllerCheck = true;
			controllerUpdate = null;
		}

		public void Rotate(DynamicFloat delta) {
			float Delta = delta.Value;
			if(!symmetric)
				transform.Rotate(0f, 0f, Delta);
			rotation += Delta;
			direction = UnitCircle (rotation);
		}
		
		/// <summary>
		/// Compares the tag of the Projectile instance to the given string.
		/// Mirrors <a href="http://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html">GameObject.CompareTag</a>.
		/// </summary>
		/// <returns><c>true</c>, if tag is an exact match to the string, <c>false</c> otherwise.</returns>
		/// <param name="tag">Tag.</param>
		public bool CompareTag(string tag) {
			return gameObject.CompareTag (tag);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.Projectile"/> class.
		/// </summary>
		internal Danmaku() {
			groups = new List<DanmakuGroup> ();
			gameObject = new GameObject ();
			transform = gameObject.transform;
			renderer = gameObject.AddComponent<SpriteRenderer> ();
			#if UNITY_EDITOR
			//This is purely for cleaning up the inspector, no need in an actual build
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif
			raycastHits = new RaycastHit2D[5];
			colliders = new Collider2D[5];
			scripts = new IDanmakuCollider[5];
		}

		internal void Update() {
			frames++;
			originalPosition.x = Position.x;
			originalPosition.y = Position.y;

			if (controllerCheck) {
				controllerUpdate(this, dt);
			}

			if(AngularVelocity != 0f) {
				Rotation += AngularVelocity * dt;
			}

			if (Velocity != 0) {
				float change = Velocity * dt;
				Position.x += direction.x * change;
				Position.y += direction.y * change;
			}
			
			movementVector.x = Position.x - originalPosition.x;
			movementVector.y = Position.y - originalPosition.y;

			if(CollisionCheck) {
				distance = movementVector.magnitude;
				//Check if the collision detection should be continuous or not
				if (distance <= circleRaidus) {
					count = Physics2D.OverlapCircleNonAlloc(originalPosition + circleCenter,
					                                        circleRaidus,
					                                        colliders,
					                                        colliderMask);
					for (int i = 0; i < count; i++) {
						GameObject go = colliders [i].gameObject;
						scripts = Util.GetComponentsPrealloc (go, scripts, out count2);
						for (int j = 0; j < count2; j++) {
							scripts [j].OnProjectileCollision (this);
						}
						if (to_deactivate) {
							Position = Physics2D.CircleCast (originalPosition + circleCenter, circleRaidus, movementVector, distance).point;
							break;
						}
					}
				} else {
					count = Physics2D.CircleCastNonAlloc(originalPosition + circleCenter, 
					                                     circleRaidus,
					                                     movementVector,
					                                     raycastHits,
					                                     distance,
					                                     colliderMask);
					for (int i = 0; i < count; i++) {
						RaycastHit2D hit = raycastHits [i];
						GameObject go = hit.collider.gameObject;
						scripts = Util.GetComponentsPrealloc (go, scripts, out count2);
						for (int j = 0; j < count2; j++) {
							scripts [j].OnProjectileCollision (this);
						}
						if (to_deactivate) {
							Position = hit.point;
							break;
						}
					}
				}
			}

			if (BoundsCheck && !bounds.Contains (Position)) {
				DeactivateImmediate();
				return;
			}

			transform.localPosition = Position;

			if (to_deactivate) {
				DeactivateImmediate();
			}
		}

		/// <summary>
		/// Makes the instance of Projectile match the given ProjectilePrefab
		/// This copies:
		/// - the sprite, material, sorting layer, and color from the ProjectilePrefab's SpriteRenderer
		/// - the collider's size and offset from the ProjectilePrefab's CircleCollider2D
		/// - the tag and layer from the ProjectilePrefab's GameObject
		/// - any <see cref="ProjectileControlBehavior"/> on the ProjectilePrefab will be included as additional <see cref="IProjectileGroupController"/> that will affect the behavior of this bullet
		/// </summary>
		/// <param name="prefab">the ProjectilePrefab to match.</param>
		public void MatchPrefab(DanmakuPrefab prefab) {
			if (this.prefab != prefab) {
				this.prefab = prefab;
				this.runtime = prefab.GetRuntime();
				Vector2 scale = transform.localScale = runtime.cachedScale;
				renderer.sharedMaterial = runtime.cachedMaterial;
				renderer.sortingLayerID = runtime.cachedSortingLayer;
				circleCenter = scale.Hadamard2(runtime.cachedColliderOffset);
				circleRaidus = runtime.cachedColliderRadius * scale.Max();
				tag = gameObject.tag = runtime.cachedTag;
				symmetric = runtime.symmetric;
			}

			renderer.sprite = runtime.Sprite;
			renderer.color = runtime.cachedColor;
			layer = runtime.cachedLayer;
			colliderMask = collisionMask [layer];

			ProjectileControlBehavior[] pcbs = runtime.ExtraControllers;
			if (pcbs.Length > 0) {
				for (int i = 0; i < pcbs.Length; i++) {
					pcbs [i].ProjectileGroup.Add (this);
				}
			}
		}

		#region IPooledObject implementation

		private IPool pool;
		public IPool Pool {
			get {
				return pool;
			}
			set {
				pool = value;
			}
		}

		internal bool is_active;
		public bool IsActive {
			get {
				return is_active;
			}
		}

		/// <summary>
		/// Activates this instance.
		/// Calling this on a already fired projectile does nothing.
		/// Calling this on a projectile marked for deactivation will unmark the projectile and keep it from deactivating.
		/// </summary>
		public void Activate () {
			is_active = true;
			to_deactivate = false;
			//gameObject.SetActive (true);
			renderer.enabled = true;
			BoundsCheck = true;
			CollisionCheck = true;
		}
		
		/// <summary>
		/// Marks the Projectile for deactivation, and the Projectile will deactivate and return to the ProjectileManager after 
		/// finishing processing current updates, or when the Projectile is next updated
		/// If Projectile needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
		/// </summary>
		public void Deactivate()  {
			to_deactivate = true;
		}

		#endregion

		/// <summary>
		/// Adds this projectile to the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be added to</param>
		public void AddToGroup(DanmakuGroup group) {
			groups.Add (group);
			group.group.Add (this);
			groupCountCache++;
			groupCheck = groupCountCache > 0;
		}

		/// <summary>
		/// Removes this projectile from the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be removed from</param>
		public void RemoveFromGroup(DanmakuGroup group) {
			groups.Remove (group);
			group.group.Remove (this);
			groupCountCache--;
			groupCheck = groupCountCache > 0;
		}

		/// <summary>
		/// Immediately deactivates this Projectile and returns it to the pool it came from
		/// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
		/// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
		/// </summary>
		public void DeactivateImmediate() {
			for (int i = 0; i < groups.Count; i++) {
				groups[i].group.Remove (this);
			}
			groups.Clear ();
			groupCountCache = 0;
			groupCheck = false;
			controllerUpdate = null;
			controllerCheck = false;
			Damage = 0;
			frames = 0;
			//gameObject.SetActive (false);
			renderer.enabled = false;
			is_active = false;
			Pool.Return (this);
			//ProjectileManager.Return (this);
		}
	}
}
