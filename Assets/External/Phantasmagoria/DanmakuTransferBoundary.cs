using UnityEngine;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {
	public class DanmakuTransferBoundary : DanmakuBoundary {

		protected override void ProcessDanmaku (Danmaku proj) {
			PhantasmagoriaGameController.Transfer(proj);
		}
	}
}