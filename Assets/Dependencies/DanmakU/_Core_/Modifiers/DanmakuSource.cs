// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vexe.Runtime.Types;
using Hourai;

namespace Hourai.DanmakU {

    public struct Pose {

        public Vector2 Position;
        public float Rotation;

    }

    public static class DanmakuSource {
        
        public static IEnumerable Circle(this IEnumerable data, 
                                         Func<FireData, int> count, 
                                         Func<FireData, float> radius,
                                         bool radialFire = true,
                                         Func<FireData, bool> filter = null) {

            Func<FireData, IEnumerable<Pose>> circleFunc =
                delegate(FireData fd) {
                    int currentCount = count(fd);

                    if (currentCount <= 0)
                        return new Pose[0];

                    float currentRadius = radius(fd);
                    float delta = 360f / currentCount;
                    Pose[] locations = new Pose[currentCount];

                    for (var i = 0; i < currentCount; i++) {
                        float currentRotation = Mathf.Deg2Rad * fd.Rotation + i * delta;
                        locations[i].Position = fd.Position + currentRadius * Util.OnUnitCircle(currentRotation);
                        if(radialFire)
                            locations[i].Rotation = Mathf.Rad2Deg * currentRotation - 90f;
                    }

                    return locations;
                };

            Action<FireData, Pose> setPose = delegate(FireData fd, Pose p) {
                                                 fd.Position = p.Position;
                                                 fd.Rotation = p.Rotation;
                                             };

            return data.Duplicate(circleFunc, setPose, false, filter);
        }

        public static IEnumerable Circle(this IEnumerable data, 
                                        int count,
                                        float radius, 
                                        bool radialFire = true, 
                                        Func<FireData, bool> filter = null) {
            return data.Circle(fireData => count, fireData => radius, radialFire, filter);
        }

    }

}