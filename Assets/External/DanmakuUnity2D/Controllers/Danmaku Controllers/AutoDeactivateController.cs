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
using UnityUtilLib;

namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {

		[SerializeField]
		private bool useTime;

		[SerializeField]
		private int frames;

		public int Frames {
			get {
				return frames;
			}
			set {
				frames = value;
			}
		}

		public float Time {
			get {
				return Util.FramesToTime(frames);
			}
			set {
				frames = Util.TimeToFrames(value);
			}
		}

		public AutoDeactivateController () {
			frames = -1;
		}

		public AutoDeactivateController(int frames) {
			this.frames = frames;
		}

		public AutoDeactivateController(float time) {
			frames = Util.TimeToFrames (time);
		}

		#region IDanmakuController implementation
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (danmaku.frames > frames) {
				danmaku.Deactivate();
			}
		}
		#endregion
		
	}

}