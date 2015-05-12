using UnityEngine;

using System.Collections.Generic;

namespace DanmakU {

	public static class DanmakuExtensions {

		//TODO: Implement and test potential higher performance versions of these functions for Arrays.

		#region Position Functions

		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception> 
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="position">the position to move the contained danmaku to, in absolute world coordinates.</param>
		public static void SetPosition(this IEnumerable<Danmaku> danmakus, Vector2 position) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Position = position;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Position = position;
				}
			}
		}

		/// <summary>
		/// Moves all of the Danmaku in the collection to random 2D points specified by a array of 2D positions.
		/// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if the position array is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="position">the potential positions to move the contained danmaku to, in absolute world coordinates.</param>
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, IList<Vector2> positions) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (positions == null)
				throw new System.ArgumentNullException ();
			int max = positions.Length;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Position = positions[Random.Range(0, max)];
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Position = positions[Random.Range(0, max)];
				}
			}
		}
		
		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Transform's absolute world position.
		/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if the transform is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="transform">The Transform to move to.</param>
		public static void SetPosition(this IEnumerable<Danmaku> danmakus, Transform transform) {
			if (transform == null)
				throw new System.ArgumentNullException ();
			SetPosition (danmakus, transform.position);
		}

		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Component's Transform's absolute world position.
		/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if the Component is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="component">The Component to move to.</param>
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, Component component) {
			if (component == null)
				throw new System.ArgumentNullException ();
			SetPosition (danmakus, component.transform.position);
		}
		
		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity GameObject's Transform's absolute world position.
		/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if the position array is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="gameObject">The GameObject to move to. Will throw System.ArgumentNullException if null.</param>
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, GameObject gameObject) {
			if (gameObject == null)
				throw new System.ArgumentNullException ();
			SetPosition (danmakus, gameObject.transform.position);
		}

		/// <summary>
		/// Moves all of the Danmaku in the collection to a random 2D points within a specified rectangular area.
		/// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="area">The rectangular area to move the contained Danmaku to.</param>
		public static void SetPosition (this IEnumerable<Danmaku> danmakus, Rect area) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Position = area.RandomPoint();
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Position = area.RandomPoint();
				}
			}
		}

		public static void MoveTowards (this IEnumerable<Danmaku> danmakus, Vector2 target, float maxDistanceDelta) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.MoveTowards(target, maxDistanceDelta);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.MoveTowards(target, maxDistanceDelta);
				}
			}
		}

		public static void MoveTowards (this IEnumerable<Danmaku> danmakus, Transform target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			danmakus.MoveTowards (target.position, maxDistanceDelta);
		}

		public static void MoveTowards (this IEnumerable<Danmaku> danmakus, Component target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			danmakus.MoveTowards (target.transform.position, maxDistanceDelta);
		}

		public static void MoveTowards (this IEnumerable<Danmaku> danmakus, GameObject target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			danmakus.MoveTowards (target.transform.position, maxDistanceDelta);
		}

		/// <summary>
		/// Instantaneously translates all of the Danmaku in the collections by a specified change in position.
		/// All contained null objects will be ignored.
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="deltaPos">The change in position.</param>
		public static void Translate (this IEnumerable<Danmaku> danmakus, Vector2 deltaPos) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			
			//if movement is nonexistent, don't waste time and return immediately
			if (deltaPos == new Vector2 (0f, 0f))
				return;
			
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Position += deltaPos;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Position += deltaPos;
				}
			}
		}
		
		#endregion
		
		#region Rotation Functions

		/// <summary>
		/// Rotates the 
		/// </summary>
		/// <exception cref="System.NullReferenceException">Thrown if the input collection is null.</exception>
		/// <param name="danmakus">Danmakus.</param>
		/// <param name="rotation">Rotation.</param>
		public static void SetRotation(this IEnumerable<Danmaku> danmakus, DynamicFloat rotation) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Rotation = rotation.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Rotation = rotation.Value;
				}
			}
		}
		
		public static void SetRotation (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> rotations) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (rotations == null)
				throw new System.ArgumentNullException ();
			int max = rotations.Length;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Rotation = rotations[Random.Range(0, max)];
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Rotation = rotations[Random.Range(0, max)];
				}
			}
		}

		public static void Rotate(this IEnumerable<Danmaku> danmakus, DynamicFloat delta) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Rotation += delta;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Rotation += delta;
				}
			}
		}
		
		#endregion
		
		#region Speed Functions
		
		public static void SetSpeed(this IEnumerable<Danmaku> danmakus, DynamicFloat velocity) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Speed = velocity.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Speed = velocity.Value;
				}
			}
		}
		
		public static void SetSpeed (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> speeds) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (speeds == null)
				throw new System.ArgumentNullException ();
			int max = speeds.Length;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Speed = speeds[Random.Range(0, max)];
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Speed = speeds[Random.Range(0, max)];
				}
			}
		}
		
		public static void Accelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Speed += deltaSpeed.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Speed += deltaSpeed.Value;
				}
			}
		}
		
		#endregion
		
		#region Angular Speed Functions
		
		public static void SetAngularSpeed(this IEnumerable<Danmaku> danmakus, DynamicFloat angularSpeed) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.AngularSpeed = angularSpeed.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.AngularSpeed = angularSpeed.Value;
				}
			}
		}
		
		public static void SetAngularSpeed (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> angularSpeeds) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (angularSpeeds == null)
				throw new System.ArgumentNullException ();
			int max = angularSpeeds.Length;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.AngularSpeed = angularSpeeds[Random.Range(0, max)];
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.AngularSpeed = angularSpeeds[Random.Range(0, max)];
				}
			}
		}
		
		public static void AngularAccelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.AngularSpeed += deltaSpeed.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.AngularSpeed += deltaSpeed.Value;
				}
			}
		}
		
		#endregion
		
		#region Damage Functions
		
		public static void SetDamage (this IEnumerable<Danmaku> danmakus, DynamicInt damage) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Damage = damage.Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Damage = damage.Value;
				}
			}
		}
		
		public static void SetDamage (this IEnumerable<Danmaku> danmakus, IList<DynamicInt> damages) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (damages == null)
				throw new System.ArgumentNullException ();
			int max = damages.Count;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Damage = damages[Random.Range(0, max)].Value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Damage = damages[Random.Range(0, max)].Value;
				}
			}
		}
		
		#endregion
		
		#region Color Functions
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, Color color) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Color = color;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Color = color;
				}
			}
		}
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, IList<Color> colors) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			if (colors == null)
				throw new System.ArgumentNullException ();
			int max = colors.Count;
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Color = colors[Random.Range(0, max)];
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Color = colors[Random.Range(0, max)];
				}
			}
		}
		
		public static void SetColor(this IEnumerable<Danmaku> danmakus, Gradient colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Color = colors.Evaluate(Random.value);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Color = colors.Evaluate(Random.value);
				}
			}
		}
		
		#endregion
		
		#region Controller Functions
		
		public static void AddController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			DanmakuController controlleDelegate = controller.Update;
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.AddController(controlleDelegate);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.AddController(controlleDelegate);
				}
			}
		}
		
		public static void RemoveController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			DanmakuController controlleDelegate = controller.Update;
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.RemoveController(controlleDelegate);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.RemoveController(controlleDelegate);
				}
			}
		}

		public static void AddController(this IEnumerable<Danmaku> danmakus, DanmakuController controller) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.AddController(controller);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.AddController(controller);
				}
			}
		}
		
		public static void RemoveController(this IEnumerable<Danmaku> danmakus, DanmakuController controller) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.RemoveController(controller);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.RemoveController(controller);
				}
			}
		}
		
		public static void ClearControllers(this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.ClearControllers();
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.ClearControllers();
				}
			}
		}
		
		#endregion
		
		#region General Functions

		public static void SetActive(this IEnumerable<Danmaku> danmakus, bool value) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.IsActive = value;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.IsActive = value;
				}
			}
		}

		public static void Activate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Activate();
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Activate();
				}
			}
		}
		
		public static void Deactivate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Deactivate();
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Deactivate();
				}
			}
		}
		
		public static void DeactivateImmediate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.DeactivateImmediate();
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.DeactivateImmediate();
				}
			}
		}
		
		#endregion
		
		#region Misc Functions
		
		public static void SetTag(this IEnumerable<Danmaku> danmakus, string tag) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Tag = tag;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Tag = tag;
				}
			}
		}
		
		public static void SetLayer(this IEnumerable<Danmaku> danmakus, int layer) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.Layer = layer;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.Layer = layer;
				}
			}
		}
		
		public static void SetBoundsCheck(this IEnumerable<Danmaku> danmakus, bool boundsCheck) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.BoundsCheck = boundsCheck;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.BoundsCheck = boundsCheck;
				}
			}
		}
		
		public static void SetCollisionCheck (this IEnumerable<Danmaku> danmakus, bool collisionCheck) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.CollisionCheck = collisionCheck;
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.CollisionCheck = collisionCheck;
				}
			}
		}
		
		public static void MatchPrefab(this IEnumerable<Danmaku> danmakus, DanmakuPrefab prefab) {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			Danmaku[] arrayTest = danmakus as Danmaku[];
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null)
						danmaku.MatchPrefab(prefab);
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null)
						danmaku.MatchPrefab(prefab);
				}
			}
		}
		
		#endregion

	}
}

