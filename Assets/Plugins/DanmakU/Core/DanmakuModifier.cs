// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    [System.Serializable]
    public abstract class DanmakuModifier : IEnumerable<DanmakuModifier> {

        private FireData _data;

        [SerializeField]
        private DanmakuModifier _subModifier;

        protected DynamicFloat Speed {
            get { return _data.Speed; }
            set {
                _data.Speed = value;
                if (_subModifier != null)
                    _subModifier.Speed = value;
            }
        }

        protected DynamicFloat AngularSpeed {
            get { return _data.AngularSpeed; }
            set {
                _data.AngularSpeed = value;
                if (_subModifier != null)
                    _subModifier.AngularSpeed = value;
            }
        }

        protected DanmakuField Field {
            get { return _data.Field; }
            set {
                _data.Field = value;
                if (_subModifier != null)
                    _subModifier.Field = value;
            }
        }

        protected DanmakuController Controller {
            get { return _data.Controller; }
            set {
                _data.Controller = value;
                if (_subModifier != null)
                    _subModifier.Controller = value;
            }
        }

        protected DanmakuPrefab Prefab {
            get { return _data.Prefab; }
            set {
                _data.Prefab = value;
                if (_subModifier != null)
                    _subModifier.Prefab = value;
            }
        }

        protected DanmakuGroup Group {
            get { return _data.Group; }
            set {
                _data.Group = value;
                if (_subModifier != null)
                    _subModifier.Group = value;
            }
        }

        public DanmakuModifier SubModifier {
            get { return _subModifier; }
            set {
                _subModifier = value;
                if (_subModifier != null)
                    _subModifier.Initialize(_data);
            }
        }

        #region IEnumerable implementation

        public IEnumerator<DanmakuModifier> GetEnumerator() {
            DanmakuModifier current = this;
            while (current != null) {
                yield return current;
                current = current._subModifier;
            }
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.
            GetEnumerator() {
            DanmakuModifier current = this;
            while (current != null) {
                yield return current;
                current = current._subModifier;
            }
        }

        #endregion

        internal void Initialize(FireData data) {
            _data = data;
            if (_subModifier != null)
                _subModifier.Initialize(data);
            OnInitialize();
        }

        protected virtual void OnInitialize() {}

        public static DanmakuModifier Construct(
            IEnumerable<DanmakuModifier> enumerable) {
            if (enumerable == null)
                throw new System.ArgumentNullException();
            if (enumerable is DanmakuModifier)
                return enumerable as DanmakuModifier;
            DanmakuModifier top = null;
            DanmakuModifier current = null;
            foreach (var next in enumerable) {
                if (next != null) {
                    if (top == null)
                        top = next;
                    else
                        current._subModifier = next;
                    current = next;
                }
            }
            return top;
        }

        public void Insert(DanmakuModifier newModifier) {
            if (newModifier == null)
                throw new System.ArgumentNullException();
            if (_subModifier == null)
                _subModifier = newModifier;
            else {
                newModifier._subModifier = _subModifier;
                _subModifier = newModifier;
            }
        }

        public void Append(DanmakuModifier newModifier) {
            DanmakuModifier parent = this;
            DanmakuModifier current = _subModifier;
            while (current != null)
                current = current._subModifier;
            parent.SubModifier = newModifier;
        }

        protected void FireSingle(Vector2 position,
                                  DynamicFloat rotation) {
            if (SubModifier == null) {
                _data.Position = position;
                _data.Rotation = rotation;
                _data.Fire();
            } else
                SubModifier.OnFire(position, rotation);
        }

        public void Fire(FireData data) {
            Initialize(data);
            OnFire(data.Position, data.Rotation);
        }

        public abstract void OnFire(Vector2 position, DynamicFloat rotation);

    }

}