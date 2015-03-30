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
	public class Burst : AttackPattern {

		[SerializeField]
		private FireBuilder fireData;

		[SerializeField]
		private BurstModifier burstData;

		[SerializeField]
		private LineModifier depthData;
		
		[SerializeField]
		private Vector2 spawnLocation;
		
		[SerializeField]
		private Vector2 spawnArea;
		
		[SerializeField]
		private Counter burstCount;
		
		[SerializeField]
		private FrameCounter burstDelay;
		
		[SerializeField]
		private DynamicFloat burstInitialRotation;
		
		[SerializeField]
		private DynamicFloat burstRotationDelta;

		[SerializeField]
		private ProjectileControlBehavior[] controllers;
		
		protected override bool IsFinished {
			get {
				return burstCount.Ready();
			}
		}

		public override void Awake () {
			base.Awake ();
			for(int i = 0; i < controllers.Length; i++) {
				if(controllers[i] != null)
					fireData.Controller += controllers[i].UpdateProjectile;
			}
			burstData.SubModifier = depthData;
			Debug.Log (depthData.SubModifier);
			fireData.Modifier = burstData;
		}
		
		protected override void OnInitialize () {
			burstCount.Reset ();
			fireData.Position = spawnLocation - 0.5f * spawnArea + spawnArea.Random();
			fireData.Rotation = burstInitialRotation;
		}

		protected override void MainLoop () {
			if(burstDelay.Tick()) {
				fireData.Rotation += burstRotationDelta;
				TargetField.Fire(fireData);
				burstCount.Tick();
			}
		}
	}
}
