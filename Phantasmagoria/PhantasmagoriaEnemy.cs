using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class PhantasmagoriaEnemy : BasicEnemy {

	[SerializeField]
	private float deathReflectDuration;

	/// <summary>
	/// The death reflect radius.
	/// </summary>
	[SerializeField]
	private float deathReflectRadius;

	[SerializeField]
	private BulletTransferArea bulletTransferPrefab;

	protected override void OnDeath () {
		BulletTransferArea transferArea = (BulletTransferArea)Instantiate (bulletTransferPrefab, Transform.position, Quaternion.identity);
		transferArea.Run (deathReflectDuration, deathReflectRadius, Field, Field.TargetField);
	}
}
