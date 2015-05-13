// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;
using Vexe.Runtime.Types;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	[RequireComponent(typeof(Collider2D))]
	public abstract class DanmakuCollider : BetterBehaviour, IDanmakuCollider {

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
		public virtual void Awake() {
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
		public void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info) {
			if(validTags == null || validTags.IsMatch(danmaku.Tag)) {
				DanmakuCollision(danmaku, info);
			}
		}

		#endregion

		/// <summary>
		/// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
		/// </summary>
		/// <param name="danmaku">the danmaku that hit the collider.</param>
		/// <param name="info"> additional information about the collision</param>
		protected abstract void DanmakuCollision (Danmaku danmaku, RaycastHit2D info);
	}
}