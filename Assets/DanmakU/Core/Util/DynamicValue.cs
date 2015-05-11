// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	[System.Serializable]
	public struct DynamicInt {
			
		public enum ValueMode { Constant, Random }

		public ValueMode Mode;
		
		[SerializeField]
		private float centerValue;
		
		[SerializeField]
		private float range;
		
		public int Value {
			get {
				if(Mode == ValueMode.Constant)
					return (int)centerValue;
				else {
					return (int)Random.Range(centerValue - range, centerValue + range);
				}
			}
		}
		
		private DynamicInt(float value) {
			centerValue = value;
			range = 0f;
			Mode = ValueMode.Constant;
		}
		
		private DynamicInt(float min, float max) {
			centerValue = (min + max) / 2;
			range = Mathf.Abs(centerValue - min);
			Mode = ValueMode.Random;
		}
		
		public DynamicInt(int value) {
			centerValue = value;
			range = 0f;
			Mode = ValueMode.Constant;
		}
		
		public DynamicInt(int min, int max) {
			centerValue = (min + max) / 2;
			range = Mathf.Abs(centerValue - min);
			Mode = ValueMode.Random;
		}
		
		public static implicit operator int(DynamicInt df) {
			return df.Value;
		}
		
		public static implicit operator DynamicInt(int f) {
			return new DynamicInt (f);
		}
		
		public static DynamicInt operator +(DynamicInt df1, DynamicInt df2) {
			if (df1.Mode == ValueMode.Constant && df2.Mode == ValueMode.Constant)
				return new DynamicInt (df1.centerValue + df2.centerValue);
			else 
				return new DynamicInt (df1.centerValue + df2.centerValue, df1.range + df2.range);
		}
		
		public static DynamicInt operator -(DynamicInt df1, DynamicInt df2) {
			if (df1.Mode == ValueMode.Constant && df2.Mode == ValueMode.Constant)
				return new DynamicInt (df1.centerValue - df2.centerValue);
			else 
				return new DynamicInt (df1.centerValue - df2.centerValue, df1.range + df2.range);
		}
		
		public static bool operator ==(DynamicInt df1, DynamicInt df2) {
			return (df1.centerValue == df2.centerValue) && (df1.range == df2.range);
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
			return (int)(centerValue * range);
		}
	}

	[System.Serializable]
	public struct DynamicFloat {

		public enum Type { Constant, Random }

		[SerializeField]
		private Type type;
		
		[SerializeField]
		private float centerValue;
		
		[SerializeField]
		private float range;

		public float Value {
			get {
				if(type == Type.Constant)
					return centerValue;
				else {
					return Random.Range(centerValue - range, centerValue + range);
				}
			}
		}
		
		public DynamicFloat(float value) {
			centerValue = value;
			range = 0f;
			type = Type.Constant;
		}
		
		public DynamicFloat(float min, float max) {
			centerValue = (min + max) / 2;
			range = Mathf.Abs(centerValue - min);
			type = Type.Random;
		}

		public static implicit operator float(DynamicFloat df) {
			return df.Value;
		}

		public static implicit operator DynamicFloat(float f) {
			return new DynamicFloat (f);
		}

		public static DynamicFloat operator +(DynamicFloat df1, DynamicFloat df2) {
			if (df1.type == Type.Constant && df2.type == Type.Constant)
				return new DynamicFloat (df1.centerValue + df2.centerValue);
			else 
				return new DynamicFloat (df1.centerValue + df2.centerValue, df1.range + df2.range);
		}

		public static DynamicFloat operator -(DynamicFloat df1, DynamicFloat df2) {
			if (df1.type == Type.Constant && df2.type == Type.Constant)
				return new DynamicFloat (df1.centerValue - df2.centerValue);
			else 
				return new DynamicFloat (df1.centerValue - df2.centerValue, df1.range - df2.range);
		}

		public static bool operator ==(DynamicFloat df1, DynamicFloat df2) {
			return (df1.centerValue == df2.centerValue) && (df1.range == df2.range);
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
			return (int)(centerValue * range);
		}
	}
}
