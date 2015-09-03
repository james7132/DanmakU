// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hourai.DanmakU {

    public static class DanmakuControllerExtensions {

        #region Action<Danmaku> Enumerable Functions

        /// <summary>
        /// Creates a single multicast Action<Danmaku> delegate from a collection of DanmakuControllers.
        /// </summary>
        /// <remarks>Any and all <c>null</c> values within the collection will be ignored.</remarks>
        /// <exception cref="NullReferenceException">thrown if the controllers collection is null.</exception>
        /// <param name="controllers">the collection of controllers to compress.</param>
        public static Action<Danmaku> Compress(
            this IEnumerable<Action<Danmaku>> controllers) {
            if (controllers == null)
                throw new NullReferenceException();

            Action<Danmaku> controller = null;
            var list = controllers as IList<Action<Danmaku>>;
            if (list != null) {
                int count = list.Count;
                for (int i = 0; i < count; i++) {
                    Action<Danmaku> current = list[i];
                    if (current != null)
                        controller += current;
                }
            } else {
                foreach (var current in controllers) {
                    if (current != null)
                        controller += current;
                }
            }
            return controller;
        }

        #endregion

        #region Action<Danmaku>

        public static Action<Danmaku>[] Decompose(this Action<Danmaku> controller) {
            if (controller == null)
                return new Action<Danmaku>[] {};
            Delegate[] elements = controller.GetInvocationList();
            Action<Danmaku>[] controllerElements = new Action<Danmaku>[elements.Length];
            for (var i = 0; i < elements.Length; i++)
                controllerElements[i] = elements[i] as Action<Danmaku>;
            return controllerElements;
        }

        public static Action<Danmaku> Remove(this Action<Danmaku> source, Action<Danmaku> toRemove) {
            Delegate[] elements = toRemove.GetInvocationList();
            foreach (var element in elements)
                source = Delegate.Remove(source, element) as Action<Danmaku>;
            return source;
        }

        public static Action<Danmaku> RemoveAll(this Action<Danmaku> source, Action<Danmaku> toRemove) {
            Delegate[] elements = toRemove.GetInvocationList();
            foreach (var element in elements)
                source = Delegate.Remove(source, element) as Action<Danmaku>;
            return source;
        }

        #endregion

    }

}