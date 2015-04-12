// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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
