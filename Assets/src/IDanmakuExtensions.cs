using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace DanmakU {

    internal static class IDanmakuExtentions {

        internal static void ApplyState(this IDanmaku danmaku, DanmakuState state) {
            Assert.IsNotNull(danmaku);
            danmaku.Position = state.Position;
            danmaku.Rotation = state.Rotation;
            danmaku.Speed = state.Speed;
            danmaku.AngularVelocity= state.AngularVelocity;
        }

        internal static void UpdateDanmaku(this DanmakuPool danmakuSet, float deltaTime) {
            if (danmakuSet == null)
                return;
            int count = 0;
            foreach (var danmaku in danmakuSet.GetActive()) {
                count++;
                danmaku.Rotation += danmaku.AngularVelocity * deltaTime;
                var velocity = MathUtils.GetDirection(danmaku.Rotation) * danmaku.Speed;
                danmaku.Position += velocity;
            }
        }

    }


}