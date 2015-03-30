using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	public abstract class DanmakuTriggerReciever : CachedObject {

		[SerializeField]
		private DanmakuTrigger[] triggers;

		public override void Awake () {
			base.Awake ();
			for(int i = 0; i < triggers.Length; i++) {
				if(triggers[i] != null) {
					triggers[i].Trigger += Trigger;
				}
			}
		}

		public void OnDestroy() {
			for(int i = 0; i < triggers.Length; i++) {
				if(triggers[i] != null) {
					triggers[i].Trigger -= Trigger;
				}
			}
		}

		public abstract void Trigger ();
	}


	public abstract class DanmakuTrigger : CachedObject {

		public delegate void TriggerCallback ();
		
		internal TriggerCallback Trigger;
		
		public void FireTrigger() {
			if(Trigger != null)
				Trigger();
		}

	}

}
