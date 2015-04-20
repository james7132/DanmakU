// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace DanmakU {

	[AddComponentMenu("DanmakU/Triggers/Timed Trigger")]
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
