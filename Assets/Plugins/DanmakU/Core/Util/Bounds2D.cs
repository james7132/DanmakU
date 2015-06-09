// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU
{
    public struct Bounds2D
    {
        private Vector2 _center;
        private Vector2 _extents;
        private Vector2 _max;
        private Vector2 _min;

        public Vector2 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                _min = value - _extents;
                _max = value + _extents;
            }
        }

        public Vector2 Extents
        {
            get { return _extents; }
            set
            {
                _extents = value;
                _min = _center - value;
                _max = _center + value;
            }
        }

        public Vector2 Max
        {
            get { return _max; }
            set
            {
                _max = value;
                if (_max.x < _min.x)
                {
                    float temp = _max.x;
                    _min.x = _max.x;
                    _max.x = temp;
                }
                if (_max.y < _min.y)
                {
                    float temp = _max.y;
                    _min.y = _max.y;
                    _max.y = temp;
                }
                _center = Vector2.Lerp(_min, _max, 0.5f);
                _extents = _max - _center;
            }
        }

        public Vector2 Min
        {
            get { return _min; }
            set
            {
                _min = value;
                if (_max.x < _min.x)
                {
                    float temp = _max.x;
                    _min.x = _max.x;
                    _max.x = temp;
                }
                if (_max.y < _min.y)
                {
                    float temp = _max.y;
                    _min.y = _max.y;
                    _max.y = temp;
                }
                _center = Vector2.Lerp(_min, _max, 0.5f);
                _extents = _max - _center;
            }
        }

        public float XMax
        {
            get { return _max.x; }
            set
            {
                _max.x = value;
                if (_max.x < _min.x)
                {
                    float temp = _max.x;
                    _min.x = _max.x;
                    _max.x = temp;
                }
                _center.x = (_min.x + _max.x)*0.5f;
                _extents.x = _max.x - _center.x;
            }
        }

        public float XMin
        {
            get { return _min.x; }
            set
            {
                _min.x = value;
                if (_max.x < _min.x)
                {
                    float temp = _max.x;
                    _min.x = _max.x;
                    _max.x = temp;
                }
                _center.x = (_min.x + _max.x)*0.5f;
                _extents.x = _max.x - _center.x;
            }
        }

        public float YMax
        {
            get { return _max.y; }
            set
            {
                _max.y = value;
                if (_max.y < _min.y)
                {
                    float temp = _max.y;
                    _min.y = _max.y;
                    _max.y = temp;
                }
                _center.y = (_min.y + _max.y)*0.5f;
                _extents.y = _max.y - _center.y;
            }
        }

        public float YMin
        {
            get { return _min.y; }
            set
            {
                _min.y = value;
                if (_max.y < _min.y)
                {
                    float temp = _max.y;
                    _min.y = _max.y;
                    _max.y = temp;
                }
                _center.y = (_min.y + _max.y)*0.5f;
                _extents.y = _max.y - _center.y;
            }
        }

        public Vector2 Size
        {
            get { return 2*Extents; }
            set { Extents = 0.5f*value; }
        }

        public float Width
        {
            get { return Size.x; }
            set
            {
                Vector2 size = Size;
                size.x = value;
                Size = size;
            }
        }

        public float Height
        {
            get { return Size.y; }
            set
            {
                Vector2 size = Size;
                size.y = value;
                Size = size;
            }
        }

        public Bounds2D(Vector2 center, Vector2 size) : this()
        {
            Center = center;
            Size = size.Abs();
        }

        public Bounds2D(Bounds bounds3D) : this()
        {
            _center = bounds3D.center;
            Size = bounds3D.size;
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= _min.x && point.y >= _min.y && point.x <= _max.x && point.y <= _max.y;
        }

        public bool Contains(Vector3 point)
        {
            return point.x >= _min.x && point.y >= _min.y && point.x <= _max.x && point.y <= _max.y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Bounds2D)
            {
                Bounds2D b = (Bounds2D) obj;
                return Center == b.Center && Size == b.Size;
            }
            return false;
        }

        public Vector2 ClosestPoint(Vector2 point)
        {
            float x = point.x;
            float y = point.y;
            if (x > _max.x)
            {
                if (y > _max.y)
                {
                    return _max;
                }
                else if (y < _min.y)
                {
                    return new Vector2(_max.x, _min.y);
                }
                else
                {
                    return new Vector2(_max.x, point.y);
                }
            }
            else if (x < _min.x)
            {
                if (y > _max.y)
                {
                    return new Vector2(_min.x, _max.y);
                }
                else if (y < _min.y)
                {
                    return _min;
                }
                else
                {
                    return new Vector2(_min.x, point.y);
                }
            }
            else
            {
                if (y > _max.y)
                {
                    return new Vector2(point.x, _max.y);
                }
                else if (y < _min.y)
                {
                    return new Vector2(point.x, _min.y);
                }
                else
                {
                    return point;
                }
            }
        }

        public void Expand(Vector2 point)
        {
            float x = point.x;
            float y = point.y;
            Rect rect = (Rect) this;
            if (x > _max.x)
            {
                rect.xMax = point.x;
            }
            else if (x < _min.x)
            {
                rect.xMin = point.x;
            }
            if (y > _max.y)
            {
                rect.xMax = point.x;
            }
            else if (y < _min.y)
            {
                rect.xMin = point.x;
            }
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode() + Extents.GetHashCode();
        }

        public static explicit operator Rect(Bounds2D bounds)
        {
            return new Rect(bounds._min.x, bounds._max.y, bounds.Width, bounds.Height);
        }

        public static explicit operator Bounds2D(Rect rect)
        {
            return new Bounds2D(rect.center, rect.size);
        }

        public static implicit operator Bounds2D(Bounds bounds)
        {
            return new Bounds2D(bounds.center, bounds.size);
        }

        public static implicit operator Bounds(Bounds2D bounds)
        {
            return new Bounds(bounds._center, bounds.Size);
        }

        public static bool operator ==(Bounds2D b1, Bounds2D b2)
        {
            return b1.Center == b2.Center && b1.Size == b2.Size;
        }

        public static bool operator !=(Bounds2D b1, Bounds2D b2)
        {
            return !(b1 == b2);
        }
    }
}