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