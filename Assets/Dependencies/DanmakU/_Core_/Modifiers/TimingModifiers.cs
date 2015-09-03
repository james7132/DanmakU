using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{

    public static class TimingModifierExtensions 
    {
        public static IEnumerable Delay(this IEnumerable coroutine,
                                        Func<FireData, int> frames,
                                        Func<FireData, bool> filter = null)
        {
            if (frames == null)
                throw new ArgumentNullException("frames");
            return coroutine.Inject(frames, fd => true, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        int frames,
                                        Func<FireData, bool> filter = null)
        {
            return coroutine.Delay(fd => frames, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        Func<FireData, float> seconds,
                                        Func<FireData, bool> filter = null)
        {
            if (seconds == null)
                throw new ArgumentNullException("seconds");
            return coroutine.Inject(seconds, fd => true, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        float seconds,
                                        Func<FireData, bool> filter = null)
        {
            return coroutine.Delay(fd => seconds, filter);
        }
    }

}
