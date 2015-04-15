// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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

		public override void Awake () {
			base.Awake ();
//			for(int i = 0; i < controllers.Length; i++) {
//				if(controllers[i] != null)
//					fireData.Controller += controllers[i].Controller;
//			}
			burstData.SubModifier = depthData;
			//Debug.Log (depthData.SubModifier);
			fireData.Modifier = burstData;
		}
		
		protected override void OnInitialize () {
			burstCount.Reset ();
			fireData.Position = spawnLocation - 0.5f * spawnArea + spawnArea.Random();
			fireData.Rotation = burstInitialRotation;
		}

		protected override IEnumerator MainLoop () {
			while (!burstCount.Ready()) {
//				yield return WaitForFrames(burstDelay.MaxCount);
				fireData.Rotation += burstRotationDelta;
				Field.Fire(fireData);
				burstCount.Tick();
			}
			yield return null;
		}
	}
}
