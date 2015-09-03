using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static partial class Controller
    {
        public static Action<Danmaku> Homing(GameObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            Transform trans = target.transform;
            return delegate(Danmaku danmaku)
            {
                if (trans)
                    danmaku.Rotation = DanmakuUtil.AngleBetween2D(danmaku.position,
                                                                  trans.position);
            };
        }

        public static Action<Danmaku> Homing(Component target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return Homing(target.gameObject);
        }

        public static Action<Danmaku> Homing(Func<Danmaku, Transform> target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return delegate(Danmaku danmaku)
            {
                Transform trans = target(danmaku);
                if (trans)
                    danmaku.Rotation = DanmakuUtil.AngleBetween2D(danmaku.position,
                                                                  trans.position);
            };
        }
    }
}