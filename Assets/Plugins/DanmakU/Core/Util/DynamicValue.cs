// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DanmakU {

    [Serializable]
    public struct DInt {

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

        public DInt(int value) {
            _min = value;
            _max = value;
            _type = ValueType.Constant;
        }

        public DInt(int min, int max) {
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
                if (_min <= _max)
                    return;
                int temp = _max;
                _max = _min;
                _min = temp;
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

        public static implicit operator int(DInt df) {
            return df.Value;
        }

        public static implicit operator DInt(int f) {
            return new DInt(f);
        }

        public static implicit operator DFloat(DInt di) {
            return new DFloat(di._min, di._max);
        }

        public static DInt operator +(DInt di1, DInt di2) {
            return new DInt(di1._min + di2._min, di1._max + di2._max);
        }

        public static DInt operator -(DInt di1, DInt di2) {
            return new DInt(di1._min - di2._min, di1._max - di2._max);
        }

        public static DInt operator *(DInt di1, DInt di2) {
            return new DInt(di1._min*di2._min, di1._max*di2._max);
        }

        public static bool operator ==(DInt di1, DInt di2) {
            if (di1._type == ValueType.Constant &&
                di2._type == ValueType.Constant)
                return di1._min == di2._min;
            return (di1._min == di2._min) && (di1._max == di2._max);
        }

        public static bool operator !=(DInt df1, DInt df2) {
            return !(df1 == df2);
        }

        public override bool Equals(object obj) {
            if (obj is DInt)
                return (DInt) obj == this;
            return false;
        }

        public override int GetHashCode() {
            return 193*_min + 389*_max;
        }

        public override string ToString() {
            if (_type == ValueType.Constant || _max == _min)
                return _max.ToString();
            else
                return string.Format("({0} - {1})", _min, _max);
        }

    }

    [Serializable]
    public struct DFloat {

        public enum ValueType {

            Constant,
            Random

        }

        [SerializeField]
        private float _max;

        [SerializeField]
        private float _min;

        [SerializeField]
        private ValueType _type;

        public DFloat(float value) {
            _min = value;
            _max = value;
            _type = ValueType.Constant;
        }

        public DFloat(float min, float max) {
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

        public float Min {
            get { return _min; }
            set {
                _min = value;
                if (!(_min > _max))
                    return;
                float temp = _max;
                _max = _min;
                _min = temp;
            }
        }

        public float Max {
            get { return _max; }
            set {
                _max = value;
                if (!(_min > _max))
                    return;
                float temp = _max;
                _max = _min;
                _min = temp;
            }
        }

        public float Value {
            get {
                if (_type == ValueType.Constant ||
                    Math.Abs(_max - _min) < float.Epsilon)
                    return _min;
                return Random.Range(_min, _max);
            }
        }

        public static implicit operator float(DFloat df) {
            return df.Value;
        }

        public static implicit operator DFloat(float f) {
            return new DFloat(f);
        }

        public static explicit operator DInt(DFloat df) {
            return new DInt((int) df._min, (int) df._max);
        }

        public static DFloat operator +(DFloat df, float f) {
            return new DFloat(df._min + f, df._max + f);
        }

        public static DFloat operator +(float f, DFloat df) {
            return new DFloat(df._min + f, df._max + f);
        }

        public static DFloat operator +(DFloat df, int i) {
            return new DFloat(df._min + i, df._max + i);
        }

        public static DFloat operator +(int i, DFloat df) {
            return new DFloat(df._min + i, df._max + i);
        }

        public static DFloat operator +(DFloat df1, DFloat df2
            ) {
            return new DFloat(df1._min + df2._min, df1._max + df2._max);
        }

        public static DFloat operator -(DFloat df, int i) {
            return new DFloat(df._min - i, df._max - i);
        }

        public static DFloat operator -(int i, DFloat df) {
            return new DFloat(i - df._min, i - df._max);
        }

        public static DFloat operator -(DFloat df1, DFloat df2
            ) {
            return new DFloat(df1._min - df2._min, df1._max - df2._max);
        }

        public static DFloat operator *(DFloat df1, DFloat df2
            ) {
            return new DFloat(df1._min*df2._min, df1._min*df2._max);
        }

        public static bool operator ==(DFloat df1, DFloat df2) {
            return (Math.Abs(df1._min - df2._min) < float.Epsilon) && (Math.Abs(df1._max - df2._max) < float.Epsilon);
        }

        public static bool operator !=(DFloat df1, DFloat df2) {
            return !(df1 == df2);
        }

        public override bool Equals(object obj) {
            if (!(obj is DFloat))
                return false;
            var df = (DFloat) obj;
            return df == this;
        }

        public override int GetHashCode() {
            return 193*_min.GetHashCode() + 389*_max.GetHashCode();
        }

        public override string ToString() {
            if (_type == ValueType.Constant || Math.Abs(_max - _min) < float.Epsilon)
                return _max.ToString();
            else
                return string.Format("({0} - {1})", _min, _max);
        }

    }

}