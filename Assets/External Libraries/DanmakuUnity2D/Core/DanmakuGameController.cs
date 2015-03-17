using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// A GameController implementation for 2D Danmaku games.
	/// </summary>
	[RequireComponent(typeof(ProjectileManager))]
	[RequireComponent(typeof(EnemyManager))]
	public abstract class DanmakuGameController : GameController {

		[SerializeField]
		private int maximumLives;

		/// <summary>
		/// The maximum number of lives a player can reach.
		/// </summary>
		/// <value>The maximum lives.</value>
		public static int MaximumLives {
			get {
				return (Instance as DanmakuGameController).maximumLives;
			}
		}

		public abstract void SpawnEnemy (Enemy prefab, Vector2 relativeLocations);
	}
}