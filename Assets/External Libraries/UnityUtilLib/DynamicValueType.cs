using System;
using UnityEngine;

namespace UnityUtilLib { 

	/// <summary>
	/// Abstract dynamic value.
	/// </summary>
	public abstract class AbstractDynamicValue<T> {

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public abstract T Value { get; }

		/// <param name="adv">Adv.</param>
		public static implicit operator T(AbstractDynamicValue<T> adv) {
			if(adv == null) {
				return default(T);
			}
			return adv.Value;		
		}
	}

	/// <summary>
	/// Fixed float.
	/// </summary>
	public class FixedFloat : AbstractDynamicValue<float> {
		private float value;

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public override float Value {
			get { return value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityUtilLib.FixedFloat"/> class.
		/// </summary>
		/// <param name="value">Value.</param>
		public FixedFloat (float value) {
			this.value = value;
		}
	}

	/// <summary>
	/// Random range float.
	/// </summary>
	public class RandomRangeFloat : AbstractDynamicValue<float> {
		private float min;
		private float max;

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public override float Value {
			get { return UnityEngine.Random.Range (min, max); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityUtilLib.RandomRangeFloat"/> class.
		/// </summary>
		/// <param name="rangeMin">Range minimum.</param>
		/// <param name="rangeMax">Range max.</param>
		public RandomRangeFloat(float rangeMin, float rangeMax) {
			min = rangeMin;
			max = rangeMax;
		}
	}
}