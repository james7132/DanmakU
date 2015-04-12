using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	[AddComponentMenu("Danmaku 2D/Triggers/Timed Trigger")]
	public class TimedTrigger : DanmakuTrigger, IPausable {

		#region IPausable implementation
		public bool Paused {
			get;
			set;
		}
		#endregion

		[SerializeField]
		private DynamicFloat delay;
		private FrameCounter delayTimer;

		public override void Awake () {
			base.Awake ();
			delayTimer = new FrameCounter (delay);
		}

		public void Update() {
			if(!Paused) {
				if(delayTimer.Tick()) {
					Trigger();
				}
			}
		}

	}

}
