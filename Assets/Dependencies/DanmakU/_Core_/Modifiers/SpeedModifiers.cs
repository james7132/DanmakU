using System;
using UnityEngine;
using System.Collections;

namespace Hourai.DanmakU
{
    public static partial class Modifier
    {
        public static float Speed(FireData fd)
        {
            return fd.Speed;
        }

        public static void SetSpeed(FireData fd, float speed)
        {
            fd.Speed = speed;
        }

        public static void Accelerate(FireData fd, float delta)
        {
            fd.Speed += delta;
        }

        public static Action<FireData> SetSpeed(Func<FireData, float> delta)
        {
            return (fd) => fd.Speed = delta(fd);
        }

        public static Action<FireData> SetSpeed(Func<float> delta)
        {
            return (fd) => fd.Speed = delta();
        }

        public static Action<FireData> SetSpeed(float delta)
        {
            return (fd) => fd.Speed = delta;
        }

        public static Action<FireData> Accelerate(Func<FireData, float> delta)
        {
            return (fd) => fd.Speed += delta(fd);
        }

        public static Action<FireData> Accelerate(Func<float> delta)
        {
            return (fd) => fd.Speed += delta();
        }

        public static Action<FireData> Accelerate(float delta)
        {
            return (fd) => fd.Speed += delta;
        }
    }

    public static class SpeedModifierExtensions
    {
        public static IEnumerable WithSpeed(this IEnumerable coroutine,
                                            Func<FireData, float> speed,
                                            Func<FireData, bool> filter = null)
        {
            if (speed == null)
                throw new ArgumentNullException("speed");
            return coroutine.ForEachFireData(Modifier.SetSpeed(speed), filter);
        }

        public static IEnumerable WithSpeed(this IEnumerable coroutine,
                                    Func<float> speed,
                                    Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(Modifier.SetSpeed(speed), filter);
        }

        public static IEnumerable WithSpeed(this IEnumerable coroutine,
                                            float speed,
                                            Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(Modifier.SetSpeed(speed), filter);
        }
    }
}
