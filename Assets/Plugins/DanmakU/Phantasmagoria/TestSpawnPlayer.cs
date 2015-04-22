// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace DanmakU.Phantasmagoria {

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
			PhantasmagoriaPlayableCharacter player1 = (PhantasmagoriaPlayableCharacter) controller.player1.Field.SpawnPlayer (character1, Vector2.one * 0.5f);
			PhantasmagoriaPlayableCharacter player2 = (PhantasmagoriaPlayableCharacter) controller.player2.Field.SpawnPlayer (character2, Vector2.one * 0.5f);
			player1.Agent = new PhantasmagoriaControlledAgent(1);
			player2.Agent = new PhantasmagoriaControlledAgent(2);
		}
	}
}