// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    [System.Serializable]
    public class LinearBurstModifier : DanmakuModifier {

        [SerializeField, Show]
        private DFloat deltaAngularSpeed;

        [SerializeField, Show]
        private DFloat deltaSpeed;

        [SerializeField, Show]
        private DInt depth = 1;

        public LinearBurstModifier(DInt depth,
                                   DFloat deltaSpeed,
                                   DFloat deltaAngularSpeed) {
            this.depth = depth;
            this.deltaSpeed = deltaSpeed;
            this.deltaAngularSpeed = deltaAngularSpeed;
        }

        public DInt Depth {
            get { return depth; }
            set { depth = value; }
        }

        public DFloat DeltaSpeed {
            get { return deltaSpeed; }
            set { deltaSpeed = value; }
        }

        public DFloat DeltaAngularSpeed {
            get { return deltaAngularSpeed; }
            set { deltaAngularSpeed = value; }
        }

        #region implemented abstract members of FireModifier

        public override void OnFire(Vector2 position, DFloat rotation) {
            DFloat tempSpeed = Speed;
            DFloat tempASpeed = AngularSpeed;
            DFloat deltaV = DeltaSpeed;
            DFloat deltaAV = DeltaAngularSpeed;
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