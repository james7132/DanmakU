// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    [System.Serializable]
    public class RandomizeVelocityModifier : DanmakuModifier {

        [SerializeField, Show]
        private DFloat range;

        public RandomizeVelocityModifier(DFloat range) {
            this.range = range;
        }

        public DFloat Range {
            get { return range; }
            set { range = value; }
        }

        #region implemented abstract members of FireModifier

        public override void OnFire(Vector2 position, DFloat rotation) {
            DFloat oldVelocity = Speed;
            float rangeValue = Range.Value;
            Speed = oldVelocity + Random.Range(-0.5f*rangeValue, 0.5f*rangeValue);
            FireSingle(position, rotation);
            Speed = oldVelocity;
        }

        #endregion
    }

}