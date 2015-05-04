// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using DanmakU.Phantasmagoria;
using UnityUtilLib;

namespace DanmakU.Phantasmagoria.GUI {

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
//			player = (PhantasmagoriaPlayableCharacter)field.Player;
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
			if (player.MaxChargeLevel > 0) {
				Vector3 capacityScale = chargeCapacity.localScale;
				capacityScale.x = player.CurrentChargeCapacity / (float)player.MaxChargeLevel;
				Vector3 levelScale = chargeCapacity.localScale;
				levelScale.x = player.CurrentChargeLevel / (float)player.MaxChargeLevel;
				chargeCapacity.localScale = capacityScale;
				chargeLevel.localScale = levelScale;
			}
		}
	}
}