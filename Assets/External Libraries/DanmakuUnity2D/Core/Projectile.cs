using UnityEngine;
using UnityUtilLib;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed class Projectile : PooledObject, IPrefabed<ProjectilePrefab> {

		private static Vector2 unchanged = Vector2.zero;
		private static int[] collisionMask;
		private bool to_deactivate;
		private GameObject gameObject;
		private Transform transform;
		private SpriteRenderer renderer;

		/// <summary>
		/// Gets or sets the damage this projectile does to entities.
		/// Generally speaking, this is only used for projectiles fired by the player at enemies
		/// </summary>
		/// <value>The damage this projectile does.</value>
		public int Damage {
			get;
			set;
		}

		private IProjectileController controller;
		/// <summary>
		/// The controller 
		/// </summary>
		/// <value>The controller.</value>
		public IProjectileController Controller {
			get {
				return controller;
			}
			set {
				controller = value;
				if(controller != null)
					controller.Projectile = this;
			}
		}

		#region IPrefabed implementation

		private ProjectilePrefab prefab;
		/// <summary>
		/// Gets the prefab that this projectile is currently based on.
		/// </summary>
		/// <value>The prefab it is currently based on.</value>
		public ProjectilePrefab Prefab {
			get {
				return prefab;
			}
		}

		#endregion

		private List<ProjectileGroup> groups;
		private Vector2 circleCenter = Vector2.zero; 
		private float circleRaidus = 1f;

		/// <summary>
		/// Gets the renderer sprite of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-sprite.html">SpriteRenderer.sprite</see>
		/// </summary>
		/// <value>The sprite.</value>
		public Sprite Sprite {
			get {
				return renderer.sprite;
			}
		}

		/// <summary>
		/// Gets or sets the renderer color of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-color.html">SpriteRenderer.color</see>
		/// </summary>
		/// <value>The renderer color.</value>
		public Color Color {
			get {
				return renderer.color;
			}
			set {
				renderer.color = value;
			}
		}

		/// <summary>
		/// Gets or sets the position, in world space, of the projectile.
		/// </summary>
		/// <value>The position of the projectile.</value>
		public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the projectile, in degrees.
		/// If viewed from a unrotated orthographic camera:
		/// 0 - Straight up
		/// 90 - Straight Left
		/// 180 - Straight Down
		/// 279 -  Straight Right
		/// </summary>
		/// <value>The rotation of the bullet in degrees.</value>
		public float Rotation {
			get {
				return transform.rotation.eulerAngles.z;
			}
			set {
				transform.rotation = Quaternion.Euler(0f, 0f, value);
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
				return transform.up;
			}
		}
		
		private int frames;

		/// <summary>
		/// The amount of time, in seconds,that has passed since this bullet has been fired.
		/// This is calculated based on the number of unpaused frames that has passed since the bullet has fired
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
				return gameObject.tag;
			}
		}

		/// <summary>
		/// Gets the projectile's layer.
		/// </summary>
		/// <value>The layer of the projectile.</value>
		public int Layer {
			get {
				return gameObject.layer;
			}
		}

		public bool CompareTag(string tag) {
			return gameObject.CompareTag (tag);
		}
		
		private RaycastHit2D[] hits;

		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.Projectile"/> class.
		/// </summary>
		public Projectile() {
			if(collisionMask == null)
				collisionMask = Util.CollisionLayers2D();
			groups = new List<ProjectileGroup> ();
			gameObject = new GameObject ("Projectile");
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			renderer = gameObject.AddComponent<SpriteRenderer> ();
			transform = gameObject.transform;
			gameObject.SetActive (false);
			hits = new RaycastHit2D[10];
		}

		internal void Update() {
			frames++;
			float dt = Util.TargetDeltaTime;

			Vector2 movementVector = Vector3.zero;

			if (Controller != null) {
				movementVector += Controller.UpdateProjectile (dt);
			}

			if(groups.Count > 0) {
				for (int i = 0; i < groups.Count; i++) {
					movementVector += groups[i].UpdateProjectile(this, dt);
				}
			}

			int count = Physics2D.CircleCastNonAlloc(transform.position + Util.To3D(circleCenter), 
			                                         circleRaidus,
			                                         movementVector,
			                                         hits,
			                                         movementVector.magnitude,
			                                         collisionMask[gameObject.layer]);
			
			if(movementVector != unchanged)
				transform.position += (Vector3)movementVector;

			//Translate
			if(count > 0) {
				int i;
				for (i = 0; i < count; i++) {
					RaycastHit2D hit = hits[i];
					if (hit.collider != null) {
						hit.collider.SendMessage("OnProjectileCollision", this, SendMessageOptions.DontRequireReceiver);
					}
					if(to_deactivate){
						Position = hits[i].point;
						break;
					}
				}
			}
			
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
		public void MatchPrefab(ProjectilePrefab prefab) {
			this.prefab = prefab;
			ProjectilePrefab runtime = prefab.GetRuntime ();
			CircleCollider2D cc = runtime.CircleCollider;
			SpriteRenderer sr = runtime.SpriteRenderer;
			ProjectileControlBehavior[] pcbs = runtime.ExtraControllers;
			
			transform.localScale = runtime.transform.localScale;
			gameObject.tag = runtime.gameObject.tag;
			gameObject.layer = runtime.gameObject.layer;
			
			if(sr != null) {
				renderer.sprite = sr.sprite;
				renderer.color = sr.color;
				renderer.sharedMaterial = sr.sharedMaterial;
				renderer.sortingLayerID = renderer.sortingLayerID;
			}
			else
				Debug.LogError("The provided prefab should have a SpriteRenderer!");
			
			if(cc != null) {
				circleCenter = Util.ComponentProduct2(transform.lossyScale, cc.offset);
				circleRaidus = cc.radius * Util.MaxComponent2(Util.To2D(transform.lossyScale));
			}
			else
				Debug.LogError("The provided prefab should a CircleCollider2D!");

			for(int i = 0; i < pcbs.Length; i++) {
				pcbs[i].ProjectileGroup.Add(this);
			}
		}

		/// <summary>
		/// Activates this instance.
		/// Calling this on a already fired projectile does nothing.
		/// Calling this on a projectile marked for deactivation will unmark the projectile and keep it from deactivating.
		/// </summary>
		public override void Activate () {
			to_deactivate = false;
			gameObject.SetActive (true);
		}

		/// <summary>
		/// Marks the Projectile for deactivation, and the Projectile will deactivate and return to the ProjectileManager after 
		/// finishing processing current updates, or when the Projectile is next updated
		/// If Projectile needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
		/// </summary>
		public override void Deactivate()  {
			to_deactivate = true;
		}

		/// <summary>
		/// Adds this projectile to the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be added to</param>
		public void AddToGroup(ProjectileGroup group) {
			groups.Add (group);
			group.Add (this);
		}

		/// <summary>
		/// Removes this projectile from the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be removed from</param>
		public void RemoveFromGroup(ProjectileGroup group) {
			groups.Remove (group);
			group.Remove (this);
		}

		/// <summary>
		/// Immediately deactivates this Projectile and returns it to the pool it came from
		/// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
		/// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
		/// </summary>
		public void DeactivateImmediate() {
			Controller = null;
			ProjectileGroup[] temp = groups.ToArray ();
			for (int i = 0; i < temp.Length; i++) {
				temp[i].Remove(this);
			}
			Damage = 0;
			frames = 0;
			gameObject.SetActive (false);
			base.Deactivate ();
		}
	}
}
