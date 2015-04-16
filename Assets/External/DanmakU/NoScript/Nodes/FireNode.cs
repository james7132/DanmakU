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

	public class FireNode : DanmakuNode {

		[SerializeField]
		private Transform target;

		protected override void Process() {
			if (target != null) {
				Target.Position = target.position;
				Target.Rotation = target.eulerAngles.z;
				DanmakuField.FindClosest (Target.Position).Fire (Target);
			} else {
				Debug.LogError("Tried to fire from a null transform");
			}
		}

	}

}
