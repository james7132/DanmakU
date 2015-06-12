// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    [System.Serializable]
    public class RandomizeAngleModifier : DanmakuModifier {

        [SerializeField, Show]
        private DFloat range;

        public RandomizeAngleModifier(DFloat range) {
            this.range = range;
        }

        public DFloat Range {
            get { return range; }
            set { range = value; }
        }

        #region implemented abstract members of FireModifier

        public override void OnFire(Vector2 position, DFloat rotation) {
            float rotationValue = rotation.Value;
            float rangeValue = Range.Value;
            FireSingle(position, rotationValue + 0.5f*Random.Range(-rangeValue, rangeValue));
        }

        #endregion
    }

}