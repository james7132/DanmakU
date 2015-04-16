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
using UnityUtilLib;

namespace Danmaku2D.AttackPatterns {
	
	public class ObjectAttackPattern : TimedAttackPattern {

		public GameObject prefab;
		public Vector2 position;
		public Vector2 range = new Vector2(0.5f, 0.25f);
		private GameObject Runtime;

		protected override void OnInitialize () {
			base.OnInitialize ();
			if(prefab == null)
				throw new MissingReferenceException(GetType().ToString() + " needs a prefab to function.");
			Vector2 spawnPos = position - range + 2 * range.Random ();
			Runtime = Field.SpawnGameObject(prefab, spawnPos);
		}

		protected override void OnFinalize () {
			base.OnFinalize ();
			if (Runtime != null)
				Destroy (Runtime);
		}
	}

}