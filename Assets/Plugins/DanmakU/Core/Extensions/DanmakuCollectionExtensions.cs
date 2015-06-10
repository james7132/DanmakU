// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public static class DanmakuCollectionExtensions {

        private static void UniformAction(IEnumerable<Danmaku> danmakus, Action<Danmaku> action) {
            if (danmakus == null)
                return;
            var arrayTest = danmakus as Danmaku[];
            if (arrayTest != null) {
                foreach (var danmaku in arrayTest) {
                    if (danmaku != null)
                        action(danmaku);
                }
            } else {
                foreach (var danmaku in danmakus) {
                    if (danmaku != null)
                        action(danmaku);
                }
            }
        }

        #region Position Functions

        /// <summary>
        /// Moves all of the Danmaku in the collection to a specified 2D point.
        /// </summary>
        /// <remarks>
        /// All contained null objects will be ignored.
        /// If the collection is <c>null</c>, this function does nothing and returns null.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <param name="danmakus">The enumerable collection of Danmaku. Will throw NullReferenceException if null.</param>
        /// <param name="position">the position to move the contained danmaku to, in absolute world coordinates.</param>
        public static T MoveTo<T>(this T danmakus, Vector2 position)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Position = position; });
            return danmakus;
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to random 2D points specified by a array of 2D positions.
        /// </summary>
        /// <remarks>
        /// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <param name="danmakus">The enumerable collection of Danmaku. Will throw NullReferenceException if null.</param>
        /// <param name="positions">the potential positions to move the contained danmaku to, in absolute world coordinates.</param>
        /// <exception cref="ArgumentNullException">Thrown if the position array is null.</exception>
        public static T MoveTo<T>(this T danmakus, ICollection<Vector2> positions)
            where T : class, IEnumerable<Danmaku> {
            if (positions == null)
                throw new ArgumentNullException("positions");
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Position = positions.Random(); });
            return danmakus;
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Transform's absolute world position.
        /// </summary>
        /// <remarks>
        /// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the transform is null.</exception>
        /// <param name="danmakus">The enumerable collection of Danmaku. Function does nothing if this is null.</param>
        /// <param name="transform">The Transform to move to.</param>
        public static T MoveTo<T>(this T danmakus, Transform transform)
            where T : class, IEnumerable<Danmaku> {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return danmakus.MoveTo(transform.position);
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity Component's Transform's absolute world position.
        /// </summary>
        /// <remarks>
        /// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the Component is null.</exception>
        /// <param name="danmakus">The enumerable collection of Danmaku. Function does nothing if this is null.</param>
        /// <param name="component">The Component to move to.</param>
        public static T MoveTo<T>(this T danmakus, Component component)
            where T : class, IEnumerable<Danmaku> {
            if (component == null)
                throw new ArgumentNullException("component");
            return danmakus.MoveTo(component.transform.position);
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to a specified 2D point based on a Unity GameObject's Transform's absolute world position.
        /// </summary>
        /// <remarks>
        /// This function discards the Z axis and will place the Danmaku at the corresponding 2D location on the Z = 0 plane.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the position array is null.</exception>
        /// <param name="danmakus">The enumerable collection of Danmaku. Does nothing if it is null.</param>
        /// <param name="gameObject">The GameObject to move to. Will throw ArgumentNullException if null.</param>
        public static T MoveTo<T>(this T danmakus, GameObject gameObject)
            where T : class, IEnumerable<Danmaku> {
            if (gameObject == null)
                throw new ArgumentNullException("gameObject");
            return danmakus.MoveTo(gameObject.transform.position);
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to a random 2D points within a specified rectangular area.
        /// </summary>
        /// <remarks>
        /// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <param name="danmakus">The enumerable collection of Danmaku. This function does nothing if it is null.</param>
        /// <param name="area">The rectangular area to move the contained Danmaku to.</param>
        public static T MoveTo<T>(this T danmakus, Rect area)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Position = area.RandomPoint(); });
            return danmakus;
        }

        /// <summary>
        /// Moves all of the Danmaku in a collection towards a specified point in space.
        /// </summary>
        /// <param name="danmakus">The enumerable collection of Danmaku. This function does nothing if it is null.</param>
        /// <param name="target">The target point in space to move towards, in absolute world coordinates.</param>
        /// <param name="maxDistanceDelta">The maximum distance to move.</param>
        /// <typeparam name="T">The type of the collection.</typeparam>
        public static T MoveTowards<T>(this T danmakus,
                                       Vector2 target,
                                       float maxDistanceDelta)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.MoveTowards(target, maxDistanceDelta); });
            return danmakus;
        }

        public static T MoveTowards<T>(this T danmakus,
                                       Transform target,
                                       float maxDistanceDelta)
            where T : class, IEnumerable<Danmaku> {
            if (target == null)
                throw new ArgumentNullException("target");
            return danmakus.MoveTowards(target.position, maxDistanceDelta);
        }

        public static T MoveTowards<T>(this T danmakus,
                                       Component target,
                                       float maxDistanceDelta)
            where T : class, IEnumerable<Danmaku> {
            if (target == null)
                throw new ArgumentNullException("target");
            return danmakus.MoveTowards(target.transform.position,
                                        maxDistanceDelta);
        }

        public static T MoveTowards<T>(this T danmakus,
                                       GameObject target,
                                       float maxDistanceDelta)
            where T : class, IEnumerable<Danmaku> {
            if (target == null)
                throw new ArgumentNullException("target");
            return danmakus.MoveTowards(target.transform.position,
                                        maxDistanceDelta);
        }

        /// <summary>
        /// Instantaneously translates all of the Danmaku in the collections by a specified change in position.
        /// All contained null objects will be ignored.
        /// </summary>
        /// <param name="danmakus">The enumerable collection of Danmaku. Does nothing if it is null.</param>
        /// <param name="deltaPos">The change in position.</param>
        public static T Translate<T>(this T danmakus, Vector2 deltaPos)
            where T : class, IEnumerable<Danmaku> {
            if (deltaPos != Vector2.zero) {
                UniformAction(danmakus,
                              delegate(Danmaku danmaku) { danmaku.Position += deltaPos; });
            }
            return danmakus;
        }

        #endregion

        #region Rotation Functions

        public static T RotateTo<T>(this T danmakus, DynamicFloat rotation)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Rotation = rotation; });
            return danmakus;
        }

        public static T RotateTo<T>(this T danmakus,
                                    ICollection<DynamicFloat> rotations)
            where T : class, IEnumerable<Danmaku> {
            if (rotations == null)
                throw new ArgumentNullException("rotations");
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Rotation = rotations.Random(); });
            return danmakus;
        }

        public static T Rotate<T>(this T danmakus, DynamicFloat delta)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Rotation += delta; });
            return danmakus;
        }

        #endregion

        #region Speed Functions

        public static T Speed<T>(this T danmakus, DynamicFloat speed)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Speed = speed; });
            return danmakus;
        }

        public static T Speed<T>(this T danmakus, ICollection<DynamicFloat> speeds)
            where T : class, IEnumerable<Danmaku> {
            if (speeds == null)
                throw new ArgumentNullException("speeds");
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Speed = speeds.Random(); });
            return danmakus;
        }

        public static T Accelerate<T>(this T danmakus, DynamicFloat deltaSpeed)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Speed += deltaSpeed; });
            return danmakus;
        }

        #endregion

        #region Angular Speed Functions

        public static T AngularSpeed<T>(this T danmakus,
                                        DynamicFloat angularSpeed)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.AngularSpeed = angularSpeed; });
            return danmakus;
        }

        public static T AngularSpeed<T>(this T danmakus,
                                        ICollection<DynamicFloat> angularSpeeds)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.AngularSpeed = angularSpeeds.Random(); });
            return danmakus;
        }

        public static T AngularAccelerate<T>(this T danmakus,
                                             DynamicFloat deltaSpeed)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.AngularSpeed += deltaSpeed; });
            return danmakus;
        }

        #endregion

        #region Damage Functions

        public static T Damage<T>(this T danmakus, DynamicInt damage)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Damage = damage; });
            return danmakus;
        }

        public static T Damage<T>(this T danmakus, ICollection<DynamicInt> damages)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.AngularSpeed = damages.Random(); });
            return danmakus;
        }

        #endregion

        #region Color Functions

        public static T Color<T>(this T danmakus, Color color)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Color = color; });
            return danmakus;
        }

        public static T Color<T>(this T danmakus, ICollection<Color> colors)
            where T : class, IEnumerable<Danmaku> {
            if (colors == null)
                throw new ArgumentNullException("colors");
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Color = colors.Random(); });
            return danmakus;
        }

        public static T Color<T>(this T danmakus, Gradient colors)
            where T : class, IEnumerable<Danmaku> {
            if (colors == null)
                throw new ArgumentNullException("colors");
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Color = colors.Random(); });
            return danmakus;
        }

        #endregion

        #region Controller Functions

        public static T AddController<T>(this T danmakus,
                                         IDanmakuController controller)
            where T : class, IEnumerable<Danmaku> {
            DanmakuController controllerUpdate = controller.Update;
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.ControllerUpdate += controllerUpdate; });
            return danmakus;
        }

        public static T AddControllers<T>(this T danmakus,
                                          IEnumerable<IDanmakuController> controllers)
            where T : class, IEnumerable<Danmaku> {
            return danmakus.AddController(controllers.Compress());
        }

        public static T AddControllers<T>(this T danmakus,
                                          IEnumerable<DanmakuController> controllers)
            where T : class, IEnumerable<Danmaku> {
            return danmakus.AddController(controllers.Compress());
        }

