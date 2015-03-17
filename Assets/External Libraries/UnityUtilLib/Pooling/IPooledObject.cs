
namespace UnityUtilLib.Pooling {
	public interface IPooledObject {
		IPool Pool { get; set; }
		bool IsActive { get; }
		void Activate();
		void Deactivate();
	}
}

