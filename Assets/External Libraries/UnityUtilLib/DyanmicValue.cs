using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	public enum DynamicValueType {
		Fixed, Random
	}

	public class DynamicValue<T> {

		protected delegate T RandomFunc(T min, T max);

		protected static RandomFunc randomFunc;

		public DynamicValueType type;
		public T FixedValue;
		public T MinRandomValue;
		public T MaxRandomValue;

		public T Value {
			get {
				switch(type) {
					default:
					case DynamicValueType.Fixed:
						return FixedValue;
					case DynamicValueType.Random:
						return randomFunc(MinRandomValue, MaxRandomValue);
				}
			}
		}

	}

	public class DynamicFloat : DynamicValue<float> {

		static DynamicFloat() {
			randomFunc = Random.Range;
		}

		public static implicit operator float(DynamicFloat dynamicValue) {
			return dynamicValue.Value;
		}
	}
}