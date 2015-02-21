using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectilePool))]
[RequireComponent(typeof(EnemyManager))]
public abstract class AbstractDanmakuGameController : MonoBehaviour {

	[SerializeField]
	private ProjectilePool bulletPool;

	public ProjectilePool BulletPool {
		get {
			return bulletPool;
		}
	}

	[SerializeField]
	private int maximumLives;
	/// <summary>
	/// Gets the maximum lives.
	/// </summary>
	/// <value>The maximum lives.</value>
	public int MaximumLives {
		get {
			return maximumLives;
		}
	}

	public abstract void SpawnEnemy (AbstractEnemy prefab, Vector2 relativeLocations);
}
