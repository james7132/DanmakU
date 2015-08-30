// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vexe.Runtime.Extensions;

namespace DanmakU {

    [System.Serializable]
    public static class FireModifier {

        private static readonly Action<FireData, Vector2> setPos = (fd, pos) => fd.Position = pos;
        private static readonly Action<FireData, float> setRot = (fd, rot) => fd.Rotation = rot;

        #region General Utilty Functions
        public static IEnumerable Flatten(this IEnumerable enumerable)
        {
            if (enumerable == null)
                yield break;
            foreach (object data in enumerable)
            {
                var fd = data as FireData;
                var asEnumerator = data as IEnumerator;
                var asEnumerable = data as IEnumerable;
                if (fd != null)
                    yield return fd;
                else if (asEnumerable != null)
                    foreach (var subData in asEnumerable)
                        yield return subData;
                else if (asEnumerator != null)
                    while (asEnumerator.MoveNext())
                        yield return asEnumerator.Current;
                else
                    yield return data;
            }
        }

        public static IEnumerable FullyFlatten(this IEnumerable enumerable)
        {
            if (enumerable == null)
                yield break;
            Stack<IEnumerator> enumerationStack = new Stack<IEnumerator>();
            enumerationStack.Push(enumerable.GetEnumerator());
            while (enumerationStack.Count > 0)
            {
                IEnumerator iterator = enumerationStack.Peek();
                while (iterator.MoveNext())
                {
                    object current = iterator.Current;
                    var fd = current as FireData;
                    if (fd != null)
                    {
                        yield return fd;
                        continue;
                    }
                    var asIterator = current as IEnumerator;
                    var asEnumerable = current as IEnumerable;
                    if (asEnumerable != null)
                        asIterator = asEnumerable.GetEnumerator();
                    if (asIterator != null)
                    {
                        enumerationStack.Push(asIterator);
                        iterator = asIterator;
                    }
                    else
                    {
                        yield return current;
                    }
                }
                enumerationStack.Pop();
            }
        }


        public static IEnumerable Replace<TIn, TOut>(this IEnumerable coroutine,
                                                     Func<TIn, TOut> replacement,
                                                     Predicate<TIn> filter = null)
        {
            if (replacement == null)
                throw new ArgumentNullException("replacement");
            if (coroutine == null)
                yield break;
            foreach (object data in coroutine)
            {
                if (data is TIn && (filter == null || filter((TIn)data)))
                    yield return replacement((TIn)data);
                else
                    yield return data;
            }
        }

        public static IEnumerable Inject<TIn, TOut>(this IEnumerable coroutine,
                                                    Func<TIn, TOut> injection,
                                                    Predicate<TIn> after = null,
                                                    Predicate<TIn> filter = null) where TIn : class
        {
            if (injection == null)
                throw new ArgumentNullException("injection");
            if (coroutine == null)
                yield break;
            foreach (object data in coroutine)
            {
                var typeChecked = data as TIn;
                if (typeChecked != null && (filter == null || filter(typeChecked)))
                {
                    if (after == null || !after((TIn)data))
                    {
                        yield return data;
                        yield return injection(typeChecked);
                    }
                    else
                    {
                        yield return injection(typeChecked);
                        yield return data;
                    }
                }
                else
                    yield return data;
            }
        }

        public static IEnumerable Duplicate<T>(this IEnumerable coroutine,
                                               Func<FireData, IEnumerable<T>> duplicationFunc,
                                               Action<FireData, T> editFunc = null,
                                               bool includeOriginal = true,
                                               Predicate<FireData> filter = null) {
            if(duplicationFunc == null)
                throw new ArgumentNullException("duplicationFunc");
            if (coroutine == null)
                yield break;

            var alterDummy = new FireData();
            foreach (var obj in coroutine) {
                var fd = obj as FireData;
                if (fd == null || (filter != null && filter(fd))) {
                    yield return obj;
                    continue;
                }
                if (includeOriginal)
                    yield return fd;
                IEnumerable<T> alterations = duplicationFunc(fd);
                if (alterations == null)
                    continue;
                foreach (T alteration in duplicationFunc(fd)) {
                    alterDummy.Copy(fd);
                    editFunc.SafeInvoke(alterDummy, alteration);
                    yield return alterDummy;
                }
            }
        }

        #endregion

        public static Task Fire(this MonoBehaviour behaviour, IEnumerable coroutine)
        {
            return coroutine.Fire(behaviour);
        }

