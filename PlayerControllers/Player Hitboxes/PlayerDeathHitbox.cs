using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// Player death hitbox.
/// </summary>
public class PlayerDeathHitbox : CachedObject {

	private AbstractPlayableCharacter player;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		player = GetComponentInParent<AbstractPlayableCharacter> ();
		if (player == null) {
			Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
		}
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Hit");
		if (player != null) {
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				player.Hit(proj);
				proj.Deactivate();
			}
		}
	}

	void OnProjectileCollision(Projectile proj) {
		if (player != null) {
			player.Hit (proj);
			proj.Deactivate();
		}
	}
}
