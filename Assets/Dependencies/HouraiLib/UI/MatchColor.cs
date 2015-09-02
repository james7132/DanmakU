using UnityEngine;
using UnityEngine.UI;

namespace Hourai {

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class MatchColor : MonoBehaviour {

        [SerializeField]
        private Graphic target;

        private Graphic _self;

        // Update is called once per frame
        void Update() {
            if (target == null)
                return;

            if (_self == null)
                _self = GetComponent<Graphic>();

            _self.color = target.color;
        }
    }

}
