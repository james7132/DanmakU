using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static partial class Controller
    {
        public static Action<Danmaku> ColorChange(Func<Danmaku, Color> colorChange)
        {
            if (colorChange == null)
                throw new ArgumentNullException("colorChange");
            return delegate(Danmaku danmaku)
            {
                danmaku.Color = colorChange(danmaku);
            };
        }
    }

}

