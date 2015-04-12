using UnityEngine;
using System;
using System.Collections;

namespace Danmaku2D {
	[Serializable]
	public abstract class PlayerAgent {

		public DanmakuPlayer Player {
			get;
			set;
		}

		public DanmakuField Field {
			get {
				return Player.Field;
			}
			set {
				Player.Field = value;
			}
		}

		/// <summary>
		/// Update the specified dt.
		/// </summary>
		/// <param name="dt">Dt.</param>
		public abstract void Update();
	}
}