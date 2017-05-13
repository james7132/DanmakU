using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

	public struct Danmaku {
        public Vector2 Position;
        public float Rotation;
        public float Speed;
        public float AngularVelocity;

        public void Move(float deltaTime) {
            Rotation += AngularVelocity * deltaTime;
            Position += Speed * MathUtils.GetDirection(Rotation) * deltaTime;
        }

        public static implicit operator Danmaku(DanmakuInitialState state) {
            return new Danmaku {
                Position = state.Position,
                Rotation = state.Rotation.GetValue(),
                Speed = state.Rotation.GetValue(),
                AngularVelocity = state.AngularVelocity.GetValue()
            };
        }

	}

    public struct DanmakuInitialState {
        public Vector2 Position;
        public Range Rotation;
        public Range Speed;
        public Range AngularVelocity;
	}

}