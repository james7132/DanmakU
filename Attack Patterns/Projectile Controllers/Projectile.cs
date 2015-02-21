using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Projectile.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : PooledObject<ProjectilePrefab> {

	private static int[] collisionMask;

	public static void RecomputeProjectileCollisions() {
		collisionMask = new int[32];
		for(int i = 0; i < 32; i++) {
			collisionMask[i] = 0;
			for (int j = 0; j < 32; j++) {
				collisionMask[i] |= (Physics2D.GetIgnoreLayerCollision(i, j)) ? 0 : (1 << j);
			}
		}
	}

	private bool to_deactivate;

	/// <summary>
	/// The damage.
	/// </summary>
	private int damage;
	public int Damage {
		get {
			return damage;
		}
		set {
			damage = value;
		}
	}

	/// <summary>
	/// The linear velocity.
	/// </summary>
	private float linearVelocity = 0f;
	public float Velocity {
		get {
			return linearVelocity;
		}
		set {
			linearVelocity = value;
		}
	}

	/// <summary>
	/// The angular velocity.
	/// </summary>
	private Quaternion angularVelocity = Quaternion.identity;
	public float AngularVelocity {
		get {
			return angularVelocity.eulerAngles.z;
		}
		set {
			angularVelocity = Quaternion.Euler(new Vector3(0f, 0f, value));
		}
	}

	/// <summary>
	/// Gets or sets the angular velocity radians.
	/// </summary>
	/// <value>The angular velocity radians.</value>
	public float AngularVelocityRadians
	{
		get {
			return AngularVelocity * Util.Degree2Rad;
		}
		set {
			AngularVelocity = value * Util.Rad2Degree;
		}
	}

	/// <summary>
	/// The controllers.
	/// </summary>
	private List<ProjectileController> controllers;

	/// <summary>
	/// The properties.
	/// </summary>
	private Dictionary<string, object> properties;

	/// <summary>
	/// The circle center.
	/// </summary>
	private Vector2 circleCenter = Vector2.zero; 

	/// <summary>
	/// The circle raidus.
	/// </summary>
	private float circleRaidus = 1f;

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private float fireTimer;

	public float BulletTime {
		get {
			return fireTimer;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public override void Awake() {
		properties = new Dictionary<string, object> ();
		controllers = new List<ProjectileController> ();
		if(spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer> ();
		if(collisionMask == null)
			RecomputeProjectileCollisions();
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate() {
		float dt = Time.fixedDeltaTime;
		fireTimer += dt;
		//Rotate
		Transform.rotation = Quaternion.Slerp (Transform.rotation, Transform.rotation * angularVelocity, dt);
		float movementDistance = linearVelocity * dt;
		Vector3 movementVector = Transform.up * movementDistance;

		Vector2 offset = Util.ComponentProduct2(Transform.lossyScale, circleCenter);
		float radius = circleRaidus * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
		RaycastHit2D[] hits = Physics2D.CircleCastAll(Transform.position + Util.To3D(offset), 
		                           radius, 
		                           movementVector, 
		                           movementDistance,
		                           collisionMask[GameObject.layer]);
		Debug.DrawRay (Transform.position, movementVector);

		//Translate
		Transform.position += movementVector;

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit2D hit = hits[i];
			if (hit.collider != null) {
				hit.collider.SendMessage("OnProjectileCollision", this, SendMessageOptions.DontRequireReceiver);
			}
			if(to_deactivate){
				Transform.position = hit.point;
				break;
			}
		}

		for(int i = 0; i < controllers.Count; i++)
			if(controllers[i] != null)
				controllers[i].UpdateBullet(this, dt);

		if (to_deactivate) {
			DeactivateImmediate();
		}
	}

	/// <summary>
	/// Transfer the specified currentController and targetField.
	/// </summary>
	/// <param name="currentController">Current controller.</param>
	/// <param name="targetField">Target field.</param>
	public void Transfer(PhantasmagoriaField currentController, PhantasmagoriaField targetField) {
		Vector2 relativePos = currentController.FieldPoint (Transform.position);
		Transform.position = targetField.WorldPoint (relativePos);
	}

	/// <summary>
	/// Matchs the prefab.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	public override void MatchPrefab(ProjectilePrefab prefab) {
		CircleCollider2D cc = prefab.CircleCollider;
		SpriteRenderer sr = prefab.SpriteRenderer;

		Transform.localScale = prefab.Transform.localScale;
		gameObject.tag = prefab.GameObject.tag;
		gameObject.layer = prefab.GameObject.layer;

		if(spriteRenderer != null) {
			spriteRenderer.sprite = sr.sprite;
			spriteRenderer.color = sr.color;
			//spriteRenderer.material = spriteTest.material;
			spriteRenderer.sortingOrder = sr.sortingOrder;
			spriteRenderer.sortingLayerID = sr.sortingLayerID;
		}
		else
			Debug.LogError("The provided prefab should have a SpriteRenderer!");

		if(cc != null) {//circleCollider.enabled = cc != null) {
//			circleCollider.center = cc.center;
//			circleCollider.radius = cc.radius;
			circleCenter = cc.center;
			circleRaidus = cc.radius;
		}
		else
			Debug.LogError("The provided prefab should a CircleCollider2D!");
	}

	/// <summary>
	/// Determines whether this instance has property the specified key.
	/// </summary>
	/// <returns><c>true</c> if this instance has property the specified key; otherwise, <c>false</c>.</returns>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public bool HasProperty<T>(string key) {
		return (properties.ContainsKey(key)) && (properties[key] is T);
	}

	/// <summary>
	/// Gets the property.
	/// </summary>
	/// <returns>The property.</returns>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T GetProperty<T>(string key) {
		if(properties.ContainsKey(key))
			return (T)properties[key];
		else 
			return default(T);
	}
	/// <summary>
	/// Sets the property.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void SetProperty<T>(string key, T value) {
		properties [key] = value;
	}

	/// <summary>
	/// Adds the controller.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public void AddController(ProjectileController controller) {
		controllers.Add (controller);
		controller.OnControllerAdd (this);
	}

	/// <summary>
	/// Removes the controller.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public void RemoveController (ProjectileController controller) {
		if(controllers.Remove(controller))
			controller.OnControllerRemove(this);
	}

	public override void Activate () {
		base.Activate ();
		to_deactivate = false;
	}

	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public override void Deactivate()  {
		to_deactivate = true;
	}

	public void DeactivateImmediate() {
		base.Deactivate();
		properties.Clear ();
		controllers.Clear ();
		linearVelocity = 0f;
		fireTimer = 0f;
		damage = 0;
		angularVelocity = Quaternion.identity;
	}
}
