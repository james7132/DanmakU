using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An abstract class implementation of IProjectileController.
	/// </summary>
	public abstract class ProjectileController : IProjectileController {
		#region IProjectileController implementation

		/// <summary>
		/// Updates the Projectile controlled by the controller instance.
		/// </summary>
		/// <returns>the displacement from the Projectile's original position after udpating</returns>
		/// <param name="dt">the change in time since the last update</param>
		public abstract void UpdateProjectile (float dt);

		/// <summary>
		/// Gets or sets the projectile controlled by this controller.
		/// </summary>
		/// <value>The projectile controlled by this controller</value>
		public Projectile Projectile {
			get;
			set;
		}
		
		#endregion

		/// <summary>
		/// Gets or sets the damage of the Projecitle controlled by this controller.
		/// See <see cref="Projectile.Damage"/>.
		/// </summary>
		/// <value>The damage of the Projectile</value>
		public int Damage {
			get {
				return Projectile.Damage;
			}
			set {
				Projectile.Damage = value;
			}
		}

		/// <summary>
		/// Gets the sprite of the Projectile controlled by this controller.
		/// See <see cref="Projectile.Sprite"/>.
		/// </summary>
		/// <value>The sprite of the Projectile controlled by this controller.</value>
		public Sprite Sprite {
			get {
				return Projectile.Sprite;
			}
		}

		/// <summary>
		/// Gets or sets the color of the Projectile controlled by this controller.
		/// See <see cref="Projectile.Color"/>.
		/// </summary>
		/// <value>The color of the Projectile controlled by this contorller.</value>
		public Color Color {
			get {
				return Projectile.Color;
			}
			set {
				Projectile.Color = value;
			}
		}

		/// <summary>
		/// Gets or sets the absolute world coordinate of the position of where the Projectile contorlled by this controller is.
		/// See <see cref="Projectile.Position"/>.
		/// </summary>
		/// <value>The absolute world coordinate of the position of where the Projectile contorlled by this controller is.</value>
		public Vector2 Position {
			get {
				return Projectile.Position;
			}
			set {
				Projectile.Position = value;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the Projectile controlled by this controller in degrees.
		/// See <see cref="Projectile.Rotation"/>.
		/// </summary>
		/// <value>The rotation of the Projectile controlled by this controller in degrees./value>
		public float Rotation {
			get {
				return Projectile.Rotation;
			}
			set {
				Projectile.Rotation = value;
			}
		}

		/// <summary>
		/// Gets the direction that Projectile controlled by this controller is moving in.
		/// See <see cref="Projectile.Direction"/>.
		/// </summary>
		/// <value>The direction that Projectile controlled by this controller is moving in.</value>
		public Vector2 Direction {
			get {
				return Projectile.Direction;
			}
		}

		/// <summary>
		/// Gets the time since the Projectile controlled by this controller was fired.
		/// See <see cref="Projectile.Time"/>.
		/// </summary>
		/// <value>The time since the Projectile controlled by this controller was fired.</value>
		public float Time {
			get {
				return Projectile.Time;
			}
		}

		/// <summary>
		/// Gets the number of frames that have passed since the Projectile controlled by this controller was fired.
		/// See <see cref="Projectile.Frames"/>. 
		/// </summary>
		/// <value>The number of frames that have passed since the Projectile controlled by this controller was fired.</value>
		public int Frames {
			get {
				return Projectile.Frames;
			}
		}


	}
}