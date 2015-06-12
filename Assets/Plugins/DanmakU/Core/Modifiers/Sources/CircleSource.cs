// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

    public class CircleSource : DanmakuSource {

        [SerializeField, Show]
        private DInt count;

        [SerializeField, Show]
        private bool normal;

        [SerializeField, Show]
        private DFloat radius;

        public CircleSource(DFloat radius,
                            DInt count,
                            bool normal = false) {
            this.radius = radius;
            this.count = count;
            this.normal = normal;
        }

        public DInt Count {
            get { return count; }
            set { count = value; }
        }

        public DFloat Radius {
            get { return radius; }
            set { radius = value; }
        }

        public bool Normal {
            get { return normal; }
            set { normal = value; }
        }

        #region implemented abstract members of ProjectileSource

        protected override void UpdateSourcePoints(Vector2 position,
                                                   float rotation) {
            SourcePoints.Clear();
            float delta = Util.TwoPI/Count;
            for (int i = 0; i < Count; i++) {
                float currentRotation = Mathf.Deg2Rad*rotation + i*delta;
                SourcePoint sourcePoint =
                    new SourcePoint(
                        position +
                        Radius*Util.OnUnitCircleRadians(currentRotation),
                        ((Normal)
                             ? Mathf.Rad2Deg*currentRotation - 90f
                             : rotation));
                SourcePoints.Add(sourcePoint);
            }
        }

        #endregion
    }

}