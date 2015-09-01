// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("DanmakU/Danmaku Field")]
    public class DanmakuField : MonoBehaviour, IFireBindable {

        public enum CoordinateSystem {

            View,
            ViewRelative,
            Relative,
            World

        }

        private static List<DanmakuField> _fields;

        private static readonly Vector2 InfiniteSize = float.PositiveInfinity*
                                                       Vector2.one;

        private DanmakuList activeDanmaku;

        internal Bounds2D bounds;

        [SerializeField]
        private Camera camera2D;

        private Transform camera2DTransform;

        [SerializeField]
        private float clipBoundary = 1f;

        [SerializeField]
        private Vector2 fieldSize = new Vector2(20f, 20f);

        private Bounds2D movementBounds;

        [SerializeField]
        private List<Camera> otherCameras;

        [SerializeField]
        private bool useClipBoundary = true;

        public bool UseClipBoundary {
            get { return useClipBoundary; }
            set { useClipBoundary = value; }
        }

        public float ClipBoundary {
            get { return clipBoundary; }
            set { clipBoundary = value; }
        }

        public Vector2 FieldSize {
            get { return fieldSize; }
            set { fieldSize = value; }
        }

        public DanmakuField TargetField { get; set; }

        public Camera Camera2D {
            get { return camera2D; }
            set { camera2D = value; }
        }

        public List<Camera> OtherCameras {
            get { return otherCameras; }
        }

        public float Camera2DRotation {
            get {
                if (camera2D == null)
                    return float.NaN;
                if (camera2DTransform == null)
                    camera2DTransform = camera2D.transform;
                return camera2DTransform.eulerAngles.z;
            }
            set { camera2DTransform.rotation = Quaternion.Euler(0f, 0f, value); }
        }

        public Bounds2D Bounds {
            get { return bounds; }
        }

        public Bounds2D MovementBounds {
            get { return movementBounds; }
        }

        /// <summary>
        /// Finds the closest DanmakuField to a GameObject's position.
        /// </summary>
        /// <returns>The closest DanmakuField to the GameObject.</returns>
        /// <param name="gameObject">the GameObject used to find the closest DanmakuField.</param>
        public static DanmakuField FindClosest(GameObject gameObject) {
            return FindClosest(gameObject.transform.position);
        }

        /// <summary>
        /// Finds the closest DanmakuField to a Component's GameObject's position.
        /// </summary>
        /// <returns>The closest DanmakuField to the Component and it's GameObject.</returns>
        /// <param name="component">the Component used to find the closest DanmakuField.</param>
        public static DanmakuField FindClosest(Component component) {
            return FindClosest(component.transform.position);
        }

        /// <summary>
        /// Finds the closest DanmakuField to the point specified by a Transform's position.
        /// </summary>
        /// <returns>The closest DanmakuField to the Transform's position.</returns>
        /// <param name="transform">the Transform used to find the closest DanmakuField.</param>
        public static DanmakuField FindClosest(Transform transform) {
            return FindClosest(transform.position);
        }

        public static DanmakuField FindClosest(Vector2 position) {
            if (_fields == null) {
                _fields = new List<DanmakuField>();
                _fields.AddRange(FindObjectsOfType<DanmakuField>());
            }
            if (_fields.Count == 0) {
                DanmakuField encompassing =
                    new GameObject("Danmaku Field").AddComponent<DanmakuField>();
                encompassing.useClipBoundary = false;
                _fields.Add(encompassing);
            }
            if (_fields.Count == 1)
                return _fields[0];
            DanmakuField closest = null;
            float minDist = float.MaxValue;
            foreach (var field in _fields) {
                Vector2 diff = field.bounds.Center - position;
                float distance = diff.sqrMagnitude;
                if (distance >= minDist)
                    continue;
                closest = field;
                minDist = distance;
            }
            return closest;
        }

        public virtual void Awake() {
            if (_fields == null)
                _fields = new List<DanmakuField>();
            _fields.Add(this);
            activeDanmaku = new DanmakuList();
            TargetField = this;
        }

        public virtual void Update() {
            if (camera2D != null) {
                camera2DTransform = camera2D.transform;
                camera2D.orthographic = true;
                movementBounds.Center =
                    bounds.Center = camera2DTransform.position;
                float size = camera2D.orthographicSize;
                movementBounds.Extents = new Vector2(camera2D.aspect*size, size);
                foreach (Camera t in otherCameras) {
                    if (t != null)
                        t.rect = camera2D.rect;
                }
            } else {
                camera2DTransform = null;
                movementBounds.Center = bounds.Center = transform.position;
                movementBounds.Extents = fieldSize*0.5f;
            }
            if (useClipBoundary) {
                bounds.Extents = movementBounds.Extents +
                                 Vector2.one*clipBoundary*
                                 movementBounds.Extents.Max();
            } else
                bounds.Extents = InfiniteSize;
            activeDanmaku.RemoveAll(d => bounds.Contains(d.Position));
        }

        private void OnDestroy() {
            if (_fields != null)
                _fields.Remove(this);
        }

        public Vector2 WorldPoint(Vector2 point,
                                  CoordinateSystem coordSys = CoordinateSystem.View) {
            switch (coordSys) {
                case CoordinateSystem.World:
                    return point;
                case CoordinateSystem.Relative:
                    return movementBounds.Min + point;
                case CoordinateSystem.ViewRelative:
                    return point.Hadamard2(movementBounds.Size);
                default:
                    return movementBounds.Min +
                           point.Hadamard2(movementBounds.Size);
            }
        }

        public Vector2 RelativePoint(Vector2 point,
                                     CoordinateSystem coordSys = CoordinateSystem.View) {
            switch (coordSys) {
                case CoordinateSystem.World:
                    return point - movementBounds.Min;
                case CoordinateSystem.Relative:
                    return point;
                default:
                case CoordinateSystem.View:
                    Vector2 size = movementBounds.Size;
                    var inverse = new Vector2(1/size.x, 1/size.y);
                    return (point - movementBounds.Min).Hadamard2(inverse);
            }
        }

        public Vector2 ViewPoint(Vector2 point,
                                 CoordinateSystem coordSys = CoordinateSystem.World) {
            Vector2 size = bounds.Size;
            switch (coordSys) {
                default:
                case CoordinateSystem.World:
                    Vector2 offset = point - movementBounds.Min;
                    return new Vector2(offset.x/size.x, offset.y/size.y);
                case CoordinateSystem.Relative:
                    return new Vector2(point.x/size.x, point.y/size.y);
                case CoordinateSystem.View:
                    return point;
            }
        }

        public GameObject SpawnGameObject(GameObject prefab,
                                          Vector2 location,
                                          CoordinateSystem coordSys = CoordinateSystem.View) {
            if (TargetField == null)
                TargetField = this;
            Quaternion rotation = prefab.transform.rotation;
            return
                Instantiate(gameObject,
                            TargetField.WorldPoint(location, coordSys),
                            rotation) as GameObject;
        }

        public T SpawnObject<T>(T prefab,
                                Vector2 location,
                                CoordinateSystem coordSys = CoordinateSystem.View)
            where T : Component {
            if (TargetField == null)
                TargetField = this;
            Quaternion rotation = prefab.transform.rotation;
            T clone = Instantiate(prefab,
                                  TargetField.WorldPoint(location, coordSys),
                                  rotation) as T;
            return clone;
        }

        public Danmaku SpawnDanmaku(DanmakuPrefab prefab,
                                    Vector2 location,
                                    DFloat rotation,
                                    CoordinateSystem coordSys = CoordinateSystem.View) {
            if (TargetField == null)
                TargetField = this;
            Danmaku danmaku = Danmaku.GetInactive(prefab,
                                                 TargetField.WorldPoint(location, coordSys),
                                                 rotation);
            danmaku.Activate();
            activeDanmaku.Add(danmaku);
            return danmaku;
        }

        public Danmaku FireLinear(DanmakuPrefab prefab,
                                  Vector2 location,
                                  DFloat rotation,
                                  DFloat speed,
                                  CoordinateSystem coordSys = CoordinateSystem.View) {
            if (TargetField == null)
                TargetField = this;
            Vector2 position = TargetField.WorldPoint(location, coordSys);
            Danmaku danmaku = Danmaku.GetInactive(prefab, position, rotation);
            danmaku.Activate();
            danmaku.Speed = speed;
            danmaku.AngularSpeed = 0f;
            activeDanmaku.Add(danmaku);
            return danmaku;
        }

        public Danmaku FireCurved(DanmakuPrefab prefab,
                                  Vector2 location,
                                  DFloat rotation,
                                  DFloat speed,
                                  DFloat angularSpeed,
                                  CoordinateSystem coordSys = CoordinateSystem.View) {
            if (TargetField == null)
                TargetField = this;
            Vector2 position = TargetField.WorldPoint(location, coordSys);
            Danmaku danmaku = Danmaku.GetInactive(prefab, position, rotation);
            danmaku.Activate();
            danmaku.Speed = speed;
            danmaku.AngularSpeed = angularSpeed;
            activeDanmaku.Add(danmaku);
            return danmaku;
        }

        public Danmaku Fire(FireData data) {
            if (TargetField == null)
                TargetField = this;
            Danmaku danmaku = data.Fire();
            activeDanmaku.Add(danmaku);
            return danmaku;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.Center, bounds.Size);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(movementBounds.Center, movementBounds.Size);
        }
#endif

        public void Bind(FireData fireData)
        {
            activeDanmaku.Bind(fireData);
        }

        public void Unbind(FireData fireData)
        {
            activeDanmaku.Unbind(fireData);
        }
    }

}