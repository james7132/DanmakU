using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	public abstract class AbstractPrefabedPoolBehavior<T, P> : AbstractPoolBehavior<T>, IPrefabedPool<T, P> where T : IPrefabPooledObject<P> {
		#region IPrefabedPool implementation

		public T Get (P prefab) {
			T val = Get ();
			val.MatchPrefab (prefab);
			return val;
		}

		#endregion
	}
}
