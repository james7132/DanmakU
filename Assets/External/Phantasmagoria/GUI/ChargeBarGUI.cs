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
using Danmaku2D.Phantasmagoria;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria.GUI {

	public class ChargeBarGUI : MonoBehaviour {

		[SerializeField]
		private DanmakuField field;
		private PhantasmagoriaPlayableCharacter player;

		[SerializeField]
		private Transform chargeCapacity;

		[SerializeField]
		private Transform chargeLevel;

		[SerializeField]
		private GameObject indicator;

		void Start() {
			player = (PhantasmagoriaPlayableCharacter)field.Player;
			int maxIndicatorLevel = player.MaxChargeLevel - 1;
			float inc = 0.5f / (float)player.MaxChargeLevel;
			Vector3 ls = indicator.transform.localScale;
			for(int i = -maxIndicatorLevel; i <= maxIndicatorLevel; i++) {
				if(i != 0) {
					GameObject newIndicator = (GameObject)Instantiate(indicator);
					newIndicator.transform.parent = transform;
					newIndicator.transform.localPosition = Vector3.right * inc * i + Vector3.forward * indicator.transform.localPosition.z;
					newIndicator.transform.localScale = new Vector3(ls.x / 2f, ls.y, ls.z);
				}
			}
		}

		void Update () {
			Vector3 capacityScale = chargeCapacity.localScale;
			Vector3 levelScale = chargeCapacity.localScale;
			if (player.MaxChargeLevel > 0) {
				capacityScale.x = player.CurrentChargeCapacity / (float)player.MaxChargeLevel;
				levelScale.x = player.CurrentChargeLevel / (float)player.MaxChargeLevel;
				chargeCapacity.localScale = capacityScale;
				chargeLevel.localScale = levelScale;
			}
		}
	}
}