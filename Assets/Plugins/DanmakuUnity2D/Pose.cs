using System;
using UnityUtilLib;
using UnityEngine;

namespace Danmaku2D {

	public struct Pose2D {

		public Vector2 Position;
		public float Rotation;

		public Pose2D(Vector2 position) {
			Position = position;
			Rotation = 0f;
		}

		public Pose2D(float rotation) {
			Position = Vector2.zero;
			Rotation = rotation;
		}

		public Pose2D(Vector2 position, float rotation) {
			Position = position;
			Rotation = rotation;
		}
		
		public Pose2D(Transform transform) {
			Position = transform.position;
			Rotation = transform.eulerAngles.z;
		}
		
		public Pose2D(Transform2D transform) {
			Position = transform.Position;
			Rotation = transform.Rotation;
		}

		public static explicit operator Pose2D(Vector2 position) {
			return new Pose2D (position);
		}

		public static explicit operator Pose2D(float rotation) {
			return new Pose2D (rotation);
		}

		public static explicit operator Pose2D(Transform transform) {
			return new Pose2D (transform);
		}

		public static explicit operator Pose2D(Transform2D transform) {
			return new Pose2D (transform);
		}
		
		public static Pose2D operator + (Pose2D p1, Pose2D p2) {
			return new Pose2D (p1.Position + p2.Position, p1.Rotation + p2.Rotation);
		}
		
		public static Pose2D operator - (Pose2D p1, Pose2D p2) {
			return new Pose2D (p1.Position - p2.Position, p1.Rotation - p2.Rotation);
		}

		public static Pose2D operator -(Pose2D pose) {
			return new Pose2D (-pose.Position, pose.Rotation + 180f);
		}

//		public Pose2D(Pose3D pose) {
//			Position = pose.Position;
//			Rotation = pose.Rotation.z;
//		}
	}

//	public struct Pose3D {
//
//		public Vector3 Position;
//		public Quaternion Rotation;
//		
//		public Pose3D(Vector3 position) {
//			Position = position;
//			Rotation = Quaternion.identity;
//		}
//		
//		public Pose3D(Vector3 position, Quaternion rotation) {
//			Position = position;
//			Rotation = rotation;
//		}
//		
//		public Pose3D(Pose2D pose) {
//			Position = pose.Position;
//			Rotation = Quaternion.Euler (0f, 0f, pose.Rotation);
//		}
//		
//		public Pose3D(Transform transform) {
//			Position = transform.position;
//			Rotation = transform.rotation;
//		}
//		
//		public Pose3D(Transform2D transform) {
//			Position = transform.Position;
//			Rotation = transform.transform.rotation;
//		}
//	}
}

