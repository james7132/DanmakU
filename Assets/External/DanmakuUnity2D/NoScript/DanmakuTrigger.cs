using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {

	public abstract class DanmakuTriggerReciever : CachedObject, IDanmakuNode {

		[SerializeField]
		private List<DanmakuTrigger> triggers;

		public override void Awake () {
			base.Awake ();
			for(int i = 0; i < triggers.Count; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback += Trigger;
				}
			}
		}

		public void OnDestroy() {
			for(int i = 0; i < triggers.Count; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback -= Trigger;
				}
			}
		}

		public abstract void Trigger ();

		#region IDanmakuNode implementation

		public virtual bool Connect (IDanmakuNode node) {
			if (node is DanmakuTrigger) {
				triggers.Add(node as DanmakuTrigger);
				return true;
			}
			return false;
		}

		public virtual string NodeName {
			get {
				return GetType().Name;
			}
		}

		public abstract Color NodeColor {
			get;
		}

		#endregion
	}

	[AddComponentMenu("Danmaku 2D/Danmaku Trigger")]
	public class DanmakuTrigger : CachedObject, IDanmakuNode {

		public delegate void TriggerCallback ();
		
		internal TriggerCallback triggerCallback;
		
		public void Trigger() {
			if(triggerCallback != null)
				triggerCallback();
		}

		#region IDanmakuNode implementation

		public string NodeName {
			get {
				return GetType().Name;
			}
		}
		
		public Color NodeColor {
			get {
				return Color.white;
			}
		}

		public virtual bool Connect (IDanmakuNode node) {
			return false;
		}

		#endregion

	}

}
