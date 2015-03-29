using UnityEngine;
using System.Collections;

namespace Danmaku2D.AttackPatterns {

	
	public class ObjectAttackPattern : TimedAttackPattern {

		public GameObject prefab;
		public Vector2 position;
		private GameObject Runtime;

		protected override void OnInitialize () {
			base.OnInitialize ();
			if(prefab == null)
				throw new MissingReferenceException(GetType().ToString() + " needs a prefab to function.");
			Runtime = TargetField.SpawnGameObject(prefab, position);
		}

		protected override void OnFinalize () {
			base.OnFinalize ();
			if (Runtime != null)
				Destroy (Runtime);
		}
	}

}