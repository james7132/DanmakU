// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

namespace DanmakU.Phantasmagoria {

	public class TransferCollider : DanmakuCollider {

		protected override void ProcessDanmaku (Danmaku proj, RaycastHit2D info) {
			PhantasmagoriaGameController.Transfer(proj);
		}

	}

}