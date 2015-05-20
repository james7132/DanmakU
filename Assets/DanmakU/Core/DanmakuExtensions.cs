using UnityEngine;

using System.Collections.Generic;

namespace DanmakU {
	
	public static class DanmakuExtensions {

		#region IDanmakuController Enumerable Extensions

		public static DanmakuController Compress(this IEnumerable<IDanmakuController> controllers) {
			if(controllers == null)
				throw new System.NullReferenceException();

			DanmakuController controller = null;
			var list = controllers as IList<IDanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					IDanmakuController current = list[i];
					if(current != null)
						controller += current.Update;
				}
			} else {
				foreach(var current in controllers) {
					if(current != null)
						controller += current.Update;
				}
			}
			return controller;
		}

		#endregion

		#region DanmakuController Enumerable Functions

		public static DanmakuController Compress (this IEnumerable<DanmakuController> controllers) {
			if(controllers == null)
				throw new System.NullReferenceException();
			
			DanmakuController controller = null;
			var list = controllers as IList<DanmakuController>;
			if(list != null) {
				int count = list.Count;
				for(int i = 0; i < count; i++) {
					DanmakuController current = list[i];
					if(current != null)
						controller += current;
				}
			} else {
				foreach(var current in controllers) {
					if(current != null)
						controller += current;
				}
			}
			return controller;
		}

		#endregion

		#region Position Functions

		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point.
		/// All contained null objects will be ignored.
		/// 
		/// If the collection is <c>null</c>, this function does nothing.
		/// 
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <param name="danmakus">The enumerable collection of Danmaku. Will throw System.NullReferenceException if null.</param>
		/// <param name="position">the position to move the contained danmaku to, in absolute world coordinates.</param>
		public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Vector2 position) {
			if (danmakus == null)
				return null;
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
			return danmakus;
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
		public static IEnumerable<Danmaku> MoveTo (this IEnumerable<Danmaku> danmakus, IList<Vector2> positions) {
			if (danmakus == null)
				return null;
			if (positions == null)
				throw new System.ArgumentNullException ();
			int max = positions.Count;
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
			return danmakus;
		}
		
		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Transform's absolute world position.
		/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the transform is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Function does nothing if this is null.</param>
		/// <param name="transform">The Transform to move to.</param>
		public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Transform transform) {
			if (transform != null)
				MoveTo (danmakus, transform.position);
			return danmakus;
		}

		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Component's Transform's absolute world position.
		/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the Component is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Function does nothing if this is null.</param>
		/// <param name="component">The Component to move to.</param>
		public static IEnumerable<Danmaku> MoveTo (this IEnumerable<Danmaku> danmakus, Component component) {
			if (component == null)
				throw new System.ArgumentNullException ();
			if(danmakus != null)
				MoveTo (danmakus, component.transform.position);
			return danmakus;
		}
		
		/// <summary>
		/// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity GameObject's Transform's absolute world position.
			/// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if the position array is null.</exception>
		/// <param name="danmakus">The enumerable collection of Danmaku. Does nothing if it is null.</param>
		/// <param name="gameObject">The GameObject to move to. Will throw System.ArgumentNullException if null.</param>
		public static IEnumerable<Danmaku> MoveTo (this IEnumerable<Danmaku> danmakus, GameObject gameObject) {
			if (gameObject == null)
				throw new System.ArgumentNullException ();
			if(danmakus != null)
				MoveTo (danmakus, gameObject.transform.position);
			return danmakus;
		}

		/// <summary>
		/// Moves all of the Danmaku in the collection to a random 2D points within a specified rectangular area.
		/// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
		/// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
		/// All contained null objects will be ignored.
		/// See: <see cref="Danmaku.Position"/>
		/// </summary>
		/// <param name="danmakus">The enumerable collection of Danmaku. Does nothing if it is null.</param>
		/// <param name="area">The rectangular area to move the contained Danmaku to.</param>
		public static IEnumerable<Danmaku> MoveTo (this IEnumerable<Danmaku> danmakus, Rect area) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}

		public static IEnumerable<Danmaku> MoveTowards (this IEnumerable<Danmaku> danmakus, Vector2 target, float maxDistanceDelta) {
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
			return danmakus;
		}

		public static IEnumerable<Danmaku> MoveTowards (this IEnumerable<Danmaku> danmakus, Transform target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			return danmakus.MoveTowards (target.position, maxDistanceDelta);
		}

		public static IEnumerable<Danmaku> MoveTowards (this IEnumerable<Danmaku> danmakus, Component target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			return danmakus.MoveTowards (target.transform.position, maxDistanceDelta);
		}

		public static IEnumerable<Danmaku> MoveTowards (this IEnumerable<Danmaku> danmakus, GameObject target, float maxDistanceDelta) {
			if (target == null)
				throw new System.ArgumentNullException ();
			return danmakus.MoveTowards (target.transform.position, maxDistanceDelta);
		}

		/// <summary>
		/// Instantaneously translates all of the Danmaku in the collections by a specified change in position.
		/// All contained null objects will be ignored.
		/// </summary>
		/// <param name="danmakus">The enumerable collection of Danmaku. Does nothing if it is null.</param>
		/// <param name="deltaPos">The change in position.</param>
		public static IEnumerable<Danmaku> Translate (this IEnumerable<Danmaku> danmakus, Vector2 deltaPos) {
			if (danmakus == null)
				return null;
			
			//if movement is nonexistent, don't waste time and return immediately
			if (deltaPos == new Vector2 (0f, 0f))
				return danmakus;
			
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
			return danmakus;
		}
		
		#endregion
		
		#region Rotation Functions

		public static IEnumerable<Danmaku> RotateTo(this IEnumerable<Danmaku> danmakus, DynamicFloat rotation) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> RotateTo (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> rotations) {
			if (danmakus == null)
				return danmakus;
			if (rotations == null)
				throw new System.ArgumentNullException ();
			int max = rotations.Count;
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
			return danmakus;
		}

		public static IEnumerable<Danmaku> Rotate (this IEnumerable<Danmaku> danmakus, DynamicFloat delta) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region Speed Functions
		
		public static IEnumerable<Danmaku> Speed(this IEnumerable<Danmaku> danmakus, DynamicFloat velocity) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Speed (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> speeds) {
			if (danmakus == null)
				return null;
			if (speeds == null)
				throw new System.ArgumentNullException ();
			int max = speeds.Count;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Accelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region Angular Speed Functions
		
		public static IEnumerable<Danmaku> AngularSpeed(this IEnumerable<Danmaku> danmakus, DynamicFloat angularSpeed) {
			if (danmakus == null)
				return null;
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
			return null;
		}
		
		public static IEnumerable<Danmaku> AngularSpeed (this IEnumerable<Danmaku> danmakus, IList<DynamicFloat> angularSpeeds) {
			if (danmakus == null)
				return null;
			if (angularSpeeds == null)
				throw new System.ArgumentNullException ();
			int max = angularSpeeds.Count;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> AngularAccelerate (this IEnumerable<Danmaku> danmakus, DynamicFloat deltaSpeed) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return null;
		}
		
		#endregion
		
		#region Damage Functions
		
		public static IEnumerable<Danmaku> Damage (this IEnumerable<Danmaku> danmakus, DynamicInt damage) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Damage (this IEnumerable<Danmaku> danmakus, IList<DynamicInt> damages) {
			if (danmakus == null)
				return null;
			if (damages == null)
				throw new System.ArgumentNullException ();
			int max = damages.Count;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region Color Functions
		
		public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, Color color) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, IList<Color> colors) {
			if (danmakus == null)
				return null;
			if (colors == null)
				throw new System.ArgumentNullException ();
			int max = colors.Count;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, Gradient colors) {
			if (colors == null)
				throw new System.ArgumentNullException ();
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region Controller Functions
		
		public static IEnumerable<Danmaku> AddController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}

		public static IEnumerable<Danmaku> AddControllers (this IEnumerable<Danmaku> danmakus, IEnumerable<IDanmakuController> controllers) {
			return danmakus.AddController(controllers.Compress());
		}

		public static IEnumerable<Danmaku> AddControllers (this IEnumerable<Danmaku> danmakus, IEnumerable<DanmakuController> controllers) {
			return danmakus.AddController(controllers.Compress());
		}

		public static IEnumerable<Danmaku> RemoveController(this IEnumerable<Danmaku> danmakus, IDanmakuController controller) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> RemoveControllers (this IEnumerable<Danmaku> danmakus, IEnumerable<IDanmakuController> controllers) {
			return danmakus.RemoveController(controllers.Compress());
		}
		
		public static IEnumerable<Danmaku> RemoveControllers (this IEnumerable<Danmaku> danmakus, IEnumerable<DanmakuController> controllers) {
			return danmakus.RemoveController(controllers.Compress());
		}

		public static IEnumerable<Danmaku> AddController(this IEnumerable<Danmaku> danmakus, DanmakuController controller) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> RemoveController(this IEnumerable<Danmaku> danmakus, DanmakuController controller) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> ClearControllers(this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region General Functions

		public static IEnumerable<Danmaku> Active(this IEnumerable<Danmaku> danmakus, bool value) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}

		public static IEnumerable<Danmaku> Activate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Deactivate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> DeactivateImmediate (this IEnumerable<Danmaku> danmakus) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		#endregion
		
		#region Misc Functions
		
		public static IEnumerable<Danmaku> Tag(this IEnumerable<Danmaku> danmakus, string tag) {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> Layer(this IEnumerable<Danmaku> danmakus, int layer) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> BoundsCheck(this IEnumerable<Danmaku> danmakus, bool boundsCheck) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> CollisionCheck (this IEnumerable<Danmaku> danmakus, bool collisionCheck) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}
		
		public static IEnumerable<Danmaku> MatchPrefab(this IEnumerable<Danmaku> danmakus, DanmakuPrefab prefab) {
			if (danmakus == null)
				return null;
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
			return danmakus;
		}
		
		#endregion

	}
}