        public static Task Fire(this IEnumerable data, MonoBehaviour context = null) {
            if (data == null)
                return null;
            var fd = data as FireData;
            var dp = data as DanmakuPrefab;
            if (fd != null)
                fd.Fire();
            else if (dp != null)
                ((FireData) dp).Fire();
            else {
                var fireTask = new Task(context ?? DanmakuGameController.Instance,
                                         FireRoutine(data));
                fireTask.Start();
                return fireTask;
            }
            return null;
        }

        private static IEnumerator FireRoutine(IEnumerable dataSource) {
            foreach (object data in dataSource.FullyFlatten())
            {
                var yi = data as YieldInstruction;
                var fd = data as FireData;
                var df = data as DFloat?;
                var di = data as DInt?;
                if (data == null || yi != null)
                    yield return data;
                else if (fd != null)
                    fd.Fire();
                else if (data is int || data is DInt) {
                    int frames;
                    if (data is DInt)
                        frames = ((DInt) data).Value;
                    else
                        frames = (int) data;
                    if (frames <= 0)
                        yield return null;
                    else
                        for (var i = 0; i < frames; i++)
                            yield return null;
                } else if (data is float || data is DFloat) {
                    float seconds;
                    if (data is DFloat)
                        seconds = ((DFloat) data).Value;
                    else
                        seconds = (float) data;
                    if (seconds < TimeUtil.DeltaTime)
                        yield return null;
                    else
                        for (var t = 0f; t < seconds; t += TimeUtil.DeltaTime)
                            yield return null;
                } else {
                    yield return data;
                }
            }
        }

        public static IEnumerable ForEachFireData(this IEnumerable coroutine, 
                                                    Action<FireData> action, 
                                                    Predicate<FireData> filter = null) {
            if (coroutine == null)
                yield break;
            if (action == null)
                throw new ArgumentNullException("action");
            var copy = new FireData();
            foreach (object data in coroutine) {
                var fd = data as FireData;
                if (fd == null || (filter != null && !filter(fd))) {
                    yield return data;
                    continue;
                }
                copy.Copy(fd);
                action(copy);
                yield return copy;
            }
        }

        public static IEnumerable RadialBurst(this IEnumerable data, DFloat range, DInt count, Predicate<FireData> filter = null) {
            
            Func<FireData, IEnumerable<float>> burstFunc = 
                delegate(FireData fd) {
                    int currentCount = count.Value;

                    if (currentCount <= 0)
                        return new float[0];
                    if (currentCount == 1)
                        return new float[] { fd.Rotation };

                    float currentRange = range.Value;
                    float delta = currentRange / count.Value;
                    float start = fd.Rotation.Value - 0.5f * currentRange;
                    float[] set = new float[count.Value];

                    for (var i = 0; i < set.Length; i++)
                        set[i] = start + i * delta;

                    return set;
                };

            Action<FireData, float> setRotation = (fireData, f) => fireData.Rotation = f;

            return data.Duplicate(burstFunc, setRotation, false, filter);
        }

        public static IEnumerable LinearBurst(this IEnumerable data, DInt count, DFloat dSpeed, Predicate<FireData> filter = null) {
            Func<FireData, IEnumerable<float>> burstFunc =
                delegate(FireData fd) {
                    float start = fd.Speed.Value;
                    float delta = dSpeed.Value;
                    float[] set = new float[count.Value];

                    for (var i = 0; i < set.Length; i++)
                        set[i] = start + i * delta;

                    return set;
                };

            Action<FireData, float> setSpeed = (fireData, f) => fireData.Speed = f;

            return data.Duplicate(burstFunc, setSpeed, false, filter);
        }

        public static IEnumerable WithController(this IEnumerable data, 
                                                 Func<FireData, DanmakuController> controller, 
                                                 Predicate<FireData> filter = null) {
            if (controller == null)
                return data;
            return data.ForEachFireData(fireData => fireData.Controller += controller(fireData), filter);
        }

        public static IEnumerable WithoutControllers(this IEnumerable data, Predicate<FireData> filter = null) {
            return data.ForEachFireData(fireData => fireData.Controller = null, filter);
        }

