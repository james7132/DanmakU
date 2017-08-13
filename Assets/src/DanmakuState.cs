using System;
using UnityEngine;

namespace DanmakU {

    [Serializable]
    public struct DanmakuState {
        public Vector2 Position;
        public Range Rotation;
        public Range Speed;
        public Range AngularVelocity;
	}

}