// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// A container class for passing information about firing a single bullet.
    /// For creating more complex firing. See FireBuilder.
    /// </summary>
    [Serializable]
    public sealed class FireData {

        public DynamicFloat AngularSpeed = 0f;
        public DanmakuController Controller;
        public DynamicInt Damage = 0;
        public DanmakuField Field;
        public DanmakuGroup Group;
        public Vector2 Position = Vector2.zero;
        public DanmakuPrefab Prefab;
        public DynamicFloat Rotation = 0f;
        public DynamicFloat Speed = 5f;

        public Danmaku Fire() {
            Danmaku danmaku = Danmaku.GetInactive(this);
            danmaku.Field = Field;
            danmaku.Activate();
            return danmaku;
        }

        public void Copy(FireData other) {
            Prefab = other.Prefab;
            Field = other.Field;
            Position = other.Position;
            Rotation = other.Rotation;
            Speed = other.Speed;
            AngularSpeed = other.AngularSpeed;
            Controller = other.Controller;
            Damage = other.Damage;
            Group = other.Group;
        }

        public FireData Clone() {
            FireData copy = new FireData();
            copy.Copy(this);
            return copy;
        }

    }

    public sealed class FireBuilder {

        private readonly FireData _data;
        private readonly List<DanmakuModifier> _modifiers;
        private Transform _positionSource;
        private Transform _rotationSource;
        private bool _targeted;
        private Transform _targetObject;
        private Vector2? _targetPosition;

        internal FireBuilder(FireBuilder other) {
            _data = new FireData();
            _modifiers = new List<DanmakuModifier>();
            Copy(other);
        }

        internal FireBuilder(DanmakuPrefab prefab, DanmakuField field = null) {
            if (prefab == null)
                throw new ArgumentNullException("prefab");

            _data = new FireData {
                Prefab = prefab,
                Field = field
            };
            _modifiers = new List<DanmakuModifier>();
        }

        public FireBuilder Copy(FireBuilder other) {
            _data.Copy(other._data);
            _modifiers.Clear();
            _modifiers.AddRange(other._modifiers);
            _positionSource = other._positionSource;
            _rotationSource = other._rotationSource;
            _targeted = other._targeted;
            _targetPosition = other._targetPosition;
            _targetObject = other._targetObject;
            return this;
        }

        public FireBuilder Clone() {
            return new FireBuilder(this);
        }

        #region Field Functions

        public FireBuilder InField(DanmakuField field) {
            if (field == null)
                throw new ArgumentNullException("Field cannot be null!");

            Field = field;
            return this;
        }

        #endregion

        public void Fire() {
            Vector2 actualPosition = Position;
            DynamicFloat actualRotation = Rotation;

            if (_positionSource != null)
                actualPosition = _positionSource.position;

            if (_rotationSource != null)
                actualRotation = _rotationSource.Rotation2D();

            if (_targeted) {
                if (_targetPosition != null) {
                    actualRotation += DanmakuUtil.AngleBetween2D(
                                                                 actualPosition,
                                                                 (Vector2) _targetPosition);
                } else if (_targetObject != null) {
                    actualRotation += DanmakuUtil.AngleBetween2D(
                                                                 actualPosition,
                                                                 _targetObject.position);
                }
            }

            Vector2 tempPos = Position;
            DynamicFloat tempRotation = Rotation;
            Position = actualPosition;
            Rotation = actualRotation;

            if (_modifiers.Count <= 0)
                _data.Fire();
            else if (_modifiers.Count == 1) {
                DanmakuModifier singleModifier = _modifiers[0];
                if (singleModifier == null)
                    _data.Fire();
                else
                    singleModifier.Fire(_data);
            } else {
                DanmakuModifier[] oldSubModifiers =
                    new DanmakuModifier[_modifiers.Count];
                DanmakuModifier previous = null, current, initial = null;
                for (int i = 0; i < oldSubModifiers.Length; i++) {
                    current = _modifiers[i];
                    if (current != null) {
                        oldSubModifiers[i] = current.SubModifier;
                        if (previous != null)
                            previous.SubModifier = current;
                        else
                            initial = current;
                        previous = current;
                    }
                }

                if (initial != null)
                    initial.Fire(_data);
                else
                    _data.Fire();

                for (int i = 0; i < oldSubModifiers.Length; i++) {
                    current = _modifiers[i];
                    if (current != null)
                        current.SubModifier = oldSubModifiers[i];
                }
            }

            _data.Position = tempPos;
            _data.Rotation = tempRotation;
        }

        #region Public Properties

        public DanmakuPrefab Prefab {
            get { return _data.Prefab; }
            set { _data.Prefab = value; }
        }

        public DanmakuField Field {
            get { return _data.Field; }
            set { _data.Field = value; }
        }

        public Vector2 Position {
            get { return _data.Position; }
            set { _data.Position = value; }
        }

        public DynamicFloat Rotation {
            get { return _data.Rotation; }
            set { _data.Rotation = value; }
        }

        public event DanmakuController Controller {
            add { _data.Controller += value; }
            remove { _data.Controller -= value; }
        }

        public float Speed {
            get { return _data.Speed; }
            set { _data.Speed = value; }
        }

        public float AngularSpeed {
            get { return _data.AngularSpeed; }
            set { _data.AngularSpeed = value; }
        }

        public DynamicInt Damage {
            get { return _data.Damage; }
            set { _data.Damage = value; }
        }

        public DanmakuGroup Group {
            get { return _data.Group; }
            set { _data.Group = value; }
        }

        #endregion

        #region Position Functions

        /// <summary>
        /// Sets the firing origin position to a fixed point in space.
        /// </summary>
        ///
        /// <remarks>
        /// To move the origin of firing after calling this requries additional calls to this method.
        /// This will also 'un-link' the instance from any GameObjects.
        /// </remarks>
        /// <param name="position">the position in which </param>
        public FireBuilder From(Vector2 position) {
            Position = position;
            _positionSource = null;
            return this;
        }

        /// <summary>
        /// Sets the firing origin to the current position of a bullet.
        /// </summary>
        /// 
        /// <remarks>
        /// To move the origin of firing after calling this requries additional calls to this method.
        /// This will also 'un-link' the instance from any GameObjects.
        /// </remarks>
        /// <param name="danmaku">Danmaku.</param>
        public FireBuilder From(Danmaku danmaku) {
            Position = danmaku.Position;
            _positionSource = null;
            return this;
        }

        /// <summary>
        /// Links the firing origin position to automatically track a Transform and its GameObject.
        /// </summary>
        /// 
        /// <remarks>
        /// After calling this, all calls to <c>Fire()</c> will automatically be fired from the absolute world
        /// position of the specified. (i.e. as the GameObject moves over time. so does position the instance fires from).
        /// Only one call is necessary to link the object.
        /// 
        /// Note that if the GameObject is destroyed, the bullets fired with the instance will originate from 
        /// the previously specified absolute world position, which by default is (0,0).
        /// 
        /// If the given Transform is already null, this method does nothing.
        /// </remarks>
        /// <param name="transform">the specified origin Transform to use.</param>
        public FireBuilder From(Transform transform) {
            if (transform != null)
                _positionSource = transform;
            return this;
        }

        /// <summary>
        /// Links the firing origin position to automatically track a GameObject.
        /// </summary>
        /// 
        /// <remarks>
        /// After calling this, all calls to <c>Fire()</c> will automatically be fired from the absolute world
        /// position of the specified. (i.e. as the GameObject moves over time. so does position the instance fires from).
        /// Only one call is necessary to link the object.
        /// 
        /// Note that if the GameObject is destroyed, the bullets fired with the instance will originate from 
        /// the previously specified absolute world position, which by default is (0,0).
        /// 
        /// If the given GameObject is already null, this method does nothing.
        /// </remarks>
        /// <param name="gameObject">the specified origin GameObject to use.</param>
        public FireBuilder From(GameObject gameObject) {
            if (gameObject != null)
                _positionSource = gameObject.transform;
            return this;
        }

        /// <summary>
        /// Links the firing origin position to automatically track a Component and its GameObject.
        /// </summary>
        /// 
        /// <remarks>
        /// After calling this, all calls to <c>Fire()</c> will automatically be fired from the absolute world
        /// position of the specified. (i.e. as the GameObject moves over time. so does position the instance fires from).
        /// Only one call is necessary to link the object.
        /// 
        /// Note that if the GameObject is destroyed, the bullets fired with the instance will originate from 
        /// the previously specified absolute world position, which by default is (0,0).
        /// 
        /// If the given Transform is already null, this method does nothing.
        /// </remarks>
        /// <param name="component">the specified origin Component to use.</param>
        public FireBuilder From(Component component) {
            if (component == null)
                throw new ArgumentNullException();

            _positionSource = component.transform;
            return this;
        }

        #endregion

        #region Rotation Functions

        public FireBuilder Facing(Vector2 direction) {
            _targeted = false;
            _targetPosition = null;
            _targetObject = null;
            Rotation = Mathf.Rad2Deg*Mathf.Atan2(direction.y, direction.x);
            return this;
        }

        public FireBuilder Towards(Vector2 position) {
            _targeted = true;
            _targetPosition = position;
            return this;
        }

        public FireBuilder Towards(Transform transform) {
            if (transform == null) {
                _targeted = true;
                _targetPosition = null;
                _targetObject = transform;
            }
            return this;
        }

        public FireBuilder Towards(Component component) {
            if (component == null) {
                _targeted = true;
                _targetPosition = null;
                _targetObject = component.transform;
            }
            return this;
        }

        public FireBuilder Towards(GameObject gameObject) {
            if (gameObject == null) {
                _targeted = true;
                _targetPosition = null;
                _targetObject = gameObject.transform;
            }
            return this;
        }

        public FireBuilder WithRotation(DynamicFloat rotation) {
            Rotation = rotation;
            return this;
        }

        public FireBuilder WithRotation(float min, float max) {
            Rotation = new DynamicFloat(min, max);
            return this;
        }

        public FireBuilder WithRotation(Transform transform) {
            if (transform != null)
                _rotationSource = transform;
            return this;
        }

        public FireBuilder WithRotation(GameObject gameObject) {
            if (gameObject != null)
                _rotationSource = gameObject.transform;
            return this;
        }

        public FireBuilder WithRotation(Component component) {
            if (component != null)
                _rotationSource = component.transform;
            return this;
        }

        #endregion

        #region Speed Functions

        public FireBuilder WithSpeed(DynamicFloat speed) {
            Speed = speed;
            return this;
        }

        public FireBuilder WithSpeed(float min, float max) {
            Speed = new DynamicFloat(min, max);
            return this;
        }

        #endregion

        #region Angular Speed Functions

        public FireBuilder WithAngularSpeed(DynamicFloat angularSpeed) {
            AngularSpeed = angularSpeed;
            return this;
        }

        public FireBuilder WithAngularSpeed(float min, float max) {
            AngularSpeed = new DynamicFloat(min, max);
            return this;
        }

        #endregion

        #region Modifier Functions

        public FireBuilder WithModifier(DanmakuModifier modifier) {
            if (modifier != null)
                _modifiers.Add(modifier);
            return this;
        }

        public FireBuilder WithModifiers(IEnumerable<DanmakuModifier> modifiers) {
            if (modifiers != null)
                _modifiers.AddRange(modifiers);
            return this;
        }

        #endregion

        #region Controller Functions

        public FireBuilder WithController(IDanmakuController controller) {
            if (controller != null)
                Controller += controller.Update;
            return this;
        }

        public FireBuilder WithController(
            IEnumerable<IDanmakuController> controllers) {
            if (controllers != null)
                WithController(controllers.Compress());
            return this;
        }

        public FireBuilder WithController(DanmakuController controller) {
            Controller += controller;
            return this;
        }

        /// <summary>
        /// Adds a set of DanmakuControllers to be added to all bullets fired by this instance.
        /// Does nothing if the given collection of controllers is null.
        /// </summary>
        /// <returns>The FireBuilder that this method was called on. </returns>
        /// <param name="controllers">the set of DanmakuControllers to add.</param>
        public FireBuilder WithControllers(
            IEnumerable<DanmakuController> controllers) {
            if (controllers != null)
                WithController(controllers.Compress());
            return this;
        }

        /// <summary>
        /// Clears all of the non-prefab defined Danmaku Controllers from each of the fires created by this instance.
        /// </summary>
        public FireBuilder WithoutControllers() {
            _data.Controller = null;
            return this;
        }

        #endregion

        #region Damage Functions

        public FireBuilder WithDamage(DynamicInt damage) {
            Damage = damage;
            return this;
        }

        public FireBuilder WithDamage(int min, int max) {
            Damage = new DynamicInt(min, max);
            return this;
        }

        #endregion

        #region Group Functions

        public FireBuilder InGroup(DanmakuGroup group) {
            Group = group;
            return this;
        }

        public FireBuilder WithoutGroup() {
            Group = null;
            return this;
        }

        #endregion
    }

}