using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	internal class StreamEmitter : ProjectileEmitter {
		
		#pragma warning disable 0649
		public FireBuilder fireData;
		public ProjectileControlBehavior controller;
		#pragma warning restore 0649

		#region implemented abstract members of ProjectileEmitter

		protected override void FireProjectiles () {
			if (controller != null)
				fireData.Controller = controller.UpdateProjectile;
			else 
				fireData.Controller = null;
			fireData.Modifier = Modifier;
			Source.Fire (fireData);
		}

		#endregion
		
	}
}