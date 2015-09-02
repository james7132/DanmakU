using UnityEngine;
using System.Collections;
using Hourai;

namespace Hourai {

    public class GradientNumberText : NumberText {

        [SerializeField]
        private Gradient _gradient;

        [SerializeField]
        private float _start;

        [SerializeField]
        private float _end;

        protected override void Update() {
            base.Update();

            if (_start > _end) {
                float temp = _start;
                _start = _end;
                _end = temp;
            }

            float point = _start == _end ? 0f : Mathf.Clamp01((Number - _start)/(_end - _start));

            Text.color = _gradient.Evaluate(point);
        }

    }

}

