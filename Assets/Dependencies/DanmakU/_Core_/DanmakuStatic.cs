// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>

namespace Hourai.DanmakU {

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

        private static int[] collisionMask;

        /// <summary>
        /// A cached delta time value for procesing bullet updates.
        /// Static member accesses are slightly faster than member accesses or passing via parameters.
        /// Since it is a static value within each frame, it is best to cache it as a static variable.
        /// </summary>
        public static float dt;

        /// <summary>
        /// A map that matches colliders to respective collision handler scripts.
        /// Used to cache the results so that not every bullet collision triggers a GetComponents call.
        /// Cleared every frame: do not put permanent data in here.
        /// </summary>
        private static Dictionary<Collider2D, IDanmakuCollider[]> colliderMap;

        static Danmaku() {
            Setup();
            Game.OnUpdate += GlobalUpdate;

            // Destroy all Danmaku when the 
            Game.OnLoad += level => DestroyAll();
        }

        static void GlobalUpdate()
        {
            dt = TimeUtil.DeltaTime;
            if (colliderMap.Count > 0)
                colliderMap.Clear();
        }

        internal static void Setup(float angRes = 0.1f) {
            colliderMap = new Dictionary<Collider2D, IDanmakuCollider[]>();
            collisionMask = Util.CollisionLayers2D();
        }

        public static void DestroyAll() {
            throw new NotImplementedException(); // TODO: Reimplement
        }

        public static void DestroyInCircle(Vector2 center,
                                              float radius,
                                              int layerMask = ~0) {
            throw new NotImplementedException(); // TODO: Reimplement
            //Danmaku current;
            //float sqrRadius = radius*radius;
            //for (int i = 0; i < _activeCount; i++) {
            //    current = all[i];
            //    if ((layerMask & (1 << current.layer)) != 0 &&
            //        sqrRadius >= (current.Position - center).sqrMagnitude)
            //        current.Destroy();
            //}
        }

        public static Danmaku FindByTag(string tag)
        {
            throw new NotImplementedException(); // TODO: Reimplement
            //if (tag == null)
            //    throw new ArgumentNullException("tag");

            //for (int i = 0; i < _activeCount; i++)
            //    if (all[i].Tag == tag)
            //        return all[i];

            //return null;
        }

        public static Danmaku[] FindAllByTag(string tag) {
            if (tag == null)
                throw new ArgumentNullException("tag");
            throw new NotImplementedException(); // TODO: Reimplement
            //List<Danmaku> matches = new List<Danmaku>();
            //for (int i = 0; i < _activeCount; i++)
            //    if(all[i].Tag == tag)
            //        matches.Add(all[i]);

            //return matches.ToArray();
        }

        public static int FindAllByTagNoAlloc(string tag,
                                              IList<Danmaku> list,
                                              int start = 0) {
            
            throw new NotImplementedException(); // TODO: Reimplement
            //if (tag == null)
            //    throw new ArgumentNullException("tag");
            //if (list == null)
            //    throw new ArgumentNullException("collection");

            //Danmaku current;
            //int index = start, count = -1, size = list.Count;
            //for (var i = 0; i < _activeCount; i++)
            //{
            //    current = all[i];
            //    if (current.Tag != tag)
            //        continue;
            //    index++;
            //    count++;
            //    list[index] = current;
            //    if (index >= size)
            //        break;
            //}
            //return count;
        }

        public static int AddAllByTag(string tag,
                                      ICollection<Danmaku> collection)
        {
            throw new NotImplementedException(); // TODO: Reimplement
            //if (tag == null)
            //    throw new ArgumentNullException("tag");
            //if (collection == null)
            //    throw new ArgumentNullException("collection");

            //int count = -1;
            //for (int i = 0; i < _activeCount; i++)
            //{
            //    if (all[i].Tag == tag)
            //        collection.Add(all[i]);
            //}
            //return count;
        }

        public Danmaku SpawnDanmaku(DanmakuPrefab prefab,
                                    Vector2 location,
                                    float rotation)
        {
            if (prefab == null)
                throw new ArgumentNullException("prefab");
            Danmaku danmaku = prefab.Get();
            danmaku.Position = position;
            danmaku.Rotation = rotation;
            return danmaku;
        }

        public Danmaku FireLinear(DanmakuPrefab prefab,
                                  Vector2 location,
                                  float rotation,
                                  float speed)
        {
            if (prefab == null)
                throw new ArgumentNullException("prefab");
            Danmaku danmaku = prefab.Get();
            danmaku.Position = position;
            danmaku.Rotation = rotation;
            danmaku.Speed = speed;
            danmaku.AngularSpeed = 0f;
            return danmaku;
        }

        public Danmaku FireCurved(DanmakuPrefab prefab,
                                  Vector2 location,
                                  float rotation,
                                  float speed,
                                  float angularSpeed)
        {
            if (prefab == null)
                throw new ArgumentNullException("prefab");
            Danmaku danmaku = prefab.Get();
            danmaku.Position = position;
            danmaku.Rotation = rotation;
            danmaku.Speed = speed;
            danmaku.AngularSpeed = angularSpeed;
            return danmaku;
        }

        public static implicit operator bool(Danmaku danmaku) {
            return danmaku != null && danmaku.IsActive;
        }

    }

}