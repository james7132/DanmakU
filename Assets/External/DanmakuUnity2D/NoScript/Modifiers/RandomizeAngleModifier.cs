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

namespace Danmaku2D {

	[System.Serializable]
	public class RandomizeAngleModifier : DanmakuModifier {

		[SerializeField, Range(0f, 360f)]
		private DynamicFloat range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, DynamicFloat rotation) {
			float rotationValue = rotation.Value;
			float rangeValue = range.Value;
			FireSingle (position, Random.Range (rotationValue - 0.5f * rangeValue, rotationValue + 0.5f * rangeValue));
		}
		#endregion

	}
}