using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	
	internal class DelayedDestruction : PausableGameObject {

		#pragma warning disable 0649
		public Object target;
		public FrameCounter delay;
		public bool destroySelf = true;
		#pragma warning restore 0649 

		public override void NormalUpdate () {
			if (delay.Tick ()) {
				if(target != null) {
					Destroy (target);
				}
				if(destroySelf) {
					Destroy (this);
				}
			}
		}

	}

}
