// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// A set of scripts for commonly created Attack Patterns
/// </summary>
namespace DanmakU.AttackPatterns {

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

		public override void Awake () {
			base.Awake ();
			burstData.SubModifier = depthData;
			fireData.Modifier = burstData;
		}
		
		protected override void OnInitialize () {
			burstCount.Reset ();
			fireData.Position = spawnLocation - 0.5f * spawnArea + spawnArea.Random();
			fireData.Rotation = burstInitialRotation;
		}

		protected override IEnumerator MainLoop () {
			while (!burstCount.Tick()) {
				yield return burstDelay.MaxCount;
				fireData.Rotation += burstRotationDelta;
				Field.Fire(fireData);
			}
		}
	}
}
