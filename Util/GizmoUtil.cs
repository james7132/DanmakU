using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hourai {

    public sealed class GizmoDisposable : IDisposable {

        private readonly Color? _oldColor;
        private readonly Matrix4x4? _oldTransform;

        public GizmoDisposable(Color color) {
            //Cache the old color
            _oldColor = Gizmos.color;
            Gizmos.color = color;
        }

        public GizmoDisposable(Matrix4x4 matrix) {
            //Cache the old matrix
            _oldTransform = Gizmos.matrix;
            Gizmos.matrix = matrix;
        }

        public GizmoDisposable(Color color, Matrix4x4 matrix) {
            //Cache the old color
            _oldColor = Gizmos.color;
            Gizmos.color = color;

            //Cache the old matrix
            _oldTransform = Gizmos.matrix;
            Gizmos.matrix = matrix;
        }

        public void Dispose() {
            if (_oldColor != null)
                Gizmos.color = (Color) _oldColor;
            if (_oldTransform != null)
                Gizmos.matrix = (Matrix4x4) _oldTransform;
        }

    }


    public static class GizmoUtil {

        public static IDisposable With(Color color) {
            return new GizmoDisposable(color);
        }

        public static IDisposable With(Matrix4x4 transform) {
            return new GizmoDisposable(transform);
        }

        public static IDisposable With(Color color, Matrix4x4 transform) {
            return new GizmoDisposable(color, transform);
        }

        public static IDisposable With(Transform transform) {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return new GizmoDisposable(transform.localToWorldMatrix);
        }

        public static IDisposable With(Color color, Transform transform) {
            if (transform == null)
                throw new ArgumentNullException("transform");
            return new GizmoDisposable(color, transform.localToWorldMatrix);
        }

        public static void DrawCollider3D(Collider collider, Color color, bool solid = false) {
            if (collider == null)
                return;
            using (With(color, collider.transform))
                DrawCollider3D_Impl(collider, solid);
        }

        public static void DrawColliders3D(IEnumerable<Collider> colliders,
                                           Color color,
                                           bool solid = false,
                                           Predicate<Collider> filter = null) {
            if (colliders == null)
                return;

            using (With(color, Matrix4x4.identity)) {
                Collider[] asArray = colliders as Collider[];
                if (asArray != null) {
                    foreach (Collider collider in asArray) {
                        if (collider == null || (filter != null && !filter(collider)))
                            continue;
                        Gizmos.matrix = collider.transform.localToWorldMatrix;
                        DrawCollider3D_Impl(collider, solid);
                    }
                } else {
                    foreach (Collider collider in colliders) {
                        if (collider == null || (filter != null && !filter(collider)))
                            continue;
                        Gizmos.matrix = collider.transform.localToWorldMatrix;
                        DrawCollider3D_Impl(collider, solid);
                    }
                }
            }
        }

        private static void DrawCollider3D_Impl(Collider collider, bool solid) {
            var boxCollider = collider as BoxCollider;
            var sphereCollider = collider as SphereCollider;
            var meshCollider = collider as MeshCollider;
            if (solid) {
                if (boxCollider != null)
                    Gizmos.DrawCube(boxCollider.center, boxCollider.size);
                else if (sphereCollider != null)
                    Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
                else if (meshCollider != null)
                    Gizmos.DrawMesh(meshCollider.sharedMesh, Vector3.zero);
            } else {
                if (boxCollider != null)
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                else if (sphereCollider != null)
                    Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
                else if (meshCollider != null)
                    Gizmos.DrawWireMesh(meshCollider.sharedMesh, Vector3.zero);
            }
        }

    }

}