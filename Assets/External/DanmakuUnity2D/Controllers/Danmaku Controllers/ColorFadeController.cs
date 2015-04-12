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

namespace Danmaku2D.DanmakuControllers {

	[System.Serializable]
	public class ColorFadeController : IDanmakuController {

		[SerializeField]
		private Color endColor;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float endTime;

		public void UpdateDanmaku (Danmaku projectile, float dt) {
			float bulletTime = projectile.Time;
			Color startColor = projectile.Prefab.cachedColor;
//			Debug.Log (bulletTime);
			if (bulletTime < startTime)
				projectile.Color = startColor;
			else if (bulletTime > endTime)
				projectile.Color = endColor;
			else {
				if(endTime <= startTime)
					projectile.color = endColor;
				else
					projectile.Color = Color.Lerp (startColor, endColor, (bulletTime - startTime) / (endTime - startTime));
			}
		}
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Controllers/Color Fade Controller")]
		internal class ColorFadeController : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.ColorFadeController> {
		}
	}
}