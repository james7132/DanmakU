using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// A script for defining boundaries for detecting collision with Projectiles
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class DanmakuBoundary : MonoBehaviour, IDanmakuCollider {

		/// <summary>
		/// A filter for a set of tags, delimited by "|" for selecting which bullets to affect
		/// Leaving this blank will affect all bullets
		/// </summary>
		[SerializeField]
		private string tagFilter;

		private HashSet<string> validTags;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		void Awake() {
			if (tagFilter == null || tagFilter == "")
				validTags = new HashSet<string> ();
			else
				validTags = new HashSet<string>(tagFilter.Split ('|'));
		}

		/// <summary>
		/// Called on collision with any Danmaku
		/// </summary>
		/// <param name="proj">Proj.</param>
		public void OnDanmakuCollision(Danmaku danmaku) {
			if(validTags.Count <= 0 || validTags.Contains(danmaku.Tag)) {
				ProcessDanmaku(danmaku);
			}
		}

		/// <summary>
		/// Processes a danmaku.
		/// By default, this deactivates all Projectiles that come in contact with the ProjectileBoundary
		/// Override this in subclasses for alternative behavior.
		/// </summary>
		/// <param name="proj">the projectile to process</param>
		protected virtual void ProcessDanmaku(Danmaku danmaku) {
			danmaku.Deactivate();
		}
	}
}