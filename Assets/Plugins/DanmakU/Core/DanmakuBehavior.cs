// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU {

    public abstract class DanmakuBehaviour : BetterBehaviour {

		static DanmakuBehaviour() {
			WaitForEndOfFrame = new UnityEngine.WaitForEndOfFrame();
		}

		protected static readonly YieldInstruction WaitForEndOfFrame;

		protected static readonly YieldInstruction WaitForNextFrame = null;

		private IEnumerator waitForFrames(int frameCount) {
			int current = 0;
			while(current < frameCount) {
				if(enabled)
					current++;
				yield return null;
			}
		}

		private IEnumerator waitForSeconds(float seconds) {
			float current = 0;
			while(current < seconds) {
				if(enabled)
					current += Dt;
				yield return null;
			}
		}

		protected YieldInstruction WaitForFrames(int frameCount) {
			if(frameCount < 0)
				return null;
			return StartCoroutine(waitForFrames(frameCount));
		}

		protected YieldInstruction WaitForSeconds(float seconds) {
			if(seconds < Dt)
				return null;
			return StartCoroutine(waitForSeconds(seconds));
		}

        /// <summary>
        /// Shorthand for <c>TimeUtil.DeltaTime</c>.
        /// </summary>
        protected static float Dt {
            get { return TimeUtil.DeltaTime; }
        }

        protected static float Cos(float degree) {
            return Danmaku.Cos(degree);
        }

        protected static float Sin(float degree) {
            return Danmaku.Sin(degree);
        }

        protected static float Tan(float degree) {
            return Danmaku.Tan(degree);
        }

        protected static Vector2 UnitCircle(float degree) {
            return Danmaku.UnitCircle(degree);
        }

        protected Task StartTask(IEnumerable task) {
            if (task == null)
                throw new System.ArgumentNullException("task");
            return new Task(this, task);
        }

        protected Task StartTask(IEnumerator task) {
            if (task == null)
                throw new System.ArgumentNullException("task");
            return new Task(this, task);
        }
    }

}