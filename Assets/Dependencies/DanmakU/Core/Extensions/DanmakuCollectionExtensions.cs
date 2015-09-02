// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hourai.DanmakU {

    public static class DanmakuCollectionExtensions {

        public static IEnumerable<Danmaku> ForEach(this IEnumerable<Danmaku> danmakus,
                                                       Action<Danmaku> action,
                                                       Func<Danmaku, bool> filter = null) {
            if (danmakus == null)
                return null;
            if (action == null)
                throw new ArgumentNullException("action");
            foreach (Danmaku danmaku in danmakus) {
                if (danmaku && (filter == null || filter(danmaku)))
                    action(danmaku);
            }
            return danmakus;
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
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Vector2 position, Func<Danmaku, bool> filter = null)
        {
            return danmakus.ForEach(x => x.Position = position, filter);
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
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, ICollection<Vector2> positions, Func<Danmaku, bool> filter = null)
            {
            if (positions == null)
                throw new ArgumentNullException("positions");
            return danmakus.ForEach(x => x.Position = positions.Random(), filter);
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
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Transform transform, Func<Danmaku, bool> filter = null)
            {
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
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Component component, Func<Danmaku, bool> filter = null)
            {
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
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, GameObject gameObject, Func<Danmaku, bool> filter = null)
            {
            if (gameObject == null)
                throw new ArgumentNullException("gameObject");
            return danmakus.MoveTo(gameObject.transform.position);
        }

        /// <summary>
        /// Moves all of the Danmaku in the collection to a random 2D points within a specified rectangular area.
        /// </summary>
        /// <remarks>
        /// Positions are chosen randomly and independently for each Danmaku from a uniform distribution within a specified Rect.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// The Danmaku objects can be filtered with the <paramref name="filter"/> parameter. If it returns <c>true</c> for an Danmaku, then it is moved.
        /// If <paramref name="filter"/> is null, all Danmaku are moved.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <param name="danmakus">The enumerable collection of Danmaku. This function does nothing if it is null.</param>
        /// <param name="area">The rectangular area to move the contained Danmaku to.</param>
        /// <param name="filter">a function to filter whether or not to apply the action. Returns true if it should. Defaults to null.</param>
        public static IEnumerable<Danmaku> MoveTo(this IEnumerable<Danmaku> danmakus, Rect area, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Position = area.RandomPoint(), filter);
        }

        /// <summary>
        /// Moves all of the Danmaku in a collection towards a specified point in space.
        /// </summary>
        /// <remarks>
        /// Positions are chosen randomly and independently for each Danmaku from a uniform distribution.
        /// This function is not thread-safe: it can only be called from the Unity main thread as it utilizes Unity API calls.
        /// All contained null objects will be ignored.
        /// The Danmaku objects can be filtered with the <paramref name="filter"/> parameter. If it returns <c>true</c> for an Danmaku, then it is moved.
        /// If <paramref name="filter"/> is null, all Danmaku are moved.
        /// See: <see cref="Danmaku.Position"/>
        /// </remarks>
        /// <param name="danmakus">The enumerable collection of Danmaku. This function does nothing if it is null.</param>
        /// <param name="target">The target point in space to move towards, in absolute world coordinates.</param>
        /// <param name="maxDistanceDelta">The maximum distance to move.</param>
        /// <param name="filter">a function to filter whether or not to apply the action. Returns true if it should. Defaults to null.</param>
        /// <typeparam name="IEnumerable<Danmaku>">The type of the collection.</typeparam>
        public static IEnumerable<Danmaku> MoveTowards(this IEnumerable<Danmaku> danmakus,
                                       Vector2 target,
                                       float maxDistanceDelta,
                                       Func<Danmaku, bool> filter = null)
        {
            return danmakus.ForEach(x => x.MoveTowards(target, maxDistanceDelta), filter);
        }

        public static IEnumerable<Danmaku> MoveTowards(this IEnumerable<Danmaku> danmakus,
                                       Transform target,
                                       float maxDistanceDelta,
                                       Func<Danmaku, bool> filter = null)
            {
            if (target == null)
                throw new ArgumentNullException("target");
            return danmakus.MoveTowards(target.position, maxDistanceDelta);
        }

        public static IEnumerable<Danmaku> MoveTowards(this IEnumerable<Danmaku> danmakus,
                                       Component target,
                                       float maxDistanceDelta,
                                       Func<Danmaku, bool> filter = null)
            {
            if (target == null)
                throw new ArgumentNullException("target");
            return danmakus.MoveTowards(target.transform.position,
                                        maxDistanceDelta);
        }

        public static IEnumerable<Danmaku> MoveTowards(this IEnumerable<Danmaku> danmakus,
                                       GameObject target,
                                       float maxDistanceDelta,
                                       Func<Danmaku, bool> filter = null)
            {
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
        public static IEnumerable<Danmaku> Translate(this IEnumerable<Danmaku> danmakus, Vector2 deltaPos, Func<Danmaku, bool> filter = null)
            {
            if (deltaPos != Vector2.zero)
                return danmakus.ForEach(x => x.Position += deltaPos, filter);
            return danmakus;
        }

        #endregion

        #region Rotation Functions

        public static IEnumerable<Danmaku> RotateTo(this IEnumerable<Danmaku> danmakus, float rotation, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Rotation = rotation, filter);
        }

        public static IEnumerable<Danmaku> RotateTo(this IEnumerable<Danmaku> danmakus,
                                    ICollection<float> rotations,
                                    Func<Danmaku, bool> filter = null)
            {
            if (rotations == null)
                throw new ArgumentNullException("rotations");
            return danmakus.ForEach(x => x.Rotation = rotations.Random(), filter);
        }

        public static IEnumerable<Danmaku> Rotate(this IEnumerable<Danmaku> danmakus, float delta, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Rotation += delta, filter);
        }

        #endregion

        #region Speed Functions

        public static IEnumerable<Danmaku> Speed(this IEnumerable<Danmaku> danmakus, float speed, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Speed = speed, filter);
        }

        public static IEnumerable<Danmaku> Speed(this IEnumerable<Danmaku> danmakus, ICollection<float> speeds, Func<Danmaku, bool> filter = null)
            {
            if (speeds == null)
                throw new ArgumentNullException("speeds");
            return danmakus.ForEach(x => x.Speed = speeds.Random(), filter);
        }

        public static IEnumerable<Danmaku> Accelerate(this IEnumerable<Danmaku> danmakus, float deltaSpeed, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Speed += deltaSpeed, filter);
        }

        #endregion

        #region Angular Speed Functions

        public static IEnumerable<Danmaku> AngularSpeed(this IEnumerable<Danmaku> danmakus,
                                        float angularSpeed,
                                        Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.AngularSpeed = angularSpeed, filter);
        }

        public static IEnumerable<Danmaku> AngularSpeed(this IEnumerable<Danmaku> danmakus,
                                        ICollection<float> angularSpeeds,
                                        Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.AngularSpeed = angularSpeeds.Random(), filter);
        }

        public static IEnumerable<Danmaku> AngularAccelerate(this IEnumerable<Danmaku> danmakus,
                                             float deltaSpeed,
                                             Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.AngularSpeed += deltaSpeed, filter);
        }

        #endregion

        #region Damage Functions

        public static IEnumerable<Danmaku> Damage(this IEnumerable<Danmaku> danmakus, int damage, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Damage = damage, filter);
        }

        public static IEnumerable<Danmaku> Damage(this IEnumerable<Danmaku> danmakus, ICollection<int> damages, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.AngularSpeed = damages.Random(), filter);
        }

        #endregion

        #region Color Functions

        public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, Color color, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Color = color, filter);
        }

        public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, ICollection<Color> colors, Func<Danmaku, bool> filter = null)
            {
            if (colors == null)
                throw new ArgumentNullException("colors");
            return danmakus.ForEach(x => x.Color = colors.Random(), filter);
        }

        public static IEnumerable<Danmaku> Color(this IEnumerable<Danmaku> danmakus, Gradient colors, Func<Danmaku, bool> filter = null)
            {
            if (colors == null)
                throw new ArgumentNullException("colors");
            return danmakus.ForEach(x => x.Color = colors.Random(), filter);
        }

        #endregion

        #region Controller Functions

        public static IEnumerable<Danmaku> AddController(this IEnumerable<Danmaku> danmakus,
                                         DanmakuController controller,
                                         Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Controller += controller, filter);
        }

        public static IEnumerable<Danmaku> RemoveController(this IEnumerable<Danmaku> danmakus,
                                            DanmakuController controller,
                                            Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Controller -= controller, filter);
        }

        public static IEnumerable<Danmaku> ClearControllers(this IEnumerable<Danmaku> danmakus, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.ClearControllers(), filter);
        }

        #endregion

        #region General Functions

        public static IEnumerable<Danmaku> Active(this IEnumerable<Danmaku> danmakus, bool value, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.IsActive = value, filter);
        }

        public static IEnumerable<Danmaku> Activate(this IEnumerable<Danmaku> danmakus, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Activate(), filter);
        }

        public static IEnumerable<Danmaku> Deactivate(this IEnumerable<Danmaku> danmakus, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.Deactivate(), filter);
        }

        public static IEnumerable<Danmaku> DeactivateImmediate(this IEnumerable<Danmaku> danmakus, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.DeactivateImmediate(), filter);
        }

        #endregion

        #region Misc Functions

        public static IEnumerable<Danmaku> CollisionCheck(this IEnumerable<Danmaku> danmakus, bool collisionCheck, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.CollisionCheck = collisionCheck, filter);
        }

        public static IEnumerable<Danmaku> MatchPrefab(this IEnumerable<Danmaku> danmakus, DanmakuPrefab prefab, Func<Danmaku, bool> filter = null)
            {
            return danmakus.ForEach(x => x.MatchPrefab(prefab), filter);
        }

        #endregion

        #region Fire Functions 

        public static IEnumerable<Danmaku> Fire(this IEnumerable<Danmaku> danmakus,
                                FireData data,
                                bool useRotation = true,
                                Func<Danmaku, bool> filter = null)
            {
            Vector2 tempPos = data.Position;
            float tempRot = data.Rotation;
            danmakus.ForEach(delegate(Danmaku danmaku) {
                                 data.Position = danmaku.Position;
                                 if (useRotation)
                                     data.Rotation = danmaku.Rotation;
                                 data.Fire();
                             },
                             filter);
            data.Position = tempPos;
            data.Rotation = tempRot;
            return danmakus;
        }

        #endregion
    }

}