using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaEnemy : BasicEnemy {

		[SerializeField]
		private float deathReflectDuration;

		[SerializeField]
		private float deathReflectRadius;

		[SerializeField]
		private BulletTransferArea bulletTransferPrefab;

		protected override void OnDeath () {
			BulletTransferArea transferArea = (BulletTransferArea)Instantiate (bulletTransferPrefab, Transform.position, Quaternion.identity);
			transferArea.Run (deathReflectDuration, deathReflectRadius, Field, Field.TargetField);
		}
	}
}