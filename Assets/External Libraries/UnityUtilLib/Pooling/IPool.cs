using System;

namespace UnityUtilLib.Pooling {
	public interface IPool {
		object Get();
		void Return (object obj);
	}

	public interface IPool<T> : IPool where T : IPooledObject {
		new T Get();
		void Return(T obj);
	}
}