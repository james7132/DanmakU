using System;
using UnityEngine;

namespace UnityUtilLib { 

	public abstract class DynamicValue<T> {
		public abstract T Value { get; }

		public static implicit operator T(DynamicValue<T> adv) {
			if(adv == null) {
				return default(T);
			}
			return adv.Value;		
		}
	}

	public class FixedFloat : DynamicValue<float> {
		private float value;

		public override float Value {
			get { return value; }
		}
		
		public FixedFloat (float value) {
			this.value = value;
		}
	}

	public class RandomRangeFloat : DynamicValue<float> {
		private float min;
		private float max;

		public override float Value {
			get { return UnityEngine.Random.Range (min, max); }
		}

		public RandomRangeFloat(float rangeMin, float rangeMax) {
			min = rangeMin;
			max = rangeMax;
		}
	}
}