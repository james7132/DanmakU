using UnityEngine;
using System;
using System.Collections;

namespace Danmaku2D {
	[Serializable]
	public abstract class AbstractPlayerAgent {

		private AbstractPlayableCharacter player;
		public AbstractPlayableCharacter Player {
			get {
				return player;
			}
			set {
				player = value;
				field = player.Field;
			}
		}
		
		private AbstractDanmakuField field;
		public AbstractDanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
				player = field.Player;
			}
		}

		/// <summary>
		/// Update the specified dt.
		/// </summary>
		/// <param name="dt">Dt.</param>
		public abstract void Update(float dt);
	}
}