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

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU.DanmakuControllers {
	
	/// <summary>
	/// A DanmakuController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class AccelerationController : IDanmakuController {

		[SerializeField]
		private float acceleration;

		public AccelerationController() {
			this.acceleration = 0f;
		}

		public AccelerationController (float acceleration) : base() {
			this.acceleration = acceleration;
		}
		
		#region IDanmakuController implementation
		
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (acceleration != 0) {
				danmaku.Speed += acceleration * dt;
			}
		}
		
		#endregion
	}

}

