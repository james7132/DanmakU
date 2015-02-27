using UnityEngine;
using UnityUtilLib;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaField : AbstractDanmakuField {

		private AbstractDanmakuField targetField;
		public override AbstractDanmakuField TargetField {
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
			Vector2 relativePos = FieldPoint (Transform.position);
			projectile.Transform.position = targetField.WorldPoint (relativePos);
		}

		public void SetTargetField(AbstractDanmakuField field) {
			targetField = field;
		}

		public void RoundReset() {
			Player.Reset (5);
		}
	}
}