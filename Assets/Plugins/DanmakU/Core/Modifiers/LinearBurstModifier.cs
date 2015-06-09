// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    [System.Serializable]
    public class LinearBurstModifier : DanmakuModifier {

        [SerializeField, Show]
        private DynamicFloat deltaAngularSpeed;

        [SerializeField, Show]
        private DynamicFloat deltaSpeed;

        [SerializeField, Show]
        private DynamicInt depth = 1;

        public LinearBurstModifier(DynamicInt depth,
                                   DynamicFloat deltaSpeed,
                                   DynamicFloat deltaAngularSpeed) {
            this.depth = depth;
            this.deltaSpeed = deltaSpeed;
            this.deltaAngularSpeed = deltaAngularSpeed;
        }

        public DynamicInt Depth {
            get { return depth; }
            set { depth = value; }
        }

        public DynamicFloat DeltaSpeed {
            get { return deltaSpeed; }
            set { deltaSpeed = value; }
        }

        public DynamicFloat DeltaAngularSpeed {
            get { return deltaAngularSpeed; }
            set { deltaAngularSpeed = value; }
        }

        #region implemented abstract members of FireModifier

        public override void OnFire(Vector2 position, DynamicFloat rotation) {
            DynamicFloat tempSpeed = Speed;
            DynamicFloat tempASpeed = AngularSpeed;
            DynamicFloat deltaV = DeltaSpeed;
            DynamicFloat deltaAV = DeltaAngularSpeed;
            float depth = Depth.Value;
            for (int i = 0; i < depth; i++) {
                Speed += deltaV;
                AngularSpeed += deltaAV;
                FireSingle(position, rotation);
            }
            Speed = tempSpeed;
            AngularSpeed = tempASpeed;
        }

        #endregion
    }

}