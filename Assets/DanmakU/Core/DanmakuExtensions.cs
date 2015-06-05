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
		public static T MoveTo<T> (this T danmakus, Vector2 position) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
		public static T MoveTo<T> (this T danmakus, IList<Vector2> positions) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			if (positions == null)
				throw new System.ArgumentNullException ();
			int max = positions.Count;
			var arrayTest = danmakus as Danmaku[];
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
		public static T MoveTo<T> (this T danmakus, Transform transform) where T : class, IEnumerable<Danmaku> {
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
		public static T MoveTo<T> (this T danmakus, Component component) where T : class, IEnumerable<Danmaku> {
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
		public static T MoveTo<T> (this T danmakus, GameObject gameObject) where T : class, IEnumerable<Danmaku> {
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
		public static T MoveTo<T> (this T danmakus, Rect area) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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

		public static T MoveTowards<T> (this T danmakus, Vector2 target, float maxDistanceDelta) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				throw new System.NullReferenceException ();
			var arrayTest = danmakus as Danmaku[];
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

		public static T MoveTowards<T> (this T danmakus, Transform target, float maxDistanceDelta) where T : class, IEnumerable<Danmaku> {
			if (target == null)
				throw new System.ArgumentNullException ();
			return danmakus.MoveTowards (target.position, maxDistanceDelta);
		}

		public static T MoveTowards<T> (this T danmakus, Component target, float maxDistanceDelta) where T : class, IEnumerable<Danmaku> {
			if (target == null)
				throw new System.ArgumentNullException ();
			return danmakus.MoveTowards (target.transform.position, maxDistanceDelta);
		}

		public static T MoveTowards<T> (this T danmakus, GameObject target, float maxDistanceDelta) where T : class, IEnumerable<Danmaku> {
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
		public static T Translate<T> (this T danmakus, Vector2 deltaPos) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			
			//if movement is nonexistent, don't waste time and return immediately
			if (deltaPos == new Vector2 (0f, 0f))
				return danmakus;
			
			var arrayTest = danmakus as Danmaku[];
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

		public static T RotateTo<T> (this T danmakus, DynamicFloat rotation) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
		
		public static T RotateTo<T> (this T danmakus, IList<DynamicFloat> rotations) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return danmakus;
			if (rotations == null)
				throw new System.ArgumentNullException ();
			int max = rotations.Count;
			var arrayTest = danmakus as Danmaku[];
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

		public static T Rotate<T> (this T danmakus, DynamicFloat delta) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Speed<T> (this T danmakus, DynamicFloat velocity) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Speed<T> (this T danmakus, IList<DynamicFloat> speeds) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			if (speeds == null)
				throw new System.ArgumentNullException ();
			int max = speeds.Count;
			var arrayTest = danmakus as Danmaku[];
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
		
		public static T Accelerate<T> (this T danmakus, DynamicFloat deltaSpeed) where T : class, IEnumerable<Danmaku> {
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
		
		public static T AngularSpeed<T> (this T danmakus, DynamicFloat angularSpeed) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
			return danmakus;
		}
		
		public static T AngularSpeed<T> (this T danmakus, IList<DynamicFloat> angularSpeeds) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			if (angularSpeeds == null)
				throw new System.ArgumentNullException ();
			int max = angularSpeeds.Count;
			var arrayTest = danmakus as Danmaku[];
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
		
		public static T AngularAccelerate<T> (this T danmakus, DynamicFloat deltaSpeed)  where T : class, IEnumerable<Danmaku> {
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
		
		public static T Damage<T> (this T danmakus, DynamicInt damage) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Damage<T> (this T danmakus, IList<DynamicInt> damages) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Color<T> (this T danmakus, Color color) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Color<T> (this T danmakus, IList<Color> colors) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Color<T> (this T danmakus, Gradient colors) where T : class, IEnumerable<Danmaku> {
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
		
		public static T AddController<T> (this T danmakus, IDanmakuController controller) where T : class, IEnumerable<Danmaku> {
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

		public static T AddControllers<T> (this T danmakus, IEnumerable<IDanmakuController> controllers) where T : class, IEnumerable<Danmaku> {
			return danmakus.AddController(controllers.Compress());
		}

		public static T AddControllers<T> (this T danmakus, IEnumerable<DanmakuController> controllers) where T : class, IEnumerable<Danmaku> {
			return danmakus.AddController(controllers.Compress());
		}

		public static T RemoveController<T> (this T danmakus, IDanmakuController controller) where T : class, IEnumerable<Danmaku> {
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
		
		public static T RemoveControllers<T> (this T danmakus, IEnumerable<IDanmakuController> controllers) where T : class, IEnumerable<Danmaku> {
			return danmakus.RemoveController(controllers.Compress());
		}
		
		public static T RemoveControllers<T> (this T danmakus, IEnumerable<DanmakuController> controllers) where T : class, IEnumerable<Danmaku> {
			return danmakus.RemoveController(controllers.Compress());
		}

		public static T AddController<T> (this T danmakus, DanmakuController controller) where T : class, IEnumerable<Danmaku> {
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
		
		public static T RemoveController<T> (this T danmakus, DanmakuController controller) where T : class, IEnumerable<Danmaku> {
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
		
		public static T ClearControllers<T> (this T danmakus) where T : class, IEnumerable<Danmaku> {
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

		public static T Active<T> (this T danmakus, bool value) where T : class, IEnumerable<Danmaku> {
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

		public static T Activate<T> (this T danmakus) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Deactivate<T> (this T danmakus) where T : class, IEnumerable<Danmaku> {
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
		
		public static T DeactivateImmediate<T> (this T danmakus) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Tag<T> (this T danmakus, string tag) where T : class, IEnumerable<Danmaku> {
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
		
		public static T Layer<T> (this T danmakus, int layer) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
		
		public static T CollisionCheck<T> (this T danmakus, bool collisionCheck) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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
		
		public static T MatchPrefab<T> (this T danmakus, DanmakuPrefab prefab) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			var arrayTest = danmakus as Danmaku[];
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

		#region Fire Functions 

		public static T Fire<T>(this T danmakus, FireData data, bool useRotation  = true) where T : class, IEnumerable<Danmaku>{
			if (danmakus == null)
				return null;
			if (data == null)
				throw new System.ArgumentNullException("Fire Data cannot be null!");
			var arrayTest = danmakus as Danmaku[];
			Vector2 tempPos = data.Position;
			DynamicFloat tempRot = data.Rotation;
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null) {
						data.Position = danmaku.Position;
						if(useRotation)
							data.Rotation = danmaku.Rotation;
						data.Fire();
					}
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null) {
						data.Position = danmaku.Position;
						if(useRotation)
							data.Rotation = danmaku.Rotation;
						data.Fire();
					}
				}
			}
			data.Position = tempPos;
			data.Rotation = tempRot;
			return danmakus;
		}

		public static T Fire<T>(this T danmakus, FireBuilder builder, bool useRotation  = true) where T : class, IEnumerable<Danmaku> {
			if (danmakus == null)
				return null;
			if (builder == null)
				throw new System.ArgumentNullException("Fire Builder cannot be null!");
			var arrayTest = danmakus as Danmaku[];
			Vector2 tempPos = builder.Position;
			DynamicFloat tempRot = builder.Rotation;
			if (arrayTest != null) {
				for(int i = 0; i < arrayTest.Length; i++) {
					Danmaku danmaku = arrayTest[i];
					if(danmaku != null) {
						builder.Position = danmaku.Position;
						if(useRotation)
							builder.Rotation = danmaku.Rotation;
						builder.Fire();
					}
				}
			} else {
				foreach(var danmaku in danmakus) {
					if(danmaku != null) {
						builder.Position = danmaku.Position;
						if(useRotation)
							builder.Rotation = danmaku.Rotation;
						builder.Fire();
					}
				}
			}
			builder.Position = tempPos;
			builder.Rotation = tempRot;
			return danmakus;
		}


		#endregion
	}
}

