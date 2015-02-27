using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.AttackPattern {
	public class BasicCircularBurst : AbstractAttackPattern {
		[SerializeField]
		private ProjectilePrefab prefab;

		[SerializeField]
		private Vector2 spawnLocation;

		[SerializeField]
		private Vector2 spawnArea;
		
		[SerializeField]
		private int bulletCount;
		
		[SerializeField]
		private float velocity;
		
		[SerializeField]
		[Range(-360f, 360f)]
		private float angV;
		
		[SerializeField]
		private Counter burstCount;
		
		[SerializeField]
		private CountdownDelay burstDelay;
		
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
			currentBurstSource = spawnLocation - 0.5f * spawnArea + Util.RandomVect2 (spawnArea);
		}
		
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
}