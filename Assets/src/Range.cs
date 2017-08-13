using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

	[Serializable]
	public struct Range {

		public float Min;
		public float Max;

		public Range(float val) : this(val, val) {
		}

		public Range(float min, float max) {
			if (min > max) {
				Min = max;
				Max = min;
			} else {
				Max = max;
				Min = min;
			}
		}

		public float GetValue() {
			return UnityEngine.Random.value * (Max - Min) + Min;
		}

		public static implicit operator float(Range range) {
			return range.GetValue();
		}

		public static implicit operator Range(float val) {
			return new Range(val);
		}

		public static Range operator +(Range lhs, Range rhs) {
			return new Range(lhs.Min + rhs.Min, lhs.Max + rhs.Max);
		}

		public static Range operator -(Range lhs, Range rhs) {
			return new Range(lhs.Min - rhs.Min, lhs.Max - rhs.Max);
		}

		public static Range operator *(Range lhs, float rhs) {
			return new Range(lhs.Min * rhs, lhs.Max * rhs);
		}

		public static Range operator /(Range lhs, float rhs) {
			return new Range(lhs.Min / rhs, lhs.Max / rhs);
		}

	}

}