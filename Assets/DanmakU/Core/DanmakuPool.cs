// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {
	
	class DanmakuPool {
		
		//FIXME: Currently not working: optimized version of pool, need to debug
		
		//			internal Danmaku[] all;
		//			
		//			internal int totalCount;
		//			internal int spawnCount;
		//			internal int activeIndex;
		//			
		//			public DanmakuPool(int initial, int spawn) {
		//				this.spawnCount = spawn;
		//				totalCount = 0;
		//				activeIndex = 0;
		//				all = new Danmaku[Mathf.NextPowerOfTwo(initial + 1)];
		//				Spawn(initial);
		//			}
		//			
		//			private void Spawn(int count) {
		//				int endCount = totalCount + count;
		//				if(all.Length < endCount) {
		//					//Debug.Log("extend");
		//					
		//					Danmaku[] temp = new Danmaku[Mathf.NextPowerOfTwo(endCount + 1)];
		//					Array.Copy(all, temp, all.Length);
		//					//					int test = 0;
		//					//					while(all[test] != null) {
		//					//						test++;
		//					//					}
		//					//					Debug.Log(test);
		//					all = temp;
		//				}
		//				for(int i = totalCount; i < endCount; i++) {
		//					Danmaku newDanmaku = new Danmaku();
		//					newDanmaku.poolIndex = i;
		//					newDanmaku.Pool = this;
		//					all[i] = newDanmaku;
		//				}
		//				totalCount = endCount;
		//			}
		//			
		//			public void Get(Danmaku[] danmaku) {
		//				if (danmaku == null)
		//					throw new ArgumentNullException ("Danmaku array can't be null");
		//				int count = danmaku.Length;
		//				if(count + activeIndex > totalCount)
		//					Spawn (count);
		//				Array.Copy(all, activeIndex + 1, danmaku, 0, count);
		//				activeIndex += count;
		//			}
		//			
		//			public void Return(Danmaku[] danmaku) {
		//				if(danmaku == null)
		//					throw new ArgumentNullException ("Danmaku array can't be null");
		//				int count = danmaku.Length;
		//				for(int i = 0; i < count; i++) {
		//					Return(danmaku[i]);
		//				}
		//			}
		//			
		//			#region IPool implementation
		//			
		//			public Danmaku Get () {
		//				activeIndex++;
		//				if (activeIndex >= totalCount) {
		//					Spawn(spawnCount);
		//				}
		//				//				if (all [activeCount] == null) {
		//				//					Debug.Log(activeCount);
		//				//				}
		//				return all [activeIndex];
		//			}
		//			
		//			public void Return (Danmaku obj) {
		//				int deadIndex = obj.poolIndex;;
		//				Danmaku temp = all [activeIndex];
		//				all [activeIndex] = obj;
		//				all [deadIndex] = temp;
		//				obj.poolIndex = activeIndex;
		//				temp.poolIndex = deadIndex;
		//				activeIndex--;
		//			}
		//			
		//			#endregion
		//			
		//			#region IPool implementation
		//			
		//			object IPool.Get () {
		//				return Get ();
		//			}
		//			
		//			public void Return (object obj) {
		//				Return (obj as Danmaku);
		//			}
		//			
		//			#endregion
		
		internal int[] queue;
		internal Danmaku[] all;
		
		private int currentIndex;
		private int endIndex;
		private int size;
		
		internal int totalCount;
		internal int inactiveCount;
		internal int spawnCount;
		
		public DanmakuPool(int initial, int spawn) {
			this.spawnCount = spawn;
			totalCount = 0;
			inactiveCount = 0;
			Spawn (initial);
		}
		
		protected void Spawn(int count) {
			if(all == null || queue == null) {
				all = new Danmaku[2];
				queue = new int[2];
			}
			int endCount = totalCount + count;
			if(all.Length < endCount) {
				size = all.Length;
				if (size <= endCount)
					size = Mathf.NextPowerOfTwo(endCount);
				
				Danmaku[] temp = new Danmaku[size];

				Array.Copy(all, temp, all.Length);
				all = temp;
				
				int[] tempQueue = new int[size];

				// if the queue's current index is less than the end index, the queue has not wrapped around
				// simply copy it as needed
				if(currentIndex < endIndex)
					Array.Copy(queue, currentIndex, tempQueue, 0, endIndex - currentIndex);
				else {
					// otherwise the queue has wrapped around and needs to be copied in two seperate chunks
					int initial = 0;
					initial = queue.Length - currentIndex - 1;
					Array.Copy(queue, currentIndex, tempQueue, 0, initial);
					Array.Copy(queue, 0, tempQueue, initial, endIndex);
				}
				currentIndex = 0;
				endIndex = inactiveCount;
				queue = tempQueue;
			}
			for(int i = totalCount; i < endCount; i++, endIndex++) {
				Danmaku danmaku = new Danmaku();
				danmaku = new Danmaku();
				danmaku.poolIndex = i;
				all[i] = danmaku;
				if(endIndex >= queue.Length)
					endIndex = 0;
				queue[endIndex] = i;
			}
			totalCount = endCount;
			inactiveCount += count;
		}
		
		public void Get(Danmaku[] danmakus) {
			if (danmakus == null)
				throw new ArgumentNullException ("Projectiles can't be null");
			int count = danmakus.Length;
			if (inactiveCount < count)
				Spawn (count - inactiveCount);
			inactiveCount -= count;
			for (int i = 0; i < danmakus.Length; i++) {
				danmakus[i] = all[queue[currentIndex]];
				currentIndex = (currentIndex + 1) % size;
			}
		}
		
		public void Return(Danmaku[] danmakus) {
			if(danmakus == null)
				throw new ArgumentNullException ("Projectiles can't be null");
			int count = danmakus.Length;
			inactiveCount += count;
			for(int i = 0; i < count; i++) {
				queue[endIndex] = danmakus[i].poolIndex;
				endIndex = (endIndex + 1) % size;
			}
		}
		
		public Danmaku Get () {
			if(inactiveCount <= 0) {
				Spawn (spawnCount);
			}
			inactiveCount--;
			int index = queue [currentIndex];
			currentIndex = (currentIndex + 1) % size;
			return all [index];
		}
		
		public void Return (Danmaku obj) {
			queue [endIndex] = obj.poolIndex;
			endIndex = (endIndex + 1) % size;
			inactiveCount++;
		}
	}
}

