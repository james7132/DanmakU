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
	public class DelayedAngleChange : IDanmakuController {

		private enum RotationMode { Absolute, Relative, Player }

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float delay;

		[SerializeField]
		private DynamicFloat angle;

		[SerializeField]
		private bool setAngV;

		[SerializeField]
		private DynamicFloat angularVelocity;

		#region implemented abstract members of IDanmakuController

		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			float time = danmaku.Time;
			if(time >= delay && time - dt <= delay) {
				float baseAngle = angle.Value;
				switch(rotationMode) {
					case RotationMode.Relative:
						baseAngle += danmaku.Rotation;
						break;
					case RotationMode.Player:
						baseAngle += danmaku.Field.AngleTowardPlayer(danmaku.Position);
						break;
					case RotationMode.Absolute:
						break;
				}
				danmaku.Rotation = baseAngle;
				if(setAngV)
					danmaku.AngularVelocity = angularVelocity;
			}
		}

		#endregion
	}
}