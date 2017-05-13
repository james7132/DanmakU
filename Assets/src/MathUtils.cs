using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

	public static class MathUtils {

		// Approximately 0.5 degrees in radians
		const float _accuracy = 0.00872665f;
		public const float TwoPI = Mathf.PI * 2;
		const float Deg2Rad = Mathf.PI / 180f;
		static Vector2[] _directions;

		static MathUtils() {
			_directions = new Vector2[Mathf.CeilToInt(TwoPI / _accuracy)];
			for (var i = 0; i < _directions.Length; i++) {
				var angle = _accuracy * i;
				_directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			}
		}

		// Quickly creates a direction vector without needing to call trig functions
		public static Vector2 GetDirection(float radians) {
			var index = Mathf.RoundToInt(radians / _accuracy);
			return _directions[index % _directions.Length];
		}

		public static Vector2 GetDirectionDegress(float degrees) {
			return GetDirection(degrees * Deg2Rad);
		}

	}

}
