using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[RequireComponent(typeof(ProjectileManager))]
	[RequireComponent(typeof(EnemyManager))]
	public abstract class DanmakuGameController : GameController {

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

		public abstract void SpawnEnemy (Enemy prefab, Vector2 relativeLocations);
	}
}