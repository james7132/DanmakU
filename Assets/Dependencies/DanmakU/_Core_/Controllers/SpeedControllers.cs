using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static partial class Controller
    {
        public static Action<Danmaku> Acceleration(Func<Danmaku, float> acceleration) {
            if(acceleration == null)
                throw new ArgumentNullException("acceleration");
            return delegate(Danmaku danmaku) {
                       danmaku.Speed  += acceleration(danmaku) * Danmaku.dt;
                   };
        }

        public static Action<Danmaku> Acceleration(float acceleration) {
            return delegate(Danmaku danmaku)
            {
                danmaku.Speed += acceleration * Danmaku.dt;
            };
        }
    
        public static Action<Danmaku> MaxSpeedLimit(Func<Danmaku, float> speedLimit) {
            if(speedLimit == null)
                throw new ArgumentNullException("speedLimit");
            return delegate(Danmaku danmaku) {
                       float limit = speedLimit(danmaku);
                       if (danmaku.Speed > limit)
                           danmaku.Speed = limit;
                   };
        }

        public static Action<Danmaku> MaxSpeedLimit(float speedLimit)
        {
            return delegate(Danmaku danmaku) {
                if (danmaku.Speed > speedLimit)
                    danmaku.Speed = speedLimit;
            };
        }

        public static Action<Danmaku> MinSpeedLimit(Func<Danmaku, float> speedLimit)
        {
            if (speedLimit == null)
                throw new ArgumentNullException("speedLimit");
            return delegate(Danmaku danmaku)
            {
                float limit = speedLimit(danmaku);
                if (danmaku.Speed < limit)
                    danmaku.Speed = limit;
            };
        }

        public static Action<Danmaku> MinSpeedLimit(float speedLimit)
        {
            return delegate(Danmaku danmaku)
            {
                if (danmaku.Speed < speedLimit)
                    danmaku.Speed = speedLimit;
            };
        }
    }
}