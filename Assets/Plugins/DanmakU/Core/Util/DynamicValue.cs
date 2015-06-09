// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DanmakU {

    [Serializable]
    public struct DynamicInt {

        public enum ValueType {

            Constant,
            Random

        }

        [SerializeField]
        private int _max;

        [SerializeField]
        private int _min;

        [SerializeField]
        private ValueType _type;

        public DynamicInt(int value) {
            _min = value;
            _max = value;
            _type = ValueType.Constant;
        }

        public DynamicInt(int min, int max) {
            if (min > max) {
                _max = min;
                _min = max;
            } else {
                _max = max;
                _min = min;
            }
            _type = (min == max) ? ValueType.Constant : ValueType.Random;
        }

        public ValueType Type {
            get { return _type; }
        }

        public int Min {
            get { return _min; }
            set {
                _min = value;
                if (_min > _max) {
                    int temp = _max;
                    _max = _min;
                    _min = temp;
                }
            }
        }

        public int Max {
            get { return _max; }
            set {
                _max = value;
                if (_min > _max) {
                    int temp = _max;
                    _max = _min;
                    _min = temp;
                }
            }
        }

        public int Value {
            get {
                if (_type == ValueType.Constant || _max == _min)
                    return _min;
                return Random.Range(_min, _max);
            }
        }

        public static implicit operator int(DynamicInt df) {
            return df.Value;
        }

        public static implicit operator DynamicInt(int f) {
            return new DynamicInt(f);
        }

        public static implicit operator DynamicFloat(DynamicInt di) {
            return new DynamicFloat(di._min, di._max);
        }

        public static DynamicInt operator +(DynamicInt di1, DynamicInt di2) {
            return new DynamicInt(di1._min + di2._min, di1._max + di2._max);
        }

        public static DynamicInt operator -(DynamicInt di1, DynamicInt di2) {
            return new DynamicInt(di1._min - di2._min, di1._max - di2._max);
        }

        public static DynamicInt operator *(DynamicInt di1, DynamicInt di2) {
            return new DynamicInt(di1._min*di2._min, di1._max*di2._max);
        }

        public static bool operator ==(DynamicInt di1, DynamicInt di2) {
            if (di1._type == ValueType.Constant &&
                di2._type == ValueType.Constant)
                return di1._min == di2._min;
            return (di1._min == di2._min) && (di1._max == di2._max);
        }

        public static bool operator !=(DynamicInt df1, DynamicInt df2) {
            return !(df1 == df2);
        }

        public override bool Equals(object obj) {
            if (obj is DynamicInt) {
                return (DynamicInt)obj == this;
            }
            return false;
        }

        public override int GetHashCode() {
            return 193*_min + 389*_max;
        }

    }

    [Serializable]
    public struct DynamicFloat {

        public enum ValueType {

            Constant,
            Random

        }

        [SerializeField]
        private float _max;

        [SerializeField]
        private float _min;

        [SerializeField]
        private ValueType type;

        public DynamicFloat(float value) {
            _min = value;
            _max = value;
            type = ValueType.Constant;
        }

        public DynamicFloat(float min, float max) {
            if (min > max) {
                _max = min;
                _min = max;
            } else {
                _max = max;
                _min = min;
            }
            type = (min == max) ? ValueType.Constant : ValueType.Random;
        }

        public ValueType Type {
            get { return type; }
        }

        public float Min {
            get { return _min; }
            set {
                _min = value;
                if (_min > _max) {
                    float temp = _max;
                    _max = _min;
                    _min = temp;
                }
            }
        }

        public float Max {
            get { return _max; }
            set {
                _max = value;
                if (_min > _max) {
                    float temp = _max;
                    _max = _min;
                    _min = temp;
                }
            }
        }

        public float Value {
            get {
                if (type == ValueType.Constant ||
                    Math.Abs(_max - _min) < float.Epsilon)
                    return _min;
                return Random.Range(_min, _max);
            }
        }

        public static implicit operator float(DynamicFloat df) {
            return df.Value;
        }

        public static implicit operator DynamicFloat(float f) {
            return new DynamicFloat(f);
        }

        public static explicit operator DynamicInt(DynamicFloat df) {
            return new DynamicInt((int) df._min, (int) df._max);
        }

        public static DynamicFloat operator +(DynamicFloat df, float f) {
            return new DynamicFloat(df._min + f, df._max + f);
        }

        public static DynamicFloat operator +(float f, DynamicFloat df) {
            return new DynamicFloat(df._min + f, df._max + f);
        }

        public static DynamicFloat operator +(DynamicFloat df, int i) {
            return new DynamicFloat(df._min + i, df._max + i);
        }

        public static DynamicFloat operator +(int i, DynamicFloat df) {
            return new DynamicFloat(df._min + i, df._max + i);
        }

        public static DynamicFloat operator +(DynamicFloat df1, DynamicFloat df2
            ) {
            return new DynamicFloat(df1._min + df2._min, df1._max + df2._max);
        }

        public static DynamicFloat operator -(DynamicFloat df, int i) {
            return new DynamicFloat(df._min - i, df._max - i);
        }

        public static DynamicFloat operator -(int i, DynamicFloat df) {
            return new DynamicFloat(i - df._min, i - df._max);
        }

        public static DynamicFloat operator -(DynamicFloat df1, DynamicFloat df2
            ) {
            return new DynamicFloat(df1._min - df2._min, df1._max - df2._max);
        }

        public static DynamicFloat operator *(DynamicFloat df1, DynamicFloat df2
            ) {
            return new DynamicFloat(df1._min*df2._min, df1._min*df2._max);
        }

        public static bool operator ==(DynamicFloat df1, DynamicFloat df2) {
            return (df1._min == df2._min) && (df1._max == df2._max);
        }

        public static bool operator !=(DynamicFloat df1, DynamicFloat df2) {
            return !(df1 == df2);
        }

        public override bool Equals(object obj) {
            if (obj is DynamicFloat) {
                DynamicFloat df = (DynamicFloat) obj;
                return df == this;
            }
            return false;
        }

        public override int GetHashCode() {
            return 193*_min.GetHashCode() + 389*_min.GetHashCode();
        }

    }

}