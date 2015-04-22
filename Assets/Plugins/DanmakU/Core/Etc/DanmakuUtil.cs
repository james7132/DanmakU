using UnityEngine;
using System.Collections;

namespace DanmakU {

	public static class DanmakuUtil {
		
		public static float AngleBetween2D(Vector2 v1, Vector2 v2) {
			Vector2 diff = v2 - v1;
			return Mathf.Atan2 (diff.y, diff.x) * 180f / Mathf.PI - 90f; 
		}
		
		public static Quaternion RotationBetween2D(Vector2 v1, Vector2 v2) {
			return Quaternion.Euler (0f, 0f, AngleBetween2D (v1, v2));
		}
	}

}