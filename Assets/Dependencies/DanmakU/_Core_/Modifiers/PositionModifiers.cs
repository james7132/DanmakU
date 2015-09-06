using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Hourai.DanmakU
{

    public static partial class Modifier
    {
        public static Vector2 Position(FireData fd)
        {
            return fd.Position;
        }

        public static void SetPosition(FireData fd, Vector2 position)
        {
            fd.Position = position;
        }

        public static void Move(FireData fd, Vector2 delta)
        {
            fd.Position += delta;
        }

        public static Action<FireData> SetPosition(Func<FireData, Vector2> delta)
        {
            return (fd) => fd.Position += delta(fd);
        }

        public static Action<FireData> SetPosition(Func<Vector2> delta)
        {
            return (fd) => fd.Position += delta();
        }

        public static Action<FireData> SetPosition(Vector2 delta)
        {
            return (fd) => fd.Position += delta;
        }

        public static Action<FireData> Move(Func<FireData, Vector2> delta)
        {
            return (fd) => fd.Position += delta(fd);
        }

        public static Action<FireData> Move(Func<Vector2> delta)
        {
            return (fd) => fd.Position += delta();
        }

        public static Action<FireData> Move(Vector2 delta)
        {
            return (fd) => fd.Position += delta;
        }
    }

    public static class PositionModifierExtensions
    {
        public static IEnumerable From(this IEnumerable data, Func<FireData, Vector2> position, Func<FireData, bool> filter = null)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            return data.ForEachFireData(fd => fd.Position = position(fd), filter);
        }

        public static IEnumerable From(this IEnumerable data, Vector2 position, Func<FireData, bool> filter = null)
        {
            return data.ForEachFireData(Modifier.SetPosition(position), filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<Vector2>> positions,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.Duplicate(positions, Modifier.SetPosition, false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<Vector2> positions,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.From(fd => positions, filter);
        }

        public static IEnumerable From(this IEnumerable data, GameObject gameObject, Func<FireData, bool> filter = null)
        {
            if (gameObject == null)
                return data;
            Transform trans = gameObject.transform;
            return data.ForEachFireData(fd => fd.Position = ((trans) ? (Vector2)trans.position : fd.Position), filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<GameObject>> gameObjects,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.Duplicate(gameObjects, ((data, o) => data.Position = o.transform.position), false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<GameObject> gameObjects,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.From(gameObjects.Wrap(), filter);
        }

        public static IEnumerable From(this IEnumerable data, Component component, Func<FireData, bool> filter = null)
        {
            return component ? data.From(component.gameObject) : data;
        }

        public static IEnumerable From<T>(this IEnumerable coroutine,
                                           Func<FireData, IEnumerable<T>> components,
                                           Func<FireData, bool> filter = null) where T : Component
        {
            return coroutine.Duplicate(components, ((data, o) => data.Position = o.transform.position), false, filter);
        }

        public static IEnumerable From<T>(this IEnumerable coroutine,
                                           IEnumerable<T> components,
                                           Func<FireData, bool> filter = null) where T : Component
        {
            return coroutine.From(fd => components, filter);
        }

        public static IEnumerable From(this IEnumerable data, Danmaku danmaku, Func<FireData, bool> filter = null)
        {
            if (danmaku == null)
                throw new ArgumentNullException("danmaku");
            return data.ForEachFireData(fd => fd.Position = (danmaku ? danmaku.Position : fd.Position), filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<Danmaku>> danmaku,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.Duplicate(danmaku, ((data, o) => data.Position = o.Position), false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<Danmaku> danmaku,
                                       Func<FireData, bool> filter = null)
        {
            return coroutine.From(fd => danmaku, filter);
        }
    }
}
