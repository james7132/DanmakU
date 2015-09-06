using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace Hourai.Editor
{
    internal class HandleDisposable : IDisposable
    {

        private readonly Color? _oldColor;
        private readonly Matrix4x4? _oldTransform;

        public HandleDisposable()
        {
        }

        public HandleDisposable(Color color) {
            //Cache the old color
            _oldColor = Handles.color;
            Handles.color = color;
        }

        public HandleDisposable(Matrix4x4 matrix) {
            //Cache the old matrix
            _oldTransform = Handles.matrix;
            Handles.matrix = matrix;
        }

        public HandleDisposable(Color color, Matrix4x4 matrix)
        {
            //Cache the old color
            _oldColor = Handles.color;
            Handles.color = color;

            //Cache the old matrix
            _oldTransform = Handles.matrix;
            Handles.matrix = matrix;
        }

        public void Dispose() {
            if (_oldColor != null)
                Handles.color = (Color)_oldColor;
            if (_oldTransform != null)
                Handles.matrix = (Matrix4x4)_oldTransform;
        }
    }


    public static class HandleUtil
    {
        public static IDisposable Save()
        {
            return new HandleDisposable();
        }

        public static IDisposable With(Color color)
        {
            return new HandleDisposable(color);
        }

        public static IDisposable With(Matrix4x4 transform)
        {
            return new HandleDisposable(transform);
        }

        public static IDisposable With(Color color, Matrix4x4 transform)
        {
            return new HandleDisposable(color, transform);
        }

        public static IDisposable With(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return new HandleDisposable(transform.localToWorldMatrix);
        }

        public static IDisposable With(Color color, Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return new HandleDisposable(color, transform.localToWorldMatrix);
        }
    }

}
