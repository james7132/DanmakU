// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	[System.Serializable]
	public struct DynamicInt {
			
		public enum Type { Constant, Random }

		[SerializeField]
		private Type type;
		
		[SerializeField]
		private int min;
		public int Min {
			get {
				return min;
			}
			set {
				min = value;
				if(min > max) {
					int temp = max;
					max = min;
					min = temp;
				}
			}
		}
		
		[SerializeField]
		private int max;
		public int Max {
			get {
				return max;
			}
			set {
				max = value;
				if(min > max) {
					int temp = max;
					max = min;
					min = temp;
				}
			}
		}
		
		public int Value {
			get {
				if(type == Type.Constant || max == min)
					return min;
				else {
					return Random.Range(min, max);
				}
			}
		}
		
		public DynamicInt(int value) {
			min = value;
			max = value;
			type = Type.Constant;
		}
		
		public DynamicInt(int min, int max) {
			Min = min;
			Max = max;
			type = (min == max) ? Type.Constant : Type.Random;
		}
		
		public static implicit operator int(DynamicInt df) {
			return df.Value;
		}
		
		public static implicit operator DynamicInt(int f) {
			return new DynamicInt (f);
		}

		public static implicit operator DynamicFloat(DynamicInt di) {
			return new DynamicFloat(di.min, di.max);
		}
		
		public static DynamicInt operator +(DynamicInt di1, DynamicInt di2) {
			return new DynamicInt (di1.min + di2.min, di1.max + di2.max);
		}
		
		public static DynamicInt operator -(DynamicInt di1, DynamicInt di2) {
			return new DynamicInt (di1.min - di2.min, di1.max - di2.max);
		}

		public static DynamicInt operator *(DynamicInt di1, DynamicInt di2) {
			return new DynamicInt (di1.min * di2.min, di1.max * di2.max);
		}
		
		public static bool operator ==(DynamicInt di1, DynamicInt di2) {
			if(di1.type == Type.Constant && di2.type == Type.Constant)
				return di1.min == di2.min;
			return (di1.min == di2.min) && (di1.max == di2.max);
		}
		
		public static bool operator !=(DynamicInt df1, DynamicInt df2) {
			return !(df1 == df2);
		}
		
		public override bool Equals (object obj) {
			if (obj is DynamicInt) {
				DynamicInt df = (DynamicInt)obj;
				return df == this;
			}
			return false;
		}
		
		public override int GetHashCode () {
			return 193 * min + 389 * max;
		}
	}

	[System.Serializable]
	public struct DynamicFloat {

		public enum Type { Constant, Random }

		[SerializeField]
		private Type type;
		
		[SerializeField]
		private float min;
		public float Min {
			get {
				return min;
			}
			set {
				min = value;
				if(min > max) {
					float temp = max;
					max = min;
					min = temp;
				}
			}
		}
		
		[SerializeField]
		private float max;
		public float Max {
			get {
				return max;
			}
			set {
				max = value;
				if(min > max) {
					float temp = max;
					max = min;
					min = temp;
				}
			}
		}

		public float Value {
			get {
				if(type == Type.Constant || max == min)
					return min;
				else {
					return Random.Range(min, max);
				}
			}
		}
		
		public DynamicFloat(float value) {
			min = value;
			max = value;
			type = Type.Constant;
		}
		
		public DynamicFloat(float min, float max) {
			Min = min;
			Max = max;
			type = (min == max) ? Type.Constant : Type.Random;
		}

		public static implicit operator float(DynamicFloat df) {
			return df.Value;
		}

		public static implicit operator DynamicFloat(float f) {
			return new DynamicFloat (f);
		}

		public static explicit operator DynamicInt (DynamicFloat df) {
			return new DynamicInt((int)df.min, (int)df.max);
		}

		public static DynamicFloat operator +(DynamicFloat df1, DynamicFloat df2) {
			return new DynamicFloat(df1.min + df2.min, df1.max + df2.max);
		}

		public static DynamicFloat operator -(DynamicFloat df1, DynamicFloat df2) {
			return new DynamicFloat(df1.min - df2.min, df1.max - df2.max);
		}

		public static DynamicFloat operator *(DynamicFloat df1, DynamicFloat df2) {
			return new DynamicFloat (df1.min * df2.min, df1.min * df2.max);
		}

		public static bool operator ==(DynamicFloat df1, DynamicFloat df2) {
			return (df1.min == df2.min) && (df1.max == df2.max);
		}

		public static bool operator !=(DynamicFloat df1, DynamicFloat df2) {
			return !(df1 == df2);
		}

		public override bool Equals (object obj) {
			if (obj is DynamicFloat) {
				DynamicFloat df = (DynamicFloat)obj;
				return df == this;
			}
			return false;
		}

		public override int GetHashCode () {
			return 193 * min.GetHashCode() + 389 * min.GetHashCode();
		}
	}
}
