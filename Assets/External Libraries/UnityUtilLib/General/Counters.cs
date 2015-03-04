using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	[System.Serializable]
	public struct CountdownDelay {

		[SerializeField]
		private float maxDelay;
		public float MaxDelay {
			get {
				return maxDelay;
			}
			set {
				maxDelay = value;
			}
		}	

		[SerializeField]
		private float currentDelay;
		public float CurrentDelay {
			get {
				return currentDelay;
			}
			set {
				currentDelay = value;
			}
		}

		public CountdownDelay(float maxDelay) {
			this.maxDelay = maxDelay;
			currentDelay = maxDelay;
		}

		public bool Tick(float dt, bool reset = true, float value = -1) {
			currentDelay -= dt;
			bool ready = currentDelay <= 0f;
			if(ready && reset)
				currentDelay = (value > 0) ? value : maxDelay;
			return ready;
		}

		public bool Ready() {
			return currentDelay <= 0f;
		}

		public void ForceReady() {
			currentDelay = 0f;
		}

		public void Reset() {
			currentDelay = maxDelay;
		}
	}

	[System.Serializable]
	public struct Counter {

		[SerializeField]
		private int maxCount;
		public int MaxCount {
			get {
				return maxCount;
			}
			set {
				maxCount = value;
			}
		}

		[SerializeField]
		private int count;
		public int Count {
			get {
				return count;
			}
			set {
				count = value;
			}
		}

		public Counter(int maxCount) {
			this.maxCount = maxCount;
			count = maxCount;
		}

		public bool Ready() {
			return count <= 0;
		}

		public bool Tick(bool reset = true) {
			count--;
			bool ready = count < 0;
			if(ready && reset)
				count = maxCount;
			return ready;
		}

		public void Reset() {
			count = maxCount;
		}

		public void ForceReady() {
			count = 0;
		}
	}

	[System.Serializable]
	public class FrameCounter {
		[SerializeField]
		private float delay;
		private float Time {
			get {
				return delay;
			}
			set {
				delay = value;
			}
		}

		private int maxCount;
		public int MaxCount {
			get {
				return maxCount;
			}
		}

		private int count;
		public int Count {
			get {
				return count;
			}
		}

		private bool init = false;
		
		public FrameCounter(float maxDelay) {
			this.delay = maxDelay;
			maxCount = Util.TimeToFrames (delay);
			count = maxCount;
		}

		public void Init() {
			maxCount = Util.TimeToFrames (delay);
			count = maxCount;
			init = true;
		}
		
		public bool Ready() {
			if(!init)
				Init ();
			return count <= 0;
		}
		
		public bool Tick(bool reset = true) {
			if(!init)
				Init ();
			count--;
			bool ready = count <= 0;
			if(ready && reset)
				Reset();
			return ready;
		}
		
		public void Reset() {
			if(!init)
				Init ();
			maxCount = Util.TimeToFrames (delay);
			count = maxCount;
		}

		public void ForceReady() {
			count = 0;
		}
	}
}
