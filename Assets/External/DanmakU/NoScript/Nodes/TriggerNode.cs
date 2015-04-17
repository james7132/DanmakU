// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public class TriggerNode : DanmakuNode {
		
		[SerializeField]
		private DanmakuTrigger sourceTrigger;

		protected internal override void Initialize () {
			base.Initialize ();
			sourceTrigger.triggerCallback += Trigger;
		}
	
		private void Trigger() {
			Trigger (new FireBuilder () {
					CoordinateSystem = DanmakuField.CoordinateSystem.World
				}
			);
		}
			
		#region implemented abstract members of DanmakuNode
		protected override void Process () {
		}
		#endregion
	}


}