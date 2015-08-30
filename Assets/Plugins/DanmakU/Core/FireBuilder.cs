// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// A container class for passing information about firing a single bullet.
    /// For creating more complex firing. See FireBuilder.
    /// </summary>
    [Serializable]
    public class FireData : IEnumerable<FireData> {

        public DFloat AngularSpeed;
        public DanmakuController Controller;
        public Action<Danmaku> OnActivate;
        public Action<Danmaku> OnDeactivate;
        public DInt Damage;
        public DanmakuField Field;
        public Vector2 Position;
        public DanmakuPrefab Prefab;
        public DFloat Rotation;
        public DFloat Speed = 5f;

        public void Copy(FireData other) {
            if (other == null || other == this)
                return;
            AngularSpeed = other.AngularSpeed;
            Controller = other.Controller;
            OnActivate = other.OnActivate;
            OnDeactivate = other.OnDeactivate;
            Damage = other.Damage;
            Field = other.Field;
            Position = other.Position;
            Prefab = other.Prefab;
            Rotation = other.Rotation;
            Speed = other.Speed;
        }

        public FireData Clone() {
            var data = new FireData();
            data.Copy(this);
            return data;
        }

        public Danmaku Fire(Vector2? position = null, DFloat? rotation = null) {
            Danmaku danmaku = Danmaku.GetInactive(this);
            danmaku.Position = position ?? Position;
            danmaku.Rotation = rotation ?? Rotation;
            danmaku.Field = Field;
            danmaku.Activate();
            return danmaku;
        }

        public void AddGroup(DanmakuGroup group) {
            if(group == null)
                throw new ArgumentNullException("group");
            OnActivate += group.Add;
        }

        public void RemoveGroup(DanmakuGroup group)
        {
            if (group == null)
                throw new ArgumentNullException("group");
            OnActivate -= group.Add;
        }

        public IEnumerator<FireData> GetEnumerator() {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable Multiple(DInt amount, Action<FireData> delta = null) {
            int count = amount.Value;
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