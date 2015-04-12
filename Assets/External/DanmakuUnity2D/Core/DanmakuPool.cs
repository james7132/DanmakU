using System;
using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Danmaku : IPooledObject, IColorable, IPrefabed<DanmakuPrefab> {

		internal class ProjectilePool : IPool<Danmaku> {
			
			internal int[] queue;
			internal Danmaku[] all;
			
			private int currentIndex;
			private int endIndex;
			private int size;
			
			internal int totalCount;
			internal int inactiveCount;
			internal int spawnCount;
			
			public ProjectilePool(int initial, int spawn) {
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
				int endCount = totalCount + spawnCount;
				if(all.Length < endCount) {
					size = all.Length;
					while (size <= endCount) {
						size = Mathf.NextPowerOfTwo(size + 1);
					}
					
					Danmaku[] temp = new Danmaku[size];
					Array.Copy(all, temp, all.Length);
					all = temp;
					
					int[] tempQueue = new int[size];
					int initial = 0;
					if(currentIndex < endIndex) {

						Array.Copy(queue, currentIndex, tempQueue, 0, endIndex - currentIndex);
					} else {
						initial = queue.Length - currentIndex - 1;
						Array.Copy(queue, currentIndex, tempQueue, 0, initial);
						Array.Copy(queue, 0, tempQueue, initial, endIndex);
					}
					currentIndex = 0;
					endIndex = inactiveCount;
					queue = tempQueue;
				}
				for(int i = totalCount; i < endCount; i++, endIndex++) {
					all[i] = new Danmaku();
					all[i].index = i;
					all[i].Pool = this;
					if(endIndex > queue.Length)
						endIndex = 0;
					queue[endIndex] = i;
				}
				totalCount = endCount;
				inactiveCount += spawnCount;
			}
			
			public void Get(Danmaku[] projectiles) {
				if (projectiles == null)
					throw new ArgumentNullException ("Projectiles can't be null");
				int count = projectiles.Length;
				while (inactiveCount < count)
					Spawn (spawnCount);
				inactiveCount -= count;
				for (int i = 0; i < projectiles.Length; i++) {
					projectiles[i] = all[queue[currentIndex]];
					currentIndex = (currentIndex + 1) % size;
				}
			}
			
			public void Return(Danmaku[] projectiles) {
				if(projectiles == null)
					throw new ArgumentNullException ("Projectiles can't be null");
				int count = projectiles.Length;
				inactiveCount += count;
				for(int i = 0; i < count; i++) {
					queue[endIndex] = projectiles[i].index;
					endIndex = (endIndex + 1) % size;
				}
			}
			
			#region IPool implementation
			
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
				queue [endIndex] = obj.index;
				endIndex = (endIndex + 1) % size;
				inactiveCount++;
			}
			
			#endregion
			
			#region IPool implementation
			
			object IPool.Get () {
				return Get ();
			}
			
			public void Return (object obj) {
				Return (obj as Danmaku);
			}
			
			#endregion
		}


	}
}

