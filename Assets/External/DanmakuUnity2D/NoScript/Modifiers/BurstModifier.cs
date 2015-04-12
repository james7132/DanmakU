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

	[System.Serializable]
	public class BurstModifier : FireModifier {

		public DynamicFloat Range = 360f;
		public DynamicInt Count = 1;
		public DynamicFloat DeltaVelocity = 0f;
		public DynamicFloat DeltaAngularVelocity = 0f;

		#region implemented abstract members of FireModifier

		public override void Fire (Vector2 position, DynamicFloat rotation) {

			int count = Count.Value;
			float range = Range.Value;
			float deltaV = DeltaVelocity.Value;
			float deltaAV = DeltaAngularVelocity.Value; 

			count = Mathf.Abs (count);

			float start = rotation - range * 0.5f;
			float delta = range / (count - 1);

			for (int i = 0; i < count; i++) {
				Velocity += deltaV;
				AngularVelocity += deltaAV;
				FireSingle(position, start + i * delta);
			}

		}

		#endregion

	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Modifiers/Burst Modifier")]
		public class BurstModifier : Modifier<Danmaku2D.BurstModifier> {
		}

	}
}
