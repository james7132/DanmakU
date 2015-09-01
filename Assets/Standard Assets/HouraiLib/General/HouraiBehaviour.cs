using System;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai {

    /// <summary>
    /// A utility behaviour with a large number of more succinct code shortcuts to make for shorter
    /// </summary>
    public abstract class HouraiBehaviour : BetterBehaviour {

        #region Time Properties

        private float _localTimeScale = 1f;

        public virtual float LocalTimeScale {
            get { return _localTimeScale; }
            set {
                float oldValue = _localTimeScale;
                _localTimeScale = value;
                if(oldValue == 0f)
                   OnPause(false);
                else if(_localTimeScale == 0f)
                   OnPause(true);
            }
        }

        public float EffectiveTimeScale {
            get { return LocalTimeScale*Time.timeScale; }
        }

        public bool Paused {
            get { return EffectiveTimeScale == 0f; }
        }

        protected float DeltaTime {
            get { return Time.deltaTime * LocalTimeScale; }
        }

        protected float FixedDeltaTime {
            get { return Time.fixedDeltaTime * LocalTimeScale; }
        }

        protected virtual void OnPause(bool paused) {
        }

        #endregion

        #region GameObject Functions

        public bool CompareLayer(int layerMask) {
            return (layerMask & (1 << layer)) != 0;
        }

        #endregion

        #region Transform Properties

        /// <summary>
        /// Shorthand for <c>transform.position</c>.
        /// Gets or sets the position of the transform in world space.
        /// </summary>
        public Vector3 position {
            get { return transform.position; }
            set { transform.position = value; }
        }

        /// <summary>
        /// Shorthand for <c>transform.localPosition</c>.
        /// Gets or sets the position of the transform relative to the parent transform.
        /// </summary>
        public Vector3 localPosition {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }

        /// <summary>
        /// Shorthand for <c>transform.rotation</c>.
        /// Gets or sets the rotation of the transform in world space stored as a Quaternion.
        /// </summary>
        public Quaternion rotation {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        /// <summary>
        /// Shorthand for <c>transform.localRotation</c>.
        /// </summary>
        public Quaternion localRotation {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }

        public Vector3 localScale {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }

        public Vector3 lossyScale {
            get { return transform.lossyScale; }
        }

        public Vector3 eulerAngles {
            get { return transform.eulerAngles; }
            set { transform.eulerAngles = value; }
        }

        public Vector3 localEulerAngles {
            get { return transform.localEulerAngles; }
            set { transform.localEulerAngles = value; }
        }

        public Vector3 up {
            get { return transform.up; }
            set { transform.up = value; }
        }

        public Vector3 forward {
            get { return transform.forward; }
            set { transform.forward = value; }
        }

        public Vector3 right {
            get { return transform.right; }
            set { transform.right = value; }
        }

        public Vector3 down {
            get { return -transform.up; }
            set { transform.up = -value; }
        }

        public Vector3 back {
            get { return -transform.forward; }
            set { transform.forward = -value; }
        }

        public Vector3 left {
            get { return -transform.right; }
            set { transform.right = -value; }
        }

        public Transform parentTransform {
            get { return transform.parent; }
            set { transform.parent = value; }
        }

        #endregion

        #region Transform Functions

        public void Rotate(float x, float y, float z) {
            transform.Rotate(x, y, z);
        }

        public void Rotate(Vector3 eulerRotation) {
            transform.Rotate(eulerRotation);
        }

        #endregion

        #region GameObject Properties

        public int layer {
            get { return gameObject.layer; }
            set { gameObject.layer = value; }
        }

        public GameObject parentObject {
            get { return transform.parent ? transform.parent.gameObject : null; }
            set { transform.parent = value ? null : value.transform; }
        }

        #endregion
    }

}