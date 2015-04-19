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
	public sealed partial class Danmaku : IPooledObject, IPrefabed<DanmakuPrefab> {

		internal class DanmakuPool : IPool<Danmaku> {
		
			internal Danmaku[] all;
			
			internal int totalCount;
			internal int spawnCount;
			internal int activeIndex;
			
			public DanmakuPool(int initial, int spawn) {
				this.spawnCount = spawn;
				totalCount = 0;
				activeIndex = 0;
				all = new Danmaku[Mathf.NextPowerOfTwo(initial + 1)];
				Spawn(initial);
			}

			private void Spawn(int count) {
				int endCount = totalCount + count;
				if(all.Length < endCount) {
					//Debug.Log("extend");
					
					Danmaku[] temp = new Danmaku[Mathf.NextPowerOfTwo(endCount + 1)];
					Array.Copy(all, temp, all.Length);
//					int test = 0;
//					while(all[test] != null) {
//						test++;
//					}
//					Debug.Log(test);
					all = temp;
				}
				for(int i = totalCount; i < endCount; i++) {
					Danmaku newDanmaku = new Danmaku();
					newDanmaku.poolIndex = i;
					newDanmaku.Pool = this;
					all[i] = newDanmaku;
				}
				totalCount = endCount;
			}
			
			public void Get(Danmaku[] danmaku) {
				if (danmaku == null)
					throw new ArgumentNullException ("Danmaku array can't be null");
				int count = danmaku.Length;
				if(count + activeIndex > totalCount)
					Spawn (count);
				Array.Copy(all, activeIndex + 1, danmaku, 0, count);
				activeIndex += count;
			}
			
			public void Return(Danmaku[] danmaku) {
				if(danmaku == null)
					throw new ArgumentNullException ("Danmaku array can't be null");
				int count = danmaku.Length;
				for(int i = 0; i < count; i++) {
					Return(danmaku[i]);
				}
			}
			
			#region IPool implementation
			
			public Danmaku Get () {
				activeIndex++;
				if (activeIndex >= totalCount) {
					Spawn(spawnCount);
				}
//				if (all [activeCount] == null) {
//					Debug.Log(activeCount);
//				}
				return all [activeIndex];
			}
			
			public void Return (Danmaku obj) {
				int deadIndex = obj.poolIndex;;
				Danmaku temp = all [activeIndex];
				all [activeIndex] = obj;
				all [deadIndex] = temp;
				obj.poolIndex = activeIndex;
				temp.poolIndex = deadIndex;
				activeIndex--;
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

