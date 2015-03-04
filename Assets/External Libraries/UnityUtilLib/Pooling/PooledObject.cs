using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityUtilLib {

	public abstract class PooledObject : IPooledObject {
		public IPool Pool {
			get;
			set;
		}
		
		private bool is_active = false;
		public bool IsActive {
			get { 
				return is_active; 
			}
		}

		public virtual void Activate() {
			is_active = true;
		}

		public virtual void Deactivate() {
			is_active = false;
			Pool.Return (this);
		}
	}
}