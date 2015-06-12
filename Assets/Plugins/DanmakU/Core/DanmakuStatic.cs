// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// A single projectile fired.
    /// The base object that represents a single bullet in a Danmaku game
    /// </summary>
    public sealed partial class Danmaku {

        /// <summary>
        /// The standard number of bullets pre-spawned in if no DanmakuGameController is present
        /// </summary>
        internal const int standardStart = 1000;

        /// <summary>
        /// The standard number of Danmaku objects that is spawned used if a DanmakuGameController is not present
        /// </summary>
        internal const int standardSpawn = 100;

        /// <summary>
        /// A set of 2D vectors corresponding to unit circle coordinates
        /// Precalculated since Cosine and Sine calculations are expensive when called thousands of times per frame.
        /// An array access on an array of structs is much cheaper than calling both Mathf.Cos, and Mathf.Sin.
        /// </summary>
        internal static Vector2[] unitCircle;

        private static float invAngRes;
        private static int unitCircleMax;
        private static int[] collisionMask;
        private static DanmakuPool danmakuPool;

        /// <summary>
        /// A cached delta time value for procesing bullet updates.
        /// Static member accesses are slightly faster than member accesses or passing via parameters.
        /// Since it is a static value within each frame, it is best to cache it as a static variable.
        /// </summary>
        private static float dt;

        /// <summary>
        /// A map that matches colliders to respective collision handler scripts.
        /// Used to cache the results so that not every bullet collision triggers a GetComponents call.
        /// Cleared every frame: do not put permanent data in here.
        /// </summary>
        private static Dictionary<Collider2D, IDanmakuCollider[]> colliderMap;

        static Danmaku() {
            Setup();
        }

        public static int TotalCount {
            get { return (danmakuPool != null) ? danmakuPool.totalCount : 0; }
        }

        public static int ActiveCount {
            get { return (danmakuPool != null) ? danmakuPool.totalCount : 0; }
        }

        internal static void Setup(int initial = standardStart,
                                   int spawn = standardSpawn,
                                   float angRes = 0.1f) {
            colliderMap = new Dictionary<Collider2D, IDanmakuCollider[]>();
            collisionMask = Util.CollisionLayers2D();
            danmakuPool = new DanmakuPool(initial, spawn);
            invAngRes = 1f/angRes;
            unitCircleMax = Mathf.CeilToInt(360f/angRes);
            float angle = 90f;
            unitCircle = new Vector2[unitCircleMax];
            for (int i = 0; i < unitCircleMax; i++) {
                angle += angRes;
                unitCircle[i] = Util.OnUnitCircle(angle);
            }
        }

        internal static int Ang2Index(float angle) {
            float clamp = angle - 360*Mathf.FloorToInt(angle/360);
            int index = (int) (clamp*invAngRes);
            if (index >= unitCircleMax)
                index = unitCircleMax - 1;
            if (index < 0)
                index = 0;
            return index;
        }

        internal static Vector2 UnitCircle(float angle) {
            return unitCircle[Ang2Index(angle)];
        }

        internal static float Cos(float angle) {
            return unitCircle[Ang2Index(angle)].x;
        }

        internal static float Sin(float angle) {
            return unitCircle[Ang2Index(angle)].y;
        }

        internal static float Tan(float angle) {
            float sin = Sin(angle);
            if (sin == 0f)
                return float.NaN;
            return Cos(angle)/Sin(angle);
        }

        /// <summary>
        /// Does a frame update on all currently active bullets.
        /// This should be called only once per frame. 
        /// </summary>
        internal static void UpdateAll() {
            colliderMap.Clear();
            //caches the change in time since the last frame
            Danmaku danmaku;
            dt = TimeUtil.DeltaTime;
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                danmaku = all[i];
                if (danmaku != null && danmaku._isActive) {
                    danmaku.Update();   
                }
            }
        }

        /// <summary>
        /// Deactivates all currently active bullets.
        /// </summary>
        public static void DeactivateAll() {
            Danmaku danmaku;
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                danmaku = all[i];
                if (danmaku != null && danmaku._isActive)
                    danmaku.Deactivate();
            }
        }

        /// <summary>
        /// Deactivates all currently active bullets.
        /// </summary>
        public static void DeactivateAllImmediate() {
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                if (all[i] != null && all[i]._isActive)
                    all[i].DeactivateImmediate();
            }
        }

		/// <summary>
		/// Deactivates all danmaku in 
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="layerMask">Layer mask.</param>
        public static void DeactivateInCircle(Vector2 center,
                                              float radius,
                                              int layerMask = ~0) {
            Danmaku[] all = danmakuPool.all;
            Danmaku current;
            float sqrRadius = radius*radius;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if ((layerMask & (1 << current.layer)) != 0 &&
                    sqrRadius >= (current.Position - center).sqrMagnitude)
                    current.Deactivate();
            }
        }

        public static void DeactivateInCircleImmediate(Vector2 center,
                                                       float radius,
                                                       int layerMask = ~0) {
            Danmaku[] all = danmakuPool.all;
            Danmaku current;
            float sqrRadius = radius*radius;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if ((layerMask & (1 << current.layer)) != 0 &&
                    sqrRadius >= (current.Position - center).sqrMagnitude)
                    current.DeactivateImmediate();
            }
        }

        public static Danmaku FindByTag(string tag) {
            Danmaku current;
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if (current._isActive && current.Tag == tag)
                    return current;
            }
            return null;
        }

        public static Danmaku[] FindAllByTag(string tag) {
            Danmaku current;
            List<Danmaku> matches = new List<Danmaku>();
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if (current._isActive && current.Tag == tag)
                    matches.Add(current);
            }
            return matches.ToArray();
        }

        public static int FindAllByTagNoAlloc(string tag,
                                              IList<Danmaku> danmaku,
                                              int start = 0) {
            if (tag == null)
                throw new System.ArgumentNullException("Tag cannot be null!");
            if (danmaku == null) {
                throw new System.ArgumentNullException(
                    "Danmaku Array cannot be null!");
            }
            Danmaku current;
            int index = start, count = -1, size = danmaku.Count;
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if (current._isActive && current.Tag == tag) {
                    index++;
                    count++;
                    danmaku[index] = current;
                    if (index >= size)
                        break;
                }
            }
            return count;
        }

        public static int AddAllByTag(string tag,
                                      ICollection<Danmaku> collection) {
            if (tag == null)
                throw new System.ArgumentNullException("Tag cannot be null!");
            if (collection == null) {
                throw new System.ArgumentNullException(
                    "Danmaku container cannot be null!");
            }
            int count = -1;
            Danmaku current;
            Danmaku[] all = danmakuPool.all;
            for (int i = 0; i < all.Length; i++) {
                current = all[i];
                if (current._isActive && current.Tag == tag)
                    collection.Add(current);
            }
            return count;
        }

        /// <summary>
        /// Retrieves an inactive Danmaku object.
        /// </summary>
        /// <remarks>
        /// The returned Danmaku object does not have any of its properties altered, and its state is likely to be completely randomized.
        /// The returned Danmaku object also is not active and must be activated before it actually appears or does anything.
        /// This method does not allocate any memory.
        /// </remarks>
        /// <returns>The inactive Danmaku.</returns>
        public static Danmaku GetInactive() {
            return danmakuPool.Get();
        }

        /// <summary>
        /// Retrieves a number of inactive Danmaku objects.
        /// </summary>
        /// <remarks>
        /// The returned Danmaku objects does not have any of its properties altered, and their state is likely to be completely randomized.
        /// The returned Danmaku objects also is not active and must be activated before it actually appears or does anything.
        /// This method allocates memory for array.
        /// </remarks>
        /// <returns>The inactive Danmaku objects in an array.</returns>
        /// <param name="count">the number of inactive Danmaku objects to retrieve.</param>
        public static Danmaku[] GetInactive(DynamicInt count) {
            Danmaku[] array = new Danmaku[count];
            danmakuPool.Get(array);
            return array;
        }

        /// <summary>
        /// Retrieves a number of inactive Danmaku objects.
        /// </summary>
        /// <remarks>
        /// This method fills an existing array of Danmaku with inactive instances.
        /// The returned Danmaku objects does not have any of its properties altered, and their state is likely to be completely randomized.
        /// The returned Danmaku objects also is not active and must be activated before it actually appears or does anything.
        /// This method does not allocate any memory.
        /// </remarks>
        /// <exception cref="ArgumentNullException">thrown if the preallocated array is null.</exception>
        /// <param name="prealloc">the preallocated array to fill.</param>
        public static void GetInactive(Danmaku[] prealloc) {
            if (prealloc == null)
                throw new System.ArgumentNullException();
            danmakuPool.Get(prealloc);
        }

        /// <summary>
        /// Retreives an inactive Danmaku object, and places it 
        /// </summary>
        /// <returns>The inactive.</returns>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static Danmaku GetInactive(Vector2 position,
                                          DynamicFloat rotation) {
            Danmaku danmaku = danmakuPool.Get();
            danmaku.position.x = position.x;
            danmaku.position.y = position.y;
            danmaku.Rotation = rotation.Value;
            return danmaku;
        }

        public static Danmaku[] GetInactive(Vector2 position,
                                            DynamicFloat rotation,
                                            DynamicInt count) {
            var array = new Danmaku[count];
            GetInactive(position, rotation, array);
            return array;
        }

        public static void GetInactive(Vector2 position,
                                       DynamicFloat rotation,
                                       Danmaku[] prealloc) {
			if(prealloc == null)
				throw new System.ArgumentNullException("prealloc");

            danmakuPool.Get(prealloc);
            prealloc.MoveTo(position).RotateTo(rotation);
        }

        public static Danmaku GetInactive(DanmakuPrefab danmakuType,
                                          Vector2 position,
                                          DynamicFloat rotation) {
            Danmaku danmaku = danmakuPool.Get();
            danmaku.MatchPrefab(danmakuType);
            danmaku.position.x = position.x;
            danmaku.position.y = position.y;
            danmaku.Rotation = rotation;
            return danmaku;
        }

        public static Danmaku[] GetInactive(DanmakuPrefab danmakuType,
                                            Vector2 position,
                                            DynamicFloat rotation,
                                            DynamicInt count) {
            Danmaku[] array = new Danmaku[count];
            danmakuPool.Get(array);
            for (int i = 0; i < array.Length; i++) {
                Danmaku danmaku = array[i];
                danmaku.MatchPrefab(danmakuType);
                danmaku.position.x = position.x;
                danmaku.position.y = position.y;
                danmaku.Rotation = rotation.Value;
            }
            return array;
        }

        public static void GetInactive(DanmakuPrefab danmakuType,
                                       Vector2 position,
                                       DynamicFloat rotation,
                                       Danmaku[] prealloc) {
            danmakuPool.Get(prealloc);
            for (int i = 0; i < prealloc.Length; i++) {
                Danmaku danmaku = prealloc[i];
                danmaku.MatchPrefab(danmakuType);
                danmaku.position.x = position.x;
                danmaku.position.y = position.y;
                danmaku.Rotation = rotation.Value;
            }
        }

        private void Match(FireData data) {
            MatchPrefab(data.Prefab);
            Position = data.Position;
            Rotation = data.Rotation;
            Speed = data.Speed;
            AngularSpeed = data.AngularSpeed;
            AddController(data.Controller);
            Damage = data.Damage.Value;
            if (data.Group != null)
                data.Group.Add(this);
        }

        public static Danmaku GetInactive(FireData data) {
            if (data == null)
                throw new System.ArgumentNullException();
            Danmaku danmaku = danmakuPool.Get();
            danmaku.Match(data);
            return danmaku;
        }

        public static Danmaku[] GetInactive(FireData data, DynamicInt count) {
            var danmakus = new Danmaku[count];
            GetInactive(data, danmakus);
            return danmakus;
        }

        public static void GetInactive(FireData data, Danmaku[] prealloc) {
            if (data == null)
                throw new System.ArgumentNullException("data");
			if(prealloc == null)
				throw new System.ArgumentNullException("prealloc");

            danmakuPool.Get(prealloc);
            for (int i = 0; i < prealloc.Length; i++)
                prealloc[i].Match(data);
        }

        public static FireBuilder ConstructFire(DanmakuPrefab prefab) {
            var builder = new FireBuilder(prefab);
            return builder;
        }

    }

}