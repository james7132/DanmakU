using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace Danmaku2D.AttackPatterns {

	/// <summary>
	/// An abstract class for consecutive basic bursts of bullets
	/// </summary>
	public abstract class Burst : AttackPattern {

		[SerializeField]
		private ProjectilePrefab prefab;
		
		[SerializeField]
		private Vector2 spawnLocation;
		
		[SerializeField]
		private Vector2 spawnArea;
		
		[SerializeField]
		private int bulletCount;

		[SerializeField]
		private int burstDepth = 1;

		[SerializeField]
		private float burstRange = 360f;
		
		[SerializeField]
		private Counter burstCount;
		
		[SerializeField]
		private FrameCounter burstDelay;
		
		[SerializeField]
		[Range(-180f, 180f)]
		private float burstInitialRotation;
		
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
			currentBurstSource = spawnLocation - 0.5f * spawnArea + spawnArea.Random();
		}

		/// <summary>
		/// An overridable factory method for subclasses to control the various 
		/// </summary>
		/// <value>The controller to be used with the bullets fired with this attack pattern</value>
		protected abstract IProjectileController GetBurstController(int depth);

		protected override void MainLoop () {
			if(burstDelay.Tick()) {
				float offset = (burstCount.MaxCount - burstCount.Count) * burstRotationDelta;
				TargetField.SpawnBurst(prefab, 
				                       currentBurstSource,
				                       burstInitialRotation + offset,
				                       burstRange,
				                       bulletCount,
				                       null,
				                       burstDepth,
				                       GetBurstController);
				burstCount.Tick();
			}
		}
	}
}
