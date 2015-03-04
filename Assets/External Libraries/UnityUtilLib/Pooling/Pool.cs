using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilLib {
	public abstract class Pool<T> : IPool<T> where T : IPooledObject {
		private Queue<T> inactiveObjs;
		private HashSet<T> activeObjs;
		private HashSet<T> all;

		private int spawnCount;

		public T[] Active {
			get {
				T[] array = new T[activeObjs.Count];
				activeObjs.CopyTo(array);
				return array;
			}
		}

		public T[] Inactive {
			get {
				return inactiveObjs.ToArray();
			}
		}

		public T[] All {
			get {
				T[] array = new T[all.Count];
				all.CopyTo(array);
				return array;
			}
		}

		public int ActiveCount {
			get {
				#pragma warning disable 0168
				try {
					return totalCount - inactiveObjs.Count;
				} catch (System.NullReferenceException nre) {
					return totalCount;
				}
				#pragma warning restore 0168
			}
		}

		private int InactiveCount {
			get {
				return inactiveObjs.Count;
			}
		}
		
		private int totalCount = 0;
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
			//Debug.Log(activeCount);
		}

		public T Get() {
			if(InactiveCount <= 0) {
				Spawn (spawnCount);
			}
			T po = inactiveObjs.Dequeue();
			activeObjs.Add (po);
			OnGet (po);
			//Debug.Log(active);
			return po;
		}

		protected void Spawn(int count) {
			for(int i = 0; i < count; i++) {
				T newPO = CreateNew();
				newPO.Pool = this;
				inactiveObjs.Enqueue(newPO);
				all.Add(newPO);
				totalCount++;
			}
		}

		protected abstract T CreateNew ();
		protected virtual void OnGet(T obj) {
		}


		#region IPool implementation
		object IPool.Get () {
			throw new System.NotImplementedException ();
		}
		void IPool.Return (object obj) {
			Return ((T)obj);
		}
		#endregion
	}
}