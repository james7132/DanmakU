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

        private bool _isActive;
        private Action<Danmaku> _onUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hourai.DanmakU.Danmaku"/> class.
        /// </summary>
        internal Danmaku(int poolIndex) {
            PoolIndex = poolIndex;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <remarks>
        /// Setting it to true while inactive is equal to calling Activate.
        /// Setting it to false while active is equal to calling DeactivateImmediate.
        /// </remarks>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive {
            get { return _isActive; }
            set {
                if (_isActive) {
                    if (!value)
                        DeactivateImmediate();
                } else {
                    if (value)
                        Activate();
                }
                _isActive = value;
            }
        }

        /// <summary>
        /// Occurs when the Danmaku instance is activated.
        /// </summary>
        public event Action<Danmaku> OnActivate;

        /// <summary>
        /// Occurs when the Danmaku instance is deactivated.
        /// </summary>
        public event Action<Danmaku> OnDeactivate;

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
                direction = FastTrig.UnitCircle(rotation);   
            }

            if (Speed != 0) {
                float movementChange = Speed * dt;
                position.x += direction.x * movementChange;
                position.y += direction.y * movementChange;   
            }

            movementVector.x = position.x - _originalPosition.x;
            movementVector.y = position.y - _originalPosition.y;

            //Debug.DrawRay(originalPosition, movementVector);

            if (CollisionCheck)
            {
                RaycastHit2D[] hits = null;
                float sqrDistance = movementVector.sqrMagnitude;
                float cx = colliderOffset.x;
                float cy = colliderOffset.y;
                if (Mathf.Approximately(cx, 0f) && Mathf.Approximately(cy, 0f))
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
                if (hits != null && hits.Length > 0) {
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

                        if (!to_deactivate)
                            continue;

                        position.x = hit.point.x;
                        position.y = hit.point.y;
                        DeactivateImmediate();
                        return;
                    }
                }
            }

            if (!_isActive) {
                DeactivateImmediate();
                return;
            }

            frames++;
            time += dt;
        }

        public void MatchPrefab(DanmakuPrefab prefab) {
            if (prefab == null) {
                Debug.LogError("Tried to match a null prefab");
                return;
            }
            if (this.prefab != prefab) {
                this.prefab = prefab;

                if (_isActive) {
                    runtime.currentDanmaku.Remove(this);
                    runtime = prefab.GetRuntime();
                    runtime.currentDanmaku.Add(this);
                } else
                    runtime = prefab.GetRuntime();

                Vector2 scale = runtime.cachedScale;
                colliderType = runtime.collisionType;
                switch (colliderType) {
                    default:
                        colliderSize = Vector2.zero;
                        sizeSquared = 0;
                        break;
                    case ColliderType.Circle:
                        colliderSize = runtime.colliderSize*scale.Max();
                        break;
                    case ColliderType.Line:
                        colliderSize = runtime.colliderSize;
                        break;
                }
                sizeSquared = colliderSize.y * colliderSize.y;
                colliderOffset = scale.Hadamard2(runtime.colliderOffset);
            }

            Color = runtime.Color;
            Scale = 1f;
            layer = runtime.cachedLayer;
            colliderMask = collisionMask[layer];
        }

        public static implicit operator FireData(Danmaku danmaku) {
            var fd = new FireData {
                Position = danmaku.Position,
                Rotation = danmaku.Rotation,
                AngularSpeed = danmaku.AngularSpeed,
                Speed = danmaku.Speed,
                Prefab = danmaku.Prefab,
                Controller = danmaku._onUpdate,
                Damage = danmaku.Damage,
            };

            return fd;
        }

        /// <summary>
        /// Fires a single bullet from the bullet's current position.
        /// </summary>
        /// <remarks>
        /// By default, firing using this method also uses the rotation of the bullet to fire the bullet.
        /// Set <c>useRotation</c> to false to disable this.
        /// </remarks>
        /// <param name="data">the data used to create the .</param>
        /// <param name="useRotation">If set to <c>true</c>, the bullet will use the current rotation of the bullet to fire with.</param>
        public Danmaku Fire(FireData data, bool useRotation = true) {
            Vector2 tempPos = data.Position;
            float tempRot = data.Rotation;
            data.Position = Position;
            if (useRotation)
                data.Rotation = Rotation;
            Danmaku danmaku = data.Fire();
            data.Position = tempPos;
            data.Rotation = tempRot;
            return danmaku;
        }

        /// <summary>
        /// Activates the Danmaku instance.
        /// </summary>
        /// <remarks>
        /// Calling this on a already active instance does nothing.
        /// Calling this on a instance marked for deactivation will unmark the projectile and keep it from deactivating.
        /// </remarks>
        public void Activate() {
            if (DanmakuGame.Instance == null)
                new GameObject("Danmaku Game Controller").AddComponent<DanmakuGame>();
            to_deactivate = false;
            runtime.currentDanmaku.Add(this);
            if (_isActive)
                return;
            if(OnActivate != null)
                OnActivate(this);
            _isActive = true;
            frames = 0;
            time = 0f;
        }

        /// <summary>
        /// Marks the instance for deactivation.
        /// </summary>
        /// <remarks>
        /// Deactivated bullets are removed from the active set, and all 
        /// The instance  removed from the active set and all bullet functionality will cease after current 
        /// If Danmaku needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
        /// </remarks>
        public void Deactivate() {
            to_deactivate = true;
        }

        /// <summary>
        /// Immediately deactivates this Danmaku and ceases all processing done on it.
        /// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
        /// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
        /// </summary>
        public void DeactivateImmediate() {
            if (_isActive && OnDeactivate != null)
                OnDeactivate(this);

            _onUpdate = null;
            OnActivate = null;
            OnDeactivate = null;
            _controllerCheck = false;
            Damage = 0;
            CollisionCheck = true;
            _isActive = false;
            danmakuPool.Return(this);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Hourai.DanmakU.Danmaku"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode() {
            return PoolIndex;
        }

        #region Private and Internal Fields

        /// <summary>
        /// The Danmaku instance's index within the DanmakuPool.
        /// </summary>
        internal readonly int PoolIndex;

        //internal int renderIndex;

        internal Vector2 direction;

        //Cached information about the Danmaku from its prefab
        internal ColliderType colliderType = ColliderType.Circle;
        internal Vector2 colliderOffset = Vector2.zero;
        internal Vector2 colliderSize = Vector2.zero;
        private float sizeSquared;
        internal int layer;
        internal int frames;
        internal float time;

        //Prefab information
        private DanmakuPrefab prefab;
        private DanmakuPrefab runtime;

        //Collision related variables
        private int colliderMask;

        private bool to_deactivate;

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
        public bool CollisionCheck;

        public float Speed;

        public float AngularSpeed;

        public DanmakuPrefab Prefab {
            get { return runtime; }
        }

        public Sprite Sprite {
            get { return runtime.Sprite; }
        }

        public Mesh Mesh {
            get { return runtime.Mesh; }
        }

        public Material Material {
            get {
                //return material;
                return runtime.Material;
            }
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
        /// 0 - Straight up
        /// 90 - Straight Left
        /// 180 - Straight Down
        /// 270 -  Straight Right
        /// </remarks>
        /// <value>The rotation of the bullet in degrees.</value>
        public float Rotation {
            get { return rotation; }
            set {
                rotation = value;
                direction = FastTrig.UnitCircle(value);
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
                rotation = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg -
                           90f;
            }
        }

        public float Scale;

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
        /// </remarks>
        /// <value>The layer used for collision detection.</value>
        public int Layer {
            get { return layer; }
            set {
                layer = value;
                colliderMask = collisionMask[layer];
            }
        }

        #endregion

        #region Position Functions

        /// <summary>
        /// Moves the bullet closer to the specified target point.
        /// 
        /// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
        /// </summary>
        /// <param name="target">The target position to move towards in absolute world coordinates.</param>
        /// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
        public void MoveTowards(Vector2 target, float maxDistanceDelta) {
            Position = Vector2.MoveTowards(position, target, maxDistanceDelta);
        }

        /// <summary>
        /// Moves the bullet closer to the specified target Transform's position.
        /// 
        /// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the target Transform is null.</exception>
        /// <param name="target">The Transform of the object to move towards.</param>
        /// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
        public void MoveTowards(Transform target, float maxDistanceDelta) {
            if (target == null)
                throw new ArgumentNullException();
            Position = Vector2.MoveTowards(position,
                                           target.position,
                                           maxDistanceDelta);
        }

        /// <summary>
        /// Moves the bullet closer to the specified target Component's position.
        /// 
        /// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the target Component is null.</exception>
        /// <param name="target">The Component of the object to move towards.</param>
        /// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
        public void MoveTowards(Component target, float maxDistanceDelta) {
            if (target == null)
                throw new ArgumentNullException();
            Position = Vector2.MoveTowards(position,
                                           target.transform.position,
                                           maxDistanceDelta);
        }

        /// <summary>
        /// Moves the bullet closer to the specified target GameObject's position.
        /// 
        /// If <c>maxDisntanceDelta</c> is negative, the bullet will instead move away from the target point.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the target GameObject is null.</exception>
        /// <param name="target">The GameObject of the object to move towards.</param>
        /// <param name="maxDistanceDelta">The maximum distance traversed by a single call to this function.</param>
        public void MoveTowards(GameObject target, float maxDistanceDelta) {
            if (target == null)
                throw new System.ArgumentNullException();
            Position = Vector2.MoveTowards(position,
                                           target.transform.position,
                                           maxDistanceDelta);
        }

        public void Translate(Vector2 deltaPos) {
            Position += deltaPos;
        }

        #endregion
    }

}