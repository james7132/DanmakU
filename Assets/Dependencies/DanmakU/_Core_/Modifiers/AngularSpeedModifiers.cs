using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{

    public static partial class Modifier
    {
        public static float AngularSpeed(FireData fd)
        {
            return fd.AngularSpeed;
        }

        public static void SetAngularSpeed(FireData fd, float speed)
        {
            fd.AngularSpeed = speed;
        }

        public static void AngularAccelerate(FireData fd, float delta)
        {
            fd.AngularSpeed += delta;
        }

        public static Action<FireData> SetAngularSpeed(Func<FireData, float> delta)
        {
            return (fd) => fd.AngularSpeed += delta(fd);
        }

        public static Action<FireData> SetAngularSpeed(Func<float> delta)
        {
            return (fd) => fd.AngularSpeed += delta();
        }

        public static Action<FireData> SetAngularSpeed(float delta)
        {
            return (fd) => fd.AngularSpeed += delta;
        }

        public static Action<FireData> AngularAccelerate(Func<FireData, float> delta)
        {
            return (fd) => fd.AngularSpeed += delta(fd);
        }

        public static Action<FireData> AngularAccelerate(Func<float> delta)
        {
            return (fd) => fd.AngularSpeed += delta();
        }

        public static Action<FireData> AngularAccelerate(float delta)
        {
            return (fd) => fd.AngularSpeed += delta;
        }
    }

    public static class AngularSpeedModifierExtensions
    {

        public static IEnumerable WithAngularSpeed(this IEnumerable coroutine,
                                            Func<FireData, float> angSpeed,
                                            Func<FireData, bool> filter = null)
        {
            if (angSpeed == null)
                throw new ArgumentNullException("angSpeed");
            return coroutine.ForEachFireData(Modifier.SetAngularSpeed(angSpeed), filter);
        }

        public static IEnumerable WithAngularSpeed(this IEnumerable coroutine,
                                            float angSpeed,
                                            Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(Modifier.SetAngularSpeed(angSpeed), filter);
        }
    }

}
