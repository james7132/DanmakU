using UnityEngine;
using UnityEngine.Networking;

namespace Hourai {

    public class HouraiNetworkBehaviour : NetworkBehaviour {
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