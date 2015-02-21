using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// Projectile data.
/// </summary>
public class ProjectileData {

	/// <summary>
	/// The quad mesh.
	/// </summary>
	private static Mesh quadMesh;

	/// <summary>
	/// The group.
	/// </summary>
	private ProjectileGroup group;

	/// <summary>
	/// The position.
	/// </summary>
	private Vector3 position = Vector3.zero;

	/// <summary>
	/// The rotation.
	/// </summary>
	private Quaternion rotation = Quaternion.identity;

	/// <summary>
	/// The scale.
	/// </summary>
	private Vector3 scale = Vector3.zero;

	/// <summary>
	/// The velocity.
	/// </summary>
	private float velocity = 0f;

	/// <summary>
	/// The angular velocity.
	/// </summary>
	private Quaternion angularVelocity = Quaternion.identity;

	/// <summary>
	/// The collision handler.
	/// </summary>
	private ProjectileCollisionHandler collisionHandler;
	
	public static Mesh ProjectileMesh {
		get {
			if(quadMesh == null) {
				//Magic numbers woooooo!
				quadMesh = new Mesh();
				Vector3[] verts = new Vector3[]{
					new Vector3( 1, 0, 1),
					new Vector3( 1, 0, -1),
					new Vector3(-1, 0, 1),
					new Vector3(-1, 0, -1),
				};
				Vector2[] uv = new Vector2[] {
					new Vector2(1, 1),
					new Vector2(1, 0),
					new Vector2(0, 1),
					new Vector2(0, 0),
				};
				int[] tris = new int[]{0, 1, 2, 2, 1, 3};
				quadMesh.vertices = verts;
				quadMesh.uv = uv;
				quadMesh.triangles = tris;
			}
			return quadMesh;
		}
	}

	/// <summary>
	/// Gets or sets the position.
	/// </summary>
	/// <value>The position.</value>
	public Vector3 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	/// <summary>
	/// Gets or sets the rotation.
	/// </summary>
	/// <value>The rotation.</value>
	public Quaternion Rotation {
		get {
			return rotation;
		}
		set {
			rotation = value;
		}
	}

	/// <summary>
	/// Gets or sets the scale.
	/// </summary>
	/// <value>The scale.</value>
	public Vector3 Scale {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	/// <summary>
	/// Gets or sets the velocity.
	/// </summary>
	/// <value>The velocity.</value>
	public float Velocity {
		get {
			return velocity;
		}
		set {
			velocity = value;
		}
	}

	/// <summary>
	/// Gets or sets the angular velocity.
	/// </summary>
	/// <value>The angular velocity.</value>
	public Quaternion AngularVelocity {
		get {
			return angularVelocity;
		}
		set {
			angularVelocity = value;
		}
	}

	/// <summary>
	/// Gets or sets the collision handler.
	/// </summary>
	/// <value>The collision handler.</value>
	public ProjectileCollisionHandler CollisionHandler {
		get {
			return collisionHandler;
		}
		set {
			collisionHandler = value;
		}
	}

	/// <summary>
	/// Gets or sets the group.
	/// </summary>
	/// <value>The group.</value>
	public ProjectileGroup Group {
		get {
			return group;
		}
		set {
			group = value;
		}
	}

	/// <summary>
	/// Draw this instance.
	/// </summary>
	public void Draw() {
		Graphics.DrawMesh (ProjectileMesh, Matrix4x4.TRS (position, rotation, scale), group.ProjectileMaterial, group.Layer, group.TargetCamera);
	}

	/// <summary>
	/// Update the specified dt.
	/// </summary>
	/// <param name="dt">Dt.</param>
	public void Update(float dt)
	{
		Quaternion newRotation = Quaternion.Slerp (rotation, rotation * angularVelocity, dt);
		Vector3 movementVector = velocity * dt * (newRotation * Vector3.up);

		//TODO: add support for ProjectileControllers here

		//RaycastHit2D[] hits = collisionHandler.CheckCollision (this, movementVector);

		rotation = newRotation;
		position += movementVector;
	}
}

/// <summary>
/// Projectile collision handler.
/// </summary>
public abstract class ProjectileCollisionHandler {
	public abstract RaycastHit2D[] CheckCollision(ProjectileData projectile, Vector3 movementVector);
}

/// <summary>
/// Box projectile collision handler.
/// </summary>
public class BoxProjectileCollisionHandler : ProjectileCollisionHandler {
	private Vector2 boxOffset;
	private Vector2 boxSize;

	/// <summary>
	/// Initializes a new instance of the <see cref="BoxProjectileCollisionHandler"/> class.
	/// </summary>
	/// <param name="offset">Offset.</param>
	/// <param name="size">Size.</param>
	public BoxProjectileCollisionHandler(Vector2 offset, Vector2 size) {
		boxOffset = offset;
		boxSize = size;
	}

	/// <summary>
	/// Checks the collision.
	/// </summary>
	/// <returns>The collision.</returns>
	/// <param name="projectile">Projectile.</param>
	/// <param name="movementVector">Movement vector.</param>
	public override RaycastHit2D[] CheckCollision (ProjectileData projectile, Vector3 movementVector) {
		Vector2 origin = Util.To2D (projectile.Position) + Util.ComponentProduct2 (projectile.Scale, boxOffset);
		Vector2 size = Util.ComponentProduct2 (projectile.Scale, boxSize);
		return Physics2D.BoxCastAll (origin, size, projectile.Rotation.eulerAngles.z, movementVector, movementVector.magnitude);
	}
}

/// <summary>
/// Circle projectile collision handler.
/// </summary>
public class CircleProjectileCollisionHandler : ProjectileCollisionHandler {
	private Vector2 circleOffset;
	private float circleRadius;

	/// <summary>
	/// Initializes a new instance of the <see cref="CircleProjectileCollisionHandler"/> class.
	/// </summary>
	/// <param name="offset">Offset.</param>
	/// <param name="radius">Radius.</param>
	public CircleProjectileCollisionHandler(Vector2 offset, float radius) {
		circleOffset = offset;
		circleRadius = radius;
	}

	/// <summary>
	/// Minimums the abs scale.
	/// </summary>
	/// <returns>The abs scale.</returns>
	/// <param name="values">Values.</param>
	private float MinAbsScale(params float[] values) {
		float min = float.MaxValue;
		for(int i = 0; i < values.Length; i++)
			if(values[i] < min)
				min = Mathf.Abs(values[i]);
		return min;
	}

	/// <summary>
	/// Checks the collision.
	/// </summary>
	/// <returns>The collision.</returns>
	/// <param name="projectile">Projectile.</param>
	/// <param name="movementVector">Movement vector.</param>
	public override RaycastHit2D[] CheckCollision (ProjectileData projectile, Vector3 movementVector) {
		Vector2 origin = Util.To2D (projectile.Position) + Util.ComponentProduct2 (projectile.Scale, circleOffset);
		float size = circleRadius * MinAbsScale (projectile.Scale.x, projectile.Scale.y, projectile.Scale.z);
		return Physics2D.CircleCastAll(origin, size, movementVector, movementVector.magnitude, projectile.Group.Layer);
	}
}
