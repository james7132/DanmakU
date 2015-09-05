// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vexe.Runtime.Extensions;

namespace Hourai.DanmakU {

    public sealed class DanmakuGroup : ICollection<Danmaku>, IFireBindable, IComparable<ICollection<Danmaku>>{

        private ICollection<Danmaku> _group;

        public ICollection<Danmaku> Group
        {
            get { return _group; }
        }

        public event Action<Danmaku> OnAdd;
        public event Action<Danmaku> OnRemove;

        public static DanmakuGroup Set(IEnumerable<Danmaku> source = null) {
            HashSet<Danmaku> set;
            if (source == null)
                set = new HashSet<Danmaku>();
            else
                set = new HashSet<Danmaku>(source);
            return new DanmakuGroup(set);
        }

        public static DanmakuGroup List(IEnumerable<Danmaku> source = null) {
            List<Danmaku> list;
            if (source == null)
                list = new List<Danmaku>();
            else
                list = new List<Danmaku>(source);
            return new DanmakuGroup(list);
        } 

        public DanmakuGroup(ICollection<Danmaku> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            _group = collection;
            if (_group.Count <= 0) 
                return;
            foreach (Danmaku danmaku in _group)
                if (danmaku != null)
                    danmaku.OnDestroy += RemoveEvent;
        }

        public void AddRange(IEnumerable<Danmaku> collection) {
            if (collection == null)
                throw new ArgumentNullException();

            if(OnAdd == null)
                foreach (var danmaku in collection) {
                    if (danmaku != null)
                        danmaku.OnDestroy += RemoveEvent;
                    _group.Add(danmaku);
                }
            else
                foreach (Danmaku danmaku in collection) {
                    if (danmaku != null) {
                        danmaku.OnDestroy += RemoveEvent;
                        OnAdd(danmaku);
                    }
                    _group.Add(danmaku);
                }
        }

        public int RemoveRange(IEnumerable<Danmaku> collection) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            int oldCount = _group.Count;
            if (OnRemove == null)
                RemoveAll(collection);
            else
                foreach (Danmaku danmaku in collection)
                    if (_group.Remove(danmaku) && danmaku != null)
                        OnRemove(danmaku);
            return oldCount - _group.Count;
        }

        void RemoveAll(IEnumerable<Danmaku> collection) {
            var set = _group as HashSet<Danmaku>;
            if (set != null)
                set.ExceptWith(collection);
            else
                foreach (Danmaku danmaku in collection)
                    _group.Remove(danmaku);
        }

        public void RemoveAll(Func<Danmaku, bool> match) {
            RemoveRange(_group.Where(match));
        }

        public Danmaku[] ToArray() {
            var array = new Danmaku[Count];
            CopyTo(array, 0);
            return array;
        }

        public override int GetHashCode() {
            return _group.GetHashCode();
        }

        public override bool Equals(object obj) {
            return _group.Equals(obj);
        }

        #region ICollection implementation

        public void Add(Danmaku item) {
            _group.Add(item);
            if (item != null)
                item.OnDestroy += RemoveEvent;

        }

        public void Clear() {
            if (OnRemove != null)
                foreach (Danmaku danmaku in _group)
                    OnRemove(danmaku);
            _group.Clear();
        }

        public bool Contains(Danmaku item) {
            return _group.Contains(item);
        }

        public void CopyTo(Danmaku[] array, int arrayIndex) {
            _group.CopyTo(array, arrayIndex);
        }

        public bool Remove(Danmaku item) {
            bool success = _group.Remove(item);
            if (success)
                OnRemove.SafeInvoke(item);
            return success;
        }

        void RemoveEvent(Danmaku item) {
            if (_group.Remove(item) && OnRemove != null)
                OnRemove(item);
        }

        public int Count {
            get { return _group.Count; }
        }

        public bool IsReadOnly {
            get { return _group.IsReadOnly; }
        }

        #endregion

        public void Bind(FireData fireData)
        {
            if(fireData == null)
                throw new ArgumentNullException("fireData");
            fireData.OnActivate += Add;
        }

        public void Unbind(FireData fireData)
        {
            if (fireData == null)
                throw new ArgumentNullException("fireData");
            fireData.OnActivate -= Add;
        }

        public int CompareTo(ICollection<Danmaku> other) {
            return _group.Count.CompareTo(other.Count);
        }

        public IEnumerator<Danmaku> GetEnumerator() {
            return _group.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _group.GetEnumerator();
        }
    }
}