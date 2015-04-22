// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System;
using System.Collections;

namespace DanmakU {
	[Serializable]
	public abstract class PlayerAgent : IDanmakuObject {

		public DanmakuPlayer Player {
			get;
			set;
		}

		#region IDanmakuObject implementation
		public DanmakuField Field {
			get {
				return Player.Field;
			}
			set {
				Player.Field = value;
			}
		}
		#endregion	

		/// <summary>
		/// Update the specified dt.
		/// </summary>
		/// <param name="dt">Dt.</param>
		public abstract void Update();
	}
}