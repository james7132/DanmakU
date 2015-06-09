// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{
    [System.Serializable]
    public class DelayedAngleChange : IDanmakuController
    {
        //TODO Document
        //TODO Find a better solution to than this

        [SerializeField, Show] private RotationMode _rotationMode;

        public RotationMode RotationMode
        {
            get { return _rotationMode; }
            set { _rotationMode = value; }
        }

        [SerializeField, Show] private float _delay;

        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        [SerializeField, Show] private DynamicFloat _angle;

        public DynamicFloat Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        [SerializeField, Show] private Transform _target;

        public Transform Target
        {
            get { return _target; }
            set { _target = value; }
        }

        #region implemented abstract members of IDanmakuController

        /// <summary>
        /// Updates the Danmaku controlled by the controller instance.
        /// </summary>
        /// <param name="danmaku">the bullet to update.</param>
        /// <param name="dt">the change in time since the last update</param>
        public void Update(Danmaku danmaku, float dt)
        {
            float time = danmaku.Time;
            if (time >= Delay && time - dt <= Delay)
            {
                float baseAngle = Angle.Value;
                switch (RotationMode)
                {
                    case RotationMode.Relative:
                        baseAngle += danmaku.Rotation;
                        break;
                    case RotationMode.Object:
                        baseAngle += DanmakuUtil.AngleBetween2D(danmaku.Position, Target.position);
                        break;
                    case RotationMode.Absolute:
                        break;
                }
                danmaku.Rotation = baseAngle;
            }
        }

        #endregion
    }
}