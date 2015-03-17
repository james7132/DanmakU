using UnityEngine;
using System.Collections;
using UnityUtilLib.GUI;

namespace Danmaku2D.Phantasmagoria.GUI {
	public class PlayerScoreIndicator : MultiObjectValueIndicator {

		private PhantasmagoriaGameController gameControl;
		
		void Awake() {
			gameControl = (PhantasmagoriaGameController)GameController;
		}

		protected override int GetMaxValue () {
			return gameControl.WinningScore;
		}

		protected override int GetValue() {
			return ((player) ? gameControl.player1 : gameControl.player2).score;
		}
	}
}