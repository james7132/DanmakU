// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;

namespace UnityUtilLib {
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
		}

		public Vector2 Min {
			get {
				return min;
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

