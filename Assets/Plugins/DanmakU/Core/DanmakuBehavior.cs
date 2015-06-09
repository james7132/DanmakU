// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using Vexe.Runtime.Types;

namespace DanmakU
{
    public abstract class DanmakuBehaviour : BetterBehaviour
    {
        protected static float Cos(float degree)
        {
            return Danmaku.Cos(degree);
        }

        protected static float Sin(float degree)
        {
            return Danmaku.Sin(degree);
        }

        protected static float Tan(float degree)
        {
            return Danmaku.Tan(degree);
        }

        protected static Vector2 UnitCircle(float degree)
        {
            return Danmaku.UnitCircle(degree);
        }

        /// <summary>
        /// Shorthand for <c>TimeUtil.DeltaTime</c>.
        /// </summary>
        protected static float Dt
        {
            get { return TimeUtil.DeltaTime; }
        }

        protected Task StartTask(IEnumerable task)
        {
            if (task == null)
                throw new System.ArgumentNullException("task");
            return new Task(this, task);
        }

        protected Task StartTask(IEnumerator task)
        {
            if (task == null)
                throw new System.ArgumentNullException("task");
            return new Task(this, task);
        }
    }
}