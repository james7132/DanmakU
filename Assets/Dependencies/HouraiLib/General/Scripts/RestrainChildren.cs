using UnityEngine;

namespace Hourai {

    [RequireComponent(typeof (BoxCollider))]
    public class RestrainChildren : MonoBehaviour {

        private BoxCollider bounds;

        private void Awake() {
            bounds = GetComponent<BoxCollider>();
            bounds.enabled = false;
        }

        private void LateUpdate() {
            var boundedArea = new Bounds(bounds.center, bounds.size);
            foreach (Transform child in transform)
                child.localPosition = boundedArea.ClosestPoint(child.localPosition);
        }

    }

}