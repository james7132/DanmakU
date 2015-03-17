
/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A generic clonable interface that allows a strongly typed clone of a given implementor.
	/// This is used for deep copies only. Make sure any implementation of this performs a deep copy, not just a shallow copy.
	/// </summary>
    public interface IClonable<T> {
        T Clone();
    }
}
