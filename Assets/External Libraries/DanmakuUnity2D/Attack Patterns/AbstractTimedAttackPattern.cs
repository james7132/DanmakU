using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public abstract class AbstractTimedAttackPattern : AbstractAttackPattern {

		[SerializeField]
		private CountdownDelay timeout;

		protected override void MainLoop (float dt) {
			timeout.Tick (dt, false);
		}

		protected override void OnExecutionStart () {
			timeout.Reset ();
		}

		protected sealed override bool IsFinished {
			get {
				return timeout.Ready();
			}
		}
	}
}