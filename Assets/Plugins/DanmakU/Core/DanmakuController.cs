// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;

namespace DanmakU {

    /// <summary>
    /// Delegate form of IDanmakuController
    /// </summary>
    public delegate void DanmakuController(Danmaku danmaku, float dt);

    public static class Controller
    {
        #region Acceleration Controllers
        public static DanmakuController Acceleration(Func<Danmaku, float> acceleration) {
            if(acceleration == null)
                throw new ArgumentNullException("acceleration");
            return delegate(Danmaku danmaku, float dt) {
                       danmaku.Speed  += acceleration(danmaku) * dt;
                   };
        }

        public static DanmakuController Acceleration(float acceleration) {
            return delegate(Danmaku danmaku, float dt)
            {
                danmaku.Speed += acceleration * dt;
            };
        }
        #endregion

        #region Deactivation Controllers

        public static DanmakuController Deactivation(Func<Danmaku, bool> deactivation) {
            if(deactivation == null)
                throw new ArgumentNullException("deactivation");
            return delegate(Danmaku danmaku, float dt)
            {
                if(deactivation(danmaku))
                    danmaku.Deactivate();
            };
        }

        #endregion

        #region Speed Limiters

        public static DanmakuController MaxSpeedLimit(Func<Danmaku, float> speedLimit) {
            if(speedLimit == null)
                throw new ArgumentNullException("speedLimit");
            return delegate(Danmaku danmaku, float dt) {
                       float limit = speedLimit(danmaku);
                       if (danmaku.Speed > limit)
                           danmaku.Speed = limit;
                   };
        }

        public static DanmakuController MaxSpeedLimit(float speedLimit)
        {
            return delegate(Danmaku danmaku, float dt) {
                if (danmaku.Speed > speedLimit)
                    danmaku.Speed = speedLimit;
            };
        }

        public static DanmakuController MinSpeedLimit(Func<Danmaku, float> speedLimit)
        {
            if (speedLimit == null)
                throw new ArgumentNullException("speedLimit");
            return delegate(Danmaku danmaku, float dt)
            {
                float limit = speedLimit(danmaku);
                if (danmaku.Speed < limit)
                    danmaku.Speed = limit;
            };
        }

        public static DanmakuController MinSpeedLimit(float speedLimit)
        {
            return delegate(Danmaku danmaku, float dt)
            {
                if (danmaku.Speed < speedLimit)
                    danmaku.Speed = speedLimit;
            };
        }

        public static DanmakuController ClampSpeed(Func<Danmaku, DFloat> ranges)
        {
            if (ranges == null)
                throw new ArgumentNullException("ranges");
            return delegate(Danmaku danmaku, float dt) {
                       DFloat range = ranges(danmaku);
                       float speed = danmaku.Speed;
                       if (speed > range.Max)
                           danmaku.Speed = range.Max;
                       else if (speed < range.Min)
                           danmaku.Speed = range.Min;
            };
        }

        public static DanmakuController ClampSpeed(DFloat range)
        {
            return delegate(Danmaku danmaku, float dt)
            {
                float speed = danmaku.Speed;
                if (speed > range.Max)
                    danmaku.Speed = range.Max;
                else if (speed < range.Min)
                    danmaku.Speed = range.Min;
            };
        }

        #endregion

        #region Color Controllers

        public static DanmakuController ColorChange(Func<Danmaku, Color> colorChange) {
            if(colorChange == null)
                throw new ArgumentNullException("colorChange");
            return delegate(Danmaku danmaku, float dt) {
                       danmaku.Color = colorChange(danmaku);
                   };
        }

        #endregion

        #region Homing Controllers

        public static DanmakuController Homing(GameObject target) {
            if(target == null)
                throw new ArgumentNullException("target");
            Transform trans = target.transform;
            return delegate(Danmaku danmaku, float dt) {
                        if (trans)
                            danmaku.Rotation = DanmakuUtil.AngleBetween2D(danmaku.position,
                                                                          trans.position);
                   };
        }

        public static DanmakuController Homing(Component target) {
            if (target == null)
                throw new ArgumentNullException("target");
            return Homing(target.gameObject);
        }

        public static DanmakuController Homing(Func<Danmaku, Transform> target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return delegate(Danmaku danmaku, float dt)
            {
                Transform trans = target(danmaku);
                if (trans)
                    danmaku.Rotation = DanmakuUtil.AngleBetween2D(danmaku.position,
                                                                  trans.position);
            };
        }

        #endregion
    }

}