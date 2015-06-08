// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;

namespace DanmakU {

	[System.Serializable]
	public struct CountdownDelay {

		//Made it a public variable to remove the computation time needed to access/edit it externally
		public float MaxDelay;
		public float CurrentDelay;

		public CountdownDelay(float maxDelay) {
			MaxDelay = maxDelay;
			CurrentDelay = maxDelay;
		}

		public bool Tick(float dt) {
			CurrentDelay -= dt;
			bool ready = CurrentDelay <= 0f;
			if(ready)
				CurrentDelay = MaxDelay;
			return ready;
		}

		public bool Ready() {
			return CurrentDelay <= 0f;
		}

		public void ForceReady() {
			CurrentDelay = 0f;
		}

		public void Reset() {
			CurrentDelay = MaxDelay;
		}
	}

	[System.Serializable]
	public struct Counter {
		
		//Made it a public variable to remove the computation time needed to access/edit it externally
		public int MaxCount;

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
			MaxCount = maxCount;
			Count = maxCount;
		}

		public bool Ready() {
			return Count <= 0;
		}

		public bool Tick(bool reset = true) {
			Count--;
			bool ready = Count < 0;
			if (ready && reset) {
				Count = MaxCount;
			}
			return ready;
		}

		public void Reset() {
			Count = MaxCount;
		}

		public void ForceReady() {
			Count = 0;
		}
		
		public static implicit operator int(Counter counter) {
			return counter.MaxCount;
		}
	}

	[System.Serializable]
	public class FrameCounter {

		public float Time;

		private int maxCount;
		public int MaxCount {
			get {
				if(!init)
					Init ();
				return maxCount;
			}
		}

		private int count;
		public int Count {
			get {
				if(!init)
					Init ();
				return count;
			}
		}

		private bool init = false;
		
		public FrameCounter(float maxDelay) {
			this.Time = maxDelay;
			maxCount = TimeUtil.TimeToFrames (Time);
			count = maxCount;
		}

		public void Init() {
			maxCount = TimeUtil.TimeToFrames (Time);
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
			maxCount = TimeUtil.TimeToFrames (Time);
			count = maxCount;
		}

		public void ForceReady() {
			count = 0;
		}

		public static implicit operator int(FrameCounter counter) {
			return counter.maxCount;
		}
	}
}
