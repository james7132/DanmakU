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
	public class SpeedLimitController : IDanmakuController {

		public enum LimitType { Maximum, Minimum }

		[SerializeField]
		private LimitType limitType;

		[SerializeField]
		private float limit;

		public SpeedLimitController () {
			limit = float.NaN;
		}

		public SpeedLimitController(float limit, LimitType type) {
			this.limit = limit;
			this.limitType = type;
		}

		#region IDanmakuController implementation

		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if(limitType == LimitType.Maximum) {
				if(danmaku.Speed > limit && !float.IsNaN(limit)) {
					danmaku.Speed = limit;
				}
			} else {
				if(danmaku.Speed < limit && !float.IsNaN(limit)) {
					danmaku.Speed = limit;
				}
			}
		}

		#endregion
	}

	namespace Wrapper {

	}
}

