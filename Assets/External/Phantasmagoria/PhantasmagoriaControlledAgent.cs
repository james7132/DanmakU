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

namespace DanmakU.Phantasmagoria {
	public class PhantasmagoriaControlledAgent : PlayerAgent  {
		private string horizontalMoveAxis;
		private string verticalMoveAxis;
		private string fireButton;
		private string focusButton;
		private string chargeButton;

		public PhantasmagoriaControlledAgent(int playerNumber) {
			string strPlay = "Player ";
			horizontalMoveAxis = "Horizontal Movement " + strPlay + playerNumber;
			verticalMoveAxis = "Vertical Movement " + strPlay + playerNumber;
			focusButton = "Focus " + strPlay + playerNumber;
			fireButton = "Fire " + strPlay + playerNumber;
			chargeButton = "Charge " + strPlay + playerNumber;
		}

		/// <summary>
		/// Update the specified dt.
		/// </summary>
		/// <param name="dt">Dt.</param>
		public override void Update () {
			PhantasmagoriaPlayableCharacter player = (PhantasmagoriaPlayableCharacter)Player;
			Vector2 movementVector = Vector2.zero;
			movementVector.x = Util.Sign(Input.GetAxis (horizontalMoveAxis));
			movementVector.y = Util.Sign(Input.GetAxis (verticalMoveAxis));
			//print (horizontalMoveAxis + " : " + Input.GetAxis (horizontalMoveAxis));
			//print ("movement vector: " + movementVector.ToString ());
			bool focus = Input.GetButton (focusButton);
			bool fire = Input.GetButton (fireButton);
			bool charge = Input.GetButton (chargeButton);

			player.IsFiring = fire;
			player.IsCharging = charge;
			player.IsFocused = focus;

			Player.Move (movementVector.x, movementVector.y);
		}
	}
}