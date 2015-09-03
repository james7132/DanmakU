// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hourai.DanmakU {

    /// <summary>
    /// A container class for passing information about firing a single bullet.
    /// For creating more complex firing. See FireBuilder.
    /// </summary>
    [Serializable]
    public class FireData : IEnumerable<FireData> {

        public Vector2 Position;
        public float Rotation;
        public Color? Color;
        public float Damage;
        public float Speed = 5f;
        public float AngularSpeed;
        public Action<Danmaku> Controller;
        public Action<Danmaku> OnActivate;
        public Action<Danmaku> OnDeactivate;
        public DanmakuPrefab Prefab;

        public void Copy(FireData other) {
            if (other == null || other == this)
                return;
            AngularSpeed = other.AngularSpeed;
            Controller = other.Controller;
            OnActivate = other.OnActivate;
            OnDeactivate = other.OnDeactivate;
            Damage = other.Damage;
            Position = other.Position;
            Prefab = other.Prefab;
            Rotation = other.Rotation;
            Speed = other.Speed;
            Color = other.Color;
        }

        public FireData Clone() {
            var data = new FireData();
            data.Copy(this);
            return data;
        }

        public Danmaku Fire() {
            Danmaku danmaku = Danmaku.GetInactive(this);
            danmaku.Activate();
            return danmaku;
        }

        public IEnumerator<FireData> GetEnumerator() {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable Multiple(int count, Action<FireData> delta = null) {
            if (delta != null) {
                FireData clone = Clone();
                for (var i = 0; i < count; i++) {
                    delta(clone);
                    yield return clone;
                }
            } else {
                for (var i = 0; i < count; i++)
                    yield return this;
            }
        }

        public IEnumerable Infinite(Action<FireData> delta = null)
        {
            if (delta != null)
            {
                FireData clone = Clone();
                while(true) {
                    delta(clone);
                    yield return clone;
                }
            }
            while (true)
                yield return this;
        }

    }

}