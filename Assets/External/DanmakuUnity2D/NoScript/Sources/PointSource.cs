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
using System.Collections.Generic;

namespace Danmaku2D.NoScript {

	[AddComponentMenu("Danmaku 2D/Sources/Point Source")]
	internal class PointSource : DanmakuSource {

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			if (sourcePoints.Count <= 0 || sourcePoints.Count > 1) {
				sourcePoints.Clear();
				sourcePoints.Add(new SourcePoint(position, rotation));
			}
			if(sourcePoints[0] == null) {
				sourcePoints[0] = new SourcePoint(position, rotation);
			}
			sourcePoints [0].Position = position;
			sourcePoints [0].BaseRotation = rotation;
		}

		#endregion

	}

}
