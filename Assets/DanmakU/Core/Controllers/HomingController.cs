// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU.DanmakuControllers {

	public class HomingController : IDanmakuController {

		public Transform Target {
			get;
			set;
		}

		#region IDanmakuController implementation
		public void Update (Danmaku danmaku, float dt) {
			danmaku.AngularSpeed = 0f;
			danmaku.Rotation = DanmakuUtil.AngleBetween2D(danmaku.position, Target.position);
		}
		#endregion

	}


}