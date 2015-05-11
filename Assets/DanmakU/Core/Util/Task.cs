// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public sealed class Task : IEnumerable {

		private MonoBehaviour context;
		private IEnumerator task;
		private IEnumerable taskE;
		internal bool started;
		private bool enumerable;
		private bool isFinished;
		private bool paused;

		#region IEnumerable implementation
		
		public IEnumerator GetEnumerator () {
			return Wrapper (context);
		}
		
		#endregion

		public Task(MonoBehaviour context, IEnumerator task) {
			if (context == null || task == null)
				throw new System.ArgumentNullException ();
			this.task = task;
			this.context = context;
			UtilCoroutines.UtilityBehaviour.latentTasks.Add (this);
			enumerable = false;
		}

		public Task(MonoBehaviour context, IEnumerable task) {
			if (context == null || task == null)
				throw new System.ArgumentNullException ();
			taskE = task;
			this.context = context;
			UtilCoroutines.UtilityBehaviour.latentTasks.Add (this);
			enumerable = true;
		}

		private IEnumerator Wrapper(MonoBehaviour currentContext) {
			while (!isFinished) {
				if (!paused) {
					isFinished = !task.MoveNext();
					object next = task.Current;
					if(next is YieldInstruction) {
//						Debug.Log("Yield: " + next);
						yield return next;
					} else if(next is int) {
//						Debug.Log("Wait: " + next);
						int frames = (int)next;
						if(frames < 0)
							frames = -frames;
						for(int i = 0; i < frames - 1; i++)
							yield return null;
					} else if(next is Task) {
//						Debug.Log("Subtask: " + next);
						yield return (next as Task).Start();
					} else {
//						Debug.Log("Other");
						yield return null;
					}
				}
				if(context != currentContext && context != null) {
					Debug.Log("hello");
					context.StartCoroutine(Wrapper (context));
					break;
				}
			}
		}

		internal YieldInstruction Start() {
			started = true;
			return context.StartCoroutine (Wrapper(context));
		}

		public void Restart() {
			if (enumerable) {
				task = taskE.GetEnumerator ();
				isFinished = false;
			} else {
				throw new System.InvalidOperationException("Cannot reset a Task that was not started from a IEnumerable block");
			}
		}

		public void ContextSwitch(MonoBehaviour newContext) {
			context = newContext;
		}

		public void Pause() {
			paused = true;
		}

		public void Resume() {
			paused = false;
		}

		public void Stop() {
			isFinished = true;
		}
	}

}
