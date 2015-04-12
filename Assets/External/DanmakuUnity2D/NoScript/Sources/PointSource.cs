using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D.NoScript {

	[AddComponentMenu("Danmaku 2D/Sources/Point Source")]
	internal class PointSource : DanmakuSource {

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints (Vector2 position, float rotation) {
			if (sourcePoints.Count <= 0 || sourcePoints.Count > 1) {
				sourcePoints.Clear();
				sourcePoints.Add(new SourcePoint(position, rotation));
			}
			if(sourcePoints[0] == null) {
				sourcePoints[0] = new SourcePoint(position, rotation);
			}
			sourcePoints [0].Position = position;
			sourcePoints [0].BaseRotation = rotation;
		}

		#endregion

	}

}
