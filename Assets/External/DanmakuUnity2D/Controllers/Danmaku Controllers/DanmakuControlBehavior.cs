using System;
using UnityEngine;

namespace Danmaku2D {

	public abstract class DanmakuControlBehavior : MonoBehaviour, IDanmakuController, IDanmakuNode {
		
		public DanmakuGroup DanmakuGroup {
			get;
			set;
		}

		public virtual DanmakuController Controller {
			get {
				return UpdateDanmaku;
			}
		}
		
		public virtual void Awake() {
			DanmakuGroup = new DanmakuGroup ();
			DanmakuGroup.AddController(this);
		}

		#region IDanmakuController implementation

		public abstract void UpdateDanmaku (Danmaku danmaku, float dt);

		#endregion

		#region IDanmakuNode implementation

		public bool Connect (IDanmakuNode node) {
			return false;
		}

		public virtual string NodeName {
			get {
				return GetType().Name;
			}
		}

		public Color NodeColor {
			get {
				return Color.blue;
			}
		}

		#endregion
	}
}

