// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace DanmakU {

	/// <summary>
	/// An interface for a pausable object.
	/// </summary>
	public interface IPausable {

		/// <summary>
		/// Whether this object <see cref="UnityUtilLib.IPausable"/> is paused.
		/// </summary>
		/// <value><c>true</c> if paused; otherwise, <c>false</c>.</value>
		bool Paused { get; set; }

	}

}