using UnityEngine;
using UnityUtilLib;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaField : DanmakuField {

		private DanmakuField targetField;
		public override DanmakuField TargetField {
			get {
				return targetField;
			}
		}

		private int playerNumber = 1;
		public int PlayerNumber {
			get {
				return playerNumber; 
			}
			set { 
				playerNumber = value; 
			}
		}

		public void Transfer(Projectile projectile) {
			Vector2 relativePos = ViewPoint (Transform.position);
			projectile.Position = targetField.WorldPoint (relativePos);
		}

		public void SetTargetField(DanmakuField field) {
			targetField = field;
		}

		public void RoundReset() {
			Player.Reset (5);
			CameraTransform.rotation = Quaternion.identity;
		}
	}
}