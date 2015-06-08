// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers {

	[System.Serializable]
	public class ColorChangeController : IDanmakuController {
		
		//TODO Document

		[SerializeField, Show]
		private Gradient colorGradient;
		public Gradient ColorGradient {
			get {
				return colorGradient;
			}
			set {
				colorGradient = value;
			}
		}
		
		[SerializeField, Show]
		private float startTime;
		public float StartTime {
			get {
				return startTime;
			}
			set {
				startTime = value;
			}
		}
		
		[SerializeField, Show]
		private float endTime;
		public float EndTime {
			get {
				return endTime;
			}
			set {
				endTime = value;
			}
		}

		#region IDanmakuController implementation

		/// <summary>
		/// Updates the Danmaku controlled by the controller instance.
		/// </summary>
		/// <param name="danmaku">the bullet to update.</param>
		/// <param name="dt">the change in time since the last update</param>
		/// <param name="projectile">Projectile.</param>
		public void Update (Danmaku projectile, float dt) {
			Gradient gradient = ColorGradient;
			if (gradient == null)
				return;
			
			float start = StartTime;
			float end = EndTime;
			float bulletTime = projectile.Time;
			Color endColor = gradient.Evaluate (1f);

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

		#endregion
	}
}