using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilLib {
	public abstract class AbstractPoolBehavior<T> : CachedObject, IPool<T> where T : IPooledObject {
		private Queue<T> inactiveObjs;
		private HashSet<T> activeObjs;
		private HashSet<T> all;

		/// <summary>
		/// The initial spawn count.
		/// </summary>
		[SerializeField]
		private int initialSpawnCount;
		
		/// <summary>
		/// The spawn count.
		/// </summary>
		[SerializeField]
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

		public override void Awake () {
			base.Awake ();
			inactiveObjs = new Queue<T> ();
			activeObjs = new HashSet<T> ();
			all = new HashSet<T> ();
			Spawn (initialSpawnCount);
		}

		/// <summary>
		/// Return the specified po.
		/// </summary>
		/// <param name="po">Po.</param>
		public void Return(T po) {
			inactiveObjs.Enqueue (po);
			activeObjs.Remove (po);
			//Debug.Log(activeCount);
		}
		
		/// <summary>
		/// Get the specified prefab.
		/// </summary>
		/// <param name="prefab">Prefab.</param>
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
		
		/// <summary>
		/// Spawn the specified count.
		/// </summary>
		/// <param name="count">Count.</param>
		protected void Spawn(int count) {
			for(int i = 0; i < count; i++) {
				T newPO = CreateNew();
				newPO.Pool = this;
				inactiveObjs.Enqueue(newPO);
				all.Add(newPO);
				OnSpawn(newPO);
				totalCount++;
			}
		}

		protected abstract T CreateNew ();
		protected virtual void OnGet(T obj) {
		}
		protected virtual void OnSpawn(T obj) {
		}

		#region IPool implementation
	
		object IPool.Get () {
			return Get ();
		}

		/// <summary>
		/// Return the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		void IPool.Return (object obj) {
			Return ((T)obj);
		}
		#endregion
	}
}