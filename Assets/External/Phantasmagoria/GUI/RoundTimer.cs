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

namespace DanmakU.Phantasmagoria.GUI {

	[RequireComponent(typeof(GUIText))]
	public class RoundTimer : MonoBehaviour {

		[SerializeField]
		private PhantasmagoriaGameController gameController;

		[SerializeField]
		private Color flashColor;

		[SerializeField]
		private FrameCounter flashInterval;

		[SerializeField]
		private float flashThreshold;

		private Color normalColor;
		private bool flashState;
		private GUIText label;

		void Start() {
			label = GetComponent<GUIText>();
			normalColor = label.color;
			flashState = false;
		}

		void Update() {
			int timeSec = Mathf.FloorToInt (gameController.RemainingRoundTime);
			int seconds = timeSec % 60;
			int minutes = timeSec / 60;
			label.text = minutes.ToString ("D2") + ":" + seconds.ToString ("D2");;
			if (timeSec < flashThreshold) {
				if(flashInterval.Tick()) {
					label.color = (flashState) ? flashColor : normalColor;
					flashState = !flashState;
				}
			} else {
				label.color = normalColor;
				flashState = false;
				flashInterval.ForceReady();
			}
		}
	}
}