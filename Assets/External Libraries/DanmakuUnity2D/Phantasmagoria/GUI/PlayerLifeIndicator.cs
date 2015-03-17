using UnityEngine;
using System.Collections;
using UnityUtilLib.GUI;
using Danmaku2D.Phantasmagoria;

namespace Danmaku2D.Phantasmagoria.GUI {

	public class PlayerLifeIndicator : MultiObjectValueIndicator {

		private PhantasmagoriaGameController gameControl;

		void Awake() {
			gameControl = (PhantasmagoriaGameController)GameController;
		}

		protected override int GetMaxValue () {
			return DanmakuGameController.MaximumLives;
		}

		protected override int GetValue () {
			return ((player) ? gameControl.player1 : gameControl.player2).Field.Player.LivesRemaining;
		}
	}
}