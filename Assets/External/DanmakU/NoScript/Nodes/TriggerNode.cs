// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;

namespace DanmakU {
	
	public class TriggerNode : DanmakuNode {
		
		[SerializeField]
		private DanmakuTrigger sourceTrigger;

		[SerializeField]
		private Path[] paths;

		protected internal override void Initialize () {
			base.Initialize ();
			sourceTrigger.triggerCallback += Trigger;
		}
	
		private void Trigger() {
			for(int i = 0; i < paths.Length; i++) {
				paths[i].Fire();
			}
		}

		public void Bake() {
			List<Path> validPaths = new List<Path>();
			GeneratePaths(new Path(this), validPaths);
			paths = validPaths.ToArray();
		}
			
		#region implemented abstract members of DanmakuNode
		public override void Process (FireBuilder target) {
		}
		#endregion
	}


}