// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;
using Vexe.Runtime.Types;

namespace DanmakU {

	public abstract class DanmakuTriggerReciever : BetterBehaviour {

		[SerializeField]
		private List<DanmakuTrigger> triggers;

		public void Awake () {
			for(int i = 0; i < triggers.Count; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback += Trigger;
				}
			}
		}

		public void OnDestroy() {
			for(int i = 0; i < triggers.Count; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback -= Trigger;
				}
			}
		}

		public abstract void Trigger ();
	}

	[AddComponentMenu("DanmakU/Danmaku Trigger")]
	public class DanmakuTrigger : CachedObject {

		public delegate void TriggerCallback ();
		
		internal TriggerCallback triggerCallback;
		
		public void Trigger() {
			if(triggerCallback != null)
				triggerCallback();
		}

	}

}
