// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    [System.Serializable]
    public class CircularBurstModifier : DanmakuModifier {

        [SerializeField, Show]
        private DInt count = 1;

        [SerializeField, Show]
        private DFloat deltaAngularSpeed = 0f;

        [SerializeField, Show]
        private DFloat deltaSpeed = 0f;

        [SerializeField, Show]
        private DFloat range = 360f;

        public CircularBurstModifier(DFloat range,
                                     DInt count,
                                     DFloat deltaSpeed,
                                     DFloat deltaAngularSpeed) {
            this.range = range;
            this.count = count;
            this.deltaSpeed = deltaSpeed;
            this.deltaAngularSpeed = deltaAngularSpeed;
        }

        public DFloat Range {
            get { return range; }
            set { range = value; }
        }

        public DInt Count {
            get { return count; }
            set { count = value; }
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
            int burstCount = Mathf.Abs(count.Value);

            if (burstCount == 1)
                FireSingle(position, rotation);
            else {
                float burstRange = range.Value;
                float start = rotation - burstRange*0.5f;
                float delta = burstRange/(burstCount - 1);

                float deltaV = deltaSpeed.Value;
                float deltaAV = deltaAngularSpeed.Value;

                DFloat tempSpeed = Speed;
                DFloat tempASpeed = AngularSpeed;

                for (int i = 0; i < burstCount; i++) {
                    Speed += deltaV;
                    AngularSpeed += deltaAV;
                    FireSingle(position, start + i*delta);
                }

                Speed = tempSpeed;
                AngularSpeed = tempASpeed;
            }
        }

        #endregion
    }

}