// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	public delegate IEnumerator DanmakuTask(Danmaku danmaku);

	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Danmaku : IPooledObject, IPrefabed<DanmakuPrefab>, IDanmakuObject {

		public enum ColliderType { Point, Line, Circle, Box }

		internal int poolIndex;
		//internal int renderIndex;

		//internal float rotation;
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
		
		private DanmakuController controllerUpdate;
		internal List<DanmakuGroup> groups;

		private DanmakuField field;
		private Bounds2D fieldBounds;

		//Preallocated variables to avoid allocation in Update
		private Vector2 originalPosition;
		private RaycastHit2D[] raycastHits;
		//private Collider2D[] colliders;
		private Vector2 collisionCenter;

		//Cached check for controllers to avoid needing to calculate them in Update
		internal bool groupCheck;
		private bool controllerCheck;
		internal int groupCountCache;

		private List<IEnumerator> tasks;

		public float Speed;
		public float AngularSpeed;

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
				//return sprite;
				return runtime.Sprite;
			}
			//set {
			//	sprite = value;
			//	renderer.sprite = value;
			//}
		}
		
		/// <summary>
		/// Gets or sets the renderer color of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-color.html">SpriteRenderer.color</see>
		/// </summary>
		/// <value>The renderer color.</value>
		public Color32 Color;

		public Material Material {
			get {
				//return material;
				return runtime.Material;
			}
			//set {
			//	material = value;
			//	renderer.material = value;
			//}
		}

		internal Vector3 position;

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

		internal float rotation;

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
				rotation = value;
				direction = UnitCircle(value);
			}
		}
		
		/// <summary>
		/// Gets the direction vector the projectile is facing.
		/// It is a unit vector.
		/// Changing <see cref="RfieldBoundsn"/> will change this vector.
		/// </summary>
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

		public float Scale;

		/// <summary>
		/// The amount of time, in seconds,that has passed since this bullet has been fired.
		/// This is calculated based on the number of AbstractDanmakuControllerd frames that has passed since the bullet has fired
		/// Pausing will cause this value to stop increasing
		/// </summary>
		/// <value>The time since the projectile has been fired.</value>
		public float Time {
			get {
				return time;
			}
		}
		
		/// <summary>
		/// The number of framesfieldBoundshave passed since this bullet has been fired.
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
		public string Tag;
		
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

		public bool BoundsCheck;
		public bool CollisionCheck;

		#region IDanmakuObject implementation
		/// <summary>
		/// Gets the DanmakuField this instance was fired from.
		/// </summary>
		/// <value>The field the projectile was fired from.</value>
		public DanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
				if(field != null) {
					fieldBounds = field.bounds;
				}
			}
		}
		#endregion

		public void StartTask(IEnumerator task) {
			if (tasks == null)
				tasks = new List<IEnumerator> ();
			if (task != null)
				tasks.Add (task);
			else
				Debug.LogError ("Attempted to start a null task");
		}

		public void StartTask(DanmakuTask task) {
			if(tasks == null)
				tasks = new List<IEnumerator>();
			if (task != null) {
				IEnumerator newTask = task (this);
				if (newTask != null)
					tasks.Add (newTask);
				else
					Debug.LogError ("Attempted to start a null task");
			} else {
				Debug.LogError ("Attempted to start a null task");
			}
		}

		public void AddController(IDanmakuController controller) {
			if(controller != null) {
				controllerUpdate += controller.UpdateDanmaku;
				controllerCheck = controllerUpdate != null;
			}
		}

		public void AddController(DanmakuController controller) {
			controllerUpdate += controller;
			controllerCheck = controllerUpdate != null;
		}

		public void RemoveController(IDanmakuController controller) {
			if(controller != null) {
				controllerUpdate -= controller.UpdateDanmaku;
				controllerCheck = controllerUpdate != null;
			}
		}

		public void RemoveController(DanmakuController controller) {
			controllerUpdate -= controller;
			controllerCheck = controllerUpdate != null;
		}

//		public T AddComponent<T>() where T : Component {
//			T component = gameObject.AddComponent<T> ();
//			if (extraComponents == null)
//				extraComponents = new Stack<Component> ();
//			extraComponents.Push (component);
//			return component;
//		}

		public void ClearControllers() {
			controllerCheck = true;
			controllerUpdate = null;
		}

		public void Rotate(DynamicFloat delta) {
			Rotation += delta.Value;
		}
		
		/// <summary>
		/// Compares the tag of the Danmaku instance to the given string.
		/// Mirrors <a href="http://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html">GameObject.CompareTag</a>.
		/// </summary>
		/// <returns><c>true</c>, if tag is an exact match to the string, <c>false</c> otherwise.</returns>
		/// <param name="tag">Tag.</param>
