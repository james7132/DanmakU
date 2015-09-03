using System;
using System.Collections;
using UnityEngine;

namespace Hourai.DanmakU
{

    public static partial class Modifier
    {
        public static float Rotation(FireData fd)
        {
            return fd.Rotation;
        }

        public static void SetRotation(FireData fd, float rotation)
        {
            fd.Rotation = rotation;
        }

        public static void Rotate(FireData fd, float rotation)
        {
            fd.Rotation += rotation;
        }

        public static Action<FireData> SetRotation(Func<FireData, float> delta)
        {
            return (fd) => fd.Rotation = delta(fd);
        }

        public static Action<FireData> SetRotation(Func<float> delta)
        {
            return (fd) => fd.Rotation = delta();
        }

        public static Action<FireData> SetRotation(float delta)
        {
            return (fd) => fd.Rotation = delta;
        }

        public static Action<FireData> Rotate(Func<FireData, float> delta)
        {
            return (fd) => fd.Rotation += delta(fd);
        }

        public static Action<FireData> Rotate(Func<float> delta)
        {
            return (fd) => fd.Rotation += delta();
        }

        public static Action<FireData> Rotate(float delta)
        {
            return (fd) => fd.Rotation += delta;
        }
    }

    public static class RotationModifierExtensions
    {
        public static IEnumerable InDirection(this IEnumerable coroutine,
                                       Func<FireData, float> angle,
                                       Func<FireData, bool> filter = null)
        {
            if (angle == null)
                throw new ArgumentNullException("angle");
            return coroutine.ForEachFireData(fd => fd.Rotation = angle(fd), filter);
        }

        public static IEnumerable InDirection(this IEnumerable coroutine,
                                               float angle,
                                               Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(fd => fd.Rotation = angle, filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Func<FireData, Vector2> target,
                                          Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(fd => fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position, target(fd)), filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Vector2 target,
                                          Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(fd => fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position, target), filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          GameObject target,
                                          Func<FireData, bool> filter = null)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            Transform trans = target.transform;
            return coroutine.ForEachFireData(delegate(FireData fd)
            {
                if (trans)
                    fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position,
                                                             trans.position);
            },
                                             filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Component target,
                                          Func<FireData, bool> filter = null)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return coroutine.Towards(target.gameObject);
        }
    }
}
