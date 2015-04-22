// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

namespace UnityUtilLib.Pooling {
	public interface IPooledObject {
		IPool Pool { get; set; }
		bool IsActive { get; }
		void Activate();
		void Deactivate();
	}
}

