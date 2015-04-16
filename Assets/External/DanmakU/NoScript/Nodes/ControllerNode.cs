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

using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public sealed class ControllerNode : DanmakuNode {

		public IDanmakuController Controller {
			get;
			set;
		}

		#region implemented abstract members of DanmakuNode
		protected override void Process () {
			if (Controller != null)
				Target.Controller += Controller.UpdateDanmaku;
			else
				Debug.LogError ("Controller Node: Attempted to add null controller");
		}
		#endregion
	}

}
