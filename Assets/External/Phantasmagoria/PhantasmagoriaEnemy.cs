using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaEnemy : BasicEnemy {

		[SerializeField]
		public GameObject onDeathSpawn;

		[SerializeField]
		private float specialBoost = 0.05f;

		protected override void OnDeath () {
			Field.SpawnGameObject (onDeathSpawn, transform.position, DanmakuField.CoordinateSystem.World);
			(Field.player as PhantasmagoriaPlayableCharacter).CurrentChargeCapacity += specialBoost;
		}
	}
}