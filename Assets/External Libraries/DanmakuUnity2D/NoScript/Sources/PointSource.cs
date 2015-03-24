using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	internal class PointSource : ProjectileSource {

		#region implemented abstract members of ProjectileSource

		protected override void UpdateSourcePoints () {
			if (sourcePoints == null)
				sourcePoints = new SourcePoint[1];
			SourcePoint point = sourcePoints [0];
			point.Location = transform.position;
			point.BaseRotation = transform.rotation.eulerAngles.z;
			sourcePoints [0] = point;
		}

		#endregion

	}

}
