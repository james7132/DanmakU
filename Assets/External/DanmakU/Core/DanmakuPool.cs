// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {
	
	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Danmaku : IPooledObject, IColorable, IPrefabed<DanmakuPrefab> {

		internal class DanmakuPool : IPool<Danmaku> {
		
			internal Danmaku[] all;

			private int size;
			
			internal int totalCount;
			internal int spawnCount;
			internal int activeCount;
			
			public DanmakuPool(int initial, int spawn) {
				this.spawnCount = spawn;
				totalCount = 0;
				activeCount = 0;
				size = initial;
				all = new Danmaku[Mathf.NextPowerOfTwo(initial + 1)];
			}

			private void Spawn(int count) {
				int endCount = totalCount + spawnCount;
				if(all.Length < endCount) {
					size = all.Length;
					while (size <= endCount) {
						size *= 2;
					}
					
					Danmaku[] temp = new Danmaku[size];
					Array.Copy(all, temp, all.Length);
					all = temp;
				}
				for(int i = totalCount; i < endCount; i++) {
					Danmaku newDanmaku = new Danmaku();
					newDanmaku = new Danmaku();
					newDanmaku.index = i;
					newDanmaku.Pool = this;
					all[i] = newDanmaku;
				}
				totalCount = endCount;
			}
			
			public void Get(Danmaku[] danmaku) {
				if (danmaku == null)
					throw new ArgumentNullException ("Projectiles can't be null");
				int count = danmaku.Length;
				if(count + activeCount > totalCount)
					Spawn (count);
				Array.Copy(all, activeCount + 1, danmaku, 0, count);
				activeCount += count;
			}
			
			public void Return(Danmaku[] danmaku) {
				if(danmaku == null)
					throw new ArgumentNullException ("Projectiles can't be null");
				int count = danmaku.Length;
				for(int i = 0; i < count; i++) {
					Return(danmaku[i]);
				}
			}
			
			#region IPool implementation
			
			public Danmaku Get () {
				activeCount++;
				if (activeCount > totalCount) {
					Spawn(spawnCount);
				}
				return all [activeCount];
			}
			
			public void Return (Danmaku obj) {
				int deadIndex = obj.index;
				Danmaku temp = all [activeCount];
				all [activeCount] = obj;
				all [deadIndex] = temp;
				obj.index = activeCount;
				temp.index = deadIndex;
				activeCount--;
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

