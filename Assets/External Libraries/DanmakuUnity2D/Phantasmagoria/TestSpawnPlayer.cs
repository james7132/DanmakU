using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {

	[RequireComponent(typeof(PhantasmagoriaGameController))]
	public class TestSpawnPlayer : TestScript {
		public PhantasmagoriaPlayableCharacter character1;
		public PhantasmagoriaPlayableCharacter character2;

		void Start() {
			PhantasmagoriaGameController controller = GetComponent<PhantasmagoriaGameController> ();
			PhantasmagoriaPlayableCharacter player1 = (PhantasmagoriaPlayableCharacter) controller.player1.Field.SpawnPlayer (character1);
			PhantasmagoriaPlayableCharacter player2 = (PhantasmagoriaPlayableCharacter) controller.player2.Field.SpawnPlayer (character2);
			player1.Initialize(new PhantasmagoriaControlledAgent(1));
			player2.Initialize(new PhantasmagoriaControlledAgent(2));
		}
	}
}