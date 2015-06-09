// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{
    [System.Serializable]
    public class SpeedCurveController : IDanmakuController
    {
        [SerializeField, Show]
        public bool Absolute { get; set; }

        [SerializeField, Show] private readonly AnimationCurve _speedCurve;

        public AnimationCurve SpeedCurve
        {
            get { return _speedCurve; }
        }

        public SpeedCurveController(AnimationCurve speedCurve)
        {
            if (speedCurve == null)
                throw new ArgumentNullException("speedCurve");
            _speedCurve = speedCurve;
        }

        #region IDanmakuController implementation

        public virtual void Update(Danmaku danmaku, float dt)
        {
            if (Absolute)
            {
                danmaku.Speed = _speedCurve.Evaluate(danmaku.Time);
            }
            else
            {
                float time = danmaku.Time;
                float oldTime = time - dt;
                if (oldTime > 0)
                {
                    float deltaV = _speedCurve.Evaluate(time) - _speedCurve.Evaluate(oldTime);
                    danmaku.Speed += deltaV;
                }
            }
        }

        #endregion
    }

    [System.Serializable]
    public class AngularSpeedCurveController : IDanmakuController
    {
        [SerializeField, Show]
        public bool Absolute { get; set; }

        [SerializeField, Show] private AnimationCurve angularSpeedCurve;

        public AnimationCurve AngularSpeedCurve
        {
            get { return angularSpeedCurve; }
        }

        #region IDanmakuController implementation

        /// <summary>
        /// Updates the Danmaku controlled by the controller instance.
        /// </summary>
        /// <returns>the displacement from the Danmaku's original position after udpating</returns>
        /// <param name="dt">the change in time since the last update</param>
        /// <param name="danmaku">Danmaku.</param>
        public virtual void Update(Danmaku danmaku, float dt)
        {
            if (Absolute)
            {
                danmaku.AngularSpeed = angularSpeedCurve.Evaluate(danmaku.Time);
            }
            else
            {
                float time = danmaku.Time;
                float oldTime = time - dt;
                if (oldTime > 0)
                {
                    float deltaV = angularSpeedCurve.Evaluate(time) - angularSpeedCurve.Evaluate(oldTime);
                    danmaku.AngularSpeed += deltaV;
                }
            }
        }

        #endregion
    }
}