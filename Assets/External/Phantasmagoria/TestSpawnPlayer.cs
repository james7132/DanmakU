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
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {

	[RequireComponent(typeof(PhantasmagoriaGameController))]
	public class TestSpawnPlayer : TestScript {
		public PhantasmagoriaPlayableCharacter character1;
		public PhantasmagoriaPlayableCharacter character2;

		void Start() {
//			if (FindObjectOfType<PlayerSpawnPass> ()) {
//				Destroy (this);
//				return;
//			}
			PhantasmagoriaGameController controller = GetComponent<PhantasmagoriaGameController> ();
			PhantasmagoriaPlayableCharacter player1 = (PhantasmagoriaPlayableCharacter) controller.player1.Field.SpawnPlayer (character1);
			PhantasmagoriaPlayableCharacter player2 = (PhantasmagoriaPlayableCharacter) controller.player2.Field.SpawnPlayer (character2);
			player1.Agent = new PhantasmagoriaControlledAgent(1);
			player2.Agent = new PhantasmagoriaControlledAgent(2);
		}
	}
}