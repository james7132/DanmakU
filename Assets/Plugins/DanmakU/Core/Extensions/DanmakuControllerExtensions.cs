// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DanmakU {

    public static class DanmakuControllerExtensions {

        #region IDanmakuController Enumerable Extensions

        /// <summary>
        /// Creates a single multicast DanmakuController delegate from a collection of IDanmakuControllers.
        /// </summary>
        /// <remarks>Any and all <c>null</c> values within the collection will be ignored.</remarks>
        /// <exception cref="NullReferenceException">thrown if the controllers collection is null.</exception>
        /// <param name="controllers">the collection of controllers to compress.</param>
        public static DanmakuController Compress(
            this IEnumerable<IDanmakuController> controllers) {
            if (controllers == null)
                throw new NullReferenceException();

            DanmakuController controller = null;
            var list = controllers as IDanmakuController[];
            if (list != null) {
                foreach (var current in list) {
                    if (current != null)
                        controller += current.Update;
                }
            } else {
                foreach (var current in controllers) {
                    if (current != null)
                        controller += current.Update;
                }
            }
            return controller;
        }

        #endregion

        #region DanmakuController Enumerable Functions

        /// <summary>
        /// Creates a single multicast DanmakuController delegate from a collection of DanmakuControllers.
        /// </summary>
        /// <remarks>Any and all <c>null</c> values within the collection will be ignored.</remarks>
        /// <exception cref="NullReferenceException">thrown if the controllers collection is null.</exception>
        /// <param name="controllers">the collection of controllers to compress.</param>
        public static DanmakuController Compress(
            this IEnumerable<DanmakuController> controllers) {
            if (controllers == null)
                throw new NullReferenceException();

            DanmakuController controller = null;
            var list = controllers as IList<DanmakuController>;
            if (list != null) {
                int count = list.Count;
                for (int i = 0; i < count; i++) {
                    DanmakuController current = list[i];
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

        #region DanmakuController

        public static DanmakuController[] Decompose(this DanmakuController controller) {
            if (controller == null)
                return new DanmakuController[] {};
            Delegate[] elements = controller.GetInvocationList();
            DanmakuController[] controllerElements = new DanmakuController[elements.Length];
            for (var i = 0; i < elements.Length; i++)
                controllerElements[i] = elements[i] as DanmakuController;
            return controllerElements;
        }

        public static DanmakuController Remove(this DanmakuController source, DanmakuController toRemove) {
            Delegate[] elements = toRemove.GetInvocationList();
            foreach (var element in elements)
                source = Delegate.Remove(source, element) as DanmakuController;
            return source;
        }

        public static DanmakuController RemoveAll(this DanmakuController source, DanmakuController toRemove) {
            Delegate[] elements = toRemove.GetInvocationList();
            foreach (var element in elements)
                source = Delegate.Remove(source, element) as DanmakuController;
            return source;
        }


        public static DanmakuController Remove(this DanmakuController source, IDanmakuController toRemove) {
            if (toRemove == null)
                return source;
            return source - toRemove.Update;
        }

        public static DanmakuController RemoveAll(this DanmakuController source, IDanmakuController toRemove) {
            if (toRemove == null)
                return source;
            return source.RemoveAll(toRemove.Update);
        }

        #endregion
    }

}