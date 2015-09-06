// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hourai.DanmakU {

    /// <summary>
    /// A single projectile fired.
    /// The base object that represents a single bullet in a bullet hell game.
    /// </summary>
    public sealed partial class Danmaku {

        /// <summary>
        /// The supported collider shapes used by danmaku.
        /// </summary>
        public enum ColliderType {

            Circle,
            Box,
            Point,
            Line

        }

        internal Action<Danmaku> _onUpdate;
        private DanmakuType type;

        internal Danmaku(int poolIndex, DanmakuType type) {
            PoolIndex = poolIndex;
            this.type = type;
            prefab = type.Prefab;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive {
            get { return PoolIndex < Type._activeCount; }
        }

        /// <summary>
        /// Called when the Danmaku instance is activated.
        /// </summary>
        public event Action<Danmaku> OnActivate;

        /// <summary>
        /// Called when the Danmaku instance is destroyed.
        /// </summary>
        public event Action<Danmaku> OnDestroy;

        public event Action<Danmaku> Controller {
            add {
                _onUpdate += value;
                _controllerCheck = _onUpdate != null;
            }
            remove {
                _onUpdate = _onUpdate.Remove(value);
                _controllerCheck = _onUpdate != null;
            }
        }

        /// <summary>
        /// Clears all cotnrollers from this 
        /// </summary>
        public void ClearControllers() {
            _controllerCheck = true;
            _onUpdate = null;
        }

        internal void Update()
        {
            _originalPosition.x = position.x;
            _originalPosition.y = position.y;
            Vector2 movementVector;

            if (_controllerCheck)
                _onUpdate(this);

            if (AngularSpeed != 0f) {
                float rotationChange = AngularSpeed * dt;
                rotation += rotationChange;
                direction = Util.OnUnitCircle(rotation);
            }

            if (Speed != 0) {
                float movementChange = Speed * dt;
                position.x += direction.x * movementChange;
                position.y += direction.y * movementChange;   
            }

            movementVector.x = position.x - _originalPosition.x;
            movementVector.y = position.y - _originalPosition.y;

            if (CollisionCheck)
            {
                RaycastHit2D[] hits = null;
                float sqrDistance = movementVector.sqrMagnitude;
                float cx = colliderOffset.x;
                float cy = colliderOffset.y;
                if (cx == 0 && cy == 0)
                    _collisionCenter = _originalPosition;
                else {
                    float c = direction.x;
                    float s = direction.y;
                    _collisionCenter.x = _originalPosition.x + c*cx - s*cy;
                    _collisionCenter.y = _originalPosition.y + s*cx + c*cy;
                }

                //Check if the collision detection should be continuous or not
                switch (colliderType) {
                    default:
                        if (sqrDistance > sizeSquared ||
                            Physics2D.OverlapPoint(_collisionCenter,
                                                   colliderMask) != null) {
                            hits = Physics2D.RaycastAll(_collisionCenter,
                                                        movementVector,
                                                        Mathf.Sqrt(sqrDistance),
                                                        colliderMask);
                        }
                        break;
                    case ColliderType.Line:
                        float length = Mathf.Sqrt(sqrDistance) + colliderSize.x;
                        if (sqrDistance > sizeSquared ||
                            Physics2D.Raycast(_collisionCenter,
                                              movementVector,
                                              length,
                                              colliderMask).collider != null)
                        {
                            hits = Physics2D.RaycastAll(_collisionCenter,
                                                        movementVector,
                                                        Mathf.Sqrt(sqrDistance) + colliderSize.x,
                                                        colliderMask);
                        }
                        break;
                    case ColliderType.Circle:
                        if (sqrDistance > sizeSquared ||
                            Physics2D.OverlapCircle(_collisionCenter,
                                                    colliderSize.x,
                                                    colliderMask) != null) {
                            hits = Physics2D.CircleCastAll(_collisionCenter,
                                                           colliderSize.x,
                                                           movementVector,
                                                           Mathf.Sqrt(sqrDistance),
                                                           colliderMask);
                        }
                        break;
                    case ColliderType.Box:
                        hits = Physics2D.BoxCastAll(_collisionCenter,
                                                       colliderSize,
                                                       rotation,
                                                       movementVector,
                                                       Mathf.Sqrt(sqrDistance),
                                                       colliderMask);
                        break;
                }
                if (hits != null && hits.Length > 0)
                {
                    foreach (RaycastHit2D hit in hits) {
                        Collider2D collider = hit.collider;

                        if (collider == null)
                            continue;

                        IDanmakuCollider[] scripts;
                        if (colliderMap.ContainsKey(collider)) {
                            scripts = colliderMap[collider];
                            if (scripts == null) {
                                scripts = collider.GetComponents<IDanmakuCollider>();
                                colliderMap[collider] = scripts;
                            }
                        } else {
                            scripts = collider.GetComponents<IDanmakuCollider>();
                            colliderMap[collider] = scripts;
                        }

                        foreach (IDanmakuCollider script in scripts)
                            script.OnDanmakuCollision(this, hit);
                    }
                }
            }

            frames++;
            time += dt;
        }

        public static implicit operator FireData(Danmaku danmaku) {
            var fd = new FireData () {
                Position = danmaku.Position,
                Rotation = danmaku.Rotation,
                AngularSpeed = danmaku.AngularSpeed,
                Speed = danmaku.Speed,
                Prefab = danmaku.prefab,
                Controller = danmaku._onUpdate,
                Damage = danmaku.Damage,
            };

            return fd;
        }

        public void Activate() {
            if (IsActive)
                return;
            Type.Activate(this);
            if (OnActivate != null)
                OnActivate(this);
            frames = 0;
            time = 0f;
        }

        internal void ActivateImpl()
        {
        }

        /// <summary>
        /// Marks the instance for destruction.
        /// 
        /// </summary>
        public void Destroy()
        {
            if (!IsActive)
                return;
            Type.Return(this);
            DestroyImpl();
        }

        /// <summary>
        /// What actually destroys 
        /// Note that this does not return the Danmaku to the pool.
        /// This is used for 
        /// </summary>
        internal void DestroyImpl()
        {
            if (OnDestroy != null)
                OnDestroy(this);

            _onUpdate = null;
            OnActivate = null;
            OnDestroy = null;
            _controllerCheck = false;
            Damage = 0;
            Speed = 0;
            AngularSpeed = 0;
            position.x = 0;
            position.y = 0;
            rotation = 0;
            CollisionCheck = true;
        }

        #region Private and Internal Fields

        internal int PoolIndex;
        internal Vector2 direction;

        //Cached information about the Danmaku from its prefab
        internal ColliderType colliderType = ColliderType.Circle;
        internal Vector2 colliderOffset = Vector2.zero;
        internal Vector2 colliderSize = Vector2.zero;
        internal float sizeSquared;
        internal int layer;
        internal int frames;
        internal float time;

        //Prefab information
        internal DanmakuPrefab prefab;

        //Collision related variables
        private int colliderMask;

        internal Vector3 position;
        internal float rotation;

        //Preallocated variables to avoid allocation in Update
        private Vector2 _originalPosition;
        private Vector2 _collisionCenter;

        //Cached check for controllers to avoid needing to calculate them in Update
        private bool _controllerCheck;

        #endregion

        #region Public Properties

        /// <summary>
        /// The vertex color to use when rendering
        /// </summary>
        public Color32 Color;

        public float Damage;

        /// <summary>
        /// Whether or not to perform collision detection with the Danmaku instance.
        /// </summary>
        public bool CollisionCheck = true;

        public float Speed;

        public float AngularSpeed;

        public DanmakuPrefab Prefab {
            get { return prefab; }
            set
            {
                if (prefab)
                    prefab.Match(this);
            }
        }

        public DanmakuType Type
        {
            get { return prefab.type; }
        }

        /// <summary>
        /// Gets or sets the position, in world space, of the projectile.
        /// </summary>
        /// <value>The position of the projectile.</value>
        public Vector2 Position {
            get { return position; }
            set {
                position.x = value.x;
                position.y = value.y;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the projectile, in degrees.
        /// </summary>
        /// <remarks>
        /// If viewed from a unrotated orthographic camera:
        /// 0 - Straight right
        /// 90 - Straight up
        /// 180 - Straight left
        /// 270 -  Straight down
        /// </remarks>
        /// <value>The rotation of the bullet in degrees.</value>
        public float Rotation {
            get { return rotation; }
            set {
                rotation = value;
                direction = Util.OnUnitCircle(rotation);
            }
        }

        /// <summary>
        /// Gets the direction vector the projectile is facing.
        /// </summary>
        /// <remarks>
        /// It is a unit vector.
        /// Changing <see cref="Rotation"/> will change this vector.
        /// </remarks>
        /// <value>The direction vector the projectile is facing toward.</value>
        public Vector2 Direction {
            get { return direction; }
            set {
                direction = value.normalized;
                rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }
        }

        public Vector2 Velocity {
            get { return direction * Speed; }
        }

        public float Size;

        /// <summary>
        /// The amount of time that has passed since this bullet has been fired.
        /// </summary>
        /// <value>The time since the projectile has been fired, in seconds.</value>
        public float Time {
            get { return time; }
        }

        /// <summary>
        /// The number of frames passed since this bullet has been fired.
        /// </summary>
        /// <value>The number of frames that have passed since this bullet has been fired.</value>
        public int Frames {
            get { return frames; }
        }

        /// <summary>
        /// Gets the instance's tag.
        /// </summary>
        /// <remarks>
        /// This is initialzied to the tag on the DanmakuPrefab.
        /// Note that changing the tag on any Danmaku will change the tag on every Danmaku spawned from
        /// the prefab that spawned that instance.
        /// </remarks>
        /// <value>The tag of the projectile.</value>
        public string Tag {
            get { return prefab.tag; }
            set { prefab.tag = value; }
        }

        /// <summary>
        /// Gets or sets the instance's layer.
        /// </summary>
        /// <remarks>
        /// Unlike GameObject's layers, this layer value only affects collision behavior.
        /// Lighting and 
        /// </remarks>
        /// <value>The layer used for collision detection.</value>
        public int Layer {
            get { return layer; }
            set {
                layer = value;
                colliderMask = collisionMask[layer];
            }
        }

        public Vector2 ColliderSize
        {
            get { return colliderSize; }
            set
            {
                colliderSize = value;
                sizeSquared = value.x * value.x;
            }
        }

        public Vector2 ColliderOffset
        {
            get { return colliderOffset; }
            set
            {
                colliderOffset = prefab.cachedScale.Hadamard2(value);
            }
        }

        #endregion
    }

}