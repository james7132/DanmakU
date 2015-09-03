using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{

    public static class BurstModifierExtensions
    {

        public static IEnumerable RadialBurst(this IEnumerable data,
                                              Func<FireData, int> count,
                                              Func<FireData, float> range,
                                              Func<FireData, bool> filter = null)
        {

            if (count == null || range == null)
                throw new ArgumentNullException();

            float delta = 0f;

            Action<FireData, int> setup =
                delegate(FireData fd, int currentCount)
                {

                    if (currentCount <= 1)
                        return;

                    float currentRange = range(fd);
                    delta = currentRange / currentCount;
                    fd.Rotation -= 0.5f * currentRange;
                };

            Action<FireData> edit = (fd) => fd.Rotation += delta;

            return data.Burst(count, edit, setup, filter);
        }

        public static IEnumerable RadialBurst(this IEnumerable coroutine,
                                              int count,
                                              float range = 360f,
                                              Func<FireData, bool> filter = null)
        {
            if (count == 1)
                return coroutine;
            return coroutine.RadialBurst(count.Wrap(), range.Wrap(), filter);
        }

        public static IEnumerable LinearBurst(this IEnumerable data,
                                              Func<FireData, int> count,
                                              Func<FireData, float> dSpeed,
                                              Func<FireData, bool> filter = null)
        {
            return data.Burst(count, (fd) => fd.Speed += dSpeed(fd), null, filter);
        }

        public static IEnumerable LinearBurst(this IEnumerable data,
                                              int count,
                                              float dSpeed,
                                              Func<FireData, bool> filter = null)
        {
            return data.Burst(count.Wrap(), (fd) => fd.Speed += dSpeed, null, filter);
        }
    }
}
