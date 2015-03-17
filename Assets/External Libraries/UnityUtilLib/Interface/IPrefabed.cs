
/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// An interface to define objects that can match a given prefab type.
	/// </summary>
	public interface IPrefabed<T> {

		/// <summary>
		/// Makes the object match the given prefab.
		/// </summary>
		/// <param name="prefab">the prefab to match.</param>
		void MatchPrefab(T prefab);
	}
}

