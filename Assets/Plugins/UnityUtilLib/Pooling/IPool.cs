// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

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