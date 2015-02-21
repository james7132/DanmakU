using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// Basic circular burst.
/// </summary>
public class BasicCircularBurst : AbstractAttackPattern {

	/// <summary>
	/// The prefab.
	/// </summary>
	[SerializeField]
	private ProjectilePrefab prefab;

	/// <summary>
	/// The spawn location.
	/// </summary>
	[SerializeField]
	private Vector2 spawnLocation;

	[SerializeField]
	private Vector2 spawnArea;

	/// <summary>
	/// The bullet count.
	/// </summary>
	[SerializeField]
	private int bulletCount;

	/// <summary>
	/// The velocity.
	/// </summary>
	[SerializeField]
	private float velocity;

	/// <summary>
	/// The ang v.
	/// </summary>
	[SerializeField]
	[Range(-360f, 360f)]
	private float angV;

	/// <summary>
	/// The burst count.
	/// </summary>
	[SerializeField]
	private Counter burstCount;

	/// <summary>
	/// The burst fire delay.
	/// </summary>
	[SerializeField]
	private CountdownDelay burstDelay;

	/// <summary>
	/// The burst initial rotation.
	/// </summary>
	[SerializeField]
	[Range(-180f, 180f)]
	private float burstInitialRotation;

	/// <summary>
	/// The burst rotation delta.
	/// </summary>
	[SerializeField]
	[Range(-360f, 360f)]
	private float burstRotationDelta;

	private Vector2 currentBurstSource;

	protected override bool IsFinished {
		get {
			return burstCount.Ready();
		}
	}

	protected override void OnExecutionStart () {
		burstCount.Reset ();
		currentBurstSource = spawnLocation - 0.5f * spawnArea + Util.RandomVect2 (spawnArea);
	}

	/// <summary>
	/// Mains the loop.
	/// </summary>
	/// <param name="dt">Dt.</param>
	protected override void MainLoop (float dt) {
		if (burstCount.Count > 0) {
			if(burstDelay.Tick(dt)) {
				float offset = (burstCount.MaxCount - burstCount.Count) * burstRotationDelta;
				for(int i = 0; i < bulletCount; i++) {
					FireCurvedBullet(prefab, currentBurstSource, offset + 360f / (float) bulletCount * (float)i, velocity, angV);
				}
				burstCount.Tick();
			}
		}
	}
}
