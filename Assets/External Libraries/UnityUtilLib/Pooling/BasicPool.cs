using System;

namespace UnityUtilLib {
	public class BasicPool<T> : Pool<T> where T : IPooledObject, new() {

		public BasicPool(int initial, int spawn) : base(initial, spawn) {
		}

		#region implemented abstract members of Pool
		protected override T CreateNew () {
			return new T ();
		}
		#endregion
	}
}

