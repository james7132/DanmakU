using UnityEngine;
using System.Collections;
using UnityUtilLib;

public abstract class AbstractTimedAttackPattern : AbstractAttackPattern {

	[SerializeField]
	private CountdownDelay timeout;

	protected override void MainLoop (float dt) {
		timeout.Tick (dt, false);
	}

	protected override void OnExecutionStart () {
		timeout.Reset ();
	}

	protected override bool IsFinished {
		get {
			return timeout.Ready();
		}
	}
}
