// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	public struct Bounds2D {

		private Vector2 center;
		private Vector2 extents;
		private Vector2 max;
		private Vector2 min;

		public Vector2 Center {
			get {
				return center;
			}
			set {
				center = value;
				min = value - extents;
				max = value + extents;
			}
		}
		public Vector2 Extents {
			get {
				return extents;
			}
			set {
				extents = value;
				min = center - value;
				max = center + value;
			}
		}

		public Vector2 Max {
			get {
				return max;
			}
			set {
				max = value;
				if(max.x < min.x) {
					float temp = max.x;
					min.x = max.x;
					max.x = temp;
				}
				if(max.y < min.y) {
					float temp = max.y;
					min.y = max.y;
					max.y = temp;
				}
				center = Vector2.Lerp(min, max, 0.5f);
				extents = max - center;
			}
		}

		public Vector2 Min {
			get {
				return min;
			}
			set {
				min = value;
				if(max.x < min.x) {
					float temp = max.x;
					min.x = max.x;
					max.x = temp;
				}
				if(max.y < min.y) {
					float temp = max.y;
					min.y = max.y;
					max.y = temp;
				}
				center = Vector2.Lerp(min, max, 0.5f);
				extents = max - center;
			}
		}

		public float XMax {
			get {
				return max.x;
			}
			set {
				max.x = value;
				if(max.x < min.x) {
					float temp = max.x;
					min.x = max.x;
					max.x = temp;
				}
				center.x = (min.x + max.x) * 0.5f;
				extents.x = max.x - center.x;
			}
		}

		public float XMin {
			get {
				return min.x;
			}
			set {
				min.x = value;
				if(max.x < min.x) {
					float temp = max.x;
					min.x = max.x;
					max.x = temp;
				}
				center.x = (min.x + max.x) * 0.5f;
				extents.x = max.x - center.x;
			}
		}

		public float YMax {
			get {
				return max.y;
			}
			set {
				max.y = value;
				if(max.y < min.y) {
					float temp = max.y;
					min.y = max.y;
					max.y = temp;
				}
				center.y = (min.y + max.y) * 0.5f;
				extents.y = max.y - center.y;
			}
		}

		public float YMin {
			get {
				return min.y;
			}
			set {
				min.y = value;
				if(max.y < min.y) {
					float temp = max.y;
					min.y = max.y;
					max.y = temp;
				}
				center.y = (min.y + max.y) * 0.5f;
				extents.y = max.y - center.y;
			}
		}

		public Vector2 Size {
			get {
				return 2 * Extents;
			}
			set {
				Extents = 0.5f * value;
			}
		}

		public float Width {
			get {
				return Size.x;
			}
			set {
				Vector2 size = Size;
				size.x = value;
				Size = size;
			}
		}
		
		public float Height {
			get {
				return Size.y;
			}
			set {
				Vector2 size = Size;
				size.y = value;
				Size = size;
			}
		}

		public Bounds2D (Vector2 center, Vector2 size) {
			this.Center = center;
			this.Size = size.Abs();
		}

		public Bounds2D(Bounds bounds3d) {
			center = bounds3d.center;
			this.Size = bounds3d.size;
		}

		public bool Contains(Vector2 point) {
			return point.x >= min.x && point.y >= min.y && point.x <= max.x && point.y <= max.y;
		}

		public bool Contains(Vector3 point) {
			return point.x >= min.x && point.y >= min.y && point.x <= max.x && point.y <= max.y;
		}

		public override bool Equals (object obj) {
			if (obj is Bounds2D) {
				Bounds2D b = (Bounds2D)obj;
				return Center == b.Center && Size == b.Size;
			}
			return false;
		}

		public Vector2 ClosestPoint (Vector2 point) {
			float x = point.x;
			float y = point.y;
			if (x > max.x) {
				if(y > max.y) {
					return max;
				} else if (y < min.y) {
					return new Vector2 (max.x, min.y);
				} else {
					return new Vector2 (max.x, point.y);
				}
			} else if (x < min.x) {
				if(y > max.y) {
					return new Vector2 (min.x, max.y);
				} else if (y < min.y) {
					return min;
				} else {
					return new Vector2 (min.x, point.y);
				}
			} else {
				if(y > max.y) {
					return new Vector2 (point.x, max.y);
				} else if (y < min.y) {
					return new Vector2 (point.x , min.y);
				} else {
					return point;
				}
			}
		}

		public void Expand (Vector2 point) {
			float x = point.x;
			float y = point.y;
			Rect rect = (Rect)this;
			if (x > max.x) {
				rect.xMax = point.x;
			} else if (x < min.x) {
				rect.xMin = point.x;
			}
			if(y > max.y) {
				rect.xMax = point.x;
			} else if (y < min.y) {
				rect.xMin = point.x;
			}
		}

		public override int GetHashCode () {
			return Center.GetHashCode() + Extents.GetHashCode ();
		}

		public static explicit operator Rect(Bounds2D bounds) {
			return new Rect (bounds.min.x, bounds.max.y, bounds.Width, bounds.Height);
		}

		public static explicit operator Bounds2D(Rect rect) {
			return new Bounds2D (rect.center, rect.size);
		}

		public static implicit operator Bounds2D(Bounds bounds) {
			return new Bounds2D (bounds.center, bounds.size);
		}

		public static implicit operator Bounds(Bounds2D bounds) {
			return new Bounds (bounds.center, bounds.Size);
		}

		public static bool operator ==(Bounds2D b1, Bounds2D b2) {
			return b1.Center == b2.Center && b1.Size == b2.Size;
		}

		public static bool operator !=(Bounds2D b1, Bounds2D b2) {
			return !(b1 == b2);
		}
	}
}

