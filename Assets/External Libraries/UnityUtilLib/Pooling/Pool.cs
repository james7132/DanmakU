using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilLib.Pooling {
	public abstract class Pool<T> : IPool<T>, IEnumerable<T> where T : IPooledObject {
		private Queue<T> inactiveObjs;
		private HashSet<T> activeObjs;
		private HashSet<T> all;

		private int spawnCount;
		private int totalCount = 0;
		private int inactiveCount = 0;

		private T[] activeArray;
		private T[] inactiveArray;
		private T[] allArray;

		public T[] Active {
			get {
				if(activeArray == null || activeArray.Length < ActiveCount)
					activeArray = new T[Mathf.NextPowerOfTwo(ActiveCount)];
				activeObjs.CopyTo(activeArray);
				return activeArray;
			}
		}
		
		public T[] Inactive {
			get {
				if(inactiveArray == null || inactiveArray.Length < inactiveCount)
					inactiveArray = new T[Mathf.NextPowerOfTwo(inactiveCount)];
				activeObjs.CopyTo(activeArray);
				return activeArray;
			}
		}
		
		public T[] All {
			get {
				if(allArray == null || allArray.Length < totalCount)
					allArray = new T[Mathf.NextPowerOfTwo(totalCount)];
				all.CopyTo(allArray);
				return allArray;
			}
		}

		public int ActiveCount {
			get {
				return totalCount - inactiveCount;
			}
		}

		private int InactiveCount {
			get {
				return inactiveCount;
			}
		}

		public int TotalCount {
			get {
				return totalCount;
			}
		}

		public Pool(int initial, int spawn) {
			spawnCount = spawn;
			inactiveObjs = new Queue<T> ();
			activeObjs = new HashSet<T> ();
			all = new HashSet<T> ();
			Spawn (initial);
		}

		public void Return(T po) {
			inactiveObjs.Enqueue (po);
			activeObjs.Remove (po);
			inactiveCount++;
			//Debug.Log(activeCount);
		}

		public T Get() {
			if(inactiveCount <= 0) {
				Spawn (spawnCount);
			}
			T po = inactiveObjs.Dequeue();
			inactiveCount--;
			activeObjs.Add (po);
			//Debug.Log(active);
			return po;
		}

		protected void Spawn(int count) {
			for(int i = 0; i < count; i++) {
				T newPO = CreateNew();
				newPO.Pool = this;
				inactiveObjs.Enqueue(newPO);
				all.Add(newPO);
			}
			totalCount += count;
			inactiveCount += count;
		}

		protected abstract T CreateNew ();

		#region IPool implementation
		object IPool.Get () {
			throw new System.NotImplementedException ();
		}
		void IPool.Return (object obj) {
			Return ((T)obj);
		}
		#endregion

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator () {
			return activeObjs.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator () {
			return Active.GetEnumerator ();
		}

		#endregion
	}
}