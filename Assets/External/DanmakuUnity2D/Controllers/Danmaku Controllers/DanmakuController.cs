using UnityEngine;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	public delegate void DanmakuController(Danmaku proj, float dt);

	/// <summary>
	/// An interface for defining any controller of Danmaku behavior.
	/// </summary>
	public interface IDanmakuController {

		/// <summary>
		/// Updates the Danmaku controlled by the controller instance.
		/// </summary>
		/// <returns>the displacement from the Danmaku's original position after udpating</returns>
		/// <param name="dt">the change in time since the last update</param>
		void UpdateDanmaku (Danmaku danmaku, float dt);

	}
}