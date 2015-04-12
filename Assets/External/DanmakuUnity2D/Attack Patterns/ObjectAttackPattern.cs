using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.AttackPatterns {

	
	public class ObjectAttackPattern : TimedAttackPattern {

		public GameObject prefab;
		public Vector2 position;
		public Vector2 range = new Vector2(0.5f, 0.25f);
		private GameObject Runtime;

		protected override void OnInitialize () {
			base.OnInitialize ();
			if(prefab == null)
				throw new MissingReferenceException(GetType().ToString() + " needs a prefab to function.");
			Vector2 spawnPos = position - range + 2 * range.Random ();
			Runtime = TargetField.SpawnGameObject(prefab, spawnPos);
		}

		protected override void OnFinalize () {
			base.OnFinalize ();
			if (Runtime != null)
				Destroy (Runtime);
		}
	}

}