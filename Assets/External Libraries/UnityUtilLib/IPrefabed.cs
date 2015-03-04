using System;

namespace UnityUtilLib {
	public interface IPrefabed<T> {
		T Prefab { get; }
		void MatchPrefab(T prefab);
	}
}

