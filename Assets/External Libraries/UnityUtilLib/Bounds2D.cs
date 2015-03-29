using System;
using UnityEngine;

namespace UnityUtilLib {
	public struct Bounds2D {

		public Vector2 Center;
		public Vector2 Extents;

		public Vector2 Max {
			get {
				return Center + Extents;
			}
		}

		public Vector2 Min {
			get {
				return Center - Extents;
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

		public Bounds2D (Vector2 center, Vector2 size) {
			this.Center = center;
			this.Size = Util.Abs(size);
		}

		public bool Contains(Vector2 point) {
			Vector2 min = Center - Extents;
			Vector2 max = Center + Extents;
			if (point.x < min.x)
				return false;
			else if (point.y < min.y)
				return false;
			else if (point.x > max.x)
				return false;
			else if(point.y > max.y)
				return false;
			return true;
		}

		public override bool Equals (object obj) {
			if (obj is Bounds2D) {
				Bounds2D b = (Bounds2D)obj;
				return Center == b.Center && Size == b.Size;
			}
			return false;
		}

		public override int GetHashCode () {
			return Center.GetHashCode() + Extents.GetHashCode ();
		}

		public static implicit operator Bounds2D(Bounds bounds) {
			return new Bounds2D (bounds.center, bounds.size);
		}

		public static bool operator ==(Bounds2D b1, Bounds2D b2) {
			return b1.Center == b2.Center && b1.Size == b2.Size;
		}

		public static bool operator !=(Bounds2D b1, Bounds2D b2) {
			return !(b1 == b2);
		}
	}
}