        #region Timing Functions
        public static IEnumerable Delay(this IEnumerable coroutine, 
                                        Func<FireData, DInt> frames, 
                                        Predicate<FireData> filter = null) {
            if(frames == null)
                throw new ArgumentNullException("frames");
            return coroutine.Inject(frames, fd => true, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        DInt frames,
                                        Predicate<FireData> filter = null) {
            return coroutine.Delay(fd => frames, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        Func<FireData, DFloat> seconds,
                                        Predicate<FireData> filter = null)
        {
            if (seconds == null)
                throw new ArgumentNullException("seconds");
            return coroutine.Inject(seconds, fd => true, filter);
        }

        public static IEnumerable Delay(this IEnumerable coroutine,
                                        DFloat seconds,
                                        Predicate<FireData> filter = null)
        {
            return coroutine.Delay(fd => seconds, filter);
        }
        #endregion

        #region Position Functions
        public static IEnumerable From(this IEnumerable data, Func<FireData, Vector2> position, Predicate<FireData> filter = null)
        {
            if(position == null)
                throw new ArgumentNullException("position");
            return data.ForEachFireData(fd => fd.Position = position(fd), filter);
        }

        public static IEnumerable From(this IEnumerable data, Vector2 position, Predicate<FireData> filter = null) {
            return data.ForEachFireData(fireData => fireData.Position = position, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<Vector2>> positions,
                                       Predicate<FireData> filter = null) {
            return coroutine.Duplicate(positions, setPos, false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<Vector2> positions,
                                       Predicate<FireData> filter = null) {
            return coroutine.From(fd => positions, filter);
        }

        public static IEnumerable From(this IEnumerable data, GameObject gameObject, Predicate<FireData> filter = null)
        {
            if (gameObject == null)
                return data;
            Transform trans = gameObject.transform;
            return data.ForEachFireData(fd => fd.Position = ((trans) ? (Vector2) trans.position : fd.Position), filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<GameObject>> gameObjects,
                                       Predicate<FireData> filter = null)
        {
            return coroutine.Duplicate(gameObjects, ((data, o) => data.Position = o.transform.position), false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<GameObject> gameObjects,
                                       Predicate<FireData> filter = null) {
            return coroutine.From(fd => gameObjects, filter);
        }

        public static IEnumerable From(this IEnumerable data, Component component, Predicate<FireData> filter = null) {
            return component ? data.From(component.gameObject) : data;
        }

        public static IEnumerable From(this IEnumerable coroutine,
                               Func<FireData, IEnumerable<Component>> components,
                               Predicate<FireData> filter = null)
        {
            return coroutine.Duplicate(components, ((data, o) => data.Position = o.transform.position), false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<Component> components,
                                       Predicate<FireData> filter = null)
        {
            return coroutine.From(fd => components, filter);
        }

        public static IEnumerable From(this IEnumerable data, Danmaku danmaku, Predicate<FireData> filter = null) {
            if(danmaku == null)
                throw new ArgumentNullException("danmaku");
            return data.ForEachFireData(fd => fd.Position = (danmaku ? danmaku.Position : fd.Position), filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       Func<FireData, IEnumerable<Danmaku>> danmaku,
                                       Predicate<FireData> filter = null)
        {
            return coroutine.Duplicate(danmaku, ((data, o) => data.Position = o.Position), false, filter);
        }

        public static IEnumerable From(this IEnumerable coroutine,
                                       IEnumerable<Danmaku> danmaku,
                                       Predicate<FireData> filter = null)
        {
            return coroutine.From(fd => danmaku, filter);
        }
        #endregion


        public static IEnumerable InDirection(this IEnumerable coroutine,
                                               Func<FireData, DFloat> angle,
                                               Predicate<FireData> filter = null)
        {
            if (angle == null)
                throw new ArgumentNullException("angle");
            return coroutine.ForEachFireData(fd => fd.Rotation = angle(fd), filter);
        }

        public static IEnumerable InDirection(this IEnumerable coroutine,
                                               DFloat angle,
                                               Predicate<FireData> filter = null)
        {
            if (angle == null)
                throw new ArgumentNullException("angle");
            return coroutine.ForEachFireData(fd => fd.Rotation = angle, filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Func<FireData, Vector2> target,
                                          Predicate<FireData> filter = null) {
            return coroutine.ForEachFireData(fd => fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position, target(fd)), filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Vector2 target,
                                          Predicate<FireData> filter = null) {
            return coroutine.ForEachFireData(fd => fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position, target), filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          GameObject target,
                                          Predicate<FireData> filter = null)
        {
            if (target == null)
                throw new ArgumentNullException("gameObj");
            Transform trans = target.transform;
            return coroutine.ForEachFireData(delegate(FireData fd) {
                                                 if (trans)
                                                     fd.Rotation = DanmakuUtil.AngleBetween2D(fd.Position,
                                                                                              trans.position);
                                             },
                                             filter);
        }

        public static IEnumerable Towards(this IEnumerable coroutine,
                                          Component target,
                                          Predicate<FireData> filter = null) 
        {
            if(target == null)
                throw new ArgumentNullException("component");
            return coroutine.Towards(target.gameObject);
        }

    }

}