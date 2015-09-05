using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Vexe.Runtime.Extensions;

namespace Hourai.DanmakU
{
    public static partial class Modifier
    {
        /// <summary>
        /// Concatenates two enumerations together. The second enumeration will be appended to the end of the first.
        /// If the first never ends, the second will never be reached.
        /// If one is null, the other will be returned.
        /// If both are null, an empty enumeration is returned;
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static IEnumerable Concat(this IEnumerable s1, IEnumerable s2)
        {
            if (s1 == null && s2 == null)
                return new object[0];
            if (s1 == null)
                return s2;
            if (s2 == null)
                return s1;
            return ConcatImpl(s1, s2);
        }

        static IEnumerable ConcatImpl(IEnumerable s1, IEnumerable s2)
        {
            foreach (var obj in s1)
                yield return obj;
            foreach (var obj in s2)
                yield return obj;
        }

        public static IEnumerable TerminateOn(this IEnumerable coroutine, Func<bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException("conition");
            foreach (var obj in coroutine)
            {
                yield return obj;
                if (condition())
                    yield break;
            }
        }
    }

    public static class ModifierUtil {

        /// <summary>
        /// Replaces enumerated enumeration with their elements.
        /// This is a shallow replacement, only works on the initial enumeration.
        /// For a deep replacement (a recursive replacement of all enumerated enumerations), use FullyFlatten() instead.
        /// </summary>
        /// <param name="enumeration">the target enumeration of enumerations</param>
        /// <returns>an flattened enumeration. Returns an empty enumeration if <paramref name="enumeration"/> is null.</returns>
        public static IEnumerable Flatten(this IEnumerable enumeration)
        {
            if (enumeration == null)
                yield break;
            foreach (object data in enumeration)
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

        /// <summary>
        /// Recursively replaces all enumerated enumerations with their elements.
        /// This is a deep replacement. To only flatten the initial enumeration, use Flatten() instead.
        /// </summary>
        /// <param name="enumeration">the target enumeration of enumerations</param>
        /// <returns>an flattened enumeration. Returns an empty enumeration if <paramref name="enumeration"/> is null.</returns>
        public static IEnumerable FullyFlatten(this IEnumerable enumeration)
        {
            if (enumeration == null)
                yield break;
            Stack<IEnumerator> enumerationStack = new Stack<IEnumerator>();
            enumerationStack.Push(enumeration.GetEnumerator());
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

        /// <summary>
        /// Replaces all elements of type <typeparamref name="TIn"/> with generated values of type <typeparamref name="TOut"/>.
        /// This removes all replaced elements from the resultant enumeration and maintains the size of the enumeration.
        /// To keep the elements in, use Inject instead.
        /// </summary>
        /// <typeparam name="TIn">the target type to replace elements of</typeparam>
        /// <typeparam name="TOut">the type of object to replace with</typeparam>
        /// <param name="enumeration">the enumeration to replace the elements of</param>
        /// <param name="replacement">a mapping function that maps elements to be replace to their replacement</param>
        /// <param name="filter">a selection function. Returns true if a element is to be replaced. If null, all elements that can be replaced will be.</param>
        /// <returns>the enumeration with replaced elements</returns>
        public static IEnumerable Replace<TIn, TOut>(this IEnumerable enumeration,
                                                     Func<TIn, TOut> replacement,
                                                     Predicate<TIn> filter = null)
        {
            if (replacement == null)
                throw new ArgumentNullException("replacement");
            if (enumeration == null)
                yield break;
            foreach (object data in enumeration)
            {
                if (data is TIn && (filter == null || filter((TIn)data)))
                    yield return replacement((TIn)data);
                else
                    yield return data;
            }
        }

        /// <summary>
        /// Injects a new elements of type <typeparamref name="TOut"/> before or after elements of type <typeparamref name="TIn"/>.
        /// This keeps all matched elements in the enumeration. This also expands the 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="coroutine"></param>
        /// <param name="injection"></param>
        /// <param name="after"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable Inject<TIn, TOut>(this IEnumerable coroutine,
                                                    Func<TIn, TOut> injection,
                                                    Func<TIn, bool> after = null,
                                                    Func<TIn, bool> filter = null) where TIn : class
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
                                               Func<FireData, bool> filter = null) {
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

        public static IEnumerable Burst(this IEnumerable coroutine,
                                        Func<FireData, int> count,
                                        Action<FireData> delta = null,
                                        Action<FireData, int> setup = null,
                                        Func<FireData, bool> filter = null)
        {
            if (count == null)
                throw new ArgumentNullException("count");
            if(coroutine == null)
                yield break;

            FireData copy = new FireData();
            foreach (var obj in coroutine) {
                var fd = obj as FireData;
                if (fd == null || (filter != null && filter(fd))) {
                    yield return obj;
                    continue;
                }
                copy.Copy(fd);
                int currentCount = count(fd);
                if (setup != null)
                    setup(fd, currentCount);
                for (var i = 0; i < currentCount; i++) {
                    yield return copy;
                    delta.SafeInvoke(copy);
                }
            }
        }

        public static Task Execute(this MonoBehaviour behaviour, IEnumerable coroutine)
        {
            return coroutine.Execute(behaviour);
        }

        public static Task Execute(this IEnumerable data, MonoBehaviour context = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var fd = data as FireData;
            var dp = data as DanmakuPrefab;
            if (fd != null)
                fd.Fire();
            else if (dp != null)
                ((FireData) dp).Fire();
            else {
                var fireTask = new Task(context ?? Game.Instance,
                                         FireRoutine(data));
                fireTask.Start();
                return fireTask;
            }
            return null;
        }

        /// The actual coroutine that is executed in Fire() calls
        private static IEnumerator FireRoutine(IEnumerable dataSource) {
            foreach (object data in dataSource.FullyFlatten())
            {
                var yi = data as YieldInstruction;
                var fd = data as FireData;
                if (data == null || yi != null)
                    yield return data;
                else if (fd != null)
                    fd.Fire();
                else if (data is int) {
                    int frames = (int) data;
                    if (frames <= 0)
                        yield return null;
                    else
                        for (var i = 0; i < frames; i++)
                            yield return null;
                } else if (data is float) {
                    float seconds = (float) data;
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

        private class FireManipulator : IEnumerable
        {
            private readonly IEnumerable coroutine;
            public readonly Func<FireData, bool> filter;
            public event Action<FireData> actions;
            private IEnumerator enumerator;

            public FireManipulator(IEnumerable source, Action<FireData> action, Func<FireData, bool> filter)
            {
                coroutine = source;
                actions = action;
                this.filter = filter;
                enumerator = filter == null ? Unfiltered() : Filtered();
            }

            public IEnumerator GetEnumerator()
            {
                return enumerator;
            }

            IEnumerator Filtered()
            {
                if (coroutine == null)
                    yield break;

                var copy = new FireData();
                foreach (object data in coroutine)
                {
                    var fd = data as FireData;
                    if (fd == null || (filter != null && !filter(fd)))
                    {
                        yield return data;
                        continue;
                    }
                    copy.Copy(fd);
                    actions(copy);
                    yield return copy;
                }
            }

            IEnumerator Unfiltered()
            {
                if (coroutine == null)
                    yield break;

                var copy = new FireData();
                foreach (object data in coroutine)
                {
                    var fd = data as FireData;
                    if (fd == null)
                    {
                        yield return data;
                        continue;
                    }
                    copy.Copy(fd);
                    actions(copy);
                    yield return copy;
                }
            }

        }

        public static IEnumerable ForEachFireData(this IEnumerable coroutine, 
                                                    Action<FireData> action,
                                                    Func<FireData, bool> filter = null)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            var fm = coroutine as FireManipulator;
            if(fm == null || fm.filter != filter)
                return new FireManipulator(coroutine, action, filter);
            fm.actions += action;
            return fm;
        }
    }
}
