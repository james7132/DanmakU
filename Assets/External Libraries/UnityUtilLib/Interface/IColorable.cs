using UnityEngine;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// An interface for any colorable object
	/// </summary>
	public interface IColorable {

		/// <summary>
		/// Gets or sets the color of the object.
		/// </summary>
		/// <value>The color of the object.</value>
		Color Color { get; set; }
	}
}