//		public bool CompareTag(string tag) {
//			return gameObject.CompareTag (tag);
//		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DanmakU.Danmaku"/> class.
		/// </summary>
		internal Danmaku() {
			groups = new List<DanmakuGroup> ();

			//gameObject = new GameObject ();
			//transform = gameObject.transform;
			//renderer = gameObject.AddComponent<SpriteRenderer> ();
			#if UNITY_EDITOR
			//This is purely for cleaning up the inspector, no need in an actual build
			//gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif
			raycastHits = new RaycastHit2D[5];
//			colliders = new Collider2D[5];
//			scripts = new IDanmakuCollider[5];
		}

		internal void Update() {

			int i, j, count;
			originalPosition.x = position.x;
			originalPosition.y = position.y;
			Vector2 movementVector;

			#region thread_unsafe
			if (controllerCheck) {
				controllerUpdate(this, dt);
			}

			if(tasks != null) {
				count = tasks.Count;
				i = 0;
				while(i < count) {
					if(!tasks[i].MoveNext())
						tasks.RemoveAt(i);
					else
						i++;
				}
			}
			#endregion

			#region thread_safe
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

			Debug.DrawRay(originalPosition, movementVector);

			#endregion
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

			if (!is_active || (BoundsCheck && !fieldBounds.Contains (position))) {
				DeactivateImmediate();
				return;
			}
			
			frames++;
			time += dt;
		}

		/// <summary>
		/// Makes the instance of Danmaku match the given ProjectilePrefab
		/// This copies:
		/// - the sprite, material, sorting layer, and color from the ProjectilePrefab's SpriteRenderer
		/// - the collider's size and offset from the ProjectilePrefab's CircleCollider2D
		/// - the tag and layer from the ProjectilePrefab's GameObject
		/// - any <see cref="DanmakuControlBehavior"/> on the ProjectilePrefab will be included as additional <see cref="IDanmakuController"/> that will affect the behavior of this bullet
		/// </summary>
		/// <param name="prefab">the ProjectilePrefab to match.</param>
		public void MatchPrefab(DanmakuPrefab prefab) {
			if (prefab == null) {
				Debug.LogError("Tried to match a null prefab");
				return;
			}
			if (this.prefab != prefab) {
				this.prefab = prefab;
				runtime = prefab.GetRuntime ();
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

		#region IPooledObject implementation

		internal DanmakuPool pool;
		public IPool Pool {
			get {
				return pool;
			}
			set {
				pool = value as DanmakuPool;
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
			to_deactivate = false;
			//gameObject.SetActive (true);
			//renderer.enabled = true;
			is_active = true;
			BoundsCheck = true;
			CollisionCheck = true;
			runtime.Add(this);
		}
		
		/// <summary>
		/// Marks the Danmaku for deactivation, and the Danmaku will deactivate and return to the ProjectileManager after 
		/// finishing processing current updates, or when the Danmaku is next updated
		/// If Danmaku needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
		/// </summary>
		public void Deactivate()  {
			to_deactivate = true;
		}

		#endregion

		/// <summary>
		/// Adds this projectile to the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Danmaku is to be added to</param>
		public void AddToGroup(DanmakuGroup group) {
			groups.Add (group);
			group.group.Add (this);
			groupCountCache++;
			groupCheck = groupCountCache > 0;
		}

		/// <summary>
		/// Removes this projectile from the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Danmaku is to be removed from</param>
		public void RemoveFromGroup(DanmakuGroup group) {
			groups.Remove (group);
			group.group.Remove (this);
			groupCountCache--;
			groupCheck = groupCountCache > 0;
		}

		/// <summary>
		/// Immediately deactivates this Danmaku and returns it to the pool it came from
		/// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
		/// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
		/// </summary>
		public void DeactivateImmediate() {
			for (int i = 0; i < groups.Count; i++) {
				groups[i].group.Remove (this);
			}
			groups.Clear ();
			if(tasks != null)
				tasks.Clear ();
			groupCountCache = 0;
			groupCheck = false;
			controllerUpdate = null;
			controllerCheck = false;
			Damage = 0;
			frames = 0;
			runtime.Remove(this);
			is_active = false;
			pool.Return (this);
		}

		public override int GetHashCode () {
			return poolIndex;;
		}

//		public override bool Equals (object obj) {
//			return this == (obj as Danmaku);
//		}
//
//		public static bool operator ==(Danmaku d1, Danmaku d2) {
//			bool d1null = (object)d1 == null;
//			bool d2null = (object)d2 == null;
//			if(d1null && d2null)
//				return true;
//			if(d1null && !d2null)
//				return d2.is_active;
//			if(!d1null && d2null)
//				return d1.is_active;
//			return System.Object.ReferenceEquals(d1, d2);
//		}
//
//		public static bool operator !=(Danmaku d1, Danmaku d2) {
//			return !(d1 == d2);
//		}
	}
}
