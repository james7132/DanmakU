// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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

