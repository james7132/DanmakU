using UnityEngine;
using System;
using System.Collections;

namespace Danmaku2D {
	[Serializable]
	public abstract class PlayerAgent {

		private DanmakuPlayer player;
		public DanmakuPlayer Player {
			get {
				return player;
			}
			set {
				player = value;
				field = player.Field;
			}
		}
		
		private DanmakuField field;
		public DanmakuField Field {
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
		public abstract void Update();
	}
}