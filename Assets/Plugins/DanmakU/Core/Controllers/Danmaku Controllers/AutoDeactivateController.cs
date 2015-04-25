// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityUtilLib;
using Vexe.Runtime.Types;

namespace DanmakU.DanmakuControllers {

	/// <summary>
	/// An Danmaku Controller that automatically deactivates Danmaku after a certain time after being fired.
	/// </summary>
	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {

		[Show, Serialize]
		public int Frames {
			get;
			set;
		}

		public float Time {
			get {
				return Util.FramesToTime(Frames);
			}
			set {
				Frames = Util.TimeToFrames(value);
			}
		}

		public AutoDeactivateController(int frames = -1) {
			this.Frames = frames;
		}

		public AutoDeactivateController(float time) {
			Frames = Util.TimeToFrames (time);
		}

		#region IDanmakuController implementation
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (danmaku.frames > Frames) {
				danmaku.Deactivate();
			}
		}
		#endregion
		
	}

}