using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

    [Serializable]
    public class Arc : ShapeFireable {

        [SerializeField]
        Range _count;
        [SerializeField]
        Range _arcLength;
        [SerializeField]
        Range _radius;

        public Range Count { 
            get { return _count;} 
            set { _count = value; } 
        }

        public Range ArcLength { 
            get { return _arcLength;} 
            set { _arcLength = value; } 
        }

        public Range Radius { 
            get { return _radius;} 
            set { _radius = value; } 
        }

        public Arc(Range count, Range arcLength, Range radius) {
            Count = count;
            ArcLength = arcLength;
            Radius = radius;
        }

        protected override IEnumerable<DanmakuInitialState> GetSubemissions(DanmakuInitialState state) {
            float radius = Radius.GetValue();
            int count = Mathf.RoundToInt(Count.GetValue());
            float arcLength = ArcLength.GetValue();
            var rotation = state.Rotation.GetValue();
            var start = rotation - arcLength / 2;
            for (int i = 0; i < count; i++) {
                var currentState = state;
                var angle = start + i * (arcLength / count);
                state.Position = state.Position + 
                    (radius * MathUtils.GetDirection(angle));
                state.Rotation = angle;
                yield return currentState;
            }
        }
    }

}