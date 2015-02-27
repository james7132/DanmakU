using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityUtilLib {
	/// <summary>
	/// Pooled object.
	/// </summary>
	public abstract class AbstractPrefabedPooledObject<T> : IPrefabPooledObject<T> {
		private IPool pool;
		public IPool Pool {
			get {
				return pool;
			}
			set {
				pool = value;
			}
		}
		
		private T prefab;
		
		/// <summary>
		/// Gets or sets the prefab.
		/// </summary>
		/// <value>The prefab.</value>
		public T Prefab {
			get { 
				return prefab; 
			}
			set {
				prefab = value;
				MatchPrefab(prefab);
			}
		}
		
		private bool is_active = false;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="UnityUtilLib.PooledObject`1"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool IsActive {
			get { 
				return is_active; 
			}
		}

		/// <summary>
		/// Matchs the prefab.
		/// </summary>
		/// <param name="gameObj">Game object.</param>
		public abstract void MatchPrefab (T gameObj);
		
		/// <summary>
		/// Activate this instance.
		/// </summary>
		public virtual void Activate() {
			is_active = true;
		}
		
		/// <summary>
		/// Deactivate this instance.
		/// </summary>
		public virtual void Deactivate() {
			is_active = false;
			pool.Return (this);
		}
	}
}