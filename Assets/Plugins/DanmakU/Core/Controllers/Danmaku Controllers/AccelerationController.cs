// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using Vexe.Runtime.Types;

/// <summary>
/// A variety of useful 
/// </summary>
namespace DanmakU.DanmakuControllers {

	/// <summary>
	/// A Danmaku Controller that makes Danmaku speed up or slow down over time.
	/// </summary>
	[System.Serializable]
	public class AccelerationController : IDanmakuController {

		[Show]
		[Serialize]
		public float Acceleration {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DanmakU.DanmakuControllers.AccelerationController"/> class.
		/// </summary>
		/// <param name="acceleration">Acceleration.</param>
		public AccelerationController (float acceleration = 0f) : base() {
			this.Acceleration = acceleration;
		}
		
		#region IDanmakuController implementation
		
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (Acceleration != 0) {
				danmaku.Speed += Acceleration * dt;
			}
		}
		
		#endregion
	}

}

