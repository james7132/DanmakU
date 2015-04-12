using UnityEngine;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D.DanmakuControllers {
	
	/// <summary>
	/// A DanmakuController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class AccelerationController : IDanmakuController {

		[SerializeField]
		private float acceleration;

		public AccelerationController() {
			this.acceleration = 0f;
		}

		public AccelerationController (float acceleration) : base() {
			this.acceleration = acceleration;
		}
		
		#region IDanmakuController implementation
		
		public virtual void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (acceleration != 0) {
				danmaku.Speed += acceleration * dt;
			}
		}
		
		#endregion
	}

	namespace Wrapper {
		
		[AddComponentMenu("Danmaku 2D/Controllers/Acceleration Controller")]
		public class AccelerationController : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.AccelerationController> {
		}
		
	}

}