/*        public static T RemoveController<T>(this T danmakus,
                                            IDanmakuController controller)
            where T : class, IEnumerable<Danmaku> {
            if (danmakus == null)
                return null;
            var arrayTest = danmakus as Danmaku[];
            DanmakuController controlleDelegate = controller.Update;
            if (arrayTest != null) {
                foreach (var danmaku in arrayTest) {
                    if (danmaku != null)
                        danmaku.RemoveController(controlleDelegate);
                }
            } else {
                foreach (var danmaku in danmakus) {
                    if (danmaku != null)
                        danmaku.RemoveController(controlleDelegate);
                }
            }
            return danmakus;
        }

        public static T RemoveControllers<T>(this T danmakus,
                                             IEnumerable<IDanmakuController> controllers)
            where T : class, IEnumerable<Danmaku> {
            return danmakus.RemoveController(controllers.Compress());
        }

        public static T RemoveControllers<T>(this T danmakus,
                                             IEnumerable<DanmakuController> controllers)
            where T : class, IEnumerable<Danmaku> {
            return danmakus.RemoveController(controllers.Compress());
        }*/

        public static T AddController<T>(this T danmakus,
                                         DanmakuController controller)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.ControllerUpdate += controller; });
            return danmakus;
        }

/*        public static T RemoveController<T>(this T danmakus,
                                            DanmakuController controller)
            where T : class, IEnumerable<Danmaku> {
            if (danmakus == null)
                return null;
            var arrayTest = danmakus as Danmaku[];
            if (arrayTest != null) {
                foreach (var danmaku in arrayTest) {
                    if (danmaku != null)
                        danmaku.RemoveController(controller);
                }
            } else {
                foreach (var danmaku in danmakus) {
                    if (danmaku != null)
                        danmaku.RemoveController(controller);
                }
            }
            return danmakus;
        }*/

        public static T ClearControllers<T>(this T danmakus)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.ClearControllers(); });
            return danmakus;
        }

        #endregion

        #region General Functions

        public static T Active<T>(this T danmakus, bool value)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.IsActive = value; });
            return danmakus;
        }

        public static T Activate<T>(this T danmakus)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Activate(); });
            return danmakus;
        }

        public static T Deactivate<T>(this T danmakus)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Deactivate(); });
            return danmakus;
        }

        public static T DeactivateImmediate<T>(this T danmakus)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.DeactivateImmediate(); });
            return danmakus;
        }

        #endregion

        #region Misc Functions

        public static T Tag<T>(this T danmakus, string tag)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Tag = tag; });
            return danmakus;
        }

        public static T Layer<T>(this T danmakus, int layer)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.Layer = layer; });
            return danmakus;
        }

        public static T CollisionCheck<T>(this T danmakus, bool collisionCheck)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.CollisionCheck = collisionCheck; });
            return danmakus;
        }

        public static T MatchPrefab<T>(this T danmakus, DanmakuPrefab prefab)
            where T : class, IEnumerable<Danmaku> {
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) { danmaku.MatchPrefab(prefab); });
            return danmakus;
        }

        #endregion

        #region Fire Functions 

        public static T Fire<T>(this T danmakus,
                                FireData data,
                                bool useRotation = true)
            where T : class, IEnumerable<Danmaku> {
            if (data == null)
                throw new ArgumentNullException("data");
            Vector2 tempPos = data.Position;
            DynamicFloat tempRot = data.Rotation;
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) {
                              data.Position = danmaku.Position;
                              if (useRotation)
                                  data.Rotation = danmaku.Rotation;
                              data.Fire();
                          });
            data.Position = tempPos;
            data.Rotation = tempRot;
            return danmakus;
        }

        public static T Fire<T>(this T danmakus,
                                FireBuilder builder,
                                bool useRotation = true)
            where T : class, IEnumerable<Danmaku> {
            if (danmakus == null)
                return null;
            if (builder == null)
                throw new ArgumentNullException("builder");
            Vector2 tempPos = builder.Position;
            DynamicFloat tempRot = builder.Rotation;
            UniformAction(danmakus,
                          delegate(Danmaku danmaku) {
                              builder.Position = danmaku.Position;
                              if (useRotation)
                                  builder.Rotation = danmaku.Rotation;
                              builder.Fire();
                          });
            builder.Position = tempPos;
            builder.Rotation = tempRot;
            return danmakus;
        }

        #endregion
    }

}