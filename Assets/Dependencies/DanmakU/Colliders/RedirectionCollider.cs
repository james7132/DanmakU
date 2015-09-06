// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai.DanmakU.Collider {

    /// <summary>
    /// A DanmakuCollider that changes the direction of motion for all valid bullets that come into contact with it.
    /// </summary>
    [AddComponentMenu("Hourai.DanmakU/Colliders/Redirection Collider")]
    public class RedirectionCollider : DanmakuCollider {


        public enum RotationType
        {
            Absolute,
            Relative,
            Reflection,
            Object
        }

        private DanmakuGroup affected;

        [SerializeField]
        private float angle;

        //TODO Document

        [SerializeField]
        private RotationType rotationMode;

        [Serialize]
        public Transform Target { get; set; }

        public RotationType RotationMode {
            get { return rotationMode; }
            set { rotationMode = value; }
        }

        public float Angle {
            get { return angle; }
            set { angle = value; }
        }

        /// <summary>
        /// Called on Component instantiation
        /// </summary>
        protected override void Awake() {
            base.Awake();
            affected = DanmakuGroup.Set();
        }

        #region implemented abstract members of DanmakuCollider

        /// <summary>
        /// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
        /// </summary>
        /// <param name="danmaku">the danmaku that hit the collider.</param>
        /// <param name="info">additional information about the collision</param>
        protected override void DanmakuCollision(Danmaku danmaku,
                                                 RaycastHit2D info) {
            if (affected.Contains(danmaku))
                return;
            if (rotationMode == RotationType.Reflection)
            {
                Vector2 normal = info.normal;
                Vector2 direction = danmaku.direction;
                danmaku.Direction = direction -
                                    2 * Vector2.Dot(normal, direction) * normal;
                affected.Add(danmaku);
                return;
            }

            float baseAngle = angle;
            switch (rotationMode) {
                case RotationType.Relative:
                    baseAngle += danmaku.Rotation;
                    break;
                case RotationType.Object:
                    if (Target != null) {
                        baseAngle += DanmakuUtil.AngleBetween2D(
                                                                danmaku.Position,
                                                                Target.position);
                    } else {
                        Debug.LogWarning(
                                         "Trying to direct at an object but no Target object assinged");
                    }
                    break;
                case RotationType.Absolute:
                    break;
            }
            danmaku.Rotation = baseAngle;
            affected.Add(danmaku);
        }

        #endregion
    }

}