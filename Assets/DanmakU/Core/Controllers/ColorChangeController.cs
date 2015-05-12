// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class ColorChangeController : IDanmakuController {
		
		[SerializeField, Show]
		public Gradient ColorGradient {
			get;
			set;
		}
		
		[SerializeField, Show]
		public float StartTime {
			get;
			set;
		}
		
		[SerializeField, Show]
		public float EndTime {
			get;
			set;
		}

		public void Update (Danmaku projectile, float dt) {
			Gradient gradient = ColorGradient;
			if (gradient == null)
				return;

			float start = StartTime;
			float end = EndTime;
			float bulletTime = projectile.Time;
			Color endColor = gradient.Evaluate (1f);

//			Debug.Log (bulletTime);
			if (bulletTime < start) {
				projectile.Color = projectile.Prefab.Color;
			} else if (bulletTime > end)
				projectile.Color = endColor;
			else {
				if (EndTime <= start)
					projectile.Color = endColor;
				else
					projectile.Color = gradient.Evaluate ((bulletTime - start) / (end - start));
			}
		}
	}
}