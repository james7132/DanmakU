// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;
using UnityUtilLib;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	/// <summary>
	/// A script for defining boundaries for detecting collision with Projectiles
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public abstract class DanmakuCollider : CachedObject, IDanmakuCollider {

		/// <summary>
		/// A filter for a set of tags, delimited by "|" for selecting which bullets to affect
		/// Leaving this blank will affect all bullets
		/// </summary>
		[SerializeField]
		private string tagFilter;

		private Regex validTags;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		public override void Awake() {
			base.Awake ();
			if (string.IsNullOrEmpty (tagFilter))
				validTags = null;
			else
				validTags = new Regex (tagFilter);
		}

		#region IDanmakuCollider implementation

		/// <summary>
		/// Called on collision with any Danmaku
		/// </summary>
		/// <param name="proj">Proj.</param>
		public void OnDanmakuCollision(Danmaku danmaku) {
			if(validTags == null || validTags.IsMatch(danmaku.Tag)) {
				ProcessDanmaku(danmaku);
			}
		}

		#endregion

		/// <summary>
		/// Processes a danmaku.
		/// By default, this deactivates all Projectiles that come in contact with the ProjectileBoundary
		/// Override this in subclasses for alternative behavior.
		/// </summary>
		/// <param name="proj">the projectile to process</param>
		protected abstract void ProcessDanmaku (Danmaku danmaku);
	}
}