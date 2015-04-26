// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public sealed class ControllerNode : DanmakuNode {

		public IDanmakuController Controller {
			get;
			set;
		}

		#region implemented abstract members of DanmakuNode
		public override void Process (FireBuilder target) {
			if (Controller != null)
				target.Controller += Controller.UpdateDanmaku;
			else
				Debug.LogError ("Controller Node: Attempted to add null controller");
		}
		#endregion
	}

}
